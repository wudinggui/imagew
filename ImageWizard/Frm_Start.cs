using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ImageWizard
{
    public partial class Frm_Start : Form
    {
        private Bitmap splashBmp;
        private int count;
        public Frm_Start()
        {
            InitializeComponent();
        }

        private void Frm_Start_Load(object sender, EventArgs e)
        {
            splashBmp = new Bitmap("adobe_photoshop.png");
            splashBmp.MakeTransparent(Color.Blue);
            this.Opacity = 0.4;
            this.timer1.Start();//启动计时器
            this.timer1.Interval = 100;//设置启动窗体停留时间
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.DrawImage((Image)splashBmp, new Point(0, 0));
        }


        private void Frm_Start_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.timer1.Stop();//关闭计时器
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Opacity += 0.15;
            count += 1;
            if (count == 15)
            {
                this.timer1.Stop();
                this.Close();
            }
        }
    }
}
