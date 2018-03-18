namespace ganntproj1
    {
    partial class WorkflowController
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WorkflowController));
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.btnHideDelay = new System.Windows.Forms.Button();
            this.btnDayInfo = new System.Windows.Forms.Button();
            this.btnSchedule = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnZoomOut = new System.Windows.Forms.Button();
            this.cbComm = new System.Windows.Forms.ComboBox();
            this.cbArt = new System.Windows.Forms.ComboBox();
            this.btnZoomIn = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnMegaBack = new System.Windows.Forms.Button();
            this.btnCallCarico = new System.Windows.Forms.Button();
            this.btnFow = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.btnMegaFow = new System.Windows.Forms.Button();
            this.cbHold = new ganntproj1.MyCheckBox();
            this.cbTree = new ganntproj1.MyCheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.spContainer = new System.Windows.Forms.SplitContainer();
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spContainer)).BeginInit();
            this.spContainer.Panel1.SuspendLayout();
            this.spContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // vScrollBar1
            // 
            this.vScrollBar1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.vScrollBar1.Dock = System.Windows.Forms.DockStyle.Right;
            this.vScrollBar1.Location = new System.Drawing.Point(1391, 0);
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new System.Drawing.Size(21, 154);
            this.vScrollBar1.TabIndex = 0;
            this.vScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar1_Scroll);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.btnHideDelay);
            this.panel1.Controls.Add(this.btnDayInfo);
            this.panel1.Controls.Add(this.btnSchedule);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.btnZoomOut);
            this.panel1.Controls.Add(this.cbComm);
            this.panel1.Controls.Add(this.cbArt);
            this.panel1.Controls.Add(this.btnZoomIn);
            this.panel1.Controls.Add(this.btnBack);
            this.panel1.Controls.Add(this.btnMegaBack);
            this.panel1.Controls.Add(this.btnCallCarico);
            this.panel1.Controls.Add(this.btnFow);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.btnMegaFow);
            this.panel1.Controls.Add(this.cbHold);
            this.panel1.Controls.Add(this.cbTree);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1414, 59);
            this.panel1.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Image = global::ganntproj1.Properties.Resources.check_list_32;
            this.button1.Location = new System.Drawing.Point(671, 10);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(37, 40);
            this.button1.TabIndex = 66;
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // btnHideDelay
            // 
            this.btnHideDelay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
            this.btnHideDelay.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnHideDelay.FlatAppearance.BorderSize = 0;
            this.btnHideDelay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHideDelay.Image = global::ganntproj1.Properties.Resources.clear_icon_32;
            this.btnHideDelay.Location = new System.Drawing.Point(631, 10);
            this.btnHideDelay.Margin = new System.Windows.Forms.Padding(2);
            this.btnHideDelay.Name = "btnHideDelay";
            this.btnHideDelay.Size = new System.Drawing.Size(37, 40);
            this.btnHideDelay.TabIndex = 65;
            this.btnHideDelay.UseVisualStyleBackColor = false;
            this.btnHideDelay.Click += new System.EventHandler(this.btnHideDelay_Click);
            // 
            // btnDayInfo
            // 
            this.btnDayInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
            this.btnDayInfo.FlatAppearance.BorderSize = 0;
            this.btnDayInfo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDayInfo.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.btnDayInfo.ForeColor = System.Drawing.Color.Black;
            this.btnDayInfo.Image = global::ganntproj1.Properties.Resources.close_icon_small_32;
            this.btnDayInfo.Location = new System.Drawing.Point(586, 10);
            this.btnDayInfo.Margin = new System.Windows.Forms.Padding(2);
            this.btnDayInfo.Name = "btnDayInfo";
            this.btnDayInfo.Size = new System.Drawing.Size(40, 40);
            this.btnDayInfo.TabIndex = 64;
            this.btnDayInfo.UseVisualStyleBackColor = false;
            this.btnDayInfo.Click += new System.EventHandler(this.btnDayInfo_Click);
            // 
            // btnSchedule
            // 
            this.btnSchedule.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
            this.btnSchedule.FlatAppearance.BorderSize = 0;
            this.btnSchedule.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSchedule.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.btnSchedule.ForeColor = System.Drawing.Color.Black;
            this.btnSchedule.Image = global::ganntproj1.Properties.Resources.schedule_24;
            this.btnSchedule.Location = new System.Drawing.Point(542, 10);
            this.btnSchedule.Margin = new System.Windows.Forms.Padding(2);
            this.btnSchedule.Name = "btnSchedule";
            this.btnSchedule.Size = new System.Drawing.Size(40, 40);
            this.btnSchedule.TabIndex = 63;
            this.btnSchedule.UseVisualStyleBackColor = false;
            this.btnSchedule.Click += new System.EventHandler(this.btnSchedule_Click);
            // 
            // label8
            // 
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label8.Location = new System.Drawing.Point(961, 19);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(2, 24);
            this.label8.TabIndex = 62;
            // 
            // label7
            // 
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label7.Location = new System.Drawing.Point(719, 19);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(2, 24);
            this.label7.TabIndex = 61;
            // 
            // label5
            // 
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label5.Location = new System.Drawing.Point(283, 19);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(2, 24);
            this.label5.TabIndex = 60;
            // 
            // btnZoomOut
            // 
            this.btnZoomOut.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
            this.btnZoomOut.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnZoomOut.FlatAppearance.BorderSize = 0;
            this.btnZoomOut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnZoomOut.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnZoomOut.Image = global::ganntproj1.Properties.Resources.zoom_out_d32;
            this.btnZoomOut.Location = new System.Drawing.Point(502, 10);
            this.btnZoomOut.Margin = new System.Windows.Forms.Padding(2);
            this.btnZoomOut.Name = "btnZoomOut";
            this.btnZoomOut.Size = new System.Drawing.Size(37, 40);
            this.btnZoomOut.TabIndex = 58;
            this.btnZoomOut.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.btnZoomOut.UseVisualStyleBackColor = false;
            this.btnZoomOut.Click += new System.EventHandler(this.btnZoomOut_Click);
            // 
            // cbComm
            // 
            this.cbComm.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbComm.FormattingEnabled = true;
            this.cbComm.Location = new System.Drawing.Point(733, 18);
            this.cbComm.Margin = new System.Windows.Forms.Padding(2);
            this.cbComm.Name = "cbComm";
            this.cbComm.Size = new System.Drawing.Size(92, 25);
            this.cbComm.TabIndex = 52;
            this.cbComm.SelectedIndexChanged += new System.EventHandler(this.cbComm_SelectedIndexChanged_1);
            // 
            // cbArt
            // 
            this.cbArt.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbArt.FormattingEnabled = true;
            this.cbArt.Location = new System.Drawing.Point(829, 18);
            this.cbArt.Margin = new System.Windows.Forms.Padding(2);
            this.cbArt.Name = "cbArt";
            this.cbArt.Size = new System.Drawing.Size(122, 25);
            this.cbArt.TabIndex = 49;
            this.cbArt.SelectedIndexChanged += new System.EventHandler(this.cbArt_SelectedIndexChanged);
            // 
            // btnZoomIn
            // 
            this.btnZoomIn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
            this.btnZoomIn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnZoomIn.FlatAppearance.BorderSize = 0;
            this.btnZoomIn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnZoomIn.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnZoomIn.Image = global::ganntproj1.Properties.Resources.zoom_in_d32;
            this.btnZoomIn.Location = new System.Drawing.Point(461, 10);
            this.btnZoomIn.Margin = new System.Windows.Forms.Padding(2);
            this.btnZoomIn.Name = "btnZoomIn";
            this.btnZoomIn.Size = new System.Drawing.Size(37, 40);
            this.btnZoomIn.TabIndex = 57;
            this.btnZoomIn.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.btnZoomIn.UseVisualStyleBackColor = false;
            this.btnZoomIn.Click += new System.EventHandler(this.btnZoomIn_Click);
            // 
            // btnBack
            // 
            this.btnBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
            this.btnBack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnBack.FlatAppearance.BorderSize = 0;
            this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBack.Image = global::ganntproj1.Properties.Resources.arrow_back_32;
            this.btnBack.Location = new System.Drawing.Point(299, 10);
            this.btnBack.Margin = new System.Windows.Forms.Padding(2);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(37, 40);
            this.btnBack.TabIndex = 53;
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // btnMegaBack
            // 
            this.btnMegaBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
            this.btnMegaBack.FlatAppearance.BorderSize = 0;
            this.btnMegaBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMegaBack.Image = global::ganntproj1.Properties.Resources.arrow_mback_32;
            this.btnMegaBack.Location = new System.Drawing.Point(340, 10);
            this.btnMegaBack.Margin = new System.Windows.Forms.Padding(2);
            this.btnMegaBack.Name = "btnMegaBack";
            this.btnMegaBack.Size = new System.Drawing.Size(37, 40);
            this.btnMegaBack.TabIndex = 54;
            this.btnMegaBack.UseVisualStyleBackColor = false;
            this.btnMegaBack.Click += new System.EventHandler(this.btnMegaBack_Click);
            // 
            // btnCallCarico
            // 
            this.btnCallCarico.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnCallCarico.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.btnCallCarico.ForeColor = System.Drawing.Color.Black;
            this.btnCallCarico.Location = new System.Drawing.Point(972, 15);
            this.btnCallCarico.Margin = new System.Windows.Forms.Padding(2);
            this.btnCallCarico.Name = "btnCallCarico";
            this.btnCallCarico.Size = new System.Drawing.Size(114, 31);
            this.btnCallCarico.TabIndex = 43;
            this.btnCallCarico.Text = "Carico lavoro";
            this.btnCallCarico.UseVisualStyleBackColor = false;
            this.btnCallCarico.Click += new System.EventHandler(this.btnCallCarico_Click);
            // 
            // btnFow
            // 
            this.btnFow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
            this.btnFow.FlatAppearance.BorderSize = 0;
            this.btnFow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFow.Image = global::ganntproj1.Properties.Resources.arrow_forw_32;
            this.btnFow.Location = new System.Drawing.Point(421, 10);
            this.btnFow.Margin = new System.Windows.Forms.Padding(2);
            this.btnFow.Name = "btnFow";
            this.btnFow.Size = new System.Drawing.Size(37, 40);
            this.btnFow.TabIndex = 56;
            this.btnFow.UseVisualStyleBackColor = false;
            this.btnFow.Click += new System.EventHandler(this.btnFow_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Enabled = false;
            this.label6.Location = new System.Drawing.Point(10, 24);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(54, 13);
            this.label6.TabIndex = 39;
            this.label6.Text = "Tree view";
            // 
            // btnMegaFow
            // 
            this.btnMegaFow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
            this.btnMegaFow.FlatAppearance.BorderSize = 0;
            this.btnMegaFow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMegaFow.Image = global::ganntproj1.Properties.Resources.arrow_mforw_32;
            this.btnMegaFow.Location = new System.Drawing.Point(380, 10);
            this.btnMegaFow.Margin = new System.Windows.Forms.Padding(2);
            this.btnMegaFow.Name = "btnMegaFow";
            this.btnMegaFow.Size = new System.Drawing.Size(37, 40);
            this.btnMegaFow.TabIndex = 55;
            this.btnMegaFow.UseVisualStyleBackColor = false;
            this.btnMegaFow.Click += new System.EventHandler(this.btnMegaFow_Click);
            // 
            // cbHold
            // 
            this.cbHold.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbHold.AutoSize = true;
            this.cbHold.BackColor = System.Drawing.Color.Transparent;
            this.cbHold.Enabled = false;
            this.cbHold.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbHold.Location = new System.Drawing.Point(226, 20);
            this.cbHold.Margin = new System.Windows.Forms.Padding(2);
            this.cbHold.Name = "cbHold";
            this.cbHold.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbHold.Size = new System.Drawing.Size(42, 23);
            this.cbHold.TabIndex = 42;
            this.cbHold.Text = "Chec";
            this.cbHold.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbHold.UseVisualStyleBackColor = true;
            this.cbHold.CheckedChanged += new System.EventHandler(this.myCheckBox1_CheckedChanged);
            // 
            // cbTree
            // 
            this.cbTree.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbTree.AutoSize = true;
            this.cbTree.BackColor = System.Drawing.Color.Transparent;
            this.cbTree.Enabled = false;
            this.cbTree.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbTree.Location = new System.Drawing.Point(67, 20);
            this.cbTree.Margin = new System.Windows.Forms.Padding(2);
            this.cbTree.Name = "cbTree";
            this.cbTree.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbTree.Size = new System.Drawing.Size(42, 23);
            this.cbTree.TabIndex = 40;
            this.cbTree.Text = "Chec";
            this.cbTree.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbTree.UseVisualStyleBackColor = true;
            this.cbTree.CheckedChanged += new System.EventHandler(this.cbDvc_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Enabled = false;
            this.label1.Location = new System.Drawing.Point(116, 24);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 13);
            this.label1.TabIndex = 41;
            this.label1.Text = "Keep collapsed state";
            // 
            // spContainer
            // 
            this.spContainer.BackColor = System.Drawing.Color.LightGray;
            this.spContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.spContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spContainer.Location = new System.Drawing.Point(0, 59);
            this.spContainer.Margin = new System.Windows.Forms.Padding(2);
            this.spContainer.Name = "spContainer";
            this.spContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // spContainer.Panel1
            // 
            this.spContainer.Panel1.BackColor = System.Drawing.Color.White;
            this.spContainer.Panel1.Controls.Add(this.vScrollBar1);
            this.spContainer.Panel1.Controls.Add(this.hScrollBar1);
            // 
            // spContainer.Panel2
            // 
            this.spContainer.Panel2.BackColor = System.Drawing.Color.White;
            this.spContainer.Size = new System.Drawing.Size(1414, 289);
            this.spContainer.SplitterDistance = 177;
            this.spContainer.SplitterIncrement = 3;
            this.spContainer.TabIndex = 3;
            // 
            // hScrollBar1
            // 
            this.hScrollBar1.Cursor = System.Windows.Forms.Cursors.Default;
            this.hScrollBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.hScrollBar1.LargeChange = 1;
            this.hScrollBar1.Location = new System.Drawing.Point(0, 154);
            this.hScrollBar1.Maximum = 31;
            this.hScrollBar1.Name = "hScrollBar1";
            this.hScrollBar1.Size = new System.Drawing.Size(1412, 21);
            this.hScrollBar1.TabIndex = 4;
            this.hScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.HScrollBar1_Scroll);
            // 
            // WorkflowController
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(1414, 348);
            this.Controls.Add(this.spContainer);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WorkflowController";
            this.Text = "Workflow controller";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.ProduzioneGantt_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.WorkflowController_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.WorkflowController_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.spContainer.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spContainer)).EndInit();
            this.spContainer.ResumeLayout(false);
            this.ResumeLayout(false);

            }

        #endregion
        private System.Windows.Forms.VScrollBar vScrollBar1;
        private System.Windows.Forms.Panel panel1;
        private MyCheckBox cbTree;
        private System.Windows.Forms.Label label6;
        private MyCheckBox cbHold;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCallCarico;
        private System.Windows.Forms.ComboBox cbArt;
        private System.Windows.Forms.ComboBox cbComm;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnMegaBack;
        private System.Windows.Forms.Button btnMegaFow;
        private System.Windows.Forms.Button btnZoomOut;
        private System.Windows.Forms.Button btnZoomIn;
        private System.Windows.Forms.Button btnFow;
        private System.Windows.Forms.SplitContainer spContainer;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnDayInfo;
        private System.Windows.Forms.Button btnSchedule;
        private System.Windows.Forms.Button btnHideDelay;
        private System.Windows.Forms.HScrollBar hScrollBar1;
        private System.Windows.Forms.Button button1;
    }
    }