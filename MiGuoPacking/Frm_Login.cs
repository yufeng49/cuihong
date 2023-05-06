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
    public partial class Frm_Login : Form
    {
        public string token="";
        public Frm_Login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string acct = this.textBox1.Text;
            string pwd = this.textBox2.Text;
           string ret =  ResponseUtil.ch_Login(ApiUrl.ch_Login,acct,pwd);
            if (!ret.StartsWith("#"))
            {
                if (checkBox1.Checked)
                {
                    Program.bcbll.CheckAndAdd(new BaseConfig()
                    {
                        Bckey = "acct",
                        Bcstatus = 1,
                        Bcvalue = acct
                    });
                    Program.bcbll.CheckAndAdd(new BaseConfig()
                    {
                        Bckey = "pwd",
                        Bcstatus = 1,
                        Bcvalue = pwd
                    });
                }
                else
                {
                    List<string> keyls = new List<string>();
                    keyls.Add("acct"); keyls.Add("pwd");
                    Program.bcbll.DeleteKey(keyls);
                }




                token = ret;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show(ret);
            }
        }

        private void Frm_Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(this.DialogResult != DialogResult.OK)
           Application.Exit();

        }

        private void Frm_Login_Load(object sender, EventArgs e)
        {
            #region 绑定账号密码
            var bs = Program.bcbll.SelectAll();
            foreach (var item in bs)
            {
                if (item.Bckey == "acct")
                {
                    textBox1.Text = item.Bcvalue;
                    checkBox1.Checked = true;
                }
                if (item.Bckey == "pwd")
                {
                
                    textBox2.Text = item.Bcvalue;
                    checkBox1.Checked = true;
                }
            }
                #endregion
            }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                button1_Click(null,null);
            }
        }
    }
}
