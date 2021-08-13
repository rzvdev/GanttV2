namespace ganntproj1
{
    partial class Mensile
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Mensile));
            this.tableView1 = new ganntproj1.TableView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.toggleCheckBox1 = new ganntproj1.ToggleCheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cboYears = new System.Windows.Forms.ComboBox();
            this.cboMonth = new System.Windows.Forms.ComboBox();
            this.cbAbatim = new ganntproj1.ToggleCheckBox();
            this.label7 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.tableView1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
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
            this.tableView1.Location = new System.Drawing.Point(0, 72);
            this.tableView1.Margin = new System.Windows.Forms.Padding(2);
            this.tableView1.MultiSelect = false;
            this.tableView1.Name = "tableView1";
            this.tableView1.ReadOnly = true;
            this.tableView1.RowTemplate.Height = 24;
            this.tableView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.tableView1.Size = new System.Drawing.Size(794, 425);
            this.tableView1.TabIndex = 0;
            this.tableView1.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.TableView1_CellPainting);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel1.Controls.Add(this.toggleCheckBox1);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.cboYears);
            this.panel1.Controls.Add(this.cboMonth);
            this.panel1.Controls.Add(this.cbAbatim);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(794, 72);
            this.panel1.TabIndex = 13;
            // 
            // toggleCheckBox1
            // 
            this.toggleCheckBox1.Appearance = System.Windows.Forms.Appearance.Button;
            this.toggleCheckBox1.BackColor = System.Drawing.Color.Transparent;
            this.toggleCheckBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.toggleCheckBox1.Location = new System.Drawing.Point(479, 23);
            this.toggleCheckBox1.Margin = new System.Windows.Forms.Padding(2);
            this.toggleCheckBox1.Name = "toggleCheckBox1";
            this.toggleCheckBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.toggleCheckBox1.Size = new System.Drawing.Size(42, 24);
            this.toggleCheckBox1.TabIndex = 57;
            this.toggleCheckBox1.Text = "Chec";
            this.toggleCheckBox1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toggleCheckBox1.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(445, 29);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 13);
            this.label3.TabIndex = 56;
            this.label3.Text = "ORE";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(152, 3);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 55;
            this.label2.Text = "Year";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 3);
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
            this.cboYears.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboYears.FormattingEnabled = true;
            this.cboYears.Location = new System.Drawing.Point(154, 21);
            this.cboYears.Margin = new System.Windows.Forms.Padding(2);
            this.cboYears.Name = "cboYears";
            this.cboYears.Size = new System.Drawing.Size(92, 28);
            this.cboYears.TabIndex = 53;
            // 
            // cboMonth
            // 
            this.cboMonth.BackColor = System.Drawing.Color.White;
            this.cboMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMonth.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this.cboMonth.Location = new System.Drawing.Point(14, 21);
            this.cboMonth.Margin = new System.Windows.Forms.Padding(2);
            this.cboMonth.Name = "cboMonth";
            this.cboMonth.Size = new System.Drawing.Size(137, 28);
            this.cboMonth.TabIndex = 52;
            // 
            // cbAbatim
            // 
            this.cbAbatim.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbAbatim.BackColor = System.Drawing.Color.Transparent;
            this.cbAbatim.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbAbatim.Location = new System.Drawing.Point(357, 23);
            this.cbAbatim.Margin = new System.Windows.Forms.Padding(2);
            this.cbAbatim.Name = "cbAbatim";
            this.cbAbatim.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbAbatim.Size = new System.Drawing.Size(42, 24);
            this.cbAbatim.TabIndex = 42;
            this.cbAbatim.Text = "Chec";
            this.cbAbatim.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbAbatim.UseVisualStyleBackColor = true;
            this.cbAbatim.CheckedChanged += new System.EventHandler(this.CbAcconto_CheckedChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Location = new System.Drawing.Point(261, 29);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(89, 13);
            this.label7.TabIndex = 41;
            this.label7.Text = "Abatimento 100%";
            // 
            // Mensile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(794, 497);
            this.Controls.Add(this.tableView1);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Mensile";
            ((System.ComponentModel.ISupportInitialize)(this.tableView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TableView tableView1;
        private System.Windows.Forms.Panel panel1;
        private ToggleCheckBox cbAbatim;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboYears;
        private System.Windows.Forms.ComboBox cboMonth;
        private ToggleCheckBox toggleCheckBox1;
        private System.Windows.Forms.Label label3;
    }
}