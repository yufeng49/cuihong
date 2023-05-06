using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace MiGuoPacking
{
    public partial class LoadProcessBar : Form
    {

        private int count = -1;
        private ArrayList images = new ArrayList();
        public Bitmap[] bitmap = new Bitmap[8];
        private int _value = 1;
        private Color _circleColor = Color.Red;
        private float _circleSize = 0.8f;

        int width = 0;
        int height = 0;
        public LoadProcessBar()
        {
            InitializeComponent();
            width = 236;
            height = 194;
            //236 194 
            //this.ShowInTaskbar = false;
            //this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        }

        public Color CircleColor
        {
            get { return _circleColor; }
            set
            {
                _circleColor = value;
                Invalidate();
            }
        }

        public float CircleSize
        {
            get { return _circleSize; }
            set
            {
                if (value <= 0.0F)
                    _circleSize = 0.05F;
                else
                    _circleSize = value > 4.0F ? 4.0F : value;
                Invalidate();
            }
        }

        public Bitmap DrawCircle(int j)
        {
            const float angle = 360.0F / 8;
            Bitmap map = new Bitmap(150, 150);
            Graphics g = Graphics.FromImage(map);

            g.TranslateTransform(width / 3.0F, height / 3.0F);
            g.RotateTransform(angle * _value);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            int[] a = new int[8] { 25, 50, 75, 100, 125, 150, 175, 200 };
            for (int i = 1; i <= 8; i++)
            {
                int alpha = a[(i + j - 1) % 8];
                Color drawColor = Color.FromArgb(alpha, _circleColor);
                using (SolidBrush brush = new SolidBrush(drawColor))
                {
                    float sizeRate = 3.5F / _circleSize;
                    float size = width / (6 * sizeRate);

                    float diff = (width / 10.0F) - size;

                    float x = (width / 80.0F) + diff;
                    float y = (height / 80.0F) + diff;
                    g.FillEllipse(brush, x, y, size, size);
                    g.RotateTransform(angle);
                }
            }
            return map;
        }


        public void Draw()
        {
            for (int j = 0; j < 8; j++)
            {
                bitmap[7 - j] = DrawCircle(j);
            }
        }
        protected override void OnResize(EventArgs e)
        {
            SetNewSize();
            base.OnResize(e);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            SetNewSize();
            base.OnSizeChanged(e);
        }

        private void SetNewSize()
        {
            int size = Math.Max(width, height);
            Size = new Size(size, size);
        }

        public void set()
        {
            for (int i = 0; i < 8; i++)
            {
                Draw();

                Bitmap map = new Bitmap((bitmap[i]), new Size(120, 110));

                images.Add(map);
            }
            pictureBox1.Image = (Image)images[0];
            pictureBox1.Size = pictureBox1.Image.Size;

        }
        System.Timers.Timer timingSub = new System.Timers.Timer();
        private void LoadProcessBar_Load(object sender, EventArgs e)
        {
            timingSub.AutoReset = true;
            timingSub.Enabled = true;
            timingSub.Interval = 100;
            timingSub.Elapsed += timer1_Tick;
            timingSub.Start();

            this.TransparencyKey = Color.White; //窗体透明
            this.BackColor = Color.White;
            set();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            count = (count + 1) % 8;
            pictureBox1.Image = (Image)images[count];

        }

        public void CloseForm()
        {
            //利用委托进行窗体的操作，避免跨线程调用时抛异常
            if (this.InvokeRequired)
            {
                SetUISomeInfo UiInfo = new SetUISomeInfo(new Action(() =>
                {
                    if (!this.IsDisposed)
                    {
                        this.Close();
                    }
                }));
                this.Invoke(UiInfo);
            }
            else
            {
                this.Close();
            }

        }
        private delegate void SetUISomeInfo();
    }
}

