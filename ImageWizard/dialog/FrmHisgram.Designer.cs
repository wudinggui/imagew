namespace ImageWizard.dialog
{
    partial class FrmHisgram
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.picHisg = new System.Windows.Forms.PictureBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.cmbChannal = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.picHisg)).BeginInit();
            this.SuspendLayout();
            // 
            // picHisg
            // 
            this.picHisg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picHisg.Location = new System.Drawing.Point(22, 48);
            this.picHisg.Name = "picHisg";
            this.picHisg.Size = new System.Drawing.Size(256, 256);
            this.picHisg.TabIndex = 0;
            this.picHisg.TabStop = false;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(292, 57);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(62, 25);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // cmbChannal
            // 
            this.cmbChannal.FormattingEnabled = true;
            this.cmbChannal.Items.AddRange(new object[] {
            "Red",
            "Green",
            "Blue"});
            this.cmbChannal.Location = new System.Drawing.Point(22, 22);
            this.cmbChannal.Name = "cmbChannal";
            this.cmbChannal.Size = new System.Drawing.Size(117, 20);
            this.cmbChannal.TabIndex = 2;
            this.cmbChannal.Text = "Red";
            this.cmbChannal.SelectedIndexChanged += new System.EventHandler(this.cmbChannal_SelectedIndexChanged);
            // 
            // FrmHisgram
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(361, 350);
            this.Controls.Add(this.cmbChannal);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.picHisg);
            this.Name = "FrmHisgram";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FrmHisgram";
            this.Load += new System.EventHandler(this.FrmHisgram_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picHisg)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picHisg;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ComboBox cmbChannal;
    }
}