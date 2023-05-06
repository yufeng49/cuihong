using Entity.entity;
using MiGuoPacking.Tool;
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
    public partial class Frm_CodeReplace : Form
    {
        private ProdInfo pinfo;
        /// <summary>
        /// 需要采集的新码数量，从产品信息里面获取
        /// </summary>
        /// <param name="count"></param>
        public Frm_CodeReplace(ProdInfo count)
        {
            pinfo = count;
            InitializeComponent();
        }
        List<string> oldls = new List<string>();
        List<string> newls= new List<string>();
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (radioButton2.Checked == false && radioButton1.Checked == false)
                {
                    MessageBox.Show("请先选择置换类型");
                }
                else
                {
                    string code = this.textBox1.Text;
                    string type = this.groupBox4.Text;
                    if (type == "请扫描旧码")
                    {
                        bool flag = Program.km_CollectCodeBll.Isexist(code);
                        if (oldls.Contains(code) )
                        {
                            richTextBox3.AddText("旧码重复");
                        }
                        else
                        {
                            if (flag)
                            {
                                if (radioButton1.Checked)
                                {
                                    List<km_CollectCode> ls = Program.km_CollectCodeBll.Searchlcode2bylocde1(code).Where(x => x.upstatus!=2).ToList();
                                    if (ls.Count > 0)
                                    {
                                        foreach (km_CollectCode kc in ls)
                                        {

                                            this.richTextBox1.AddText(kc.lcode1, false);
                                            oldls.Add(kc.lcode1);
                                            this.groupBox4.Text = "请扫描新码";
                                        }
                                    }
                                    else
                                    {
                                        richTextBox3.AddText("未查询到码或者已上传");

                                    }

                                }
                                else
                                {
                                    this.richTextBox1.AddText(code, false);
                                    this.groupBox4.Text = "请扫描新码";
                                    //查询关联的旧码 并填充
                                    oldls.Add(code);
                                }
                            }
                            else
                            {
                                richTextBox3.AddText("未查询到码");
                            }
                        }

                    }
                    if (type == "请扫描新码")
                    {
                        if (radioButton1.Checked)
                        {
                            if (newls.Count ==Convert.ToInt32( pinfo.CollectCount))
                            {
                                richTextBox3.AddText("新码数量已超出置换数量");
                            }
                            else
                            {
                                if (newls.Contains(code))
                                {
                                    richTextBox3.AddText(code + " 码重复");
                                }
                                else
                                {
                                    bool flag = Program.km_CollectCodeBll.Isexist(code);
                                    if (flag) {
                                        richTextBox3.AddText(code+" 已采集");
                                    }
                                    else
                                    {
                                        this.richTextBox2.AddText(code, false);
                                        newls.Add(code);
                                    }
                                }
                            }
                        }
                        else if (radioButton2.Checked)
                        {
                            bool flag = Program.km_CollectCodeBll.Isexist(code);
                            if (flag)
                            {
                                richTextBox3.AddText(code + " 已采集");
                            }
                            else
                            {
                                this.richTextBox2.AddText(code, false);
                                richTextBox3.AddText("一换一，置换成功");
                                ClearInfo();
                            }
                        }
                    }
                    this.textBox1.Clear();
                }
            }
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            if (oldls.Count == newls.Count&&oldls.Count>0)
            {
                string msg = "";
                bool flag = Program.km_CollectCodeBll.ReplacementCode(oldls, newls, out msg);

                richTextBox3.AddText(msg);
                if (flag)
                {
                    ClearInfo();
                }
            }
            else
            {
                MessageBox.Show("置换数量不一致");
            }
        }

        private void Frm_CodeReplace_Load(object sender, EventArgs e)
        {
            //    this.label2.Text = "需采集的新码数量：" + num;
            this.label1.Text = pinfo.ProductName +" "+pinfo.SubType+" "+pinfo.PackageSpec +" "+pinfo.SpecDesc;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                ClearInfo();
                radioButton1.Checked = false;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

            if (radioButton1.Checked)
            {
                ClearInfo();
                radioButton2.Checked = false;
            }
        }
        private void ClearInfo()
        {
            newls.Clear();
            oldls.Clear();
            richTextBox1.ClearInfo();
            richTextBox2.ClearInfo();
            groupBox4.Text = "请扫描旧码";
        }
    }
}
