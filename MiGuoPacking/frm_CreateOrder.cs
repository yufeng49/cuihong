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
    public partial class frm_CreateOrder : Form
    {
        string token = "";
        public frm_CreateOrder(string tt)
        {
            token = tt;
            InitializeComponent();
        }
        string workshopCode;
        string plantCode;
        string plantName;
        string workshopName;
        private void frm_CreateOrder_Load(object sender, EventArgs e)
        {
            workshopCode = ConfigHelper.GetValue("workshopCode", "gc1001");
            plantName = ConfigHelper.GetValue("plantName", "gc1001");
            workshopName = ConfigHelper.GetValue("workshopName", "gc1001");
            plantCode = ConfigHelper.GetValue("plantCode", "gc1001");
            BandProd();
            dateTimePicker1.Value = DateTime.Now;
            this.textBox1.Text = DateTime.Now.ToString("yyyyMMdd");


           List<BaseConfig> ls =  Program.bcbll.SelectAll();
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
        private void SaveConfig()
        {
            Program.bcbll.CheckAndAdd(new BaseConfig()
            {
                Bckey = "box",
                Bcvalue = this.textBox5.Text,
                Bcstatus = 1
            });

            Program.bcbll.CheckAndAdd(new BaseConfig()
            {
                Bckey = "bag",
                Bcvalue = this.textBox6.Text,
                Bcstatus = 1
            });

        }
        private void BandProd()
        {
            string msg = "";
            List<Product> ret = ResponseUtil.getProduct(ApiUrl.getProduct, token, out msg);
            if (msg == "成功")
            {
                comboBox1.DisplayMember = "productName";
                comboBox1.ValueMember = "id";
                comboBox1.DataSource = ret;
            }
            else
            {
                MessageBox.Show(msg);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string msg = "";
            if (comboBox1.SelectedIndex >= 0)
            {
                ///api/client/currency/getProductBasic
                long productid = (comboBox1.SelectedItem as Product).id;
                List<ProductBasic> ret = ResponseUtil.getProductBasic(ApiUrl.getProductBasic, token, productid, out msg);
                if (msg.Equals("成功"))
                {
                    comboBox2.DisplayMember = "productBasicName";
                    comboBox2.ValueMember = "id";
                    comboBox2.DataSource = ret;

                }
                else
                {
                    MessageBox.Show(msg);
                }
            }
        }
        public Dictionary<string, object> createinfo;
        private void button1_Click(object sender, EventArgs e)
        {
            string batch = this.textBox1.Text;
            int num = 0;
            string date = this.dateTimePicker1.Value.ToString("yyyy-MM-dd");
            int prop1 = 0;
            int prop2 = 0;
            if (int.TryParse(this.textBox2.Text, out num) == false)
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

            string productBasicCode = (this.comboBox2.SelectedItem as ProductBasic).productBasicCode;
            string productBasicName = (this.comboBox2.SelectedItem as ProductBasic).productBasicName;
            string productCode = (this.comboBox1.SelectedItem as Product).productCode;
            string productName = (this.comboBox1.SelectedItem as Product).productName;
     
            string ret = ResponseUtil.getBillNo(ApiUrl.getBillNo, token, batch, plantCode, plantName, productBasicCode, productBasicName, productCode, productName, num, workshopCode, workshopName);
            if (ret.StartsWith("#"))
            {
                MessageBox.Show("创建生产任务失败：" + ret);
            }
            else
            {
                SaveConfig();
               // MessageBox.Show("创建生产任务成功，单号：" + ret);
                createinfo = new Dictionary<string, object>();
                createinfo.Add("Product", (this.comboBox1.SelectedItem as Product));
                createinfo.Add("ProductBasic", (this.comboBox2.SelectedItem as ProductBasic));
                createinfo.Add("batch", batch);
                createinfo.Add("billno", ret);
                createinfo.Add("creatime", date);
                createinfo.Add("prop", prop);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
