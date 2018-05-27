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
    public unsafe partial class FrmAdjust : Form
    {
        private bool Excute=false;                     //表示是否执行处理操作
        private FastBitmap bmp;
        private byte* DataCopy;
        private PictureBox canvas;
        private String method;
        public String Method
        {
            get { return method; }
            set { method = value;}
        }
        public FrmAdjust(FastBitmap fBmp,PictureBox mainCanvas)
        {
            InitializeComponent();
            bmp = fBmp;
            canvas = mainCanvas;
        }

        private void FrmAdjust_Load(object sender, EventArgs e)
        {                            
            if (bmp.Handle != IntPtr.Zero)//以下代码拷贝一份原始图像数据用来还原！
            {
                DataCopy = (byte*)Marshal.AllocHGlobal(bmp.Stride * bmp.Height);  //分配一块内存
                Win32Api.CopyMemory((IntPtr)DataCopy, (IntPtr)bmp.Pointer, bmp.Stride * bmp.Height);
            }
            UpdateImage();
        }

        private void UpdateImage()
        {
            if (bmp.Handle != IntPtr.Zero) //以下代码将原始图像数据用来进行图像！
            {
                Win32Api.CopyMemory((IntPtr)bmp.Pointer, (IntPtr)DataCopy, bmp.Stride *bmp.Height);
            }
            switch (method)
            {
                case "Bin":
                    AdjustEffect.Bin(bmp, (int)trbValue.Value);
                    break;
                case "Gama":
                    AdjustEffect.Gama(bmp, (int)trbValue.Value); 
                    break;
                case "FilmStyle":
                    AdjustEffect.FilmStyle(bmp, (int)trbValue.Value);
                    break;
            }
            Graphics G= canvas.CreateGraphics();
            IntPtr Hdc = G.GetHdc();
            bmp.DrawImage(Hdc, 0, 0, canvas.Width, canvas.Height, 0, 0, bmp.Width, bmp.Height);
            G.ReleaseHdc();
            G.Dispose();
            canvas.Invalidate(); 
        }

        private void trbValue_Scroll(object sender, EventArgs e)
        {
            UpdateImage();
        }

        private void trbValue_ValueChanged(object sender, EventArgs e)
        {
            UpdateImage();
        }

        private void FrmAdjust_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Excute==false) //以下代码将原始图像数据用来进行图像
            {
                Win32Api.CopyMemory((IntPtr)bmp.Pointer, (IntPtr)DataCopy, bmp.Stride * bmp.Height);
            }
            Marshal.FreeHGlobal((IntPtr)DataCopy);  //清空备份数据
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Excute= false;
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Excute = true;
            this.Close();
        }
    }
}
