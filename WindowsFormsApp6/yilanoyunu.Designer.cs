namespace WindowsFormsApp6
{
    partial class yilanoyunu
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pnl = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblpuan = new System.Windows.Forms.Label();
            this.lblsure = new System.Windows.Forms.Label();
            this.timerYilanHareket = new System.Windows.Forms.Timer(this.components);
            this.timersaat = new System.Windows.Forms.Timer(this.components);
            this.label5 = new System.Windows.Forms.Label();
            this.RakipPuanGost = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "label1";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(400, 70);
            this.label2.TabIndex = 1;
            this.label2.Text = "label2";
            // 
            // pnl
            // 
            this.pnl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnl.Location = new System.Drawing.Point(12, 82);
            this.pnl.Name = "pnl";
            this.pnl.Size = new System.Drawing.Size(400, 260);
            this.pnl.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label3.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label3.Location = new System.Drawing.Point(313, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 20);
            this.label3.TabIndex = 0;
            this.label3.Text = "süre:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label4.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label4.Location = new System.Drawing.Point(22, 40);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(101, 20);
            this.label4.TabIndex = 1;
            this.label4.Text = "oyuncuPuan:";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // lblpuan
            // 
            this.lblpuan.AutoSize = true;
            this.lblpuan.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lblpuan.Location = new System.Drawing.Point(129, 40);
            this.lblpuan.Name = "lblpuan";
            this.lblpuan.Size = new System.Drawing.Size(18, 20);
            this.lblpuan.TabIndex = 3;
            this.lblpuan.Text = "0";
            // 
            // lblsure
            // 
            this.lblsure.AutoSize = true;
            this.lblsure.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lblsure.Location = new System.Drawing.Point(363, 40);
            this.lblsure.Name = "lblsure";
            this.lblsure.Size = new System.Drawing.Size(18, 20);
            this.lblsure.TabIndex = 4;
            this.lblsure.Text = "0";
            this.lblsure.Click += new System.EventHandler(this.lblsure_Click);
            // 
            // timerYilanHareket
            // 
            this.timerYilanHareket.Tick += new System.EventHandler(this.timerYilanHareket_Tick);
            // 
            // timersaat
            // 
            this.timersaat.Interval = 1000;
            this.timersaat.Tick += new System.EventHandler(this.timersaat_Tick);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.label5.Location = new System.Drawing.Point(187, 40);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(84, 20);
            this.label5.TabIndex = 5;
            this.label5.Text = "rakipPuan:";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // RakipPuanGost
            // 
            this.RakipPuanGost.AutoSize = true;
            this.RakipPuanGost.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.RakipPuanGost.Location = new System.Drawing.Point(277, 40);
            this.RakipPuanGost.Name = "RakipPuanGost";
            this.RakipPuanGost.Size = new System.Drawing.Size(18, 20);
            this.RakipPuanGost.TabIndex = 7;
            this.RakipPuanGost.Text = "0";
            // 
            // yilanoyunu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(425, 348);
            this.Controls.Add(this.RakipPuanGost);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblsure);
            this.Controls.Add(this.lblpuan);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.pnl);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.KeyPreview = true;
            this.Name = "yilanoyunu";
            this.Text = "yılan oyunu";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel pnl;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblpuan;
        private System.Windows.Forms.Label lblsure;
        private System.Windows.Forms.Timer timerYilanHareket;
        private System.Windows.Forms.Timer timersaat;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label RakipPuanGost;
    }

}

