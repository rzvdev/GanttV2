namespace ganntproj1.Views
{
    partial class GraficoRespinte
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tblRespinte = new ganntproj1.TableView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.pnGraphs = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbAbatim = new ganntproj1.ToggleCheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cbCom = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbAr = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbLin = new System.Windows.Forms.ComboBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tblRespinte)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuBar;
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("Bahnschrift", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(0, 72);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.Padding = new System.Drawing.Point(6, 10);
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.ShowToolTips = true;
            this.tabControl1.Size = new System.Drawing.Size(800, 378);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tblRespinte);
            this.tabPage1.Location = new System.Drawing.Point(4, 39);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(792, 335);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Situazione controllo commesse";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tblRespinte
            // 
            this.tblRespinte.AllowUserToAddRows = false;
            this.tblRespinte.AllowUserToDeleteRows = false;
            this.tblRespinte.AllowUserToResizeColumns = false;
            this.tblRespinte.AllowUserToResizeRows = false;
            this.tblRespinte.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.tblRespinte.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tblRespinte.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tblRespinte.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblRespinte.EnableHeadersVisualStyles = false;
            this.tblRespinte.Location = new System.Drawing.Point(3, 3);
            this.tblRespinte.Margin = new System.Windows.Forms.Padding(2);
            this.tblRespinte.MultiSelect = false;
            this.tblRespinte.Name = "tblRespinte";
            this.tblRespinte.ReadOnly = true;
            this.tblRespinte.RowTemplate.Height = 24;
            this.tblRespinte.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.tblRespinte.Size = new System.Drawing.Size(786, 329);
            this.tblRespinte.TabIndex = 16;
            this.tblRespinte.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.tblRespinte_CellDoubleClick);
            this.tblRespinte.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.tblRespinte_CellMouseClick);
            this.tblRespinte.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.tblRespinte_DataBindingComplete);
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.White;
            this.tabPage2.Controls.Add(this.pnGraphs);
            this.tabPage2.Location = new System.Drawing.Point(4, 39);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(792, 349);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Fino giorno attuale";
            // 
            // pnGraphs
            // 
            this.pnGraphs.AutoScroll = true;
            this.pnGraphs.AutoScrollMargin = new System.Drawing.Size(10, 10);
            this.pnGraphs.BackColor = System.Drawing.Color.White;
            this.pnGraphs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnGraphs.Location = new System.Drawing.Point(3, 3);
            this.pnGraphs.Name = "pnGraphs";
            this.pnGraphs.Size = new System.Drawing.Size(786, 343);
            this.pnGraphs.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.cbLin);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.cbCom);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.cbAr);
            this.panel1.Controls.Add(this.cbAbatim);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 72);
            this.panel1.TabIndex = 16;
            // 
            // cbAbatim
            // 
            this.cbAbatim.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbAbatim.BackColor = System.Drawing.Color.Transparent;
            this.cbAbatim.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbAbatim.Location = new System.Drawing.Point(119, 28);
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
            this.label7.Location = new System.Drawing.Point(19, 34);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(92, 13);
            this.label7.TabIndex = 41;
            this.label7.Text = "Commesse chiuse";
            // 
            // label5
            // 
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label5.Location = new System.Drawing.Point(188, 22);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(2, 34);
            this.label5.TabIndex = 69;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(222, 13);
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
            this.cbCom.Location = new System.Drawing.Point(224, 31);
            this.cbCom.Margin = new System.Windows.Forms.Padding(2);
            this.cbCom.Name = "cbCom";
            this.cbCom.Size = new System.Drawing.Size(113, 23);
            this.cbCom.TabIndex = 67;
            this.cbCom.SelectedIndexChanged += new System.EventHandler(this.cbCom_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(338, 13);
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
            this.cbAr.Location = new System.Drawing.Point(341, 31);
            this.cbAr.Margin = new System.Windows.Forms.Padding(2);
            this.cbAr.Name = "cbAr";
            this.cbAr.Size = new System.Drawing.Size(131, 23);
            this.cbAr.TabIndex = 65;
            this.cbAr.SelectedIndexChanged += new System.EventHandler(this.cbAr_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(473, 13);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 71;
            this.label1.Text = "Linea";
            // 
            // cbLin
            // 
            this.cbLin.BackColor = System.Drawing.Color.White;
            this.cbLin.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbLin.FormattingEnabled = true;
            this.cbLin.Location = new System.Drawing.Point(476, 31);
            this.cbLin.Margin = new System.Windows.Forms.Padding(2);
            this.cbLin.Name = "cbLin";
            this.cbLin.Size = new System.Drawing.Size(95, 23);
            this.cbLin.TabIndex = 70;
            this.cbLin.SelectedIndexChanged += new System.EventHandler(this.cbLin_SelectedIndexChanged);
            // 
            // GraficoRespinte
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel1);
            this.Name = "GraficoRespinte";
            this.Text = "GraficoRespinte";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tblRespinte)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private TableView tblRespinte;
        private System.Windows.Forms.Panel panel1;
        private ToggleCheckBox cbAbatim;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel pnGraphs;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbCom;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbAr;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbLin;
    }
}