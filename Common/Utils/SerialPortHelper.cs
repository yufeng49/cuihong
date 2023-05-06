using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Common.Utils
{
    public static class SerialPortHelper
    {
        /// <summary>
        /// 寻找电脑连接的串口端口号
        /// </summary>
        /// <param name="myport"></param>
        /// <param name="mybox"></param>
        public static void Search_Port(SerialPort myport, ComboBox mybox)
        {
            mybox.Items.Clear();
            mybox.Items.AddRange(SerialPort.GetPortNames());
            if (mybox.Items.Count > 0)
            {
                mybox.SelectedIndex = 0;
            }
        }

        public static void Close(SerialPort port)
        {
            port.Close();
        }

        public static (bool, string) Open(SerialPort port, string com, string baud)
        {
            try
            {
                if (port.IsOpen)
                {
                    port.Close();
                    port.PortName = com;
                    port.Parity = Parity.None;
                    port.BaudRate = Convert.ToInt32(baud);
                    port.DataBits = 8;
                    port.StopBits = StopBits.One;
                    port.Open();
                }
                else
                {
                    port.PortName = com;
                    port.Parity = Parity.None;
                    port.BaudRate = Convert.ToInt32(baud);
                    port.DataBits = 8;
                    port.StopBits = StopBits.One;
                    port.Open();
                }
                var arr = HerartPackets(port);
                if (!arr.Item1)
                {
                    return arr;
                }
                return (true, "串口已打开");
            }
            catch (Exception e)
            {
                return (false, e.Message);
            }

        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="serialPort1"></param>
        /// <param name="Buffer"></param>
        /// <param name="leng"></param>
        /// <returns></returns>
        public static string SendNews(SerialPort serialPort1, byte[] Buffer, int leng)
        {
            if (serialPort1.IsOpen)
            {
                try
                {
                    serialPort1.Write(Buffer, 0, leng);    //写数据
                    return "发送成功";
                }
                catch (Exception e)
                {
                    //MessageBox.Show("串口数据发送出错，请检查.", "About");//错误处理
                    return e.Message;
                }
            }
            else
            {
                return "注意：\n端口未打开，请检查.";
            }
        }

        public static (bool, string) HerartPackets(SerialPort serialPort)
        {
            var bt = SerialPortHelper.StringToByte("01020000001079C6");
            string mes = SerialPortHelper.SendNews(serialPort, bt, bt.Length);
            Thread.Sleep(100);
            var sr = SerialPortHelper.ReceiveNews(serialPort);
            if (sr.Item1)
            {
                return (true, "串口打开成功");
            }
            else
            {
                return (false, "串口打开失败");
            }
        }


        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="serialPort1"></param>
        /// <returns></returns>
        public static (bool, string) ReceiveNews(SerialPort serialPort1)
        {
            if (serialPort1.IsOpen)
            {
                try
                {
                    byte[] ReDatas = new byte[serialPort1.BytesToRead];
                    //从串口读取数据
                    int bytecount = serialPort1.Read(ReDatas, 0, ReDatas.Length);    //写数据
                    string str = HexStringToASCII(AddData(ReDatas));
                    return (true, str);
                }
                catch (Exception e)
                {
                    //MessageBox.Show("串口数据发送出错，请检查.", "About");//错误处理
                    return (false, e.Message);
                }
            }
            else
            {
                return (false, "注意：\n端口未打开，请检查.");
            }

        }

        /// <summary>
        /// 数组转字符串
        /// </summary>
        /// <param name="bt"></param>
        /// <returns></returns>
        public static string ByteToString(byte[] bt)
        {
            string str = "";
            for (int i = 0; i < bt.Length / 2; i++)
            {
                // str = Convert.ToString(bt[i], 16).ToUpper();
                str += Convert.ToString(bt[i], 16).ToUpper().Length == 1 ? "0" + Convert.ToString(bt[i], 16).ToUpper() : Convert.ToString(bt[i], 16).ToUpper();
            }
            return str.Trim();
        }

        /// <summary>
        /// 字符串转数组
        /// </summary>
        /// <param name="byteStr"></param>
        /// <returns></returns>
        public static byte[] StringToByte(string byteStr)
        {
            if (byteStr.Length % 2 != 0)
            {
                return null;
            }
            byte[] bt = new byte[byteStr.Length / 2];
            for (int i = 0; i < byteStr.Length / 2; i++)
            {
                bt[i] = Convert.ToByte(byteStr.Substring(i * 2, 2), 16);
            }
            return bt;
        }
        /// <summary>
        /// 解码过程
        /// </summary>
        /// <param name="data">串口通信的数据编码方式因串口而异，需要查询串口相关信息以获取</param>
        public static string AddData(byte[] data)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sb.AppendFormat("{0:x2}" + " ", data[i]);
            }
            return sb.ToString();
        }

        public static string HexStringToASCII(string hexstring)
        {
            byte[] bt = HexStringToBinary(hexstring);
            string lin = "";
            for (int i = 0; i < bt.Length; i++)
            {
                lin = lin + bt[i] + " ";
            }

            string[] ss = lin.Trim().Split(new char[] { ' ' });
            char[] c = new char[ss.Length];
            int a;
            for (int i = 0; i < c.Length; i++)
            {
                a = Convert.ToInt32(ss[i]);
                c[i] = Convert.ToChar(a);
            }

            string b = new string(c);
            return b;
        }

        /// <summary>
        /// 16进制字符串转换为二进制数组
        /// </summary>
        /// <param name="hexstring">用空格切割字符串</param>
        /// <returns>返回一个二进制字符串</returns>
        private static byte[] HexStringToBinary(string hexstring)
        {

            string[] tmpary = hexstring.Trim().Split(' ');
            byte[] buff = new byte[tmpary.Length];
            for (int i = 0; i < buff.Length; i++)
            {
                buff[i] = Convert.ToByte(tmpary[i], 16);
            }
            return buff;
        }

        /// <summary>
        /// CRC码
        /// </summary>
        /// <param name="data"></param>
        /// <param name="leng"></param>
        /// <returns></returns>
        public static int modbus_crc(byte[] data, byte leng)
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

    }
}
