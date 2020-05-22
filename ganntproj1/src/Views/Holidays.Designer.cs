namespace ganntproj1
    {
    partial class Holidays
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Holidays));
            this.pnMasthead = new System.Windows.Forms.Panel();
            this.pnControlSave = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cboYears = new System.Windows.Forms.ComboBox();
            this.cboMonth = new System.Windows.Forms.ComboBox();
            this.lblSavedInfo = new System.Windows.Forms.Label();
            this.dgvCheck = new ganntproj1.TableView();
            this.pbCheck = new System.Windows.Forms.PictureBox();
            this.pbDiscard = new System.Windows.Forms.PictureBox();
            this.pnMasthead.SuspendLayout();
            this.pnControlSave.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCheck)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbCheck)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbDiscard)).BeginInit();
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
            this.pnMasthead.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pnMasthead.Name = "pnMasthead";
            this.pnMasthead.Size = new System.Drawing.Size(862, 67);
            this.pnMasthead.TabIndex = 0;
            // 
            // pnControlSave
            // 
            this.pnControlSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
            this.pnControlSave.Controls.Add(this.pbCheck);
            this.pnControlSave.Controls.Add(this.pbDiscard);
            this.pnControlSave.Location = new System.Drawing.Point(288, 0);
            this.pnControlSave.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pnControlSave.Name = "pnControlSave";
            this.pnControlSave.Size = new System.Drawing.Size(132, 67);
            this.pnControlSave.TabIndex = 54;
            this.pnControlSave.Paint += new System.Windows.Forms.PaintEventHandler(this.PnControlSave_Paint);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(163, 6);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 51;
            this.label2.Text = "Year";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 6);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 50;
            this.label1.Text = "Month";
            // 
            // cboYears
            // 
            this.cboYears.BackColor = System.Drawing.Color.White;
            this.cboYears.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboYears.Font = new System.Drawing.Font("Segoe UI", 12.2F, System.Drawing.FontStyle.Bold);
            this.cboYears.FormattingEnabled = true;
            this.cboYears.Location = new System.Drawing.Point(165, 25);
            this.cboYears.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cboYears.Name = "cboYears";
            this.cboYears.Size = new System.Drawing.Size(92, 29);
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
            this.cboMonth.Location = new System.Drawing.Point(25, 25);
            this.cboMonth.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cboMonth.Name = "cboMonth";
            this.cboMonth.Size = new System.Drawing.Size(137, 29);
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
            this.lblSavedInfo.Location = new System.Drawing.Point(382, 226);
            this.lblSavedInfo.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSavedInfo.Name = "lblSavedInfo";
            this.lblSavedInfo.Size = new System.Drawing.Size(99, 28);
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
            this.dgvCheck.Location = new System.Drawing.Point(0, 67);
            this.dgvCheck.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
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
            this.dgvCheck.Size = new System.Drawing.Size(862, 413);
            this.dgvCheck.TabIndex = 1;
            this.dgvCheck.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCheck_CellClick);
            this.dgvCheck.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCheck_CellContentClick);
            this.dgvCheck.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvCheck_CellPainting);
            // 
            // pbCheck
            // 
            this.pbCheck.Image = global::ganntproj1.Properties.Resources.check;
            this.pbCheck.Location = new System.Drawing.Point(14, 21);
            this.pbCheck.Margin = new System.Windows.Forms.Padding(2);
            this.pbCheck.Name = "pbCheck";
            this.pbCheck.Size = new System.Drawing.Size(32, 34);
            this.pbCheck.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbCheck.TabIndex = 0;
            this.pbCheck.TabStop = false;
            this.pbCheck.Click += new System.EventHandler(this.pbCheck_Click_1);
            // 
            // pbDiscard
            // 
            this.pbDiscard.Image = global::ganntproj1.Properties.Resources.close;
            this.pbDiscard.Location = new System.Drawing.Point(61, 21);
            this.pbDiscard.Margin = new System.Windows.Forms.Padding(2);
            this.pbDiscard.Name = "pbDiscard";
            this.pbDiscard.Size = new System.Drawing.Size(32, 34);
            this.pbDiscard.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbDiscard.TabIndex = 1;
            this.pbDiscard.TabStop = false;
            this.pbDiscard.Click += new System.EventHandler(this.pbDiscard_Click_1);
            // 
            // Holidays
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(862, 480);
            this.Controls.Add(this.dgvCheck);
            this.Controls.Add(this.lblSavedInfo);
            this.Controls.Add(this.pnMasthead);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Holidays";
            this.Text = "Holidays checker";
            this.pnMasthead.ResumeLayout(false);
            this.pnMasthead.PerformLayout();
            this.pnControlSave.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCheck)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbCheck)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbDiscard)).EndInit();
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
    }
    }