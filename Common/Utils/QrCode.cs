using Helper;
using System;
using System.Drawing;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;

namespace Common.Utils
{
    public class QrCode
    {
        /// <summary>
        /// 二维码方法
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        public static Bitmap CreateQRCode(string asset, int width, int height)
        {
            EncodingOptions options = new QrCodeEncodingOptions
            {
                DisableECI = true,
                CharacterSet = "UTF-8", //编码
                Width = width,             //宽度
                Height = height,             //高度
                Margin = 0
            };
            BarcodeWriter writer = new BarcodeWriter();
            writer.Format = BarcodeFormat.QR_CODE;
            writer.Options = options;
            return writer.Write(asset);

        }

        public static Bitmap CreateBarCode(string asset, int width, int height)
        {
            EncodingOptions options = new QrCodeEncodingOptions
            {
                DisableECI = true,
                CharacterSet = "UTF-8", //编码
                Width = width,             //宽度
                Height = height,             //高度
                Margin = 0
            };
            BarcodeWriter writer = new BarcodeWriter();
            writer.Format = BarcodeFormat.CODE_128;
            writer.Options = options;
            return writer.Write(asset);

        }
    }


    public class SendVoice
    {

        public static SpeechHelper CreateSp(string ip, int port)
        {
            SpeechHelper sp = new SpeechHelper(ip, port);
            try
            {
                sp.Connect();
            }
            catch (Exception ex)
            {
                sp = null;
            }
            return sp;
        }
        public static bool SendVoiceMsg(SpeechHelper sp, string content)
        {
            return sp.TTS(content);
        }

    }
}
