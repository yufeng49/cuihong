using Common;
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
    public partial class Frm_ProSet : Form
    {
        public Frm_ProSet()
        {
            InitializeComponent();
        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void BandData()
        {
            List<ProdInfo> ls = Program.pibll.SelectAll();
            //  this.dataGridView1.DataSource = ls;
            this.dataGridView1.Rows.Clear();

            foreach (var item in ls)
            {
                int index = this.dataGridView1.Rows.Add();
                dataGridView1.Rows[index].Cells["ProductName"].Value = item.ProductName;
                dataGridView1.Rows[index].Cells["SubType"].Value = item.SubType;
                dataGridView1.Rows[index].Cells["SubTypeno"].Value = item.SubTypeno;
                dataGridView1.Rows[index].Cells["SpecDesc"].Value = item.SpecDesc;
                dataGridView1.Rows[index].Cells["packageSpec"].Value = item.PackageSpec;
                dataGridView1.Rows[index].Cells["VerfyCode"].Value = item.VerfyCode;
                dataGridView1.Rows[index].Cells["PkgRatio"].Value = item.PkgRatio;
                dataGridView1.Rows[index].Cells["CollectCount"].Value = item.CollectCount;
                dataGridView1.Rows[index].Cells["ProdId"].Value = item.ProdId;

            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            if (index >= 0)
            {
                this.textBox3.Text = this.dataGridView1.Rows[index].Cells["ProductName"].Value.ToString();
                this.textBox5.Text = this.dataGridView1.Rows[index].Cells["PkgRatio"].Value.ToString();
                this.textBox4.Text = this.dataGridView1.Rows[index].Cells["VerfyCode"].Value.ToString();
                this.numericUpDown1.Value = Convert.ToInt32(this.dataGridView1.Rows[index].Cells["CollectCount"].Value.ToString());
                label10.Text = this.dataGridView1.Rows[index].Cells["ProdId"].Value.ToString();
            }

        }

        private void Frm_ProSet_Load(object sender, EventArgs e)
        {
            BandData();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string prodid = label10.Text;
            if (prodid.Length > 0)
            {
                ProdInfo pi = new ProdInfo()
                {
                    ProdId = prodid,
                    ProductName = this.textBox3.Text,
                    PkgRatio = this.textBox5.Text,
                    VerfyCode = this.textBox4.Text,
                    CollectCount = this.numericUpDown1.Value.ToString()

                };

               int ret =  Program.pibll.UpdateByProdid(pi);
                if (ret > 0)
                {
                    MessageBox.Show("保存设置成功");
                }
                else
                {
                    MessageBox.Show("保存设置失败");
                }
            }
        }
        /// <summary>
        /// 生产信息
        /// </summary>
     public   ProdInfo pi = new ProdInfo();
        /// <summary>
        /// 批号
        /// </summary>
        public string batch = "";
        /// <summary>
        /// 产线
        /// </summary>
        public string cx = "";
        /// <summary>
        /// 班组
        /// </summary>
        public string bz = "";
        /// <summary>
        /// 生产时间
        /// </summary>
        public DateTime creatime;
        /// <summary>
        /// 包装时间
        /// </summary>
        public DateTime packtime;
        private void button2_Click(object sender, EventArgs e)
        {
            if (label10.Text.Length > 0)
            {
                if (this.textBox2.Text.Length > 0)
                {
                    Dictionary<string, string> dict = new Dictionary<string, string>();
                    dict.Add("Id", label10.Text);
                    pi = Program.pibll.SelectBySearch(dict)[0];
                    batch = this.textBox2.Text;
                    cx = this.textBox1.Text;
                    bz = this.comboBox1.Text;
                    creatime = dateTimePicker1.Value;
                    packtime = dateTimePicker2.Value;
                    this.DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show("请设置生产批号");
                }
            }
            else
            {
                MessageBox.Show("请选择要采集的产品");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var datas = ResponseUtil.GetKangmeiProd(ApiUrl.km_GetProdList);
            if (datas.code.Equals("0"))
            {
                int ret = Program.pibll.CheckAndAdd(datas.result);
                BandData();
            }
            else
            {
              //  MessageBox.Show(datas.result);
            }
        }
    }
}
