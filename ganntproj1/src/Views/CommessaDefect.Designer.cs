namespace ganntproj1.Views
{
    partial class CommessaDefect
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.cboYears = new System.Windows.Forms.ComboBox();
            this.cbAbatim = new ganntproj1.ToggleCheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tableView1 = new ganntproj1.TableView();
            this.label1 = new System.Windows.Forms.Label();
            this.cboMonth = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbCom = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbAr = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tableView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.cbCom);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.cbAr);
            this.panel1.Controls.Add(this.cboMonth);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.cboYears);
            this.panel1.Controls.Add(this.cbAbatim);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 72);
            this.panel1.TabIndex = 15;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(164, 13);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 57;
            this.label2.Text = "Year";
            // 
            // cboYears
            // 
            this.cboYears.BackColor = System.Drawing.Color.White;
            this.cboYears.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboYears.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboYears.FormattingEnabled = true;
            this.cboYears.Location = new System.Drawing.Point(166, 31);
            this.cboYears.Margin = new System.Windows.Forms.Padding(2);
            this.cboYears.Name = "cboYears";
            this.cboYears.Size = new System.Drawing.Size(92, 28);
            this.cboYears.TabIndex = 56;
            // 
            // cbAbatim
            // 
            this.cbAbatim.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbAbatim.BackColor = System.Drawing.Color.Transparent;
            this.cbAbatim.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbAbatim.Location = new System.Drawing.Point(383, 32);
            this.cbAbatim.Margin = new System.Windows.Forms.Padding(2);
            this.cbAbatim.Name = "cbAbatim";
            this.cbAbatim.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbAbatim.Size = new System.Drawing.Size(42, 24);
            this.cbAbatim.TabIndex = 42;
            this.cbAbatim.Text = "Chec";
            this.cbAbatim.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbAbatim.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Location = new System.Drawing.Point(283, 38);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(91, 13);
            this.label7.TabIndex = 41;
            this.label7.Text = "Commesse aperte";
            // 
            // tableView1
            // 
            this.tableView1.AllowUserToAddRows = false;
            this.tableView1.AllowUserToDeleteRows = false;
            this.tableView1.AllowUserToResizeColumns = false;
            this.tableView1.AllowUserToResizeRows = false;
            this.tableView1.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.tableView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tableView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tableView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.tableView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tableView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableView1.EnableHeadersVisualStyles = false;
            this.tableView1.Location = new System.Drawing.Point(0, 72);
            this.tableView1.Margin = new System.Windows.Forms.Padding(2);
            this.tableView1.MultiSelect = false;
            this.tableView1.Name = "tableView1";
            this.tableView1.ReadOnly = true;
            this.tableView1.RowTemplate.Height = 24;
            this.tableView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.tableView1.Size = new System.Drawing.Size(800, 378);
            this.tableView1.TabIndex = 14;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 13);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 59;
            this.label1.Text = "Month";
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
            this.cboMonth.Location = new System.Drawing.Point(23, 31);
            this.cboMonth.Margin = new System.Windows.Forms.Padding(2);
            this.cboMonth.Name = "cboMonth";
            this.cboMonth.Size = new System.Drawing.Size(137, 28);
            this.cboMonth.TabIndex = 58;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(487, 17);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 63;
            this.label3.Text = "Commessa";
            // 
            // cbCom
            // 
            this.cbCom.BackColor = System.Drawing.Color.White;
            this.cbCom.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbCom.FormattingEnabled = true;
            this.cbCom.Location = new System.Drawing.Point(489, 35);
            this.cbCom.Margin = new System.Windows.Forms.Padding(2);
            this.cbCom.Name = "cbCom";
            this.cbCom.Size = new System.Drawing.Size(113, 23);
            this.cbCom.TabIndex = 62;
            this.cbCom.SelectedIndexChanged += new System.EventHandler(this.cbCom_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(603, 17);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 61;
            this.label4.Text = "Articoli";
            // 
            // cbAr
            // 
            this.cbAr.BackColor = System.Drawing.Color.White;
            this.cbAr.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbAr.FormattingEnabled = true;
            this.cbAr.Location = new System.Drawing.Point(606, 35);
            this.cbAr.Margin = new System.Windows.Forms.Padding(2);
            this.cbAr.Name = "cbAr";
            this.cbAr.Size = new System.Drawing.Size(131, 23);
            this.cbAr.TabIndex = 60;
            this.cbAr.SelectedIndexChanged += new System.EventHandler(this.cbAr_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label5.Location = new System.Drawing.Point(454, 26);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(2, 34);
            this.label5.TabIndex = 64;
            // 
            // CommessaDefect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tableView1);
            this.Controls.Add(this.panel1);
            this.Name = "CommessaDefect";
            this.Text = "CommessaDefect";
            this.Load += new System.EventHandler(this.CommessaDefect_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tableView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private TableView tableView1;
        private System.Windows.Forms.Panel panel1;
        private ToggleCheckBox cbAbatim;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboYears;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboMonth;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbCom;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbAr;
        private System.Windows.Forms.Label label5;
    }
}