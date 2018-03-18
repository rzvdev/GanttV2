namespace ganntproj1
    {
    partial class SplitInput
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
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cbCommLinea = new System.Windows.Forms.ComboBox();
            this.txtComm = new System.Windows.Forms.TextBox();
            this.lblCommTit = new System.Windows.Forms.Label();
            this.pnHeader = new System.Windows.Forms.Panel();
            this.lblClose = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblOrigCapi = new System.Windows.Forms.Label();
            this.lblOrigLine = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtCommCapi = new System.Windows.Forms.TextBox();
            this.lblCapiTit = new System.Windows.Forms.Label();
            this.lblError = new System.Windows.Forms.Label();
            this.pbHistory = new System.Windows.Forms.PictureBox();
            this.lblSave = new System.Windows.Forms.Label();
            this.dtpCommData = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.pnSplitCommands = new System.Windows.Forms.Panel();
            this.lblStart = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblEnd = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.pnHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbHistory)).BeginInit();
            this.pnSplitCommands.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.label2.Location = new System.Drawing.Point(27, 94);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 18);
            this.label2.TabIndex = 36;
            this.label2.Text = "Line";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.label1.Location = new System.Drawing.Point(36, 104);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 18);
            this.label1.TabIndex = 35;
            this.label1.Text = "Commessa";
            // 
            // cbCommLinea
            // 
            this.cbCommLinea.BackColor = System.Drawing.Color.CadetBlue;
            this.cbCommLinea.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCommLinea.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbCommLinea.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbCommLinea.Location = new System.Drawing.Point(78, 83);
            this.cbCommLinea.Name = "cbCommLinea";
            this.cbCommLinea.Size = new System.Drawing.Size(181, 36);
            this.cbCommLinea.TabIndex = 34;
            // 
            // txtComm
            // 
            this.txtComm.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtComm.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtComm.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtComm.Location = new System.Drawing.Point(39, 127);
            this.txtComm.Name = "txtComm";
            this.txtComm.ReadOnly = true;
            this.txtComm.Size = new System.Drawing.Size(169, 27);
            this.txtComm.TabIndex = 33;
            // 
            // lblCommTit
            // 
            this.lblCommTit.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblCommTit.Font = new System.Drawing.Font("Tahoma", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCommTit.ForeColor = System.Drawing.Color.CadetBlue;
            this.lblCommTit.Location = new System.Drawing.Point(14, 34);
            this.lblCommTit.Name = "lblCommTit";
            this.lblCommTit.Size = new System.Drawing.Size(193, 41);
            this.lblCommTit.TabIndex = 37;
            this.lblCommTit.Text = "Split";
            this.lblCommTit.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.lblCommTit.Paint += new System.Windows.Forms.PaintEventHandler(this.lblCommTit_Paint);
            // 
            // pnHeader
            // 
            this.pnHeader.BackColor = System.Drawing.Color.CadetBlue;
            this.pnHeader.Controls.Add(this.lblClose);
            this.pnHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnHeader.Location = new System.Drawing.Point(0, 0);
            this.pnHeader.Name = "pnHeader";
            this.pnHeader.Size = new System.Drawing.Size(486, 70);
            this.pnHeader.TabIndex = 38;
            // 
            // lblClose
            // 
            this.lblClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClose.ForeColor = System.Drawing.Color.White;
            this.lblClose.Location = new System.Drawing.Point(414, 12);
            this.lblClose.Name = "lblClose";
            this.lblClose.Size = new System.Drawing.Size(60, 44);
            this.lblClose.TabIndex = 0;
            this.lblClose.Text = "X";
            this.lblClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblClose.Click += new System.EventHandler(this.lblClose_Click);
            this.lblClose.Paint += new System.Windows.Forms.PaintEventHandler(this.lblClose_Paint);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.label3.Location = new System.Drawing.Point(36, 206);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 18);
            this.label3.TabIndex = 39;
            this.label3.Text = "Original qty:";
            // 
            // lblOrigCapi
            // 
            this.lblOrigCapi.AutoSize = true;
            this.lblOrigCapi.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOrigCapi.Location = new System.Drawing.Point(133, 206);
            this.lblOrigCapi.Name = "lblOrigCapi";
            this.lblOrigCapi.Size = new System.Drawing.Size(0, 18);
            this.lblOrigCapi.TabIndex = 40;
            // 
            // lblOrigLine
            // 
            this.lblOrigLine.AutoSize = true;
            this.lblOrigLine.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOrigLine.Location = new System.Drawing.Point(133, 168);
            this.lblOrigLine.Name = "lblOrigLine";
            this.lblOrigLine.Size = new System.Drawing.Size(0, 18);
            this.lblOrigLine.TabIndex = 42;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.label6.Location = new System.Drawing.Point(36, 168);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(88, 18);
            this.label6.TabIndex = 41;
            this.label6.Text = "Original line:";
            // 
            // label11
            // 
            this.label11.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.label11.Location = new System.Drawing.Point(11, 277);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(481, 2);
            this.label11.TabIndex = 43;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.label4.ForeColor = System.Drawing.Color.DimGray;
            this.label4.Location = new System.Drawing.Point(7, 255);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 18);
            this.label4.TabIndex = 44;
            this.label4.Text = "Split input";
            // 
            // txtCommCapi
            // 
            this.txtCommCapi.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCommCapi.ForeColor = System.Drawing.Color.CadetBlue;
            this.txtCommCapi.Location = new System.Drawing.Point(157, 141);
            this.txtCommCapi.Name = "txtCommCapi";
            this.txtCommCapi.Size = new System.Drawing.Size(102, 43);
            this.txtCommCapi.TabIndex = 46;
            this.txtCommCapi.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtCommCapi.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCommCapi_KeyDown);
            // 
            // lblCapiTit
            // 
            this.lblCapiTit.AutoSize = true;
            this.lblCapiTit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.lblCapiTit.Location = new System.Drawing.Point(24, 159);
            this.lblCapiTit.Name = "lblCapiTit";
            this.lblCapiTit.Size = new System.Drawing.Size(38, 18);
            this.lblCapiTit.TabIndex = 45;
            this.lblCapiTit.Text = "Capi";
            // 
            // lblError
            // 
            this.lblError.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblError.ForeColor = System.Drawing.Color.Red;
            this.lblError.Location = new System.Drawing.Point(9, 540);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(483, 44);
            this.lblError.TabIndex = 51;
            this.lblError.Text = "You\'re not able to split this order again.";
            this.lblError.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblError.Visible = false;
            // 
            // pbHistory
            // 
            this.pbHistory.Image = global::ganntproj1.Properties.Resources.history_40;
            this.pbHistory.Location = new System.Drawing.Point(266, 88);
            this.pbHistory.Name = "pbHistory";
            this.pbHistory.Size = new System.Drawing.Size(47, 45);
            this.pbHistory.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbHistory.TabIndex = 52;
            this.pbHistory.TabStop = false;
            this.pbHistory.Click += new System.EventHandler(this.pbHistory_Click);
            // 
            // lblSave
            // 
            this.lblSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSave.ForeColor = System.Drawing.Color.Black;
            this.lblSave.Image = global::ganntproj1.Properties.Resources.save_48;
            this.lblSave.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblSave.Location = new System.Drawing.Point(312, 69);
            this.lblSave.Name = "lblSave";
            this.lblSave.Size = new System.Drawing.Size(122, 97);
            this.lblSave.TabIndex = 47;
            this.lblSave.Text = "Save";
            this.lblSave.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.lblSave.Click += new System.EventHandler(this.lblSave_Click);
            // 
            // dtpCommData
            // 
            this.dtpCommData.CalendarTitleForeColor = System.Drawing.Color.AliceBlue;
            this.dtpCommData.CustomFormat = "dd/MM/yyyy";
            this.dtpCommData.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpCommData.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpCommData.Location = new System.Drawing.Point(78, 23);
            this.dtpCommData.Name = "dtpCommData";
            this.dtpCommData.Size = new System.Drawing.Size(181, 38);
            this.dtpCommData.TabIndex = 54;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.label5.Location = new System.Drawing.Point(23, 36);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 18);
            this.label5.TabIndex = 53;
            this.label5.Text = "Start";
            // 
            // pnSplitCommands
            // 
            this.pnSplitCommands.Controls.Add(this.label2);
            this.pnSplitCommands.Controls.Add(this.cbCommLinea);
            this.pnSplitCommands.Controls.Add(this.dtpCommData);
            this.pnSplitCommands.Controls.Add(this.lblCapiTit);
            this.pnSplitCommands.Controls.Add(this.label5);
            this.pnSplitCommands.Controls.Add(this.txtCommCapi);
            this.pnSplitCommands.Controls.Add(this.lblSave);
            this.pnSplitCommands.Location = new System.Drawing.Point(11, 292);
            this.pnSplitCommands.Name = "pnSplitCommands";
            this.pnSplitCommands.Size = new System.Drawing.Size(467, 234);
            this.pnSplitCommands.TabIndex = 55;
            // 
            // lblStart
            // 
            this.lblStart.AutoSize = true;
            this.lblStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStart.Location = new System.Drawing.Point(300, 168);
            this.lblStart.Name = "lblStart";
            this.lblStart.Size = new System.Drawing.Size(0, 18);
            this.lblStart.TabIndex = 57;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.label8.Location = new System.Drawing.Point(263, 168);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(31, 18);
            this.label8.TabIndex = 56;
            this.label8.Text = "Da:";
            // 
            // lblEnd
            // 
            this.lblEnd.AutoSize = true;
            this.lblEnd.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEnd.Location = new System.Drawing.Point(300, 206);
            this.lblEnd.Name = "lblEnd";
            this.lblEnd.Size = new System.Drawing.Size(0, 18);
            this.lblEnd.TabIndex = 59;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.label9.Location = new System.Drawing.Point(263, 206);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(21, 18);
            this.label9.TabIndex = 58;
            this.label9.Text = "A:";
            // 
            // SplitInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(486, 606);
            this.ControlBox = false;
            this.Controls.Add(this.lblEnd);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.lblStart);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.pnSplitCommands);
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.pbHistory);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.lblOrigLine);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lblOrigCapi);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblCommTit);
            this.Controls.Add(this.pnHeader);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtComm);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SplitInput";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.SplitInput_Load);
            this.pnHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbHistory)).EndInit();
            this.pnSplitCommands.ResumeLayout(false);
            this.pnSplitCommands.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

            }

        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbCommLinea;
        private System.Windows.Forms.TextBox txtComm;
        private System.Windows.Forms.Label lblCommTit;
        private System.Windows.Forms.Panel pnHeader;
        private System.Windows.Forms.Label lblClose;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblOrigCapi;
        private System.Windows.Forms.Label lblOrigLine;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtCommCapi;
        private System.Windows.Forms.Label lblCapiTit;
        private System.Windows.Forms.Label lblSave;
        private System.Windows.Forms.Label lblError;
        private System.Windows.Forms.PictureBox pbHistory;
        private System.Windows.Forms.DateTimePicker dtpCommData;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel pnSplitCommands;
        private System.Windows.Forms.Label lblStart;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblEnd;
        private System.Windows.Forms.Label label9;
        }
    }