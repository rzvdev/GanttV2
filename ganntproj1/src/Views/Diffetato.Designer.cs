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
            this.panel1.Controls.Add(this.cbAbatim);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(797, 72);
            this.panel1.TabIndex = 16;
            // 
            // cbAbatim
            // 
            this.cbAbatim.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbAbatim.BackColor = System.Drawing.Color.Transparent;
            this.cbAbatim.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbAbatim.Location = new System.Drawing.Point(124, 24);
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
            this.label7.Location = new System.Drawing.Point(24, 30);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(91, 13);
            this.label7.TabIndex = 41;
            this.label7.Text = "Commesse aperte";
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
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TableView tableView1;
        private System.Windows.Forms.Panel panel1;
        private ToggleCheckBox cbAbatim;
        private System.Windows.Forms.Label label7;
    }
}