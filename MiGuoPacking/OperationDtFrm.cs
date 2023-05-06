using Common.Mysql;
using Common.Sqlite;
using Common.Utils;
using MiGuoPacking.Tool;
using System;
using System.Data;
using System.Windows.Forms;

namespace MiGuoPacking
{
    public partial class OperationDtFrm : Form
    {
        public OperationDtFrm()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            richTextBox2.ClearInfo();
            var sql = richTextBox1.Text;
            WriteLog("操作数据库 ：" + sql);
            string sqlType = ConfigHelper.GetValue("SqlType", "0");

            if (sqlType == "Sqlite")
            {
                try
                {
                    var s = SqliteHelper.MyExecuteNonQuery(ConfigHelper.GetValue("Connstr", "0"), CommandType.Text, sql);
                    richTextBox2.AddText(s.ToString());
                }
                catch (Exception ex)
                {

                    richTextBox2.AddText(ex.Message);
                    WriteLog("操作数据库失败 ：" + ex.Message);
                }
            }
            else
            {
                try
                {
                    var s = MysqlHepler.MyExecuteNonQuery(ConfigHelper.GetValue("Connstr", "0"), CommandType.Text, sql);
                    richTextBox2.AddText(s.ToString());
                }
                catch (Exception ex)
                {

                    richTextBox2.AddText(ex.Message);
                    WriteLog("操作数据库失败 ：" + ex.Message);
                }
            }





        }

        private string LogPathLog = Application.StartupPath + "\\Log";
        public void WriteLog(string info)
        {

            LogHelper.WriteLog(LogPathLog, DateTime.Now.ToString() + info);
        }
    }
}
