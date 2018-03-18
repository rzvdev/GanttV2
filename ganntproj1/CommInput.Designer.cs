namespace ganntproj1
    {
    partial class CommInput
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
            {
            this.txtComm = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dtpCommData = new System.Windows.Forms.DateTimePicker();
            this.txtCommCapi = new System.Windows.Forms.TextBox();
            this.lblMaxCapi = new System.Windows.Forms.Label();
            this.cbCommLinea = new System.Windows.Forms.ComboBox();
            this.txtPersone = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.lblCommTit = new System.Windows.Forms.Label();
            this.pnHeader = new System.Windows.Forms.Panel();
            this.lblClose = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblProdStartInfo = new System.Windows.Forms.Label();
            this.tableView1 = new System.Windows.Forms.DataGridView();
            this.lblTotalQty = new System.Windows.Forms.Label();
            this.lblSavedInfo = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblOverQty = new System.Windows.Forms.Label();
            this.dtpTime = new System.Windows.Forms.DateTimePicker();
            this.label7 = new System.Windows.Forms.Label();
            this.lblDifQty = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.pnQtyInfo = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.lblDelay = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label10 = new System.Windows.Forms.Label();
            this.lblProdQty = new System.Windows.Forms.Label();
            this.lblCloseOrder = new System.Windows.Forms.Label();
            this.pbCloseOrder = new System.Windows.Forms.PictureBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.lblSave = new System.Windows.Forms.Label();
            this.lblSingle = new System.Windows.Forms.Label();
            this.rbDay = new System.Windows.Forms.RadioButton();
            this.rbHour = new System.Windows.Forms.RadioButton();
            this.lblStartDate = new System.Windows.Forms.Label();
            this.pnHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tableView1)).BeginInit();
            this.pnQtyInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbCloseOrder)).BeginInit();
            this.SuspendLayout();
            // 
            // txtComm
            // 
            this.txtComm.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtComm.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtComm.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtComm.Location = new System.Drawing.Point(41, 132);
            this.txtComm.Name = "txtComm";
            this.txtComm.ReadOnly = true;
            this.txtComm.Size = new System.Drawing.Size(169, 27);
            this.txtComm.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.label3.Location = new System.Drawing.Point(38, 215);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 18);
            this.label3.TabIndex = 3;
            this.label3.Text = "Data";
            // 
            // dtpCommData
            // 
            this.dtpCommData.CalendarTitleForeColor = System.Drawing.Color.AliceBlue;
            this.dtpCommData.CustomFormat = "dd/MM/yyyy";
            this.dtpCommData.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpCommData.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpCommData.Location = new System.Drawing.Point(41, 236);
            this.dtpCommData.Name = "dtpCommData";
            this.dtpCommData.Size = new System.Drawing.Size(181, 38);
            this.dtpCommData.TabIndex = 4;
            this.dtpCommData.CloseUp += new System.EventHandler(this.dtpCommData_CloseUp);
            // 
            // txtCommCapi
            // 
            this.txtCommCapi.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCommCapi.ForeColor = System.Drawing.Color.SeaGreen;
            this.txtCommCapi.Location = new System.Drawing.Point(235, 231);
            this.txtCommCapi.Name = "txtCommCapi";
            this.txtCommCapi.Size = new System.Drawing.Size(102, 43);
            this.txtCommCapi.TabIndex = 6;
            this.txtCommCapi.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtCommCapi.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCommCapi_KeyDown);
            this.txtCommCapi.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCommCapi_KeyPress);
            // 
            // lblMaxCapi
            // 
            this.lblMaxCapi.AutoSize = true;
            this.lblMaxCapi.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.lblMaxCapi.Location = new System.Drawing.Point(232, 191);
            this.lblMaxCapi.Name = "lblMaxCapi";
            this.lblMaxCapi.Size = new System.Drawing.Size(38, 18);
            this.lblMaxCapi.TabIndex = 5;
            this.lblMaxCapi.Text = "Capi";
            // 
            // cbCommLinea
            // 
            this.cbCommLinea.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCommLinea.Enabled = false;
            this.cbCommLinea.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbCommLinea.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbCommLinea.Location = new System.Drawing.Point(258, 128);
            this.cbCommLinea.Name = "cbCommLinea";
            this.cbCommLinea.Size = new System.Drawing.Size(131, 36);
            this.cbCommLinea.TabIndex = 8;
            // 
            // txtPersone
            // 
            this.txtPersone.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPersone.ForeColor = System.Drawing.Color.Black;
            this.txtPersone.Location = new System.Drawing.Point(353, 231);
            this.txtPersone.Name = "txtPersone";
            this.txtPersone.ReadOnly = true;
            this.txtPersone.Size = new System.Drawing.Size(102, 43);
            this.txtPersone.TabIndex = 15;
            this.txtPersone.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtPersone.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox2_KeyPress);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.label6.Location = new System.Drawing.Point(350, 210);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(88, 18);
            this.label6.TabIndex = 14;
            this.label6.Text = "Nr. Persone";
            // 
            // lblCommTit
            // 
            this.lblCommTit.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblCommTit.Font = new System.Drawing.Font("Tahoma", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCommTit.ForeColor = System.Drawing.Color.SeaGreen;
            this.lblCommTit.Location = new System.Drawing.Point(14, 34);
            this.lblCommTit.Name = "lblCommTit";
            this.lblCommTit.Size = new System.Drawing.Size(193, 41);
            this.lblCommTit.TabIndex = 18;
            this.lblCommTit.Text = "Produzione";
            this.lblCommTit.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.lblCommTit.Paint += new System.Windows.Forms.PaintEventHandler(this.lblCommTit_Paint);
            // 
            // pnHeader
            // 
            this.pnHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(119)))), ((int)(((byte)(55)))));
            this.pnHeader.Controls.Add(this.lblClose);
            this.pnHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnHeader.Location = new System.Drawing.Point(0, 0);
            this.pnHeader.Name = "pnHeader";
            this.pnHeader.Size = new System.Drawing.Size(478, 70);
            this.pnHeader.TabIndex = 23;
            // 
            // lblClose
            // 
            this.lblClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClose.ForeColor = System.Drawing.Color.White;
            this.lblClose.Location = new System.Drawing.Point(406, 12);
            this.lblClose.Name = "lblClose";
            this.lblClose.Size = new System.Drawing.Size(60, 44);
            this.lblClose.TabIndex = 0;
            this.lblClose.Text = "X";
            this.lblClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblClose.Click += new System.EventHandler(this.label9_Click);
            this.lblClose.Paint += new System.Windows.Forms.PaintEventHandler(this.lblClose_Paint);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.label1.Location = new System.Drawing.Point(38, 103);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 18);
            this.label1.TabIndex = 31;
            this.label1.Text = "Commessa";
            this.label1.Click += new System.EventHandler(this.Label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.label2.Location = new System.Drawing.Point(255, 103);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 18);
            this.label2.TabIndex = 32;
            this.label2.Text = "Line";
            // 
            // lblProdStartInfo
            // 
            this.lblProdStartInfo.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblProdStartInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblProdStartInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProdStartInfo.ForeColor = System.Drawing.Color.Black;
            this.lblProdStartInfo.Location = new System.Drawing.Point(41, 299);
            this.lblProdStartInfo.Name = "lblProdStartInfo";
            this.lblProdStartInfo.Size = new System.Drawing.Size(414, 33);
            this.lblProdStartInfo.TabIndex = 34;
            this.lblProdStartInfo.Text = "Start:";
            this.lblProdStartInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblProdStartInfo.Click += new System.EventHandler(this.lblProdStartInfo_Click);
            // 
            // tableView1
            // 
            this.tableView1.AllowUserToAddRows = false;
            this.tableView1.AllowUserToDeleteRows = false;
            this.tableView1.AllowUserToResizeRows = false;
            this.tableView1.BackgroundColor = System.Drawing.Color.White;
            this.tableView1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tableView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tableView1.Location = new System.Drawing.Point(503, 93);
            this.tableView1.Name = "tableView1";
            this.tableView1.RowHeadersVisible = false;
            this.tableView1.RowTemplate.Height = 24;
            this.tableView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.tableView1.Size = new System.Drawing.Size(696, 424);
            this.tableView1.TabIndex = 37;
            // 
            // lblTotalQty
            // 
            this.lblTotalQty.AutoSize = true;
            this.lblTotalQty.Font = new System.Drawing.Font("Microsoft Sans Serif", 25.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalQty.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblTotalQty.Location = new System.Drawing.Point(96, 25);
            this.lblTotalQty.Name = "lblTotalQty";
            this.lblTotalQty.Size = new System.Drawing.Size(47, 51);
            this.lblTotalQty.TabIndex = 45;
            this.lblTotalQty.Text = "0";
            // 
            // lblSavedInfo
            // 
            this.lblSavedInfo.BackColor = System.Drawing.Color.Silver;
            this.lblSavedInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblSavedInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSavedInfo.ForeColor = System.Drawing.Color.Black;
            this.lblSavedInfo.Location = new System.Drawing.Point(181, 299);
            this.lblSavedInfo.Name = "lblSavedInfo";
            this.lblSavedInfo.Size = new System.Drawing.Size(129, 34);
            this.lblSavedInfo.TabIndex = 47;
            this.lblSavedInfo.Text = "Saving...";
            this.lblSavedInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblSavedInfo.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(33, 42);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 20);
            this.label4.TabIndex = 48;
            this.label4.Text = "Total:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(35, 189);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 20);
            this.label5.TabIndex = 49;
            this.label5.Text = "Over:";
            // 
            // lblOverQty
            // 
            this.lblOverQty.AutoSize = true;
            this.lblOverQty.Font = new System.Drawing.Font("Microsoft Sans Serif", 25.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOverQty.ForeColor = System.Drawing.Color.Gold;
            this.lblOverQty.Location = new System.Drawing.Point(96, 172);
            this.lblOverQty.Name = "lblOverQty";
            this.lblOverQty.Size = new System.Drawing.Size(47, 51);
            this.lblOverQty.TabIndex = 50;
            this.lblOverQty.Text = "0";
            // 
            // dtpTime
            // 
            this.dtpTime.CalendarTitleForeColor = System.Drawing.Color.AliceBlue;
            this.dtpTime.Checked = false;
            this.dtpTime.CustomFormat = "HH:mm";
            this.dtpTime.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpTime.Location = new System.Drawing.Point(41, 298);
            this.dtpTime.Name = "dtpTime";
            this.dtpTime.ShowCheckBox = true;
            this.dtpTime.ShowUpDown = true;
            this.dtpTime.Size = new System.Drawing.Size(181, 34);
            this.dtpTime.TabIndex = 51;
            this.dtpTime.Visible = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.label7.Location = new System.Drawing.Point(38, 273);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 18);
            this.label7.TabIndex = 52;
            this.label7.Text = "Time";
            this.label7.Visible = false;
            // 
            // lblDifQty
            // 
            this.lblDifQty.AutoSize = true;
            this.lblDifQty.Font = new System.Drawing.Font("Microsoft Sans Serif", 25.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDifQty.ForeColor = System.Drawing.Color.Crimson;
            this.lblDifQty.Location = new System.Drawing.Point(96, 234);
            this.lblDifQty.Name = "lblDifQty";
            this.lblDifQty.Size = new System.Drawing.Size(47, 51);
            this.lblDifQty.TabIndex = 53;
            this.lblDifQty.Text = "0";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.Location = new System.Drawing.Point(44, 251);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(46, 20);
            this.label8.TabIndex = 54;
            this.label8.Text = "Diff:";
            // 
            // pnQtyInfo
            // 
            this.pnQtyInfo.BackColor = System.Drawing.Color.AliceBlue;
            this.pnQtyInfo.Controls.Add(this.label9);
            this.pnQtyInfo.Controls.Add(this.lblDelay);
            this.pnQtyInfo.Controls.Add(this.pictureBox1);
            this.pnQtyInfo.Controls.Add(this.pictureBox2);
            this.pnQtyInfo.Controls.Add(this.label10);
            this.pnQtyInfo.Controls.Add(this.lblProdQty);
            this.pnQtyInfo.Controls.Add(this.label4);
            this.pnQtyInfo.Controls.Add(this.label8);
            this.pnQtyInfo.Controls.Add(this.lblTotalQty);
            this.pnQtyInfo.Controls.Add(this.lblDifQty);
            this.pnQtyInfo.Controls.Add(this.label5);
            this.pnQtyInfo.Controls.Add(this.lblOverQty);
            this.pnQtyInfo.Location = new System.Drawing.Point(1216, 93);
            this.pnQtyInfo.Name = "pnQtyInfo";
            this.pnQtyInfo.Size = new System.Drawing.Size(278, 424);
            this.pnQtyInfo.TabIndex = 55;
            this.pnQtyInfo.Paint += new System.Windows.Forms.PaintEventHandler(this.Panel1_Paint);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.Black;
            this.label9.Location = new System.Drawing.Point(27, 345);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(63, 20);
            this.label9.TabIndex = 60;
            this.label9.Text = "Delay:";
            // 
            // lblDelay
            // 
            this.lblDelay.AutoSize = true;
            this.lblDelay.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDelay.ForeColor = System.Drawing.Color.Orange;
            this.lblDelay.Location = new System.Drawing.Point(96, 341);
            this.lblDelay.Name = "lblDelay";
            this.lblDelay.Size = new System.Drawing.Size(32, 32);
            this.lblDelay.TabIndex = 59;
            this.lblDelay.Text = "0";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Gainsboro;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox1.Location = new System.Drawing.Point(33, 303);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(150, 2);
            this.pictureBox1.TabIndex = 58;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Gainsboro;
            this.pictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox2.Location = new System.Drawing.Point(33, 88);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(150, 2);
            this.pictureBox2.TabIndex = 57;
            this.pictureBox2.TabStop = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.Black;
            this.label10.Location = new System.Drawing.Point(36, 130);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(54, 20);
            this.label10.TabIndex = 56;
            this.label10.Text = "Prod:";
            // 
            // lblProdQty
            // 
            this.lblProdQty.AutoSize = true;
            this.lblProdQty.Font = new System.Drawing.Font("Microsoft Sans Serif", 25.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProdQty.ForeColor = System.Drawing.Color.SeaGreen;
            this.lblProdQty.Location = new System.Drawing.Point(96, 110);
            this.lblProdQty.Name = "lblProdQty";
            this.lblProdQty.Size = new System.Drawing.Size(47, 51);
            this.lblProdQty.TabIndex = 55;
            this.lblProdQty.Text = "0";
            // 
            // lblCloseOrder
            // 
            this.lblCloseOrder.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblCloseOrder.Location = new System.Drawing.Point(75, 461);
            this.lblCloseOrder.Name = "lblCloseOrder";
            this.lblCloseOrder.Size = new System.Drawing.Size(376, 50);
            this.lblCloseOrder.TabIndex = 57;
            this.lblCloseOrder.Text = "      Close commessa";
            this.lblCloseOrder.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblCloseOrder.Click += new System.EventHandler(this.PbCloseOrder_Click);
            // 
            // pbCloseOrder
            // 
            this.pbCloseOrder.Image = global::ganntproj1.Properties.Resources.discard_changes_48;
            this.pbCloseOrder.Location = new System.Drawing.Point(27, 461);
            this.pbCloseOrder.Name = "pbCloseOrder";
            this.pbCloseOrder.Size = new System.Drawing.Size(48, 50);
            this.pbCloseOrder.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbCloseOrder.TabIndex = 56;
            this.pbCloseOrder.TabStop = false;
            this.pbCloseOrder.Click += new System.EventHandler(this.PbCloseOrder_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnDelete.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.btnDelete.ForeColor = System.Drawing.Color.Black;
            this.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelete.Location = new System.Drawing.Point(503, 523);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(207, 52);
            this.btnDelete.TabIndex = 44;
            this.btnDelete.Text = "Delete record";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // lblSave
            // 
            this.lblSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblSave.ForeColor = System.Drawing.Color.Black;
            this.lblSave.Image = global::ganntproj1.Properties.Resources.save_multidm_48;
            this.lblSave.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblSave.Location = new System.Drawing.Point(163, 354);
            this.lblSave.Name = "lblSave";
            this.lblSave.Size = new System.Drawing.Size(139, 92);
            this.lblSave.TabIndex = 30;
            this.lblSave.Text = "Save";
            this.lblSave.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.lblSave.Click += new System.EventHandler(this.lblSave_Click);
            this.lblSave.Paint += new System.Windows.Forms.PaintEventHandler(this.lblSave_Paint);
            this.lblSave.MouseEnter += new System.EventHandler(this.label8_MouseEnter);
            this.lblSave.MouseLeave += new System.EventHandler(this.label8_MouseLeave);
            // 
            // lblSingle
            // 
            this.lblSingle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSingle.ForeColor = System.Drawing.Color.Black;
            this.lblSingle.Image = global::ganntproj1.Properties.Resources.monitoring_multidm_48;
            this.lblSingle.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblSingle.Location = new System.Drawing.Point(27, 524);
            this.lblSingle.Name = "lblSingle";
            this.lblSingle.Size = new System.Drawing.Size(426, 50);
            this.lblSingle.TabIndex = 25;
            this.lblSingle.Text = "               Overview >>";
            this.lblSingle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblSingle.Click += new System.EventHandler(this.lblSingle_Click);
            this.lblSingle.MouseEnter += new System.EventHandler(this.label8_MouseEnter);
            this.lblSingle.MouseLeave += new System.EventHandler(this.label8_MouseLeave);
            // 
            // rbDay
            // 
            this.rbDay.AutoSize = true;
            this.rbDay.Checked = true;
            this.rbDay.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbDay.Location = new System.Drawing.Point(315, 357);
            this.rbDay.Name = "rbDay";
            this.rbDay.Size = new System.Drawing.Size(78, 21);
            this.rbDay.TabIndex = 58;
            this.rbDay.TabStop = true;
            this.rbDay.Text = "By day";
            this.rbDay.UseVisualStyleBackColor = true;
            this.rbDay.CheckedChanged += new System.EventHandler(this.RbDay_CheckedChanged);
            // 
            // rbHour
            // 
            this.rbHour.AutoSize = true;
            this.rbHour.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbHour.Location = new System.Drawing.Point(315, 384);
            this.rbHour.Name = "rbHour";
            this.rbHour.Size = new System.Drawing.Size(85, 21);
            this.rbHour.TabIndex = 59;
            this.rbHour.Text = "By hour";
            this.rbHour.UseVisualStyleBackColor = true;
            this.rbHour.CheckedChanged += new System.EventHandler(this.RbHour_CheckedChanged);
            // 
            // lblStartDate
            // 
            this.lblStartDate.AutoSize = true;
            this.lblStartDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStartDate.ForeColor = System.Drawing.Color.DarkGray;
            this.lblStartDate.Location = new System.Drawing.Point(38, 164);
            this.lblStartDate.Name = "lblStartDate";
            this.lblStartDate.Size = new System.Drawing.Size(0, 18);
            this.lblStartDate.TabIndex = 60;
            // 
            // CommInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(478, 593);
            this.ControlBox = false;
            this.Controls.Add(this.lblStartDate);
            this.Controls.Add(this.rbHour);
            this.Controls.Add(this.rbDay);
            this.Controls.Add(this.lblCommTit);
            this.Controls.Add(this.pnHeader);
            this.Controls.Add(this.lblCloseOrder);
            this.Controls.Add(this.pbCloseOrder);
            this.Controls.Add(this.pnQtyInfo);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.dtpTime);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.lblProdStartInfo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblSave);
            this.Controls.Add(this.lblSingle);
            this.Controls.Add(this.txtPersone);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cbCommLinea);
            this.Controls.Add(this.txtCommCapi);
            this.Controls.Add(this.lblMaxCapi);
            this.Controls.Add(this.dtpCommData);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtComm);
            this.Controls.Add(this.tableView1);
            this.Controls.Add(this.lblSavedInfo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CommInput";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.CommInput_Load_1);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CommInput_KeyDown);
            this.pnHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tableView1)).EndInit();
            this.pnQtyInfo.ResumeLayout(false);
            this.pnQtyInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbCloseOrder)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

            }

        #endregion
        private System.Windows.Forms.TextBox txtComm;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtpCommData;
        private System.Windows.Forms.TextBox txtCommCapi;
        private System.Windows.Forms.Label lblMaxCapi;
        private System.Windows.Forms.ComboBox cbCommLinea;
        private System.Windows.Forms.TextBox txtPersone;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblCommTit;
        private System.Windows.Forms.Panel pnHeader;
        private System.Windows.Forms.Label lblSingle;
        private System.Windows.Forms.Label lblSave;
        private System.Windows.Forms.Label lblClose;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblProdStartInfo;
        private System.Windows.Forms.DataGridView tableView1;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Label lblTotalQty;
        private System.Windows.Forms.Label lblSavedInfo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblOverQty;
        private System.Windows.Forms.DateTimePicker dtpTime;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblDifQty;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel pnQtyInfo;
        private System.Windows.Forms.PictureBox pbCloseOrder;
        private System.Windows.Forms.Label lblCloseOrder;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lblProdQty;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lblDelay;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.RadioButton rbDay;
        private System.Windows.Forms.RadioButton rbHour;
        private System.Windows.Forms.Label lblStartDate;
    }
    }
