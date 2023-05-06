using Common;
using Common.Utils;
using Entity.entity;
using MiGuoPacking.Tool;
using Server.constant;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Speech.Synthesis;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace MiGuoPacking
{
    public partial class Frm_Main : Form
    {
        string token = "";

        /// <summary>
        /// 当前轨道值 0代表左，1代表右
        /// </summary>
        int switchid = 0;

        /// <summary>
        /// 轨道状态 true 代表开，false代表关闭
        /// </summary>
        bool switch_status=false;
        /// <summary>
        /// 工厂编码	
        /// </summary>
        string plantCode = "";
        /// <summary>
        /// 车间编码
        /// </summary>
        string workshopCode = "";
        /// <summary>
        /// 需要验证的链接
        /// </summary>
        string CompareUrl;

        /// <summary>
        /// 已采集的袋码,验重用
        /// </summary>
        List<string> bagls = new List<string>();
        /// <summary>
        /// 已采集 未装箱的袋码，装箱后清空,对应switchid=0
        /// </summary>
        List<string> bagls_left = new List<string>();
        /// <summary>
        /// 已采集 未装箱的袋码，装箱后清空,对应switchid=1
        /// </summary>
        List<string> bagls_right = new List<string>();
        /// <summary>
        /// 已采集的箱码
        /// </summary>
        List<string> boxls = new List<string>();

        /// <summary>
        /// 已采集但未组垛的箱码，组垛后清空
        /// </summary>
        List<string> temp_boxls = new List<string>();

        /// <summary>
        /// 单位比例,达到装箱的袋数，比如10袋为一箱
        /// </summary>
        int unit_bag = 0;
        /// <summary>
        /// 单位比例，达到组垛的箱数
        /// </summary>
        int unit_box = 0;
        /// <summary>
        /// 允许容错数量
        /// </summary>
        int errcount = 0;
        /// <summary>
        /// 采集Noread数量
        /// </summary>
        int writerrorcount = 0;
        /// <summary>
        /// 相机状态 true为链接状态 false为断开
        /// </summary>
        bool OneCamera = false;
        /// <summary>
        /// 定时上传
        /// </summary>
        System.Timers.Timer timingSub = new System.Timers.Timer();
        /// <summary>
        /// 生产信息 Product(实体类)产品信息，creatime生产日期，billno单据号，ProductBasic(实体类)规格信息，prop包装比例，batch批次号
        /// </summary>
        Dictionary<string, object> creat_dict;

        /// <summary>
        /// 每次设置成功后拉去的箱码，用于强制组箱
        /// </summary>
        List<string> boxcode = new List<string>();
        /// <summary>
        /// 每次设置成功后拉取的垛码
        /// </summary>
        List<string> duocode = new List<string>();
        /// <summary>
        /// 语音播报
        /// </summary>
        SpeechSynthesizer speech = new SpeechSynthesizer();
        public Frm_Main(string tt)
        {
            token = tt;
            InitializeComponent();

            if (token.Length <= 0)
            {
                this.Hide();
                Frm_Login fl = new Frm_Login();
                DialogResult dr = fl.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    token = fl.token;
                }
            }
        }
        private void Frm_Main_Load(object sender, EventArgs e)
        {
            CompareUrl = ConfigHelper.GetValue("CompareUrl", "http://ch.mga1.cn/p/");
            plantCode = ConfigHelper.GetValue("plantCode", "cj1001");
            workshopCode = ConfigHelper.GetValue("workshopCode", "gc1001");

            errcount =Convert.ToInt32( ConfigHelper.GetValue("errcount", "1"));
            timingSub.AutoReset = true;
            timingSub.Enabled = true;
            timingSub.Interval = 10000;
            timingSub.Elapsed += Timer_Elapsed;
            timingSub.Start();

            string ismul = ConfigHelper.GetValue("Ismultiple", "false");
            if (ismul.Equals("false"))
            {
                button6.Enabled = false;
            }else
            { button6.Enabled = true; }
        }
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            timingSub.Stop();
            List<ch_Uploadstr> listStr = Program.ch_Collectbll.GetUploadls();
          

            int num = 0;
            if (listStr.Count == 0)
            {
                timingSub.Start();
                return;
            }
            for (int i = 0; i < listStr.Count; i++)
            {
                Thread.Sleep(500);
                string sr = listStr[i].uploadstr;
               // WriteLog("Upload"," 上传信息: " + sr);
                if (UploadData(listStr[i]))
                {
                    num++;
                }
            }
            richTextBox3.AddText("定时上传数据 总数据" + listStr.Count + "条 成功上传" + num + "条数据 未上传" + (listStr.Count - num) + "条");
            timingSub.Start();
        }
        public bool UploadData(ch_Uploadstr info)
        {
            bool flag = ResponseUtil.ch_UpLoad(ApiUrl.uploadCode, info.uploadstr, token);
            if (flag)
            {
                bool ret = Program.ch_Collectbll.FinshUpLoadinfo(info);
                if (!ret)
                {
                    WriteLog("Log", "上传数据成功，本地修改状态失败，" + info.fcode);
                }
            }
            return flag;
        }
        #region 相机
        SocketClient BottleClient;
        private void OneLineStart()
        {

            //richTextBox1.AddText("连接相机请等待...");
            string ip = ConfigHelper.GetValue("Server", "192.168.3.100");
            string prot = ConfigHelper.GetValue("Prot", "51236");
            BottleClient = new SocketClient("一");
            BottleClient.OnError += new SocketClient.SocketEventHandler(OnClientError);
            BottleClient.OnReceive += new SocketClient.SocketEventHandler(OnReceiveMessage);
            BottleClient.OnClosed += new SocketClient.SocketEventHandler(OnClientClosed);
            BottleClient.OnSucess += new SocketClient.SocketEventHandler(OnClientSucess);
            BottleClient.ConnectServer(ip, prot);
       
        }
      
        private bool OnReceiveMessage(object obj, string text)
        {
            /*
                        http://ch.mga1.cn/p/4xsi05gJ4M1YmVgI
                        http://ch.mga1.cn/p/4xsi0l6krI1LZjMZ
                        http://ch.mga1.cn/p/4xsi0a9LRs1YDjdq
                        http://ch.mga1.cn/p/4xsi0KQXjJ1Uk3d7
                        http://ch.mga1.cn/p/4xsi0p40YQ1fEGCQ
                        http://ch.mga1.cn/p/4xsi04nXfE1CLoNp
                        http://ch.mga1.cn/p/4xsi01S9ZB1bFJ63
            */
            string info = text.Replace("\u0002", "").Replace("\n", "").Replace("\r", "").Replace("#","").Replace("&","");
            WriteLog("CecData", info);

            if (errcount < writerrorcount && info.ToLower().Contains("noread"))
            {
                StartEliminate("X2");
                richTextBox3.AddText("超出容错");
            }
            else
            {
                if (info.ToLower().Contains("noread"))
                {
                    writerrorcount++;
                    TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                    info = info +"_"+ Convert.ToInt64(ts.TotalMilliseconds);
                }
                if (info.Contains(CompareUrl)  )
                {
                    info = Collect(info);
                }
                else
                {
                    if (info.ToLower().Contains("noread")) {
                        info = Collect(info);
                    }
                    else
                    {
                        richTextBox3.AddText(info + "域名错误");
                    }
                }
            }
            return false;
        }

        private string Collect(string info)
        {
            info = info.ToLower().Contains("noread") ? info : info.Substring(info.LastIndexOf('/') + 1);
            if (bagls.Contains(info))
            {
                richTextBox3.AddText(info + "  当次已采集");
            }
            else
            {

                if (switchid == 0)
                {
                    bagls.Add(info);
                    bagls_left.Add(info);
                    richTextBox2.AddText(info);
                    label3.AddLab(bagls.Count + "");
                    label9.AddLab(bagls_left.Count.ToString());
                    if (bagls_left.Count % unit_bag == 0)
                    {
                        if (switch_status)
                        {
                            switchid = 1;
                            StartEliminate("X3");
                        }
                        //已达到装箱比例 切换分轨
                    }
                }
                else
                {

                    bagls.Add(info);
                    bagls_right.Add(info);
                    richTextBox2.AddText(info);
                    label3.AddLab(bagls.Count + "");
                    label11.AddLab(bagls_right.Count.ToString());
                    if (bagls_right.Count % unit_bag == 0)
                    {
                        if (switch_status)
                        {
                            switchid = 0;
                            StartEliminate("X3");
                        }
                        //已达到装箱比例，切换分轨
                    }
                }
            }
            return info;

        }

        /// <summary>
        /// 装箱 1.袋装箱 2.箱组垛
        /// </summary>
        /// <param name="type"></param>
        /// <param name="code">type为1时  code为箱码，type为2时 code为垛码</param>
        /// <param name="iscompulsion">是否是强制组</param>
        /// <param name="switch_id">开关0为左1为右</param>
        private void Package(int type, string code,bool iscompulsion,int switch_id)
        {
            if (type == 1)
            {
                if (!boxls.Contains(code))
                {
                    ///检测待组箱数据在强制组箱时，是否大于一箱的数量
                    bool fff = switch_id == 1 ? bagls_right.Count >= unit_bag : bagls_left.Count >= unit_bag;
                    if (iscompulsion && fff)
                    {
                        //语音强制装箱失败
                        speech.Speak("强制装箱失败");
                        StartEliminate((switch_id ==0 ?"X8" :"X7"));
                        richTextBox3.AddText("待组箱数据大于一箱!!", true);
                    }
                    else
                    {

                        //待组箱数据
                        int lscount = iscompulsion ? (switch_id ==0? bagls_left.Count :bagls_right.Count): unit_bag;
                        List<string> packagebag = new List<string>();
                        for (int i = 0; i < lscount; i++)
                        {
                            packagebag.Add((switch_id ==0 ? bagls_left[i] : bagls_right[i]));
                        }
                        if (iscompulsion)
                        {
                            if (switch_id == 0)
                                bagls_left.Clear();
                          else
                                bagls_right.Clear();
                        }
                        else
                        {
                            if (switch_id == 0)
                                bagls_left.RemoveRange(0, unit_bag);
                            else
                                bagls_right.RemoveRange(0, unit_bag);

                        }
                    //  Task.Run(() =>
                   //  {
                       //  Thread.CurrentThread.Priority = ThreadPriority.Highest;
                            List<ch_Collect> ls = new List<ch_Collect>();
                            foreach (string mcode in packagebag)
                            {
                                ls.Add(new ch_Collect()
                                {
                                    code = mcode,
                                    codelevel = 1,
                                    codestatus = 0,
                                    collectime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                    parentcode = code

                                });
                            }
                            bool fag = Program.ch_Collectbll.Addch_Collectls(ls);
                            if (fag)
                            {

                                writerrorcount = 0;//清空容错
                                richTextBox4.AddText(code);
                                string txt = switch_id == 0 ? "左侧装箱成功" : "右侧装箱成功";
                                richTextBox3.AddText(txt, true);
                                speech.Speak(txt);
                                if (txt.Equals("左侧装箱成功"))
                                {
                                    StartEliminate("X6");
                                }
                                else
                                { StartEliminate("X5"); }
                                int boxcount = Convert.ToInt32(label5.Text) + 1;
                                label5.AddLab(boxcount.ToString());

                                if (switch_id == 0)
                                    label9.AddLab(bagls_left.Count + "");//待装箱的袋数
                                else
                                    label11.AddLab(bagls_right.Count + "");//待装箱的袋数

                                int duocount = Convert.ToInt32(label10.Text) + 1;
                                label10.AddLab(duocount.ToString());//待组垛的箱数
                                temp_boxls.Add(code);
                                boxls.Add(code);
                                Program.ch_Collectbll.AddUploadStr(creat_dict["batch"].ToString(), creat_dict["billno"].ToString(),
                                   creat_dict["creatime"].ToString(), DateTime.Now.ToString("yyyy-MM-dd"), plantCode, (creat_dict["ProductBasic"] as ProductBasic).productBasicCode, (creat_dict["ProductBasic"] as ProductBasic).id, (creat_dict["Product"] as Product).productCode, (creat_dict["Product"] as Product).id, workshopCode,
                                  ls, code);

                                //组垛比例相等时 或者为强制组垛时
                                if (temp_boxls.Count == unit_box || iscompulsion == true)
                                {//组垛 
                                    if (duocode.Count <= 0)
                                    {
                                        richTextBox3.AddText("垛码不足", true);
                                    }
                                    else
                                    {
                                        Package(2, duocode[0], false, switch_id);
                                    }
                                }


                                richTextBox2.AddList(bagls_left);
                            }
                            else
                            {//语音装箱失败 程序错误
                                StartEliminate(((switch_id == 0 ? "X8" : "X7")));
                             //   speech.Speak("装箱失败,程序错误");
                                richTextBox3.AddText("装箱失败_database error", true);
                            }
                    //    });
                    }
                   
                }else
                {
                    //语音箱码重复采集
                    StartEliminate(((switch_id == 0 ? "X8" : "X7")));
                  //  speech.Speak("箱码重复采集");
                    richTextBox3.AddText("箱码重复采集!!", true); }
            }
            else if (type == 2)
            {
                List<ch_Collect> ls = new List<ch_Collect>();
                foreach (string str in temp_boxls)
                {
                    ls.Add(new ch_Collect()
                    {
                        code = str,
                        codelevel = 2,
                        codestatus = 0,
                        collectime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        parentcode = code

                    });
                }
                bool fag = Program.ch_Collectbll.Addch_Collectls(ls);
                if (fag)
                {//组垛成功
                    temp_boxls.Clear();
                    int Groupcount = Convert.ToInt32(this.label7.Text) + 1;//已组垛数量
                    label7.AddLab(Groupcount.ToString());
                    label10.AddLab("0");
                    Program.ch_Collectbll.AddUploadStr(creat_dict["batch"].ToString(), creat_dict["billno"].ToString(),
                                  creat_dict["creatime"].ToString(), DateTime.Now.ToString("yyyy-MM-dd"), plantCode, (creat_dict["ProductBasic"] as ProductBasic).productBasicCode, (creat_dict["ProductBasic"] as ProductBasic).id, (creat_dict["Product"] as Product).productCode, (creat_dict["Product"] as Product).id, workshopCode,
                                 ls, code);
                 
                    duocode.Remove(code);
                    StartEliminate("X1");
                    richTextBox3.AddText("组垛成功!!", true);
                }
                else
                {
                    richTextBox3.AddText("组垛失败", true);
                }
            }
        }
        private bool OnClientError(object obj, string text)
        {
            SocketClient sc = (SocketClient)obj;
            richTextBox3.AddText("相机连接失败!!", true);
            OneCamera = false;
            //   uiLight1.State = Sunny.UI.UILightState.Off;


            WriteLog("log", "相机连接失败!!  " + text);
            button1.BeginInvoke((MethodInvoker)(() => { button1.Text = "重新连接"; }));
            return false;
        }

        private bool OnClientClosed(object obj, string text)
        {
            SocketClient sc = (SocketClient)obj;
            richTextBox3.AddText("相机连接断开!!", true);
            OneCamera = false;
            //  uiLight1.State = Sunny.UI.UILightState.Off;
            WriteLog("log", "相机连接断开!!  " + text);
            button1.BeginInvoke((MethodInvoker)(() => { button1.Text = "重新连接"; }));
            return false;
        }
   
        private bool OnClientSucess(object obj, string text)
        {
            SocketClient sc = (SocketClient)obj;
            richTextBox3.AddText("相机连接成功", true);
            OneCamera = true;
            //  uiLight1.State = Sunny.UI.UILightState.On;
            WriteLog("log", "相机连接成功!!  " + text);
            button1.BeginInvoke((MethodInvoker)(() => { button1.Text = "断开链接"; }));
            return false;
        }
        #endregion 
        #region 日志记录
        private void WriteLog(string file, string info)
        {
            LogHelper.WriteLog(Application.StartupPath + "\\" + file, DateTime.Now.ToString() + info);
        }

        #endregion 
        #region  串口
        /// <summary>
        /// 剔除
        /// </summary>
        /// <param name="name"></param>
        public void StartEliminate(string name)
        {
            if (name.ToLower() == "x3" || name.ToLower() == "x4")
            {
                if (switch_status)
                {
                    if (switchid == 0)
                    {
                        name = "X4";
                    }
                    else
                    {
                        name = "X3";
                    }


                }
            }
           // richTextBox3.AddText("发送信号："+name);
           Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("Command_name", name);
            var list = Program.eliminateBll.SelectBySearch(dict);
            foreach (var item in list)
            {
                var factory = Task.Factory.StartNew(() =>
                {
                    Thread.Sleep(Convert.ToInt32(item.TimeDelay));
                    byte[] Data = new byte[8];
                    string instructions = item.Instructions;
                    if (instructions.Length % 2 == 0)//偶数个数字
                    {
                        for (int j = 0; j < instructions.Length / 2; j++)
                        {
                            Data[j] = Convert.ToByte(instructions.Substring(j * 2, 2), 16);
                        }
                    }
                    serial_send(Data, 8);
                });
                richTextBox3.AddText("发送信号："+ item.CommandName+" 值："+ item.Instructions);
            }
        }
        //串口发送函数
        private void serial_send(byte[] Buffer, int leng)
        {
            if (serialPort1.IsOpen)
            {
                try
                {
                    serialPort1.Write(Buffer, 0, leng);    //写数据

                }
                catch (Exception e)
                {
                    //MessageBox.Show("串口数据发送出错，请检查.", "About");//错误处理
                    //richTextBox3.AddText(e.Message);
                }
            }
            else
            {
                //richTextBox3.AddText("注意：端口未打开，请检查");
            }
        }
        private void Open()
        {
            try
            {
                if (serialPort1.IsOpen)
                {
                    // richTextBox1.AddText("串口打开");
                }
                else
                {
                    serialPort1.PortName = ConfigHelper.GetValue("Com", "COM1");
                    serialPort1.BaudRate = Convert.ToInt32(ConfigHelper.GetValue("BaudRate", "9600"));
                    serialPort1.Parity = Parity.None;
                    serialPort1.DataBits = 8;
                    serialPort1.StopBits = StopBits.One;
                    serialPort1.Open();
                    // richTextBox1.AddText("串口已打开");
                    //  uiLight2.State = Sunny.UI.UILightState.On;
                }
            }
            catch
            {
                //  uiLight2.State = Sunny.UI.UILightState.Off;
                // richTextBox1.AddText("注意：\n端口不存在或者被占用。\r");
            }
        }

        #endregion


        private void button1_Click_1(object sender, EventArgs e)
        {
            if (this.button1.Text.Equals("重新连接") || this.button1.Text.Equals("开始采集"))
            {
                if (creat_dict == null)
                {
                    richTextBox3.AddText("请先设置生产信息", true);
                    return;
                }
                Open();
                ConnScanning();
                if (!OneCamera) OneLineStart();
            }
            else
            {
                DialogResult dr = MessageBox.Show("确认是否断开链接", "断开链接", MessageBoxButtons.YesNo);

                if (dr == DialogResult.Yes)
                {
                    this.button1.Text = "开始采集";
                    WriteLog("log", "手动断开相机");
                    BottleClient.Dispose();
                }
            }
        }
        /// <summary>
        /// 左侧扫码枪
        /// </summary>
        SerialPort sp1 = new SerialPort();
        /// <summary>
        /// 右侧扫码枪
        /// </summary>
        SerialPort sp2 = new SerialPort();
        /// <summary>
        /// 链接扫码枪
        /// </summary>
        private void ConnScanning() {

            //扫码枪类型 1.串口 2.USB
            string ScanType = ConfigHelper.GetValue("ScanType", "1");

            if (ScanType.Equals("1"))
            {
                try
                {
                    if (sp1.IsOpen)  //串口打开就关闭
                    {

                    }
                    else
                    {
                        sp1.PortName = ConfigHelper.GetValue("ScanningCom_left", "com2");
                        sp1.BaudRate = Convert.ToInt32(ConfigHelper.GetValue("ScanningBaudRate_left", "115200"));
                        sp1.Parity = Parity.None;
                        sp1.DataBits = 8;
                        sp1.StopBits = StopBits.One;
                        sp1.DataReceived += new SerialDataReceivedEventHandler(Com_DataReceived_left);
                        sp1.Open();
                    }
                }
                catch (Exception ex)
                {
                    richTextBox3.AddText(ex.Message);
                    return;
                }
                richTextBox3.AddText("扫描枪连接成功1");



                try
                {
                    if (sp2.IsOpen)  //串口打开就关闭
                    {

                    }
                    else
                    {
                        sp2.PortName = ConfigHelper.GetValue("ScanningCom_right", "com2");
                        sp2.BaudRate = Convert.ToInt32(ConfigHelper.GetValue("ScanningBaudRate_right", "115200"));
                        sp2.Parity = Parity.None;
                        sp2.DataBits = 8;
                        sp2.StopBits = StopBits.One;
                        sp2.DataReceived += new SerialDataReceivedEventHandler(Com_DataReceived_right);
                        sp2.Open();
                    }
                }
                catch (Exception ex)
                {
                    richTextBox3.AddText(ex.Message);
                    return;
                }
                richTextBox3.AddText("扫描枪连接成功2");
            }

        }


        private void Com_DataReceived_left(object sender, SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(50);
            try
            {
                byte[] ReDatas = new byte[sp1.BytesToRead];
                //从串口读取数据
                int bytecount = sp1.Read(ReDatas, 0, ReDatas.Length);
                var str = SerialPortHelper.HexStringToASCII(SerialPortHelper.AddData(ReDatas));
                if (str != "")
                {
                    str = str.Replace("\r","");
                    richTextBox3.AddText("左侧采集数据：" + str + " #end");
                    TxtCollect(str + "#L");
                }

            }
            catch (Exception ex)
            {
                 richTextBox3.AddText("注意：\n数据接受失败 " + ex.Message);
                // return;
            }
        }
        private void Com_DataReceived_right(object sender, SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(50);
             try
            {
                byte[] ReDatas = new byte[sp2.BytesToRead];
                //从串口读取数据
                int bytecount = sp2.Read(ReDatas, 0, ReDatas.Length);
                var str = SerialPortHelper.HexStringToASCII(SerialPortHelper.AddData(ReDatas));
                if (str != "")
                {
                    richTextBox3.AddText("右侧采集数据："+str +" #end");
                    TxtCollect(str + "#R");
                }

            }
            catch (Exception ex)
            {
               richTextBox3.AddText("注意：\n数据接受失败 " + ex.Message);
                // return;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Eliminate sp = new Eliminate(serialPort1);
            sp.StartPosition = FormStartPosition.CenterParent;
            sp.ShowDialog();
        }
       
        private void button3_Click(object sender, EventArgs e)
        {
            frm_CreateOrder fc = new frm_CreateOrder(token);
            fc.StartPosition = FormStartPosition.CenterParent;
            if (DialogResult.OK == fc.ShowDialog())
            {
                clearall();
                SaveInfo(fc.createinfo);
                GetCode();
                string text = "          生产单据：" + creat_dict["billno"].ToString() + "         产品名称：" + (creat_dict["Product"] as Product).productName + " \r\n        规格：" + (creat_dict["ProductBasic"] as ProductBasic).productBasicName + "         包装比例：" + creat_dict["prop"].ToString() +
                    "         生产日期：" + creat_dict["creatime"].ToString();
                richTextBox1.AddText(text, false, false);
            }
        }
        /// <summary>
        /// 获取垛码
        /// </summary>
        private void GetCode() {
            string msg = "";
            int count = 1000;
          Dictionary<string,List<string>> dict =   ResponseUtil.ch_GetCode(ApiUrl.getParentQrCode,token,out msg,count);
            if (dict.ContainsKey("duo"))
            {
                duocode = dict["duo"];
                boxcode = dict["box"];
            }
            else {
                richTextBox1.AddText("拉取垛码失败");
            }
        }

        /// <summary>
        /// 保存各类生产信息(软件启动时 读取上次或生产设置后
        /// </summary>
        private void SaveInfo(Dictionary<string, object>  ddd)
        {
            creat_dict = ddd;
            string prop = creat_dict["prop"].ToString();
            unit_bag = Convert.ToInt32(prop.Split(':')[2]) / Convert.ToInt32(prop.Split(':')[1]);
            unit_box = Convert.ToInt32(prop.Split(':')[1]);
        }
        private void button4_Click(object sender, EventArgs e)
        {
            Frm_SearchTask fc = new Frm_SearchTask(token);
            fc.StartPosition = FormStartPosition.CenterParent;
            if (DialogResult.OK == fc.ShowDialog())
            {
                clearall();
                GetCode();
                SaveInfo(fc.createinfo);
                string text = "          生产单据：" + creat_dict["billno"].ToString() + "         产品名称：" + (creat_dict["Product"] as Product).productName + " \r\n        规格：" + (creat_dict["ProductBasic"] as ProductBasic).productBasicName + "         包装比例：" + creat_dict["prop"].ToString() +
                    "         生产日期：" + creat_dict["creatime"].ToString();
                richTextBox1.AddText(text, false, false);
            }
        }


        private void clearall() {
            richTextBox1.ClearInfo();
            richTextBox2.ClearInfo();
            richTextBox4.ClearInfo();

            bagls.Clear();
            bagls_left.Clear();
            boxls.Clear();
            temp_boxls.Clear();
            label3.AddLab("0");
            label5.AddLab("0");
            label7.AddLab("0");
            label9.AddLab("0");
            label10.AddLab("0");
        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (!switch_status)
                {
                    string text = this.textBox1.Text;
                    TxtCollect(text);
                    this.textBox1.Clear();
                }
                else
                { //允许分轨的情况下不能使用扫码枪
                  //扫码枪无法判定来源
                    richTextBox3.AddText("允许分轨的情况下不能使用扫码枪");
                }
            }
        }

        private void TxtCollect(string text)
        {
            if (text.Length > 0)
            {
                string[] array = text.Split('#');
                int switch_id = 0;
                if (array.Count() > 1)
                {
                    text = array[0];
                    if (array[1].Equals("R"))
                    {
                        switch_id = 1;
                    }
                }
                if (text.Contains(CompareUrl))
                {
                    string info = text.Replace("\u0002", "").Replace("\n", "").Replace("\r", "").Replace("#", "").Replace("&", "");
                    info = info.Substring(info.LastIndexOf('/') + 1);

                    if (switch_id == 0)
                    {
                        if (bagls_left.Count >= unit_bag)
                        {
                            Package(1, info, false, switch_id);
                        }
                        else
                        {//语音装箱失败
                         //  speech.Speak("装箱失败");
                            StartEliminate(switchid == 0 ? "X8" : "X7");
                            richTextBox3.AddText("袋码采集数量不足以装箱!!", true);
                        }
                    }
                    else
                    {

                        if (bagls_right.Count >= unit_bag)
                        {
                            Package(1, info, false, switch_id);
                        }
                        else
                        { //语音装箱失败
                          //  speech.Speak("装箱失败");
                            StartEliminate(switchid == 0 ? "X8" : "X7");
                            richTextBox3.AddText("袋码采集数量不足以装箱!!", true);
                        }
                    }

                }
                else
                {   //语音装箱失败
                    //  speech.Speak("装箱失败");
                    StartEliminate(switch_id == 0 ? "X8" : "X7");
                    richTextBox3.AddText(text + "箱码采集数据格式错误", true);
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
        }

        private void button5_Click(object sender, EventArgs e)
        {

            if (bagls_left.Count > 0)
            {
                //表示有零箱 ，先强制组箱 再强制组垛
                if (boxcode.Count > 0)
                {
                    if (bagls_left.Count > unit_bag)
                    {
                        StartEliminate(switchid == 0 ? "X8" : "X7");
                        richTextBox3.AddText("未装箱数量大于装箱比例，强制装箱失败", true);
                        speech.Speak("强制装箱失败");
                        //语音强制装箱失败
                        return;
                    }
                    else
                    {
                        Package(1, boxcode[0], true, switchid);
                        boxcode.Remove(boxcode[0]);
                    }

                }
                else
                {   //语音强制装箱失败
                    //    speech.Speak("强制装箱失败");
                    StartEliminate(switchid == 0 ? "X8" : "X7");
                    richTextBox3.AddText("强制组箱失败，箱码不足", true);
                }
            }
            else//无零的袋码 直接箱码组垛
            {
                if (temp_boxls.Count > 0)
                {
                    if (duocode.Count > 0)
                    {
                        Package(2, duocode[0], true,switchid);
                    }
                    else
                    {
                        //语音强制组垛失败
                        //speech.Speak("强制组垛失败");
                        StartEliminate(switchid ==0 ? "X8": "X7");
                        richTextBox3.AddText("强制组垛失败，垛码不足", true);
                    }
                }
                else
                {   //语音强制组垛失败
                    //  speech.Speak("强制组垛失败");
                    StartEliminate(switchid == 0 ? "X8" : "X7");
                    richTextBox3.AddText("没有箱码可以组垛", true);
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            int removecount = bagls_left.Count+ bagls_right.Count;

            List<string> lst = new List<string>();
            lst.AddRange(bagls_right);lst.AddRange(bagls_left);
            foreach (string str in lst)
            {
                bagls.Remove(str);
            }
            bagls_left.Clear();
            bagls_right.Clear();
            richTextBox2.ClearInfo();
            label9.AddLab("0");
            label11.AddLab("0");
            int total = Convert.ToInt32(label3.Text);
            label3.AddLab((total - removecount).ToString());
        }
        private void button6_Click_1(object sender, EventArgs e)
        {
            if (switch_status)
            {
                switch_status = false;
                button6.Text = "分轨切换（关）";
                richTextBox3.AddText("关闭分轨");
            }
            else {
                switch_status = true;
                button6.Text = "分轨切换（开）";
                richTextBox3.AddText("打开分轨");
            }
        }
    }
}
