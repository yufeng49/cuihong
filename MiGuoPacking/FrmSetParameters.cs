using Common.Utils;
using Entity.entity;
using MiGuoPacking.Model;
using System;
using System.Drawing;
using System.Drawing.Printing;
using System.IO.Ports;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace MiGuoPacking
{
    public partial class FrmSetParameters : Form
    {
        PrintConfig printConfig;
        public FrmSetParameters(PrintConfig pc)
        {
            printConfig = pc;
            InitializeComponent();

        }

        private void SetParameters_Load(object sender, EventArgs e)
        {
            //List<string> shortcutPaths = GetQuickFromFolder(systemStartPath, appAllPath);
            ////存在2个以快捷方式则保留一个快捷方式-避免重复多于
            //if (shortcutPaths.Count > 0)
            //{
            //    checkBox1.Checked = true;
            //}
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            textBox1.Text = ConfigHelper.GetValue("SingleNumber", "1");

            System.Drawing.Printing.PrintDocument print = new System.Drawing.Printing.PrintDocument();
            string sDefault = print.PrinterSettings.PrinterName;//默认打印机名
                                                                //if (sDefault.Length == 0)
                                                                //    MessageBox.Show("没有设置默认打印机");
            textBox2.Text = sDefault;
            comboBox2.Hide();
            comboBox3.Hide();
            button2.Hide();

            if (printConfig != null)
            {
                if (printConfig.PrintName != "")
                    textBox2.Text = printConfig.PrintName;
                textBox3.Text = printConfig.FontX.ToString();
                textBox4.Text = printConfig.FontY.ToString();
                textBox5.Text = printConfig.CodeX.ToString();
                textBox6.Text = printConfig.CodeY.ToString();
                comboBox1.Text = printConfig.PrintType;
                textBox7.Text = printConfig.CodeSize.ToString();
                if (printConfig.PrintType == "COM")
                {
                    comboBox2.Text = printConfig.Com;
                    comboBox3.Text = printConfig.BaudRate;
                    textBox7.Text = printConfig.CodeSize.ToString();
                }
            }
            else
            {
                comboBox1.SelectedIndex = 0;
                printConfig = new PrintConfig();
                Program.printConfigBll.Add(printConfig);
                printConfig = Program.printConfigBll.SelectAll()[0];
                comboBox3.SelectedIndex = 0;
            }
        }

        //寻找电脑连接的串口端口号：
        public void Search_Port(ComboBox mybox)
        {
            mybox.Items.Clear();
            mybox.Items.AddRange(SerialPort.GetPortNames());
            if (mybox.Items.Count > 0)
            {
                mybox.SelectedIndex = 0;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Dictionary<string, string> dic = new Dictionary<string, string>();

            //dic.Add("OperationName", "开机自启");
            //dic.Add("OperationName", "自动登录");
            //dic.Add("OperationName", "记住密码");
            //Program.basicSettingBll.Delete();
            //Program.basicSettingBll.Add(new BasicSetting { OperationName = "开机自启", Status = checkBox1.Checked });
            //Program.basicSettingBll.Add(new BasicSetting { OperationName = "自动登录", Status = checkBox1.Checked });
            //Program.basicSettingBll.Add(new BasicSetting { OperationName = "记住密码", Status = checkBox1.Checked });
        }

        private void button3_Click(object sender, EventArgs e)
        {

            try
            {
                if (comboBox1.Text == "USB" && textBox2.Text == "")
                {
                    MessageBox.Show("没有打印机名称");
                    return;
                }
                textBox1.Text = Convert.ToInt32(textBox1.Text).ToString();
                Program.singleNumber = textBox1.Text;
                ConfigHelper.Modify("SingleNumber", textBox1.Text);
                //ConfigHelper.Modify("PrinterName", textBox2.Text);
                //ConfigHelper.Modify("X", textBox3.Text);
                //ConfigHelper.Modify("Y", textBox4.Text);
                printConfig.PrintName = textBox2.Text;
                printConfig.FontX = Convert.ToInt32(textBox3.Text);
                printConfig.FontY = Convert.ToInt32(textBox4.Text);
                printConfig.CodeX = Convert.ToInt32(textBox5.Text);
                printConfig.CodeY = Convert.ToInt32(textBox6.Text);
                printConfig.PrintType = comboBox1.Text;

                printConfig.CodeSize = Convert.ToInt32(textBox7.Text);
                if (printConfig.PrintType == "COM")
                {
                    printConfig.Com = comboBox2.Text;
                    printConfig.BaudRate = comboBox3.Text;
                }
                Program.printConfigBll.Update(printConfig);
                // this.DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {

                textBox1.Text = "1";
            }
        }

        int X = 0;
        int Y = 0;
        int Qr_X = 0;
        int Qr_Y = 0;
        PrintDocument pd;
        /// <summary>
        /// 打印
        /// </summary>
        private void Myprinter()
        {
            try
            {
                pd = new PrintDocument();
                pd.PrintPage += new PrintPageEventHandler(printDocument_PrintA4Page);

                pd.DefaultPageSettings.PrinterSettings.PrinterName = textBox2.Text; ;//"ZDesigner GX430t";       //打印机名称
                                                                                     //  pd.DefaultPageSettings.PaperSize = new PaperSize("newPage70X40"
                                                                                     //  , (int)(m_pageWidth / 25.4 * 100)
                                                                                     //  , (int)(m_pageHeight / 25.4 * 100));                                                            //pd.DefaultPageSettings.Landscape = true;  //设置横向打印，不设置默认是纵向的
                pd.PrintController = new System.Drawing.Printing.StandardPrintController();
                pd.Print();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void printDocument_PrintA4Page(object sender, PrintPageEventArgs e)
        {
            Font fntTxt = new Font("楷体", 14, System.Drawing.FontStyle.Bold);//正文文字         
            Font fntTxt3 = new Font("楷体", 9, System.Drawing.FontStyle.Bold);
            System.Drawing.Brush brush = new SolidBrush(System.Drawing.Color.Black);//画刷      
            System.Drawing.Pen pen = new System.Drawing.Pen(System.Drawing.Color.Black);           //线条颜色         

            try
            {
                string code = qr_code;
                e.Graphics.DrawString("测试打印", fntTxt, brush, new System.Drawing.Point(X + 5, Y + 10));
              

                int qrsize = Convert.ToInt32(this.textBox7.Text);
                Bitmap bitmap = QrCode.CreateQRCode(code, qrsize, qrsize);
                e.Graphics.DrawImage(bitmap, new System.Drawing.Point(Qr_X + 65, Qr_Y + 25));


                //WriteLog("成功打印二维码: https://z.bzlsp.cn/p/14i7232bg8KG074");
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }
        string qr_code = "";
        private void button1_Click_1(object sender, EventArgs e)
        {
            qr_code = this.textBox8.Text.Trim();
            if (qr_code.Length > 0)
            {
                if (comboBox1.Text == "COM")
                {
                    PrintLab.PTK_CloseSerialPort();
                    int err = 0;
                    err = PrintLab.PTK_OpenSerialPort(uint.Parse(comboBox2.Text.Replace("COM", "")), uint.Parse(comboBox3.Text));
                    if (err != 0)
                    {
                        showErrorInfo(err);
                        return;
                    }
                    ComPrint();
                }
                else
                {
                    try
                    {
                        X = Convert.ToInt32(textBox3.Text);
                        Y = Convert.ToInt32(textBox4.Text);


                        Qr_X = Convert.ToInt32(textBox5.Text);
                        Qr_Y = Convert.ToInt32(textBox6.Text);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    Myprinter();
                }
            }
            else {
                MessageBox.Show("打印内容不能为空");
            }
        }

        PosTek_Text_T text;
        Bar2D_QR qr;
        private void ComPrint()
        {
            text.str = "14i7232bg8KG07\r\n" + DateTime.Now.ToString("yyyy-MM-dd") + "\r\nradmin-1\r\n【1*1*1】\r\n打印测试";
            int x2 = 660 - Convert.ToInt32(textBox3.Text);
            int y2 = 300 - Convert.ToInt32(textBox4.Text);
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

            var x = 570 - Convert.ToInt32(textBox5.Text);
            var y = 210 - Convert.ToInt32(textBox6.Text);
            qr.x = uint.Parse(x.ToString());
            qr.y = uint.Parse(y.ToString());
            qr.rotate = (uint)0;
            qr.zoom = uint.Parse(textBox7.Text);
            qr.version = (uint)0 + 1;
            qr.errorCorrectionLevel = (uint)3;
            qr.maskGraphics = (uint)8;
            qr.str = "https://z.bzlsp.cn/p/14i7232bg8KG07";
            byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(qr.str);
            errCode = PrintLab.PTK_DrawBar2D_QR(qr.x, qr.y, 0, qr.version, qr.rotate, qr.zoom, 0, qr.errorCorrectionLevel, qr.maskGraphics, byteArray);

            errCode = PrintLab.PTK_PrintLabel(1, 1);
            if (errCode != 0) showErrorInfo(errCode);
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "COM")
            {
                comboBox2.Show();
                comboBox3.Show();
                //  button2.Show();
                Search_Port(comboBox2);
            }
            else
            {
                comboBox2.Hide();
                comboBox3.Hide();
                //   button2.Hide();
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox2.Text == "")
            {
                MessageBox.Show("请选择串口");
                return;
            }
            if (comboBox3.Text == "")
            {
                MessageBox.Show("请选择波特率");
                return;
            }
            if (button2.Text == "关闭打印机")
            {
                PrintLab.PTK_CloseSerialPort();
                button2.Text = " 连接打印机";
                return;
            }
            int err = 0;
            err = PrintLab.PTK_OpenSerialPort(uint.Parse(comboBox2.Text.Replace("COM", "")), uint.Parse(comboBox3.Text));
            if (err != 0)
            {
                showErrorInfo(err);
                return;
            }
            button2.Text = "关闭打印机";
        }

        private void showErrorInfo(int errCode)
        {
            byte[] errInfo_Byte = new byte[1024];
            PrintLab.PTK_GetErrorInfo(errCode, errInfo_Byte, 1024);
            //string errInfo = System.Text.Encoding.Unicode.GetString(errInfo_Byte);
            string errInfo = System.Text.Encoding.GetEncoding("gbk").GetString(errInfo_Byte);
            MessageBox.Show(errInfo);
        }
    }
}
