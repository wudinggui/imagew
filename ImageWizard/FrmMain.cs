using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Drawing.Imaging;
using ImageWizard.Effect;
using ImageWizard.dialog;

namespace ImageWizard
{
    public partial class FrmMain : Form
    {
        private   FastBitmap bmp =new FastBitmap() ;

        public FrmMain()
        {
            InitializeComponent();
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmMain());
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            Frm_Start frm = new Frm_Start();//实例化窗体对象
            frm.StartPosition = FormStartPosition.CenterScreen;//设置窗体居中显示
            frm.ShowDialog();//显示窗体
        }

  /*      protected override void OnPaint(PaintEventArgs e)
        {
            if (bmp != null)
            {
                if (bmp.Handle != IntPtr.Zero)
                {
                    IntPtr Hdc = e.Graphics.GetHdc();
                    bmp.DrawImage(Hdc,100, 50, this.Width, this.Height, -this.AutoScrollPosition.X, -this.AutoScrollPosition.Y,this.Width ,this.Height);
                    e.Graphics.ReleaseHdc();
                }
                else
                    base.OnPaint(e);
            }
            else
                base.OnPaint(e);
        }*/

      /*  protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (bmp != null)
            {
                if (bmp.Handle != IntPtr.Zero)
                {
                    IntPtr Hdc = e.Graphics.GetHdc();
                    bmp.DrawImage(Hdc,100, 50, this.Width, this.Height, -this.AutoScrollPosition.X, -this.AutoScrollPosition.Y, this.Width, this.Height);
                    e.Graphics.ReleaseHdc();
                }
                else
                    base.OnPaintBackground(e);
            }
            else
                base.OnPaintBackground(e);
        }*/

        private void OpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "Bitmap files (*.bmp)|*.bmp|Jpeg files (*.jpg)|*.jpg|Png files (*.png)|*.png|All valid files (*.bmp/*.jpg/*.png)|*.bmp;*.jpg;*.png";
            openFileDialog.FilterIndex = 4;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                bool Result = bmp.LoadImageFormFile(openFileDialog.FileName);
                this.AutoScroll = true;
                this.AutoScrollMinSize = new Size(bmp.Width, bmp.Height);
                this.Canvas.Width = bmp.Width;
                this.Canvas.Height = bmp.Height;
                this.Canvas.Invalidate();
            }
        }

        private void EffectInvert_Click(object sender, EventArgs e)
        {
            Stopwatch Sw = new Stopwatch();
            Sw.Start();
            AdjustEffect.Invert(bmp);
            Sw.Stop();
            this.Text = Sw.ElapsedMilliseconds.ToString();
            this.Canvas.Invalidate();
        }

        private void DesatulateEffect_Click(object sender, EventArgs e)
        {
            Stopwatch Sw = new Stopwatch();
            Sw.Start();
            AdjustEffect.Desaturate(bmp);
            Sw.Stop();
            this.Text = Sw.ElapsedMilliseconds.ToString();
            this.Canvas.Invalidate();
        }

        private void ZoomBlurEffect_Click(object sender, EventArgs e)
        {
            Stopwatch Sw = new Stopwatch();
            Sw.Start();
            BlurEffect.ZoomBlur(bmp);
            Sw.Stop();
            this.Text = Sw.ElapsedMilliseconds.ToString();
            this.Canvas.Invalidate();
        }

        private void CopyMenu_Click(object sender, EventArgs e)
        {
            bmp.CopyToClipBoard(true);
        }

        private void PasteMenu_Click(object sender, EventArgs e)
        {
            bmp.Dispose();
            bmp.LoadPictureFromClipBoard();
            this.AutoScroll = true;
            this.AutoScrollMinSize = new Size(bmp.Width, bmp.Height);
            this.Canvas.Invalidate();
        }

        private void UsedAmount_Click(object sender, EventArgs e)
        {
            int Amount;
            Stopwatch Sw = new Stopwatch();
            Sw.Start();
            Amount = bmp.GetImageUesdColorAmount();
            Sw.Stop();
            this.Text = Sw.ElapsedMilliseconds.ToString() + "毫秒，像素数量"  + Amount.ToString();
        }

        private void FlipVerticalMenu_Click(object sender, EventArgs e)
        {
            Stopwatch Sw = new Stopwatch();
            Sw.Start();
            bmp.FlipVertical();
            Sw.Stop();
            this.Text = Sw.ElapsedMilliseconds.ToString();
            this.Canvas.Invalidate();
        }

        private void FlipHorizontalMenu_Click(object sender, EventArgs e)
        {
            Stopwatch Sw = new Stopwatch();
            Sw.Start();
            bmp.FlipHorizontal();
            Sw.Stop();
            this.Text = Sw.ElapsedMilliseconds.ToString();
            this.Canvas.Invalidate();
        }

        private void binMenuItem_Click(object sender, EventArgs e)
        {
            if (bmp.Handle != IntPtr.Zero)
            {
                FrmAdjust frmadjust = new FrmAdjust(bmp, Canvas);
                frmadjust.Method = "Bin";
                frmadjust.ShowDialog();
            }
            this.Canvas.Invalidate();
        }

        private void boxBlurStripMenuItem_Click(object sender, EventArgs e)
        {
            if (bmp.Handle != IntPtr.Zero)
            {
                FrmFilter frmFilter = new FrmFilter(bmp,Canvas);
                frmFilter.Filters = FrmFilter.FiltersMethod.BoxBlur;
                frmFilter.ShowDialog();
            }
            this.Canvas.Invalidate();
        }

        private void hisgMenuItem_Click(object sender, EventArgs e)
        {
            if (bmp.Handle != IntPtr.Zero)
            {
                FrmHisgram frmhisgram = new FrmHisgram(bmp);
                frmhisgram.ShowDialog();
            }
        }

        private void Canvas_Paint(object sender, PaintEventArgs e)
        {
            if (bmp != null)
            {
                if (bmp.Handle != IntPtr.Zero)
                {
                    IntPtr Hdc = e.Graphics.GetHdc();
                    bmp.DrawImage(Hdc, 0, 0, Canvas.Width, Canvas.Height, 0, 0, bmp.Width, bmp.Height);
                    e.Graphics.ReleaseHdc();
                }
            }
        }

        private void mosaicItem_Click(object sender, EventArgs e)
        {
            if (bmp.Handle != IntPtr.Zero)
            {
                FrmFilter frmFilter = new FrmFilter(bmp, Canvas);
                frmFilter.Filters = FrmFilter.FiltersMethod.Mosaic;
                frmFilter.ShowDialog();
            }
            this.Canvas.Invalidate();
        }

        private void gamaMenuItem_Click(object sender, EventArgs e)
        {
            if (bmp.Handle != IntPtr.Zero)
            {
                FrmAdjust frmadjust = new FrmAdjust(bmp, Canvas);
                frmadjust.Method = "Gama";
                frmadjust.ShowDialog();
            }
            this.Canvas.Invalidate();
        }

        private void oldStyleMenuItem_Click(object sender, EventArgs e)
        {
            Stopwatch Sw = new Stopwatch();
            Sw.Start();
            AdjustEffect.OldStyle(bmp);
            Sw.Stop();
            this.Text = Sw.ElapsedMilliseconds.ToString();
            this.Canvas.Invalidate();
        }

        private void FilmStyleMenuItem_Click(object sender, EventArgs e)
        {
            if (bmp.Handle != IntPtr.Zero)
            {
                FrmAdjust frmadjust = new FrmAdjust(bmp, Canvas);
                frmadjust.Method = "FilmStyle";
                frmadjust.ShowDialog();
            }
            this.Canvas.Invalidate();
        }

        private void crossPhotoMenuItem_Click(object sender, EventArgs e)
        {
            Stopwatch Sw = new Stopwatch();
            Sw.Start();
            AdjustEffect.CrossPhoto(bmp);
            Sw.Stop();
            this.Text = Sw.ElapsedMilliseconds.ToString();
            this.Canvas.Invalidate();
        }

        private void lomoItem_Click(object sender, EventArgs e)
        {
            Stopwatch Sw = new Stopwatch();
            Sw.Start();
            AdjustEffect.LomoStyle(bmp);
            Sw.Stop();
            this.Text = Sw.ElapsedMilliseconds.ToString();
            this.Canvas.Invalidate();
        }
    }
}
