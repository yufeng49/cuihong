
using Common.Utils;
using MiGuoPacking.Model;
using MiGuoPacking.Tool;
using Entity.entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using MiGuoPacking.service;
using Server.constant;

namespace MiGuoPacking
{
    public partial class PlumIncenseGardenHome : Form
    {
        public PlumIncenseGardenHome(string name, string token)
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            InitializeComponent();
            Token = token;
            UserNmae = name;
        }

        /// <summary>
        /// 按钮布局
        /// </summary>
        private void OverallArrangement()
        {
            var btns = this.Controls.OfType<Button>().Where(s => s.Name.Contains("button"));
            int num = this.Width / (btns.Count());
            int x = 0;
            foreach (Button col in btns)
            {
                col.Location = new System.Drawing.Point(num * x + 20, 11);
                x++;
            }
        }

        private string Token = "";
        private string UserNmae = "";

        private string Version = "1.1.1";
        System.Timers.Timer timingSub = new System.Timers.Timer();
        private string Title = "";
        private int timing = 0;
        private string lineNumber = "";
        private PrintConfig printConfig;
        private void BetelNut_Load(object sender, EventArgs e)
        {
            Title = "   欢迎您 " + UserNmae + "        米果码追溯系统   版本号: " + Version + "    技术电话 13668279355   ";

            if (UserNmae == "ht")
            {
                button9.Show();
                button10.Show();
                sub.Show();
            }
            var ver = Program.clientVersionBll.SelectAll();
            if (ver.Count > 0)
            {
                Version = ver[0].NowVersionCode;
            }

            timing = Convert.ToInt32(ConfigHelper.GetValue("Time", "1500"));
            //欢迎使用米果码采集关联软件   

            this.Text = Title;
            richTextBox1.ReadOnly = true;
            richTextBox2.ReadOnly = true;
            richTextBox3.ReadOnly = true;
            richTextBox4.ReadOnly = true;

            codeUrl = ConfigHelper.GetValue("Url", "http://mxy.mga1.cn") + "/p/";
            Program.singleNumber = ConfigHelper.GetValue("SingleNumber", "1");//打印数量

            var prints = Program.printConfigBll.SelectAll();
            if (prints.Count != 0)
            {
                printConfig = prints[0];
            }

            timingSub.AutoReset = true;
            timingSub.Enabled = true;
            timingSub.Interval = timing;
            timingSub.Elapsed += Timer_Elapsed;
            timingSub.Start();

        }

        private int submitCount = 0;
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            timingSub.Stop();
            var list = Program.uplaodInfoBll.SelectAll();
            int num = 0;
            if (list.Count == 0)
            {
                timingSub.Start();
                return;
            }
            foreach (var item in list)
            {
                Thread.Sleep(1000);
                int x = num;
                string sr = Encoding.Default.GetString(Convert.FromBase64String(item.SubmitInfo));
                // WriteData("上传信息:  " + sr);
                if (UploadData(sr))
                {
                    Program.uplaodInfoBll.Delete(item.Id);
                    submitCount++;
                    num++;
                }
               var pro = Program.productionRecordBll.SelectBySearch(new Dictionary<string, string> { { "ProductId", item.ProductId }, { "ProductSpecId", item.ProductSpecId }, { "TeamId", item.TeamId }, { "CreateTime", item.Time } });
                if (pro.Count != 0)
                {
                    pro[0].SuccessfulNumber++;
                    Program.productionRecordBll.Update(pro);
                }
            }

            label6.AddLab("当天" + lineNumber + "产线共上传 " + submitCount + " 件");
            Program.lineInfoBll.Delete();
            // Program.lineInfoBll.Add(new LineInfo { Line = ProductLine, NowTime = DateTime.Now.ToString(), Total = submitCount });

            richTextBox3.AddText("定时上传数据 总数据" + list.Count + "条 成功上传" + num + "条数据 未上传" + (list.Count - num) + "条");
            timingSub.Start();
        }

        /// <summary>
        /// 上传
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool UploadData(string info)
        {
            var b = RequestSpecific.Submit(ApiUrl.CollectUrl, info, Token);
            if (b.code == "200")
            {
                return true;
            }
            return false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Eliminate es = new Eliminate(serialPort1);
            es.StartPosition = FormStartPosition.CenterParent;
            es.ShowDialog();
        }
        private string LogPathData = Application.StartupPath + "\\Data";
        private static string LogPathLog = Application.StartupPath + "\\Log";
        public static void WriteLog(string info)
        {

            LogHelper.WriteLog(LogPathLog, DateTime.Now.ToString() + info);
        }

        private void WriteData(string info)
        {
            LogHelper.WriteLog(LogPathData, DateTime.Now.ToString() + info);
        }
        private List<string> bagList = new List<string>();
        private List<string> boxList = new List<string>();

        private Dictionary<string, string> bagDic = new Dictionary<string, string>();
        private Dictionary<string, string> bagDicTwo = new Dictionary<string, string>();
        private Dictionary<string, string> bagDicThree = new Dictionary<string, string>();

        private Dictionary<string, int> judgePage = new Dictionary<string, int>(); //重复判定
        private SocketClient oneClient;


        /// <summary>
        /// 开始采集
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            if (productionInfo == null) {
                richTextBox3.AddText("请先设置生产信息");
                return;
            }
            if (printConfig == null || printConfig.PrintType == "")
            {
                richTextBox3.AddText("请先设置打印机");
                return;
            }

            X = printConfig.FontX;
            Y = printConfig.FontY;
            X2 = printConfig.CodeX;
            Y2 = printConfig.CodeY;
            if (button2.Text == "开始采集")
            {
                WriteLog("开始采集");
                this.Text = Title + "                                         系统运行状态  运行中";
                button2.Text = "停止";
                Open();
                OneLineStart();
            }
            else if (button2.Text == "重新连接")
            {
                WriteLog("重新连接");
                Open();
                this.Text = Title + "                                         系统运行状态  运行中";
                button2.Text = "停止";
                OneLineStart();
            }
            else if (button2.Text == "停止")
            {
                WriteLog("停止");
                this.Text = Title + "                                         系统运行状态  停止";
                button2.Text = "开始";
                oneClient.Dispose();
            }
        }

        private void Open()
        {
            try
            {
                if (serialPort1.IsOpen)
                {
                    richTextBox3.AddText("串口已打开");
                }
                else
                {
                    serialPort1.PortName = ConfigHelper.GetValue("Com", "COM1");
                    serialPort1.BaudRate = Convert.ToInt32(ConfigHelper.GetValue("BaudRate", "9600"));
                    serialPort1.Parity = Parity.None;
                    serialPort1.DataBits = 8;
                    serialPort1.StopBits = StopBits.One;
                    serialPort1.Open();
                    richTextBox3.AddText("串口已打开");
                }
            }
            catch
            {
                richTextBox3.AddText("注意：\n端口不存在或者被占用。");
            }
        }
        private string codeUrl;
        private static readonly object ConsoleLock = new object();
        private void OneLineStart()
        {

            richTextBox3.AddText("连接一号相机请等待...");
            string ip = ConfigHelper.GetValue("Server", "192.168.3.100");
            string prot = ConfigHelper.GetValue("Prot", "51236");
            oneClient = new SocketClient("一");
            oneClient.OnError += new SocketClient.SocketEventHandler(OnClientError);
            oneClient.OnReceive += new SocketClient.SocketEventHandler(OnReceiveMessage);
            oneClient.OnClosed += new SocketClient.SocketEventHandler(OnClientClosed);
            oneClient.OnSucess += new SocketClient.SocketEventHandler(OnClientSucess);
            oneClient.ConnectServer(ip, prot);
        }

        int dicSwitchover = 1;// 字典切换
        private bool OnReceiveMessage(object obj, string text)
        {
            string getInfo = text.Replace("\u0002", "").Replace("\n", "").Replace("\r", "");
            WriteLog("相机一采集数据:" + getInfo);
            if (getInfo.StartsWith("&") && getInfo.EndsWith("#") && getInfo.Contains(codeUrl))
            {
                getInfo = getInfo.Replace("&", "").Replace("#", "").Replace(codeUrl, "");

                if (packgeCode.Count < 2 || bigBagCode.Count < 2)
                {
                    richTextBox3.AddText("二、三级码不足 请重新拉取订单");
                    return false;
                }
                if (getInfo == "NoRead" || getInfo == "")
                {
                    richTextBox3.AddText("采集数据失败");
                    StartEliminate(1);
                    return false;
                }
                //if (getInfo.Length != 14)
                //{
                //    richTextBox3.AddText("数据长度不正确");
                //    StartEliminate(1);
                //    return false;
                //}
                if (judgePage.ContainsKey(getInfo))
                {
                    richTextBox3.AddText("产品重复");
                    StartEliminate(1);
                    return false;
                }
                judgePage.Add(getInfo, 0);

                if (dicSwitchover == 1)
                {
                    bagDic.Add(getInfo, packgeCode[0]);
                    int relevanceCodeCount = bagDic.Count;
                    richTextBox1.AddText(getInfo);
                    oneLineCount++;
                    if (relevanceCodeCount % multiple == 0)
                    {
                        richTextBox2.AddText(packgeCode[0]);
                       var qr = packgeCode[0];
                        Task.Run(() => { Myprinter(qr); });

                        packgeCode.Remove(packgeCode[0]);
                        richTextBox1.ClearInfo();
                        label3.AddLab("总计：" + twoLineCount + "件  待组件：" + relevanceCodeCount / multiple);
                    }
                    if (relevanceCodeCount == productionInfo.Bag)
                    {
                        Packing(bigBagCode[0], bagDic);
                        bigBagCode.Remove(bigBagCode[0]);
                        richTextBox2.ClearInfo();
                        twoLineCount++;
                        StartEliminate(1);
                        label3.AddLab("总计：" + twoLineCount + "件  待组件：0");
                        dicSwitchover = 2;
                    }
                    label2.AddLab("总计：" + oneLineCount + "  待装袋：" + relevanceCodeCount % multiple);
                    return false;
                }
                if (dicSwitchover == 2)
                {
                    bagDicTwo.Add(getInfo, packgeCode[0]);
                    int relevanceCodeCount = bagDicTwo.Count;
                    richTextBox1.AddText(getInfo);
                    oneLineCount++;
                    if (relevanceCodeCount % multiple == 0)
                    {
                        richTextBox2.AddText(packgeCode[0]);
                       var  qr = packgeCode[0];
                        Task.Run(() => { Myprinter(qr); });
                        packgeCode.Remove(packgeCode[0]);
                        richTextBox1.ClearInfo();
                        label3.AddLab("总计：" + twoLineCount + "件  待组件：" + relevanceCodeCount / multiple);
                    }
                    if (relevanceCodeCount == productionInfo.Bag)
                    {
                        Packing(bigBagCode[0], bagDicTwo);
                        bigBagCode.Remove(bigBagCode[0]);
                        richTextBox2.ClearInfo();
                        twoLineCount++;
                        label3.AddLab("总计：" + twoLineCount + "件  待组件：0");
                        StartEliminate(2);
                        dicSwitchover = 3;
                    }
                    label2.AddLab("总计：" + oneLineCount + "  待装袋：" + relevanceCodeCount % multiple);
                    return false;
                }
                if (dicSwitchover == 3)
                {
                    bagDicThree.Add(getInfo, packgeCode[0]);
                    int relevanceCodeCount = bagDicThree.Count;
                    richTextBox1.AddText(getInfo);
                    oneLineCount++;
                    if (relevanceCodeCount % multiple == 0)
                    {
                        richTextBox2.AddText(packgeCode[0]);
                       var qr = packgeCode[0];
                        Task.Run(() => { Myprinter(qr); });
                       
                        packgeCode.Remove(packgeCode[0]);
                        richTextBox1.ClearInfo();
                        label3.AddLab("总计：" + twoLineCount + "件  待组件：" + relevanceCodeCount / multiple);
                    }
                    if (relevanceCodeCount == productionInfo.Bag)
                    {

                        Packing(bigBagCode[0], bagDicThree);
                        bigBagCode.Remove(bigBagCode[0]);
                        richTextBox2.ClearInfo();
                        twoLineCount++;
                        label3.AddLab("总计：" + twoLineCount + "件  待组件：0");
                        StartEliminate(2);
                        dicSwitchover = 1;
                    }
                    label2.AddLab("总计：" + oneLineCount + "  待装袋：" + relevanceCodeCount % multiple);
                    return false;
                }
                richTextBox3.AddText("字典切换超出3");
                return false;
            }
            else
            {
                richTextBox3.AddText("采集数据 失败");
                StartEliminate(1);
                return false;
            }
        }


        /// <summary>
        /// 装箱
        /// </summary>
        private void Packing(string code, Dictionary<string, string> dics)
        {
            Task.Run(() =>
            {
                List<ChildPro> cp = new List<ChildPro>();
                List<ThreeChild> tc = new List<ThreeChild>();

                // var dt2 = dt.GroupBy(pp => pp.BaseCode).Select(pp => pp.First()).ToList(); //分组 去重
                var dt2 = dics.GroupBy(pp => pp.Value).Select(pp => pp.First());
                foreach (var item2 in dt2)
                {
                    List<Code> c = new List<Code>();
                    foreach (var item in dics)
                    {
                        if (item.Value == item2.Value)
                        {
                            c.Add(new Code { qrCode = item.Key });
                        }
                    }
                    tc.Add(new ThreeChild { qrCode = item2.Value, children = c });
                }

                cp.Add(new ChildPro { children = tc, qrCode = code });

                ProudctCorrelation p = new ProudctCorrelation
                {
                    productId = productionInfo.ProductId,
                    productName = productionInfo.ProductName,
                    packingTime = productionInfo.PackingDate,
                    productionTime = productionInfo.CreateDate,
                    productSpecificationsId = productionInfo.ProductSpecId,
                    productSpecificationsName = productionInfo.ProductSpec,
                    productionBatch = productionInfo.Batch,
                    teamId = productionInfo.TeamId,
                    relationUploadReqs = cp
                };
                dics.Clear();

                string submitInfo = JsonHelper.SerializeObject(p);
                string bs64 = Convert.ToBase64String(Encoding.Default.GetBytes(submitInfo));
                Program.uplaodInfoBll.Add(new UplaodInfo { SubmitInfo = bs64, Time = DateTime.Now.ToString("yyyy-MM-dd"), ProductId = productionInfo.ProductId,ProductSpecId = productionInfo.ProductSpecId,TeamId = productionInfo.TeamId });
                if (twoLineCount % 10 == 0)
                {
                    richTextBox3.ClearInfo();
                }
            });

        }
        private int oneLineCount = 0;
        private int twoLineCount = 0;

        private bool OnClientError(object obj, string text)
        {
            SocketClient sc = (SocketClient)obj;
            richTextBox3.AddText(sc.SocketName + "号相机连接失败!!");
            WriteLog(sc.SocketName + "号相机连接失败!!  " + text);
            button2.BeginInvoke((MethodInvoker)(() => { button2.Text = "重新连接"; }));
            return false;
        }

        private bool OnClientClosed(object obj, string text)
        {
            SocketClient sc = (SocketClient)obj;
            richTextBox3.AddText(sc.SocketName + "号相机连接断开!!");
            WriteLog(sc.SocketName + "号相机连接断开!!  " + text);
            button2.BeginInvoke((MethodInvoker)(() => { button2.Text = "重新连接"; }));
            return false;
        }

        private bool OnClientSucess(object obj, string text)
        {
            SocketClient sc = (SocketClient)obj;
            richTextBox3.AddText(sc.SocketName + "号相机连接成功");
            WriteLog(sc.SocketName + "号相机连接成功!!  " + text);
            // button2.BeginInvoke((MethodInvoker)(() => { button2.Text = "开始采集"; }));// button2.Enabled = false;
            return false;
        }
        /// <summary>
        /// 剔除
        /// </summary>
        /// <param name="name"></param>
        public void StartEliminate(int id)
        {
            var list = Program.eliminateBll.GetDataById(id);
            foreach (var item in list)
            {
                Thread thread = new Thread(() =>
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
                thread.Start();
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
                    richTextBox3.AddText(e.Message);
                }
            }
            else
            {
                richTextBox3.AddText("注意：端口未打开，请检查");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (bagDic.Count > 0 || bagDicTwo.Count > 0 || bagDicThree.Count > 0)
            {
                MessageBox.Show("有未组件数据，请组件完成或者点击特殊操作");
                return;
            }

            if (bagDic.Count == 0 && bagDicTwo.Count == 0 && bagDicThree.Count == 0)
            {
                PlumIncenseGardenProSet bp = new PlumIncenseGardenProSet(Token, UserNmae);
                bp.OnGetInfo = GetProductInfo;
                bp.StartPosition = FormStartPosition.CenterParent;
                bp.ShowDialog();
                return;
            }
        }

        private int multiple = 0;
        private List<string> packgeCode = new List<string>();
        private List<string> bigBagCode = new List<string>();
        private ProductionInfo productionInfo;
        public void GetProductInfo(ProductionInfo proInfo)
        {
            bagDic.Clear();
            bagDicTwo.Clear();
            bagDicThree.Clear();

            judgePage.Clear();
            oneLineCount = 0;
            twoLineCount = 0;
            richTextBox1.ClearInfo();
            richTextBox2.ClearInfo();
            richTextBox3.ClearInfo();
            richTextBox4.ClearInfo();
            label2.AddLab("总计：0");
            label3.AddLab("总计：0");
         var list =  Program.productionRecordBll.SelectBySearch(new Dictionary<string, string> { { "ProductId", proInfo.ProductId },{ "ProductSpecId" , proInfo.ProductSpecId },{ "TeamId",proInfo.TeamId },{ "CreateTime", DateTime.Now.ToString("yyyy-MM-dd") } });
            if (list.Count == 0) {
                Program.productionRecordBll.Add(new ProductionRecord {
                    ProductId = proInfo.ProductId,
                    ProductName = proInfo.ProductName,
                    ProductSpec = proInfo.ProductSpec,
                    ProductSpecId = proInfo.ProductSpecId,
                    Batch = proInfo.Batch,
                    Team = proInfo.Team,
                    TeamId = proInfo.TeamId,
                    CreateTime = DateTime.Now.ToString("yyyy-MM-dd"),
                    SuccessfulNumber = 0
                });
            }

            productionInfo = proInfo;
            packgeCode = productionInfo.ParentCodes["1"];
            bigBagCode = productionInfo.ParentCodes["2"];
            multiple = productionInfo.Bag / productionInfo.Box;

            WriteLog("    产品名称：" + proInfo.ProductName + "   规格：" + proInfo.ProductSpec + "   批次：" + proInfo.Batch + "    班组：" + proInfo.Team + "    包装比例：" + proInfo.Proportion + "    生产日期：" + proInfo.CreateDate + "    包装日期：" + proInfo.PackingDate);
            richTextBox4.AppendText("    产品名称：" + proInfo.ProductName + "   规格：" + proInfo.ProductSpec + "   批次：" + proInfo.Batch + "    班组：" + proInfo.Team + "\r    包装比例：" + proInfo.Proportion + "    生产日期：" + proInfo.CreateDate + "    包装日期：" + proInfo.PackingDate);
        }


        /// <summary>
        /// 修改图片大小
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public Bitmap GetThumbnail(Bitmap bmp, int width, int height)
        {
            if (width == 0)
            {
                width = height * bmp.Width / bmp.Height;
            }
            if (height == 0)
            {
                height = width * bmp.Height / bmp.Width;
            }
            Image imgSource = bmp;
            Bitmap outBmp = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(outBmp);
            g.Clear(Color.Transparent);
            // 设置画布的描绘质量   
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(imgSource, new Rectangle(0, 0, width, height + 1), 0, 0, imgSource.Width, imgSource.Height, GraphicsUnit.Pixel);

            g.Dispose();
            imgSource.Dispose();
            bmp.Dispose();
            return outBmp;
        }

        int X = 0;
        int Y = 0;
        int X2 = 0;
        int Y2 = 0;
        private string qrCodePrint = "";
        PrintDocument pd;
        /// <summary>
        /// 打印
        /// </summary>
        private void Myprinter(string qr)
        {
            qrCodePrint = qr;
            if (printConfig == null || printConfig.PrintType == "")
            {
                richTextBox3.AddText("请先设置打印机");
                return;
            }
            if (printConfig.PrintType == "USB")
            {
                try
                {
                    pd = new PrintDocument();
                    pd.PrintPage += new PrintPageEventHandler(printDocument_PrintA4Page);

                    pd.DefaultPageSettings.PrinterSettings.PrinterName = printConfig.PrintName; ;//"ZDesigner GX430t";       //打印机名称
                                                                                                 //  pd.DefaultPageSettings.PaperSize = new PaperSize("newPage70X40"
                                                                                                 //  , (int)(m_pageWidth / 25.4 * 100)
                                                                                                 //  , (int)(m_pageHeight / 25.4 * 100));                                                            //pd.DefaultPageSettings.Landscape = true;  //设置横向打印，不设置默认是纵向的
                    pd.PrintController = new System.Drawing.Printing.StandardPrintController();
                    // string str2 = PrinterHelper.GetPrinterStatus(PrinterName);
                    pd.Print();
                }
                catch (Exception ex)
                {
                    richTextBox3.AddText(ex.Message);
                }
                return;
            }
            if (printConfig.PrintType == "COM")
            {
                PrintLab.PTK_CloseSerialPort();
                int err = 0;
                err = PrintLab.PTK_OpenSerialPort(uint.Parse(printConfig.Com.Replace("COM", "")), uint.Parse(printConfig.BaudRate));
                if (err != 0)
                {
                    showErrorInfo(err);
                    return;
                }
                ComPrint();
            }
        }

        private void printDocument_PrintA4Page(object sender, PrintPageEventArgs e)
        {
            Font fntTxt = new Font("楷体", 14, System.Drawing.FontStyle.Bold);//正文文字         
            Font fntTxt3 = new Font("楷体", 9, System.Drawing.FontStyle.Bold);
            System.Drawing.Brush brush = new SolidBrush(System.Drawing.Color.Black);//画刷      
            System.Drawing.Pen pen = new System.Drawing.Pen(System.Drawing.Color.Black);           //线条颜色         

            try
            {//
              e.Graphics.DrawString(productionInfo.ProductName, fntTxt, brush, new System.Drawing.Point(X + 5, Y + 10));
                e.Graphics.DrawString(productionInfo.ProductSpec, fntTxt, brush, new System.Drawing.Point(X + 5, Y + 30));
                e.Graphics.DrawString(UserNmae , fntTxt, brush, new System.Drawing.Point(X + 5, Y + 50));
                e.Graphics.DrawString(productionInfo.CreateDate, fntTxt3, brush, new System.Drawing.Point(X + 5, Y + 70));
                e.Graphics.DrawString(qrCodePrint, fntTxt3, brush, new System.Drawing.Point(X + 5, Y + 90));

                string code = codeUrl + qrCodePrint;
                Bitmap bitmap = QrCode.CreateQRCode(code, 74, 74);
                e.Graphics.DrawImage(bitmap, new System.Drawing.Point(X2 + 125, Y2 + 55));
                WriteLog("成功打印二维码: https://z.bzlsp.cn/p/" + qrCodePrint);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }


        PosTek_Text_T text;
        Bar2D_QR qr;
        private void ComPrint()
        {
            text.str = qrCodePrint + "\r\n" +productionInfo.CreateDate + "\r\n" + UserNmae + "\r\n" + productionInfo.ProductSpec + "\r\n" + productionInfo.ProductName;
            int x2 = 660 - Convert.ToInt32(X);
            int y2 = 300 - Convert.ToInt32(Y);
            text.x = uint.Parse(x2.ToString());
            text.y = uint.Parse(y2.ToString());
            text.rotate = uint.Parse("180") / 90;//旋转角度
            text.zoom = (uint)1 + 1; //字体大小
            text.thickness = uint.Parse("100");//字体粗细
            text.fontType = FontType_E.microsoft; //微软字库
            text.TrueTypeFont = "微软雅黑"; //字体
            int errCode = 0;
            errCode = PrintLab.PTK_ClearBuffer();
            if (errCode != 0)
            {
                showErrorInfo(errCode);
                return;
            }

            uint line = 0;
            string[] destStr = Regex.Split(text.str, "\r\n", RegexOptions.IgnoreCase);
            foreach (string outstr in destStr)
            {

                uint unitLW = 16;
                unitLW = unitLW * text.zoom;
                errCode = PrintLab.PTK_DrawText_TrueType(text.x, text.y + (text.lineGap + unitLW) * line++, unitLW, 0,
                                text.TrueTypeFont, text.rotate + 1, text.thickness, false, false, false, outstr);

                if (errCode != 0)
                {
                    showErrorInfo(errCode);
                    return;
                }

            }

            var x = 570 - Convert.ToInt32(X2);
            var y = 210 - Convert.ToInt32(Y2);
            qr.x = uint.Parse(x.ToString());
            qr.y = uint.Parse(y.ToString());
            qr.rotate = (uint)0;
            qr.zoom = uint.Parse(printConfig.CodeSize.ToString());
            qr.version = (uint)0 + 1;
            qr.errorCorrectionLevel = (uint)3;
            qr.maskGraphics = (uint)8;
            qr.str = codeUrl + qrCodePrint;
            byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(qr.str);
            errCode = PrintLab.PTK_DrawBar2D_QR(qr.x, qr.y, 0, qr.version, qr.rotate, qr.zoom, 0, qr.errorCorrectionLevel, qr.maskGraphics, byteArray);

            errCode = PrintLab.PTK_PrintLabel(1, 1);
            if (errCode != 0) showErrorInfo(errCode);
        }

        private void showErrorInfo(int errCode)
        {
            byte[] errInfo_Byte = new byte[1024];
            PrintLab.PTK_GetErrorInfo(errCode, errInfo_Byte, 1024);
            //string errInfo = System.Text.Encoding.Unicode.GetString(errInfo_Byte);
            string errInfo = System.Text.Encoding.GetEncoding("gbk").GetString(errInfo_Byte);
            richTextBox3.AddText(errInfo);
        }

        /// <summary>
        /// 特殊操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            var dicPk = bagDic.Count == 0 ? (bagDicTwo.Count == 0 ? bagDicThree : bagDicTwo) : bagDic;
            if (dicPk.Count == 0)
            {
                richTextBox3.AddText("没有可操作的数据");
                return;
            }
            Operation op = new Operation();
            op.StartPosition = FormStartPosition.CenterParent;
            op.SubmitEvent += new Operation.SubmitDelegate(SumitPacking);
            op.DeleteEvent += new Operation.DeleteDelegate(DeleteCode);
            op.ShowDialog();
        }

        private void DeleteCode(string reason)
        {
            var dicPk = bagDic.Count == 0 ? (bagDicTwo.Count == 0 ? bagDicThree : bagDicTwo) : bagDic;
            WriteLog("删除数据" + dicPk.Count + "条 原因:" + reason);
           
            richTextBox4.ClearInfo();
            richTextBox2.ClearInfo();
            richTextBox3.ClearInfo();
            richTextBox1.ClearInfo();
            packgeCode.Clear();
            bigBagCode.Clear();
            dicPk.Clear();
            judgePage.Clear();
            label2.AddLab("总计：0");
            label3.AddLab("总计：0");
            richTextBox3.AddText("删除成功");
        }

        private void SumitPacking()
        {
            var dicPk = bagDic.Count == 0 ? (bagDicTwo.Count == 0 ? bagDicThree : bagDicTwo) : bagDic;

            if(dicPk.Count % multiple != 0) {
                var qr = packgeCode[0];
                Task.Run(() => { Myprinter(qr); });
                packgeCode.Remove(packgeCode[0]);
            }
            WriteLog("强制组垛" + dicPk.Count + "条");
            ConstraintPacking(dicPk);
            

        }

        /// <summary>
        /// 强制组包
        /// </summary>
        private void ConstraintPacking(Dictionary<string, string> dicPk)
        {
            string threeCode = "";
            try
            {
               // qrCodePrint = bigBagCode[0];
                threeCode = bigBagCode[0];
                bigBagCode.Remove(bigBagCode[0]);

            }
            catch (Exception)
            {
                richTextBox3.AddText("三级码比例不足");
                return;
            }

                Packing(threeCode, dicPk);

            
            packgeCode.Clear();
            bigBagCode.Clear();
            richTextBox4.ClearInfo();
            richTextBox3.ClearInfo();
            richTextBox1.ClearInfo();
            richTextBox2.ClearInfo();

            judgePage.Clear();
            label2.AddLab("总计：0");
            label3.AddLab("总计：0");
            richTextBox3.AddText("强制组包成功");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = true;//该值确定是否可以选择多个文件
            dialog.Title = "请选择文件夹";
            dialog.Filter = "所有文件(*.*)|*.*";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string file = dialog.FileName;
                var fileList = File.ReadAllLines(file).ToList();
                string str = fileList[0];
                var p = JsonHelper.DeserializeObject<ProudctCorrelation>(str);
                string submitInfo = JsonHelper.SerializeObject(p);
                string bs64 = Convert.ToBase64String(Encoding.Default.GetBytes(submitInfo));
                Program.uplaodInfoBll.Add(new UplaodInfo { SubmitInfo = bs64, Time = DateTime.Now.ToString() });
            }
        }


        private void BetelNutSingleCamera_FormClosed(object sender, FormClosedEventArgs e)
        {
            WriteLog("窗体关闭 保存数据");

            Application.Exit();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            var ver = Program.clientVersionBll.SelectAll();
            if (ver.Count == 0)
            {
                if (SystemUpdate("", ""))
                {
                    var verNew = Program.clientVersionBll.SelectAll();
                    //verNew[0].NowVersionCode = verNew[0].UpdateVersionCode;
                    //verNew[0].NowVersionNo = verNew[0].UpdateVersionNo;
                    // Program.clientVersionBll.Update(verNew[0]);
                    Update update = new Update(verNew[0].VersionUrl, verNew[0].UpdateVersionCode, verNew[0].UpdateVersionNo, verNew[0].UpdateContent);
                    update.StartPosition = FormStartPosition.CenterParent;
                    update.ShowDialog();
                }
            }
            else
            {
                if (SystemUpdate(ver[0].NowVersionCode, ver[0].NowVersionNo))
                {
                    var verNew = Program.clientVersionBll.SelectAll();
                    //verNew[0].NowVersionCode = verNew[0].UpdateVersionCode;
                    //verNew[0].NowVersionNo = verNew[0].UpdateVersionNo;
                    //Program.clientVersionBll.Update(verNew[0]);
                    Update update = new Update(verNew[0].VersionUrl, verNew[0].UpdateVersionCode, verNew[0].UpdateVersionNo, verNew[0].UpdateContent);
                    update.StartPosition = FormStartPosition.CenterParent;
                    update.ShowDialog();
                }
            }
        }

        /// <summary>
        /// 软件更新
        /// </summary>
        private bool SystemUpdate(string nowVersion, string nowNo)
        {
            try
            {

                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("appCode", "BetelNutNew");
                dic.Add("clientType", "PC");
                if (nowVersion == "")
                {
                    dic.Add("nowVersionCode", "1.0.1");
                    dic.Add("nowVersionNo", "101");
                }
                else
                {
                    dic.Add("nowVersionCode", nowVersion);
                    dic.Add("nowVersionNo", nowNo);
                }
                //var rm = ResponseUtil.SoftwareUpdate(ApiUrl.GetVersion, dic, Token);
                //if (rm.code == "200")
                //{
                //    var ver = rm.data;
                //    if (ver == null)
                //    {
                //        richTextBox3.AddText("没有可更新版本");
                //        return false;
                //    }

                //    if (nowVersion == ver.updateVersionCode)
                //    {
                //        richTextBox3.AddText("已是最新版本");
                //        return false; //不需要更新
                //    }

                //    Program.clientVersionBll.Delete();
                //    Program.clientVersionBll.Add(new ClientVersion { Status = ver.status, AppCode = ver.appCode, ClientType = ver.clientType, CreateBy = ver.createBy, CreateTime = ver.createTime, NowVersionCode = ver.nowVersionCode, NowVersionNo = ver.nowVersionNo, Remark = ver.remark, UpdateBy = ver.updateBy, UpdateContent = ver.updateContent, UpdateTime = ver.updateTime, UpdateVersionCode = ver.updateVersionCode, UpdateVersionNo = ver.updateVersionNo, VersionUrl = ver.versionUrl });
                //    return true;
                //}
                //else
                //{
                //    richTextBox3.AddText("获取更新失败!!" + rm.msg);
                //}
                return false;
            }
            catch (Exception ex)
            {

                WriteLog("获取更新失败!! : " + ex.Message);
                richTextBox3.AddText("获取更新失败!!" + ex.Message);
                return false;
            }

        }
        
        private void button3_Click(object sender, EventArgs e)
        {
            FrmSetParameters sp = new FrmSetParameters(printConfig);
            sp.StartPosition = FormStartPosition.CenterParent;
            sp.ShowDialog();
            printConfig = Program.printConfigBll.SelectAll()[0];
            X = printConfig.FontX;
            Y = printConfig.FontY;
            X2 = printConfig.CodeX;
            Y2 = printConfig.CodeY;
            //X = Convert.ToInt32(ConfigHelper.GetValue("X", "0"));
            //Y = Convert.ToInt32(ConfigHelper.GetValue("Y", "0"));
        }

        private void button8_Click(object sender, EventArgs e)
        {
            FrmProductionRecord prf = new FrmProductionRecord(Token);
            prf.StartPosition = FormStartPosition.CenterParent;
            prf.ShowDialog();
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            if (productionInfo == null)
                return;
                Myprinter(qrCodePrint);
                WriteLog("重新打印");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            OperationDtFrm od = new OperationDtFrm();
            od.StartPosition = FormStartPosition.CenterParent;
            od.ShowDialog();
        }

        private void button11_Click(object sender, EventArgs e)
        {
           
            SumitPacking();
         
        }

        private void button12_Click(object sender, EventArgs e)
        {
            var dicPk = bagDic.Count == 0 ? (bagDicTwo.Count == 0 ? bagDicThree : bagDicTwo) : bagDic;
            label3.AddLab("总计：" + twoLineCount + "件  待组件：0");
            oneLineCount = oneLineCount - dicPk.Count;
            label2.AddLab("总计：" + oneLineCount + "  待装袋：0");
            richTextBox1.ClearInfo();
            richTextBox2.ClearInfo();
            judgePage.Clear();
            dicPk.Clear();
            WriteLog("返工 清除数据" + dicPk.Count + "条");
        }

        private void button13_Click(object sender, EventArgs e)
        {
            FrmPrintApplyFor fpaf = new FrmPrintApplyFor(Token,printConfig,codeUrl,UserNmae);
            fpaf.StartPosition = FormStartPosition.CenterParent;
            fpaf.ShowDialog();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            FrmAdministrator admin = new FrmAdministrator();
            admin.StartPosition = FormStartPosition.CenterParent;
            admin.ShowDialog();
        }
    }
    public class ProudctCorrelation
    {
        public string productId;
        public string productName;
        public string packingTime;
        public string productionTime;
        public string productSpecificationsId;
        public string productSpecificationsName;
        public string productionBatch; 
        public string teamId; 
        public List<ChildPro> relationUploadReqs;
    }

    public class ChildPro
    {
        public List<ThreeChild> children;
        public string qrCode;
    }

    public class ThreeChild
    {
        public List<Code> children;

        public string qrCode;
    }

    public class Code
    {
        public string qrCode;
    }
}
