namespace ganntproj1
{
    partial class FrmCarico
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
            this.lblCommTit = new System.Windows.Forms.Label();
            this.pnHeader = new System.Windows.Forms.Panel();
            this.lblClose = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dgvCarico = new System.Windows.Forms.DataGridView();
            this.dgvDistinta = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.lblComTxt = new System.Windows.Forms.Label();
            this.pnHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCarico)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDistinta)).BeginInit();
            this.SuspendLayout();
            // 
            // lblCommTit
            // 
            this.lblCommTit.BackColor = System.Drawing.Color.White;
            this.lblCommTit.Font = new System.Drawing.Font("Tahoma", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCommTit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(98)))), ((int)(((byte)(124)))));
            this.lblCommTit.Location = new System.Drawing.Point(14, 34);
            this.lblCommTit.Name = "lblCommTit";
            this.lblCommTit.Size = new System.Drawing.Size(335, 41);
            this.lblCommTit.TabIndex = 24;
            this.lblCommTit.Text = "Situazione del carico ";
            this.lblCommTit.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.lblCommTit.Paint += new System.Windows.Forms.PaintEventHandler(this.LblCommTit_Paint);
            // 
            // pnHeader
            // 
            this.pnHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(98)))), ((int)(((byte)(124)))));
            this.pnHeader.Controls.Add(this.lblClose);
            this.pnHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnHeader.Location = new System.Drawing.Point(0, 0);
            this.pnHeader.Name = "pnHeader";
            this.pnHeader.Size = new System.Drawing.Size(1064, 70);
            this.pnHeader.TabIndex = 25;
            // 
            // lblClose
            // 
            this.lblClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClose.ForeColor = System.Drawing.Color.White;
            this.lblClose.Location = new System.Drawing.Point(992, 12);
            this.lblClose.Name = "lblClose";
            this.lblClose.Size = new System.Drawing.Size(60, 44);
            this.lblClose.TabIndex = 0;
            this.lblClose.Text = "X";
            this.lblClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblClose.Click += new System.EventHandler(this.LblClose_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(56, 310);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 21);
            this.label1.TabIndex = 26;
            this.label1.Text = "Distinta";
            // 
            // dgvCarico
            // 
            this.dgvCarico.AllowUserToAddRows = false;
            this.dgvCarico.AllowUserToDeleteRows = false;
            this.dgvCarico.AllowUserToResizeColumns = false;
            this.dgvCarico.AllowUserToResizeRows = false;
            this.dgvCarico.BackgroundColor = System.Drawing.Color.White;
            this.dgvCarico.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvCarico.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvCarico.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvCarico.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvCarico.ColumnHeadersHeight = 35;
            this.dgvCarico.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvCarico.GridColor = System.Drawing.Color.White;
            this.dgvCarico.Location = new System.Drawing.Point(60, 115);
            this.dgvCarico.Name = "dgvCarico";
            this.dgvCarico.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvCarico.RowHeadersVisible = false;
            this.dgvCarico.RowTemplate.Height = 24;
            this.dgvCarico.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCarico.Size = new System.Drawing.Size(500, 178);
            this.dgvCarico.TabIndex = 27;
            // 
            // dgvDistinta
            // 
            this.dgvDistinta.AllowUserToAddRows = false;
            this.dgvDistinta.AllowUserToDeleteRows = false;
            this.dgvDistinta.AllowUserToResizeColumns = false;
            this.dgvDistinta.AllowUserToResizeRows = false;
            this.dgvDistinta.BackgroundColor = System.Drawing.Color.White;
            this.dgvDistinta.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvDistinta.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvDistinta.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDistinta.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvDistinta.ColumnHeadersHeight = 35;
            this.dgvDistinta.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvDistinta.GridColor = System.Drawing.Color.White;
            this.dgvDistinta.Location = new System.Drawing.Point(60, 339);
            this.dgvDistinta.Name = "dgvDistinta";
            this.dgvDistinta.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvDistinta.RowHeadersVisible = false;
            this.dgvDistinta.RowTemplate.Height = 24;
            this.dgvDistinta.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDistinta.Size = new System.Drawing.Size(945, 210);
            this.dgvDistinta.TabIndex = 28;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(742, 115);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 18);
            this.label2.TabIndex = 29;
            this.label2.Text = "Commessa";
            // 
            // lblComTxt
            // 
            this.lblComTxt.AutoSize = true;
            this.lblComTxt.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblComTxt.Location = new System.Drawing.Point(742, 148);
            this.lblComTxt.Name = "lblComTxt";
            this.lblComTxt.Size = new System.Drawing.Size(0, 29);
            this.lblComTxt.TabIndex = 30;
            // 
            // FrmCarico
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1064, 586);
            this.ControlBox = false;
            this.Controls.Add(this.lblComTxt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dgvDistinta);
            this.Controls.Add(this.dgvCarico);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblCommTit);
            this.Controls.Add(this.pnHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmCarico";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FrmCarico";
            this.pnHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCarico)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDistinta)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblCommTit;
        private System.Windows.Forms.Panel pnHeader;
        private System.Windows.Forms.Label lblClose;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgvCarico;
        private System.Windows.Forms.DataGridView dgvDistinta;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblComTxt;
    }
}