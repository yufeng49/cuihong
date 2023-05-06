using Common;
using Common.Utils;
using Entity.entity;
using Server.constant;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiGuoPacking
{
    public partial class Frm_SearchTask : Form
    {
        string token;
        public Dictionary<string, object> createinfo;
        public Frm_SearchTask(string tk)
        {
            token = tk;
            InitializeComponent();
        }
        List<Createtask> ls;
        private void Frm_SearchTask_Load(object sender, EventArgs e)
        {
            Createtask ct = new Createtask()
            {
                plantCode = ConfigHelper.GetValue("plantCode", "gc1001"),
                workshopCode = ConfigHelper.GetValue("workshopCode", "gc1001")
            };
            string message;
            ls = ResponseUtil.GetTask(ApiUrl.getTask, ct, token, out message).OrderByDescending(x=>x.id).ToList();
            if (message.Equals("成功"))
            {
                dataGridView1.DataSource = ls;
                dataGridView1.AutoGenerateColumns = false;

            }
            else
            {
                MessageBox.Show(message);
            }

            bandconfig();


        }
        private void bandconfig() {
            List<BaseConfig> ls = Program.bcbll.SelectAll();
            foreach (BaseConfig bc in ls)
            {
                if (bc.Bckey.Equals("box"))
                {
                    this.textBox5.Text = bc.Bcvalue;
                    continue;
                }
                if (bc.Bckey.Equals("bag"))
                {
                    this.textBox6.Text = bc.Bcvalue;
                    continue;
                }
            }
        }

        private void SaveConfig() {
            Program.bcbll.CheckAndAdd(new BaseConfig() { 
             Bckey="box", Bcvalue =this.textBox5.Text, Bcstatus =1
            });

            Program.bcbll.CheckAndAdd(new BaseConfig()
            {
                Bckey = "bag",
                Bcvalue = this.textBox6.Text,
                Bcstatus = 1
            });

        }
        private void button2_Click(object sender, EventArgs e)
        {
            var rows = this.dataGridView1.SelectedRows[0];
            if (rows != null)
            {

                string batch = rows.Cells["batchCode"].Value.ToString();
                int num = 0;
                string date = this.dateTimePicker1.Value.ToString("yyyy-MM-dd");
                int prop1 = 0;
                int prop2 = 0;
                string plantCode = "";
                string plantName = "";
                string workshopCode = "";
                string workshopName = "";
                if (int.TryParse(rows.Cells["taskNumber"].Value.ToString(), out num) == false)
                {
                    MessageBox.Show("任务数量须为合法数字");
                    return;
                }
                if (int.TryParse(this.textBox5.Text, out prop1) == false || int.TryParse(this.textBox6.Text, out prop2) == false)
                {
                    MessageBox.Show("包装比例须为合法数字");
                    return;
                }
                if (batch.Length <= 0)
                {
                    MessageBox.Show("批次号不能为空");
                    return;
                }

                if (prop2 % prop1 != 0)
                {
                    MessageBox.Show("比例错误");
                    return;
                }
                string prop = "1:" + prop1 + ":" + prop2;

                string productBasicCode = rows.Cells["productBasicCode"].Value.ToString();
                string billNo = rows.Cells["billNo"].Value.ToString();
                string productBasicName = rows.Cells["productBasicName"].Value.ToString();
                string productCode = rows.Cells["productCode"].Value.ToString();
                string productName = rows.Cells["productName"].Value.ToString();
                workshopCode = rows.Cells["workshopCode"].Value.ToString();
                workshopName = rows.Cells["workshopName"].Value.ToString();
                long productId = Convert.ToInt64(rows.Cells["productId"].Value.ToString());
                long productBasicId = Convert.ToInt64(rows.Cells["productBasicId"].Value.ToString());


                SaveConfig();
                // MessageBox.Show("创建生产任务成功，单号：" + ret);
                createinfo = new Dictionary<string, object>();
                createinfo.Add("Product", new Product() { productCode = productCode, productName = productName, id = productId });
                createinfo.Add("ProductBasic", new ProductBasic() { productBasicName = productBasicName, productBasicCode = productBasicCode, id = productBasicId });
                createinfo.Add("batch", batch);
                createinfo.Add("billno", billNo);
                createinfo.Add("creatime", date);
                createinfo.Add("prop", prop);
                this.DialogResult = DialogResult.OK;
                this.Close();

            }
            else
            {
                MessageBox.Show("请选择任务");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string condtion = this.textBox1.Text;
            if (condtion.Length > 0)
            {
                dataGridView1.DataSource = ls.Where(x => x.batchCode.Contains(condtion)).ToList();
            }
            else
            {
                dataGridView1.DataSource = ls;
            }
        }
    }
}
