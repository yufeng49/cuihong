using Common.Utils;
using System;
using System.Windows.Forms;

namespace MiGuoPacking
{
    public partial class Operation : Form
    {
        public Operation()
        {
            InitializeComponent();
        }
        public delegate void SubmitDelegate();
        public event SubmitDelegate SubmitEvent;
        public delegate void DeleteDelegate(string reason);
        public event DeleteDelegate DeleteEvent;
        private void button1_Click(object sender, EventArgs e)
        {
            var dr = MessageBox.Show("不足一件是否强制组成一件", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (dr != DialogResult.OK)
                return;
            if (textBox2.Text != ConfigHelper.GetValue("PassWord", "123456"))
            {
                MessageBox.Show("密码不正确");
                return;
            }
            SubmitEvent();
            Close();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != ConfigHelper.GetValue("PassWord", "123456"))
            {
                MessageBox.Show("密码不正确");
                return;
            }
            var dr = MessageBox.Show("删除后数据不会上传", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (dr != DialogResult.OK)
                return;
            if (textBox1.Text.Length < 3)
            {
                MessageBox.Show("请输入删除原因");
                return;
            }
            DeleteEvent(textBox1.Text);
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox3.Text != ConfigHelper.GetValue("PassWord", "123456") || textBox4.Text != textBox5.Text)
            {
                MessageBox.Show("密码不正确");
                return;
            }
            ConfigHelper.Modify("PassWord", textBox4.Text);
            MessageBox.Show("修改成功");
            groupBox1.Visible = false;
        }
    }
}
