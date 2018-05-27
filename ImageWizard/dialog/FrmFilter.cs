using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ImageWizard.Effect;
using ImageWizard;
using System.Runtime.InteropServices;

namespace ImageWizard.dialog
{ 
    public unsafe partial class FrmFilter : Form
    {
        public enum FiltersMethod 
        {
            None,
            BoxBlur,
            Mosaic
        }
        private FiltersMethod filters = FiltersMethod.None;
        public FiltersMethod Filters
        {
            get
            {
                return filters;
            }
            set
            {
                filters = value;
            }
        }

        private bool Excute = false;   //表示是否执行处理操作
        private FastBitmap bmp;
        private byte* DataCopy;
        private PictureBox canvas;
        int DestX, DestY;             //预览窗口起始坐标和长宽
        int ViewWidth, ViewHeight;    

        public FrmFilter(FastBitmap fBmp,PictureBox mainCanvas)
        {
            InitializeComponent();
            bmp = fBmp;
            canvas = mainCanvas;
            DestX = 0; DestY = 0;
            ViewWidth = bmp.Width;
            ViewHeight = bmp.Height;
        }

        private void UpdateImage()
        {
            if (bmp.Handle != IntPtr.Zero) //以下代码将原始图像数据用来进行图像！
            {
                Win32Api.CopyMemory((IntPtr)bmp.Pointer, (IntPtr)DataCopy, bmp.Stride * bmp.Height);
            }
            switch (filters)
            {
                case FiltersMethod.BoxBlur:                  
                    BlurEffect.BoxBlur(bmp, (int)trbValue.Value);
                    break;
                case FiltersMethod.Mosaic:
                    BlurEffect.Mosaic(bmp, (int)trbValue.Value);
                    break;
            }
            Graphics G = picPreview.CreateGraphics();
            IntPtr Hdc = G.GetHdc();
            bmp.DrawImage(Hdc, DestX, DestY , ViewWidth, ViewHeight, 0, 0, bmp.Width, bmp.Height);
            G.ReleaseHdc();
            G.Dispose();
            picPreview.Invalidate();

            Graphics Gcanvas = canvas.CreateGraphics();
            IntPtr canvasHdc = Gcanvas.GetHdc();
            bmp.DrawImage(canvasHdc, 0, 0, canvas.Width, canvas.Height, 0, 0, bmp.Width, bmp.Height);
            Gcanvas.ReleaseHdc();
            Gcanvas.Dispose();
            canvas.Invalidate(); 
        }

        private void FrmFilter_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Excute == false) 
            {
                Win32Api.CopyMemory((IntPtr)bmp.Pointer, (IntPtr)DataCopy, bmp.Stride * bmp.Height);
            }
            Marshal.FreeHGlobal((IntPtr)DataCopy);  //清空备份数据
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Excute = true;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Excute = false;
            this.Close();
        }

        private void trbValue_Scroll(object sender, EventArgs e)
        {
            UpdateImage();
        }

        private void trbValue_ValueChanged(object sender, EventArgs e)
        {
            UpdateImage();
        }

        private void FrmFilter_Load(object sender, EventArgs e)
        {
            if (bmp.Handle != IntPtr.Zero)//以下代码拷贝一份原始图像数据用来还原！
            {
                DataCopy = (byte*)Marshal.AllocHGlobal(bmp.Stride * bmp.Height);  //分配一块内存
                Win32Api.CopyMemory((IntPtr)DataCopy, (IntPtr)bmp.Pointer, bmp.Stride * bmp.Height);
            }
            GetViewSize(DestX, DestY, ViewWidth, ViewHeight);
            UpdateImage();
        }

        private void picPreview_Paint(object sender, PaintEventArgs e)
        {
            if (bmp.Handle != IntPtr.Zero)
            {
                IntPtr Hdc = e.Graphics.GetHdc();
                bmp.DrawImage(Hdc, DestX, DestY, ViewWidth, ViewHeight, 0, 0, bmp.Width, bmp.Height);
                e.Graphics.ReleaseHdc();
            }

          }

        private void GetViewSize(int destX, int destY, int viewWidth, int viewHeight)//根据图像宽高得到最佳输出picturebox的宽高     
        {

            if ((viewWidth > picPreview.Width) || viewHeight > picPreview.Height)
            {
                double ratioW = (double)picPreview.Width / viewWidth;
                double ratioH = (double)picPreview.Height / viewHeight;
                if (ratioW >ratioH)
                {
                    viewHeight = picPreview.Height;
                    viewWidth =(int)(ratioH *viewWidth);
                }
                else 
                {
                    viewWidth = picPreview.Width;
                    viewHeight =(int)(ratioW *viewHeight);
                }
            }
            destX = (int)(picPreview.Width - viewWidth) / 2;
            destY = (int)(picPreview.Height - viewHeight) / 2;
               //return ;
            DestX = destX; DestY = destY; ViewWidth = viewWidth; ViewHeight = viewHeight;

        }
         
    }
}
