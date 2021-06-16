namespace ganntproj1.src.Views
{
    partial class BlockOrder
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtQty = new System.Windows.Forms.TextBox();
            this.cboLine = new System.Windows.Forms.ComboBox();
            this.txtPersons = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lblMaxQty = new System.Windows.Forms.Label();
            this.lblMaxMembers = new System.Windows.Forms.Label();
            this.lblCurrentLine = new System.Windows.Forms.Label();
            this.lblSave = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblTotQty = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.lblArticle = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.lblLine = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.lblOrder = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dtpStart = new System.Windows.Forms.DateTimePicker();
            this.label15 = new System.Windows.Forms.Label();
            this.lblDateInfo = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnResetSuggDate = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Bahnschrift", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(23, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 16);
            this.label1.TabIndex = 111;
            this.label1.Text = "New qty";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Bahnschrift", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(251, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 16);
            this.label2.TabIndex = 11;
            this.label2.Text = "Line";
            // 
            // txtQty
            // 
            this.txtQty.Font = new System.Drawing.Font("Bahnschrift", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtQty.ForeColor = System.Drawing.Color.ForestGreen;
            this.txtQty.Location = new System.Drawing.Point(26, 34);
            this.txtQty.Name = "txtQty";
            this.txtQty.Size = new System.Drawing.Size(100, 33);
            this.txtQty.TabIndex = 0;
            this.txtQty.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // cboLine
            // 
            this.cboLine.Font = new System.Drawing.Font("Bahnschrift", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboLine.FormattingEnabled = true;
            this.cboLine.Location = new System.Drawing.Point(254, 34);
            this.cboLine.Name = "cboLine";
            this.cboLine.Size = new System.Drawing.Size(207, 33);
            this.cboLine.TabIndex = 2;
            this.cboLine.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // txtPersons
            // 
            this.txtPersons.Font = new System.Drawing.Font("Bahnschrift", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPersons.ForeColor = System.Drawing.Color.Brown;
            this.txtPersons.Location = new System.Drawing.Point(153, 34);
            this.txtPersons.Name = "txtPersons";
            this.txtPersons.Size = new System.Drawing.Size(73, 33);
            this.txtPersons.TabIndex = 1;
            this.txtPersons.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Bahnschrift", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(150, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 16);
            this.label3.TabIndex = 5;
            this.label3.Text = "Nr. persons";
            // 
            // lblMaxQty
            // 
            this.lblMaxQty.AutoSize = true;
            this.lblMaxQty.Font = new System.Drawing.Font("Bahnschrift", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMaxQty.ForeColor = System.Drawing.Color.DimGray;
            this.lblMaxQty.Location = new System.Drawing.Point(23, 70);
            this.lblMaxQty.Name = "lblMaxQty";
            this.lblMaxQty.Size = new System.Drawing.Size(0, 16);
            this.lblMaxQty.TabIndex = 34;
            // 
            // lblMaxMembers
            // 
            this.lblMaxMembers.AutoSize = true;
            this.lblMaxMembers.Font = new System.Drawing.Font("Bahnschrift", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMaxMembers.ForeColor = System.Drawing.Color.DimGray;
            this.lblMaxMembers.Location = new System.Drawing.Point(150, 70);
            this.lblMaxMembers.Name = "lblMaxMembers";
            this.lblMaxMembers.Size = new System.Drawing.Size(0, 16);
            this.lblMaxMembers.TabIndex = 35;
            // 
            // lblCurrentLine
            // 
            this.lblCurrentLine.AutoSize = true;
            this.lblCurrentLine.Font = new System.Drawing.Font("Bahnschrift", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentLine.ForeColor = System.Drawing.Color.DimGray;
            this.lblCurrentLine.Location = new System.Drawing.Point(251, 70);
            this.lblCurrentLine.Name = "lblCurrentLine";
            this.lblCurrentLine.Size = new System.Drawing.Size(0, 16);
            this.lblCurrentLine.TabIndex = 36;
            // 
            // lblSave
            // 
            this.lblSave.Font = new System.Drawing.Font("Bahnschrift", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSave.ForeColor = System.Drawing.Color.Black;
            this.lblSave.Image = global::ganntproj1.Properties.Resources.save_multidm_48;
            this.lblSave.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblSave.Location = new System.Drawing.Point(473, 51);
            this.lblSave.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSave.Name = "lblSave";
            this.lblSave.Size = new System.Drawing.Size(209, 75);
            this.lblSave.TabIndex = 38;
            this.lblSave.Text = "Reprogram order";
            this.lblSave.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.lblSave.Click += new System.EventHandler(this.lblSave_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.AntiqueWhite;
            this.panel1.Controls.Add(this.lblTotQty);
            this.panel1.Controls.Add(this.label14);
            this.panel1.Controls.Add(this.lblArticle);
            this.panel1.Controls.Add(this.label12);
            this.panel1.Controls.Add(this.lblLine);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.lblOrder);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(731, 152);
            this.panel1.TabIndex = 112;
            // 
            // lblTotQty
            // 
            this.lblTotQty.AutoSize = true;
            this.lblTotQty.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.lblTotQty.Location = new System.Drawing.Point(103, 101);
            this.lblTotQty.Name = "lblTotQty";
            this.lblTotQty.Size = new System.Drawing.Size(0, 20);
            this.lblTotQty.TabIndex = 119;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(16, 101);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(73, 20);
            this.label14.TabIndex = 118;
            this.label14.Text = "Total qty:";
            // 
            // lblArticle
            // 
            this.lblArticle.AutoSize = true;
            this.lblArticle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.lblArticle.Location = new System.Drawing.Point(103, 75);
            this.lblArticle.Name = "lblArticle";
            this.lblArticle.Size = new System.Drawing.Size(0, 20);
            this.lblArticle.TabIndex = 117;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(16, 75);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(57, 20);
            this.label12.TabIndex = 116;
            this.label12.Text = "Article:";
            // 
            // lblLine
            // 
            this.lblLine.AutoSize = true;
            this.lblLine.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.lblLine.Location = new System.Drawing.Point(103, 49);
            this.lblLine.Name = "lblLine";
            this.lblLine.Size = new System.Drawing.Size(0, 20);
            this.lblLine.TabIndex = 115;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(16, 49);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(43, 20);
            this.label10.TabIndex = 114;
            this.label10.Text = "Line:";
            // 
            // lblOrder
            // 
            this.lblOrder.AutoSize = true;
            this.lblOrder.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.lblOrder.Location = new System.Drawing.Point(103, 23);
            this.lblOrder.Name = "lblOrder";
            this.lblOrder.Size = new System.Drawing.Size(0, 20);
            this.lblOrder.TabIndex = 113;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(16, 23);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(57, 20);
            this.label7.TabIndex = 112;
            this.label7.Text = "Order: ";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnResetSuggDate);
            this.groupBox1.Controls.Add(this.dtpStart);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.lblDateInfo);
            this.groupBox1.Location = new System.Drawing.Point(26, 29);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(436, 108);
            this.groupBox1.TabIndex = 113;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Date and time";
            // 
            // dtpStart
            // 
            this.dtpStart.Checked = false;
            this.dtpStart.CustomFormat = "dd/MM/yyyy HH:mm";
            this.dtpStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpStart.Location = new System.Drawing.Point(176, 45);
            this.dtpStart.Name = "dtpStart";
            this.dtpStart.ShowCheckBox = true;
            this.dtpStart.Size = new System.Drawing.Size(159, 22);
            this.dtpStart.TabIndex = 2;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Bahnschrift", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(29, 51);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(104, 16);
            this.label15.TabIndex = 6;
            this.label15.Text = "Order start date:";
            // 
            // lblDateInfo
            // 
            this.lblDateInfo.AutoSize = true;
            this.lblDateInfo.Font = new System.Drawing.Font("Bahnschrift", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDateInfo.ForeColor = System.Drawing.Color.Green;
            this.lblDateInfo.Location = new System.Drawing.Point(173, 73);
            this.lblDateInfo.Name = "lblDateInfo";
            this.lblDateInfo.Size = new System.Drawing.Size(112, 13);
            this.lblDateInfo.TabIndex = 9;
            this.lblDateInfo.Text = "Suggested date-time";
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.txtQty);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.cboLine);
            this.panel2.Controls.Add(this.lblCurrentLine);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.lblMaxMembers);
            this.panel2.Controls.Add(this.txtPersons);
            this.panel2.Controls.Add(this.lblMaxQty);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 152);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(731, 100);
            this.panel2.TabIndex = 114;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.groupBox1);
            this.panel3.Controls.Add(this.lblSave);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 252);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(731, 167);
            this.panel3.TabIndex = 115;
            // 
            // btnResetSuggDate
            // 
            this.btnResetSuggDate.BackgroundImage = global::ganntproj1.Properties.Resources.history_40;
            this.btnResetSuggDate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnResetSuggDate.Location = new System.Drawing.Point(341, 44);
            this.btnResetSuggDate.Name = "btnResetSuggDate";
            this.btnResetSuggDate.Size = new System.Drawing.Size(25, 23);
            this.btnResetSuggDate.TabIndex = 10;
            this.btnResetSuggDate.UseVisualStyleBackColor = true;
            this.btnResetSuggDate.Click += new System.EventHandler(this.btnResetSuggDate_Click);
            // 
            // BlockOrder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(731, 419);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BlockOrder";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Reprogram order";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtQty;
        private System.Windows.Forms.ComboBox cboLine;
        private System.Windows.Forms.TextBox txtPersons;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblMaxQty;
        private System.Windows.Forms.Label lblMaxMembers;
        private System.Windows.Forms.Label lblCurrentLine;
        private System.Windows.Forms.Label lblSave;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblTotQty;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label lblArticle;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label lblLine;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lblOrder;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DateTimePicker dtpStart;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label lblDateInfo;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnResetSuggDate;
    }
}