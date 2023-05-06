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
    public partial class frm_CodeSearch : Form
    {
        public frm_CodeSearch()
        {
            InitializeComponent();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                ClearAll();
                string code = this.textBox1.Text;
                if (code.Length > 0)
                {

                    int level = Program.km_CollectCodeBll.CheckLevel(code);
                    label3.Text = level == 1 ? "小码" : (level == 2 ? "件吗" : (level == 3 ? "箱码" : "未知"));

                    if (level <= 3)
                    {

                        if (level == 1)
                        {
                            List<km_CollectCode> dd = Program.km_CollectCodeBll.SearchBaglistBycode(code).OrderBy(x=>x.collectime).ToList();
                            this.listBox1.DataSource = dd;
                            this.listBox1.DisplayMember = "lcode1";

                            km_CollectCode km = dd.Where(x => x.lcode1.Equals(code)).Single();
                            label4.Text = km.upstatus == 0 ? "已采集" : (km.upstatus == 1 ? "已装箱" : (km.upstatus == 2 ? "已上传" : "未知状态:" + km.upstatus));
                            label9.Text = km.batch;
                            label6.Text = km.collectime;
                            label11.Text = (dd.IndexOf(km) + 1) + "";



                            SeachInfo(km.lcode3,"lcode1");
                        }
                        else if (level == 3)
                        {
                            SeachInfo(code, "lcode3");
                           

                        }
                    }
                }
            
            }
        }

        /// <summary>
        /// 根据箱码查询信息
        /// </summary>
        /// <param name="code"></param>
        private void SeachInfo(string code,string level )
        {
            string order = Program.km_CollectCodeBll.SearchOrderBycode(code);
            km_UploadInfo kmup = Program.km_CollectCodeBll.GetUploadinfo(order);

            label18.Text = kmup.UploadOrder;
            label27.Text = kmup.proddate;
            label25.Text = kmup.expiredate;
            label15.Text = kmup.cxid;
            label23.Text = kmup.cxmanager;
            label21.Text = kmup.upstatus == 1 ? "已上传" : "未上传";

            //查询此次采集的箱顺序
            List<km_CollectCode> alldata = Program.km_CollectCodeBll.GetCollectedData(order, 3).OrderBy(x=>x.collectime).ToList();
            Dictionary<string, int> ds = alldata.GroupBy(x => x.lcode3).ToDictionary(g => g.Key, g => g.Count());

            label13.Text = (ds.Keys.ToList().IndexOf(code) + 1).ToString();

            //查询产品信息
            Dictionary<string, string> dictv = new Dictionary<string, string>();
            dictv.Add("Prod_id", kmup.proidid);
            List<ProdInfo> orderls = Program.pibll.SelectBySearch(dictv);
            if (orderls.Count > 0)
            {
                label19.Text = orderls[0].ProductName;
            }


            if (level.Equals("lcode3"))
            {
                this.listBox1.DataSource = ds.Keys.ToList(); ;
            }
            else if (level.Equals("lcode1"))
            {
                //string lcode3 = alldata.Where(x => x.lcode1.Equals(code)).Single().lcode3;
                //this.listBox1.DataSource = alldata.GroupBy(x => x.lcode3).ToDictionary(g => g.Key, g => g.ToList())[lcode3];
            }
        }

        private void ClearAll()
        {
            label18.Text = "";
            label27.Text = "";
            label25.Text = "";
            label15.Text = "";
            label23.Text = "";
            label21.Text = "";
            label13.Text = "";
            label19.Text = "";
            label4.Text = "";
            label9.Text = "";
            label6.Text = "";
            label11.Text = "";
        }
    }
}
