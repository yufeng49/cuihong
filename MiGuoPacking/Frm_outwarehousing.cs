using Common;
using Common.Utils;
using Entity.entity;
using Helper;
using MiGuoPacking.Tool;
using Server.constant;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace MiGuoPacking
{
    public partial class Frm_outwarehousing : Form
    {
        public static string LogPath = Application.StartupPath + "\\Log";
        private string hdNo = "2d-TPX01#";

        /// <summary>
        /// 轮询出库单
        /// </summary>
        System.Timers.Timer timerRepeat = new System.Timers.Timer();
        string order = "";
        public Frm_outwarehousing()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            InitializeComponent();

        }


        System.Timers.Timer CheckTimer = new System.Timers.Timer();
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
        void BandData()
        {
            uiDataGridView1.ClrearData();
            //    List<EshengEntity> list = Program.eeb.SearchData(batch);
            // uiDataGridView1.DataSource = list;
            //uiDataGridView1.BandEntityData(list);

        }
        System.Timers.Timer SafeVoice = new System.Timers.Timer();
        private void WriteData(string folder, string info)
        {
            LogHelper.WriteLog(LogPath + folder, DateTime.Now.ToString() + info);
        }
        /// <summary>
        /// 生产语音
        /// </summary>
        SpeechHelper Prodsp = null;

        /// <summary>
        /// 软件状态，true为正在运行，false为停止
        /// </summary>
        bool FrmStatus = false;
        /// <summary>
        /// 安全语音
        /// </summary>
        SpeechHelper Safesp = null;
        int SafeVoice_Delay = 0;

        private void FlyHorseHome_Load(object sender, EventArgs e)
        {
            hdNo = ConfigHelper.GetValue("hdNo", "SYS001");
            SafeVoice_Delay = Convert.ToInt32(ConfigHelper.GetValue("SafeDelay", "5000"));

            #region 213
            timerRepeat.AutoReset = true;
            timerRepeat.Enabled = false;
            timerRepeat.Interval = 2000;
            timerRepeat.Elapsed += Timer_Elapsed;

            #region 扫码枪

            textBox1.LostFocus += new EventHandler((obj, ex) =>
            {
                textBox1.Focus();
            });
            #endregion
            CheckTimer.AutoReset = true;
            CheckTimer.Enabled = true;
            CheckTimer.Interval = 1000;
            CheckTimer.Elapsed += CheckTimer_Elapsed;
            CheckTimer.Start();

            SafeVoice.AutoReset = true;
            SafeVoice.Enabled = false;
            SafeVoice.Interval = 180000;
            SafeVoice.Elapsed += VoidSend_Elapsed;// new ElapsedEventHandler((s, es) => CheckOneTrackStatus_Elapsed(s, es, 1));
            #endregion 
            string Voiceip = ConfigHelper.GetValue("ProdVoiceip", "192.168.90.1");
            int Voiceport = Convert.ToInt32(ConfigHelper.GetValue("ProdVoiceport", "11"));


            string SVoiceip = ConfigHelper.GetValue("SProdVoiceip", "192.168.90.1");
            int SVoiceport = Convert.ToInt32(ConfigHelper.GetValue("SProdVoiceport", "11"));



            try
            {
                Prodsp = new SpeechHelper(Voiceip, Voiceport);//SendVoice.CreateSp(Voiceip, Voiceport);
                Safesp = new SpeechHelper(SVoiceip, SVoiceport);//SendVoice.CreateSp(Voiceip, Voiceport);
                Prodsp.Connect();
                richTextBox1.AddText("生产语音链接成功");

            }
            catch (Exception ex)
            {
                richTextBox1.AddText("生产语音链接失败:" + ex.Message);
            }

            try
            {
                Safesp.Connect();
                richTextBox1.AddText("安全语音链接成功");
            }
            catch (Exception ex)
            {
                richTextBox1.AddText("安全语音链接失败:" + ex.Message);
            }


            button9_Click(null, null);
        }
        private void CheckTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            groupBox4.AddText("状态  " + DateTime.Now.ToString("HH:mm:ss"));
        }
        private void VoidSend_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (Safesp.Connected)
            {
                try
                {
                    Safesp.TTS("#非工作人员请离开叉车作业现场");
                    Thread.Sleep(5000);
                    Safesp.TTS("#非工作人员请离开叉车作业现场");
                    Thread.Sleep(5000);
                    Safesp.TTS("#非工作人员请离开叉车作业现场");
                    // 安全语音每次播报重复三次内容，间隔三分钟播报一次    
                }
                catch { }

            }
            else
            {
                richTextBox1.AddText("播放失败，安全语音未链接");
            }
        }
        void resetinfo()
        {
            ls.Clear();
            order = "";
            uiLabel7.AddLab("未感应到派车单");
            uiLabel8.AddLab("请获取");
            uiLabel10.AddLab("0 吨");
            uiLabel12.AddLab("0 吨");
            uiLabel1.AddLab("请获取车牌号");
            uiDataGridView1.ClrearData();
        }
        int EliminateStatus = 0;
        public void StartEliminate(string name)
        {
            richTextBox1.AddText("发送信号：" + name);
            var list = Program.eliminateBll.SearchData(name);
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
            }
        }
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
                    richTextBox1.AddText(e.Message);
                }
            }
            else
            {
                richTextBox1.AddText("注意：端口未打开，请检查");
            }
        }
        private void SendVoice(string content, int delay, int times)
        {
            ThreadLocal<int> t_delay = new ThreadLocal<int>();//延迟
            ThreadLocal<string> t_content = new ThreadLocal<string>();//播报内容
            t_delay.Value = delay;
            t_content.Value = content;
            Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < times; i++)
                {
                    Thread.Sleep(t_delay.Value);
                    try
                    {
                        Prodsp.TTS(content);
                    }
                    catch { }
                }
            });
        }
        /// <summary>
        /// 出库
        /// </summary>
        private void outer()
        {
            //无出库数据，拿走出库单，删除本地数据并提示
            //  Program.ecb.DeleteData(order);

            if (order.Length > 0 && ls.Count > 0)
            {

                List<string> qrls = new List<string>();
                string qr = "";
                foreach (qrcodeinfo i in ls)
                {
                    if (i.status.Equals("1"))
                    {
                        qr += i.qrCode + ",";
                        qrls.Add(i.qrCode);
                    }
                }
                if (qrls.Count > 0)
                {
                    RequestData<object> rq = ResponseUtil.submitConfirm(ApiUrl.submitConfirm, order, hdNo, qrls);
                    if (rq.code.Equals("200"))
                    {
                        Program.ecb.FinshData(order, qr);
                        richTextBox1.AddText("出库单：" + order + " 已完成出库");
                        order = "";
                        try
                        {
                            //更新码状态
                            Program.ecb.UpdateDetailStatus(qrls);
                            Prodsp.TTS("#装货完成");
                            if (SafeVoice.Enabled)
                            {
                                SafeVoice.Stop();
                            }
                        }
                        catch (Exception ez)
                        {
                            richTextBox1.AddText("语音异常：" + ez.Message);
                        }
                        resetinfo();
                    }
                    else
                    {
                        richTextBox1.AddText("出库单异常：" + rq.msg);
                        resetinfo(); order = "";
                        //出库单异常 怎么处理？
                    }
                }
                else
                {
                    order = ""; resetinfo();

                    //无数据不提交
                }
            }
            else
            {
                //装货完成 关闭安全语音
                if (SafeVoice.Enabled)
                {
                    SafeVoice.Stop();
                }
                order = ""; resetinfo();
                richTextBox1.AddText("未感应到出库单");
                try
                {
                    Prodsp.TTS("#未感应到出库单");
                }
                catch (Exception ez)
                {
                    richTextBox1.AddText("语音异常：" + ez.Message);
                }
            }
            if (EliminateStatus == 1)
            {
                EliminateStatus = 0;
                StartEliminate("X4");
            }
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            timerRepeat.Stop();
            var res = ResponseUtil.CheckOutOrder(ApiUrl.checkout, hdNo, order);
            if (res.code.Equals("200"))
            {//获取数据成功
                if (order.Length <= 0)
                {
                    //感应到小票 打开安全语音
                    if (SafeVoice.Enabled == false)
                    {
                        Task.Factory.StartNew(() =>
                        {
                            try
                            {
                                Safesp.TTS("#非工作人员请离开叉车作业现场");
                                Thread.Sleep(5000);
                                Safesp.TTS("#非工作人员请离开叉车作业现场");
                                Thread.Sleep(5000);
                                Safesp.TTS("#非工作人员请离开叉车作业现场");
                            }
                            catch { }
                        });


                        SafeVoice.Start();
                    }
                    EliminateStatus = 1;
                    StartEliminate("X3");
                    try
                    {
                        Task.Factory.StartNew(() =>
                        {
                            try
                            {
                                Thread.Sleep(SafeVoice_Delay);
                                Prodsp.TTS("#开始装货");
                                richTextBox1.AddText("开始装货语音延迟：" + SafeVoice_Delay);
                            }
                            catch { }

                        });
                        //     SendVoice("#开始装货", SafeVoice_Delay,1);
                    }
                    catch (Exception ez)
                    {
                        richTextBox1.AddText("语音：" + ez.Message);
                    }
                }
                order = res.data.DispatchOrderId;
                Program.ecb.InsertData(res.data);
                ls = Program.ecb.GetDetailData(order);
                uiDataGridView1.BandEntityData(ls);
                uiLabel7.AddLab(order);
                uiLabel8.AddLab(res.data.MaterialName);
                uiLabel10.AddLab(res.data.TotalWeight + " 吨");
                uiLabel12.AddLab(res.data.OutWeight + " 吨");
                uiLabel1.AddLab(res.data.LicensePlateNo);
                totalduo = res.data.OutNum;
                totalweight = Convert.ToDecimal(res.data.OutWeight);
                planeweight = Convert.ToDecimal(res.data.TotalWeight);
            }
            else if (res.code.Equals("100008"))
            {
                try
                {
                    Prodsp.TTS("#装货完成");
                }
                catch (Exception ez)
                {
                    richTextBox1.AddText("语音异常：" + ez.Message);
                }
                richTextBox1.AddText(res.msg + "100");
            }
            else if (res.code.Equals("100009"))
            {
                try
                {
                    Prodsp.TTS("#车辆已超载");
                }
                catch (Exception ez)
                {
                    richTextBox1.AddText("语音异常：" + ez.Message);
                }
                richTextBox1.AddText(res.msg);
            }
            else if (res.code.Equals("100010"))
            {
                outer();
            }
            else if (res.code.Equals("100011"))
            {//无出库单
             //订单已完成  
                if (EliminateStatus == 1)
                {
                    EliminateStatus = 0;
                    StartEliminate("X4");
                }
                //装货完成 关闭安全语音
                if (SafeVoice.Enabled)
                {
                    SafeVoice.Stop();
                }
            }
            else
            {
                richTextBox1.AddText("获取数据异常：" + res.msg);
            }
            //    richTextBox3.AddText("定时上传数据 总数据" + list.Count + "条 成功上传" + num + "条数据 未上传" + (list.Count - num) + "条");
            timerRepeat.Start();
        }
        private void button9_Click(object sender, EventArgs e)
        {
            if (this.button9.Text.Equals("重新连接") || this.button9.Text.Equals("开启软件"))
            {
                FrmStatus = true;
                timerRepeat.Start();
                Open(); OneLineStart();
                SecondLineStart();
                bool flag = ResponseUtil.CheckStatus(ApiUrl.ping, "", "", new List<string>());
                uiLight1.State = flag ? Sunny.UI.UILightState.On : Sunny.UI.UILightState.Off;

                textBox1.Focus();
            }
            else
            {
                FrmStatus = false;
                timerRepeat.Stop();
                this.button9.Text = "开启软件";
                uiLight1.State = Sunny.UI.UILightState.Off;
                serialPort1.Close();
                OneClient.Dispose();
                SecondClient.Dispose();

            }
        }

        private void FlyHorseHome_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {

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
        private SocketClient OneClient;
        private SocketClient SecondClient;
        private void OneLineStart()
        {
            richTextBox1.AddText("连接一号相机请等待...");
            string ip = ConfigHelper.GetValue("Server", "192.168.3.3");
            string prot = ConfigHelper.GetValue("Prot", "51236");
            OneClient = new SocketClient("一");
            OneClient.OnError += new SocketClient.SocketEventHandler(OnClientError);
            OneClient.OnReceive += new SocketClient.SocketEventHandler(OnReceiveMessage);
            OneClient.OnClosed += new SocketClient.SocketEventHandler(OnClientClosed);
            OneClient.OnSucess += new SocketClient.SocketEventHandler(OnClientSucess);
            OneClient.ConnectServer(ip, prot);
        }
        private void SecondLineStart()
        {
            richTextBox1.AddText("连接二号相机请等待...");
            string ip = ConfigHelper.GetValue("Server1", "192.168.3.3");
            string prot = ConfigHelper.GetValue("Prot1", "51236");
            SecondClient = new SocketClient("二");
            SecondClient.OnError += new SocketClient.SocketEventHandler(OnClientError);
            SecondClient.OnReceive += new SocketClient.SocketEventHandler(OnReceiveMessage);
            SecondClient.OnClosed += new SocketClient.SocketEventHandler(OnClientClosed);
            SecondClient.OnSucess += new SocketClient.SocketEventHandler(OnClientSucess);
            SecondClient.ConnectServer(ip, prot);
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
            FrmStatus = false;
            return false;
        }
        private void WriteLog(string info)
        {
            LogHelper.WriteLog(LogPath, DateTime.Now.ToString() + info);
        }
        private bool OnClientSucess(object obj, string text)
        {
            SocketClient sc = (SocketClient)obj;
            richTextBox1.AddText(sc.SocketName + "号相机连接成功");
            if (sc.SocketName.Equals("一")) uiLight3.State = Sunny.UI.UILightState.On;
            button9.BeginInvoke((MethodInvoker)(() => { button9.Text = "断开链接"; }));
            return false;
        }
        List<qrcodeinfo> ls = new List<qrcodeinfo>();
        int totalduo = 0;
        //计划出库重量
        decimal planeweight = 0;
        //实际出库的总重量
        decimal totalweight = 0;
        private bool OnReceiveMessage(object obj, string text)
        {//相机采集到数据
            string getInfo = text.Replace("\u0002", "").Replace("\n", "").Replace("\r", "");
            getInfo = getInfo.Replace("#", "").Replace("&", "");

            WriteData("CecData", getInfo);
            if (getInfo.ToLower().Contains("noread"))
            {
                //noread不做任何处理
            }
            else
            {

                if (order.Length > 0)
                {
                    if (ls.Where(x => x.qrCode.Equals(getInfo)).Count() > 0)
                    {
                        richTextBox1.AddText("重复采集");
                    }
                    else
                    {
                        uiDataGridView1.ClrearData();
                        var ret = ResponseUtil.stockOut(ApiUrl.stockOut, order, hdNo, getInfo);
                        if (ret.code.Equals("200"))
                        {
                            ret.data.qrcodeinfo.status = "1";
                            ls.Add(ret.data.qrcodeinfo);
                            //添加单条出库记录
                            Program.ecb.InsertData(order, ret.data.qrcodeinfo);
                            totalduo++;
                            totalweight += Convert.ToDecimal(ret.data.qrcodeinfo.weight);
                            try
                            {
                                Prodsp.TTS("#寻卡成功");

                            }
                            catch (Exception ez)
                            {
                                richTextBox1.AddText("语音异常：" + ez.Message);
                            }
                            if (totalweight == planeweight)
                            {
                                outer();
                                //出库完成
                            }
                        }
                        else if (ret.code.Equals("100014"))
                        {
                            richTextBox1.AddText("实际出库重量超过计划出库重量");
                            try
                            {
                                Prodsp.TTS("#车辆已超载");
                            }
                            catch (Exception ez)
                            {
                                richTextBox1.AddText("语音异常：" + ez.Message);
                            }
                        }
                        else if (ret.code.Equals("100015"))
                        {
                            richTextBox1.AddText("出库物料与出库单物料不一致");
                            try
                            {
                                Prodsp.TTS("#品种不符");
                            }
                            catch (Exception ez)
                            {
                                richTextBox1.AddText("语音异常：" + ez.Message);
                            }
                        }
                        else
                        {
                            richTextBox1.AddText(ret.msg);
                        }
                        ls = ls.OrderByDescending(x => x.createTime).ToList();
                        uiDataGridView1.BandEntityData(ls);
                    }
                }
                else
                {
                    richTextBox1.AddText("无出库单信息");
                }
            }
            /*     if (eshengEntity == null)
            {
                richTextBox1.AddText("没有入库数据");
            }
            else
            {
                if (getInfo == eshengEntity.QrCode)
                {
                    eshengEntity.Collecttime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

                    //调用提交
                    RequestData<object> flag = ResponseUtil.CollectData(ApiUrl.collect, hdNo, eshengEntity.QrCode);

                    if (flag.code.Equals("200"))
                    {
                        Program.eeb.UpdateEntity(eshengEntity);
                        richTextBox1.AddText("入库成功");
                        count++;
                        label5.AddLab(count.ToString());
                        BandData();
                    }
                    else
                    {
                        richTextBox1.AddText("入库失败," + flag.msg);
                    }
                }
                else
                {
                    richTextBox1.AddText("采集的数据和当前不一致");
                }
            } 
       */
            return false;
        }
        #endregion

        private void button4_Click(object sender, EventArgs e)
        {
            Eliminate es = new Eliminate(serialPort1);
            // EliminateSet es = new EliminateSet(serialPort1);
            es.StartPosition = FormStartPosition.CenterScreen;
            es.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OutWareHouseLog es = new OutWareHouseLog();
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

        private void Frm_outwarehousing_Activated(object sender, EventArgs e)
        {
            this.textBox1.Focus();
        }

        private void uiDataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var val = this.uiDataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                string buttonText = val == null ? "" : val.Value.ToString();
                if (buttonText.Equals("删除"))
                {
                    string qrcode = this.uiDataGridView1.Rows[e.RowIndex].Cells["Column1"].Value.ToString();
                    RequestData<object> ret = ResponseUtil.delStockOut(ApiUrl.delStockOut, qrcode, hdNo);
                    if (ret.code.Equals("200"))
                    {
                        qrcodeinfo qq = ls.Where(x => x.qrCode.Equals(qrcode)).Single();
                        bool flag = ls.Remove(qq);

                        if (flag) richTextBox1.AddText("删除 " + qrcode + " 成功");
                        else richTextBox1.AddText("删除失败 查询到对应数据：" + ls.Where(x => x.qrCode.Equals(qrcode)).Count() + " 总数据：" + ls.Count);
                        uiDataGridView1.ClrearData();
                        ls = ls.OrderByDescending(x => x.createTime).ToList();
                        uiDataGridView1.BandEntityData(ls);
                    }
                    else
                    {
                        MessageBox.Show(ret.msg);
                    }
                }
            }
        }

        private void uiDataGridView1_Paint(object sender, PaintEventArgs e)
        {
            int rowcount = uiDataGridView1.RowCount;

            int showcount = uiDataGridView1.DisplayedRowCount(true);

            if (showcount == 0) return;

            System.Drawing.Rectangle currrct;

            int startNo = uiDataGridView1.FirstDisplayedCell.RowIndex;

            int ColNo = uiDataGridView1.FirstDisplayedCell.ColumnIndex;

            string stext = "";

            int nowy = 0;

            int hDelta = 0;

            for (int i = startNo; i < startNo + showcount; i++)

            {

                currrct = (System.Drawing.Rectangle)uiDataGridView1.GetCellDisplayRectangle(ColNo, i, true);

                nowy = currrct.Y + 2;

                stext = string.Format("{0, 3}", rowcount - i);

                if (hDelta == 0)

                    hDelta = (currrct.Height - uiDataGridView1.Font.Height) / 2;

                if (uiDataGridView1.Rows[i].Selected == true)

                    e.Graphics.DrawString(stext, uiDataGridView1.Font, new System.Drawing.SolidBrush(System.Drawing.Color.White), 10, nowy + hDelta);

                else

                    e.Graphics.DrawString(stext, uiDataGridView1.Font, new System.Drawing.SolidBrush(System.Drawing.Color.Black), 10, nowy + hDelta);

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Prodsp.TTS("#播放生产语音");
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Safesp.TTS("#播放安全语音");
        }

        private void Frm_outwarehousing_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (FrmStatus)
            {
                richTextBox1.AddText("请先停止生产");
                e.Cancel = true;
            }
        }
    }

}
