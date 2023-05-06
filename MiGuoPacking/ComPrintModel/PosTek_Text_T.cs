namespace MiGuoPacking.Model
{
    public struct PosTek_Text_T
    {
        public uint x;
        public uint y;
        public FontType_E fontType;
        public char charSet;
        public uint dotFont;
        public uint rotate;
        public uint zoom;
        public uint lenth;
        public uint width;
        public uint thickness;
        public uint lineGap;
        public bool isVertical;
        public bool isCustomLW;
        public char color;
        public string TrueTypeFont;
        public string str;
    }

    public enum FontType_E
    {
        inline = 0,
        microsoft = 1,
        custom = 2
    }

    public enum Bar2DType_E
    {
        QR = 0,
        HANXIN = 1,
        PDF417 = 2,
        MAXICODE = 3,
        DATAMATRIX = 4
    }

    public struct Bar2D_QR
    {
        public uint x;
        public uint y;
        public uint version;
        public uint rotate;
        public uint zoom;
        public uint errorCorrectionLevel;
        public uint maskGraphics;
        public string str;
    }
}
