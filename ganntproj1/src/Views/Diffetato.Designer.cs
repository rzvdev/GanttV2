namespace ganntproj1
{
    partial class Diffetato
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Diffetato));
            this.tableView1 = new ganntproj1.TableView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbAbatim = new ganntproj1.ToggleCheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cbCom = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbAr = new System.Windows.Forms.ComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.tableView1)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
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
            this.tableView1.GridColor = System.Drawing.Color.White;
            this.tableView1.Location = new System.Drawing.Point(0, 72);
            this.tableView1.Margin = new System.Windows.Forms.Padding(2);
            this.tableView1.MultiSelect = false;
            this.tableView1.Name = "tableView1";
            this.tableView1.ReadOnly = true;
            this.tableView1.RowTemplate.Height = 24;
            this.tableView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.tableView1.Size = new System.Drawing.Size(797, 443);
            this.tableView1.TabIndex = 0;
            this.tableView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.tableView1_CellDoubleClick);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(797, 72);
            this.panel1.TabIndex = 16;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // cbAbatim
            // 
            this.cbAbatim.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbAbatim.BackColor = System.Drawing.Color.Transparent;
            this.cbAbatim.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbAbatim.Location = new System.Drawing.Point(121, 27);
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
            this.label7.Location = new System.Drawing.Point(21, 33);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(91, 13);
            this.label7.TabIndex = 41;
            this.label7.Text = "Commesse aperte";
            // 
            // label5
            // 
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label5.Location = new System.Drawing.Point(186, 22);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(2, 34);
            this.label5.TabIndex = 69;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 13);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 68;
            this.label3.Text = "Commessa";
            // 
            // cbCom
            // 
            this.cbCom.BackColor = System.Drawing.Color.White;
            this.cbCom.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbCom.FormattingEnabled = true;
            this.cbCom.Location = new System.Drawing.Point(17, 31);
            this.cbCom.Margin = new System.Windows.Forms.Padding(2);
            this.cbCom.Name = "cbCom";
            this.cbCom.Size = new System.Drawing.Size(113, 23);
            this.cbCom.TabIndex = 67;
            this.cbCom.SelectedIndexChanged += new System.EventHandler(this.cbCom_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(131, 13);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 66;
            this.label4.Text = "Articoli";
            // 
            // cbAr
            // 
            this.cbAr.BackColor = System.Drawing.Color.White;
            this.cbAr.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbAr.FormattingEnabled = true;
            this.cbAr.Location = new System.Drawing.Point(134, 31);
            this.cbAr.Margin = new System.Windows.Forms.Padding(2);
            this.cbAr.Name = "cbAr";
            this.cbAr.Size = new System.Drawing.Size(131, 23);
            this.cbAr.TabIndex = 65;
            this.cbAr.SelectedIndexChanged += new System.EventHandler(this.cbAr_SelectedIndexChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.cbAbatim);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(200, 72);
            this.panel2.TabIndex = 70;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.cbAr);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.cbCom);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(200, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(282, 72);
            this.panel3.TabIndex = 71;
            // 
            // Diffetato
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(797, 515);
            this.Controls.Add(this.tableView1);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Diffetato";
            this.Text = "Diffetato";
            ((System.ComponentModel.ISupportInitialize)(this.tableView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TableView tableView1;
        private System.Windows.Forms.Panel panel1;
        private ToggleCheckBox cbAbatim;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbCom;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbAr;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
    }
}