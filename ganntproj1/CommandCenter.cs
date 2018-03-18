using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Linq;
using System.Data.SqlClient;

namespace ganntproj1
    {
    /// <summary>
    /// Class that represents a retaining data from sql or textual file.
    /// </summary>
    public partial class CommandCenter : Form
        {       
        #region Declerations

        //configuration
        private const string HeaderTitle = "ONLYOU";
        private int _colapsed;
        private const int SwHide = 0;
        private const int SwShow = 5;

        private Output _output = new Output();
        private TableView _dgvGantt;
        private readonly Channels _channels = new Channels();
        private readonly Config _config = new Config();
        private string[] _aim;
        private readonly BindingSource _bsRoot = new BindingSource();
        private readonly BindingSource _bsGantt = new BindingSource();
        private Filter _filter = new Filter();
        private List<Filter> _filtersList = new List<Filter>();
        private Ganttogram _gantt = new Ganttogram();
        private Label _title = new Label();

        //temporary save selected channel
        private string _tmp_channel, _sql_channel, _sql_mode,_sql_rdd_filter;

        private readonly MyPrintDialog _mpd = new MyPrintDialog();
        private readonly PrintDocument _printDoc = new PrintDocument();

        private Button btnFilter;
        private Button btnNavBack;
        private Button btnNavBackMega;
        private Button btnNavBackMegaPlus;
        private Button btnNavBackPlus;
        private Button btnNavForw;
        private Button btnNavForwMega;
        private Button btnNavForwMegaPlus;
        private Button btnNavForwPlus;
        private Button btnOpen;
        private Button btnPrint;
        private Button btnSave;
        private Button btnSaveAs;
        private Button btnSpecSort;
        private Button btnCleanse;
        private Button btnSwitch;
        private Button btnZoomIn;
        private Button btnZoomInPlus;
        private Button btnZoomOut;
        private Button btnZoomOutPlus;
        private Label lblChannels;
        private Label lblNavig;

        private SplitContainer DisplaySplitContainer;
        private StatusStrip displayStatus;
        private ToolStripStatusLabel DisplayStatusConsole;
        private Panel ganttContainer;

        private ToolStripMenuItem hideConsoleOutput;
        private Label lblFiltersTit;
        
        private ToolStripStatusLabel lblDateTimeInterval;
        private ToolStripStatusLabel lblDepthStatus;
        private Title mainTitle;
        private MiniTitle miniTitle1;
        private Panel pnNavPlus;
        private ToolStripMenuItem showConsoleOutput;
        private Button btnFullPrev;
        private ToolStripSplitButton toolStripSplitButton1;

        private Panel _panel1;
        private Button btnReport;
        private Panel pnSplitter1;
        private Panel panel3;
        private Label label3;
        private PictureBox pbStiro;
        private PictureBox pbConf;
        private PictureBox pbTess;
        private PictureBox pbStaz;
        private Panel panel2;
        private Label label4;
        private PictureBox pbStiro1;
        private PictureBox pbConf1;
        private PictureBox pbTess1;
        private PictureBox pbStaz1;

        private Button _btnTess;
        private Button _btnStiro;
        public Button BtnConf;
        private SplitContainer splitContainer1;
        private Button btnEndRemDays;
        private Button btnEndAddDays;
        private Button btnStartRemDays;
        private Button btnStartAddDays;
        private SplitContainer splitContainer2;

        private VScrollBar _vscroll = new VScrollBar();
        #endregion

        private void _vscroll_scroll(object sender, ScrollEventArgs e)
            {
            //FIXME (Calculate dgv vertical rows offset)
            try
                {
                _dgvGantt.FirstDisplayedScrollingRowIndex = _vscroll.Value;
                _dgvGantt.Refresh();

                _gantt.ScrollPosition = _vscroll.Value;
                _gantt.Refresh();
                }
            catch
                {

                }
            }

        //API declerations
        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);
        private const int WM_SETREDRAW = 11;

        //console drivers
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();
        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        public virtual CurrencyManager _currency_manager { get; set; }
        public Panel GanttContainer { get => ganttContainer; set => ganttContainer = value; }

        public delegate void FormClosingEventHandler(object sender, FormClosingEventArgs e);
        private Thread _trd_update_display;

        private DataTable _datatable = new DataTable();

        #region StartTask

        [STAThread]
        private static void Main(string[] args)
            {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(true);

            // Start with parameters
            //-->
            
            var menu = new Central();
            //foreach (var men in Central.ListOfModels)
            //{
            //    Console.WriteLine(men);
            //}
            menu.ShowDialog();
        }

        public CommandCenter()
            {
            InitializeDisplay();

            this.DoubleBuffered(true);
            _dgvGantt.DoubleBuffered(true);

            _gantt.Click += Gantt_Click;
            _gantt.DoubleClick += Gantt_DoubleClick;
            _dgvGantt.CellDoubleClick += dgvGannt_CellDoubleClick;
            _dgvGantt.CellClick += dgvGant_CellClick;
            _dgvGantt.ScrollBars = ScrollBars.Horizontal;
            Controls.Add(_vscroll);
            _vscroll.Dock = DockStyle.Right;
            _vscroll.SendToBack();
            _vscroll.Height = 100;
            _vscroll.Location = new Point(10, 10);
            _vscroll.Scroll += _vscroll_scroll;
            
            mainTitle.SendToBack();

            DisplaySplitContainer.SplitterDistance = 350;
            }

        private void CallingFunction()
            {
            var methodInvoker = new MethodInvoker(HighlightDisplay);
            Invoke(methodInvoker);
            }

        private void DisplayData()
            {
            _trd_update_display = new Thread(new ThreadStart(CallingFunction))
                {
                Name = "Updating " + Text + " display",
                Priority = ThreadPriority.Normal
                };
            
            _trd_update_display.TrySetApartmentState(ApartmentState.STA);
            if (_trd_update_display.ThreadState != ThreadState.Running) _trd_update_display.Start();
            }

        protected override void OnLoad(EventArgs e)
            { 
            _gantt.AllowRowSelection = true;

            DistributeExcelFileToDba();

            // Automatically set filter to default channel

            Filter.SetFilteredChannel(_channels.ListOfChannels().First().Channel); //default value

            // Set filtering properties to default values
            
            _tmp_channel = Filter.GetFilteredChannel();
            Filter.SetFilteredKey(null);
            Filter.SetAllChannels(false);
            Filter.SetFilteredKeys(new List<string>());
            _aim = new[] { "Commessa", "Articolo", "Finezza", "Capi commessa", "Stagione" };
            _sql_channel = "[Data Consegna Tessitura]";
            _sql_mode = @"normal";
            _sql_rdd_filter = "[Data Dvc]";

            // Load data in full mode

            LoadTableFromDba();
            DisplayData();
            btnFullPrev.PerformClick();

            base.OnLoad(e);
            }
        
        #endregion

        #region Engine

        private void DistributeExcelFileToDba()
            {
            var srcFile = Store.Default.outputDir;  //gets Excel source path
            var sheetName = Store.Default.worksheetName; //gets Excel worksheet
            var tgFile = Store.Default.targetFile;  //gets target path

            //var strConn = string.Empty; 
            //var path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "set.txt"); 
            //using (var sr = new System.IO.StreamReader(path))
            //    {
            //    strConn = sr.ReadLine();
            //    _config.Set_sql_conn(new SqlConnection(strConn));
            //    }
            _config.Set_sql_conn(new SqlConnection(_config.ReadSqlConnectionString(1)));
            _config.PopulateTableUsingCsv(srcFile);
            _config.ExportTableToDba();

            var currentDate = DateTime.Now;
            _config.StartDate = new DateTime(currentDate.Year, currentDate.Month, 1);
            _config.EndDate = _config.StartDate.AddMonths(1).AddDays(-1);
            
            _gantt.FromDate = _config.StartDate;
            _gantt.ToDate = _config.EndDate;
            }

        private void LoadTableFromDba()
            {
            //LoadingInfo.InfoText = "Updating records from database...";
            //LoadingInfo.ShowLoading();
            _datatable = new DataTable();

            var cmd = new SqlCommand
                {
                Connection = _config.Get_sql_conn(),
                CommandType = CommandType.StoredProcedure,
                CommandText = "get_data_avanzamento"
                };

            // add command params
            cmd.Parameters.AddWithValue("@from_date", SqlDbType.DateTime).Value = _config.StartDate;
            cmd.Parameters.AddWithValue("@to_date", SqlDbType.DateTime).Value = _config.EndDate;
            cmd.Parameters.AddWithValue("@filter", SqlDbType.VarChar).Value = _sql_channel;
            cmd.Parameters.AddWithValue("@rdd_filter", SqlDbType.VarChar).Value = _sql_rdd_filter;
            cmd.Parameters.AddWithValue("@order_mode", SqlDbType.VarChar).Value = _sql_mode;

            _config.Get_sql_conn().Open();
            var dr = cmd.ExecuteReader();
            _datatable.Load(dr);
            _config.Get_sql_conn().Close();
            dr.Close();
            cmd = null;
       
            //tuning datagridview
            _dgvGantt.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
            _dgvGantt.RowHeadersVisible = false;

            if (_bsGantt.DataSource != null)
                {
                _bsGantt.DataSource = null;
                _dgvGantt.DataSource = null;
                }
            
            _bsGantt.DataSource = _datatable;
            _dgvGantt.DataSource = _bsGantt;

            for (var i = 0; i <= 4; i++)
                {
                _dgvGantt.Columns[i].DefaultCellStyle.BackColor = Color.Silver;
                _dgvGantt.Columns[i].Frozen = true;
                }

            for (var col = 5; col <= 17; col++) _dgvGantt.Columns[col].HeaderCell.Style.BackColor = Color.DarkTurquoise;
            for (var col = 18; col <= 31; col++) _dgvGantt.Columns[col].HeaderCell.Style.BackColor = Color.Violet;
            for (var col = 32; col <= _dgvGantt.Columns.Count - 6; col++) _dgvGantt.Columns[col].HeaderCell.Style.BackColor = Color.LightGreen;

            _dgvGantt.Columns[2].HeaderText = "Fin";
            _dgvGantt.Columns[4].HeaderText = "Stag";
            _dgvGantt.Columns[17].HeaderText = "Dest";
            _dgvGantt.Columns[7].ValueType = typeof(double);

            //resize filter columns
            _dgvGantt.Columns[0].Width = 80;
            _dgvGantt.Columns[1].Width = 80;
            _dgvGantt.Columns[2].Width = 40;
            _dgvGantt.Columns[3].Width = 50;
            _dgvGantt.Columns[4].Width = 50;
            _dgvGantt.Columns[27].HeaderText = "C. usciti Conf";
            _dgvGantt.Columns[41].HeaderText = "C. usciti Stiro";

            foreach (DataGridViewColumn dgvc in _dgvGantt.Columns)
                {
                if (dgvc.ValueType == typeof(DateTime))
                    {
                    dgvc.Width = 80;
                    dgvc.DefaultCellStyle.BackColor = Color.Gainsboro;
                    }
                    }

            _dgvGantt.ColumnHeadersHeight = 40;
            
            //foreach (DataRow row in _datatable.Rows)
            //    {
            //    for (var k = 0; k <= 4; k++)
            //        {
            //        if (k == 3) continue;

            //        _filtersList.Add(new Filter(_aim[k], row.ItemArray.GetValue(k).ToString()));
            //        }
            //    }

            //LoadingInfo.CloseLoading();
            }

        /// <summary>
        /// Crucial structure for maintaining objects through the algorithm.
        /// </summary>
        private void HighlightDisplay()
            {
            _gantt.FromDate = _config.StartDate;
            _gantt.ToDate = _config.EndDate;
            
            if (_filter_multiply) _dgvGantt.MultiSelect = true; else _dgvGantt.MultiSelect = false;

            //initialize bars lists                
            _gantt.Bars = new List<BarProperty>();
            _gantt.HeaderList = new List<Header>();

            var barList = new List<Bar>();
            var fullBarList = new List<Bar>();

            var sb = new StringBuilder();
            var lastChannel = _channels.ListOfChannels().Last().Channel;

            //initializes filters and visibility lists

            _filtersList = new List<Filter>();

            if (Filter.GetAllChannels())
                {
                //show all columns in full mode
                for (var c = 5; c <= _dgvGantt.Columns.Count - 6; c++)
                    _dgvGantt.Columns[c].Visible = true;

                for (var c = _dgvGantt.Columns.Count - 5; c <= _dgvGantt.Columns.Count - 1; c++)
                    _dgvGantt.Columns[c].Visible = false;

                //build title
                foreach (var c in _channels.ListOfChannels())
                    if (c.Channel != lastChannel)
                        sb.Append(c.Channel + " + ");
                    else
                        sb.Append(c.Channel);
                }
            else
                {
                for (var c = 5; c <= _dgvGantt.Columns.Count - 1; c++)
                    {
                    _dgvGantt.Columns[c].Visible = false;
                    }

                //collect responsible channels
                sb.Append(Filter.GetFilteredChannel());
                }

            var filteredChannel = sb.ToString();
            lblChannels.Text = filteredChannel;
            lblDepthStatus.Text = @"Channel " + filteredChannel;

            for (var i = 0; i <= _channels.ListOfChannels().Count - 1; i++)
                {
                //skip unfiltered channels
                if (_channels.ListOfChannels()[i].Channel != Filter.GetFilteredChannel())
                    continue;
                
                //sets new columns visibility
                if (!Filter.GetAllChannels())
                    {
                    if (i == 0)
                        for (var col = 0; col <= 17; col++) _dgvGantt.Columns[col].Visible = true;
                    else if (i == 1)
                        for (var col = 18; col <= 31; col++) _dgvGantt.Columns[col].Visible = true;
                    else if (i == 2)
                        for (var col = 32; col <= _dgvGantt.Columns.Count - 6; col++) _dgvGantt.Columns[col].Visible = true;
                    }

                var index = 0;
                var fullIndex = 0;

                foreach (DataRow row in _datatable.Rows)
                {
                    var comesa = row[0].ToString();

                    var article = row[1].ToString();
                    var finesess = row[2].ToString();
                    var qty = row[3].ToString();
                    var season = row[4].ToString();

                    if (_filter_multiply && !Filter.GetFilteredKeys().Contains(comesa)) continue;

                    //gets date points through channels

                    var tessYarn = _config.ConvertToUniTime(row[8]);
                    var tessInit = _config.ConvertToUniTime(row[9]);
                    var tessEnd = _config.ConvertToUniTime(row[11]);
                    var tessConseg = _config.ConvertToUniTime(row[13]);
                    var tessRdd = _config.ConvertToUniTime(row[16]);
                    var confYarn = _config.ConvertToUniTime(row[18]);
                    var confInit = _config.ConvertToUniTime(row[19]);
                    var confEnd = _config.ConvertToUniTime(row[21]);
                    var confConseg = _config.ConvertToUniTime(row[23]);
                    var confRdd = _config.ConvertToUniTime(row[26]);
                    var stiYarn = _config.ConvertToUniTime(row[32]);
                    var stiInit = _config.ConvertToUniTime(row[33]);
                    var stiEnd = _config.ConvertToUniTime(row[35]);
                    var stiConseg = _config.ConvertToUniTime(row[37]);
                    var stiRdd = _config.ConvertToUniTime(row[40]);
                    var dvc = _config.ConvertToUniTime(row[48]);

                    //Populate chart in full mode, using temporary end date parameter,
                    //on basic channel state

                    if (!string.IsNullOrEmpty(Filter.GetFilteredKey()) &&
                        (Filter.GetFilteredKey() == comesa) |
                        (Filter.GetFilteredKey() == article) |
                        (Filter.GetFilteredKey() == finesess) |
                        Filter.GetFilteredKey() == season)
                    {
                        fullBarList.Add(new Bar(comesa, tessYarn, tessInit, DateTime.MinValue, dvc, Color.Silver, Color.WhiteSmoke, fullIndex));
                        fullBarList.Add(new Bar(comesa, tessInit, tessEnd, DateTime.MinValue, dvc, Color.DarkTurquoise, Color.PapayaWhip, fullIndex));
                        fullBarList.Add(new Bar(comesa, tessEnd, tessConseg, DateTime.MinValue, dvc, Color.Gainsboro, Color.WhiteSmoke, fullIndex));
                        fullBarList.Add(new Bar(comesa, confYarn, confInit, DateTime.MinValue, dvc, Color.Gainsboro, Color.WhiteSmoke, fullIndex));
                        fullBarList.Add(new Bar(comesa, confInit, confEnd, DateTime.MinValue, dvc, Color.Violet, Color.PapayaWhip, fullIndex));
                        fullBarList.Add(new Bar(comesa, confEnd, confConseg, DateTime.MinValue, dvc, Color.Gainsboro, Color.WhiteSmoke, fullIndex));
                        fullBarList.Add(new Bar(comesa, stiYarn, stiInit, DateTime.MinValue, dvc, Color.Gainsboro, Color.WhiteSmoke, fullIndex));
                        fullBarList.Add(new Bar(comesa, stiInit, stiEnd, DateTime.MinValue, dvc, Color.LightGreen, Color.PapayaWhip, fullIndex));
                        fullBarList.Add(new Bar(comesa, stiEnd, stiConseg, DateTime.MinValue, dvc, Color.Gainsboro, Color.WhiteSmoke, fullIndex));

                        fullIndex++;
                    }
                    else if (string.IsNullOrEmpty(Filter.GetFilteredKey()))
                    {
                        fullBarList.Add(new Bar(comesa, tessYarn, tessInit, DateTime.MinValue, dvc, Color.Silver, Color.WhiteSmoke, fullIndex));
                        fullBarList.Add(new Bar(comesa, tessInit, tessEnd, DateTime.MinValue, dvc, Color.DarkTurquoise, Color.PapayaWhip, fullIndex));
                        fullBarList.Add(new Bar(comesa, tessEnd, tessConseg, DateTime.MinValue, dvc, Color.Gainsboro, Color.WhiteSmoke, fullIndex));
                        fullBarList.Add(new Bar(comesa, confYarn, confInit, DateTime.MinValue, dvc, Color.Gainsboro, Color.WhiteSmoke, fullIndex));
                        fullBarList.Add(new Bar(comesa, confInit, confEnd, DateTime.MinValue, dvc, Color.Violet, Color.PapayaWhip, fullIndex));
                        fullBarList.Add(new Bar(comesa, confEnd, confConseg, DateTime.MinValue, dvc, Color.Gainsboro, Color.WhiteSmoke, fullIndex));
                        fullBarList.Add(new Bar(comesa, stiYarn, stiInit, DateTime.MinValue, dvc, Color.Gainsboro, Color.WhiteSmoke, fullIndex));
                        fullBarList.Add(new Bar(comesa, stiInit, stiEnd, DateTime.MinValue, dvc, Color.LightGreen, Color.PapayaWhip, fullIndex));
                        fullBarList.Add(new Bar(comesa, stiEnd, stiConseg, DateTime.MinValue, dvc, Color.Gainsboro, Color.WhiteSmoke, fullIndex));

                        fullIndex++;
                    }

                    //populate chart in single mode
                    switch (i)
                    {
                        case 0:

                            //load data based on date interval selection
                            if (!string.IsNullOrEmpty(Filter.GetFilteredKey()) &&
                              (Filter.GetFilteredKey() == comesa) |
                              (Filter.GetFilteredKey() == article) |
                              (Filter.GetFilteredKey() == finesess) |
                              Filter.GetFilteredKey() == season)
                            {
                                barList.Add(new Bar(comesa, tessYarn, tessInit, tessRdd, Color.Silver, Color.WhiteSmoke, index));
                                barList.Add(new Bar(comesa, tessInit, tessEnd, tessRdd, Color.DarkTurquoise, Color.PapayaWhip, index));
                                barList.Add(new Bar(comesa, tessEnd, tessConseg, tessRdd, Color.Gainsboro, Color.WhiteSmoke, index));
                                index++;
                            }
                            else if (string.IsNullOrEmpty(Filter.GetFilteredKey()))
                            {
                                barList.Add(new Bar(comesa, tessYarn, tessInit, tessRdd, Color.Silver, Color.WhiteSmoke, index));
                                barList.Add(new Bar(comesa, tessInit, tessEnd, tessRdd, Color.DarkTurquoise, Color.PapayaWhip, index));
                                barList.Add(new Bar(comesa, tessEnd, tessConseg, tessRdd, Color.Gainsboro, Color.WhiteSmoke, index));
                                index++;
                            }
                            break;
                        case 1:
                            if (!string.IsNullOrEmpty(Filter.GetFilteredKey()) &&
                              (Filter.GetFilteredKey() == comesa) |
                              (Filter.GetFilteredKey() == article) |
                              (Filter.GetFilteredKey() == finesess) |
                              Filter.GetFilteredKey() == season)
                            {
                                barList.Add(new Bar(comesa, confYarn, confInit, confRdd, Color.Gainsboro, Color.WhiteSmoke, index));
                                barList.Add(new Bar(comesa, confInit, confEnd, confRdd, Color.Violet, Color.PapayaWhip, index));
                                barList.Add(new Bar(comesa, confEnd, confConseg, confRdd, Color.Gainsboro, Color.WhiteSmoke, index));
                                index++;
                            }
                            else if (string.IsNullOrEmpty(Filter.GetFilteredKey()))
                            {
                                barList.Add(new Bar(comesa, confYarn, confInit, confRdd, Color.Gainsboro, Color.WhiteSmoke, index));
                                barList.Add(new Bar(comesa, confInit, confEnd, confRdd, Color.Violet, Color.PapayaWhip, index));
                                barList.Add(new Bar(comesa, confEnd, confConseg, confRdd, Color.Gainsboro, Color.WhiteSmoke, index));
                                index++;
                            }
                            break;
                        case 2:
                            if (!string.IsNullOrEmpty(Filter.GetFilteredKey()) &&
                               (Filter.GetFilteredKey() == comesa) |
                               (Filter.GetFilteredKey() == article) |
                               (Filter.GetFilteredKey() == finesess) |
                               Filter.GetFilteredKey() == season)
                            {
                                barList.Add(new Bar(comesa, stiYarn, stiInit, stiRdd, Color.Gainsboro, Color.WhiteSmoke, index));
                                barList.Add(new Bar(comesa, stiInit, stiEnd, stiRdd, Color.LightGreen, Color.PapayaWhip, index));
                                barList.Add(new Bar(comesa, stiEnd, stiConseg, stiRdd, Color.Gainsboro, Color.WhiteSmoke, index));
                                index++;
                            }
                            else if (string.IsNullOrEmpty(Filter.GetFilteredKey()))
                            {
                                barList.Add(new Bar(comesa, stiYarn, stiInit, stiRdd, Color.Gainsboro, Color.WhiteSmoke, index));
                                barList.Add(new Bar(comesa, stiInit, stiEnd, stiRdd, Color.LightGreen, Color.PapayaWhip, index));
                                barList.Add(new Bar(comesa, stiEnd, stiConseg, stiRdd, Color.Gainsboro, Color.WhiteSmoke, index));
                                index++;
                            }
                            break;
                    }
                }
            }

            //if (Filter.GetAllChannels())
            //{
            //    foreach (var bar in fullBarList)    //use full bar list
            //    {
            //        _gantt.AddBars(bar.RowText,
            //            bar, bar.ToDvc, bar.ToRealTime, bar.FromTime, bar.ToTime, bar.Color, bar.HoverColor, bar.Index);
            //    }
            //}
            //else
            //{
            //    foreach (var bar in barList)    //use single list
            //    {
            //        _gantt.AddBars(bar.RowText,
            //            bar, bar.ToDvc, bar.ToRealTime, bar.FromTime, bar.ToTime, bar.Color, bar.HoverColor, bar.Index);
            //    }
            //}

            _gantt.Refresh();
            
            _dtpDateFrom.Value = _config.StartDate;
            _dtpDateTo.Value = _config.EndDate;
            
            lblDateTimeInterval.Text = @"Interval from " + _config.StartDate.ToLongDateString() + @" to " + _config.EndDate.ToLongDateString();
            lblNavig.Text = "Commesse: " + _dgvGantt.Rows.Count.ToString() + " / Filtro: " + Filter.GetFilteredKeys().Count.ToString();

            IntegrateFilterBoxesIntoList();

            HoldFilterMultiSelection();
            }

        #endregion

        #region TitleCommandButtonsAndNavigation

        private void btnOpen_Click(object sender, EventArgs e)
            {
            var commandCenter = new CommandCenter();
            commandCenter.Close();

            var outputFileDialog = new Config.OpenDialog();
            outputFileDialog.ShowDialog();
            outputFileDialog.Dispose();

            if (outputFileDialog.DialogResult != DialogResult.OK) return;

            LoadTableFromDba();

            DisplayData();
            }
        private void btnNavBackDoub_Click(object sender, EventArgs e)
            {
            _config.StartDate = _config.StartDate.AddDays(-7);
            _config.EndDate = _config.EndDate.AddDays(-7);
         
            _gantt.FromDate = _config.StartDate;
            _gantt.ToDate = _config.EndDate;
            _gantt.Refresh();
            }

        private void btnNavForwDoub_Click(object sender, EventArgs e)
            {
            _config.StartDate = _config.StartDate.AddDays(7);
            _config.EndDate = _config.EndDate.AddDays(7);

            _gantt.FromDate = _config.StartDate;
            _gantt.ToDate = _config.EndDate;
            _gantt.Refresh();
            }

        private void btnNavBack_Click(object sender, EventArgs e)
            {
            _config.StartDate = _config.StartDate.AddDays(-1);
            _config.EndDate = _config.EndDate.AddDays(-1);

            _gantt.FromDate = _config.StartDate;
            _gantt.ToDate = _config.EndDate;
            _gantt.Refresh();
            }

        private void btnNavForw_Click(object sender, EventArgs e)
            {
            _config.StartDate = _config.StartDate.AddDays(1);
            _config.EndDate = _config.EndDate.AddDays(1);

            _gantt.FromDate = _config.StartDate;
            _gantt.ToDate = _config.EndDate;
            _gantt.Refresh();
            }

        private void btnZoomIn_Click(object sender, EventArgs e)
            {
            if (_config.EndDate.Subtract(_config.StartDate).TotalDays < 8)
                {
                return;
                }

            _config.StartDate = _config.StartDate.AddDays(+1);
            _config.EndDate = _config.EndDate.AddDays(-1);

            _gantt.FromDate = _config.StartDate;
            _gantt.ToDate = _config.EndDate;
            _gantt.Refresh();
            }

        private void btnZoomOut_Click(object sender, EventArgs e)
            {
            if (_config.EndDate.Subtract(_config.StartDate).TotalDays > 90) //three months maximum
                {
                return;
                }

            _config.StartDate = _config.StartDate.AddDays(-1);
            _config.EndDate = _config.EndDate.AddDays(+1);

            _gantt.FromDate = _config.StartDate;
            _gantt.ToDate = _config.EndDate;
            _gantt.Refresh();
            }

        private void btnSwitch_Click(object sender, EventArgs e)
            {
            pnNavPlus.Visible = false;
            pnSplitter1.Visible = false;

            switch (_colapsed)
                {
                case 0:
                DisplaySplitContainer.Panel1Collapsed = true;
                _colapsed = 1; //only list
                break;
                case 1:
                DisplaySplitContainer.Panel2Collapsed = true;
                pnNavPlus.Visible = true;
                pnSplitter1.Visible = true;
                _colapsed = 2;  //list + gantt
                break;
                case 2:
                DisplaySplitContainer.Panel1Collapsed = false;
                DisplaySplitContainer.Panel2Collapsed = false;
                _colapsed = 0;  //full
                break;
                }
            }

        private void btnFilter_Click(object sender, EventArgs e)
            {
            var frm = new FilterSet(GetFilterSource());
            frm.ShowDialog();
            frm.Dispose();

            if (frm.DialogResult == DialogResult.OK && Filter.GetFilteredKey() != string.Empty)
                {
                DisplayData();

                foreach (var kvp in GetFiltersDictionary())
                    {
                    if (kvp.Key != Filter.GetFilteredKey()) continue;

                    var aim = kvp.Value;

                    //apply filtering on binding source for gantt
                    _bsGantt.Filter = string.Format("CONVERT(" +
                                                        _dgvGantt.Columns[aim]?.Name +
                                                        ", System.String) like '%" + kvp.Key.Replace("'", "''") + "%'");
                    }
                }
            else if (frm.DialogResult == DialogResult.OK && Filter.GetFilteredKey() == string.Empty)
                {
                DisplayData();
                }
            }
        
        private string _tmpImageFilePath;
        private void btnPrint_Click(object sender, EventArgs e)
            {
            if (_colapsed == 0)
                {
                MessageBox.Show(@"Printing function is available, only in an expanded state.");
                return;
                }

            //LoadingInfo.InfoText = "Executing printers query";
            //LoadingInfo.ShowLoading();

            _mpd.Show();    //show custom print dialog

            if (!_mpd.Printing) return;

            //save gannt as a document
            if (_colapsed == 1)
                {
                //print data grid
                if (_dgvGantt.Rows.Count <= 0)
                    {
                    MessageBox.Show(@"No records");
                    return;
                    }
                }
            else
                {
                //take some time, or other format, that will never be repeated 
                var tmpFile = "\\gnt" + DateTime.UtcNow.ToString("ssffffff");

                _tmpImageFilePath = (_config.GlobalDir + tmpFile);
                _gantt.SaveImage(_tmpImageFilePath);
                }

            //sets the document properties

            _printDoc.DocumentName = _mpd.DocumentTitle = HeaderTitle + " " + Text + _mpd.DocumentTitle;
            _printDoc.PrinterSettings.PrintFileName = _mpd.DocumentTitle;
            _printDoc.PrinterSettings.PrinterName = _mpd.Printer;   //set printer
            _printDoc.DefaultPageSettings.Landscape = true;

            PrinterSettings ps = new PrinterSettings();
            IEnumerable<PaperSize> paperSizes = ps.PaperSizes.Cast<PaperSize>();

            PaperSize paperSize = paperSizes.FirstOrDefault(size => size.Kind == _mpd.Formater);
            _printDoc.DefaultPageSettings.PaperSize = paperSize;

            _printDoc.BeginPrint += Document_StartPrint;
            _printDoc.PrintPage += Document_Print;
            _printDoc.Print();
            }

        private void btnSpecSort_Click(object sender, EventArgs eventArgs)
            {
            _sql_mode = @"sort";

            LoadTableFromDba();
            DisplayData();
            }
        #endregion

        #region Filters
        private AutoCompleteStringCollection GetFilterSource()
            {
            //populate filter with completed custom source

            var source = new AutoCompleteStringCollection();
            for (var i = 0; i <= _filtersList.Count - 1; i++)
                source.AddRange(new[]
                {
                    //import keys
                    _filtersList.ElementAt(i).GetFilterKey()
                });

            return source;
            }

        private Dictionary<string, string> GetFiltersDictionary()
            {
            //initialize filtering structure

            var dictionary = new Dictionary<string, string>();
            foreach (var item in _filtersList)
                if (!dictionary.ContainsKey(item.GetFilterKey()))
                    dictionary.Add(item.GetFilterKey(), item.GetFilterAim());

            return dictionary;
            }

        private bool _filtersCreated = false;
        private bool _text_filtere_Mode = false;
        private void IntegrateFilterBoxesIntoList()
            {
            if (_filtersCreated) return;

            if (_dgvGantt.Rows.Count <= 0) return;

            for (var i = 0; i <= 4; i++)
                {
                if (i == 3) continue;   //skip capi commesa

                //make space for textboxes
                _dgvGantt.Columns[i].DataGridView.ColumnHeadersDefaultCellStyle.Alignment =
                    DataGridViewContentAlignment.TopLeft;

                var txt = new TextBox
                    {
                    Name = _aim[i].ToString(),
                    BorderStyle = BorderStyle.None,
                    BackColor = Color.AliceBlue,
                    Font = new Font("Microsoft Sans Serif", 9, FontStyle.Regular),

                    //!use this if you not have paired values

                    //txt.DataBindings.Add("Text", _datatable, _aim[i]);
                    //_currency_manager = (CurrencyManager)this.BindingContext[_datatable];
                    //_currency_manager.Position = 0;
                    Parent = _dgvGantt
                    };

                _dgvGantt.Controls.Add(txt);
                //pairs string collection with dictionared values

                var acsc = new AutoCompleteStringCollection();

                foreach (var kvp in GetFiltersDictionary())
                    {
                    if (_aim[i] == kvp.Value) acsc.AddRange(new[] { kvp.Key });
                    }
                
                txt.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                txt.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txt.AutoCompleteCustomSource = acsc; //bind
                
                var headerRect = _dgvGantt.GetColumnDisplayRectangle(i, true);
                txt.Location = new Point(headerRect.Location.X + 1, 40 - txt.Height - 1);
                txt.Size = new Size(headerRect.Width - 3, _dgvGantt.ColumnHeadersHeight);
                
                //perform filtering

                txt.TextChanged += delegate
                    {
                        if (txt.Text != string.Empty)
                            {
                            _text_filtere_Mode = true;
                            }
                        else
                            {
                            _text_filtere_Mode = false;                            
                            }

                        //apply filtering
                        _bsGantt.Filter = string.Format("CONVERT(" + _dgvGantt.Columns[txt.Name]?.DataPropertyName +
                                                            ", System.String) like '%" + txt.Text.Replace("'", "''") +
                                                            "%'");
                        Filter.SetFilteredKey(txt.Text);
                        _dgvGantt.DataSource = _bsGantt;
                        _dgvGantt.Refresh();

                        DisplayData();
                        };
                }

            _filtersCreated = true;
            }
        
        private void ClearFilters()
            {
            for (var i = 0; i <= _dgvGantt.Controls.Count -1; i++)
                {
                var c = _dgvGantt.Controls[i];

                if (c is TextBox txt) txt.ResetText();
                }
            }
        #endregion

        #region SideBar 

        private Panel _pnSideBar;
        private DateTimePicker _dtpDateTo;
        private DateTimePicker _dtpDateFrom;
        private Button _btnColapseSideBar;
        private Button _btnReload;
        private Button _btnFilterSelection;
        private Label label2;
        private Label label1;
        private int _collapseState = -1;
        
        private void _btnFullPrev_Click(object sender, EventArgs e)
            {
            _btnTess.BackColor = Color.WhiteSmoke;
            BtnConf.BackColor = Color.WhiteSmoke;
            _btnStiro.BackColor = Color.WhiteSmoke;

            if (!Filter.GetAllChannels())
                {
                Filter.SetFilteredChannel(_channels.ListOfChannels().First().Channel); ;   //starts full mode from tess
                Filter.SetAllChannels(true);
                
                btnFullPrev.Image = Properties.Resources.full_fill_40;
                _sql_rdd_filter = "[Data Dvc]";

                _btnTess.BackColor = Color.Gainsboro;
                BtnConf.BackColor = Color.Gainsboro;
                _btnStiro.BackColor = Color.Gainsboro;
                }
            else
                {
                Filter.SetAllChannels(false);
                Filter.SetFilteredChannel(_tmp_channel);  //cursor back to channel 
                btnFullPrev.Image = Properties.Resources.full_40;

                if (_tmp_channel == "TESSITURA")
                    _btnTess.BackColor = Color.Gainsboro;
                else if (_tmp_channel == "CONFEZIONE")
                    BtnConf.BackColor = Color.Gainsboro;
                else
                    _btnStiro.BackColor = Color.Gainsboro;
                }

            _sql_channel = "[Data Consegna Tessitura]";
            
            LoadTableFromDba();
            DisplayData();
            }

        private void btnReload_Click_1(object sender, EventArgs e)
            {
            if (_config.EndDate < _config.StartDate)    //invalid date interval
                {
                MessageBox.Show("Invalid date selection.");     
                return;
                }

            if (_dtpDateTo.Value.Subtract(_dtpDateFrom.Value).TotalDays > 92) //three months maximum
                {
                MessageBox.Show("Choose maximum 3 months or 92 days.");
                return;
                }

            _gantt.AllowRowSelection = false;
            ResetConfiguration();

            LoadTableFromDba();
            DisplayData();
            }

        private void ResetConfiguration()
            {
            ClearFilters();

            Filter.SetFilteredKey(null);
            Filter.GetFilteredKeys().Clear();
            _text_filtere_Mode = false;
            _filter_multiply = false;
            _bsRoot.Filter = null;
            _bsGantt.Filter = null;
            _btnFilterSelection.Image = Properties.Resources.filter_40;
            btnFullPrev.Image = Properties.Resources.full_40;
    
            //_gantt._scrollPosition = 0;
            
            _sql_mode = @"normal";
            _sql_rdd_filter = "[Rdd " + Filter.GetFilteredChannel() + "]";

            //set new date configuration
            _config.StartDate = _dtpDateFrom.Value;
            _config.EndDate = _dtpDateTo.Value;

            if (Filter.GetAllChannels())
                {
                Filter.SetFilteredChannel(_tmp_channel);  //cursor back to channel
                Filter.SetAllChannels(false);
                }
            
            _tmp_channel = Filter.GetFilteredChannel();

            _btnTess.BackColor = Color.WhiteSmoke;
            BtnConf.BackColor = Color.WhiteSmoke;
            _btnStiro.BackColor = Color.WhiteSmoke;

            if (_tmp_channel == "TESSITURA")
                _btnTess.BackColor = Color.Gainsboro;
            else if (_tmp_channel == "CONFEZIONE")
                BtnConf.BackColor = Color.Gainsboro;
            else
                _btnStiro.BackColor = Color.Gainsboro;
            }

        private bool _filter_multiply = false;
        private void btnFilterSelection_Click(object sender, EventArgs eventArgs)
            {
            if (!_filter_multiply)
                {
                _filter_multiply = true;
                _gantt.AllowRowSelection = false;
                _btnFilterSelection.Image = Properties.Resources.filter_edit_40;
                }
            else
                {
                _filter_multiply = false;
                _gantt.AllowRowSelection = true;
                Filter.GetFilteredKeys().Clear();
                _btnFilterSelection.Image = Properties.Resources.filter_40;
                }
            _gantt.ScrollPosition = 0;

            DisplayData();
            }

        private void btn_Tess_Click(object sender, EventArgs e)
            {
            Filter.SetAllChannels(false);
            btnFullPrev.Image = Properties.Resources.full_40;
            Filter.SetFilteredChannel("TESSITURA");
            _tmp_channel = Filter.GetFilteredChannel();
            _sql_channel = "[Data Consegna Tessitura]";
            _sql_rdd_filter = "[Rdd Tessitura]";
            _gantt.ScrollPosition = 0;

            _btnTess.BackColor = Color.Gainsboro;
            BtnConf.BackColor = Color.WhiteSmoke;
            _btnStiro.BackColor = Color.WhiteSmoke;

            //set new date configuration
            _config.StartDate = _dtpDateFrom.Value;
            _config.EndDate = _dtpDateTo.Value;

            LoadTableFromDba();
            DisplayData();
            }

        private void btn_Conf_Click(object sender, EventArgs e)
            {
            Filter.SetAllChannels(false);
            btnFullPrev.Image = Properties.Resources.full_40;
            Filter.SetFilteredChannel("CONFEZIONE");
            _tmp_channel = Filter.GetFilteredChannel();
            _sql_channel = "[Data Cons Confezione]";
            _sql_rdd_filter = "[Rdd Confezione]";
            _gantt.ScrollPosition = 0;

            _btnTess.BackColor = Color.WhiteSmoke;
            BtnConf.BackColor = Color.Gainsboro;
            _btnStiro.BackColor = Color.WhiteSmoke;

            //set new date configuration
            _config.StartDate = _dtpDateFrom.Value;
            _config.EndDate = _dtpDateTo.Value;

            LoadTableFromDba();
            DisplayData();
            }

        private void btn_Stiro_Click(object sender, EventArgs e)
            {
            Filter.SetAllChannels(false);
            Filter.SetFilteredChannel("STIRO");
            _tmp_channel = Filter.GetFilteredChannel();
            _sql_channel = "[Data Cons Stiro]";
            _sql_rdd_filter = "[Rdd Stiro]";
            btnFullPrev.Image = Properties.Resources.full_40;
            _gantt.ScrollPosition = 0;

            _btnTess.BackColor = Color.WhiteSmoke;
            BtnConf.BackColor = Color.WhiteSmoke;
            _btnStiro.BackColor = Color.Gainsboro;

            //set new date configuration
            _config.StartDate = _dtpDateFrom.Value;
            _config.EndDate = _dtpDateTo.Value;

            LoadTableFromDba();
            DisplayData();
            }

        private void btnColapseSideBar_Click_1(object sender, EventArgs e)
            {
            if (_collapseState == -1)
                {
                _pnSideBar.Width = 69;
                _collapseState = 0;
                }
            else if (_collapseState == 0)
                {
                _pnSideBar.Width = 176;
                _collapseState = -1;
                }
            }

        #endregion

        #region Printing

        //datagridview variables
        private StringFormat strFormat;
        private System.Collections.ArrayList arrColumnLefts = new System.Collections.ArrayList();
        private System.Collections.ArrayList arrColumnWidths = new System.Collections.ArrayList();
        private int iCellHeight = 0;
        private int iTotalWidth = 0;
        private int iRow = 0;
        private bool bFirstPage = false;
        private bool bNewPage = false;
        private int iHeaderHeight = 0;

        private void Document_StartPrint(object sender, PrintEventArgs e)
            {
            strFormat = new StringFormat
                {
                Alignment = StringAlignment.Near,
                LineAlignment = StringAlignment.Center,
                Trimming = StringTrimming.EllipsisCharacter
                };

            arrColumnLefts.Clear();
            arrColumnWidths.Clear();
            iCellHeight = 0;
            //iCount = 0;
            bFirstPage = true;
            bNewPage = true;

            // Calculating Total Widths
            iTotalWidth = 0;
            foreach (DataGridViewColumn dgvGridCol in _dgvGantt.Columns)
                {
                if (dgvGridCol.Visible != false)
                    {
                    iTotalWidth += dgvGridCol.Width;
                    }
                }
            }

        private void Document_Print(object sender, PrintPageEventArgs e)
            {
            // Set up string.
            string title = string.Empty;
            if (_colapsed == 1)
                {
                title = "Document: " + Filter.GetFilteredChannel() + " table preview" + Environment.NewLine + "Selected interval: " + _config.StartDate.ToShortDateString() + " - " + _config.EndDate.ToShortDateString() + Environment.NewLine + "ID " + DateTime.Now.ToString("ssffffff");
                }
            else
                {
                title = "Document: " + Filter.GetFilteredChannel() + " gantt preview" + Environment.NewLine + "Selected interval: " + _config.StartDate.ToShortDateString() + " - " + _config.EndDate.ToShortDateString() + Environment.NewLine + "ID " + DateTime.Now.ToString("ssffffff");
                }

            string printDate = _mpd.PrintDate.ToString(System.Globalization.CultureInfo.CurrentUICulture);
            string dedication = _mpd.Dedication = "";  //modified
            string notify = _mpd.Notification;

            Font stringFont = new Font("Times New Roman", 9);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(title);
            sb.AppendLine("Print date: " + printDate + " " + DateTime.Now.ToString("HH:mm", System.Globalization.CultureInfo.CurrentCulture));
            sb.AppendLine(new string('_', title.Length - 1));
            sb.AppendLine("Note: " + notify);
            var strSb = sb.ToString();

            var measureString = strSb;
            // Measure string.
            SizeF stringSize = new SizeF();
            stringSize = e.Graphics.MeasureString(measureString, stringFont);

            var pen = new Pen(Brushes.Gainsboro, 1);
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;

            //measure main title string to define start point
            var measureHeaderTitle = e.Graphics.MeasureString(HeaderTitle, new Font("Tahoma", 20, FontStyle.Bold));
            var headerTitleW = (int)measureHeaderTitle.Width;
            var headerX = e.PageBounds.Width - headerTitleW - 10;
            var headerY = (int)stringSize.Height / 2 - 10;

            if (_colapsed == 1)
                {
                int iLeftMargin = 2;
                int iTopMargin = (int)stringSize.Height + 20;
                bool bMorePagesToPrint = false;
                int iTmpWidth = 0;

                if (bFirstPage)
                    {
                    foreach (DataGridViewColumn GridCol in _dgvGantt.Columns)
                        {
                        if (GridCol.Visible != false)
                            {
                            iTmpWidth = (int)(Math.Floor((double)((double)GridCol.Width /
                              (double)iTotalWidth * (double)iTotalWidth *
                              ((double)e.PageBounds.Width / (double)iTotalWidth))));

                            iHeaderHeight = (int)(e.Graphics.MeasureString(GridCol.HeaderText,
                                GridCol.InheritedStyle.Font, iTmpWidth).Height) + 11;

                            arrColumnLefts.Add(iLeftMargin);
                            arrColumnWidths.Add(iTmpWidth);
                            iLeftMargin += iTmpWidth;
                            }
                        }
                    }

                while (iRow <= _dgvGantt.Rows.Count - 1)
                    {
                    DataGridViewRow GridRow = _dgvGantt.Rows[iRow];
                    iCellHeight = GridRow.Height + 10;

                    int iCount = 0;
                    if (iTopMargin + iCellHeight >= e.MarginBounds.Height + e.MarginBounds.Top)
                        {
                        bNewPage = true;
                        bFirstPage = false;
                        bMorePagesToPrint = true;
                        break;
                        }
                    else
                        {
                        if (bNewPage)
                            {
                            // Draw rectangle representing size of string.
                            e.Graphics.FillRectangle(Brushes.Gainsboro, new Rectangle(0, 0, e.PageBounds.Width, (int)stringSize.Height + 10));
                            e.Graphics.DrawString(strSb, new Font("Times New Roman", 8, FontStyle.Bold), Brushes.Black, new PointF(10, 10));
                            e.Graphics.DrawRectangle(new Pen(Color.Silver, 3), 0.0F, 0.0F, e.PageBounds.Width, stringSize.Height + 10);
                            e.Graphics.DrawString(HeaderTitle, new Font("Tahoma", 20, FontStyle.Bold), Brushes.Orange, headerX, headerY);

                            iTopMargin = (int)stringSize.Height + 20;

                            //draw column headers
                            foreach (DataGridViewColumn GridCol in _dgvGantt.Columns)
                                {
                                if (GridCol.Visible != false)
                                    {
                                    e.Graphics.FillRectangle(new SolidBrush(Color.Gainsboro),
                                           new Rectangle((int)arrColumnLefts[iCount], iTopMargin, (int)arrColumnWidths[iCount], iHeaderHeight));
                                    e.Graphics.DrawRectangle(pen,
                                        new Rectangle((int)arrColumnLefts[iCount], iTopMargin,
                                        (int)arrColumnWidths[iCount], iHeaderHeight));
                                    e.Graphics.DrawString(GridCol.HeaderText,
                                        new Font("Times New Roman", 6, FontStyle.Bold),
                                        new SolidBrush(GridCol.InheritedStyle.ForeColor),
                                        new RectangleF((int)arrColumnLefts[iCount], iTopMargin,
                                        (int)arrColumnWidths[iCount], iHeaderHeight), strFormat);

                                    iCount++;
                                    }
                                }
                            bNewPage = false;
                            iTopMargin += iHeaderHeight;
                            }
                        iCount = 0;

                        //draw records
                        foreach (DataGridViewCell Cel in GridRow.Cells)
                            {
                            if (Cel.Visible != false)
                                {
                                if (Cel.Value != null)
                                    {
                                    e.Graphics.FillRectangle(Brushes.White,
                                        new Rectangle((int)arrColumnLefts[iCount], iTopMargin,
                                        (int)arrColumnWidths[iCount], iCellHeight));

                                    e.Graphics.DrawString(Cel.Value.ToString(),
                                        new Font("Times New Roman", 7),
                                        new SolidBrush(Cel.InheritedStyle.ForeColor),
                                        new RectangleF((int)arrColumnLefts[iCount],
                                        (float)iTopMargin,
                                        (int)arrColumnWidths[iCount], (float)iCellHeight),
                                        strFormat);
                                    }
                                e.Graphics.DrawRectangle(pen,
                                    new Rectangle((int)arrColumnLefts[iCount], iTopMargin,
                                    (int)arrColumnWidths[iCount], iCellHeight));

                                iCount++;
                                }
                            }
                        }
                    iRow++;
                    iTopMargin += iCellHeight;
                    }

                if (bMorePagesToPrint)
                    e.HasMorePages = true;
                else
                    e.HasMorePages = false;
                }
            else if (_colapsed == 2)
                {
                // Draw rectangle representing size of string.
                e.Graphics.FillRectangle(Brushes.Gainsboro, new Rectangle(0, 0, e.PageBounds.Width, (int)stringSize.Height + 10));
                e.Graphics.DrawString(strSb, new Font("Microsoft Sans Serif", 8, FontStyle.Bold), Brushes.Black, new PointF(10, 10));
                e.Graphics.DrawRectangle(new Pen(Color.Silver, 3), 0.0F, 0.0F, e.PageBounds.Width, stringSize.Height + 10);
                e.Graphics.DrawString(HeaderTitle, new Font("Tahoma", 20, FontStyle.Bold), Brushes.Orange, headerX, headerY);

                Image img = Image.FromFile(_tmpImageFilePath);

                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                e.Graphics.DrawImage(img, new Rectangle(0, (int)stringSize.Height + 30, img.Width, img.Height));
                e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                }
            }
        #endregion

        #region Files

        private string[] _dropped = { };
        private List<string> _files = new List<string>();

        protected override void OnDragEnter(DragEventArgs drgevent)
            {
            //get files array
            _dropped = drgevent.Data.GetData(DataFormats.FileDrop) as string[];
            _files = _dropped.ToList();

            foreach (var file in _files)
                {
                //accept only csv files
                if (file.ToLower().EndsWith(".csv"))
                    drgevent.Effect = DragDropEffects.Copy;
                else
                    drgevent.Effect = DragDropEffects.None;
                }

            base.OnDragEnter(drgevent);
            }

        protected override void OnDragDrop(DragEventArgs drgevent)
            {
            foreach (var file in _files)
                {
                Console.WriteLine("Uploaded: " + file + " " + DateTime.Now);

                DisplayData();
                }

            base.OnDragDrop(drgevent);
            }

        private void btnReport_Click(object sender, EventArgs e)
            {
            var f = new SummaryReport();
            f.Show();
            }

        #endregion

        private void btnCleanse_Click(object sender, EventArgs eventArgs)
            {
            if (!_gantt.HideDelay)
                {
                _gantt.HideDelay = true;
                btnCleanse.Image = Properties.Resources.cleanse_selected;
                btnCleanse.ForeColor = Color.Gold;
                }
            else
                {
                _gantt.HideDelay = false;
                btnCleanse.Image = Properties.Resources.cleanse;
                btnCleanse.ForeColor = Color.White;
                }

            DisplayData();
            }

        private void Gantt_Click(object sender, EventArgs e)
            {
            if (_gantt.MouseOverRowValue == null) return;
            
            Bar val = (Bar)_gantt.MouseOverRowValue;

            HoldFilterMultiSelection();

            if (val.RowText == _keyPressed)
                {
                _gantt.AllowRowSelection = false;        
                }           
          
            foreach (DataGridViewRow row in _dgvGantt.Rows)
                {
                if (row.Cells[0].Value.ToString() != val.RowText) continue;

                row.Selected = true;
                _dgvGantt.FirstDisplayedScrollingRowIndex = row.Index;
                row.DefaultCellStyle.SelectionBackColor = Color.SteelBlue;
                row.DefaultCellStyle.SelectionForeColor = Color.White;
                }
            }

        private void Gantt_DoubleClick(object sender ,EventArgs eventArgs)
            {
            var popUp = new Popup(this);
            var strInfo = new StringBuilder();

            if (_gantt.IsMouseOverRowText == false && _gantt.MouseOverRowValue != null)
                {
                Bar val = (Bar)_gantt.MouseOverRowValue;

                var rows = from DataRow dRow in _datatable.Rows
                          where (string)dRow["Commessa"] == val.RowText.ToString()
                          select dRow;

                popUp.Order = val.RowText.ToString();

                foreach (var row in rows)
                    {
                    //global
                    popUp.Art = row[1].ToString();
                    popUp.Qty = row[3].ToString();
                    popUp.Stag = row[4].ToString();
                    popUp.Fin = row[2].ToString();
                    popUp.Dvc = IntersectDate(row[48].ToString());
                    popUp.Diff = row[6].ToString();

                    int.TryParse(row[15].ToString(), out int daysT);
                    int.TryParse(row[25].ToString(), out int daysC);
                    int.TryParse(row[39].ToString(), out int daysS);

                    var totalDays = daysT + daysC + daysS;

                    //filtered
                    if (!Filter.GetAllChannels())
                        {
                        if (Filter.GetFilteredChannel() == "TESSITURA")
                            {
                            popUp.TotDays = row[15].ToString();
                            popUp.IniT = IntersectDate(row[9].ToString());
                            popUp.ConT = IntersectDate(row[13].ToString());
                            popUp.RddT = IntersectDate(row[16].ToString());
                            popUp.gnT = daysT.ToString();
                            }
                        else if (Filter.GetFilteredChannel() == "CONFEZIONE")
                            {
                            popUp.TotDays = row[25].ToString();
                            popUp.IniC = IntersectDate(row[19].ToString());
                            popUp.ConC = IntersectDate(row[23].ToString());
                            popUp.RddC = IntersectDate(row[26].ToString());
                            popUp.gnC = daysC.ToString();
                            }
                        else if (Filter.GetFilteredChannel() == "STIRO")
                            {
                            popUp.TotDays = row[39].ToString();
                            popUp.IniS = IntersectDate(row[33].ToString());
                            popUp.ConS = IntersectDate(row[37].ToString());
                            popUp.RddS = IntersectDate(row[40].ToString());
                            popUp.gnS = daysS.ToString();
                            }
                        }
                   else
                        {
                        popUp.TotDays = totalDays.ToString();
                        popUp.IniT = IntersectDate(row[9].ToString());
                        popUp.ConT = IntersectDate(row[13].ToString());
                        popUp.RddT = IntersectDate(row[16].ToString());
                        popUp.IniC = IntersectDate(row[19].ToString());
                        popUp.ConC = IntersectDate(row[23].ToString());
                        popUp.RddC = IntersectDate(row[26].ToString());
                        popUp.IniS = IntersectDate(row[33].ToString());
                        popUp.ConS = IntersectDate(row[37].ToString());
                        popUp.RddS = IntersectDate(row[40].ToString());
                        popUp.gnT = daysT.ToString();
                        popUp.gnC = daysC.ToString();
                        popUp.gnS = daysS.ToString();
                        }
                    }
               
                var frmTransp = new Form();
                frmTransp.Size = this.ClientRectangle.Size;
                frmTransp.BackColor = Color.Black;
                frmTransp.Opacity = 0.4;
                frmTransp.FormBorderStyle = FormBorderStyle.None;
                frmTransp.WindowState = FormWindowState.Maximized;
                frmTransp.Show();
                frmTransp.ShowIcon = false;
                frmTransp.ShowInTaskbar = false;

                popUp.StartPosition = FormStartPosition.CenterScreen;
                popUp.ShowInTaskbar = false;
                popUp.Show();

                popUp.FormClosed += (ss, ee) => frmTransp.Close();               

                frmTransp.Click += (s,e) =>
                    {
                        popUp.Close();
                        frmTransp.Close();
                        };
                }
            }

        private string IntersectDate(object date)
            {
            DateTime.TryParse(date.ToString(), out DateTime dt);
            if (dt == DateTime.MinValue)
                return "-";
            else return dt.ToShortDateString();
            }

        System.Threading.Timer _filterTimer = null;
        private string _keyPressed;
        private int _rowIndex;
        private void dgvGannt_CellDoubleClick(object sender, DataGridViewCellEventArgs eventArgs)
            {
            _gantt.AllowRowSelection = true;
            _rowIndex = eventArgs.RowIndex;
            _keyPressed = _dgvGantt.Rows[eventArgs.RowIndex].Cells[0].Value.ToString();

            if (_gantt.AllowRowSelection)
                {
                    _gantt.ScrollPosition = _dgvGantt.FirstDisplayedScrollingRowIndex;
                _gantt.Refresh();
                }

            if (!_text_filtere_Mode) return;

            _dgvGantt.Rows[_rowIndex].DefaultCellStyle.SelectionBackColor = Color.Red;
            _dgvGantt.Refresh();

            TimerCallback tcb = new TimerCallback(TcbState);
            Thread.BeginThreadAffinity();
            _filterTimer = new System.Threading.Timer(tcb, null, 0, 1500);
            
            Thread.EndThreadAffinity();
            Thread.Sleep(100);
            }

        private void dgvGant_CellClick(object sender, DataGridViewCellEventArgs e)
            {
            if (!_filter_multiply) return;

            if (e.RowIndex < 0) return;

            if (_dgvGantt.SelectedRows.Count == 0) return;

            var f = Filter.GetFilteredKey();
            if (f != string.Empty) Filter.SetFilteredKey(string.Empty);

            if (!Filter.GetFilteredKeys().Contains(_dgvGantt.Rows[e.RowIndex].Cells[0].Value.ToString()))
                {
                Filter.GetFilteredKeys().Add(_dgvGantt.Rows[e.RowIndex].Cells[0].Value.ToString());

                _dgvGantt.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = Color.Gold;
                _dgvGantt.Rows[e.RowIndex].DefaultCellStyle.SelectionForeColor = Color.Black;
                }
            else
                {
                Filter.GetFilteredKeys().Remove(_dgvGantt.Rows[e.RowIndex].Cells[0].Value.ToString());
                _dgvGantt.Rows[e.RowIndex].DefaultCellStyle.Font = default(Font);
                }

            _dgvGantt.Rows[e.RowIndex].Selected = !_dgvGantt.Rows[e.RowIndex].Selected;

            DisplayData();
            }

        private void HoldFilterMultiSelection()
            {
            foreach (var str in Filter.GetFilteredKeys())
                {
                foreach (DataGridViewRow row in _dgvGantt.Rows)
                    {
                    if (row.Cells[0].Value.ToString() != str) continue;

                    row.Selected = true;

                    row.DefaultCellStyle.SelectionBackColor = Color.Gold;
                    row.DefaultCellStyle.SelectionForeColor = Color.Black;
                    }
                }
            }

        public void TcbState(object info)
            {
            Filter.SetFilteredKey(_keyPressed);
            DisplayData();

            _dgvGantt.Rows[_rowIndex].DefaultCellStyle.SelectionBackColor = Color.SteelBlue;
            _dgvGantt.Refresh();
            _filterTimer.Dispose();
            }
        }
    }