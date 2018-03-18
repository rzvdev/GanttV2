namespace ganntproj1
    {
    partial class LoadingJobController
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoadingJobController));
            this.cmsTable = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tmsiSplitCommessa = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.resetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pnArticles = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblModelsTitle = new System.Windows.Forms.Label();
            this.dgvReport = new ganntproj1.TableView();
            this.cmsTable.SuspendLayout();
            this.pnArticles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).BeginInit();
            this.SuspendLayout();
            // 
            // cmsTable
            // 
            this.cmsTable.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmsTable.ImageScalingSize = new System.Drawing.Size(40, 40);
            this.cmsTable.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tmsiSplitCommessa,
            this.toolStripSeparator1,
            this.resetToolStripMenuItem});
            this.cmsTable.Name = "cmsTable";
            this.cmsTable.Size = new System.Drawing.Size(197, 102);
            // 
            // tmsiSplitCommessa
            // 
            this.tmsiSplitCommessa.Image = global::ganntproj1.Properties.Resources.split_16;
            this.tmsiSplitCommessa.Name = "tmsiSplitCommessa";
            this.tmsiSplitCommessa.Size = new System.Drawing.Size(196, 46);
            this.tmsiSplitCommessa.Text = "Split commessa";
            this.tmsiSplitCommessa.Click += new System.EventHandler(this.tmsiSplitCommessa_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(193, 6);
            // 
            // resetToolStripMenuItem
            // 
            this.resetToolStripMenuItem.Image = global::ganntproj1.Properties.Resources.reset_24;
            this.resetToolStripMenuItem.Name = "resetToolStripMenuItem";
            this.resetToolStripMenuItem.Size = new System.Drawing.Size(196, 46);
            this.resetToolStripMenuItem.Text = "Reset";
            this.resetToolStripMenuItem.Click += new System.EventHandler(this.ResetToolStripMenuItem_Click);
            // 
            // pnArticles
            // 
            this.pnArticles.AutoScroll = true;
            this.pnArticles.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnArticles.Controls.Add(this.pictureBox1);
            this.pnArticles.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnArticles.Location = new System.Drawing.Point(0, 29);
            this.pnArticles.Name = "pnArticles";
            this.pnArticles.Size = new System.Drawing.Size(1386, 119);
            this.pnArticles.TabIndex = 7;
            this.pnArticles.Visible = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(98)))), ((int)(((byte)(124)))));
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pictureBox1.Location = new System.Drawing.Point(0, 116);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1386, 3);
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // lblModelsTitle
            // 
            this.lblModelsTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(98)))), ((int)(((byte)(124)))));
            this.lblModelsTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblModelsTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblModelsTitle.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblModelsTitle.Location = new System.Drawing.Point(0, 0);
            this.lblModelsTitle.Name = "lblModelsTitle";
            this.lblModelsTitle.Size = new System.Drawing.Size(1386, 29);
            this.lblModelsTitle.TabIndex = 8;
            this.lblModelsTitle.Text = "Programming proposal";
            this.lblModelsTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblModelsTitle.Visible = false;
            // 
            // dgvReport
            // 
            this.dgvReport.AllowUserToAddRows = false;
            this.dgvReport.AllowUserToDeleteRows = false;
            this.dgvReport.AllowUserToResizeColumns = false;
            this.dgvReport.AllowUserToResizeRows = false;
            this.dgvReport.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.dgvReport.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvReport.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvReport.ColumnHeadersHeight = 90;
            this.dgvReport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvReport.ContextMenuStrip = this.cmsTable;
            this.dgvReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvReport.EnableHeadersVisualStyles = false;
            this.dgvReport.Location = new System.Drawing.Point(0, 148);
            this.dgvReport.MultiSelect = false;
            this.dgvReport.Name = "dgvReport";
            this.dgvReport.ReadOnly = true;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.SteelBlue;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvReport.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.SteelBlue;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.White;
            this.dgvReport.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvReport.RowTemplate.Height = 24;
            this.dgvReport.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvReport.Size = new System.Drawing.Size(1386, 550);
            this.dgvReport.TabIndex = 6;
            this.dgvReport.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvReport_CellClick);
            this.dgvReport.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvReport_CellDoubleClick);
            this.dgvReport.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvReport_CellEndEdit);
            this.dgvReport.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvReport_CellMouseEnter);
            this.dgvReport.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.DgvReport_CellPainting);
            this.dgvReport.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dgvReport_Scroll);
            // 
            // LoadingJobController
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1386, 698);
            this.Controls.Add(this.dgvReport);
            this.Controls.Add(this.pnArticles);
            this.Controls.Add(this.lblModelsTitle);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "LoadingJobController";
            this.Text = "Carico lavoro";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.cmsTable.ResumeLayout(false);
            this.pnArticles.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).EndInit();
            this.ResumeLayout(false);

            }

        #endregion
        private TableView dgvReport;
        private System.Windows.Forms.ContextMenuStrip cmsTable;
        private System.Windows.Forms.ToolStripMenuItem tmsiSplitCommessa;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem resetToolStripMenuItem;
        private System.Windows.Forms.Panel pnArticles;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblModelsTitle;
    }
    }