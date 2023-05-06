using Entity.entity;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MiGuoPacking
{
    public partial class OutWareHouseLog : Form
    {
        public OutWareHouseLog()
        {
            InitializeComponent();
        }

        decimal outweight = 0;
        private void button9_Click(object sender, EventArgs e)
        {
            outweight = 0;
            string material_name = this.uiTextBox1.Text;
            string licensePlateNo = this.uiTextBox2.Text;
            string colstime = this.uiDatetimePicker1.Value.ToString("yyyy/MM/dd HH:mm:ss");
            string coletime = this.uiDatetimePicker2.Value.ToString("yyyy/MM/dd HH:mm:ss");
            string condition = "";
            int count = 0;
            if (material_name.Length > 0)
            {
                condition += "material_name like '%" + material_name + "%' and";
            }
            if (licensePlateNo.Length > 0)
            {
                condition += "  license_plate_no like '%" + licensePlateNo + "%' and";
            }
            if (colstime.Length > 0)
            {
                condition += " out_time >'" + colstime + "' and";

                if (coletime.Length > 0)
                {
                    condition += "  out_time <'" + coletime + "' and";
                }
            }
            condition = condition.Length > 0 ? condition.Substring(0, condition.Length - 3) : condition;

            List<EshengCheckout> ls = Program.ecb.SearchDataByCondition(condition);
            uiDataGridView1.Rows.Clear();
            foreach (var item in ls)
            {
                int index = uiDataGridView1.Rows.Add();
                uiDataGridView1.Rows[index].Cells[0].Value = item.DispatchOrderId;
                uiDataGridView1.Rows[index].Cells[1].Value = item.LicensePlateNo;
                uiDataGridView1.Rows[index].Cells[2].Value = item.MaterialName;
                uiDataGridView1.Rows[index].Cells[3].Value = item.TotalWeight;

                uiDataGridView1.Rows[index].Cells[4].Value = item.OutWeight;
                outweight += Convert.ToDecimal(item.OutWeight.Length > 0 ? item.OutWeight : "0");
                uiDataGridView1.Rows[index].Cells[5].Value = item.OutTime;
                uiDataGridView1.Rows[index].Cells[6].Value = item.OoutStatus == 1 ? "已出库" : "未出库";
                count++;
            }
            uiLabel5.Text = count + "";
            uiLabel2.Text = outweight + " 吨";
        }

        private void OutWareHouseLog_Load(object sender, EventArgs e)
        {
            this.uiDatetimePicker1.Value = DateTime.Now.AddDays(-1);
            this.uiDatetimePicker2.Value = DateTime.Now;
        }
    }
}
