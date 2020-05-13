using MorphxLibs;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Phyllotaxis {
    public partial class FormMain : Form {
        private const double ToRad = Math.PI / 180.0;

        private Graphics g;
        private Bitmap bmp;
        private double w2;
        private double h2;
        private readonly HLSRGB color = new HLSRGB(0.5, 0.5, 1.0);

        private double n = 0;
        private const double c = 6.0;
        private const double baseAngle = 137.5 * ToRad;

        public FormMain() {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.UserPaint |
                          ControlStyles.OptimizedDoubleBuffer, true);

            CreateBitmap();

            Task.Run(() => {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                while(true) {
                    Render(g, 4);

                    Thread.Sleep(30);
                    this.Invalidate();
                }

            });

            this.SizeChanged += (_, __) => CreateBitmap();
        }

        private void CreateBitmap() {
            g?.Dispose();
            bmp?.Dispose();

            bmp = new Bitmap(this.DisplayRectangle.Width, this.DisplayRectangle.Height);
            g = Graphics.FromImage(bmp);
            w2 = bmp.Width / 2;
            h2 = bmp.Height / 2;

            n = 0;
        }

        private void Render(Graphics g, int frames = 1) {
            while(frames-- > 0) { // http://algorithmicbotany.org/papers/abop/abop-ch4.pdf
                double a = n * baseAngle;
                double r = c * Math.Sqrt(n);

                float x = (float)(w2 + r * Math.Cos(a));
                float y = (float)(h2 - r * Math.Sin(a));

                //color.Hue = n;
                color.Hue = a;
                //color.Hue = r;
                using(SolidBrush sb = new SolidBrush(color.Color))
                    g.FillEllipse(sb, x, y, (float)c, (float)c);

                n += 1.0;
            }
        }

        protected override void OnPaint(PaintEventArgs e) {
            e.Graphics.DrawImageUnscaled(bmp, 0, 0);
        }
    }
}