using Common;
using Common.Utils;
using Entity.entity;
using Helper;
using MiGuoPacking.Tool;
using Server.constant;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Timers;
using System.Windows.Forms;

namespace MiGuoPacking
{
    public partial class Frm_inwarehousing : Form
    {
        /// <summary>
        /// 设备码，硬件号
        /// </summary>
        private string hdNo = "";
        /// <summary>
        /// 批次号  默认为当前时间
        /// </summary>
        string batch = "";
        /// <summary>
        /// 日志路径
        /// </summary>
        private string LogPath = Application.StartupPath + "\\Log";
        private string CollectionData = Application.StartupPath + "\\CecData";
        private string PrintData = Application.StartupPath + "\\PrintData";
        /// <summary>
        /// 记录当次打印标签时间 打印标签时间间隔应该大于2秒
        /// </summary>
        DateTime Nowtime;
        /// <summary>
        /// 检测是服务器状态
        /// </summary>

        System.Timers.Timer timingSub = new System.Timers.Timer();

        SpeechHelper Prodsp = null;
        public Frm_inwarehousing()
        {
            InitializeComponent();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            //uiLight1.State = Sunny.UI.UILightState.On;
            //uiLight2.State = Sunny.UI.UILightState.On;
            //uiLight3.State = Sunny.UI.UILightState.On;
            //开启相机
            Nowtime = DateTime.Now;
            if (this.button9.Text.Equals("重新连接") || this.button9.Text.Equals("开启软件"))
            {
                OneLineStart();
                Open();
                FrmStatus = true;
                bool flag = ResponseUtil.CheckStatus(ApiUrl.ping, "", "", new List<string>());
                uiLight1.State = flag ? Sunny.UI.UILightState.On : Sunny.UI.UILightState.Off;


            }
            else
            {
                FrmStatus = false;
                this.button9.Text = "开启软件";
                uiLight1.State = Sunny.UI.UILightState.Off;
                serialPort1.Close();
                BottleClient.Dispose();

            }
        }

        #region 串口
        private void Open()
        {
            try
            {
                if (serialPort1.IsOpen)
                {
                    richTextBox1.AddText("串口打开");
                }
                else
                {
                    serialPort1.PortName = ConfigHelper.GetValue("Com", "Com2");
                    serialPort1.BaudRate = Convert.ToInt32(ConfigHelper.GetValue("BaudRate", "123"));
                    serialPort1.Parity = Parity.None;
                    serialPort1.DataBits = 8;
                    serialPort1.StopBits = StopBits.One;
                    serialPort1.Open();
                    richTextBox1.AddText("串口已打开");
                }
                uiLight2.State = Sunny.UI.UILightState.On;
            }
            catch
            {
                uiLight2.State = Sunny.UI.UILightState.Off;
                richTextBox1.AddText("注意：\n端口不存在或者被占用。\r");
            }
        }
        #endregion
        #region 相机
        private SocketClient BottleClient;

        private void OneLineStart()
        {
            richTextBox1.AddText("连接一号相机请等待...");
            string ip = ConfigHelper.GetValue("Server", "192.168.3.3");
            string prot = ConfigHelper.GetValue("Prot", "51236");
            BottleClient = new SocketClient("一");
            BottleClient.OnError += new SocketClient.SocketEventHandler(OnClientError);
            BottleClient.OnReceive += new SocketClient.SocketEventHandler(OnReceiveMessage);
            BottleClient.OnClosed += new SocketClient.SocketEventHandler(OnClientClosed);
            BottleClient.OnSucess += new SocketClient.SocketEventHandler(OnClientSucess);
            BottleClient.ConnectServer(ip, prot);
        }

        private bool OnClientError(object obj, string text)
        {
            SocketClient sc = (SocketClient)obj;
            richTextBox1.AddText(sc.SocketName + "号相机连接失败!!");
            if (sc.SocketName.Equals("一")) uiLight3.State = Sunny.UI.UILightState.Off;


            WriteLog(sc.SocketName + "号相机连接失败!!  " + text);
            button9.BeginInvoke((MethodInvoker)(() => { button9.Text = "重新连接"; FrmStatus = false; }));
            return false;
        }

        private bool OnClientClosed(object obj, string text)
        {
            SocketClient sc = (SocketClient)obj;
            richTextBox1.AddText(sc.SocketName + "号相机连接断开!!");
            if (sc.SocketName.Equals("一")) uiLight3.State = Sunny.UI.UILightState.Off;

            WriteLog(sc.SocketName + "号相机连接断开!!  " + text);
            button9.BeginInvoke((MethodInvoker)(() => { button9.Text = "重新连接"; FrmStatus = false; }));
            return false;
        }

        private bool OnClientSucess(object obj, string text)
        {
            SocketClient sc = (SocketClient)obj;
            richTextBox1.AddText(sc.SocketName + "号相机连接成功");
            if (sc.SocketName.Equals("一")) uiLight3.State = Sunny.UI.UILightState.On;
            button9.BeginInvoke((MethodInvoker)(() => { button9.Text = "断开链接"; }));
            return false;
        }
        //入库成功数量统计
        int count = 0;

        /// <summary>
        /// 采集成功写入list 验重用
        /// </summary>
        List<string> list = new List<string>();
        private bool OnReceiveMessage(object obj, string text)
        {//相机采集到数据
            string getInfo = text.Replace("\u0002", "").Replace("\n", "").Replace("\r", "");
            getInfo = getInfo.Replace("#", "").Replace("&", "");
            ThreadLocal<string> t_getInfo = new ThreadLocal<string>();//缓存区阻挡
            t_getInfo.Value = getInfo;

            WriteCollectionLog(t_getInfo.Value);
            if (eshengEntity == null)
            {
                richTextBox1.AddText("没有入库数据");
                try
                {
                    Prodsp.TTS("#入库失败入库失败入库失败");
                }
                catch { }
            }
            else
            {
                //if (list.Contains(t_getInfo.Value))
                //{
                //    richTextBox1.AddText(t_getInfo.Value + "已采集");
                //    Prodsp.TTS("#入库失败");
                //}
                //else
                {
                    if (t_getInfo.Value == eshengEntity.QrCode)
                    {
                        eshengEntity.Collecttime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

                        //调用提交
                        RequestData<object> flag = ResponseUtil.CollectData(ApiUrl.collect, hdNo, eshengEntity.QrCode, "");

                        if (flag.code.Equals("200"))
                        {
                            eshengEntity.Status = "3";
                            Program.eeb.UpdateEntity(eshengEntity);
                            richTextBox1.AddText("入库成功");
                            count++;
                            list.Add(t_getInfo.Value);
                            label5.AddLab(count.ToString());
                            BandData();

                        }
                        else
                        {
                            if (flag.code.Equals("100002"))
                            {
                                eshengEntity.Status = "3";
                                Program.eeb.UpdateEntity(eshengEntity);
                                richTextBox1.AddText("入库成功_100002");
                                count++;
                                list.Add(t_getInfo.Value);
                                label5.AddLab(count.ToString());
                                BandData();
                            }
                            else
                            {
                                try
                                {
                                    Prodsp.TTS("#入库失败入库失败入库失败");
                                }
                                catch(Exception exx) {
                                    richTextBox1.AddText(eshengEntity.QrCode + "入库异常," + exx.Message);
                                }
                                richTextBox1.AddText(eshengEntity.QrCode + "入库失败," + flag.msg );
                            }
                        }
                    }
                    else
                    { //获取的随机码和扫描的随机码不一致则先比对已有的随机码 
                        if (t_getInfo.Value.ToLower().Contains("noread"))
                        {//表示当前获取的码采集失败，通知服务器 采集下一个
                            RequestData<object> flag = ResponseUtil.CollectData(ApiUrl.collect, hdNo, eshengEntity.QrCode, "&qrCodeFlow=COLLECT_FAIL");

                            eshengEntity.Status = "4";
                            Program.eeb.UpdateEntity(eshengEntity);
                            //  Program.eeb.DeleteEntity(eshengEntity.QrCode);
                            BandData();
                            richTextBox1.AddText("采集失败，本次获取数据后延");
                            try
                            {
                                Prodsp.TTS("#入库失败入库失败入库失败");
                            }
                            catch { }
                        }
                        else
                        {
                            var datas = uiDataGridView1.Rows;
                            bool status = false;
                            string instatus = "";
                            foreach (DataGridViewRow datarow in datas)
                            {
                                string str = datarow.Cells[1].Value.ToString().Trim();
                                instatus = datarow.Cells[7].Value.ToString().Trim();
                                if (t_getInfo.Value.Equals(str) && (instatus.Equals("已打印") || instatus.Contains("未采集")))
                                {
                                    /*        gridView.Rows[index].Cells[0].Value = item.Id;
                        gridView.Rows[index].Cells[1].Value = item.QrCode;
                        gridView.Rows[index].Cells[2].Value = item.MaterialName;
                        gridView.Rows[index].Cells[3].Value = item.MaterialNo;
                        gridView.Rows[index].Cells[4].Value = item.WeightType;
                        gridView.Rows[index].Cells[5].Value = item.Printtime.Length > 0 ? Convert.ToDateTime(item.Printtime).ToString("yyyy-MM-dd HH:mm") : item.Printtime;
                        gridView.Rows[index].Cells[6].Value = item.Collecttime.Length>0 ?Convert.ToDateTime(item.Collecttime).ToString("yyyy-MM-dd HH:mm") : item.Collecttime;
                        gridView.Rows[index].Cells[7].Value = getstatus(item.Inwarehouse);
                        gridView.Rows[index].Cells[8].Value = "补打";*/
                                    RequestData<object> flag = ResponseUtil.CollectData(ApiUrl.collect, hdNo, t_getInfo.Value, "");
                                    eshengEntity.Status = "3";
                                    eshengEntity.Id = Convert.ToInt64(datarow.Cells[0].Value);
                                    eshengEntity.Collecttime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                                    Program.eeb.UpdateEntity(eshengEntity);
                                    status = true;
                                    list.Add(t_getInfo.Value);
                                    richTextBox1.AddText("补打当前数据:" + t_getInfo.Value);
                                    count++;
                                    label5.AddLab(count.ToString());

                                    BandData();
                                    break;
                                }
                            }
                            if (status)
                            {
                                richTextBox1.AddText("采集补打成功");
                            }
                            else
                            {
                                RequestData<object> flag = ResponseUtil.CollectData(ApiUrl.collect, hdNo, eshengEntity.QrCode, "&qrCodeFlow=COLLECT_FAIL");
                                WriteCollectionLog("采集码不一致或状态错误,采集码:" + t_getInfo.Value + "#列表码:" + eshengEntity.QrCode + " 状态:" + instatus);
                                richTextBox1.AddText("采集的数据未在待采集列表");
                                try
                                {
                                    Prodsp.TTS("#入库失败入库失败入库失败");
                                }
                                catch { }
                            }

                        }
                        //  WriteCollectionLog("采集不一致,采集码:" + getInfo + "#列表码:" + eshengEntity.QrCode);
                        //    richTextBox1.AddText("采集的数据和当前不一致");
                    }
                }
            }
            return false;
        }
        #endregion

        private void WriteLog(string info)
        {
            LogHelper.WriteLog(LogPath, DateTime.Now.ToString() + info);
        }
        private void WritePrintLog(string info)
        {
            LogHelper.WriteLog(PrintData, DateTime.Now.ToString() + info);
        }
        private void WriteCollectionLog(string info)
        {
            LogHelper.WriteLog(CollectionData, DateTime.Now.ToString() + info);
        }
        /// <summary>
        /// 查询入库信息
        /// </summary>
        void BandData()
        {
            uiDataGridView1.ClrearData();
            List<EshengEntity> list = Program.eeb.SearchData(batch).OrderByDescending(x => x.Printtime).ToList();
            // uiDataGridView1.DataSource = list;
            uiDataGridView1.BandEntityData(list);

        }
        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            //数据处理
            DateTime indatatime = DateTime.Now;
            if (indatatime.AddSeconds(-DelayTime) > Nowtime)
            {
                Thread.Sleep(50);
                string signal = "";
                byte[] buffer = new byte[1024];
                int bytes = 0;
                bytes = serialPort1.BytesToRead;
                serialPort1.Read(buffer, 0, bytes);
                for (int i = 0; i < bytes; i++)
                {
                    signal += Convert.ToString(buffer[i], 16).Replace("\r\n", "") + "-";
                }
                signal = signal.Length > 0 ? signal.Substring(0, signal.Length - 1) : signal;
                if (signal.Contains(gdsignal))
                { //拉取物料信息
                    RequestData<EshengEntity> ret = ResponseUtil.CheckOrder(ApiUrl.printQrCode, hdNo);
                    if (ret.code.Equals("200"))
                    {//有数据写入
                        ret.data.Batch = batch;
                        //拉取成功以后打印
                        Nowtime = indatatime;
                        ret.data.Printtime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                        Myprinter(ret.data.MaterialName, ret.data.QrCode, ret.data.Printtime, ret.data.WeightType);
                        bool flag = Program.eeb.InsertData(ret.data);
                        if (flag) eshengEntity = ret.data;
                        else
                        {
                            richTextBox1.AddText("写入数据失败:" + ret.data.QrCode);
                        }
                        //写入完成后绑定列表
                        BandData();
                    }
                    else
                    {
                        richTextBox1.AddText(ret.msg);
                    }
                }
                else
                {
                    richTextBox1.AddText(signal);
                }
            }
        }
        /// <summary>
        /// 软件状态，true为正在运行，false为停止
        /// </summary>
        bool FrmStatus = false;
        int DelayTime = 2;
        /// <summary>
        /// 光电信号
        /// </summary>
        string gdsignal;
        private void Frm_inwarehousing_Load(object sender, EventArgs e)
        {
            hdNo = ConfigHelper.GetValue("hdNo", "SYS001");
            gdsignal = ConfigHelper.GetValue("gdsignal", "gdsignal");
            string Voiceip = ConfigHelper.GetValue("ProdVoiceip", "192.168.1.1");


            DelayTime = Convert.ToInt32(ConfigHelper.GetValue("DelayTime", "2"));
            int Voiceport = Convert.ToInt32(ConfigHelper.GetValue("ProdVoiceport", "11"));

            try
            {
                Prodsp = new SpeechHelper(Voiceip, Voiceport);//SendVoice.CreateSp(Voiceip, Voiceport);
                Prodsp.Connect();
                richTextBox1.AddText("语音链接成功");
            }
            catch (Exception ex)
            {
                richTextBox1.AddText("语音链接失败:" + ex.Message);
            }
            batch = DateTime.Now.ToString("yyyyMMdd");
            this.label6.AddLab(batch);

            //获取打印机设置
            var prints = Program.printConfigBll.SelectAll();
            if (prints.Count != 0)
            {
                printConfig = prints[0];
                X = printConfig.FontX;
                Y = printConfig.FontY;
                X2 = printConfig.CodeX;
                Y2 = printConfig.CodeY;
                CodeSize = printConfig.CodeSize;

            }

            #region 扫码枪
            //textBox1.LostFocus += new EventHandler((obj, ex) => {
            //    textBox1.Focus();
            //});
            #endregion


            timingSub.AutoReset = true;
            timingSub.Enabled = true;
            timingSub.Interval = 1000;
            timingSub.Elapsed += Timer_Elapsed;
            timingSub.Start();



            button9_Click(null, null);
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            timingSub.Stop();
            groupBox4.AddText("状态  " + DateTime.Now.ToString("HH:mm:ss"));

            timingSub.Start();
        }
        EshengEntity eshengEntity = new EshengEntity();
        private void button1_Click(object sender, EventArgs e)
        {
            string qrcode = this.textBox2.Text.Trim();

            RequestData<EshengEntity> ret = ResponseUtil.FromOrder(ApiUrl.setQrCode, hdNo, qrcode);

            if (ret.code.Equals("200"))
            {//有数据写入
                if (qrcode.Length > 0)
                {
                    richTextBox1.AddText("从 " + qrcode + " 开始拉取码");
                    this.textBox2.Clear();
                }


                ret.data.Batch = batch;
                //拉取成功以后打印
                ret.data.Printtime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                Myprinter(ret.data.MaterialName, ret.data.QrCode, ret.data.Printtime, ret.data.WeightType);



                bool flag = Program.eeb.InsertData(ret.data);
                if (flag) eshengEntity = ret.data;
                //写入完成后绑定列表
                BandData();
            }
            else
            {
                richTextBox1.AddText(ret.msg);
            }
        }

        private void uiDataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var val = this.uiDataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                string buttonText = val == null ? "" : val.Value.ToString();
                if (buttonText.Equals("补打"))
                {
                    string qrcode = this.uiDataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                    long id = Convert.ToInt64(this.uiDataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                    string materialname = this.uiDataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                    string weight_type = this.uiDataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();

                    Myprinter(materialname, qrcode, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), weight_type);
                    richTextBox1.AddText("补打标签" + qrcode);
                }
            }
        }


        #region  打印
        private PrintConfig printConfig;
        private string qrCodePrint = "";
        private string p_material_name = "";
        private string p_collecttime = "";
        private string p_weight_type = "";

        PrintDocument pd;
        int X = 0;
        int Y = 0;
        int X2 = 0;
        int Y2 = 0;
        int CodeSize = 74;

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="material_name">物料名称</param>
        /// <param name="qr_code">垛型</param>
        /// <param name="collecttime">采集时间</param>
        /// <param name="weight_type">二维码</param>
        private void Myprinter(string material_name, string qr_code, string collecttime, string weight_type)
        {

            qrCodePrint = qr_code;
            p_material_name = material_name;
            p_collecttime = collecttime;
            p_weight_type = weight_type;
            if (printConfig == null || printConfig.PrintType == "")
            {
                richTextBox1.AddText("请先设置打印机");
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
                                                                                                 //  , (int)(m_pageHeight / 25.4 * 100));                                                            //pd.DefaultPageSettings.Landscape = true;  //设置横向打印，不设置默认是纵向的
                    pd.PrintController = new System.Drawing.Printing.StandardPrintController();
                    // string str2 = PrinterHelper.GetPrinterStatus(PrinterName);

                    pd.Print();
                }
                catch (Exception ex)
                {
                    richTextBox1.AddText(ex.Message);
                }
                return;
            }
            else
            {
                richTextBox1.AddText("打印机类型设置错误");
            }
        }
        private void printDocument_PrintA4Page(object sender, PrintPageEventArgs e)
        {

            //  Thread.CurrentThread.Priority = ThreadPriority.Highest;
            ThreadLocal<string> t_material_name = new ThreadLocal<string>();//缓存区阻挡
            ThreadLocal<string> t_weight_type = new ThreadLocal<string>();//缓存区阻挡
            ThreadLocal<string> t_qrCodePrint = new ThreadLocal<string>();//缓存区阻挡
            ThreadLocal<string> t_p_collecttime = new ThreadLocal<string>();//缓存区阻挡

            t_material_name.Value = p_material_name;
            t_weight_type.Value = p_weight_type;
            t_qrCodePrint.Value = qrCodePrint;
            t_p_collecttime.Value = p_collecttime;

            Font fntTxt = new Font("楷体", 14, System.Drawing.FontStyle.Bold);//正文文字         
            Font fntTxt3 = new Font("楷体", 8, System.Drawing.FontStyle.Bold);
            System.Drawing.Brush brush = new SolidBrush(System.Drawing.Color.Black);//画刷      
            System.Drawing.Pen pen = new System.Drawing.Pen(System.Drawing.Color.Black);           //线条颜色         

            try
            {//
                e.Graphics.DrawString("物料名称:" + t_material_name.Value, fntTxt, brush, new System.Drawing.Point(X + 5, Y + 10));
                e.Graphics.DrawString("垛型:" + t_weight_type.Value + " 吨", fntTxt, brush, new System.Drawing.Point(X + 5, Y + 60));
                e.Graphics.DrawString("随机码:" + t_qrCodePrint.Value, fntTxt, brush, new System.Drawing.Point(X + 5, Y + 35));
                //e.Graphics.DrawString("生产日期：" + prodate, fntTxt, brush, new System.Drawing.Point(X + 5, Y + 35));
                //  e.Graphics.DrawString("序号：" + printIndex + "", fntTxt, brush, new System.Drawing.Point(X + 5, Y + 60));
                //e.Graphics.DrawString(DateTime.Now.ToString(), fntTxt3, brush, new System.Drawing.Point(X, Y + 30));
                // e.Graphics.DrawString(qrCodePrint, fntTxt3, brush, new System.Drawing.Point(X + 5, Y + 80));
                //  e.Graphics.DrawString(policyLp, fntTxt3, brush, new System.Drawing.Point(X + 5, Y + 95));

                string code = t_qrCodePrint.Value;
                Bitmap bitmap = QrCode.CreateQRCode(code, CodeSize, CodeSize);
                e.Graphics.DrawImage(bitmap, new System.Drawing.Point(X2 + 65, Y2 + 25));
                // richTextBox5.AddText(code);
                richTextBox1.AddText("已成功打印二维码");
                WriteLog("成功打印二维码: " + code);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        #endregion

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

            CodeSize = printConfig.CodeSize;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Eliminate es = new Eliminate(serialPort1);
            // EliminateSet es = new EliminateSet(serialPort1);
            es.StartPosition = FormStartPosition.CenterScreen;
            es.ShowDialog();
        }


        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            string text = this.textBox1.Text;
            if (e.KeyChar == 13 && text.Length > 0)
            {
                OnReceiveMessage(null, text);
                this.textBox1.Clear();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            InWareHouseLog es = new InWareHouseLog();
            // EliminateSet es = new EliminateSet(serialPort1);
            es.StartPosition = FormStartPosition.CenterScreen;
            es.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            richTextBox1.ClearInfo();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Program.eeb.DeleteAll();
            BandData();
        }

        private void Frm_inwarehousing_Activated(object sender, EventArgs e)
        {
            this.textBox1.Focus();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string qrcode = this.textBox2.Text;

            RequestData<EshengEntity> ret = ResponseUtil.CheckOrder(ApiUrl.printQrCode, hdNo, qrcode);

            if (ret.code.Equals("200"))
            {
                richTextBox1.AddText("提交成功，从 " + qrcode + " 后开始拉取");

            }
            else
            {
                richTextBox1.AddText(ret.msg);
            }
            this.textBox2.Clear();


            textBox1.Focus();
        }

        private void textBox2_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void Frm_inwarehousing_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (FrmStatus)
            {
                richTextBox1.AddText("请先停止生产");
                e.Cancel = true;
            }
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            RequestData<EshengEntity> ret = ResponseUtil.CheckOrder(ApiUrl.printQrCode, hdNo);
            if (ret.code.Equals("200"))
            {//有数据写入
                ret.data.Batch = batch;
                //拉取成功以后打印
                ret.data.Printtime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                Myprinter(ret.data.MaterialName, ret.data.QrCode, ret.data.Printtime, ret.data.WeightType);
                bool flag = Program.eeb.InsertData(ret.data);
                if (flag) eshengEntity = ret.data;
                else
                {
                    richTextBox1.AddText("写入数据失败:" + ret.data.QrCode);
                }
                //写入完成后绑定列表
                BandData();
            }
            else
            {
                richTextBox1.AddText(ret.msg);
            }
        }

        private void button7_Click_2(object sender, EventArgs e)
        {
            bool flag = Prodsp.TTS("#测试语音发送");
            if (flag) richTextBox1.AddText("语音发送成功");
            else richTextBox1.AddText("语音发送失败");
        }
    }
}
