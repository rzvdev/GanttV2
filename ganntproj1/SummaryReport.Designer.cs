namespace ganntproj1
    {
    partial class SummaryReport
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SummaryReport));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblChanell = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblInterval = new System.Windows.Forms.ToolStripStatusLabel();
            this.pnSideBar = new System.Windows.Forms.Panel();
            this.rbData = new System.Windows.Forms.RadioButton();
            this.rbRitardi = new System.Windows.Forms.RadioButton();
            this.rbAnticipi = new System.Windows.Forms.RadioButton();
            this.btn_Stiro = new System.Windows.Forms.Button();
            this.btn_Conf = new System.Windows.Forms.Button();
            this.btnColapseSideBar = new System.Windows.Forms.Button();
            this.lblFiltersTit = new System.Windows.Forms.Label();
            this.btn_Tess = new System.Windows.Forms.Button();
            this.btnReload = new System.Windows.Forms.Button();
            this.dtpDateTo = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.dtpDateFrom = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.pnFields = new System.Windows.Forms.Panel();
            this.dgvReport = new ganntproj1.TableView();
            this.miniTitle1 = new ganntproj1.MiniTitle();
            this.mainTitle = new ganntproj1.Title();
            this.statusStrip1.SuspendLayout();
            this.pnSideBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).BeginInit();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus,
            this.lblChanell,
            this.lblInterval});
            this.statusStrip1.Location = new System.Drawing.Point(0, 684);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1422, 30);
            this.statusStrip1.TabIndex = 7;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(60, 25);
            this.lblStatus.Text = "Ready";
            // 
            // lblChanell
            // 
            this.lblChanell.Name = "lblChanell";
            this.lblChanell.Size = new System.Drawing.Size(69, 25);
            this.lblChanell.Text = "Chanell";
            // 
            // lblInterval
            // 
            this.lblInterval.Name = "lblInterval";
            this.lblInterval.Size = new System.Drawing.Size(70, 25);
            this.lblInterval.Text = "Interval";
            // 
            // pnSideBar
            // 
            this.pnSideBar.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnSideBar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnSideBar.Controls.Add(this.rbData);
            this.pnSideBar.Controls.Add(this.rbRitardi);
            this.pnSideBar.Controls.Add(this.rbAnticipi);
            this.pnSideBar.Controls.Add(this.btn_Stiro);
            this.pnSideBar.Controls.Add(this.btn_Conf);
            this.pnSideBar.Controls.Add(this.btnColapseSideBar);
            this.pnSideBar.Controls.Add(this.lblFiltersTit);
            this.pnSideBar.Controls.Add(this.btn_Tess);
            this.pnSideBar.Controls.Add(this.btnReload);
            this.pnSideBar.Controls.Add(this.dtpDateTo);
            this.pnSideBar.Controls.Add(this.label2);
            this.pnSideBar.Controls.Add(this.dtpDateFrom);
            this.pnSideBar.Controls.Add(this.label1);
            this.pnSideBar.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnSideBar.Location = new System.Drawing.Point(0, 70);
            this.pnSideBar.Name = "pnSideBar";
            this.pnSideBar.Size = new System.Drawing.Size(224, 614);
            this.pnSideBar.TabIndex = 10;
            // 
            // rbData
            // 
            this.rbData.AutoSize = true;
            this.rbData.Checked = true;
            this.rbData.Location = new System.Drawing.Point(19, 155);
            this.rbData.Name = "rbData";
            this.rbData.Size = new System.Drawing.Size(94, 24);
            this.rbData.TabIndex = 26;
            this.rbData.TabStop = true;
            this.rbData.Text = "Per data";
            this.rbData.UseVisualStyleBackColor = true;
            // 
            // rbRitardi
            // 
            this.rbRitardi.AutoSize = true;
            this.rbRitardi.Location = new System.Drawing.Point(19, 227);
            this.rbRitardi.Name = "rbRitardi";
            this.rbRitardi.Size = new System.Drawing.Size(101, 24);
            this.rbRitardi.TabIndex = 25;
            this.rbRitardi.Text = "Per ritardi";
            this.rbRitardi.UseVisualStyleBackColor = true;
            // 
            // rbAnticipi
            // 
            this.rbAnticipi.AutoSize = true;
            this.rbAnticipi.Location = new System.Drawing.Point(19, 191);
            this.rbAnticipi.Name = "rbAnticipi";
            this.rbAnticipi.Size = new System.Drawing.Size(111, 24);
            this.rbAnticipi.TabIndex = 24;
            this.rbAnticipi.Text = "Per anticipi";
            this.rbAnticipi.UseVisualStyleBackColor = true;
            // 
            // btn_Stiro
            // 
            this.btn_Stiro.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_Stiro.FlatAppearance.BorderSize = 0;
            this.btn_Stiro.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Stiro.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Stiro.Image = global::ganntproj1.Properties.Resources.stiro;
            this.btn_Stiro.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Stiro.Location = new System.Drawing.Point(19, 505);
            this.btn_Stiro.Name = "btn_Stiro";
            this.btn_Stiro.Size = new System.Drawing.Size(137, 47);
            this.btn_Stiro.TabIndex = 23;
            this.btn_Stiro.Text = "Stiro";
            this.btn_Stiro.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_Stiro.UseVisualStyleBackColor = false;
            // 
            // btn_Conf
            // 
            this.btn_Conf.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_Conf.FlatAppearance.BorderSize = 0;
            this.btn_Conf.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Conf.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Conf.Image = global::ganntproj1.Properties.Resources.conf;
            this.btn_Conf.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Conf.Location = new System.Drawing.Point(19, 447);
            this.btn_Conf.Name = "btn_Conf";
            this.btn_Conf.Size = new System.Drawing.Size(137, 47);
            this.btn_Conf.TabIndex = 22;
            this.btn_Conf.Text = "Confezione";
            this.btn_Conf.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_Conf.UseVisualStyleBackColor = false;
            // 
            // btnColapseSideBar
            // 
            this.btnColapseSideBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnColapseSideBar.FlatAppearance.BorderSize = 0;
            this.btnColapseSideBar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnColapseSideBar.Image = global::ganntproj1.Properties.Resources.resize;
            this.btnColapseSideBar.Location = new System.Drawing.Point(0, 579);
            this.btnColapseSideBar.Name = "btnColapseSideBar";
            this.btnColapseSideBar.Size = new System.Drawing.Size(222, 33);
            this.btnColapseSideBar.TabIndex = 5;
            this.btnColapseSideBar.UseVisualStyleBackColor = true;
            // 
            // lblFiltersTit
            // 
            this.lblFiltersTit.AutoSize = true;
            this.lblFiltersTit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFiltersTit.Location = new System.Drawing.Point(15, 340);
            this.lblFiltersTit.Name = "lblFiltersTit";
            this.lblFiltersTit.Size = new System.Drawing.Size(52, 20);
            this.lblFiltersTit.TabIndex = 13;
            this.lblFiltersTit.Text = "Filters";
            // 
            // btn_Tess
            // 
            this.btn_Tess.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_Tess.FlatAppearance.BorderSize = 0;
            this.btn_Tess.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Tess.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Tess.Image = global::ganntproj1.Properties.Resources.tess;
            this.btn_Tess.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Tess.Location = new System.Drawing.Point(19, 386);
            this.btn_Tess.Name = "btn_Tess";
            this.btn_Tess.Size = new System.Drawing.Size(137, 47);
            this.btn_Tess.TabIndex = 21;
            this.btn_Tess.Text = "Tessitura";
            this.btn_Tess.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_Tess.UseVisualStyleBackColor = false;
            this.btn_Tess.Click += new System.EventHandler(this.btn_Tess_Click);
            // 
            // btnReload
            // 
            this.btnReload.FlatAppearance.BorderSize = 0;
            this.btnReload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReload.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReload.Image = global::ganntproj1.Properties.Resources.reload;
            this.btnReload.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReload.Location = new System.Drawing.Point(19, 268);
            this.btnReload.Name = "btnReload";
            this.btnReload.Size = new System.Drawing.Size(117, 47);
            this.btnReload.TabIndex = 4;
            this.btnReload.Text = "&Reload";
            this.btnReload.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnReload.UseVisualStyleBackColor = true;
            this.btnReload.Click += new System.EventHandler(this.btnReload_Click);
            // 
            // dtpDateTo
            // 
            this.dtpDateTo.CustomFormat = "dd/MM/yyyy";
            this.dtpDateTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpDateTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDateTo.Location = new System.Drawing.Point(19, 109);
            this.dtpDateTo.Name = "dtpDateTo";
            this.dtpDateTo.Size = new System.Drawing.Size(126, 26);
            this.dtpDateTo.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(15, 86);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "To";
            // 
            // dtpDateFrom
            // 
            this.dtpDateFrom.CustomFormat = "dd/MM/yyyy";
            this.dtpDateFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDateFrom.Location = new System.Drawing.Point(19, 38);
            this.dtpDateFrom.Name = "dtpDateFrom";
            this.dtpDateFrom.Size = new System.Drawing.Size(126, 26);
            this.dtpDateFrom.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(15, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "From";
            // 
            // pnFields
            // 
            this.pnFields.AutoScroll = true;
            this.pnFields.BackColor = System.Drawing.Color.White;
            this.pnFields.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnFields.Location = new System.Drawing.Point(224, 110);
            this.pnFields.Name = "pnFields";
            this.pnFields.Size = new System.Drawing.Size(1198, 74);
            this.pnFields.TabIndex = 12;
            // 
            // dgvReport
            // 
            this.dgvReport.AllowUserToAddRows = false;
            this.dgvReport.AllowUserToDeleteRows = false;
            this.dgvReport.AllowUserToResizeRows = false;
            this.dgvReport.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.dgvReport.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvReport.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvReport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvReport.EnableHeadersVisualStyles = false;
            this.dgvReport.GridColor = System.Drawing.Color.Gainsboro;
            this.dgvReport.Location = new System.Drawing.Point(224, 184);
            this.dgvReport.MultiSelect = false;
            this.dgvReport.Name = "dgvReport";
            this.dgvReport.ReadOnly = true;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.SteelBlue;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvReport.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.SteelBlue;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.White;
            this.dgvReport.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvReport.RowTemplate.Height = 28;
            this.dgvReport.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvReport.Size = new System.Drawing.Size(1198, 500);
            this.dgvReport.TabIndex = 9;
            this.dgvReport.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dgvReport_Scroll);
            // 
            // miniTitle1
            // 
            this.miniTitle1.BackColor = System.Drawing.Color.White;
            this.miniTitle1.Dock = System.Windows.Forms.DockStyle.Top;
            this.miniTitle1.Location = new System.Drawing.Point(224, 70);
            this.miniTitle1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.miniTitle1.Name = "miniTitle1";
            this.miniTitle1.Size = new System.Drawing.Size(1198, 40);
            this.miniTitle1.TabIndex = 11;
            this.miniTitle1.TitleText = "Tessitura";
            // 
            // mainTitle
            // 
            this.mainTitle.BackColor = System.Drawing.Color.Transparent;
            this.mainTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.mainTitle.Location = new System.Drawing.Point(0, 0);
            this.mainTitle.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.mainTitle.Name = "mainTitle";
            this.mainTitle.Size = new System.Drawing.Size(1422, 70);
            this.mainTitle.TabIndex = 5;
            this.mainTitle.TitleText = " ONLYOU";
            // 
            // SummaryReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1422, 714);
            this.Controls.Add(this.dgvReport);
            this.Controls.Add(this.pnFields);
            this.Controls.Add(this.miniTitle1);
            this.Controls.Add(this.pnSideBar);
            this.Controls.Add(this.mainTitle);
            this.Controls.Add(this.statusStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SummaryReport";
            this.Text = "Summary report";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.pnSideBar.ResumeLayout(false);
            this.pnSideBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

            }

        #endregion
        private Title mainTitle;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ToolStripStatusLabel lblChanell;
        private System.Windows.Forms.ToolStripStatusLabel lblInterval;
        private TableView dgvReport;
        private System.Windows.Forms.Panel pnSideBar;
        private System.Windows.Forms.Button btn_Stiro;
        private System.Windows.Forms.Button btn_Conf;
        private System.Windows.Forms.Button btnColapseSideBar;
        private System.Windows.Forms.Label lblFiltersTit;
        private System.Windows.Forms.Button btn_Tess;
        private System.Windows.Forms.Button btnReload;
        private System.Windows.Forms.DateTimePicker dtpDateTo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtpDateFrom;
        private System.Windows.Forms.Label label1;
        private MiniTitle miniTitle1;
        private System.Windows.Forms.Panel pnFields;
        private System.Windows.Forms.RadioButton rbData;
        private System.Windows.Forms.RadioButton rbRitardi;
        private System.Windows.Forms.RadioButton rbAnticipi;
        }
    }