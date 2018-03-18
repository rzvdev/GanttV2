namespace ganntproj1
{
    partial class LineGraph
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LineGraph));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnZoomOut = new System.Windows.Forms.Button();
            this.btnZoomIn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cboYears = new System.Windows.Forms.ComboBox();
            this.cboMonth = new System.Windows.Forms.ComboBox();
            this.tblGraph = new ganntproj1.TableView();
            this.cbYearAll = new ganntproj1.MyCheckBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tblGraph)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel1.Controls.Add(this.cbYearAll);
            this.panel1.Controls.Add(this.btnZoomOut);
            this.panel1.Controls.Add(this.btnZoomIn);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.cboYears);
            this.panel1.Controls.Add(this.cboMonth);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(600, 85);
            this.panel1.TabIndex = 14;
            // 
            // btnZoomOut
            // 
            this.btnZoomOut.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnZoomOut.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnZoomOut.FlatAppearance.BorderSize = 0;
            this.btnZoomOut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnZoomOut.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnZoomOut.Image = global::ganntproj1.Properties.Resources.zoom_out_d32;
            this.btnZoomOut.Location = new System.Drawing.Point(396, 34);
            this.btnZoomOut.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnZoomOut.Name = "btnZoomOut";
            this.btnZoomOut.Size = new System.Drawing.Size(37, 40);
            this.btnZoomOut.TabIndex = 60;
            this.btnZoomOut.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.btnZoomOut.UseVisualStyleBackColor = false;
            this.btnZoomOut.Click += new System.EventHandler(this.BtnZoomOut_Click);
            // 
            // btnZoomIn
            // 
            this.btnZoomIn.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnZoomIn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnZoomIn.FlatAppearance.BorderSize = 0;
            this.btnZoomIn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnZoomIn.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnZoomIn.Image = global::ganntproj1.Properties.Resources.zoom_in_d32;
            this.btnZoomIn.Location = new System.Drawing.Point(355, 34);
            this.btnZoomIn.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnZoomIn.Name = "btnZoomIn";
            this.btnZoomIn.Size = new System.Drawing.Size(37, 40);
            this.btnZoomIn.TabIndex = 59;
            this.btnZoomIn.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.btnZoomIn.UseVisualStyleBackColor = false;
            this.btnZoomIn.Click += new System.EventHandler(this.BtnZoomIn_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(156, 17);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 55;
            this.label2.Text = "Year";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 17);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 54;
            this.label1.Text = "Month";
            // 
            // cboYears
            // 
            this.cboYears.BackColor = System.Drawing.Color.White;
            this.cboYears.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboYears.Font = new System.Drawing.Font("Segoe UI", 12.2F, System.Drawing.FontStyle.Bold);
            this.cboYears.FormattingEnabled = true;
            this.cboYears.Location = new System.Drawing.Point(158, 40);
            this.cboYears.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cboYears.Name = "cboYears";
            this.cboYears.Size = new System.Drawing.Size(177, 29);
            this.cboYears.TabIndex = 53;
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
            this.cboMonth.Location = new System.Drawing.Point(18, 40);
            this.cboMonth.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cboMonth.Name = "cboMonth";
            this.cboMonth.Size = new System.Drawing.Size(137, 29);
            this.cboMonth.TabIndex = 52;
            // 
            // tblGraph
            // 
            this.tblGraph.AllowUserToAddRows = false;
            this.tblGraph.AllowUserToDeleteRows = false;
            this.tblGraph.AllowUserToResizeColumns = false;
            this.tblGraph.AllowUserToResizeRows = false;
            this.tblGraph.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.tblGraph.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tblGraph.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tblGraph.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblGraph.Location = new System.Drawing.Point(0, 85);
            this.tblGraph.Margin = new System.Windows.Forms.Padding(2);
            this.tblGraph.MultiSelect = false;
            this.tblGraph.Name = "tblGraph";
            this.tblGraph.ReadOnly = true;
            this.tblGraph.RowTemplate.Height = 24;
            this.tblGraph.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.tblGraph.Size = new System.Drawing.Size(600, 281);
            this.tblGraph.TabIndex = 15;
            this.tblGraph.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.TblGraph_CellPainting);
            this.tblGraph.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.TblGraph_DataBindingComplete);
            this.tblGraph.SelectionChanged += new System.EventHandler(this.TblGraph_SelectionChanged);
            this.tblGraph.SizeChanged += new System.EventHandler(this.TblGraph_SizeChanged);
            // 
            // cbYearAll
            // 
            this.cbYearAll.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbYearAll.AutoSize = true;
            this.cbYearAll.BackColor = System.Drawing.Color.Transparent;
            this.cbYearAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbYearAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbYearAll.Location = new System.Drawing.Point(192, 12);
            this.cbYearAll.Margin = new System.Windows.Forms.Padding(2);
            this.cbYearAll.Name = "cbYearAll";
            this.cbYearAll.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbYearAll.Size = new System.Drawing.Size(42, 23);
            this.cbYearAll.TabIndex = 62;
            this.cbYearAll.Text = "Chec";
            this.cbYearAll.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbYearAll.UseVisualStyleBackColor = false;
            this.cbYearAll.CheckedChanged += new System.EventHandler(this.CbYearAll_CheckedChanged);
            // 
            // LineGraph
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 366);
            this.Controls.Add(this.tblGraph);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "LineGraph";
            this.Text = "LineGraph";
            this.Resize += new System.EventHandler(this.LineGraph_Resize);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tblGraph)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboYears;
        private System.Windows.Forms.ComboBox cboMonth;
        private TableView tblGraph;
        private System.Windows.Forms.Button btnZoomOut;
        private System.Windows.Forms.Button btnZoomIn;
        private MyCheckBox cbYearAll;
    }
}