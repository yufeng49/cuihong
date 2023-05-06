using CCWin.Win32.Const;
using System.Runtime.InteropServices;

namespace MiGuoPacking.Model
{
    public class PrintLab
    {

        [DllImport("CDFPSK.dll")]
        public static extern int PTK_OpenLogMode(LPSTR filePath);
        [DllImport("CDFPSK.dll")]
        public static extern int PTK_GetErrorInfo(int error_n, byte[] errorInfo, uint infoSize);
        [DllImport("CDFPSK.dll")]
        public static extern int PTK_SendCmd(string data, uint datalen);
        [DllImport("CDFPSK.dll")]
        public static extern int PTK_OpenUSBPort(uint port);
        [DllImport("CDFPSK.dll")]
        public static extern int PTK_CloseUSBPort();
        [DllImport("CDFPSK.dll")]
        public static extern int PTK_OpenSerialPort(uint port, uint bRate);
        [DllImport("CDFPSK.dll")]
        public static extern int PTK_CloseSerialPort();
        [DllImport("CDFPSK.dll")]
        public static extern int PTK_Connect_Timer(string IPAddr, uint netPort, uint time_sec);
        [DllImport("CDFPSK.dll")]
        public static extern int PTK_CloseConnect();
        [DllImport("CDFPSK.dll")]
        public static extern int PTK_PrintConfiguration();
        [DllImport("CDFPSK.dll")]
        public static extern int PTK_ClearBuffer();
        [DllImport("CDFPSK.dll")]
        public static extern int PTK_SetPrintSpeed(uint speed);
        [DllImport("CDFPSK.dll")]
        public static extern int PTK_SetDarkness(uint dark);
        [DllImport("CDFPSK.dll")]
        public static extern int PTK_SetDirection(char direct);
        [DllImport("CDFPSK.dll")]
        public static extern int PTK_SetLabelHeight(uint lheight, uint gapH, int gapOffset, bool bFlag);
        [DllImport("CDFPSK.dll")]
        public static extern int PTK_SetLabelWidth(uint lwidth);
        [DllImport("CDFPSK.dll")]
        public static extern int PTK_PrintLabel(uint number, uint cpnumber);
        [DllImport("CDFPSK.dll")]
        public static extern int PTK_MediaDetect();
        [DllImport("CDFPSK.dll")]
        public static extern int PTK_DrawText(uint px, uint py, uint pdirec, uint pFont,
        uint pHorizontal, uint pVertical, char pColor, string pstr);
        [DllImport("CDFPSK.dll")]
        public static extern int PTK_DrawTextEx(uint px, uint py, uint pdirec, uint pFont,
        uint pHorizontal, uint pVertical, char pColor, string pstr, bool Varible);
        [DllImport("CDFPSK.dll")]
        public static extern int PTK_DrawText_TrueType(uint x, uint y, uint FHeight, uint FWidth,
        string FType, uint Fspin, uint FWeight, bool FItalic,
        bool FUnline, bool FStrikeOut, string data);
        [DllImport("CDFPSK.dll")]
        public static extern int PTK_DrawText_TrueTypeEx(uint x, uint y, uint FHeight, uint FWidth,
            string FType, uint Fspin, uint FWeight, bool FItalic, bool FUnline, bool FStrikeOut,
            uint lineMaxWidth, uint lineMaxNum, int lineGapH, bool middleSwitch, string data);

        [DllImport("CDFPSK.dll")]
        public static extern int PTK_RenameDownloadFont(uint StoreType, char Fontname, string DownloadFontName);

        [DllImport("CDFPSK.dll")]
        public static extern int PTK_AnyGraphicsPrint(uint px, uint py, string pcxname, string filePath,
        float ratio, uint width, uint height, uint iDire);

        [DllImport("CDFPSK.dll")]
        public static extern int PTK_DrawBinGraphics(uint px, uint py, uint pbyte, uint pH, byte[] Gdata);
        [DllImport("CDFPSK.dll")]
        public static extern int PTK_DrawRectangle(uint px, uint py, uint thickness, uint pEx, uint pEy);
        [DllImport("CDFPSK.dll")]
        public static extern int PTK_DrawLineXor(uint px, uint py, uint pL, uint pH);
        [DllImport("CDFPSK.dll")]
        public static extern int PTK_DrawLineOr(uint px, uint py, uint pL, uint pH);
        [DllImport("CDFPSK.dll")]
        public static extern int PTK_DrawDiagonal(uint px, uint py, uint thickness, uint pEx, uint pEy);
        [DllImport("CDFPSK.dll")]
        public static extern int PTK_DrawWhiteLine(uint px, uint py, uint pL, uint pH);
        [DllImport("CDFPSK.dll")]
        public static extern int PTK_DrawBar2D_QR(uint x, uint y, uint w, uint v, uint o, uint r, uint m, uint g, uint s, string pstr);
        [DllImport("CDFPSK.dll")]
        public static extern int PTK_DrawBar2D_QR(uint x, uint y, uint w, uint v, uint o, uint r, uint m, uint g, uint s, byte[] pstr);
        [DllImport("CDFPSK.dll")]
        public static extern int PTK_DrawBar2D_HANXIN(uint x, uint y, uint w, uint v, uint o, uint r, uint m, uint g, uint s, string pstr);
        [DllImport("CDFPSK.dll")]
        public static extern int PTK_DrawBar2D_HANXIN(uint x, uint y, uint w, uint v, uint o, uint r, uint m, uint g, uint s, byte[] pstr);
        [DllImport("CDFPSK.dll")]
        public static extern int PTK_DrawBar2D_Pdf417(uint x, uint y, uint w, uint v, uint s, uint c, uint px, uint py, uint r, uint l, uint t, uint o, string pstr);
        [DllImport("CDFPSK.dll")]
        public static extern int PTK_DrawBar2D_Pdf417(uint x, uint y, uint w, uint v, uint s, uint c, uint px, uint py, uint r, uint l, uint t, uint o, byte[] pstr);
        [DllImport("CDFPSK.dll")]
        public static extern int PTK_DrawBar2D_MaxiCode(uint x, uint y, uint m, uint u, string pstr);
        [DllImport("CDFPSK.dll")]
        public static extern int PTK_DrawBar2D_MaxiCode(uint x, uint y, uint m, uint u, byte[] pstr);
        [DllImport("CDFPSK.dll")]
        public static extern int PTK_DrawBar2D_DATAMATRIX(uint x, uint y, uint w, uint v, uint o, uint m, string pstr);
        [DllImport("CDFPSK.dll")]
        public static extern int PTK_DrawBar2D_DATAMATRIX(uint x, uint y, uint w, uint v, uint o, uint m, byte[] pstr);
        [DllImport("CDFPSK.dll")]
        public static extern int PTK_DrawBarcode(uint px, uint py, uint pdirec, string pCode, uint NarrowWidth, uint pHorizontal, uint pVertical, char ptext, string pstr);
        [DllImport("CDFPSK.dll")]
        public static extern int PTK_DrawBarcode(uint px, uint py, uint pdirec, string pCode, uint NarrowWidth, uint pHorizontal, uint pVertical, char ptext, byte[] pstr);

        [DllImport("CDFPSK.dll")]
        public static extern int PTK_DrawBarcodeEx(uint px, uint py, uint pdirec, string pCode, uint NarrowWidth, uint pHorizontal, uint pVertical, char ptext, string pstr, bool Varible);
        [DllImport("CDFPSK.dll")]
        public static extern int PTK_DrawBarcodeEx(uint px, uint py, uint pdirec, string pCode, uint NarrowWidth, uint pHorizontal, uint pVertical, char ptext, byte[] pstr, bool Varible);
        [DllImport("CDFPSK.dll")]
        public static extern int PTK_RFIDCalibrate();
        [DllImport("CDFPSK.dll")]
        public static extern int PTK_RWRFIDLabel(uint nRWMode, uint nWForm, uint nStartBlock, uint nWDataNum, uint nWArea, string pstr);
        [DllImport("CDFPSK.dll")]
        public static extern int PTK_SetRFID(uint nReservationParameters, uint nReadWriteLocation, uint ReadWriteArea, uint nMaxErrNum, uint nErrProcessingMethod);
        [DllImport("CDFPSK.dll")]
        public static extern int PTK_ReadRFIDLabelData(uint nDataBlock, uint nRFPower, uint bFeed, byte[] data, uint dataSize);
        [DllImport("CDFPSK.dll")]
        public static extern int PTK_SetHFRFID(char pWForm, uint nProtocolType, uint nMaxErrNumd);
        [DllImport("CDFPSK.dll")]
        public static extern int PTK_RWHFLabel(char nRWMode, uint nStartBlock, uint nBlockNum, string pstr, bool Varible);
        [DllImport("CDFPSK.dll")]
        public static extern int PTK_ReadHFLabelData(uint nStartBlock, uint nBlockNum, char pFeed, byte[] data, uint dataSize);
        [DllImport("CDFPSK.dll")]
        public static extern int PTK_ReadHFLabeUID(char pFeed, byte[] data, uint dataSize);
        [DllImport("CDFPSK.dll")]
        public static extern int PTK_GetUtilityInfo(uint infoNum, byte[] data, uint dataSize);
        [DllImport("CDFPSK.dll")]
        public static extern int PTK_SetUtilityInfoProc(byte[] _G1Info, uint infoNum, string info);
        [DllImport("CDFPSK.dll")]
        public static extern int PTK_SetUtilityInfo(byte[] _G1Info);
        [DllImport("CDFPSK.dll")]
        public static extern int PTK_EnableFLASH();
        [DllImport("CDFPSK.dll")]
        public static extern int PTK_DisableFLASH();
        [DllImport("CDFPSK.dll")]
        public static extern int PTK_FormDel(string pid);
        [DllImport("CDFPSK.dll")]
        public static extern int PTK_FormDownload(string pid);
        [DllImport("CDFPSK.dll")]
        public static extern int PTK_FormEnd();
        [DllImport("CDFPSK.dll")]
        public static extern int PTK_ExecForm(string pid);
        [DllImport("CDFPSK.dll")]
        public static extern int PTK_DefineVariable(uint id, uint maxNum, char ptext, string hintMsg);
        [DllImport("CDFPSK.dll")]
        public static extern int PTK_DefineCounter(uint id, uint maxNum, char ptext, string prule, string hintMsg);
        [DllImport("CDFPSK.dll")]
        public static extern int PTK_Download();
        [DllImport("CDFPSK.dll")]
        public static extern int PTK_DownloadInitVar(string pstr);
    }
}
