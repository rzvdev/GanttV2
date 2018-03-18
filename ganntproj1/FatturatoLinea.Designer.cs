namespace ganntproj1
{
    partial class FatturatoLinea
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FatturatoLinea));
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cboYears = new System.Windows.Forms.ComboBox();
            this.cboMonth = new System.Windows.Forms.ComboBox();
            this.tableView1 = new ganntproj1.TableView();
            this.cbYearAll = new ganntproj1.MyCheckBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tableView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cbYearAll);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.cboYears);
            this.panel1.Controls.Add(this.cboMonth);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 85);
            this.panel1.TabIndex = 14;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(152, 19);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 55;
            this.label2.Text = "Year";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 19);
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
            this.cboYears.Location = new System.Drawing.Point(154, 41);
            this.cboYears.Margin = new System.Windows.Forms.Padding(2);
            this.cboYears.Name = "cboYears";
            this.cboYears.Size = new System.Drawing.Size(92, 29);
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
            this.cboMonth.Location = new System.Drawing.Point(14, 41);
            this.cboMonth.Margin = new System.Windows.Forms.Padding(2);
            this.cboMonth.Name = "cboMonth";
            this.cboMonth.Size = new System.Drawing.Size(137, 29);
            this.cboMonth.TabIndex = 52;
            // 
            // tableView1
            // 
            this.tableView1.AllowUserToAddRows = false;
            this.tableView1.AllowUserToDeleteRows = false;
            this.tableView1.AllowUserToResizeColumns = false;
            this.tableView1.AllowUserToResizeRows = false;
            this.tableView1.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.tableView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tableView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tableView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableView1.Location = new System.Drawing.Point(0, 85);
            this.tableView1.MultiSelect = false;
            this.tableView1.Name = "tableView1";
            this.tableView1.ReadOnly = true;
            this.tableView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.tableView1.Size = new System.Drawing.Size(800, 365);
            this.tableView1.TabIndex = 15;
            // 
            // cbYearAll
            // 
            this.cbYearAll.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbYearAll.AutoSize = true;
            this.cbYearAll.BackColor = System.Drawing.Color.Transparent;
            this.cbYearAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbYearAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbYearAll.Location = new System.Drawing.Point(187, 14);
            this.cbYearAll.Margin = new System.Windows.Forms.Padding(2);
            this.cbYearAll.Name = "cbYearAll";
            this.cbYearAll.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbYearAll.Size = new System.Drawing.Size(42, 23);
            this.cbYearAll.TabIndex = 63;
            this.cbYearAll.Text = "Chec";
            this.cbYearAll.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbYearAll.UseVisualStyleBackColor = false;
            this.cbYearAll.CheckedChanged += new System.EventHandler(this.CbYearAll_CheckedChanged);
            // 
            // FatturatoLinea
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tableView1);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FatturatoLinea";
            this.Text = "Fatturato per linea";
            this.Load += new System.EventHandler(this.FatturatoLinea_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tableView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboYears;
        private System.Windows.Forms.ComboBox cboMonth;
        private TableView tableView1;
        private MyCheckBox cbYearAll;
    }
}