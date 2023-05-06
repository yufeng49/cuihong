using Entity.entity;
using MiGuoPacking.Tool;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Ports;
using System.Threading;
using System.Windows.Forms;

namespace MiGuoPacking
{
    public partial class Eliminate : Form
    {
        public Eliminate(SerialPort sp)
        {
            serialPort1 = sp;
            //serialPort1.DiscardInBuffer();
            InitializeComponent();
        }

        private SerialPort serialPort1;
        private int selectOff = 0;

        public List<EliminateModel> eliminate = new List<EliminateModel>();
        private void Form2_Load(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                button1.Text = "关闭";
            }
            else
            {
                button1.Text = "打开";
            }

            Search_Port(serialPort1, comboBox1);
            eliminate = Program.eliminateBll.SelectAll();
            if (eliminate.Count == 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    EliminateModel em = new EliminateModel();
                    em.CommandName = "X" + (i + 1);
                    em.TimeDelay = 500;
                    em.Switch = (i + 1);
                    em.CloseTime = 50;
                    em.Description = "";
                    em.Instructions = "";
                    eliminate.Add(em);
                }
                Program.eliminateBll.Add(eliminate);
                eliminate = Program.eliminateBll.SelectAll();
            }
            foreach (var item in eliminate)
            {
                int index = this.dataGridView1.Rows.Add();
                dataGridView1.Rows[index].Cells[0].Value = item.CommandName;
                dataGridView1.Rows[index].Cells[1].Value = item.TimeDelay;
                dataGridView1.Rows[index].Cells[2].Value = item.Switch;
                dataGridView1.Rows[index].Cells[3].Value = item.CloseTime;
                dataGridView1.Rows[index].Cells[4].Value = item.Description;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (dataGridView1.Rows[e.RowIndex].Cells[4].Selected)
                {
                    int index = dataGridView1.CurrentRow.Index;//选中的行    
                    if (this.dataGridView1.Rows[index].Cells[4].EditedFormattedValue.ToString() == "True") //checkbox的是否勾选
                    {
                        //   dataGridView1.Rows[index].Cells[0].Value = false;

                    }
                    else
                    {

                    }
                }
            }
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            SolidBrush b = new SolidBrush(this.dataGridView1.RowHeadersDefaultCellStyle.ForeColor);
            e.Graphics.DrawString((e.RowIndex + 1).ToString(System.Globalization.CultureInfo.CurrentUICulture), this.dataGridView1.DefaultCellStyle.Font, b, e.RowBounds.Location.X + 20, e.RowBounds.Location.Y + 4);
        }

        /// <summary>
        /// 将16进制的字符串转为byte[]
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static byte[] StrToHexBytePart(byte[] bt, string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString = "0" + hexString;
            if (hexString.Length == 2)
            {
                bt[4] = 0x00;
                bt[5] = Convert.ToByte(hexString.Substring(0 * 2, 2), 16);
                return bt;
            }
            bt[4] = Convert.ToByte(hexString.Substring(0 * 2, 2), 16);
            bt[5] = Convert.ToByte(hexString.Substring(1 * 2, 2), 16);
            return bt;
        }

        public static byte StrToBtPart(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString = "0" + hexString;
            byte bt = Convert.ToByte(hexString.Substring(0 * 2, 2), 16);

            return bt;
        }

        public byte[] CreateBtToStr(int off, int closeTime)
        {
            String strA = closeTime.ToString("x2");
            byte[] byte1 = new byte[8];
            byte1[0] = 0x01;
            byte1[1] = 0x06;
            byte1[2] = 0x00;
            byte1[3] = Convert.ToByte(off);
            //  byte1[4] = 0x00;
            byte1 = StrToHexBytePart(byte1, strA);
            //  byte1[5] = Convert.ToByte(closeTime, 16);
            int crc16 = modbus_crc(byte1, 6);
            byte1[6] = (byte)(crc16 & 0x00FF);
            byte1[7] = (byte)((crc16 >> 8) & 0xFF);
            return byte1;
        }
        public byte[] CreateBtToStrTwo(int off, int closeTime)
        {
            string strA = closeTime.ToString("x2");
          //  string offStr = off.ToString("x2");
            byte[] byte1 = new byte[8];
            byte1[0] = 0x01;
            byte1[1] = 0x06;
            byte1[2] = 0x00;
            byte1[3] = StrToBtPart(off.ToString()); //
            //  byte1[4] = 0x00;
            byte1 = StrToHexBytePart(byte1, strA);
            //  byte1[5] = Convert.ToByte(closeTime, 16);
            int crc16 = modbus_crc(byte1, 6);
            byte1[6] = (byte)(crc16 & 0x00FF);
            byte1[7] = (byte)((crc16 >> 8) & 0xFF);
            return byte1;
        }


        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow item in dataGridView1.Rows)
            {
                string xh = item.Cells[0].Value.ToString();
                int ys = Convert.ToInt32(item.Cells[1].Value.ToString());
                int kg = Convert.ToInt32(item.Cells[2].Value.ToString()) + 99;
                int closeTime = Convert.ToInt32(item.Cells[3].Value.ToString());
                if (closeTime > 65535)
                {
                    closeTime = 65535;
                    richTextBox1.AppendText("闭合时间最大为99");
                }
                // int yx = item.Cells[4].Value.ToString() == "True" ? 1 : 2;
                var byte1 = CreateBtToStr(kg, closeTime);
                string str = "";
                string str1 = "";
                for (int i = 0; i < 8; i++)
                {
                    str = Convert.ToString(byte1[i], 16).ToUpper();
                    str1 = str1 + (str.Length == 1 ? "0" + str : str) + "";
                }
                str1 = str1.Trim();

                int x = item.Index;
                eliminate[x].CommandName = xh;
                eliminate[x].TimeDelay = ys;
                eliminate[x].Switch = Convert.ToInt32(item.Cells[2].Value.ToString());
                eliminate[x].CloseTime = closeTime; ;
                eliminate[x].Description = item.Cells[4].Value.ToString();
                eliminate[x].Instructions = str1;
                //if (eliminate[x].CommandName == "X3")
                //    eliminate[x].Instructions = "0105000200006C0A";
                //if (eliminate[x].CommandName == "X4")
                //    eliminate[x].Instructions = "01050002FF002DFA";
            }
            Program.eliminateBll.Update(eliminate);
            richTextBox1.AddText("保存成功");

        }

        /// <summary>
        /// 将16进制的字符串转为byte[]
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static byte[] StrToHexByte(byte[] by, string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }

        //串口
        /// <summary>
        /// 打开串口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                if (button1.Text == "打开")
                {
                    button1.Text = "关闭";
                    serialPort1.PortName = comboBox1.Text;
                    serialPort1.BaudRate = Convert.ToInt32(comboBox2.Text);
                    serialPort1.Parity = Parity.None;
                    serialPort1.DataBits = 8;
                    serialPort1.StopBits = StopBits.One;
                    serialPort1.Open();
                    richTextBox1.AddText("串口已打开\r");
                }
                else if (button1.Text == "关闭")
                {
                    button1.Text = "打开";
                    serialPort1.Close();
                    richTextBox1.AddText("串口已关闭\r");
                }
            }
            catch
            {
                richTextBox1.AddText("注意：\n端口不存在或者被占用。\r");
            }
        }

        private int stopTime = 0; //暂停时间
        /// <summary>
        /// 剔除信息发送测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            selectOff = dataGridView1.CurrentRow.Index;
            stopTime = Int32.Parse(dataGridView1.Rows[selectOff].Cells[1].Value.ToString());
            Thread t = new Thread(Test);
            t.Start();

        }

        public void StartEliminate(int id)
        {
            var list = Program.eliminateBll.GetDataById(id + 1);
            foreach (var item in list)
            {
                Thread thread = new Thread(() =>
                {
                    //                             0105000200006C0A
                    // eliminate[x].Instructions = "0105000100009C0A";
                    //   eliminate[x].Instructions = "01050001FF00DDFA";
                    //01050002FF002DFA
                    Thread.Sleep(Convert.ToInt32(item.TimeDelay));
                    byte[] Data = new byte[8];
                    string instructions = item.Instructions;
                    richTextBox1.AddText("成功发送指令：" + instructions + "\r");
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

        private void Test()
        {
            Thread.Sleep(stopTime);
            // int crc16;//01 06 00 66 00 38 68 07
            int off = Int32.Parse(dataGridView1.Rows[selectOff].Cells[2].Value.ToString()) + 99;
            int closeTime = Convert.ToInt32(dataGridView1.Rows[selectOff].Cells[3].Value.ToString());//闭合时间
            byte[] byte1 = CreateBtToStr(off, closeTime);

            string str = "";
            string str1 = "";
            for (int i = 0; i < 8; i++)
            {
               // str = i ==3? Convert.ToString(byte1[i]).ToUpper(): Convert.ToString(byte1[i], 16).ToUpper();
                str  = Convert.ToString(byte1[i], 16).ToUpper();
                str1 = str1 + (str.Length == 1 ? "0" + str : str) + " ";
            }
            richTextBox1.AddText("成功发送指令：" + str1 + "\r");
            serial_send(byte1, 8);
        }

        /// <summary>
        /// CRC码
        /// </summary>
        /// <param name="data"></param>
        /// <param name="leng"></param>
        /// <returns></returns>
        private int modbus_crc(byte[] data, byte leng)
        {
            int i, j;
            int crc16 = 0xFFFF;
            for (i = 0; i < leng; i++)
            {
                crc16 ^= data[i];
                for (j = 0; j < 8; j++)
                {
                    if ((crc16 & 0x01) == 1)
                    {
                        crc16 = (crc16 >> 1) ^ 0xA001;

                    }
                    else
                    {
                        crc16 = crc16 >> 1;
                    }
                }
            }
            return crc16;
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
                    // richTextBox1.AddText(e.Message+"\r");
                }
            }
            else
            {
                // richTextBox1.AddText("注意：\n端口未打开，请检查.\r");
            }
        }
        /// <summary>
        /// 刷新端口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button7_Click(object sender, EventArgs e)
        {
            //  richTextBox1.AddText("刷新成功\r");
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

        }

        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;

            // comm.Close();
        }
        /// <summary>
        /// 从十进制转换到十六进制
        /// </summary>
        /// <param name="ten"></param>
        /// <returns></returns>
        public static string Ten2Hex(string ten)
        {
            ulong tenValue = Convert.ToUInt64(ten);
            ulong divValue, resValue;
            string hex = "";
            do
            {
                //divValue = (ulong)Math.Floor(tenValue / 16);

                divValue = (ulong)Math.Floor((decimal)(tenValue / 16));

                resValue = tenValue % 16;
                hex = tenValue2Char(resValue) + hex;
                tenValue = divValue;
            }
            while (tenValue >= 16);
            if (tenValue != 0)
                hex = tenValue2Char(tenValue) + hex;
            return hex;
        }

        public static string tenValue2Char(ulong ten)
        {
            switch (ten)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                    return ten.ToString();
                case 10:
                    return "A";
                case 11:
                    return "B";
                case 12:
                    return "C";
                case 13:
                    return "D";
                case 14:
                    return "E";
                case 15:
                    return "F";
                default:
                    return "";
            }
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            //  serialPort1.Close();
        }

        //寻找电脑连接的串口端口号：
        public void Search_Port(SerialPort myport, ComboBox mybox)
        {
            mybox.Items.Clear();
            mybox.Items.AddRange(SerialPort.GetPortNames());
            if (mybox.Items.Count > 0)
            {
                mybox.SelectedIndex = 0;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button6_Click_2(object sender, EventArgs e)
        {
            selectOff = dataGridView1.CurrentRow.Index;
            //if (selectOff == 3 || selectOff == 2)
            //{
            //    StartEliminate(selectOff);
            //    return;
            //}
            stopTime = Int32.Parse(dataGridView1.Rows[selectOff].Cells[1].Value.ToString());
            Thread t = new Thread(ShuRu);
            t.Start();
        }

        private void ShuRu()
        {
            int off = Int32.Parse(dataGridView1.Rows[selectOff].Cells[2].Value.ToString()) + 59;
            byte[] byte1 = CreateBtToStrTwo(off, stopTime);

            string str = "";
            string str1 = "";
            for (int i = 0; i < 8; i++)
            {
                str = Convert.ToString(byte1[i], 16).ToUpper();
                str1 = str1 + (str.Length == 1 ? "0" + str : str) + " ";
            }
            richTextBox1.AddText("成功发送指令：" + str1 + "\r");
            serial_send(byte1, 8);
            Thread.Sleep(100);
            ShuRuT();
        }
        private void ShuRuT()
        {
            Thread.Sleep(100);
            int off = Int32.Parse(dataGridView1.Rows[selectOff].Cells[2].Value.ToString()) + 63;
            int closeTime = Convert.ToInt32(dataGridView1.Rows[selectOff].Cells[3].Value.ToString());//闭合时间
            byte[] byte1 = CreateBtToStrTwo(off, closeTime);

            string str = "";
            string str1 = "";
            for (int i = 0; i < 8; i++)
            {
                str = Convert.ToString(byte1[i], 16).ToUpper();
                str1 = str1 + (str.Length == 1 ? "0" + str : str) + " ";
            }
            richTextBox1.AddText("成功发送指令：" + str1 + "\r");
            serial_send(byte1, 8);
        }

    }
}
