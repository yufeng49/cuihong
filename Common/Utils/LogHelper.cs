using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Common.Utils
{
    public class LogHelper
    {
        public static void WriteLog(string filePath, string strLog)
        {
            if (filePath == "")
                return;
            string sFilePath = filePath; //+"\\"+ DateTime.Now.ToString("yyyyMM");
            string sFileName = ReplaceFormat() + ".txt";
            sFileName = sFilePath + "\\" + sFileName; //文件的绝对路径
            if (!Directory.Exists(sFilePath))//验证路径是否存在
            {
                Directory.CreateDirectory(sFilePath);
                //不存在则创建
            }
            FileStream fs;
            StreamWriter sw;
            if (File.Exists(sFileName))
            //验证文件是否存在，有则追加，无则创建
            {
                // fs = new FileStream(sFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                fs = new FileStream(sFileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
            }
            else
            {
                fs = new FileStream(sFileName, FileMode.Create, FileAccess.Write);
            }
            sw = new StreamWriter(fs);
            sw.WriteLine(strLog);
            sw.Close();
            fs.Close();
        }

        private static string ReplaceFormat()
        {
            Regex r = new Regex("-");
            // string time = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            string time = DateTime.Now.ToString("yyyy-MM-dd");
            time = r.Replace(time, "年", 1);
            time = r.Replace(time, "月", 1);
            time = time.Replace(" ", "日");
            //Regex s = new Regex(":");
            //time = s.Replace(time, "时", 1);
            //time = s.Replace(time, "分", 1);
            return time;// + "秒";
        }

    }
}
