using Entity.entity;
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
    public partial class Frm_Config : Form
    {
        public Frm_Config()
        {
            InitializeComponent();
        }

        private void Frm_Config_Load(object sender, EventArgs e)
        {
            BandData();
        }

        private void BandData()
        {
            List<BaseConfig> ls = Program.bcbll.SelectAll();
            dataGridView1.Rows.Clear();
            foreach (BaseConfig bc in ls)
            {
                int index = dataGridView1.Rows.Add();

                dataGridView1.Rows[index].Cells["Column1"].Value = bc.Bckey;
                dataGridView1.Rows[index].Cells["Column2"].Value = bc.Bcvalue;
                dataGridView1.Rows[index].Cells["Column3"].Value = bc.Id;
                dataGridView1.Rows[index].Cells["Column4"].Value = bc.Bcstatus;
            }
    
        }
        BaseConfig BcInfo = new BaseConfig();
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                this.textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells["Column1"].Value.ToString();
                this.textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells["Column2"].Value.ToString();
                BcInfo.Bckey= dataGridView1.Rows[e.RowIndex].Cells["Column1"].Value.ToString();
                BcInfo.Bcvalue = dataGridView1.Rows[e.RowIndex].Cells["Column2"].Value.ToString();
                BcInfo.Id =Convert.ToInt32( dataGridView1.Rows[e.RowIndex].Cells["Column3"].Value.ToString());
                BcInfo.Bcstatus = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["Column4"].Value.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string key = this.textBox1.Text;
            if (key.Length > 0)
            {
                string val = this.textBox2.Text;
                BcInfo.Bcvalue = val;
                Program.bcbll.Update(BcInfo);
                BandData();
                MessageBox.Show("修改成功");
            }
        }
    }
}
