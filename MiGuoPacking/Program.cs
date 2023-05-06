using BLL.Sub;
using Common.Utils;
using System;
using System.Text;
using System.Windows.Forms;

namespace MiGuoPacking
{
    static class Program
    {
        public static EliminateBll eliminateBll = new EliminateBll();//剔除
        public static BaseConfigBll bcbll = new BaseConfigBll();//基础配置                 
        public static ProdInfoBll pibll = new ProdInfoBll();//产品信息 
        public static PrintConfigBll printConfigBll = new PrintConfigBll();//打印机
        public static ch_CollectBll ch_Collectbll = new ch_CollectBll();//采集的码
        public static km_CollectCodeBll km_CollectCodeBll = new km_CollectCodeBll();
        public static bool isSavePassword = false;
        public static bool isAutomaticStart = false;
        public static bool isAutomaticRegister = false;

        public static string singleNumber = "";
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                bool createdNew;
                System.Threading.Mutex instance = new System.Threading.Mutex(true, "MiGuoPacking", out createdNew);
                if (createdNew)
                {
                    //设置应用程序处理异常方式：ThreadException处理
                    Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                    //处理UI线程异常
                    Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
                    //处理非UI线程异常
                    AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);

                        Application.Run(new Frm_Main(""));
                }
                else
                {

                    MessageBox.Show("程序已打开");
                    Application.Exit();
                }
            }
            catch (Exception ex)
            {
                string str = GetExceptionMsg(ex, string.Empty);
                MessageBox.Show(str, "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            string str = GetExceptionMsg(e.Exception, e.ToString());
            MessageBox.Show(str, "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //LogManager.WriteLog(str);
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            string str = GetExceptionMsg(e.ExceptionObject as Exception, e.ToString());
            MessageBox.Show(str, "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //LogManager.WriteLog(str);
        }

        /// <summary>
        /// 生成自定义异常消息
        /// </summary>
        /// <param name="ex">异常对象</param>
        /// <param name="backStr">备用异常消息：当ex为null时有效</param>
        /// <returns>异常字符串文本</returns>
        static string GetExceptionMsg(Exception ex, string backStr)
        {
            StringBuilder sb = new StringBuilder();
            string stackTrace = "";
            sb.AppendLine("****************************异常文本****************************");
            sb.AppendLine("【出现时间】：" + DateTime.Now.ToString());
            if (ex != null)
            {
                sb.AppendLine("【异常类型】：" + ex.GetType().Name);
                sb.AppendLine("【异常信息】：" + ex.Message);
                stackTrace = ex.StackTrace;
                // sb.AppendLine("【堆栈调用】：" + ex.StackTrace);
            }
            else
            {
                sb.AppendLine("【未处理异常】：" + backStr);
            }
            sb.AppendLine("***************************************************************");
            string LogPath = Application.StartupPath + "\\Err";
            // string LogPath = @"D:\宾之郎产品化\槟榔\MiGuoPacking.exe.config\Err";
            LogHelper.WriteLog(LogPath, sb.ToString() + "【堆栈调用】：" + stackTrace + " \r");
            return sb.ToString();
        }
    }
}
