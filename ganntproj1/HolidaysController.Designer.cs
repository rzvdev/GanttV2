namespace ganntproj1
    {
    partial class HolidaysController
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HolidaysController));
            this.pnMasthead = new System.Windows.Forms.Panel();
            this.pnControlSave = new System.Windows.Forms.Panel();
            this.pbCheck = new System.Windows.Forms.PictureBox();
            this.pbDiscard = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cboYears = new System.Windows.Forms.ComboBox();
            this.cboMonth = new System.Windows.Forms.ComboBox();
            this.lblSavedInfo = new System.Windows.Forms.Label();
            this.dgvCheck = new ganntproj1.TableView();
            this.label4 = new System.Windows.Forms.Label();
            this.pnMasthead.SuspendLayout();
            this.pnControlSave.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCheck)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbDiscard)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCheck)).BeginInit();
            this.SuspendLayout();
            // 
            // pnMasthead
            // 
            this.pnMasthead.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
            this.pnMasthead.Controls.Add(this.pnControlSave);
            this.pnMasthead.Controls.Add(this.label2);
            this.pnMasthead.Controls.Add(this.label1);
            this.pnMasthead.Controls.Add(this.cboYears);
            this.pnMasthead.Controls.Add(this.cboMonth);
            this.pnMasthead.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnMasthead.Location = new System.Drawing.Point(0, 0);
            this.pnMasthead.Name = "pnMasthead";
            this.pnMasthead.Size = new System.Drawing.Size(1149, 83);
            this.pnMasthead.TabIndex = 0;
            // 
            // pnControlSave
            // 
            this.pnControlSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
            this.pnControlSave.Controls.Add(this.label4);
            this.pnControlSave.Controls.Add(this.pbCheck);
            this.pnControlSave.Controls.Add(this.pbDiscard);
            this.pnControlSave.Location = new System.Drawing.Point(402, 0);
            this.pnControlSave.Name = "pnControlSave";
            this.pnControlSave.Size = new System.Drawing.Size(158, 83);
            this.pnControlSave.TabIndex = 54;
            this.pnControlSave.Paint += new System.Windows.Forms.PaintEventHandler(this.PnControlSave_Paint);
            // 
            // pbCheck
            // 
            this.pbCheck.Image = global::ganntproj1.Properties.Resources.accept_changes_48;
            this.pbCheck.Location = new System.Drawing.Point(9, 27);
            this.pbCheck.Name = "pbCheck";
            this.pbCheck.Size = new System.Drawing.Size(42, 42);
            this.pbCheck.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbCheck.TabIndex = 0;
            this.pbCheck.TabStop = false;
            this.pbCheck.Click += new System.EventHandler(this.pbCheck_Click_1);
            // 
            // pbDiscard
            // 
            this.pbDiscard.Image = global::ganntproj1.Properties.Resources.discard_changes_48;
            this.pbDiscard.Location = new System.Drawing.Point(103, 27);
            this.pbDiscard.Name = "pbDiscard";
            this.pbDiscard.Size = new System.Drawing.Size(40, 40);
            this.pbDiscard.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbDiscard.TabIndex = 1;
            this.pbDiscard.TabStop = false;
            this.pbDiscard.Click += new System.EventHandler(this.pbDiscard_Click_1);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(217, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 17);
            this.label2.TabIndex = 51;
            this.label2.Text = "Year";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 17);
            this.label1.TabIndex = 50;
            this.label1.Text = "Month";
            // 
            // cboYears
            // 
            this.cboYears.BackColor = System.Drawing.Color.White;
            this.cboYears.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboYears.Font = new System.Drawing.Font("Segoe UI", 12.2F, System.Drawing.FontStyle.Bold);
            this.cboYears.FormattingEnabled = true;
            this.cboYears.Location = new System.Drawing.Point(220, 31);
            this.cboYears.Name = "cboYears";
            this.cboYears.Size = new System.Drawing.Size(121, 36);
            this.cboYears.TabIndex = 1;
            // 
            // cboMonth
            // 
            this.cboMonth.BackColor = System.Drawing.Color.White;
            this.cboMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMonth.Font = new System.Drawing.Font("Segoe UI", 12.2F, System.Drawing.FontStyle.Bold);
            this.cboMonth.FormattingEnabled = true;
            this.cboMonth.Items.AddRange(new object[] {
            "Gennaio",
            "Febbraio",
            "Marzo",
            "Aprile",
            "Maggio",
            "Giugno",
            "Luglio",
            "Agosto",
            "Settembre",
            "Ottobre",
            "Novembre",
            "Dicembre"});
            this.cboMonth.Location = new System.Drawing.Point(33, 31);
            this.cboMonth.Name = "cboMonth";
            this.cboMonth.Size = new System.Drawing.Size(181, 36);
            this.cboMonth.TabIndex = 0;
            // 
            // lblSavedInfo
            // 
            this.lblSavedInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSavedInfo.AutoSize = true;
            this.lblSavedInfo.BackColor = System.Drawing.Color.PaleGreen;
            this.lblSavedInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSavedInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSavedInfo.ForeColor = System.Drawing.Color.Black;
            this.lblSavedInfo.Location = new System.Drawing.Point(510, 278);
            this.lblSavedInfo.Name = "lblSavedInfo";
            this.lblSavedInfo.Size = new System.Drawing.Size(129, 34);
            this.lblSavedInfo.TabIndex = 48;
            this.lblSavedInfo.Text = "Saving...";
            this.lblSavedInfo.Visible = false;
            // 
            // dgvCheck
            // 
            this.dgvCheck.AllowUserToAddRows = false;
            this.dgvCheck.AllowUserToDeleteRows = false;
            this.dgvCheck.AllowUserToResizeColumns = false;
            this.dgvCheck.AllowUserToResizeRows = false;
            this.dgvCheck.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.dgvCheck.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvCheck.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCheck.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCheck.Location = new System.Drawing.Point(0, 83);
            this.dgvCheck.MultiSelect = false;
            this.dgvCheck.Name = "dgvCheck";
            this.dgvCheck.ReadOnly = true;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.SteelBlue;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvCheck.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.SteelBlue;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            this.dgvCheck.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvCheck.RowTemplate.Height = 24;
            this.dgvCheck.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCheck.Size = new System.Drawing.Size(1149, 508);
            this.dgvCheck.TabIndex = 1;
            this.dgvCheck.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCheck_CellClick);
            this.dgvCheck.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCheck_CellContentClick);
            this.dgvCheck.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvCheck_CellPainting);
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label4.Location = new System.Drawing.Point(76, 33);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(3, 30);
            this.label4.TabIndex = 56;
            // 
            // HolidaysController
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1149, 591);
            this.Controls.Add(this.dgvCheck);
            this.Controls.Add(this.lblSavedInfo);
            this.Controls.Add(this.pnMasthead);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "HolidaysController";
            this.Text = "Holidays checker";
            this.pnMasthead.ResumeLayout(false);
            this.pnMasthead.PerformLayout();
            this.pnControlSave.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbCheck)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbDiscard)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCheck)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

            }

        #endregion
        private System.Windows.Forms.Panel pnMasthead;
        private TableView dgvCheck;
        private System.Windows.Forms.ComboBox cboYears;
        private System.Windows.Forms.ComboBox cboMonth;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblSavedInfo;
        private System.Windows.Forms.Panel pnControlSave;
        private System.Windows.Forms.PictureBox pbCheck;
        private System.Windows.Forms.PictureBox pbDiscard;
        private System.Windows.Forms.Label label4;
    }
    }