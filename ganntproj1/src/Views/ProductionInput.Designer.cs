namespace ganntproj1
    {
    partial class ProductionInput
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
            this.pnHeader = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblProdStartInfo = new System.Windows.Forms.Label();
            this.lblTotalQty = new System.Windows.Forms.Label();
            this.lblSavedInfo = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblOverQty = new System.Windows.Forms.Label();
            this.lblDifQty = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.pnQtyInfo = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.lblDelay = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label10 = new System.Windows.Forms.Label();
            this.lblProdQty = new System.Windows.Forms.Label();
            this.btnDelete = new System.Windows.Forms.Button();
            this.rbDay = new System.Windows.Forms.RadioButton();
            this.rbHour = new System.Windows.Forms.RadioButton();
            this.lblStartDate = new System.Windows.Forms.Label();
            this.cboShift = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pbRightbnd = new System.Windows.Forms.PictureBox();
            this.pbBottombnd = new System.Windows.Forms.PictureBox();
            this.lblCloseOrder = new System.Windows.Forms.Label();
            this.lblSave = new System.Windows.Forms.Label();
            this.lblSingle = new System.Windows.Forms.Label();
            this.lblAbatim = new System.Windows.Forms.Label();
            this.tableView1 = new ganntproj1.TableView();
            this.label11 = new System.Windows.Forms.Label();
            this.pnHeader.SuspendLayout();
            this.pnQtyInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRightbnd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbBottombnd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tableView1)).BeginInit();
            this.SuspendLayout();
            // 
            // txtComm
            // 
            this.txtComm.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtComm.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtComm.Font = new System.Drawing.Font("Bahnschrift", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtComm.Location = new System.Drawing.Point(31, 107);
            this.txtComm.Margin = new System.Windows.Forms.Padding(2);
            this.txtComm.Name = "txtComm";
            this.txtComm.ReadOnly = true;
            this.txtComm.Size = new System.Drawing.Size(127, 20);
            this.txtComm.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Bahnschrift", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(28, 169);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 14);
            this.label3.TabIndex = 3;
            this.label3.Text = "Data";
            // 
            // dtpCommData
            // 
            this.dtpCommData.CalendarTitleForeColor = System.Drawing.Color.AliceBlue;
            this.dtpCommData.CustomFormat = "dd/MM/yyyy";
            this.dtpCommData.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpCommData.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpCommData.Location = new System.Drawing.Point(31, 186);
            this.dtpCommData.Margin = new System.Windows.Forms.Padding(2);
            this.dtpCommData.Name = "dtpCommData";
            this.dtpCommData.Size = new System.Drawing.Size(141, 29);
            this.dtpCommData.TabIndex = 4;
            this.dtpCommData.CloseUp += new System.EventHandler(this.dtpCommData_CloseUp);
            // 
            // txtCommCapi
            // 
            this.txtCommCapi.Font = new System.Drawing.Font("Bahnschrift", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCommCapi.ForeColor = System.Drawing.Color.ForestGreen;
            this.txtCommCapi.Location = new System.Drawing.Point(176, 182);
            this.txtCommCapi.Margin = new System.Windows.Forms.Padding(2);
            this.txtCommCapi.Name = "txtCommCapi";
            this.txtCommCapi.Size = new System.Drawing.Size(78, 33);
            this.txtCommCapi.TabIndex = 6;
            this.txtCommCapi.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtCommCapi.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCommCapi_KeyDown);
            this.txtCommCapi.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCommCapi_KeyPress);
            // 
            // lblMaxCapi
            // 
            this.lblMaxCapi.AutoSize = true;
            this.lblMaxCapi.Font = new System.Drawing.Font("Bahnschrift", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMaxCapi.Location = new System.Drawing.Point(174, 149);
            this.lblMaxCapi.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblMaxCapi.Name = "lblMaxCapi";
            this.lblMaxCapi.Size = new System.Drawing.Size(30, 14);
            this.lblMaxCapi.TabIndex = 5;
            this.lblMaxCapi.Text = "Capi";
            // 
            // cbCommLinea
            // 
            this.cbCommLinea.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCommLinea.Enabled = false;
            this.cbCommLinea.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbCommLinea.Font = new System.Drawing.Font("Bahnschrift", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbCommLinea.Location = new System.Drawing.Point(194, 104);
            this.cbCommLinea.Margin = new System.Windows.Forms.Padding(2);
            this.cbCommLinea.Name = "cbCommLinea";
            this.cbCommLinea.Size = new System.Drawing.Size(99, 27);
            this.cbCommLinea.TabIndex = 8;
            // 
            // txtPersone
            // 
            this.txtPersone.Font = new System.Drawing.Font("Bahnschrift", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPersone.ForeColor = System.Drawing.Color.Brown;
            this.txtPersone.Location = new System.Drawing.Point(264, 182);
            this.txtPersone.Margin = new System.Windows.Forms.Padding(2);
            this.txtPersone.Name = "txtPersone";
            this.txtPersone.Size = new System.Drawing.Size(78, 33);
            this.txtPersone.TabIndex = 15;
            this.txtPersone.Text = "0";
            this.txtPersone.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtPersone.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox2_KeyPress);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Bahnschrift", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(261, 149);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(61, 28);
            this.label6.TabIndex = 14;
            this.label6.Text = "Members/\r\nmachines";
            // 
            // pnHeader
            // 
            this.pnHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(13)))), ((int)(((byte)(23)))));
            this.pnHeader.Controls.Add(this.btnClose);
            this.pnHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnHeader.Location = new System.Drawing.Point(0, 0);
            this.pnHeader.Margin = new System.Windows.Forms.Padding(2);
            this.pnHeader.Name = "pnHeader";
            this.pnHeader.Size = new System.Drawing.Size(370, 46);
            this.pnHeader.TabIndex = 23;
            this.pnHeader.Paint += new System.Windows.Forms.PaintEventHandler(this.PnHeader_Paint);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(13)))), ((int)(((byte)(23)))));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold);
            this.btnClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.btnClose.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnClose.Location = new System.Drawing.Point(324, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(46, 46);
            this.btnClose.TabIndex = 22;
            this.btnClose.Tag = "1";
            this.btnClose.Text = "X";
            this.btnClose.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Bahnschrift", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(28, 84);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 14);
            this.label1.TabIndex = 31;
            this.label1.Text = "Commessa";
            this.label1.Click += new System.EventHandler(this.Label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Bahnschrift", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(191, 84);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 14);
            this.label2.TabIndex = 32;
            this.label2.Text = "Line";
            // 
            // lblProdStartInfo
            // 
            this.lblProdStartInfo.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblProdStartInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblProdStartInfo.Font = new System.Drawing.Font("Bahnschrift", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProdStartInfo.ForeColor = System.Drawing.Color.Black;
            this.lblProdStartInfo.Location = new System.Drawing.Point(31, 272);
            this.lblProdStartInfo.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblProdStartInfo.Name = "lblProdStartInfo";
            this.lblProdStartInfo.Size = new System.Drawing.Size(311, 27);
            this.lblProdStartInfo.TabIndex = 34;
            this.lblProdStartInfo.Text = "Start:";
            this.lblProdStartInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblProdStartInfo.Click += new System.EventHandler(this.lblProdStartInfo_Click);
            // 
            // lblTotalQty
            // 
            this.lblTotalQty.AutoSize = true;
            this.lblTotalQty.Font = new System.Drawing.Font("Bahnschrift", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalQty.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblTotalQty.Location = new System.Drawing.Point(79, 20);
            this.lblTotalQty.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTotalQty.Name = "lblTotalQty";
            this.lblTotalQty.Size = new System.Drawing.Size(37, 42);
            this.lblTotalQty.TabIndex = 45;
            this.lblTotalQty.Text = "0";
            // 
            // lblSavedInfo
            // 
            this.lblSavedInfo.BackColor = System.Drawing.Color.Silver;
            this.lblSavedInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblSavedInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSavedInfo.ForeColor = System.Drawing.Color.Black;
            this.lblSavedInfo.Location = new System.Drawing.Point(136, 272);
            this.lblSavedInfo.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSavedInfo.Name = "lblSavedInfo";
            this.lblSavedInfo.Size = new System.Drawing.Size(97, 28);
            this.lblSavedInfo.TabIndex = 47;
            this.lblSavedInfo.Text = "Saving...";
            this.lblSavedInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblSavedInfo.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Bahnschrift", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(20, 31);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 23);
            this.label4.TabIndex = 48;
            this.label4.Text = "Total:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Bahnschrift", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(21, 151);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 23);
            this.label5.TabIndex = 49;
            this.label5.Text = "Over:";
            // 
            // lblOverQty
            // 
            this.lblOverQty.AutoSize = true;
            this.lblOverQty.Font = new System.Drawing.Font("Bahnschrift", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOverQty.ForeColor = System.Drawing.Color.Gold;
            this.lblOverQty.Location = new System.Drawing.Point(79, 140);
            this.lblOverQty.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblOverQty.Name = "lblOverQty";
            this.lblOverQty.Size = new System.Drawing.Size(37, 42);
            this.lblOverQty.TabIndex = 50;
            this.lblOverQty.Text = "0";
            // 
            // lblDifQty
            // 
            this.lblDifQty.AutoSize = true;
            this.lblDifQty.Font = new System.Drawing.Font("Bahnschrift", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDifQty.ForeColor = System.Drawing.Color.Crimson;
            this.lblDifQty.Location = new System.Drawing.Point(79, 190);
            this.lblDifQty.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblDifQty.Name = "lblDifQty";
            this.lblDifQty.Size = new System.Drawing.Size(37, 42);
            this.lblDifQty.TabIndex = 53;
            this.lblDifQty.Text = "0";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Bahnschrift", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.Location = new System.Drawing.Point(28, 201);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(42, 23);
            this.label8.TabIndex = 54;
            this.label8.Text = "Diff:";
            // 
            // pnQtyInfo
            // 
            this.pnQtyInfo.BackColor = System.Drawing.Color.WhiteSmoke;
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
            this.pnQtyInfo.Location = new System.Drawing.Point(912, 76);
            this.pnQtyInfo.Margin = new System.Windows.Forms.Padding(2);
            this.pnQtyInfo.Name = "pnQtyInfo";
            this.pnQtyInfo.Size = new System.Drawing.Size(208, 344);
            this.pnQtyInfo.TabIndex = 55;
            this.pnQtyInfo.Paint += new System.Windows.Forms.PaintEventHandler(this.Panel1_Paint);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Bahnschrift", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.Black;
            this.label9.Location = new System.Drawing.Point(15, 277);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(60, 23);
            this.label9.TabIndex = 60;
            this.label9.Text = "Delay:";
            this.label9.Visible = false;
            // 
            // lblDelay
            // 
            this.lblDelay.AutoSize = true;
            this.lblDelay.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDelay.ForeColor = System.Drawing.Color.Orange;
            this.lblDelay.Location = new System.Drawing.Point(79, 277);
            this.lblDelay.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblDelay.Name = "lblDelay";
            this.lblDelay.Size = new System.Drawing.Size(25, 26);
            this.lblDelay.TabIndex = 59;
            this.lblDelay.Text = "0";
            this.lblDelay.Visible = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Gainsboro;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox1.Location = new System.Drawing.Point(25, 246);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(114, 2);
            this.pictureBox1.TabIndex = 58;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Gainsboro;
            this.pictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox2.Location = new System.Drawing.Point(25, 72);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(114, 2);
            this.pictureBox2.TabIndex = 57;
            this.pictureBox2.TabStop = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Bahnschrift", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.Black;
            this.label10.Location = new System.Drawing.Point(22, 103);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(54, 23);
            this.label10.TabIndex = 56;
            this.label10.Text = "Prod:";
            // 
            // lblProdQty
            // 
            this.lblProdQty.AutoSize = true;
            this.lblProdQty.Font = new System.Drawing.Font("Bahnschrift", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProdQty.ForeColor = System.Drawing.Color.SeaGreen;
            this.lblProdQty.Location = new System.Drawing.Point(79, 89);
            this.lblProdQty.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblProdQty.Name = "lblProdQty";
            this.lblProdQty.Size = new System.Drawing.Size(37, 42);
            this.lblProdQty.TabIndex = 55;
            this.lblProdQty.Text = "0";
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnDelete.Font = new System.Drawing.Font("Bahnschrift", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.ForeColor = System.Drawing.Color.Black;
            this.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelete.Location = new System.Drawing.Point(377, 425);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(2);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(155, 42);
            this.btnDelete.TabIndex = 44;
            this.btnDelete.Text = "Delete record";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // rbDay
            // 
            this.rbDay.AutoSize = true;
            this.rbDay.Checked = true;
            this.rbDay.Font = new System.Drawing.Font("Bahnschrift", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbDay.Location = new System.Drawing.Point(223, 341);
            this.rbDay.Margin = new System.Windows.Forms.Padding(2);
            this.rbDay.Name = "rbDay";
            this.rbDay.Size = new System.Drawing.Size(63, 20);
            this.rbDay.TabIndex = 58;
            this.rbDay.TabStop = true;
            this.rbDay.Text = "By day";
            this.rbDay.UseVisualStyleBackColor = true;
            this.rbDay.CheckedChanged += new System.EventHandler(this.RbDay_CheckedChanged);
            // 
            // rbHour
            // 
            this.rbHour.AutoSize = true;
            this.rbHour.Font = new System.Drawing.Font("Bahnschrift", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbHour.Location = new System.Drawing.Point(223, 365);
            this.rbHour.Margin = new System.Windows.Forms.Padding(2);
            this.rbHour.Name = "rbHour";
            this.rbHour.Size = new System.Drawing.Size(70, 20);
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
            this.lblStartDate.Location = new System.Drawing.Point(28, 133);
            this.lblStartDate.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblStartDate.Name = "lblStartDate";
            this.lblStartDate.Size = new System.Drawing.Size(0, 15);
            this.lblStartDate.TabIndex = 60;
            // 
            // cboShift
            // 
            this.cboShift.BackColor = System.Drawing.Color.White;
            this.cboShift.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboShift.Font = new System.Drawing.Font("Bahnschrift", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboShift.FormattingEnabled = true;
            this.cboShift.Location = new System.Drawing.Point(70, 234);
            this.cboShift.Name = "cboShift";
            this.cboShift.Size = new System.Drawing.Size(137, 24);
            this.cboShift.TabIndex = 61;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Bahnschrift", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(28, 237);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(35, 14);
            this.label7.TabIndex = 62;
            this.label7.Text = "Shift:";
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(13)))), ((int)(((byte)(23)))));
            this.pictureBox3.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox3.Location = new System.Drawing.Point(0, 46);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(1, 471);
            this.pictureBox3.TabIndex = 65;
            this.pictureBox3.TabStop = false;
            // 
            // pbRightbnd
            // 
            this.pbRightbnd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(13)))), ((int)(((byte)(23)))));
            this.pbRightbnd.Dock = System.Windows.Forms.DockStyle.Right;
            this.pbRightbnd.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pbRightbnd.Location = new System.Drawing.Point(369, 46);
            this.pbRightbnd.Name = "pbRightbnd";
            this.pbRightbnd.Size = new System.Drawing.Size(1, 471);
            this.pbRightbnd.TabIndex = 64;
            this.pbRightbnd.TabStop = false;
            // 
            // pbBottombnd
            // 
            this.pbBottombnd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(13)))), ((int)(((byte)(23)))));
            this.pbBottombnd.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pbBottombnd.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pbBottombnd.Location = new System.Drawing.Point(0, 517);
            this.pbBottombnd.Name = "pbBottombnd";
            this.pbBottombnd.Size = new System.Drawing.Size(370, 1);
            this.pbBottombnd.TabIndex = 63;
            this.pbBottombnd.TabStop = false;
            // 
            // lblCloseOrder
            // 
            this.lblCloseOrder.Font = new System.Drawing.Font("Bahnschrift", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCloseOrder.Image = global::ganntproj1.Properties.Resources.close;
            this.lblCloseOrder.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblCloseOrder.Location = new System.Drawing.Point(26, 409);
            this.lblCloseOrder.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCloseOrder.Name = "lblCloseOrder";
            this.lblCloseOrder.Size = new System.Drawing.Size(312, 41);
            this.lblCloseOrder.TabIndex = 57;
            this.lblCloseOrder.Text = "          Close commessa";
            this.lblCloseOrder.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblCloseOrder.Click += new System.EventHandler(this.PbCloseOrder_Click);
            // 
            // lblSave
            // 
            this.lblSave.Font = new System.Drawing.Font("Bahnschrift", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSave.ForeColor = System.Drawing.Color.Black;
            this.lblSave.Image = global::ganntproj1.Properties.Resources.save_multidm_48;
            this.lblSave.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblSave.Location = new System.Drawing.Point(144, 310);
            this.lblSave.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSave.Name = "lblSave";
            this.lblSave.Size = new System.Drawing.Size(70, 75);
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
            this.lblSingle.Font = new System.Drawing.Font("Bahnschrift", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSingle.ForeColor = System.Drawing.Color.Black;
            this.lblSingle.Image = global::ganntproj1.Properties.Resources.folder;
            this.lblSingle.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblSingle.Location = new System.Drawing.Point(26, 454);
            this.lblSingle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSingle.Name = "lblSingle";
            this.lblSingle.Size = new System.Drawing.Size(312, 41);
            this.lblSingle.TabIndex = 25;
            this.lblSingle.Text = "          Overview >>";
            this.lblSingle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblSingle.Click += new System.EventHandler(this.lblSingle_Click);
            this.lblSingle.MouseEnter += new System.EventHandler(this.label8_MouseEnter);
            this.lblSingle.MouseLeave += new System.EventHandler(this.label8_MouseLeave);
            // 
            // lblAbatim
            // 
            this.lblAbatim.AutoSize = true;
            this.lblAbatim.Font = new System.Drawing.Font("Bahnschrift", 12F);
            this.lblAbatim.ForeColor = System.Drawing.Color.ForestGreen;
            this.lblAbatim.Location = new System.Drawing.Point(294, 104);
            this.lblAbatim.Name = "lblAbatim";
            this.lblAbatim.Size = new System.Drawing.Size(0, 19);
            this.lblAbatim.TabIndex = 66;
            this.lblAbatim.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblAbatim.DoubleClick += new System.EventHandler(this.LblAbatim_DoubleClick);
            // 
            // tableView1
            // 
            this.tableView1.AllowUserToAddRows = false;
            this.tableView1.AllowUserToDeleteRows = false;
            this.tableView1.AllowUserToResizeColumns = false;
            this.tableView1.AllowUserToResizeRows = false;
            this.tableView1.BackgroundColor = System.Drawing.Color.White;
            this.tableView1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tableView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tableView1.EnableHeadersVisualStyles = false;
            this.tableView1.Location = new System.Drawing.Point(377, 76);
            this.tableView1.Margin = new System.Windows.Forms.Padding(2);
            this.tableView1.MultiSelect = false;
            this.tableView1.Name = "tableView1";
            this.tableView1.ReadOnly = true;
            this.tableView1.RowHeadersVisible = false;
            this.tableView1.RowTemplate.Height = 24;
            this.tableView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.tableView1.Size = new System.Drawing.Size(522, 344);
            this.tableView1.TabIndex = 37;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Bahnschrift", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(291, 84);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(26, 14);
            this.label11.TabIndex = 67;
            this.label11.Text = "Abt";
            // 
            // ProductionInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.ClientSize = new System.Drawing.Size(370, 518);
            this.ControlBox = false;
            this.Controls.Add(this.label11);
            this.Controls.Add(this.lblAbatim);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.pbRightbnd);
            this.Controls.Add(this.pbBottombnd);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.cboShift);
            this.Controls.Add(this.lblStartDate);
            this.Controls.Add(this.rbHour);
            this.Controls.Add(this.rbDay);
            this.Controls.Add(this.pnHeader);
            this.Controls.Add(this.lblCloseOrder);
            this.Controls.Add(this.pnQtyInfo);
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
            this.Font = new System.Drawing.Font("Bahnschrift", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProductionInput";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.CommInput_Load_1);
            this.SizeChanged += new System.EventHandler(this.ProductionInput_SizeChanged);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CommInput_KeyDown);
            this.pnHeader.ResumeLayout(false);
            this.pnQtyInfo.ResumeLayout(false);
            this.pnQtyInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRightbnd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbBottombnd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tableView1)).EndInit();
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
        private System.Windows.Forms.Panel pnHeader;
        private System.Windows.Forms.Label lblSingle;
        private System.Windows.Forms.Label lblSave;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblProdStartInfo;
        private TableView tableView1;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Label lblTotalQty;
        private System.Windows.Forms.Label lblSavedInfo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblOverQty;
        private System.Windows.Forms.Label lblDifQty;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel pnQtyInfo;
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
        private System.Windows.Forms.ComboBox cboShift;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.PictureBox pbBottombnd;
        private System.Windows.Forms.PictureBox pbRightbnd;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblAbatim;
        private System.Windows.Forms.Label label11;
    }
    }
