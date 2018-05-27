using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ImageWizard.FastbmpInfo;

namespace ImageWizard.dialog
{
    public partial class FrmHisgram : Form
    {
        private  FastBitmap bmp;
        private  BmpInfo bmpInfo;
        public FrmHisgram(FastBitmap fBmp)
        {
            InitializeComponent();
            bmp = fBmp;
            bmpInfo = new BmpInfo();

        }

        private void FrmHisgram_Load(object sender, EventArgs e)
        {
            bmpInfo.GetHisgram(bmp);
            DrewHisgram();
        }

        private void cmbChannal_SelectedIndexChanged(object sender, EventArgs e)
        {
            DrewHisgram();
        }

        private  void DrewHisgram()
        {
            double[] Properbility = new double[256];
            Color color = new Color();
            double maxColorProperbility;
            string ColorMode =(string) cmbChannal.Text;
            switch (ColorMode)
            {
                case "Red":
                    /*...*/
                    color = Color.Red;
                    Properbility = this.CountProbability(bmpInfo.Red);
                    maxColorProperbility = this.MaxProbability(bmpInfo.Red);
                    break;
                case "Green":
                    /*...*/
                    color = Color.Green;
                    Properbility = this.CountProbability(bmpInfo.Green);
                    maxColorProperbility = this.MaxProbability(bmpInfo.Green);
                    break;
                case "Blue":
                    /*...*/
                    color = Color.Blue;
                    Properbility = this.CountProbability(bmpInfo.Blue);
                    maxColorProperbility = this.MaxProbability(bmpInfo.Blue);
                    break;
                default :
                    return ;
            }
            Pen pen = new Pen(color, 1);
            Bitmap hisgBmp=new Bitmap (256,256);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage((Image)hisgBmp);
            int y;
            for (int i = 0; i < 256; i++)
            {               
                double percent = Properbility[i] / maxColorProperbility;  // 当前灰度级与最大灰度级的比率
                y = (int)(256 * (1 - percent));
                g.DrawLine(pen, i, y, i, 256);
            } 
            g.Save();
            g.Dispose();
            picHisg.Image = hisgBmp;
            return ;

        }

        private double[] CountProbability(int[] colorPixel)  //计算每种颜色各个灰度级的概率密度
        {
            double total = bmpInfo.PixelCount;
            double[] probability = new double[256];
            for (int i = 0; i < 256; i++)                   // 计算各灰度级概率密度
            {
                probability[i] = colorPixel[i] / total;
            } 
            return probability;
        }
        private double MaxProbability(int[] colorPixel)       //计算每种颜色某一灰度级最大的概率密度
        {
            int max = colorPixel[0];
            int maxIndex = 0;
            double total = bmpInfo.PixelCount;
            for (int i = 1; i < colorPixel.Length ; i++)
            {
                int temp = colorPixel[i];
                if (temp > max)
                {
                    max = temp;
                    maxIndex = i;
                }
            }
            double properbility=max / total;
            return properbility;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
