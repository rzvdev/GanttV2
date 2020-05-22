namespace ganntproj1.Views
{
    partial class LineGraphMonth
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
            this.pnData = new System.Windows.Forms.Panel();
            this.lblDept = new System.Windows.Forms.Label();
            this.lblLine = new System.Windows.Forms.Label();
            this.zedGraph = new ZedGraph.ZedGraphControl();
            this.lblMedia = new System.Windows.Forms.Label();
            this.pnData.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnData
            // 
            this.pnData.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnData.Controls.Add(this.lblMedia);
            this.pnData.Controls.Add(this.lblDept);
            this.pnData.Controls.Add(this.lblLine);
            this.pnData.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnData.Location = new System.Drawing.Point(0, 0);
            this.pnData.Name = "pnData";
            this.pnData.Size = new System.Drawing.Size(1119, 73);
            this.pnData.TabIndex = 0;
            // 
            // lblDept
            // 
            this.lblDept.AutoSize = true;
            this.lblDept.Font = new System.Drawing.Font("Bahnschrift", 12F);
            this.lblDept.Location = new System.Drawing.Point(13, 44);
            this.lblDept.Name = "lblDept";
            this.lblDept.Size = new System.Drawing.Size(54, 19);
            this.lblDept.TabIndex = 1;
            this.lblDept.Text = "label2";
            // 
            // lblLine
            // 
            this.lblLine.AutoSize = true;
            this.lblLine.Font = new System.Drawing.Font("Bahnschrift", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLine.Location = new System.Drawing.Point(11, 9);
            this.lblLine.Name = "lblLine";
            this.lblLine.Size = new System.Drawing.Size(74, 29);
            this.lblLine.TabIndex = 0;
            this.lblLine.Text = "label1";
            // 
            // zedGraph
            // 
            this.zedGraph.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.zedGraph.Location = new System.Drawing.Point(0, 73);
            this.zedGraph.Name = "zedGraph";
            this.zedGraph.ScrollGrace = 0D;
            this.zedGraph.ScrollMaxX = 0D;
            this.zedGraph.ScrollMaxY = 0D;
            this.zedGraph.ScrollMaxY2 = 0D;
            this.zedGraph.ScrollMinX = 0D;
            this.zedGraph.ScrollMinY = 0D;
            this.zedGraph.ScrollMinY2 = 0D;
            this.zedGraph.Size = new System.Drawing.Size(1119, 520);
            this.zedGraph.TabIndex = 1;
            this.zedGraph.UseExtendedPrintDialog = true;
            // 
            // lblMedia
            // 
            this.lblMedia.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblMedia.Font = new System.Drawing.Font("Bahnschrift", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMedia.ForeColor = System.Drawing.Color.Crimson;
            this.lblMedia.Location = new System.Drawing.Point(697, 0);
            this.lblMedia.Name = "lblMedia";
            this.lblMedia.Size = new System.Drawing.Size(420, 71);
            this.lblMedia.TabIndex = 2;
            this.lblMedia.Text = "label1";
            this.lblMedia.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LineGraphMonth
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1119, 593);
            this.Controls.Add(this.zedGraph);
            this.Controls.Add(this.pnData);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(900, 400);
            this.Name = "LineGraphMonth";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "LineGraphMonth";
            this.pnData.ResumeLayout(false);
            this.pnData.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnData;
        private System.Windows.Forms.Label lblDept;
        private System.Windows.Forms.Label lblLine;
        private ZedGraph.ZedGraphControl zedGraph;
        private System.Windows.Forms.Label lblMedia;
    }
}