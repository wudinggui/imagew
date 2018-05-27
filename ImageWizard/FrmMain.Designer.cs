namespace ImageWizard
{
    partial class FrmMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
                bmp.Dispose(); 
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.File = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenFile = new System.Windows.Forms.ToolStripMenuItem();
            this.Filter = new System.Windows.Forms.ToolStripMenuItem();
            this.EffectInvert = new System.Windows.Forms.ToolStripMenuItem();
            this.DesatulateEffect = new System.Windows.Forms.ToolStripMenuItem();
            this.oldStyleMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ZoomBlurEffect = new System.Windows.Forms.ToolStripMenuItem();
            this.binMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.boxBlurStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mosaicItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gamaMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FilmStyleMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.编辑ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CopyMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.PasteMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.FlipHoriza = new System.Windows.Forms.ToolStripMenuItem();
            this.FlipVerticalMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.FlipHorizontalMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.信息ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.UsedAmount = new System.Windows.Forms.ToolStripMenuItem();
            this.hisgMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Canvas = new System.Windows.Forms.PictureBox();
            this.crossPhotoMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lomoItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Canvas)).BeginInit();
            this.SuspendLayout();
            // 
            // File
            // 
            this.File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenFile});
            this.File.Name = "File";
            this.File.Size = new System.Drawing.Size(59, 20);
            this.File.Text = "文件(&F)";
            // 
            // OpenFile
            // 
            this.OpenFile.Name = "OpenFile";
            this.OpenFile.Size = new System.Drawing.Size(112, 22);
            this.OpenFile.Text = "打开(&O)";
            this.OpenFile.Click += new System.EventHandler(this.OpenFile_Click);
            // 
            // Filter
            // 
            this.Filter.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.EffectInvert,
            this.DesatulateEffect,
            this.oldStyleMenuItem,
            this.ZoomBlurEffect,
            this.binMenuItem,
            this.boxBlurStripMenuItem,
            this.mosaicItem,
            this.gamaMenuItem,
            this.FilmStyleMenuItem,
            this.crossPhotoMenuItem,
            this.lomoItem});
            this.Filter.Name = "Filter";
            this.Filter.Size = new System.Drawing.Size(41, 20);
            this.Filter.Text = "滤镜";
            // 
            // EffectInvert
            // 
            this.EffectInvert.Name = "EffectInvert";
            this.EffectInvert.Size = new System.Drawing.Size(152, 22);
            this.EffectInvert.Text = "反色";
            this.EffectInvert.Click += new System.EventHandler(this.EffectInvert_Click);
            // 
            // DesatulateEffect
            // 
            this.DesatulateEffect.Name = "DesatulateEffect";
            this.DesatulateEffect.Size = new System.Drawing.Size(152, 22);
            this.DesatulateEffect.Text = "去色";
            this.DesatulateEffect.Click += new System.EventHandler(this.DesatulateEffect_Click);
            // 
            // oldStyleMenuItem
            // 
            this.oldStyleMenuItem.Name = "oldStyleMenuItem";
            this.oldStyleMenuItem.Size = new System.Drawing.Size(152, 22);
            this.oldStyleMenuItem.Text = "复古风";
            this.oldStyleMenuItem.Click += new System.EventHandler(this.oldStyleMenuItem_Click);
            // 
            // ZoomBlurEffect
            // 
            this.ZoomBlurEffect.Name = "ZoomBlurEffect";
            this.ZoomBlurEffect.Size = new System.Drawing.Size(152, 22);
            this.ZoomBlurEffect.Text = "缩放模糊";
            this.ZoomBlurEffect.Click += new System.EventHandler(this.ZoomBlurEffect_Click);
            // 
            // binMenuItem
            // 
            this.binMenuItem.Name = "binMenuItem";
            this.binMenuItem.Size = new System.Drawing.Size(152, 22);
            this.binMenuItem.Text = "阈值";
            this.binMenuItem.Click += new System.EventHandler(this.binMenuItem_Click);
            // 
            // boxBlurStripMenuItem
            // 
            this.boxBlurStripMenuItem.Name = "boxBlurStripMenuItem";
            this.boxBlurStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.boxBlurStripMenuItem.Text = "方框模糊";
            this.boxBlurStripMenuItem.Click += new System.EventHandler(this.boxBlurStripMenuItem_Click);
            // 
            // mosaicItem
            // 
            this.mosaicItem.Name = "mosaicItem";
            this.mosaicItem.Size = new System.Drawing.Size(152, 22);
            this.mosaicItem.Text = "马赛克";
            this.mosaicItem.Click += new System.EventHandler(this.mosaicItem_Click);
            // 
            // gamaMenuItem
            // 
            this.gamaMenuItem.Name = "gamaMenuItem";
            this.gamaMenuItem.Size = new System.Drawing.Size(152, 22);
            this.gamaMenuItem.Text = "伽马";
            this.gamaMenuItem.Click += new System.EventHandler(this.gamaMenuItem_Click);
            // 
            // FilmStyleMenuItem
            // 
            this.FilmStyleMenuItem.Name = "FilmStyleMenuItem";
            this.FilmStyleMenuItem.Size = new System.Drawing.Size(152, 22);
            this.FilmStyleMenuItem.Text = "老电影海报";
            this.FilmStyleMenuItem.Click += new System.EventHandler(this.FilmStyleMenuItem_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.File,
            this.Filter,
            this.编辑ToolStripMenuItem,
            this.信息ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(750, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 编辑ToolStripMenuItem
            // 
            this.编辑ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CopyMenu,
            this.PasteMenu,
            this.FlipHoriza});
            this.编辑ToolStripMenuItem.Name = "编辑ToolStripMenuItem";
            this.编辑ToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
            this.编辑ToolStripMenuItem.Text = "编辑";
            // 
            // CopyMenu
            // 
            this.CopyMenu.Name = "CopyMenu";
            this.CopyMenu.Size = new System.Drawing.Size(94, 22);
            this.CopyMenu.Text = "复制";
            this.CopyMenu.Click += new System.EventHandler(this.CopyMenu_Click);
            // 
            // PasteMenu
            // 
            this.PasteMenu.Name = "PasteMenu";
            this.PasteMenu.Size = new System.Drawing.Size(94, 22);
            this.PasteMenu.Text = "粘贴";
            this.PasteMenu.Click += new System.EventHandler(this.PasteMenu_Click);
            // 
            // FlipHoriza
            // 
            this.FlipHoriza.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FlipVerticalMenu,
            this.FlipHorizontalMenu});
            this.FlipHoriza.Name = "FlipHoriza";
            this.FlipHoriza.Size = new System.Drawing.Size(94, 22);
            this.FlipHoriza.Text = "旋转";
            // 
            // FlipVerticalMenu
            // 
            this.FlipVerticalMenu.Name = "FlipVerticalMenu";
            this.FlipVerticalMenu.Size = new System.Drawing.Size(118, 22);
            this.FlipVerticalMenu.Text = "垂直翻转";
            this.FlipVerticalMenu.Click += new System.EventHandler(this.FlipVerticalMenu_Click);
            // 
            // FlipHorizontalMenu
            // 
            this.FlipHorizontalMenu.Name = "FlipHorizontalMenu";
            this.FlipHorizontalMenu.Size = new System.Drawing.Size(118, 22);
            this.FlipHorizontalMenu.Text = "水平翻转";
            this.FlipHorizontalMenu.Click += new System.EventHandler(this.FlipHorizontalMenu_Click);
            // 
            // 信息ToolStripMenuItem
            // 
            this.信息ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.UsedAmount,
            this.hisgMenuItem});
            this.信息ToolStripMenuItem.Name = "信息ToolStripMenuItem";
            this.信息ToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
            this.信息ToolStripMenuItem.Text = "信息";
            // 
            // UsedAmount
            // 
            this.UsedAmount.Name = "UsedAmount";
            this.UsedAmount.Size = new System.Drawing.Size(142, 22);
            this.UsedAmount.Text = "不同的像素数";
            this.UsedAmount.Click += new System.EventHandler(this.UsedAmount_Click);
            // 
            // hisgMenuItem
            // 
            this.hisgMenuItem.Name = "hisgMenuItem";
            this.hisgMenuItem.Size = new System.Drawing.Size(142, 22);
            this.hisgMenuItem.Text = "直方图";
            this.hisgMenuItem.Click += new System.EventHandler(this.hisgMenuItem_Click);
            // 
            // Canvas
            // 
            this.Canvas.Location = new System.Drawing.Point(0, 24);
            this.Canvas.Name = "Canvas";
            this.Canvas.Size = new System.Drawing.Size(274, 200);
            this.Canvas.TabIndex = 1;
            this.Canvas.TabStop = false;
            this.Canvas.Paint += new System.Windows.Forms.PaintEventHandler(this.Canvas_Paint);
            // 
            // crossPhotoMenuItem
            // 
            this.crossPhotoMenuItem.Name = "crossPhotoMenuItem";
            this.crossPhotoMenuItem.Size = new System.Drawing.Size(152, 22);
            this.crossPhotoMenuItem.Text = "非主流";
            this.crossPhotoMenuItem.Click += new System.EventHandler(this.crossPhotoMenuItem_Click);
            // 
            // lomoItem
            // 
            this.lomoItem.Name = "lomoItem";
            this.lomoItem.Size = new System.Drawing.Size(152, 22);
            this.lomoItem.Text = "LOMO";
            this.lomoItem.Click += new System.EventHandler(this.lomoItem_Click);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(750, 507);
            this.Controls.Add(this.Canvas);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "C#图像处理";
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Canvas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripMenuItem File;
        private System.Windows.Forms.ToolStripMenuItem OpenFile;
        private System.Windows.Forms.ToolStripMenuItem Filter;
        private System.Windows.Forms.ToolStripMenuItem EffectInvert;
        private System.Windows.Forms.ToolStripMenuItem DesatulateEffect;
        private System.Windows.Forms.ToolStripMenuItem ZoomBlurEffect;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 编辑ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CopyMenu;
        private System.Windows.Forms.ToolStripMenuItem PasteMenu;
        private System.Windows.Forms.ToolStripMenuItem 信息ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem UsedAmount;
        private System.Windows.Forms.ToolStripMenuItem FlipHoriza;
        private System.Windows.Forms.ToolStripMenuItem FlipVerticalMenu;
        private System.Windows.Forms.ToolStripMenuItem FlipHorizontalMenu;
        private System.Windows.Forms.ToolStripMenuItem binMenuItem;
        private System.Windows.Forms.ToolStripMenuItem boxBlurStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hisgMenuItem;
        private System.Windows.Forms.PictureBox Canvas;
        private System.Windows.Forms.ToolStripMenuItem mosaicItem;
        private System.Windows.Forms.ToolStripMenuItem gamaMenuItem;
        private System.Windows.Forms.ToolStripMenuItem oldStyleMenuItem;
        private System.Windows.Forms.ToolStripMenuItem FilmStyleMenuItem;
        private System.Windows.Forms.ToolStripMenuItem crossPhotoMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lomoItem;


    }
}

