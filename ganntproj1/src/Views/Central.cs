namespace ganntproj1
{
    using ganntproj1.Models;
    using ganntproj1.src.Helpers;
    using ganntproj1.src.Views;
    using ganntproj1.Views;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Data.Linq;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using System.Xml.Linq;

    public partial class Central : Form
    {
        public enum SettingsSys
        {
            Department,
            Shift,
            Line,
            Backup,
            Server,
            Completed,
            NewUpdate,
        }

        private IntPtr _console = new IntPtr();
        public static string SpecialConnStr = "data source=192.168.96.17;initial catalog=Ganttproj; User ID=sa; password=onlyouolimpias;";
        //public static string SpecialConnStr = "data source=192.168.96.17;initial catalog=Ganttproj; User ID=sa; password=onlyouolimpias;";
        public static string ConnStr = "data source=192.168.96.37;initial catalog=ONLYOU; User ID=nicu; password=onlyouolimpias;";

        public static List<JobModel> TaskList { get; set; }
        public static DateTime DateFrom { get; private set; }
        public static DateTime DateTo { get; private set; }
        public static TimeSpan ShiftFrom { get; private set; }
        public static TimeSpan ShiftTo { get; private set; }
        public static bool IsDvc { get; private set; }
        public static bool IsRdd { get; private set; }
        public static bool IsAcconto { get; private set; }
        public static bool IsSaldo { get; private set; }
        public static bool IsChiuso { get; private set; }
        public static bool IsProgramare { get; set; }
        public static bool IsResetJobLoader { get; set; }
        public static SettingsSys SettingsCompleted { get; set; }
        public static System.Text.StringBuilder IdStateArray { get; private set; }
        public static bool IsArticleSelection { get; set; }
        public static bool IsQtySelection { get; set; }
        public static bool IsActiveOrdersSelection { get; set; }
        
        public static List<Lines> ListOfLines = new List<Lines>();
        public static int LowEff { get; set; }
        public static int MediumEff { get; set; }
        public static int HighEff { get; set; }
        public static Color LowColor { get; set; }
        public static Color MediumColor { get; set; }
        public static Color HighColor { get; set; }
        public string RefreshTitle { get; set; }
        public string SectorTitle { get; set; }
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();
        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        private const int SwHide = 0;
        private Settings _settings = new Settings();

        #region FormMovementService

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        ReSize resize = new ReSize();

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
            (
        int nLeftRect,
        int nTopRect,
        int nRightRect,
        int nBottomRect,
        int nWidthEllipse,
        int nHeightEllipse

            );
        [DllImport("dwmapi.dll")]
        public static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref Margins pMarInset);

        [DllImport("dwmapi.dll")]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        [DllImport("dwmapi.dll")]
        public static extern int DwmIsCompositionEnabled(ref int pfEnabled);

        private bool m_aeroEnabled;
        private const int CS_DROPSHADOW = 0x00020000;
        private const int WM_NCPAINT = 0x0085;

        public struct Margins
        {
            public int leftWidth;
            public int rightWidth;
            public int topHeight;
            public int bottomHeight;
        }

        private const int WM_NCHITTEST = 0x84;
        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;

        protected override CreateParams CreateParams
        {
            get
            {
                m_aeroEnabled = CheckAeroEnabled();

                CreateParams cp = base.CreateParams;
                if (m_aeroEnabled)
                    cp.ClassStyle |= CS_DROPSHADOW;

                return cp;
            }
        }

        private bool CheckAeroEnabled()
        {
            if (Environment.OSVersion.Version.Major >= 6)
            {
                int enabled = 0;
                DwmIsCompositionEnabled(ref enabled);
                return (enabled == 1) ? true : false;
            }
            return false;
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WmLbuttondown || (m.Msg == WmParentnotify && (int)m.WParam == WmLbuttondown))
            {
                if (!pnReload.ClientRectangle.Contains(
                                pnReload.PointToClient(Cursor.Position)))
                {
                    pnReload.Visible = false;
                }
            }

            int x = (int)(m.LParam.ToInt64() & 0xFFFF);
            int y = (int)((m.LParam.ToInt64() & 0xFFFF0000) >> 16);
            Point pt = PointToClient(new Point(x, y));

            if (m.Msg == 0x84)
            {
                switch (resize.getMosuePosition(pt, this))
                {
                    case "l": m.Result = (IntPtr)10; return;
                    case "r": m.Result = (IntPtr)11; return;
                    case "a": m.Result = (IntPtr)12; return;
                    case "la": m.Result = (IntPtr)13; return;
                    case "ra": m.Result = (IntPtr)14; return;
                    case "u": m.Result = (IntPtr)15; return;
                    case "lu": m.Result = (IntPtr)16; return;
                    case "ru": m.Result = (IntPtr)17; return;
                    case "": m.Result = pt.Y < 32 ? (IntPtr)2 : (IntPtr)1; return;

                }
            }

            switch (m.Msg)
            {
                case WM_NCPAINT:
                    if (m_aeroEnabled)
                    {
                        var v = 2;
                        DwmSetWindowAttribute(this.Handle, 2, ref v, 4);
                        Margins margins = new Margins()
                        {
                            bottomHeight = 1,
                            leftWidth = 1,
                            rightWidth = 1,
                            topHeight = 1
                        };
                        DwmExtendFrameIntoClientArea(this.Handle, ref margins);

                    }
                    break;
                default:
                    break;
            }
            base.WndProc(ref m);

            if (m.Msg == WM_NCHITTEST && (int)m.Result == HTCLIENT)
                m.Result = (IntPtr)HTCAPTION;         
        }

        #endregion FormMovementService

        [STAThread]
        [Obsolete]
        private static void Main(string[] args)
        {
            Console.WriteLine("Starting (ganttproj1 " + Application.ProductVersion + ")...\n" + AppDomain.CurrentDomain.BaseDirectory.ToString());

            ShowWindow(GetConsoleWindow(), SwHide);
            var menu = new Central();
            menu.MaximizedBounds = Screen.PrimaryScreen.WorkingArea;
            menu.WindowState = FormWindowState.Maximized;
            menu.ShowDialog();
        }
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (Form.ModifierKeys == Keys.None && keyData == Keys.Escape)
            {
                return false;
            }
            return base.ProcessDialogKey(keyData);
        }

        public Central()
        {
            InitializeComponent();
            pnForms.DoubleBuffered(true);
            SetStyle(ControlStyles.ResizeRedraw, true);
        }

        private void Menu_Load(object send, EventArgs args)
        {
            if (Store.Default.UpdateSettings)
            {
                Store.Default.Upgrade();
                Store.Default.UpdateSettings = false;
                Store.Default.Save();
            }
            RefreshTitle = "Commesse in lavoro/ commesse da programmare";

            pnTitlebar.MouseMove += (s, mv) =>
            {
                if (mv.Button == MouseButtons.Left && WindowState != FormWindowState.Maximized)
                {
                    ReleaseCapture();
                    SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
                }
            };

            lblShiftInfo.MouseMove += (s, mv) =>
            {
                if (mv.Button == MouseButtons.Left && WindowState != FormWindowState.Maximized)
                {
                    ReleaseCapture();
                    SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
                }
            };

            lblVersion.Text = "Ganntproj1 " + Application.ProductVersion;
            foreach (var file in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory))
            {
                if (Path.GetExtension(file) != ".exe") continue;

                label18.Text = label18.Text + '\n' + File.GetLastWriteTime(file).ToString("dd.MM.yyyy");
            }

            Cursor = Cursors.AppStarting;
            LoadingInfo.ShowLoading();      
            LoadingInfo.InfoText = "Downloading data from \n     " + ConnStr + "\n     Please wait...";

            if (_hasNewUpdate)
            {
                pbSettings.Refresh();
            }

            SuspendLayout();
            if (Store.Default.confHour == 0)
            {
                Store.Default.confHour = 7.5;
                Store.Default.Save();
            }
            Config.SetGanttConn(new DataContext(SpecialConnStr));
            Config.SetOlyConn(new DataContext(ConnStr));
            LoadShifts();
            LoadingInfo.UpdateText("Obtaining shifts...");
            LoadHolidays();

            LoadingInfo.UpdateText("Obtaining holidays...");

            var settDom = new SettingsDom();
            settDom.SaveHoursToSettings();
            LoadingInfo.UpdateText("Generating settings...");
            GetProductionColor();
            AddModels(false);

            var lst = (from models in TaskList
                       select models);

            TaskList = lst.ToList();

            treeMenu.Width = 0;
            pbReload.MouseEnter += delegate
            {
                pbReload.BackColor = Color.Gainsboro;
                btnLoadPan.BackColor = Color.Gainsboro;
            };
            pbReload.MouseLeave += delegate
            {
                pbReload.BackColor = Color.FromArgb(235, 235, 235);
                btnLoadPan.BackColor = Color.FromArgb(235, 235, 235);
            };
            SettingsCompleted = SettingsSys.Completed;

            btnMensile.Click += (s, g) =>
            {
                _fromNavigation = false;
                treeMenu.SelectedNode = treeMenu.Nodes[0].Nodes[6];
                treeMenu.Select();
            };
            btnEffizLinea.Click += (s, g) =>
            {
                _fromNavigation = false;
                treeMenu.SelectedNode = treeMenu.Nodes[0].Nodes[7];
                treeMenu.Select();
            };
            btnGraphEffLinea.Click += (s, g) =>
            {
                _fromNavigation = false;
                treeMenu.SelectedNode = treeMenu.Nodes[0].Nodes[8];
                treeMenu.Select();
            };
            btnFatturatoLinea.Click += (s, g) =>
            {
                _fromNavigation = false;
                treeMenu.SelectedNode = treeMenu.Nodes[0].Nodes[9];
                treeMenu.Select();
            };
            lblDepartment.Text = Store.Default.selDept + "/" + Store.Default.selDept;

            AddDepartmentsToCombo();

            IdStateArray = new System.Text.StringBuilder();
            IsDvc = false;
            IsChiuso = false;    
            IsResetJobLoader = true;
            IsAcconto = true;   
            IsSaldo = true; 
            cbAcconto.Checked = true;
            cbSaldo.Checked = true;
            pbReload.Image = Properties.Resources.refresh_total_32;
            IsArticleSelection = false;
            IsQtySelection = false;
            IsActiveOrdersSelection = false;

            var currentDate = DateTime.Now;
            dtpFrom.Value = new DateTime(currentDate.Year, currentDate.Month, 1);
            dtpTo.Value = dtpFrom.Value.AddMonths(1).AddDays(-1);
            DateFrom = new DateTime(dtpFrom.Value.Year, dtpFrom.Value.Month, dtpFrom.Value.Day);
            DateTo = new DateTime(dtpTo.Value.Year, dtpTo.Value.Month, dtpTo.Value.Day);
            JobModel.DateFromLast = DateFrom;
            JobModel.DateToLast = DateTo;
            pnDockBar.DoubleBuffered(true);
            pnDockBar.BackColor = Color.WhiteSmoke;
            pnDockBar.Width = 287;
            Workflow.ListOfRemovedOrders = new List<string>();

            CreateMenuTree();
            _fromNavigation = false;
            ResumeLayout(true);
            LoadingInfo.CloseLoading();
         
            treeMenu.SelectedNode = treeMenu.Nodes[0].Nodes[0];
            treeMenu.Select();

            _console = GetConsoleWindow();  
            Cursor = Cursors.Default;
           
            if (string.IsNullOrEmpty(Store.Default.selShift))   
            {
                MessageBox.Show("Shift must be configured.", "Settings", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SettingsCompleted = SettingsSys.Shift;
                var settings = new Settings();
                settings.ShowDialog();
                settings.Dispose();
            }
            if (Store.Default.sectorId <= 0 || Store.Default.arrDept == string.Empty)
            {
                MessageBox.Show("Department is not defined. Please define departments you want to use.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                SettingsCompleted = SettingsSys.Department;
                var settings = new Settings();
                settings.ShowDialog();
                settings.Dispose();
            }
            


            if (Store.Default.sectorId == 1)
            {
                Store.Default.manualDate = false;
            }
        }

        private void LoadShifts()
        {
            var q = "select starttime,endtime from shifts where shift='" + Store.Default.selShift + "'";
            using (var c = new SqlConnection(SpecialConnStr))
            {
                var cmd = new SqlCommand(q, c);
                c.Open();
                var dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        ShiftFrom = TimeSpan.Parse(dr[0].ToString());
                        ShiftTo = TimeSpan.Parse(dr[1].ToString());
                    }
                }
                c.Close();
            }
            lblShiftInfo.Text = "Turno " + 
                ShiftFrom.ToString(@"hh\:mm") + " - " + 
                ShiftTo.ToString(@"hh\:mm");
        }
        public static List<LineHolidaysEmbeded> ListOfHolidays { get; set; }
        private void LoadHolidays()
        {
            var lst = new List<LineHolidaysEmbeded>();
            ListOfHolidays = new List<LineHolidaysEmbeded>();
            var q = "select line,hdate,month,year,department from holidays " +
                "where idsector='" + Store.Default.sectorId + "' " +
                "order by year, month,len(line),line";
            using (var con = new SqlConnection(SpecialConnStr))
            {
                var cmd = new SqlCommand(q, con);
                con.Open();
                var dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        lst.Add(new LineHolidaysEmbeded(dr[0].ToString(),
                            dr[1].ToString(),
                            Convert.ToInt32(dr[2]),
                            Convert.ToInt32(dr[3]),
                            dr[4].ToString()));
                    }
                }
                con.Close();
                dr.Close();
            }
            foreach (var item in lst)
            {
                var l = (from items in item.HolidayArray.Split(',')
                         select items).ToList();
                foreach (var items in l)
                {
                    var dt = DateTime.ParseExact(items, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                    ListOfHolidays.Add(new LineHolidaysEmbeded(
                        item.Line, dt, item.Month, item.Year, item.Department));
                }
            }
        }

        public void GetBase()
        {
            try
            {
                Cursor = Cursors.AppStarting;
                Config.SetGanttConn(new DataContext(SpecialConnStr));
                Config.SetOlyConn(new DataContext(ConnStr));
                LoadShifts();
                LoadHolidays();
                AddModels(false);
                var lst = (from models in TaskList
                           select models).OrderBy(x => x.Department)
                          .ThenBy(x => Convert.ToDouble(x.Aim.Remove(0, 5)))
                          .ThenBy(x => x.StartDate);
                TaskList = lst.ToList();

                var ln = from lines in Tables.Lines
                         select lines;

                ListOfLines = ln.ToList();

                Cursor = Cursors.Default;
            }
            catch
            {
                Cursor = Cursors.Default;
                LoadingInfo.CloseLoading();
                MessageBox.Show("Internal server error",
                    Application.ProductName,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                Close();  
            }                       
        }

        private void AddModels(bool updateProduction)
        {
            try { 
            var tbl = new System.Data.DataTable();

            var q = "select * from [dbo].[viewobjects] where charindex(+',' + department + ',', '" + Store.Default.selDept + "') > 0 " +
                "order by department";

            TaskList = new List<JobModel>();
                using (var con = new System.Data.SqlClient.SqlConnection(SpecialConnStr))
                {
                    var cmd = new System.Data.SqlClient.SqlCommand(q, con);
                    con.Open();
                    var dtr = cmd.ExecuteReader();
                    tbl.Load(dtr);
                    con.Close();
                    dtr.Close();


                    TaskList = (from DataRow dr in tbl.Rows
                                select new JobModel()
                                {
                                    Name = dr["ordername"].ToString(),
                                    Aim = dr["aim"].ToString(),
                                    Article = dr["article"].ToString(),
                                    StateId = int.Parse(dr["stateid"].ToString()),
                                    LoadedQty = int.Parse(dr["loadedqty"].ToString()),
                                    QtyH = double.Parse(dr["qtyh"].ToString()),
                                    StartDate = Config.MinimalDate.AddTicks(long.Parse(dr["startdate"].ToString())),
                                    Duration = double.Parse(dr["duration"].ToString()),
                                    EndDate = Config.MinimalDate.AddTicks(long.Parse(dr["enddate"].ToString())),
                                    Dvc = Config.MinimalDate.AddTicks(long.Parse(dr["dvc"].ToString())),
                                    Rdd = Config.MinimalDate.AddTicks(long.Parse(dr["rdd"].ToString())),
                                    ProductionStartDate = Config.MinimalDate.AddTicks(long.Parse(dr["startprod"].ToString())),
                                    ProductionEndDate = Config.MinimalDate.AddTicks(long.Parse(dr["endprod"].ToString())),
                                    DailyProd = int.Parse(dr["dailyprod"].ToString()),
                                    ProdQty = int.Parse(dr["prodqty"].ToString()),
                                    OverQty = int.Parse(dr["overqty"].ToString()),
                                    ProdOverDays = int.Parse(dr["prodoverdays"].ToString()),
                                    DelayTime = long.Parse(dr["delayts"].ToString()),
                                    ProdOverTime = long.Parse(dr["prodoverts"].ToString()),
                                    IsLockedProduction = bool.Parse(dr["locked"].ToString()),
                                    HolidayRange = int.Parse(dr["holiday"].ToString()),
                                    ClosedByUser = bool.Parse(dr["closedord"].ToString()),
                                    ArtPrice = double.Parse(dr["artprice"].ToString()),
                                    HasProduction = bool.Parse(dr["hasprod"].ToString()),
                                    //= dr["lockedprod"]
                                    DelayStartDate = Config.MinimalDate.AddTicks(long.Parse(dr["delaystart"].ToString())),
                                    DelayEndDate = Config.MinimalDate.AddTicks(long.Parse(dr["delayend"].ToString())),
                                    ProductionDone = bool.Parse(dr["doneprod"].ToString()),
                                    // = dr["lockdate"]
                                    IsBase = bool.Parse(dr["base"].ToString()),
                                    Department = dr["department"].ToString(),
                                    // WorkingDays =int.Parse( dr["workingdays"].ToString()),
                                    Members = int.Parse(dr["membersnr"].ToString()),
                                    ManualDate = bool.Parse(dr["manualDate"].ToString()),
                                    Abatimen = int.Parse(dr["abatimen"].ToString()),
                                    //Launched = bool.Parse(dr["launched"].ToString()),
                                    //Operation = dr["operation"]
                                    //Idx= dr["idx"]
                                    ParentIdx = 0
                                }
                                ).ToList();

                    //if (updateProduction)
                    //{
                    //    if (Store.Default.sectorId == 8)
                    //    {
                    //        var cmd1 = new SqlCommand("UpdatePricesSartoria", con);
                    //        cmd1.CommandType = CommandType.StoredProcedure;
                    //        con.Open();
                    //        cmd1.ExecuteNonQuery();
                    //        con.Close();
                    //    }
                    //    else if (Store.Default.sectorId != 2 && Store.Default.sectorId != 8)
                    //    {
                    //        var cmd2 = new SqlCommand("UpdatePrices", con);
                    //        cmd2.CommandType = CommandType.StoredProcedure;
                    //        cmd2.Parameters.Add("@IdSector", SqlDbType.Int).Value = Store.Default.sectorId;
                    //        con.Open();
                    //        cmd2.ExecuteNonQuery();
                    //        con.Close();
                    //    }
                    //}
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
            
        }

        private void CreateMenuTree()
        {
            var root = new TreeNode("");
            var node1 = root.Nodes.Add("Carico lavoro");
            var node2 = root.Nodes.Add("Produzione gantt");
            var node3 = root.Nodes.Add("Produzione");
            var node4 = root.Nodes.Add("Fatturato");
            var node5 = root.Nodes.Add("Diffetato");
            var node6 = root.Nodes.Add("Respinte");
            var node7 = root.Nodes.Add("Produzione Mensile");
            var node8 = root.Nodes.Add("Effizienza/linea");
            var node9 = root.Nodes.Add("Grafico di efficienza/linea");
            var node10 = root.Nodes.Add("Fatturato per Linea");
            var node11 = root.Nodes.Add("Chiusura commesse piu difettato");
            var node12 = root.Nodes.Add("Grafico commesse respinte");

            treeMenu.BeginUpdate();
            treeMenu.Nodes.Add(root);
            treeMenu.EndUpdate();

            treeMenu.AfterSelect += (sender, eventArgs) =>
            {
                LoadingInfo.BorderColor = Brushes.SeaGreen;
                LoadingInfo.ShowLoading();
                LoadingInfo.InfoText = "Loading data...";
                pnNavi.Enabled = false;
                pbShowHide.Visible = false;
                foreach (var ctrl in pnForms.Controls)
                {
                    if (ctrl is Form f)
                    {
                        f.Close();
                        f.Dispose();
                    }
                }

                ClearReflectedHandlers();

                ResetMenuCommands();

                if (treeMenu.SelectedNode == node1)
                {
                    if (IsResetJobLoader)
                    {
                        IsAcconto = true;
                        IsSaldo = true;
                        cbAcconto.Checked = true;
                        cbSaldo.Checked = true;
                    }
                    pbShowHide.Visible = true;
                    var frm = new LoadingJob(false);
                    var wFlow = new Workflow();
                    LoadingInfo.UpdateText("Loading carico lavoro...");
                    wFlow.LoadDataWithDateChange();
                    frm.WindowState = FormWindowState.Minimized;
                    frm.FormBorderStyle = FormBorderStyle.None;
                    frm.ShowInTaskbar = false;
                    frm.ControlBox = false;
                    frm.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
                    frm.Visible = false;
                    frm.Show();
                    frm.TopLevel = false;
                    pnForms.Controls.Add(frm);
                    frm.Location = new Point(0, 0);
                    frm.Size = pnForms.Size;
                    frm.Visible = true;
                   
                    btnExcel.Click += (se, e) =>
                    {
                        frm.LoadExcelData();
                    };
                    pbReload.Click += (se, e) =>
                    {
                        if (IsResetJobLoader)
                        {
                            IsResetJobLoader = true;
                            IsAcconto = true;
                            IsSaldo = true;
                            cbAcconto.Checked = true;
                            cbSaldo.Checked = true;
                        }
                        else
                        {
                            IsResetJobLoader = false;
                            ResetStateFilters();
                        }
                        wFlow.LoadDataWithDateChange();
                        frm.LoadCaricoLavoro();
                    };
                    lblRefreshGlobal.Click += (se, e) =>
                    {
                        if (IsResetJobLoader)
                        {
                            ResetStateFilters();
                            IsResetJobLoader = false;
                        }
                        wFlow.LoadDataWithDateChange();
                        frm.LoadCaricoLavoro();
                        pnReload.Visible = false;
                        pbReload.Image = Properties.Resources.reset_total_32;
                        RefreshTitle = "Commesse sul selezione data";
                        pnTitlebar.Refresh();
                    };
                    lblResetGlobal.Click += (se, e) =>
                    {
                        IsResetJobLoader = true;
                        IsAcconto = true;
                        IsSaldo = true;
                        cbAcconto.Checked = true;
                        cbSaldo.Checked = true;
                        GetBase();
                        wFlow.LoadDataWithDateChange();
                        frm.LoadCaricoLavoro();
                        pnReload.Visible = false;
                        pbReload.Image = Properties.Resources.refresh_total_32;
                        RefreshTitle = "Commesse in lavoro/ commesse da programmare";
                        pnTitlebar.Refresh();
                    };
                    pbPrint.Click += (se, e) =>
                    {
                        frm.PrintGrid();
                    };
                    cbDvc.Click += (se, e) =>
                    {
                        IsProgramare = false;
                        cbProgrammare.Checked = false;
                        var c = (CheckBox)se;
                        ResetStateFilters();
                        IsDvc = c.Checked;
                        if (cbRdd.Checked)
                        {
                            cbRdd.Checked = false;
                            IsRdd = false;
                        }
                        frm.LoadCaricoLavoro();
                    };
                    cbRdd.Click += (se, e) =>
                    {
                        IsProgramare = false;
                        cbProgrammare.Checked = false;
                        var c = (CheckBox)se;
                        ResetStateFilters();
                        IsRdd = c.Checked;
                        if (cbDvc.Checked)
                        {
                            cbDvc.Checked = false;
                            IsDvc = false;
                        }
                        frm.LoadCaricoLavoro();
                    };
                    cbAcconto.Click += (se, e) =>
                    {
                        IsProgramare = false;
                        cbProgrammare.Checked = false;
                        var c = (CheckBox)se;
                        IsAcconto = false;
                        if (c.Checked == true) IsAcconto = true;
                        frm.LoadCaricoLavoro();
                    };
                    cbSaldo.Click += (se, e) =>
                    {
                        IsProgramare = false;
                        cbProgrammare.Checked = false;
                        var c = (CheckBox)se;
                        IsSaldo = false;
                        if (c.Checked == true) IsSaldo = true;
                        frm.LoadCaricoLavoro();
                    };
                    cbChiuso.Click += (se, e) =>
                    {
                        IsProgramare = false;
                        cbProgrammare.Checked = false;
                        var c = (CheckBox)se;
                        IsChiuso = false;
                        if (c.Checked == true) IsChiuso = true;
                        frm.LoadCaricoLavoro();
                    };
                    cbProgrammare.Click += (se, e) =>
                    {
                        var c = (CheckBox)se;
                        ResetStateFilters();
                        IsProgramare = c.Checked;
                        frm.LoadCaricoLavoro();
                    };
                    pbShowHide.Click += (se, args) =>
                      {
                          frm.HideLightColumns();
                      };

                    btnCarico.ForeColor = Color.White;
                    btnCarico.Image = Properties.Resources.switch_arrow_triangle_right_white;
                    btnCarico.BackColor = Color.FromArgb(125,141,161);
                    LoadingInfo.CloseLoading();
                }

                if (treeMenu.SelectedNode == node2)
                {
                    if (IsResetJobLoader)
                    {
                        ResetStateFilters();
                    }

                    var frm = new Workflow();
                    frm.WindowState = FormWindowState.Minimized;
                    frm.FormBorderStyle = FormBorderStyle.None;
                    frm.ShowInTaskbar = false;
                    frm.ControlBox = false;
                    frm.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
                    frm.Visible = false;
                    frm.Show();
                    frm.TopLevel = false;
                    pnForms.Controls.Add(frm);
                    frm.Location = new Point(0, 0);
                    frm.Size = pnForms.Size;
                    frm.Visible = true;

                    pbReload.Click += (se, e) =>
                    {
                        if (cbDept.SelectedIndex == 0)
                        {
                            MessageBox.Show("not allowed");
                            return;
                        }

                        frm.LoadDataWithDateChange();
                    };
                    lblRefreshGlobal.Click += (se, e) =>
                    {
                        if (cbDept.SelectedIndex == 0)
                        {
                            MessageBox.Show("not allowed");
                            return;
                        }

                        IsResetJobLoader = false;
                        frm.LoadDataWithDateChange();
                        pnReload.Visible = false;
                        pbReload.Image = Properties.Resources.reset_total_32;
                        RefreshTitle = "Commesse sul selezione data";
                        pnTitlebar.Refresh();
                    };
                    lblResetGlobal.Click += (se, e) =>
                    {
                        if (cbDept.SelectedIndex == 0)
                        {
                            MessageBox.Show("not allowed");
                            return;
                        }

                        IsResetJobLoader = true;
                        frm.LoadDataWithDateChange();
                        pnReload.Visible = false;
                        pbReload.Image = Properties.Resources.refresh_total_32;
                        RefreshTitle = "Commesse in lavoro/ commesse da programmare";
                        pnTitlebar.Refresh();
                    };
                    cbAcconto.Click += (se, e) =>
                    {
                        var c = (CheckBox)se;
                        IsAcconto = false;
                        if (c.Checked == true) IsAcconto = true;
                        frm.AddTimelineObjects();
                    };
                    cbSaldo.Click += (se, e) =>
                    {
                        var c = (CheckBox)se;
                        IsSaldo = false;
                        if (c.Checked == true) IsSaldo = true;
                        frm.AddTimelineObjects();
                    };
                    cbChiuso.Click += (se, e) =>
                    {
                        var c = (CheckBox)se;
                        IsChiuso = false;
                        if (c.Checked == true) IsChiuso = true;
                        frm.AddTimelineObjects();
                    };
                    btnProduzioneGantt.BackColor = Color.FromArgb(125, 141, 161);
                    btnProduzioneGantt.ForeColor = Color.White;
                    btnProduzioneGantt.Image = Properties.Resources.switch_arrow_triangle_right_white;
                }

                if (treeMenu.SelectedNode == node3)
                {
                    if (IsResetJobLoader)
                    {
                        ResetStateFilters();
                    }
                    var frm = new Produzione();
                    frm.WindowState = FormWindowState.Minimized;
                    frm.FormBorderStyle = FormBorderStyle.None;
                    frm.ShowInTaskbar = false;
                    frm.ControlBox = false;
                    frm.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
                    frm.Visible = false;
                    frm.Show();
                    frm.TopLevel = false;
                    pnForms.Controls.Add(frm);
                    frm.Location = new Point(0, 0);
                    frm.Size = pnForms.Size;
                    frm.Visible = true;
                    btnProduzione.BackColor = Color.FromArgb(125, 141, 161);
                    btnProduzione.ForeColor = Color.White;
                    btnProduzione.Image = Properties.Resources.switch_arrow_triangle_right_white;
                    pbReload.Click += (se, e) =>
                    {
                        frm.LoadReportTable();
                    };
                    btnExcel.Click += (se, e) =>
                    {
                        frm.ExportToExcel();
                    };
                }
                if (treeMenu.SelectedNode == node4)
                {
                    if (IsResetJobLoader) ResetStateFilters();

                    LoadingInfo.CloseLoading();

                    var frmPin = new PinInput();
                    frmPin.StartPosition = FormStartPosition.CenterScreen;
                    frmPin.ShowDialog();
                    if (!frmPin.PinCorrect)
                    {
                        _fromNavigation = false;
                        treeMenu.SelectedNode = treeMenu.Nodes[0].Nodes[0];
                        treeMenu.Select();
                        return;
                    }
                    frmPin.Dispose();
                    var frm = new Fatturato();
                    frm.WindowState = FormWindowState.Minimized;
                    frm.FormBorderStyle = FormBorderStyle.None;
                    frm.ShowInTaskbar = false;
                    frm.ControlBox = false;
                    frm.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
                    frm.Visible = false;
                    frm.Show();
                    frm.TopLevel = false;
                    pnForms.Controls.Add(frm);
                    frm.Location = new Point(0, 0);
                    frm.Size = pnForms.Size;
                    frm.Visible = true;
                    btnFatturato.BackColor = Color.FromArgb(125, 141, 161);
                    btnFatturato.ForeColor = Color.White;
                    btnFatturato.Image = Properties.Resources.switch_arrow_triangle_right_white;
                    pbReload.Click += (se, e) =>
                    {
                        frm.LoadData();
                    };
                    btnExcel.Click += (se, e) =>
                    {
                        frm.ExportToExcel();
                    };
                }
                if (treeMenu.SelectedNode == node5)
                {
                    if (IsResetJobLoader) ResetStateFilters();

                    var frm = new Diffetato();
                    frm.WindowState = FormWindowState.Minimized;
                    frm.FormBorderStyle = FormBorderStyle.None;
                    frm.ShowInTaskbar = false;
                    frm.ControlBox = false;
                    frm.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
                    frm.Visible = false;
                    frm.Show();
                    if (Store.Default.sectorId != 2) frm.LoadReportByDate(true); else frm.LoadReportByDateStiro(true);
                    frm.TopLevel = false;
                    pnForms.Controls.Add(frm);
                    frm.Location = new Point(0, 0);
                    frm.Size = pnForms.Size;
                    frm.Visible = true;
                    btnDiffetato.BackColor = Color.FromArgb(125, 141, 161);
                    btnDiffetato.ForeColor = Color.White;
                    btnDiffetato.Image = Properties.Resources.switch_arrow_triangle_right_white;
                    btnExcel.Click += (se, e) =>
                    {
                        frm.ExportToExcel();
                    };
                    pbPrint.Click += (se, e) =>
                    {
                        frm.PrintGrid();
                    };
                    pbReload.Click += (se, e) =>
                    {
                        if (IsResetJobLoader)
                        {
                            IsResetJobLoader = true;
                            IsAcconto = true;
                            IsSaldo = true;
                            cbAcconto.Checked = true;
                            cbSaldo.Checked = true;
                        }
                        else
                        {
                            IsResetJobLoader = false;
                            ResetStateFilters();
                        }
                        if (Store.Default.sectorId != 2) frm.LoadReportByDate(!IsResetJobLoader); else frm.LoadReportByDateStiro(!IsResetJobLoader);

                    };
                    lblRefreshGlobal.Click += (se, e) =>
                    {
                        if (IsResetJobLoader)
                        {
                            ResetStateFilters();
                            IsResetJobLoader = false;
                        }
                        if (Store.Default.sectorId != 2) frm.LoadReportByDate(!IsResetJobLoader); else frm.LoadReportByDateStiro(!IsResetJobLoader);

                        pnReload.Visible = false;
                        pbReload.Image = Properties.Resources.reset_total_32;
                        RefreshTitle = "Commesse sul selezione data";
                    };
                    lblResetGlobal.Click += (se, e) =>
                    {
                        IsResetJobLoader = true;
                        IsAcconto = true;
                        IsSaldo = true;
                        cbAcconto.Checked = true;
                        cbSaldo.Checked = true;
                        if (Store.Default.sectorId != 2) frm.LoadReportByDate(!IsResetJobLoader); else frm.LoadReportByDateStiro(!IsResetJobLoader);

                        pnReload.Visible = false;
                        pbReload.Image = Properties.Resources.refresh_total_32;
                        RefreshTitle = "Commesse in lavoro/ commesse da programmare";
                    };
                }
                if (treeMenu.SelectedNode == node6)
                {
                    if (IsResetJobLoader) ResetStateFilters();

                    var frm = new Respinte();
                    frm.WindowState = FormWindowState.Minimized;
                    frm.FormBorderStyle = FormBorderStyle.None;
                    frm.ShowInTaskbar = false;
                    frm.ControlBox = false;
                    frm.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
                    frm.Visible = false;
                    frm.Show();
                    frm.LoadReportByDate(true);
                    frm.TopLevel = false;
                    pnForms.Controls.Add(frm);
                    frm.Location = new Point(0, 0);
                    frm.Size = pnForms.Size;
                    frm.Visible = true;
                    btnRespinte.BackColor = Color.FromArgb(125, 141, 161);
                    btnRespinte.ForeColor = Color.White;
                    btnRespinte.Image = Properties.Resources.switch_arrow_triangle_right_white;
                    btnExcel.Click += (se, e) =>
                    {
                        frm.ExportToExcel();
                    };
                    pbPrint.Click += (se, e) =>
                    {
                        frm.PrintGrid();
                    };
                    pbReload.Click += (se, e) =>
                    {
                        if (IsResetJobLoader)
                        {
                            IsResetJobLoader = true;
                            IsAcconto = true;
                            IsSaldo = true;
                            cbAcconto.Checked = true;
                            cbSaldo.Checked = true;
                        }
                        else
                        {
                            IsResetJobLoader = false;
                            ResetStateFilters();
                        }
                        frm.LoadReportByDate(!IsResetJobLoader);
                    };
                    lblRefreshGlobal.Click += (se, e) =>
                    {
                        if (IsResetJobLoader)
                        {
                            ResetStateFilters();
                            IsResetJobLoader = false;
                        }
                        frm.LoadReportByDate(!IsResetJobLoader);
                        pnReload.Visible = false;
                        pbReload.Image = Properties.Resources.reset_total_32;
                        RefreshTitle = "Commesse sul selezione data";
                    };
                    lblResetGlobal.Click += (se, e) =>
                    {
                        IsResetJobLoader = true;
                        IsAcconto = true;
                        IsSaldo = true;
                        cbAcconto.Checked = true;
                        cbSaldo.Checked = true;
                        frm.LoadReportByDate(!IsResetJobLoader);
                        pnReload.Visible = false;
                        pbReload.Image = Properties.Resources.refresh_total_32;
                        RefreshTitle = "Commesse in lavoro/ commesse da programmare";
                    };
                }
                if (treeMenu.SelectedNode == node7)
                {
                    if (IsResetJobLoader) ResetStateFilters();
                    var frm = new Mensile("mens");   
                    frm.WindowState = FormWindowState.Minimized;
                    frm.FormBorderStyle = FormBorderStyle.None;
                    frm.ShowInTaskbar = false;
                    frm.ControlBox = false;
                    frm.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
                    frm.Visible = false;
                    frm.Show();
                    frm.LoadDataMensile();
                    frm.TopLevel = false;
                    pnForms.Controls.Add(frm);
                    frm.Location = new Point(0, 0);
                    frm.Size = pnForms.Size;
                    frm.Visible = true;
                    btnMensile.BackColor = Color.FromArgb(125, 141, 161);
                    btnMensile.ForeColor = Color.White;
                    btnMensile.Image = Properties.Resources.switch_arrow_triangle_right_white;
                    btnExcel.Click += (se, e) =>
                    {
                        frm.ExportToExcel();
                    };
                    pbPrint.Click += (se, e) =>
                    {
                        frm.PrintGrid();
                    };

                    _settings.FormClosing += (s, g) =>
                    {
                        frm.LoadDataMensile();
                    };
                }
                if (treeMenu.SelectedNode == node8)
                {
                    if (IsResetJobLoader) ResetStateFilters();
                    var frm = new Mensile("eff");                 
                    frm.WindowState = FormWindowState.Minimized;
                    frm.FormBorderStyle = FormBorderStyle.None;
                    frm.ShowInTaskbar = false;
                    frm.ControlBox = false;
                    frm.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
                    frm.Visible = false;
                    frm.Show();
                    frm.LoadEff();
                    frm.TopLevel = false;
                    pnForms.Controls.Add(frm);
                    frm.Location = new Point(0, 0);
                    frm.Size = pnForms.Size;
                    frm.Visible = true;
                    btnEffizLinea.BackColor = Color.FromArgb(125, 141, 161);
                    btnEffizLinea.ForeColor = Color.White;
                    btnEffizLinea.Image = Properties.Resources.switch_arrow_triangle_right_white;
                    btnExcel.Click += (se, e) =>
                    {
                        frm.ExportToExcel();
                    };
                    pbPrint.Click += (se, e) =>
                    {
                        frm.PrintGrid();
                    };
                }
                if (treeMenu.SelectedNode == node9)
                {
                    if (IsResetJobLoader) ResetStateFilters();

                    var frm = new LineGraph();
                    frm.WindowState = FormWindowState.Minimized;
                    frm.FormBorderStyle = FormBorderStyle.None;
                    frm.ShowInTaskbar = false;
                    frm.ControlBox = false;
                    frm.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
                    frm.Visible = false;
                    frm.Show();
                    frm.LoadGraph();
                    frm.TopLevel = false;
                    pnForms.Controls.Add(frm);
                    frm.Location = new Point(0, 0);
                    frm.Size = pnForms.Size;
                    frm.Visible = true;
                    btnGraphEffLinea.BackColor = Color.FromArgb(125, 141, 161);
                    btnGraphEffLinea.ForeColor = Color.White;
                    btnGraphEffLinea.Image = Properties.Resources.switch_arrow_triangle_right_white;
                }
                if (treeMenu.SelectedNode == node10)
                {
                    if (IsResetJobLoader) ResetStateFilters();
                    LoadingInfo.CloseLoading();

                    var frmPin = new PinInput();
                    frmPin.StartPosition = FormStartPosition.CenterScreen;
                    frmPin.ShowDialog();
                    if (!frmPin.PinCorrect)
                    {
                        _fromNavigation = false;
                        treeMenu.SelectedNode = treeMenu.Nodes[0].Nodes[0];
                        treeMenu.Select();
                        return;
                    }
                    frmPin.Dispose();
                    var frm = new FatturatoLinea();
                    frm.WindowState = FormWindowState.Minimized;
                    frm.FormBorderStyle = FormBorderStyle.None;
                    frm.ShowInTaskbar = false;
                    frm.ControlBox = false;
                    frm.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
                    frm.Visible = false;
                    frm.Show();
                    frm.LoadData();
                    frm.TopLevel = false;
                    pnForms.Controls.Add(frm);
                    frm.Location = new Point(0, 0);
                    frm.Size = pnForms.Size;
                    frm.Visible = true;
                    btnFatturatoLinea.BackColor = Color.FromArgb(125, 141, 161);
                    btnFatturatoLinea.ForeColor = Color.White;
                    btnFatturatoLinea.Image = Properties.Resources.switch_arrow_triangle_right_white;

                    btnExcel.Click += (se, e) =>
                    {
                        frm.ExportToExcel();
                    };
                }
                if (treeMenu.SelectedNode == node11)
                {
                    if (IsResetJobLoader) ResetStateFilters();

                    var frm = new CommessaDefect();
                    frm.WindowState = FormWindowState.Minimized;
                    frm.FormBorderStyle = FormBorderStyle.None;
                    frm.ShowInTaskbar = false;
                    frm.ControlBox = false;
                    frm.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
                    frm.Visible = false;
                    frm.Show();
                    frm.TopLevel = false;
                    pnForms.Controls.Add(frm);
                    frm.Location = new Point(0, 0);
                    frm.Size = pnForms.Size;
                    frm.Visible = true;
                    btnCommessaDefects.BackColor = Color.FromArgb(125, 141, 161);
                    btnCommessaDefects.ForeColor = Color.White;
                    btnCommessaDefects.Image = Properties.Resources.switch_arrow_triangle_right_white;

                }
                if (treeMenu.SelectedNode == node12)
                {
                    if (IsResetJobLoader) ResetStateFilters();

                    var frm = new GraficoRespinte
                    {
                        WindowState = FormWindowState.Minimized,
                        FormBorderStyle = FormBorderStyle.None,
                        ShowInTaskbar = false,
                        ControlBox = false,
                        Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom,
                        Visible = false
                    };
                    frm.Show();
                    frm.TopLevel = false;
                    pnForms.Controls.Add(frm);
                    frm.Location = new Point(0, 0);
                    frm.Size = pnForms.Size;
                    frm.Visible = true;
                    btnGraphRespinte.BackColor = Color.FromArgb(125, 141, 161);
                    btnGraphRespinte.ForeColor = Color.White;
                    btnGraphRespinte.Image = Properties.Resources.switch_arrow_triangle_right_white;
                    pbReload.Click += (se, e) =>
                    {
                        if (IsResetJobLoader)
                        {
                            IsResetJobLoader = true;
                            IsAcconto = true;
                            IsSaldo = true;
                            cbAcconto.Checked = true;
                            cbSaldo.Checked = true;
                        }
                        else
                        {
                            IsResetJobLoader = false;
                            ResetStateFilters();
                        }
                        frm.LoadaDataFromServer();
                        frm.CreateSituationContolReport();
                        frm.CreateGraphReport();
                    };
                    lblRefreshGlobal.Click += (se, e) =>
                    {
                        if (IsResetJobLoader)
                        {
                            ResetStateFilters();
                            IsResetJobLoader = false;
                        }
                        frm.LoadaDataFromServer();
                        frm.CreateSituationContolReport();
                        frm.CreateGraphReport();
                        pnReload.Visible = false;
                        pbReload.Image = Properties.Resources.reset_total_32;
                        RefreshTitle = "Commesse sul selezione data";
                    };
                }

                pnNavi.Enabled = true;
                treeMenu.Enabled = true;

                if (eventArgs.Action != TreeViewAction.Unknown) return;
                if (!_fromNavigation)
                {
                    if (!listBox1.Items.Contains(treeMenu.SelectedNode))
                        listBox1.Items.Add(treeMenu.SelectedNode);

                    listBox1.SelectedIndex = listBox1.Items.Count - 1;
                }

                btnBack.Enabled = true;

                ResumeLayout(true);
                LoadingInfo.CloseLoading();
            };
            treeMenu.BeginUpdate();
            treeMenu.CollapseAll();
            treeMenu.BeforeExpand += CheckForCheckedChildrenHandler;
            treeMenu.ExpandAll();
            treeMenu.BeforeExpand -= CheckForCheckedChildrenHandler;
            treeMenu.EndUpdate();
        }

        private void ResetStateFilters()
        {
            cbAcconto.Checked = false;
            cbSaldo.Checked = false;
            cbChiuso.Checked = false;
            IsAcconto = false;
            IsSaldo = false;
            IsChiuso = false;
        }

        private static void CheckForCheckedChildrenHandler(object sender,
            TreeViewCancelEventArgs e)
        {
            if (!HasCheckedChildNodes(e.Node)) e.Cancel = true;
        }

        private static bool HasCheckedChildNodes(TreeNode node)
        {
            if (node.Nodes.Count == 0) return false;
            foreach (TreeNode childNode in node.Nodes)
            {
                if (childNode.Checked == false) return true;
                if (HasCheckedChildNodes(childNode)) return true;
            }
            return false;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (pnDockBar.Width > 0)
            {
                SuspendLayout();
                pnDockBar.Width = 0;
                ResumeLayout(true);
            }
            else
            {
                SuspendLayout();
                pnDockBar.Width = 252;
                ResumeLayout(true);
            }
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            DateFrom = new DateTime(dtpFrom.Value.Year, dtpFrom.Value.Month, dtpFrom.Value.Day);
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            DateTo = new DateTime(dtpTo.Value.Year, dtpTo.Value.Month, dtpTo.Value.Day);
            if (DateTo <= DateFrom)
            {
                dtpTo.Value = DateFrom.AddDays(+15);
                DateTo = new DateTime(dtpTo.Value.Year, dtpTo.Value.Month, dtpTo.Value.Day);
            }
        }

        private void ShowPopup(Form frm)
        {
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.ShowDialog(this);
        }
        private void btnSync_ButtonClick(object sender, EventArgs e)
        {
           
        }
        private void btnFilters_Click(object sender, EventArgs e)
        {
            var flag = pnChecks.Visible;
            pnChecks.Visible = !flag;
        }
        private const int WmParentnotify = 0x210;

        private const int WmLbuttondown = 0x201;

        private void ClearReflectedHandlers()
        {
            for (var i = pnChecks.Controls.Count - 1; i >= 0; i--)
            {
                if ((!(pnChecks.Controls[i] is CheckBox cbs))) continue;
                var f1 = typeof(Control).GetField("EventClick", BindingFlags.Static | BindingFlags.NonPublic);
                var obj = f1.GetValue(cbs);
                var pi = cbs.GetType().GetProperty("Events", BindingFlags.NonPublic | BindingFlags.Instance);
                var list = (EventHandlerList)pi.GetValue(cbs, null);
                list.RemoveHandler(obj, list[obj]);
            }
            for (var i = pnReload.Controls.Count - 1; i >= 0; i--)
            {
                if ((!(pnReload.Controls[i] is Label cbs))) continue;
                var f1 = typeof(Control).GetField("EventClick", BindingFlags.Static | BindingFlags.NonPublic);
                var obj = f1.GetValue(cbs);
                var pi = cbs.GetType().GetProperty("Events", BindingFlags.NonPublic | BindingFlags.Instance);
                var list = (EventHandlerList)pi.GetValue(cbs, null);
                list.RemoveHandler(obj, list[obj]);
            }
            for (var i = pnNavi.Controls.Count - 1; i >= 0; i--)

                switch (pnNavi.Controls[i])
                {
                    case PictureBox pb when pb.Name != "pbMenu":
                        {
                            if (pb.Name == "pbSettings") continue;
                            var f1 = typeof(Control).GetField("EventClick", BindingFlags.Static | BindingFlags.NonPublic);
                            var obj = f1.GetValue(pb);
                            var pi = pb.GetType().GetProperty("Events", BindingFlags.NonPublic | BindingFlags.Instance);
                            var list = (EventHandlerList)pi.GetValue(pb, null);
                            list.RemoveHandler(obj, list[obj]);
                            break;
                        }
                    case CheckBox cb:
                        {
                            var f1 = typeof(Control).GetField("EventClick", BindingFlags.Static | BindingFlags.NonPublic);
                            var obj = f1.GetValue(cb);
                            var pi = cb.GetType().GetProperty("Events", BindingFlags.NonPublic | BindingFlags.Instance);
                            var list = (EventHandlerList)pi.GetValue(cb, null);
                            list.RemoveHandler(obj, list[obj]);
                            break;
                        }
                    case Button btn when btn.Name != "pbMenu":
                        {
                            if (btn.Name != "btnExcel") continue;

                            var f1 = typeof(Control).GetField("EventClick", BindingFlags.Static | BindingFlags.NonPublic);
                            var obj = f1.GetValue(btn);
                            var pi = btn.GetType().GetProperty("Events", BindingFlags.NonPublic | BindingFlags.Instance);
                            var list = (EventHandlerList)pi.GetValue(btn, null);
                            list.RemoveHandler(obj, list[obj]);
                            break;
                        }
                }
        }

        private void pbMenu_MouseEnter(object sender, EventArgs e)
        {
            var btn = (PictureBox)sender;
            btn.BackColor = Color.Gainsboro;
        }

        private void pbMenu_MouseLeave(object sender, EventArgs e)
        {
            var btn = (PictureBox)sender;
            btn.BackColor = Color.FromArgb(235, 235, 235);
        }

        private void btnProgram_Click(object sender, EventArgs e)
        {
            treeMenu.SelectedNode = treeMenu.Nodes[0];
            treeMenu.Select();

            foreach (Control c in pnDockBar.Controls)
            {
                if (c is Button b && b.Tag.ToString() == 1.ToString() &&
                    b.Name != "btnProgram")
                {
                    c.Visible = !c.Visible;

                    if (!c.Visible)
                    {
                        btnProgram.Image = Properties.Resources.arrow_right;
                    }
                    else
                    {
                        btnProgram.Image = Properties.Resources.arrow_down;
                        if (c.Visible)
                        {
                            btnCarico.PerformClick();
                        }
                    }
                }
            }
        }

        private void btnCarico_Click(object sender, EventArgs e)
        {
            _fromNavigation = false;
            treeMenu.SelectedNode = treeMenu.Nodes[0].Nodes[0];
            treeMenu.Select();
        }

        private void btnProduzioneGantt_Click(object sender, EventArgs e)
        {
            _fromNavigation = false;
            treeMenu.SelectedNode = treeMenu.Nodes[0].Nodes[1];
            treeMenu.Select();
        }

        private void BtnProduzione_Click(object sender, EventArgs e)
        {
            _fromNavigation = false;
            treeMenu.SelectedNode = treeMenu.Nodes[0].Nodes[2];
            treeMenu.Select();
        }

        private void BtnFatturato_Click(object sender, EventArgs e)
        {
            _fromNavigation = false;
            treeMenu.SelectedNode = treeMenu.Nodes[0].Nodes[3];
            treeMenu.Select();
        }

        private void BtnDiffetato_Click(object sender, EventArgs e)
        {
            _fromNavigation = false;
            treeMenu.SelectedNode = treeMenu.Nodes[0].Nodes[4];
            treeMenu.Select();
        }

        private void BtnRespinte_Click(object sender, EventArgs e)
        {
            _fromNavigation = false;
            treeMenu.SelectedNode = treeMenu.Nodes[0].Nodes[5];
            treeMenu.Select();
        }
        private void Button2_Click_1(object sender, EventArgs e)
        {
            _fromNavigation = false;
            treeMenu.SelectedNode = treeMenu.Nodes[0].Nodes[6];
            treeMenu.Select();
        }


        private void btnCommessaDefects_Click(object sender, EventArgs e)
        {
            _fromNavigation = false;
            treeMenu.SelectedNode = treeMenu.Nodes[0].Nodes[10];
            treeMenu.Select();
        }

        private void btnGraphRespinte_Click(object sender, EventArgs e)
        {
            _fromNavigation = false;
            treeMenu.SelectedNode = treeMenu.Nodes[0].Nodes[11];
            treeMenu.Select();
        }
        private void ResetMenuCommands()
        {
            btnCarico.Text = "Carico lavoro";
            btnCarico.BackColor = Color.FromArgb(210,210,210);
            btnCarico.ForeColor = Color.FromArgb(113,113,113);
            btnCarico.Image = Properties.Resources.arrow_triangle_right;
            btnProduzioneGantt.Text = "Produzione gantt";
            btnProduzioneGantt.BackColor = Color.FromArgb(210, 210, 210);
            btnProduzioneGantt.ForeColor = Color.FromArgb(113, 113, 113);
            btnProduzioneGantt.Image = Properties.Resources.arrow_triangle_right;
            btnProduzione.Text = "Produzione";
            btnProduzione.BackColor = Color.FromArgb(210, 210, 210);
            btnProduzione.ForeColor = Color.FromArgb(113, 113, 113);
            btnProduzione.Image = Properties.Resources.arrow_triangle_right;
            btnFatturato.Text = "Valore produzione";
            btnFatturato.BackColor = Color.FromArgb(210, 210, 210);
            btnFatturato.ForeColor = Color.FromArgb(113, 113, 113);
            btnFatturato.Image = Properties.Resources.arrow_triangle_right;
            btnDiffetato.Text = "Diffetato";
            btnDiffetato.BackColor = Color.FromArgb(210, 210, 210);
            btnDiffetato.ForeColor = Color.FromArgb(113, 113, 113);
            btnDiffetato.Image = Properties.Resources.arrow_triangle_right;
            btnRespinte.Text = "Respinte";
            btnRespinte.BackColor = Color.FromArgb(210, 210, 210);
            btnRespinte.ForeColor = Color.FromArgb(113, 113, 113);
            btnRespinte.Image = Properties.Resources.arrow_triangle_right;
            btnMensile.Text = "Produzione Mensile";
            btnMensile.BackColor = Color.FromArgb(210, 210, 210);
            btnMensile.ForeColor = Color.FromArgb(113, 113, 113);
            btnMensile.Image = Properties.Resources.arrow_triangle_right;
            btnEffizLinea.Text = "Effizienza/linea";
            btnEffizLinea.BackColor = Color.FromArgb(210, 210, 210);
            btnEffizLinea.ForeColor = Color.FromArgb(113, 113, 113);
            btnEffizLinea.Image = Properties.Resources.arrow_triangle_right;
            btnGraphEffLinea.Text = "Grafico di efficienza/linea";
            btnGraphEffLinea.BackColor = Color.FromArgb(210, 210, 210);
            btnGraphEffLinea.ForeColor = Color.FromArgb(113, 113, 113);
            btnGraphEffLinea.Image = Properties.Resources.arrow_triangle_right;
            btnFatturatoLinea.Text = "Valore produzione per line";
            btnFatturatoLinea.BackColor = Color.FromArgb(210, 210, 210);
            btnFatturatoLinea.ForeColor = Color.FromArgb(113, 113, 113);
            btnFatturatoLinea.Image = Properties.Resources.arrow_triangle_right;
            btnCommessaDefects.Text = "Chiusura commesse piu difettato";
            btnCommessaDefects.BackColor = Color.FromArgb(210, 210, 210);
            btnCommessaDefects.ForeColor = Color.FromArgb(113, 113, 113);
            btnCommessaDefects.Image = Properties.Resources.arrow_triangle_right;

            btnGraphRespinte.Text = "Grafico commesse respinte";
            btnGraphRespinte.BackColor = Color.FromArgb(210, 210, 210);
            btnGraphRespinte.ForeColor = Color.FromArgb(113, 113, 113);
            btnGraphRespinte.Image = Properties.Resources.arrow_triangle_right;
        }

        private void BackUpDataOnSync()
        {
            var xEle = new XElement("ProduzioneGantt",
              from models in TaskList
              select new XElement("Model",
              new XAttribute("Order", models.Name),
              new XElement("QtyH", models.QtyH),
              new XElement("StartDate", models.StartDate),
              new XElement("Duration", models.Duration),
              new XElement("EndDate", models.EndDate),
              new XElement("LoadQty", models.LoadedQty),
              new XElement("ProdStart", models.ProductionStartDate),
              new XElement("ProdEnd", models.ProductionEndDate),
              new XElement("DailyProd", models.DailyProd)
              ));

            if (!System.IO.Directory.Exists(Application.StartupPath + "\\" + "Models backup"))
            {
                System.IO.Directory.CreateDirectory(Application.StartupPath + "\\" + "Models backup");
            }

            xEle.Save(Application.StartupPath + "\\" +
                "Models backup\\"
                + "model"
                + DateTime.Now.ToString("ddMMyyyy-HHmmss")
                + ".xml");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pnReload.Visible = true;
        }

        private Geometry _geometry = new Geometry();

        private void btnReloadItem_Paint(object sender, PaintEventArgs e)
        {
            var btn = (Button)sender;

            using (GraphicsPath path = _geometry.RoundedRectanglePath(new Rectangle(0, 0, btn.Width, btn.Height), 30))
            {
                SmoothingMode old = e.Graphics.SmoothingMode;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.DrawPath(new Pen(Brushes.DimGray, 1), _geometry.RoundedRectanglePath(new Rectangle(-1, -1, btn.Width, btn.Height), 30));

                btn.Region = new Region(path);
                e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            }
        }

        private void pnReload_Paint(object sender, PaintEventArgs e)
        {
            using (GraphicsPath path = _geometry.RoundedRectanglePath(new Rectangle(0, 0, pnReload.Width, pnReload.Height), 10))
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.DrawPath(new Pen(Brushes.Black, 1), _geometry.RoundedRectanglePath(new Rectangle(-1, -1, pnReload.Width, pnReload.Height), 10));
                e.Graphics.FillPath(new SolidBrush(Color.FromArgb(50, 255, 255, 255)), path);

                pnReload.Region = new Region(path);

                e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            }
        }

        private void label11_Paint(object sender, PaintEventArgs e)
        {

            var btn = (Label)sender;

            using (GraphicsPath path = _geometry.RoundedRectanglePath(new Rectangle(0, 0, btn.Width, btn.Height), 10))
            {
                SmoothingMode old = e.Graphics.SmoothingMode;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.DrawPath(new Pen(Brushes.DimGray, 1), _geometry.RoundedRectanglePath(new Rectangle(-1, -1, btn.Width, btn.Height), 10));

                btn.Region = new Region(path);
                e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            }
        }

        private void btnResetItem_MouseEnter(object sender, EventArgs e)
        {
            lblResetGlobal.BackColor = Color.Gainsboro;
        }

        private void btnResetItem_MouseLeave(object sender, EventArgs e)
        {
            lblResetGlobal.BackColor = Color.White;
        }

        private void btnReloadItem_MouseEnter(object sender, EventArgs e)
        {
            lblRefreshGlobal.BackColor = Color.Gainsboro;
        }

        private void btnReloadItem_MouseLeave(object sender, EventArgs e)
        {
            lblRefreshGlobal.BackColor = Color.White;
        }

        private void lblResetGlobal_MouseEnter(object sender, EventArgs e)
        {
            lblResetGlobal.BackColor = Color.Gainsboro;
        }

        private void lblResetGlobal_MouseLeave(object sender, EventArgs e)
        {

            lblResetGlobal.BackColor = Color.WhiteSmoke;
        }

        private void lblRefreshGlobal_MouseEnter(object sender, EventArgs e)
        {
            lblRefreshGlobal.BackColor = Color.Gainsboro;
        }

        private void lblRefreshGlobal_MouseLeave(object sender, EventArgs e)
        {
            lblRefreshGlobal.BackColor = Color.WhiteSmoke;
        }

        internal bool _fromNavigation = false;
        private void BtnForward_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == listBox1.Items.Count - 1)
            {
                btnForward.Enabled = false;
                return;
            }
            btnBack.Enabled = true;
            _fromNavigation = true;
            if ((TreeNode)listBox1.SelectedItem == FindLastNode(treeMenu.SelectedNode))
                listBox1.SelectedIndex += 1;
            else
                listBox1.SelectedIndex = listBox1.Items.IndexOf(FindLastNode(treeMenu.SelectedNode));

            treeMenu.SelectedNode = (TreeNode)listBox1.SelectedItem;
        }
        private void BtnBack_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == 0)
            {
                btnBack.Enabled = false;
                return;
            }
            btnForward.Enabled = true;
            _fromNavigation = true;

            if ((TreeNode)listBox1.SelectedItem == FindLastNode(treeMenu.SelectedNode))
                listBox1.SelectedIndex -= 1;
            else
                listBox1.SelectedIndex = listBox1.Items.IndexOf(treeMenu.SelectedNode);

            treeMenu.SelectedNode = (TreeNode)listBox1.SelectedItem;
        }

        private TreeNode FindLastNode(TreeNode x)
        {
            return x;
        }

        private void CbDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbDept.Text == "<Multiple>")
            {
                Store.Default.selDept = Store.Default.arrDept;
                Store.Default.Save();
            }
            else
            {
                Store.Default.selDept = "," + cbDept.Text + ",";
                Store.Default.Save();
            }
        }

        public void AddDepartmentsToCombo()
        {
            cbDept.Items.Clear();

            var dpt = Store.Default.arrDept.Split(',');

            cbDept.Items.Add("<Multiple>");

            for (var i = 0; i <= dpt.Length - 1; i++)
            {
                if (dpt[i] == string.Empty) continue;
                cbDept.Items.Add(dpt[i]);
            }

            cbDept.SelectedIndex = cbDept.FindString(Store.Default.selDept.TrimStart(',').TrimEnd(','));
            if (cbDept.SelectedIndex < 0)
            {
                cbDept.SelectedIndex = cbDept.FindString("<Multiple>");
            }
        }

        private void BtnFatturatoLinea_Paint(object sender, PaintEventArgs e)
        {
            var btn = (Button)sender;

            using (GraphicsPath path = _geometry.RoundedRectanglePath(new Rectangle(-1,-1, btn.Width - 1, btn.Height - 1), 4))
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                btn.Region = new Region(path);
                e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            }
        }

        private void PnTitleBar_Paint(object sender, PaintEventArgs e)
        {
            var pn = (Panel)sender;
            var fnt = new Font("Segoe UI", 12);

            var logoRect = new Rectangle(10, 5, 102, 40);
            e.Graphics.DrawImage(Properties.Resources.Logo, logoRect);

            SectorTitle = Store.Default.selDept.Replace(',', '/').TrimStart('/').TrimEnd('/') + " - ";       
            var refreshTitle = SectorTitle + RefreshTitle;
            var refreshTitleSize = e.Graphics.MeasureString(refreshTitle, fnt);

            var posX = pn.Width / 2 - refreshTitleSize.Width / 2;
            var posY = pn.Height / 2 - refreshTitleSize.Height / 2;

            e.Graphics.DrawString(refreshTitle, fnt, Brushes.WhiteSmoke, posX, posY);
        }

        private void Button2_Click_2(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void Button1_Click_1(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
            {
                WindowState = FormWindowState.Normal;
                StartPosition = FormStartPosition.Manual;
                Location = new Point(Screen.PrimaryScreen.WorkingArea.Left + 10, Screen.PrimaryScreen.WorkingArea.Top + 10);
            }
            else
            {
                WindowState = FormWindowState.Maximized;
            }
        }

        private void Central_SizeChanged(object sender, EventArgs e)
        {
            pnTitlebar.Invalidate();
        }

        private void PnTitlebar_DoubleClick(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
            {
                WindowState = FormWindowState.Normal;
                StartPosition = FormStartPosition.Manual;
                Location = new Point(Screen.PrimaryScreen.WorkingArea.Left + 10, Screen.PrimaryScreen.WorkingArea.Top + 10);
            }
            else
            {
                WindowState = FormWindowState.Maximized;
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            _settings = new Settings();
            if (_hasNewUpdate) SettingsCompleted = SettingsSys.NewUpdate;
            _settings.FormClosing += (se, ee) =>
            {
                if (_settings.DialogResult == DialogResult.OK)
                {
                    lblDepartment.Text = Store.Default.selSector;
                    AddDepartmentsToCombo();
                    GetBase();

                    SectorTitle = Store.Default.selDept.Replace(',', '/').TrimStart('/').TrimEnd('/') + " - ";       

                    pnTitlebar.Invalidate();
                }
            };
            statusStrip.Refresh();
            ShowPopup(_settings);
        }

        private void LblResetGlobal_Click(object sender, EventArgs e)
        {
            pnTitlebar.Refresh();
        }

        private void LblRefreshGlobal_Click(object sender, EventArgs e)
        {
            pnTitlebar.Refresh();
        }

        private void BtnSync_Click(object sender, EventArgs e)
        {
            if (treeMenu.SelectedNode != treeMenu.Nodes[0].Nodes[1])
            {
                MessageBox.Show("Sync function works only when 'Produzione Gantt' is selected.", "Sync", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            LoadingInfo.ShowLoading();
            LoadingInfo.InfoText = "Computing...";
            //using (var context = new DataContext(SpecialConnStr))
            //using (var con = new System.Data.SqlClient.SqlConnection(SpecialConnStr))
            //{
            //    string dep = string.Empty;
            //    switch(Store.Default.sectorId)
            //    {
            //        case 2:
            //            dep = "Stiro";
            //            break;
            //        case 7:
            //            dep = "Tessitura";
            //            break;
            //        case 8:
            //            dep = "Sartoria";
            //            break;
            //    }
            //    if (Store.Default.sectorId == 1)
            //    {
            //        string department="Confezione B";
            //        var cmd = new SqlCommand("UpdateDelays", con);
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        cmd.Parameters.Add("@Department", SqlDbType.NVarChar).Value = department;
            //        con.Open();
            //        cmd.ExecuteNonQuery();
            //        con.Close();
            //        string department1 = "Confezione C";
            //        var cmd1 = new SqlCommand("UpdateDelays", con);
            //        cmd1.CommandType = CommandType.StoredProcedure;
            //        cmd1.Parameters.Add("@Department", SqlDbType.NVarChar).Value = department1;
            //        con.Open();
            //        cmd1.ExecuteNonQuery();
            //        con.Close();
            //    }
            //    else
            //    {
            //        var cmd = new SqlCommand("UpdateDelays", con);
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        cmd.Parameters.Add("@Department", SqlDbType.NVarChar).Value = dep;
            //        con.Open();
            //        cmd.ExecuteNonQuery();
            //        con.Close();
            //    }
                //    foreach (var model in TaskList)
                //{
                //    DateTime.TryParse(model.StartDate.ToString(), out var start);
                //    DateTime.TryParse(model.EndDate.ToString(), out var end);

                //    var dS = Config.MinimalDate;
                //    var dE = Config.MinimalDate;

                //    if (model.DelayStartDate != DateTime.MinValue && model.DelayEndDate != DateTime.MinValue)
                //    {
                //        DateTime.TryParse(model.DelayStartDate.ToString(), out dS);
                //        DateTime.TryParse(model.DelayEndDate.ToString(), out dE);
                //    }
                //    else
                //    {
                //        dS = Config.MinimalDate;
                //        dE = Config.MinimalDate;
                //    }


                //        DateTime.TryParse(model.Dvc.ToString(), out var dvc);
                //        DateTime.TryParse(model.Rdd.ToString(), out var rdd);

                //        var d = dvc == DateTime.MinValue ? 0L : dvc.Subtract(Config.MinimalDate).Ticks;
                //        var r = rdd == DateTime.MinValue ? 0L : rdd.Subtract(Config.MinimalDate).Ticks;
                //        var cmd = new SqlCommand("UpdateDelays", con);
                //        cmd.CommandType = CommandType.StoredProcedure;
                //        cmd.Parameters.Add("@StartFlow", SqlDbType.DateTime).Value = start;
                //        cmd.Parameters.Add("@EndFlow", SqlDbType.DateTime).Value = end;
                //        cmd.Parameters.Add("@DelayStartFlow", SqlDbType.DateTime).Value = dS;
                //        cmd.Parameters.Add("@DelayEndFlow", SqlDbType.DateTime).Value = dE;
                //        cmd.Parameters.Add("@dvc", SqlDbType.BigInt).Value = d;
                //        cmd.Parameters.Add("@rdd", SqlDbType.BigInt).Value = r;
                //        cmd.Parameters.Add("@order", SqlDbType.NVarChar).Value = model.Name;
                //        cmd.Parameters.Add("@aim", SqlDbType.NVarChar).Value = model.Aim;
                //        cmd.Parameters.Add("@department", SqlDbType.NVarChar).Value = model.Department;
                //        con.Open();
                //        cmd.ExecuteNonQuery();
                //        con.Close();
                //        context.ExecuteCommand("update objects set " +
                //                "flowstartdate={0},flowenddate={1},delflowstartdate={2},delflowenddate={3}, dvc={4}, rdd={5} " +
                //                "where ordername={6} and aim={7} and department={8}",
                //                start, end,
                //                dS, dE, d, r,
                //                model.Name, model.Aim, model.Department);
                //    }
           // }

            Config.SetGanttConn(new DataContext(SpecialConnStr));
                Config.SetOlyConn(new DataContext(ConnStr));
                LoadShifts();
                LoadHolidays();
                LoadingInfo.UpdateText("Synchronizing data... Please wait...");
                AddModels(true);
            
            var lst = (from models in TaskList
                       select models).OrderBy(x => x.Department)
                      .ThenBy(x => Convert.ToDouble(x.Aim.Remove(0, 5)))
                      .ThenBy(x => x.StartDate);

                TaskList = lst.ToList();

            LoadingInfo.CloseLoading();      
        }

        private void BtnHelp_Click(object sender, EventArgs e)
        {
            byte[] pdf = Properties.Resources.Help;

            MemoryStream ms = new MemoryStream(pdf);
            FileStream f = new FileStream("Help6786.pdf", FileMode.OpenOrCreate);
            ms.WriteTo(f);
            f.Close();
            ms.Close();
            System.Diagnostics.Process.Start("Help6786.pdf");
        }

        private void Central_Paint(object sender, PaintEventArgs e)
        {                
            var pn = pnForms.Location;   
            var pnS = pnForms.Size;
            var p = new Pen(Brushes.Silver, 2);
            var pX = pn.X - 2;
            var pY = pn.Y - 2;
            var pW = pnS.Width + 4;
            var pH = pnS.Height + 4;
            e.Graphics.DrawRectangle(p, pX, pY, pW,  pH);
            p.Dispose();
        }

        private bool _hasNewUpdate = false;
        private void CheckForUpdates(object info)
        {
            try
            {
                var pathMain = AppDomain.CurrentDomain.BaseDirectory;
                var strAssembOld = "";
                foreach (var file in Directory.GetFiles(pathMain))
                {
                    if (Path.GetExtension(file) != ".exe") continue;
                    strAssembOld = Assembly.LoadFile(file).GetName().Version.ToString();
                }

                var strAssembNew = "";
                foreach (var file in Directory.GetFiles(Store.Default.downloadSource))
                {
                    if (Path.GetExtension(file) != ".exe") continue;
                    strAssembNew = Assembly.LoadFile(file).GetName().Version.ToString();
                }

                var newVr = Config.ReturnAssemblyNumber(strAssembNew);
                var oldVr = Config.ReturnAssemblyNumber(strAssembOld);

                if (newVr <= oldVr)
                {
                    _hasNewUpdate = false;
                    return;
                }

                _hasNewUpdate = true;
                pbSettings.Refresh();
            }
            catch
            {
                MessageBox.Show("Invalid path or network connection.", Application.ProductName + " Settings",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _hasNewUpdate = false;
                pbSettings.Refresh();
            }
        }

        private void PbSettings_Paint(object sender, PaintEventArgs e)
        {
            var notif = 0;
            if (_hasNewUpdate) notif++;
            else { return; }       

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;            
            var rect = new Rectangle(e.ClipRectangle.X  + e.ClipRectangle.Width - 20,2,17,17);
            e.Graphics.FillEllipse(Brushes.Red, rect);         
            e.Graphics.DrawString(notif.ToString(), new Font("Segoe UI", 8, FontStyle.Bold), Brushes.WhiteSmoke, rect.X + 4, rect.Y + 2);
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
        }

        private void BtnChart_Click(object sender, EventArgs e)
        {
        }

        private void lblResetGlobal_Click_1(object sender, EventArgs e)
        {

        }

        public void GetProductionColor()
        {
            var q = "select * from produzioneRelation where mode='production'";
            var dt = new System.Data.DataTable();

            using (var c = new SqlConnection(SpecialConnStr))
            {
                c.Open();
                var cmd = new SqlCommand(q, c);
                var dr = cmd.ExecuteReader();
                dt.Load(dr);
                c.Close();
                dr.Close();
            }

            if (dt.Rows.Count == 0)
            {
                return;
            }

            int.TryParse(dt.Rows[0][2].ToString(), out var leff);
            int.TryParse(dt.Rows[1][2].ToString(), out var meff);
            int.TryParse(dt.Rows[2][2].ToString(), out var heff);

            LowEff = leff;
            MediumEff = meff;
            HighEff = heff;

            var c1 = dt.Rows[0][3].ToString();
            var c2 = dt.Rows[1][3].ToString();
            var c3 = dt.Rows[2][3].ToString();

            LowColor = ColorTranslator.FromHtml(c1);
            MediumColor = ColorTranslator.FromHtml(c2);
            HighColor = ColorTranslator.FromHtml(c3);
        }

        private void Central_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                return;
            }
        }
    }

    public class ReSize
    {
        private bool Above, Right, Under, Left, Right_above, Right_under, Left_under, Left_above;

        int Thickness = 6;
        int Area = 8;

        public ReSize(int thickness)
        {
            Thickness = thickness;
        }

        public ReSize()
        {
            Thickness = 10;
        }

        public string getMosuePosition(Point mouse, Form form)
        {
            if (form.WindowState == FormWindowState.Maximized) return "";

            bool above_underArea = mouse.X > Area && mouse.X < form.ClientRectangle.Width - Area;
            bool right_left_Area = mouse.Y > Area && mouse.Y < form.ClientRectangle.Height - Area;

            bool _Above = mouse.Y <= Thickness;
            bool _Right = mouse.X >= form.ClientRectangle.Width - Thickness;
            bool _Under = mouse.Y >= form.ClientRectangle.Height - Thickness;
            bool _Left = mouse.X <= Thickness;

            Above = _Above && (above_underArea); if (Above) return "a";
            Right = _Right && (right_left_Area); if (Right) return "r";
            Under = _Under && (above_underArea); if (Under) return "u";
            Left = _Left && (right_left_Area); if (Left) return "l";

            Right_above =(_Right && (!right_left_Area)) &&
                (_Above && (!above_underArea)); if (Right_above) return "ra";
            Right_under = ((_Right) && (!right_left_Area)) &&
                (_Under && (!above_underArea)); if (Right_under) return "ru";
            Left_under = ((_Left) && (!right_left_Area)) &&
                (_Under && (!above_underArea)); if (Left_under) return "lu";
            Left_above = ((_Left) && (!right_left_Area)) &&
                (_Above && (!above_underArea)); if (Left_above) return "la";

            return "";
        }
    }
}