using ganntproj1.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ganntproj1
    {
    partial class CommandCenter
        {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
            {
            if (disposing && (components != null))
                {
                components.Dispose();
                }
            base.Dispose(disposing);
            }

        private void InitializeDisplay()
            {
            var dataGridViewCellStyle8 = new DataGridViewCellStyle();
            var dataGridViewCellStyle9 = new DataGridViewCellStyle();
            var dataGridViewCellStyle10 = new DataGridViewCellStyle();
            var resources = new ComponentResourceManager(typeof(CommandCenter));
            var dataGridViewCellStyle11 = new DataGridViewCellStyle();
            var dataGridViewCellStyle12 = new DataGridViewCellStyle();
            var dataGridViewCellStyle13 = new DataGridViewCellStyle();
            var dataGridViewCellStyle14 = new DataGridViewCellStyle();
            displayStatus = new StatusStrip();
            DisplayStatusConsole = new ToolStripStatusLabel();
            lblDepthStatus = new ToolStripStatusLabel();
            lblDateTimeInterval = new ToolStripStatusLabel();
            toolStripSplitButton1 = new ToolStripSplitButton();
            showConsoleOutput = new ToolStripMenuItem();
            hideConsoleOutput = new ToolStripMenuItem();
            _pnSideBar = new Panel();
            btnEndRemDays = new Button();
            btnEndAddDays = new Button();
            btnStartRemDays = new Button();
            btnStartAddDays = new Button();
            _btnStiro = new Button();
            btnFullPrev = new Button();
            BtnConf = new Button();
            _btnColapseSideBar = new Button();
            lblFiltersTit = new Label();
            _btnTess = new Button();
            _btnReload = new Button();
            _btnFilterSelection = new Button();
            _dtpDateTo = new DateTimePicker();
            label2 = new Label();
            _dtpDateFrom = new DateTimePicker();
            label1 = new Label();
            DisplaySplitContainer = new SplitContainer();
            GanttContainer = new Panel();
            pnSplitter1 = new Panel();
            pnNavPlus = new Panel();
            splitContainer1 = new SplitContainer();
            btnNavBackPlus = new Button();
            btnNavBackMegaPlus = new Button();
            btnZoomOutPlus = new Button();
            btnNavForwMegaPlus = new Button();
            btnZoomInPlus = new Button();
            btnNavForwPlus = new Button();
            panel3 = new Panel();
            label3 = new Label();
            pbStiro = new PictureBox();
            pbConf = new PictureBox();
            pbTess = new PictureBox();
            pbStaz = new PictureBox();
            miniTitle1 = new MiniTitle();
            lblChannels = new Label();
            _dgvGantt = new TableView();
            _panel1 = new Panel();
            splitContainer2 = new SplitContainer();
            btnNavBack = new Button();
            btnNavForwMega = new Button();
            btnZoomOut = new Button();
            btnNavForw = new Button();
            btnNavBackMega = new Button();
            btnZoomIn = new Button();
            lblNavig = new Label();
            panel2 = new Panel();
            label4 = new Label();
            pbStiro1 = new PictureBox();
            pbConf1 = new PictureBox();
            pbTess1 = new PictureBox();
            pbStaz1 = new PictureBox();
            btnReport = new Button();
            btnSaveAs = new Button();
            btnFilter = new Button();
            btnSpecSort = new Button();
            btnSwitch = new Button();
            btnOpen = new Button();
            btnPrint = new Button();
            btnSave = new Button();
            btnCleanse = new Button();
            mainTitle = new Title();
            displayStatus.SuspendLayout();
            _pnSideBar.SuspendLayout();
            ((ISupportInitialize)DisplaySplitContainer).BeginInit();
            DisplaySplitContainer.Panel1.SuspendLayout();
            DisplaySplitContainer.Panel2.SuspendLayout();
            DisplaySplitContainer.SuspendLayout();
            GanttContainer.SuspendLayout();
            pnNavPlus.SuspendLayout();
            ((ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            panel3.SuspendLayout();
            ((ISupportInitialize)pbStiro).BeginInit();
            ((ISupportInitialize)pbConf).BeginInit();
            ((ISupportInitialize)pbTess).BeginInit();
            ((ISupportInitialize)pbStaz).BeginInit();
            ((ISupportInitialize)_dgvGantt).BeginInit();
            _panel1.SuspendLayout();
            ((ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            panel2.SuspendLayout();
            ((ISupportInitialize)pbStiro1).BeginInit();
            ((ISupportInitialize)pbConf1).BeginInit();
            ((ISupportInitialize)pbTess1).BeginInit();
            ((ISupportInitialize)pbStaz1).BeginInit();
            SuspendLayout();
            // 
            // displayStatus
            // 
            displayStatus.ImageScalingSize = new Size(24, 24);
            displayStatus.Items.AddRange(new ToolStripItem[]
            {
                    DisplayStatusConsole,
                    lblDepthStatus,
                    lblDateTimeInterval,
                    toolStripSplitButton1
            });
            displayStatus.Location = new Point(0, 620);
            displayStatus.Name = "displayStatus";
            displayStatus.Size = new Size(1162, 30);
            displayStatus.TabIndex = 2;
            displayStatus.Text = "statusStrip1";
            // 
            // DisplayStatusConsole
            // 
            DisplayStatusConsole.Name = "DisplayStatusConsole";
            DisplayStatusConsole.Size = new Size(60, 25);
            DisplayStatusConsole.Text = "Ready";
            // 
            // lblDepthStatus
            // 
            lblDepthStatus.AutoToolTip = true;
            lblDepthStatus.Name = "lblDepthStatus";
            lblDepthStatus.Size = new Size(521, 25);
            lblDepthStatus.Spring = true;
            // 
            // lblDateTimeInterval
            // 
            lblDateTimeInterval.AutoToolTip = true;
            lblDateTimeInterval.Name = "lblDateTimeInterval";
            lblDateTimeInterval.Size = new Size(521, 25);
            lblDateTimeInterval.Spring = true;
            // 
            // toolStripSplitButton1
            // 
            toolStripSplitButton1.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripSplitButton1.DropDownItems.AddRange(new ToolStripItem[]
            {
                    showConsoleOutput,
                    hideConsoleOutput
            });
            toolStripSplitButton1.Image = Resources.outputfile;
            toolStripSplitButton1.ImageTransparentColor = Color.Magenta;
            toolStripSplitButton1.Name = "toolStripSplitButton1";
            toolStripSplitButton1.Size = new Size(45, 28);
            toolStripSplitButton1.Text = "toolStripSplitButton1";
            // 
            // showConsoleOutput
            // 
            showConsoleOutput.Name = "showConsoleOutput";
            showConsoleOutput.Size = new Size(265, 30);
            showConsoleOutput.Text = "Show console output";
            // 
            // hideConsoleOutput
            // 
            hideConsoleOutput.Name = "hideConsoleOutput";
            hideConsoleOutput.Size = new Size(265, 30);
            hideConsoleOutput.Text = "Hide console output";
            // 
            // _pnSideBar
            // 
            _pnSideBar.BackColor = Color.WhiteSmoke;
            _pnSideBar.BorderStyle = BorderStyle.None;
            _pnSideBar.Controls.Add(btnEndRemDays);
            _pnSideBar.Controls.Add(btnEndAddDays);
            _pnSideBar.Controls.Add(btnStartRemDays);
            _pnSideBar.Controls.Add(btnStartAddDays);
            _pnSideBar.Controls.Add(_btnStiro);
            _pnSideBar.Controls.Add(btnFullPrev);
            _pnSideBar.Controls.Add(BtnConf);
            _pnSideBar.Controls.Add(_btnColapseSideBar);
            _pnSideBar.Controls.Add(lblFiltersTit);
            _pnSideBar.Controls.Add(_btnTess);
            _pnSideBar.Controls.Add(_btnReload);
            _pnSideBar.Controls.Add(_btnFilterSelection);
            _pnSideBar.Controls.Add(_dtpDateTo);
            _pnSideBar.Controls.Add(label2);
            _pnSideBar.Controls.Add(_dtpDateFrom);
            _pnSideBar.Controls.Add(label1);
            _pnSideBar.Dock = DockStyle.Left;
            _pnSideBar.Location = new Point(0, 70);
            _pnSideBar.Name = "_pnSideBar";
            _pnSideBar.Size = new Size(176, 550);
            _pnSideBar.TabIndex = 5;
            // 
            // btnEndRemDays
            // 
            btnEndRemDays.Location = new Point(153, 89);
            btnEndRemDays.Name = "btnEndRemDays";
            btnEndRemDays.Size = new Size(18, 21);
            btnEndRemDays.TabIndex = 27;
            btnEndRemDays.Text = ">";
            btnEndRemDays.UseVisualStyleBackColor = true;
            // 
            // btnEndAddDays
            // 
            btnEndAddDays.Location = new Point(133, 89);
            btnEndAddDays.Name = "btnEndAddDays";
            btnEndAddDays.Size = new Size(18, 21);
            btnEndAddDays.TabIndex = 26;
            btnEndAddDays.Text = "<";
            btnEndAddDays.UseVisualStyleBackColor = true;
            // 
            // btnStartRemDays
            // 
            btnStartRemDays.Location = new Point(153, 31);
            btnStartRemDays.Name = "btnStartRemDays";
            btnStartRemDays.Size = new Size(18, 21);
            btnStartRemDays.TabIndex = 25;
            btnStartRemDays.Text = ">";
            btnStartRemDays.UseVisualStyleBackColor = true;
            // 
            // btnStartAddDays
            // 
            btnStartAddDays.Location = new Point(133, 31);
            btnStartAddDays.Name = "btnStartAddDays";
            btnStartAddDays.Size = new Size(18, 21);
            btnStartAddDays.TabIndex = 24;
            btnStartAddDays.Text = "<";
            btnStartAddDays.UseVisualStyleBackColor = true;
            // 
            // btn_Stiro
            // 
            _btnStiro.FlatAppearance.BorderSize = 0;
            _btnStiro.FlatStyle = FlatStyle.Flat;
            _btnStiro.Font = new Font("Microsoft Sans Serif", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            _btnStiro.Image = Resources.stiro;
            _btnStiro.ImageAlign = ContentAlignment.MiddleLeft;
            _btnStiro.Location = new Point(13, 454);
            _btnStiro.Name = "_btnStiro";
            _btnStiro.Size = new Size(117, 47);
            _btnStiro.TabIndex = 23;
            _btnStiro.Text = "Stiro";
            _btnStiro.TextAlign = ContentAlignment.MiddleRight;
            _btnStiro.UseVisualStyleBackColor = false;
            _btnStiro.Click += btn_Stiro_Click;
            // 
            // btn_Conf
            // 
            BtnConf.FlatAppearance.BorderSize = 0;
            BtnConf.FlatStyle = FlatStyle.Flat;
            BtnConf.Font = new Font("Microsoft Sans Serif", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            BtnConf.Image = Resources.conf;
            BtnConf.ImageAlign = ContentAlignment.MiddleLeft;
            BtnConf.Location = new Point(13, 401);
            BtnConf.Name = "BtnConf";
            BtnConf.Size = new Size(117, 47);
            BtnConf.TabIndex = 22;
            BtnConf.Text = "Confezione";
            BtnConf.TextAlign = ContentAlignment.MiddleRight;
            BtnConf.UseVisualStyleBackColor = false;
            BtnConf.Click += btn_Conf_Click;
            // 
            // _btnColapseSideBar
            // 
            _btnColapseSideBar.Dock = DockStyle.Bottom;
            _btnColapseSideBar.FlatAppearance.BorderSize = 0;
            _btnColapseSideBar.FlatStyle = FlatStyle.Flat;
            _btnColapseSideBar.Image = Resources.resize;
            _btnColapseSideBar.Location = new Point(0, 515);
            _btnColapseSideBar.Name = "_btnColapseSideBar";
            _btnColapseSideBar.Size = new Size(174, 33);
            _btnColapseSideBar.TabIndex = 5;
            _btnColapseSideBar.UseVisualStyleBackColor = true;
            _btnColapseSideBar.Click += btnColapseSideBar_Click_1;
            // 
            // lblFiltersTit
            // 
            lblFiltersTit.AutoSize = true;
            lblFiltersTit.Font = new Font("Microsoft Sans Serif", 8F, FontStyle.Underline, GraphicsUnit.Point, 0);
            lblFiltersTit.Location = new Point(9, 304);
            lblFiltersTit.Name = "lblFiltersTit";
            lblFiltersTit.Size = new Size(52, 20);
            lblFiltersTit.TabIndex = 13;
            lblFiltersTit.Text = "Filters";
            // 
            // btn_Tess
            // 
            _btnTess.FlatAppearance.BorderSize = 0;
            _btnTess.FlatStyle = FlatStyle.Flat;
            _btnTess.Font = new Font("Microsoft Sans Serif", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            _btnTess.Image = Resources.tess;
            _btnTess.ImageAlign = ContentAlignment.MiddleLeft;
            _btnTess.Location = new Point(13, 348);
            _btnTess.Name = "_btnTess";
            _btnTess.Size = new Size(117, 47);
            _btnTess.TabIndex = 21;
            _btnTess.Text = "Tessitura";
            _btnTess.TextAlign = ContentAlignment.MiddleRight;
            _btnTess.UseVisualStyleBackColor = false;
            _btnTess.Click += btn_Tess_Click;
            // 
            // _btnReload
            // 
            _btnReload.FlatAppearance.BorderSize = 0;
            _btnReload.FlatStyle = FlatStyle.Flat;
            _btnReload.Font = new Font("Microsoft Sans Serif", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            _btnReload.Image = Resources.refresh_40;
            _btnReload.ImageAlign = ContentAlignment.MiddleLeft;
            _btnReload.Location = new Point(13, 135);
            _btnReload.Name = "_btnReload";
            _btnReload.Text = "Reload";
            _btnReload.Size = new Size(117, 50);
            _btnReload.TabIndex = 4;
            _btnReload.TextAlign = ContentAlignment.MiddleRight;
            _btnReload.UseVisualStyleBackColor = true;
            _btnReload.Click += btnReload_Click_1;
            // 
            // btnFullPrev
            // 
            btnFullPrev.FlatAppearance.BorderSize = 0;
            btnFullPrev.FlatStyle = FlatStyle.Flat;
            btnFullPrev.Font = new Font("Microsoft Sans Serif", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnFullPrev.Image = Resources.full_40;
            btnFullPrev.ImageAlign = ContentAlignment.MiddleLeft;
            btnFullPrev.Location = new Point(13, 189);
            btnFullPrev.Name = "btnFullPrev";
            btnFullPrev.Text = "Full mode";
            btnFullPrev.Size = new Size(117, 50);
            btnFullPrev.TabIndex = 17;
            btnFullPrev.TextAlign = ContentAlignment.MiddleRight;
            btnFullPrev.UseVisualStyleBackColor = true;
            btnFullPrev.Click += _btnFullPrev_Click;
            // 
            // _btnFilterSelection
            // 
            _btnFilterSelection.FlatAppearance.BorderSize = 0;
            _btnFilterSelection.FlatStyle = FlatStyle.Flat;
            _btnFilterSelection.Font = new Font("Microsoft Sans Serif", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            _btnFilterSelection.Image = Resources.filter_40;
            _btnFilterSelection.ImageAlign = ContentAlignment.MiddleLeft;
            _btnFilterSelection.Location = new Point(13, 243);
            _btnFilterSelection.Name = "_btnFilterSelection";
            _btnFilterSelection.Text = "Selection";
            _btnFilterSelection.Size = new Size(117, 50);
            _btnFilterSelection.TabIndex = 5;
            _btnFilterSelection.TextAlign = ContentAlignment.MiddleRight;
            _btnFilterSelection.UseVisualStyleBackColor = true;
            _btnFilterSelection.Click += btnFilterSelection_Click;
            // 
            // _dtpDateTo
            // 
            _dtpDateTo.CustomFormat = "dd/MM/yyyy";
            _dtpDateTo.Font = new Font("Microsoft Sans Serif", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            _dtpDateTo.Format = DateTimePickerFormat.Custom;
            _dtpDateTo.Location = new Point(19, 96);
            _dtpDateTo.Name = "_dtpDateTo";
            _dtpDateTo.Size = new Size(111, 26);
            _dtpDateTo.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Microsoft Sans Serif", 8F, FontStyle.Underline, GraphicsUnit.Point, 0);
            label2.Location = new Point(15, 73);
            label2.Name = "label2";
            label2.Size = new Size(27, 20);
            label2.TabIndex = 2;
            label2.Text = "To";
            // 
            // _dtpDateFrom
            // 
            _dtpDateFrom.CustomFormat = "dd/MM/yyyy";
            _dtpDateFrom.Font = new Font("Microsoft Sans Serif", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            _dtpDateFrom.Format = DateTimePickerFormat.Custom;
            _dtpDateFrom.Location = new Point(19, 38);
            _dtpDateFrom.Name = "_dtpDateFrom";
            _dtpDateFrom.Size = new Size(111, 26);
            _dtpDateFrom.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft Sans Serif", 8F, FontStyle.Underline, GraphicsUnit.Point, 0);
            label1.Location = new Point(15, 15);
            label1.Name = "label1";
            label1.Size = new Size(46, 20);
            label1.TabIndex = 0;
            label1.Text = "From";
            // 
            // DisplaySplitContainer
            // 
            DisplaySplitContainer.BackColor = Color.Gainsboro;
            DisplaySplitContainer.Dock = DockStyle.Fill;
            DisplaySplitContainer.Location = new Point(176, 70);
            DisplaySplitContainer.Name = "DisplaySplitContainer";
            DisplaySplitContainer.Orientation = Orientation.Horizontal;
            // 
            // DisplaySplitContainer.Panel1
            // 
            DisplaySplitContainer.Panel1.Controls.Add(GanttContainer);
            // 
            // DisplaySplitContainer.Panel2
            // 
            DisplaySplitContainer.Panel2.Controls.Add(_dgvGantt);
            DisplaySplitContainer.Panel2.Controls.Add(_panel1);
            DisplaySplitContainer.Panel2MinSize = 100;
            DisplaySplitContainer.Size = new Size(986, 550);
            DisplaySplitContainer.SplitterDistance = 305;
            DisplaySplitContainer.TabIndex = 3;
            // 
            // ganttContainer
            // 
            GanttContainer.Dock = DockStyle.Fill;
            GanttContainer.Location = new Point(0, 0);
            GanttContainer.Name = "ganttContainer";
            GanttContainer.Controls.Add(miniTitle1); 
            ganttContainer.Controls.Add(pnNavPlus);
            GanttContainer.BackColor = Color.Gainsboro;
            // 
            // dgvRoot
            // 
            dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = SystemColors.Control;
            dataGridViewCellStyle8.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);
            dataGridViewCellStyle8.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle8.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = DataGridViewTriState.True;
            dataGridViewCellStyle9.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = SystemColors.Control;
            dataGridViewCellStyle9.Font =
                new Font("Microsoft Sans Serif", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle9.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = Color.SteelBlue;
            dataGridViewCellStyle9.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = DataGridViewTriState.True;
            dataGridViewCellStyle10.Font = new Font("Microsoft Sans Serif", 9F);
            dataGridViewCellStyle10.SelectionBackColor = Color.SteelBlue;
            dataGridViewCellStyle10.SelectionForeColor = Color.White;
            // 
            // pnSplitter1
            // 
            pnSplitter1.BackColor = Color.WhiteSmoke;
            pnSplitter1.Dock = DockStyle.Bottom;
            pnSplitter1.Location = new Point(0, 265);
            pnSplitter1.Name = "pnSplitter1";
            pnSplitter1.Size = new Size(25, 40);
            pnSplitter1.TabIndex = 2;
            pnSplitter1.Visible = false;
            // 
            // pnNavPlus
            // 
            pnNavPlus.BackColor = Color.WhiteSmoke;
            pnNavPlus.BorderStyle = BorderStyle.FixedSingle;
            pnNavPlus.Controls.Add(splitContainer1);
            pnNavPlus.Dock = DockStyle.Bottom;
            pnNavPlus.Location = new Point(0, 265);
            pnNavPlus.Name = "pnNavPlus";
            pnNavPlus.Size = new Size(957, 40);
            pnNavPlus.TabIndex = 1;
            pnNavPlus.Visible = false;
            // 
            // splitContainer1
            // 
            splitContainer1.BorderStyle = BorderStyle.Fixed3D;
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(btnNavBackPlus);
            splitContainer1.Panel1.Controls.Add(btnNavBackMegaPlus);
            splitContainer1.Panel1.Controls.Add(btnZoomOutPlus);
            splitContainer1.Panel1.Controls.Add(btnNavForwMegaPlus);
            splitContainer1.Panel1.Controls.Add(btnZoomInPlus);
            splitContainer1.Panel1.Controls.Add(btnNavForwPlus);
           
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(panel3);
            splitContainer1.Size = new Size(955, 38);
            splitContainer1.SplitterDistance = 635;
            splitContainer1.TabIndex = 24;
            // 
            // btnNavBackPlus
            // 
            btnNavBackPlus.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnNavBackPlus.FlatAppearance.BorderSize = 0;
            btnNavBackPlus.Image = Resources.back; //(Image)resources.GetObject("btnNavBackPlus.Image");
            btnNavBackPlus.Location = new Point(352, 2);
            btnNavBackPlus.Name = "btnNavBackPlus";
            btnNavBackPlus.Size = new Size(33, 33);
            btnNavBackPlus.TabIndex = 17;
            btnNavBackPlus.UseVisualStyleBackColor = false;
            btnNavBackPlus.Click += btnNavBack_Click;
            // 
            // btnNavBackMegaPlus
            // 
            btnNavBackMegaPlus.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnNavBackMegaPlus.FlatAppearance.BorderSize = 0;
            btnNavBackMegaPlus.Image = Resources.backback;
            btnNavBackMegaPlus.Location = new Point(391, 2);
            btnNavBackMegaPlus.Name = "btnNavBackMegaPlus";
            btnNavBackMegaPlus.Size = new Size(33, 33);
            btnNavBackMegaPlus.TabIndex = 18;
            btnNavBackMegaPlus.UseVisualStyleBackColor = false;
            btnNavBackMegaPlus.Click += btnNavBackDoub_Click;
            // 
            // btnZoomOutPlus
            // 
            btnZoomOutPlus.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnZoomOutPlus.FlatAppearance.BorderSize = 0;
            btnZoomOutPlus.Image = Resources.zoomOut;
            btnZoomOutPlus.Location = new Point(547, 2);
            btnZoomOutPlus.Name = "btnZoomOutPlus";
            btnZoomOutPlus.Size = new Size(33, 33);
            btnZoomOutPlus.TabIndex = 22;
            btnZoomOutPlus.UseVisualStyleBackColor = false;
            btnZoomOutPlus.Click += btnZoomOut_Click;
            // 
            // btnNavForwMegaPlus
            // 
            btnNavForwMegaPlus.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnNavForwMegaPlus.FlatAppearance.BorderSize = 0;
            btnNavForwMegaPlus.Image = Resources.forwforw;
            btnNavForwMegaPlus.Location = new Point(430, 2);
            btnNavForwMegaPlus.Name = "btnNavForwMegaPlus";
            btnNavForwMegaPlus.Size = new Size(33, 33);
            btnNavForwMegaPlus.TabIndex = 19;
            btnNavForwMegaPlus.UseVisualStyleBackColor = false;
            btnNavForwMegaPlus.Click += btnNavForwDoub_Click;
            // 
            // btnZoomInPlus
            // 
            btnZoomInPlus.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnZoomInPlus.FlatAppearance.BorderSize = 0;
            btnZoomInPlus.Image = Resources.zoomIn;
            btnZoomInPlus.Location = new Point(509, 2);
            btnZoomInPlus.Name = "btnZoomInPlus";
            btnZoomInPlus.Size = new Size(33, 33);
            btnZoomInPlus.TabIndex = 21;
            btnZoomInPlus.UseVisualStyleBackColor = false;
            btnZoomInPlus.Click += btnZoomIn_Click;
            // 
            // btnNavForwPlus
            // 
            btnNavForwPlus.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnNavForwPlus.FlatAppearance.BorderSize = 0;
            btnNavForwPlus.Image = Resources.forward;
            btnNavForwPlus.Location = new Point(470, 2);
            btnNavForwPlus.Name = "btnNavForwPlus";
            btnNavForwPlus.Size = new Size(33, 33);
            btnNavForwPlus.TabIndex = 20;
            btnNavForwPlus.UseVisualStyleBackColor = false;
            btnNavForwPlus.Click += btnNavForw_Click;
            // 
            // panel3
            // 
            panel3.Controls.Add(label3);
            panel3.Controls.Add(pbStiro);
            panel3.Controls.Add(pbConf);
            panel3.Controls.Add(pbTess);
            panel3.Controls.Add(pbStaz);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(0, 0);
            panel3.Name = "panel3";
            panel3.Size = new Size(312, 34);
            panel3.TabIndex = 23;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.ForeColor = Color.DarkGray;
            label3.Location = new Point(3, 2);
            label3.Name = "label3";
            label3.Size = new Size(89, 40);
            label3.TabIndex = 9;
            label3.Text = "Legend\r\nin the order";
            // 
            // pbStiro
            // 
            pbStiro.BackColor = Color.LightGreen;
            pbStiro.BorderStyle = BorderStyle.FixedSingle;
            pbStiro.Location = new Point(201, 11);
            pbStiro.Name = "pbStiro";
            pbStiro.Size = new Size(19, 17);
            pbStiro.TabIndex = 7;
            pbStiro.TabStop = false;
            // 
            // pbConf
            // 
            pbConf.BackColor = Color.Violet;
            pbConf.BorderStyle = BorderStyle.FixedSingle;
            pbConf.Location = new Point(176, 11);
            pbConf.Name = "pbConf";
            pbConf.Size = new Size(19, 17);
            pbConf.TabIndex = 4;
            pbConf.TabStop = false;
            // 
            // pbTess
            // 
            pbTess.BackColor = Color.DarkTurquoise;
            pbTess.BorderStyle = BorderStyle.FixedSingle;
            pbTess.Location = new Point(151, 11);
            pbTess.Name = "pbTess";
            pbTess.Size = new Size(19, 17);
            pbTess.TabIndex = 1;
            pbTess.TabStop = false;
            // 
            // pbStaz
            // 
            pbStaz.BackColor = Color.Silver;
            pbStaz.BorderStyle = BorderStyle.FixedSingle;
            pbStaz.Location = new Point(126, 11);
            pbStaz.Name = "pbStaz";
            pbStaz.Size = new Size(19, 17);
            pbStaz.TabIndex = 0;
            pbStaz.TabStop = false;
            //
            // lblChannels
            //
            lblChannels.BackColor = Color.DimGray;
            lblChannels.ForeColor = Color.WhiteSmoke;
            //lblChannels.Dock = DockStyle.Top;
            lblChannels.Location = new Point(150, 28);
            lblChannels.Margin = new Padding(4, 5, 4, 5);
            lblChannels.Font = new Font("Microsoft Sans Serif", 8, FontStyle.Bold);
            lblChannels.Name = "lblChannels";
            lblChannels.Width = 250;
            //lblChannels.TabIndex = 0;
            lblChannels.Text = "Tessitura";
            //
            // lblChannels
            //
            lblNavig.BackColor = Color.WhiteSmoke;
            lblNavig.ForeColor = Color.DimGray;
            //lblChannels.Dock = DockStyle.Top;
            lblNavig.Location = new Point(10, 10);
            lblNavig.Margin = new Padding(4, 5, 4, 5);
            lblNavig.Name = "lblChannels";
            lblNavig.Width = 250;
            //lblChannels.TabIndex = 0;
            lblNavig.Text = "";
            // 
            // miniTitle1
            // 
            miniTitle1.BackColor = Color.White;
            miniTitle1.Dock = DockStyle.Top;
            miniTitle1.Location = new Point(0, 0);
            miniTitle1.Margin = new Padding(4, 5, 4, 5);
            miniTitle1.Name = "miniTitle1";
            miniTitle1.Size = new Size(957, 40);
            miniTitle1.TabIndex = 0;
            miniTitle1.TitleText = "";

            // dgvGantt
            // 
            _dgvGantt.AllowUserToAddRows = false;
            _dgvGantt.AllowUserToDeleteRows = false;
            _dgvGantt.AllowUserToResizeColumns = false;
            _dgvGantt.AllowUserToResizeRows = false;
            _dgvGantt.BackgroundColor = Color.FromArgb(235, 235, 235);
            _dgvGantt.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dataGridViewCellStyle11.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.BackColor = SystemColors.Control;
            dataGridViewCellStyle11.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);
            dataGridViewCellStyle11.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle11.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle11.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle11.WrapMode = DataGridViewTriState.True;
            _dgvGantt.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle11;
            _dgvGantt.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle12.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle12.BackColor = SystemColors.Window;
            dataGridViewCellStyle12.Font =
                new Font("Microsoft Sans Serif", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle12.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle12.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle12.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle12.WrapMode = DataGridViewTriState.False;
            _dgvGantt.DefaultCellStyle = dataGridViewCellStyle12;
            _dgvGantt.Dock = DockStyle.Fill;
            _dgvGantt.EnableHeadersVisualStyles = false;
            _dgvGantt.GridColor = Color.White;
            _dgvGantt.Location = new Point(0, 40);
            _dgvGantt.MultiSelect = false;
            _dgvGantt.Name = "_dgvGantt";
            _dgvGantt.ReadOnly = true;
            dataGridViewCellStyle13.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle13.BackColor = SystemColors.Control;
            dataGridViewCellStyle13.Font =
                new Font("Microsoft Sans Serif", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle13.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle13.SelectionBackColor = Color.SteelBlue;
            dataGridViewCellStyle13.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle13.WrapMode = DataGridViewTriState.True;
            _dgvGantt.RowHeadersDefaultCellStyle = dataGridViewCellStyle13;
            _dgvGantt.RowHeadersVisible = false;
            dataGridViewCellStyle14.Font = new Font("Microsoft Sans Serif", 9F);
            dataGridViewCellStyle14.SelectionBackColor = Color.SteelBlue;
            dataGridViewCellStyle14.SelectionForeColor = Color.White;
            _dgvGantt.RowsDefaultCellStyle = dataGridViewCellStyle14;
            _dgvGantt.RowTemplate.Height = 28;
            _dgvGantt.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            _dgvGantt.Size = new Size(986, 201);
            _dgvGantt.TabIndex = 1;
            // 
            // panel1
            // 
            _panel1.BackColor = Color.WhiteSmoke;
            _panel1.BorderStyle = BorderStyle.FixedSingle;
            _panel1.Controls.Add(splitContainer2);
            _panel1.Dock = DockStyle.Top;
            _panel1.Location = new Point(0, 0);
            _panel1.Name = "_panel1";
            _panel1.Size = new Size(986, 40);
            _panel1.TabIndex = 17;
            // 
            // splitContainer2
            // 
            splitContainer2.BorderStyle = BorderStyle.Fixed3D;
            splitContainer2.Dock = DockStyle.Fill;
            splitContainer2.Location = new Point(0, 0);
            splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(btnNavBack);
            splitContainer2.Panel1.Controls.Add(btnNavForwMega);
            splitContainer2.Panel1.Controls.Add(btnZoomOut);
            splitContainer2.Panel1.Controls.Add(btnNavForw);
            splitContainer2.Panel1.Controls.Add(btnNavBackMega);
            splitContainer2.Panel1.Controls.Add(btnZoomIn);
            splitContainer2.Panel1.Controls.Add(lblNavig);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(panel2);
            splitContainer2.Size = new Size(984, 38);
            splitContainer2.SplitterDistance = 664;
            splitContainer2.TabIndex = 18;
            // 
            // btnNavBack
            // 
            btnNavBack.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnNavBack.FlatAppearance.BorderSize = 0;
            btnNavBack.Image = Resources.back; //(Image)resources.GetObject("btnNavBack.Image");
            btnNavBack.Location = new Point(381, 2);
            btnNavBack.Name = "btnNavBack";
            btnNavBack.Size = new Size(33, 33);
            btnNavBack.TabIndex = 10;
            btnNavBack.UseVisualStyleBackColor = false;
            btnNavBack.Click += btnNavBack_Click;
            // 
            // btnNavForwMega
            // 
            btnNavForwMega.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnNavForwMega.FlatAppearance.BorderSize = 0;
            btnNavForwMega.Image = Resources.forwforw;
            btnNavForwMega.Location = new Point(459, 2);
            btnNavForwMega.Name = "btnNavForwMega";
            btnNavForwMega.Size = new Size(33, 33);
            btnNavForwMega.TabIndex = 12;
            btnNavForwMega.UseVisualStyleBackColor = false;
            btnNavForwMega.Click += btnNavForwDoub_Click;
            // 
            // btnZoomOut
            // 
            btnZoomOut.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnZoomOut.FlatAppearance.BorderSize = 0;
            btnZoomOut.Image = Resources.zoomOut;
            btnZoomOut.Location = new Point(575, 2);
            btnZoomOut.Name = "btnZoomOut";
            btnZoomOut.Size = new Size(33, 33);
            btnZoomOut.TabIndex = 16;
            btnZoomOut.UseVisualStyleBackColor = false;
            btnZoomOut.Click += btnZoomOut_Click;
            // 
            // btnNavForw
            // 
            btnNavForw.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnNavForw.FlatAppearance.BorderSize = 0;
            btnNavForw.Image = Resources.forward;
            btnNavForw.Location = new Point(498, 2);
            btnNavForw.Name = "btnNavForw";
            btnNavForw.Size = new Size(33, 33);
            btnNavForw.TabIndex = 13;
            btnNavForw.UseVisualStyleBackColor = false;
            btnNavForw.Click += btnNavForw_Click;
            // 
            // btnNavBackMega
            // 
            btnNavBackMega.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnNavBackMega.FlatAppearance.BorderSize = 0;
            btnNavBackMega.Image = Resources.backback;
            btnNavBackMega.Location = new Point(420, 2);
            btnNavBackMega.Name = "btnNavBackMega";
            btnNavBackMega.Size = new Size(33, 33);
            btnNavBackMega.TabIndex = 11;
            btnNavBackMega.UseVisualStyleBackColor = false;
            btnNavBackMega.Click += btnNavBackDoub_Click;
            // 
            // btnZoomIn
            // 
            btnZoomIn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnZoomIn.FlatAppearance.BorderSize = 0;
            btnZoomIn.Image = Resources.zoomIn;
            btnZoomIn.Location = new Point(537, 2);
            btnZoomIn.Name = "btnZoomIn";
            btnZoomIn.Size = new Size(33, 33);
            btnZoomIn.TabIndex = 15;
            btnZoomIn.UseVisualStyleBackColor = false;
            btnZoomIn.Click += btnZoomIn_Click;
            // 
            // panel2
            // 
            panel2.Controls.Add(label4);
            panel2.Controls.Add(pbStiro1);
            panel2.Controls.Add(pbConf1);
            panel2.Controls.Add(pbTess1);
            panel2.Controls.Add(pbStaz1);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(312, 34);
            panel2.TabIndex = 17;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.ForeColor = Color.DarkGray;
            label4.Location = new Point(3, 1);
            label4.Name = "label4";
            label4.Size = new Size(89, 40);
            label4.TabIndex = 10;
            label4.Text = "Legend\r\nin the order";
            // 
            // pbStiro1
            // 
            pbStiro1.BackColor = Color.LightGreen;
            pbStiro1.BorderStyle = BorderStyle.FixedSingle;
            pbStiro1.Location = new Point(201, 11);
            pbStiro1.Name = "pbStiro1";
            pbStiro1.Size = new Size(19, 17);
            pbStiro1.TabIndex = 7;
            pbStiro1.TabStop = false;
            // 
            // pbConf1
            // 
            pbConf1.BackColor = Color.Violet;
            pbConf1.BorderStyle = BorderStyle.FixedSingle;
            pbConf1.Location = new Point(176, 11);
            pbConf1.Name = "pbConf1";
            pbConf1.Size = new Size(19, 17);
            pbConf1.TabIndex = 4;
            pbConf1.TabStop = false;
            // 
            // pbTess1
            // 
            pbTess1.BackColor = Color.DarkTurquoise;
            pbTess1.BorderStyle = BorderStyle.FixedSingle;
            pbTess1.Location = new Point(151, 11);
            pbTess1.Name = "pbTess1";
            pbTess1.Size = new Size(19, 17);
            pbTess1.TabIndex = 1;
            pbTess1.TabStop = false;
            // 
            // pbStaz1
            // 
            pbStaz1.BackColor = Color.Silver;
            pbStaz1.BorderStyle = BorderStyle.FixedSingle;
            pbStaz1.Location = new Point(126, 11);
            pbStaz1.Name = "pbStaz1";
            pbStaz1.Size = new Size(19, 17);
            pbStaz1.TabIndex = 0;
            pbStaz1.TabStop = false;
            // 
            // btnReport
            // 
            btnReport.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnReport.BackColor = Color.DimGray;
            btnReport.FlatAppearance.BorderSize = 0;
            btnReport.FlatStyle = FlatStyle.Flat;
            btnReport.ForeColor = Color.White;
            btnReport.Image = Resources.report;
            btnReport.ImageAlign = ContentAlignment.TopCenter;
            btnReport.Location = new Point(1056, 5);
            btnReport.Name = "btnReport";
            btnReport.Size = new Size(93, 65);
            btnReport.TabIndex = 15;
            btnReport.Text = "Reports ▼";
            btnReport.TextAlign = ContentAlignment.BottomCenter;
            btnReport.UseVisualStyleBackColor = false;
            btnReport.Click += btnReport_Click;
            // 
            // btnSaveAs
            // 
            btnSaveAs.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSaveAs.BackColor = Color.DimGray;
            btnSaveAs.Enabled = false;
            btnSaveAs.FlatAppearance.BorderSize = 0;
            btnSaveAs.FlatStyle = FlatStyle.Flat;
            btnSaveAs.ForeColor = Color.White;
            btnSaveAs.Image = Resources.save1;
            btnSaveAs.ImageAlign = ContentAlignment.TopCenter;
            btnSaveAs.Location = new Point(673, 5);
            btnSaveAs.Name = "btnSaveAs";
            btnSaveAs.Size = new Size(79, 65);
            btnSaveAs.TabIndex = 14;
            btnSaveAs.Text = "Save As";
            btnSaveAs.TextAlign = ContentAlignment.BottomCenter;
            btnSaveAs.UseVisualStyleBackColor = false;
            // 
            // btnFilter
            // 
            btnFilter.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnFilter.BackColor = Color.DimGray;
            btnFilter.FlatAppearance.BorderSize = 0;
            btnFilter.FlatStyle = FlatStyle.Flat;
            btnFilter.ForeColor = Color.White;
            btnFilter.Image = Resources.filter;
            btnFilter.ImageAlign = ContentAlignment.TopCenter;
            btnFilter.Location = new Point(832, 5);
            btnFilter.Name = "btnFilter";
            btnFilter.Size = new Size(70, 65);
            btnFilter.TabIndex = 13;
            btnFilter.Text = "Filter";
            btnFilter.TextAlign = ContentAlignment.BottomCenter;
            btnFilter.UseVisualStyleBackColor = false;
            btnFilter.Click += btnFilter_Click;
            // 
            // btnResize
            // 
            btnSpecSort.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSpecSort.BackColor = Color.DimGray;
            btnSpecSort.FlatAppearance.BorderSize = 0;
            btnSpecSort.FlatStyle = FlatStyle.Flat;
            btnSpecSort.ForeColor = Color.White;
            btnSpecSort.Image = Resources.sort_32;
            btnSpecSort.ImageAlign = ContentAlignment.TopCenter;
            btnSpecSort.Location = new Point(905, 5);
            btnSpecSort.Name = "btnSpecSort";
            btnSpecSort.Size = new Size(70, 65);
            btnSpecSort.TabIndex = 12;
            btnSpecSort.Text = "Sort";
            btnSpecSort.TextAlign = ContentAlignment.BottomCenter;
            btnSpecSort.UseVisualStyleBackColor = false;
            btnSpecSort.Click += btnSpecSort_Click;
            // 
            // btnSwitch
            // 
            btnSwitch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSwitch.BackColor = Color.DimGray;
            btnSwitch.FlatAppearance.BorderSize = 0;
            btnSwitch.FlatStyle = FlatStyle.Flat;
            btnSwitch.ForeColor = Color.White;
            btnSwitch.Image = Resources.switch1;
            btnSwitch.ImageAlign = ContentAlignment.TopCenter;
            btnSwitch.Location = new Point(981, 5);
            btnSwitch.Name = "btnSwitch";
            btnSwitch.Size = new Size(70, 65);
            btnSwitch.TabIndex = 9;
            btnSwitch.Text = "Switch";
            btnSwitch.TextAlign = ContentAlignment.BottomCenter;
            btnSwitch.UseVisualStyleBackColor = false;
            btnSwitch.Click += btnSwitch_Click;
            // 
            // btnCleanse
            // 
            btnCleanse.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCleanse.BackColor = Color.DimGray;
            btnCleanse.Enabled = true;
            btnCleanse.FlatAppearance.BorderSize = 0;
            btnCleanse.FlatStyle = FlatStyle.Flat;
            btnCleanse.ForeColor = Color.White;
            btnCleanse.Image = Resources.cleanse;
            btnCleanse.ImageAlign = ContentAlignment.TopCenter;
            btnCleanse.Location = new Point(444, 5);
            btnCleanse.Name = "btnCleanse";
            btnCleanse.Size = new Size(79, 65);
            btnCleanse.TabIndex = 14;
            btnCleanse.Text = "Cleanse";
            btnCleanse.TextAlign = ContentAlignment.BottomCenter;
            btnCleanse.UseVisualStyleBackColor = false;
            btnCleanse.Click += btnCleanse_Click;
            // 
            // btnOpen
            // 
            btnOpen.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnOpen.BackColor = Color.DimGray;
            btnOpen.FlatAppearance.BorderSize = 0;
            btnOpen.FlatStyle = FlatStyle.Flat;
            btnOpen.ForeColor = Color.White;
            btnOpen.Image = Resources.fileopen;
            btnOpen.ImageAlign = ContentAlignment.TopCenter;
            btnOpen.Location = new Point(521, 5);
            btnOpen.Name = "btnOpen";
            btnOpen.Size = new Size(70, 65);
            btnOpen.TabIndex = 8;
            btnOpen.Text = "Open";
            btnOpen.TextAlign = ContentAlignment.BottomCenter;
            btnOpen.UseVisualStyleBackColor = false;
            btnOpen.Click += btnOpen_Click;
            // 
            // btnPrint
            // 
            btnPrint.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnPrint.BackColor = Color.DimGray;
            btnPrint.FlatAppearance.BorderSize = 0;
            btnPrint.FlatStyle = FlatStyle.Flat;
            btnPrint.ForeColor = Color.White;
            btnPrint.Image = Resources.print1;
            btnPrint.ImageAlign = ContentAlignment.TopCenter;
            btnPrint.Location = new Point(758, 5);
            btnPrint.Name = "btnPrint";
            btnPrint.Size = new Size(70, 65);
            btnPrint.TabIndex = 7;
            btnPrint.Text = "Print";
            btnPrint.TextAlign = ContentAlignment.BottomCenter;
            btnPrint.UseVisualStyleBackColor = false;
            btnPrint.Click += btnPrint_Click;
            // 
            // btnSave
            // 
            btnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSave.BackColor = Color.DimGray;
            btnSave.Enabled = false;
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.ForeColor = Color.White;
            btnSave.Image = Resources.save;
            btnSave.ImageAlign = ContentAlignment.TopCenter;
            btnSave.Location = new Point(597, 5);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(70, 65);
            btnSave.TabIndex = 6;
            btnSave.Text = "Save";
            btnSave.TextAlign = ContentAlignment.BottomCenter;
            btnSave.UseVisualStyleBackColor = false;
            // 
            // mainTitle
            // 
            mainTitle.BackColor = Color.DimGray;
            mainTitle.Dock = DockStyle.Top;
            mainTitle.Location = new Point(0, 0);
            mainTitle.Margin = new Padding(4, 5, 4, 5);
            mainTitle.Name = "mainTitle";
            mainTitle.Size = new Size(1162, 70);
            mainTitle.TabIndex = 4;
            mainTitle.TitleText = "";
            // 
            // Display
            // 
            BackColor = Color.White;
            ClientSize = new Size(1162, 650);
            Controls.Add(btnReport);
            Controls.Add(btnSaveAs);
            Controls.Add(btnFilter);
            Controls.Add(btnSpecSort);
            Controls.Add(btnSwitch);
            Controls.Add(btnOpen);
            Controls.Add(btnCleanse);
            Controls.Add(btnPrint);
            Controls.Add(btnSave);
            Controls.Add(DisplaySplitContainer);
            Controls.Add(_pnSideBar);
            Controls.Add(displayStatus);
            Controls.Add(mainTitle);
            Controls.Add(lblChannels);
            lblChannels.BringToFront();
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(700, 400);
            Name = "Display";
            Text = "Avanzamento commesse";
            WindowState = FormWindowState.Maximized;
            displayStatus.ResumeLayout(false);
            displayStatus.PerformLayout();
            _pnSideBar.ResumeLayout(false);
            _pnSideBar.PerformLayout();
            DisplaySplitContainer.Panel1.ResumeLayout(false);
            DisplaySplitContainer.Panel2.ResumeLayout(false);
            ((ISupportInitialize)DisplaySplitContainer).EndInit();
            DisplaySplitContainer.ResumeLayout(false);
            GanttContainer.ResumeLayout(false);
            pnNavPlus.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ((ISupportInitialize)pbStiro).EndInit();
            ((ISupportInitialize)pbConf).EndInit();
            ((ISupportInitialize)pbTess).EndInit();
            ((ISupportInitialize)pbStaz).EndInit();
            ((ISupportInitialize)_dgvGantt).EndInit();
            _panel1.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            ((ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((ISupportInitialize)pbStiro1).EndInit();
            ((ISupportInitialize)pbConf1).EndInit();
            ((ISupportInitialize)pbTess1).EndInit();
            ((ISupportInitialize)pbStaz1).EndInit();
            _gantt = new Ganttogram
                {
                Dock = DockStyle.Fill,
                BackColor = Color.WhiteSmoke,
                FromDate = _config.StartDate,
                ToDate = _config.EndDate,
                };
            GanttContainer.Controls.Add(_gantt);
            //_gantt.DoubleBuffered(true);
            _gantt.BringToFront();
            _gantt.MouseMove += _gantt.Chart_MouseMove;
            _gantt.MouseDragged += _gantt.Chart_MouseDragged;
            _gantt.MouseLeave += _gantt.Chart_MouseLeave;
            _gantt.MouseDown += _gantt.Chart_MouseDown;
            ResumeLayout(false);
            PerformLayout();
            }
        }
    }
