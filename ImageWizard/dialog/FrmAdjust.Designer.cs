namespace ImageWizard.dialog
{
    partial class FrmAdjust
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
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.trbValue = new System.Windows.Forms.TrackBar();
            this.chkPreview = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.trbValue)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(163, 79);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(66, 27);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(235, 79);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(63, 27);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // trbValue
            // 
            this.trbValue.AutoSize = false;
            this.trbValue.Location = new System.Drawing.Point(30, 48);
            this.trbValue.Maximum = 255;
            this.trbValue.Name = "trbValue";
            this.trbValue.Size = new System.Drawing.Size(254, 25);
            this.trbValue.TabIndex = 2;
            this.trbValue.Value = 128;
            this.trbValue.Scroll += new System.EventHandler(this.trbValue_Scroll);
            this.trbValue.ValueChanged += new System.EventHandler(this.trbValue_ValueChanged);
            // 
            // chkPreview
            // 
            this.chkPreview.AutoSize = true;
            this.chkPreview.Location = new System.Drawing.Point(250, 26);
            this.chkPreview.Name = "chkPreview";
            this.chkPreview.Size = new System.Drawing.Size(48, 16);
            this.chkPreview.TabIndex = 3;
            this.chkPreview.Text = "预览";
            this.chkPreview.UseVisualStyleBackColor = true;
            // 
            // FrmAdjust
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(310, 118);
            this.Controls.Add(this.chkPreview);
            this.Controls.Add(this.trbValue);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Name = "FrmAdjust";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "调整";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmAdjust_FormClosed);
            this.Load += new System.EventHandler(this.FrmAdjust_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trbValue)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TrackBar trbValue;
        private System.Windows.Forms.CheckBox chkPreview;
    }
}