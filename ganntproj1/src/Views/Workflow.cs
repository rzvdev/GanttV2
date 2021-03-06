namespace ganntproj1
{
    using ganntproj1.Models;
    using ganntproj1.src.Views;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Linq;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;

    public partial class Workflow : Form
    {
        private readonly Ganttchart _gChart = new Ganttchart();

        private List<Bar> _objList = new List<Bar>();

        private List<Index> _clonedIndexes = new List<Index>();
        public  Button reset =  new Button();

        private readonly List<string> _lstExpanded = new List<string>();

        private ContextMenuStrip _ctxMenuStrip = new ContextMenuStrip();

        private bool _ctxActive = false;

        private bool _clActive = false;

        private readonly Config _config = new Config();

        public static List<string> ListOfLinesSelected { get; set; }

        private LoadingJob _loadingJobForm;
        public LoadingJob LoadingJobForm { get => _loadingJobForm; set => _loadingJobForm = value; }

        public int TaskId { get; set; }

        private bool IsTreeState { get; set; }

        private bool HoldCollapsedNodes { get; set; }

        public static bool ByQty { get; set; }

        public static string TargetOrder { get; set; }

        public static string TargetLine { get; set; }
        public static string TargetDepartment { get; set; }
        public static DateTime TargetProgramDate { get; set; }

        public static DateTime TargetModelStartDate { get; set; }

        public static bool TargetLaunched { get; set; }

        public static Color TargetModelColor { get; set; }

        public static int Members { get; set; }

        public static bool ByManualDate { get; set; }

        public static DateTime ManualDateTime { get; set; }

        public static bool TargetClosedByUser { get; set; }

        public static bool TargetLocked { get; set; }

        public static bool TargetLockedProd { get; set; }

        public static double TargetQtyH { get; set; }

        public static string Article { get; set; }

        public static List<string> ListOfRemovedOrders { get; set; }

        private string DefaultAim { get; set; }

        private string DefaultDept { get; set; }

        public static bool IsDelayHidden { get; set; }

        public static bool SkipLines = false;
        private List<Button> BtnGroupLst = new List<Button>();
        public Workflow()
        {
            InitializeComponent();

            _gChart.Dock = DockStyle.Fill;
            spContainer.Panel1.Controls.Add(_gChart);
            _gChart.BringToFront();
            _gChart.DoubleBuffered(true);
            _gChart.MouseMove += _gChart.Chart_MouseMove;
            _gChart.DoubleClick += Gantt_DoubleClick;
            _gChart.MouseMove += Gantt_MouseMove;

            _config.Set_sql_conn(new SqlConnection
                (Config.GetOlyConn().Connection.ConnectionString));
            spContainer.Panel2Collapsed = true;

            FormClosing += (s, e) =>
            {
                _gChart?.Dispose();
                _tmTip?.Dispose();
                GC.Collect();
            };
            reset.Click += Reset_Click;
            var tlp = new ToolTip();
            tlp.SetToolTip(btnBack, "Back");
            tlp.SetToolTip(btnMegaBack, "Back one week");
            tlp.SetToolTip(btnFow, "Forward");
            tlp.SetToolTip(btnMegaFow, "Forward one week");
            tlp.SetToolTip(btnZoomIn, "Zoom in");
            tlp.SetToolTip(btnZoomOut, "Zoom out");
            tlp.SetToolTip(btnZoomOut, "Zoom out");
            tlp.SetToolTip(btnDayInfo, "Hide completed tasks");
            tlp.SetToolTip(btnHideDelay, "Hide delay bars");
            tlp.SetToolTip(btnZoomOut, "Zoom out");
            tlp.SetToolTip(button1, "Lines filter");
            tlp.SetToolTip(btnHolidays, "Holidays calendar");
            tlp.SetToolTip(btnShowEff, "Show production efficiency");
            tlp.SetToolTip(button2, "Hide lines where are no active orders");
        }


        private void Reset_Click(object sender, EventArgs e)
        {
            AddTimelineObjects();
        }

        private void ProduzioneGantt_Load(object sender, EventArgs e)
        {
            if (Store.Default.sectorId == 7)
            {
                panel1.Height = 68;
                lbl_separator1.Height = 58;
                lbl_separator2.Height = 58;
                BtnGroupLst = new List<Button>() { grp1, grp2, grp3, grp4, grp5 };
                Central.GroupIndex = 1;
            }
            else
            {
                panel1.Height = 46;
                lbl_separator1.Height = 34;
                lbl_separator2.Height = 34;
            }
            cbArticle.Checked = Central.IsArticleSelection;

            ByQty = false;
            PopulateFilters();
            LoadDataWithDateChange();
            AddGanttContextMenu();
            _gChart.FromDate = JobModel.DateFromLast;
            _gChart.ToDate = JobModel.DateToLast;
            TargetOrder = string.Empty;
            FormClosed += (s, ev) =>
            {
                JobModel.DateFromLast = _gChart.FromDate;
                JobModel.DateToLast = _gChart.ToDate;
            };

            IsDelayHidden = false;
            _gChart.FilteredRowText = string.Empty;
            _gChart.FilteredRowAtt = string.Empty;


            

        }
    
        public void LoadDataWithDateChange()
        {
            _gChart.FromDate = Central.DateFrom;
            _gChart.ToDate = Central.DateTo;
            SkipLines = false;
            _gChart.BarHeight = 40;
            ListOfLinesSelected = new List<string>();
            
            if (Store.Default.sectorId == 7)
            {
                //Central.TessituraTaskList = Central.TaskList;
                Central.GroupIndex = 1;
                AddTimelineObjects();
                panel1.Height = 68;
                lbl_separator1.Height = 58;
                lbl_separator2.Height = 58;
            }
            else
            {
                AddTimelineObjects();
                panel1.Height = 46;
                lbl_separator1.Height = 34;
                lbl_separator2.Height = 34;
            }
        }

        private List<Index> _indexerList = new List<Index>();

        private void GenerateBarsIndexer()
        {
            _indexerList = new List<Index>();
            DefaultAim = Central.TaskList.First().Aim;
            DefaultDept = Central.TaskList.First().Department;
            
            var rowIdx = 0;
            var elementIdx = 0;
            List<JobModel> tasklist = new List<JobModel>();
            if(Store.Default.sectorId==7)
            {
                 var dbline = (from lines in Models.Tables.Lines where lines.Department == "Tessitura" select lines).OrderBy(a => a.Groupby.Length).ThenBy(a => a.Line).Select(a=>a.Line).ToList();
                //string test = string.Empty;
                //foreach(var x in dbline)
                //{
                //    test += x + ";";
                //}
                //MessageBox.Show(test);
                 tasklist = Central.TessituraTaskList.OrderBy(a=> dbline.IndexOf(a.Aim)).ThenBy(a=>a.Aim.Length).ThenBy(a => a.FlowEnd).ToList();
               DefaultAim = tasklist.First().Aim;
               DefaultDept = tasklist.First().Department;
            }
            else
            {
                 tasklist = Central.TaskList.OrderBy(a => a.Aim.Length).ThenBy(a => a.Aim).ThenBy(a => a.FlowEnd).ToList();
            }
            
            foreach (var model in tasklist)
            {
                if (SkipLines && !ListOfLinesSelected.Contains(model.Aim)) continue;

                if (model.Aim == DefaultAim && model.Department == DefaultDept)
                {
                    _indexerList.Add(new Index(rowIdx, elementIdx, model.Name, model.Aim, model.Department, model.Idx));
                    elementIdx++;
                }
                else
                {
                    elementIdx = 0;
                    rowIdx++;
                    //if (elementIdx == 0) rowIdx = 0;
                    _indexerList.Add(new Index(rowIdx, elementIdx, model.Name, model.Aim, model.Department, model.Idx));
                    elementIdx++;
                }
                DefaultAim = model.Aim;
                DefaultDept = model.Department;
            }
        }

        public void SetPrincipalBarIndex(bool effColor)
        {
            _gChart.Bars = new List<BarProperty>();
            _gChart.HeaderList = new List<Header>();
            _objList = new List<Bar>();

            var jobModel = new JobModel();
            var c = new Central();
            c.GetBase();
            c.GetProductionColor();
            
            if (!Central.TaskList.Any()) return;

            GenerateBarsIndexer();

            var timeToMoveForward = 0L;
            var timeToMoveBack = 0L;
            //var objLockIndex = 0;

            foreach (var item in _indexerList)
            {
                timeToMoveForward = 0L;
                timeToMoveBack = 0L;
                var model = Central.TaskList.FirstOrDefault(
                    x => x.Name == item.ObjText && x.Aim == item.ObjAim && x.Department == item.ObjDept && x.Idx == item.Idx);
                if (model == null)
                    return;
                var modelBefore = JobModel.GetModelIndex(model.Name, _indexerList, -1);
               
                if (modelBefore != null)
                {
                    var delayTicks = 0L;
                    if (modelBefore.DelayEndDate == DateTime.MinValue)
                    {
                        delayTicks = 0L;

                    }
                    else
                    {
                        delayTicks = modelBefore.DelayEndDate.Subtract(modelBefore.DelayStartDate).Ticks;
                    }
                    
                    var beforeFullEndTime = modelBefore.EndDate.AddTicks(delayTicks);
                   
                     
                    if (!model.ManualDate)
                    {
                        timeToMoveForward = beforeFullEndTime.Subtract(model.StartDate).Ticks;
                        timeToMoveBack = model.StartDate.Subtract(beforeFullEndTime).Ticks;
                        if (timeToMoveForward < 0L) timeToMoveForward = 0L;
                        if (timeToMoveBack < 0L) timeToMoveBack = 0L;
                    }
                    else
                    {
                        if (beforeFullEndTime < model.StartDate)
                        {
                            beforeFullEndTime = model.StartDate;
                        }
                        // Do this just in case when delay goes over manually programmed order
                        //if ((model.StartDate-DateTime.Now.Date))
                        //{

                        timeToMoveForward = beforeFullEndTime.Subtract(model.StartDate).Ticks;
                        //}
                        //timeToMoveBack = 0L;
                    }
                }

                var isClosed = model.ClosedByUser;
                var jobEnd = model.EndDate.AddTicks(+timeToMoveForward).AddTicks(-timeToMoveBack);
                var h = JobModel.SkipDateRange(model.StartDate.AddTicks(+timeToMoveForward).AddTicks(-timeToMoveBack), jobEnd.AddMinutes(-1), model.Aim, model.Department);
                jobEnd = jobEnd.AddDays(+h);
                var prodEnd = new DateTime(model.ProductionEndDate.Year, model.ProductionEndDate.Month, model.ProductionEndDate.Day, model.ProductionEndDate.Hour, model.ProductionEndDate.Minute, model.ProductionEndDate.Second);
                var delayTs = TimeSpan.FromTicks(model.DelayTime);
                var prodQty = model.ProdQty;
                var spCarico = model.LoadedQty;
                var delEnd = jobEnd.AddDays(delayTs.Days).AddHours(+delayTs.Hours);
                var delayStart = new DateTime(jobEnd.Year, jobEnd.Month, jobEnd.Day, jobEnd.Hour, jobEnd.Minute, jobEnd.Second);
                var delayEnd = delEnd;

                if (prodQty > 0)
                {
                    if (isClosed && prodEnd.Date <= jobEnd.Date)
                    {
                        jobEnd = prodEnd;
                        delayTs = new TimeSpan(0, 0, 0, 0, 0);
                        delayStart = DateTime.MinValue;
                        delayEnd = DateTime.MinValue;
                    }
                    else if (isClosed && prodEnd.Date > jobEnd.Date)
                    {
                        var d = prodEnd.Subtract(jobEnd).Days;

                        delayTs = new TimeSpan(0, 0, 0, 0, 0);
                        delayTs = new TimeSpan(
                           Convert.ToInt32(d), prodEnd.Hour, prodEnd.Minute, prodEnd.Second, prodEnd.Millisecond);
                        delayEnd = new DateTime(prodEnd.Year, prodEnd.Month, prodEnd.Day,
                         prodEnd.Hour, prodEnd.Minute, prodEnd.Second, prodEnd.Millisecond);
                    }
                    else if (!isClosed && prodQty.Equals(spCarico) && prodEnd <= jobEnd)
                    {
                        jobEnd = prodEnd;
                        delayTs = new TimeSpan(0, 0, 0, 0, 0);
                        delayStart = Config.MinimalDate;
                        delayEnd = Config.MinimalDate;
                    }
                    else if (!isClosed && prodQty < spCarico && jobEnd < prodEnd)
                    {
                        delayStart = jobEnd.AddMinutes(+1);
                        delayEnd = prodEnd;
                    }
                    else if (!isClosed && prodQty < spCarico && delayTs != new TimeSpan(0, 0, 0, 0, 0))
                    {
                        var j = delayEnd;
                        delayStart = jobEnd.AddMinutes(+1);
                        if (j.Hour > 15)
                        {
                            var dh = 7 + (j.Hour - 15);
                            delayTs = new TimeSpan(delayTs.Days + 1, dh, 59, 59, 59);
                            delayEnd = new DateTime(delayEnd.Year, delayEnd.Month, delayEnd.Day, 0, 0, 0, 0);
                            delayEnd = delayEnd.AddDays(+1).AddHours(+dh).AddMinutes(+59).AddSeconds(+59);
                        }
                        else if (j.Hour < 7)
                        {
                            var cs = 7 + j.Hour;
                            delayTs = new TimeSpan(delayTs.Days, cs, 59, 59, 59);
                            delayEnd = new DateTime(delayEnd.Year, delayEnd.Month, delayEnd.Day, 0, 0, 0, 0);
                            delayEnd = delayEnd.AddHours(+delayTs.Hours).AddMinutes(+59).AddSeconds(+59);
                        }
                        var delInc = JobModel.SkipDateRange(delayStart, delayEnd, model.Aim, model.Department);

                        delayTs = new TimeSpan(delayTs.Days + delInc, delayTs.Hours, 0, 0, 0);
                        delayEnd = delayEnd.AddDays(+delInc);
                    }
                }

                var query = (from md in Central.TaskList
                             where md.Name == model.Name && md.Aim == model.Aim && md.Department == model.Department
                             select md)
                             .Update(st =>
                             {
                                 st.StartDate = model.StartDate.AddTicks(+timeToMoveForward).AddTicks(-timeToMoveBack);
                                 st.EndDate = jobEnd;
                                 st.DelayTime = delayTs.Ticks;
                                 st.DelayStartDate = delayStart;
                                 st.DelayEndDate = delayEnd;
                             });

                var barColor = Color.FromArgb(80, 144, 169);
                var barOverColor = Color.FromArgb(27, 98, 124);        
                
                var prodQtyDiff = model.LoadedQty - model.ProdQty;
                double.TryParse(model.WorkingDays.ToString(), out var workDays);

                if (workDays == 0) workDays = 1;
                var eff = (model.ProdQty / (model.DailyProd * workDays)) * 100;

                var prodBarColor = new Color();

                if (prodQtyDiff == 0 || model.ClosedByUser)
                {
                    barColor = Color.Gray;
                    barOverColor = Color.DarkGray;
                    prodBarColor = Color.FromArgb(175, 175, 175);
                }
                else
                {                    
                    if (!effColor)
                    {
                        prodBarColor = Central.HighColor;
                    }
                    else
                    {
                        var production = from prod in Models.Tables.Productions
                                         where prod.Commessa == model.Name
                                         && prod.Line == model.Aim && prod.Department == model.Department
                                         orderby prod.Data
                                         select prod;

                        var prodList = production.ToList();

                        var lastDate = DateTime.MinValue;
                        var daysCount = 0;
                        var totalEff = 0.0;

                        foreach (var prod in prodList)
                        {
                            if (prod.Data.Date != lastDate.Date)
                            {
                                daysCount++;
                            }

                            bool.TryParse(prod.IncludeHours.ToString(), out var hasHours);
                            if (hasHours)
                            {
                                var start = prod.Data;
                                DateTime.TryParse(prod.Times.ToString(), out var end);
                                var hours = end.Subtract(start).Hours;
                                double.TryParse(prod.Capi.ToString(), out var qty);
                                double.TryParse(prod.Dailyqty.ToString(), out var qtyTarget);
                                if (hours == 0) hours = 1;
                                var e1 = qty / hours;
                                var e2 = qtyTarget / 7.5;
                                var efx = (e1 / e2) * 100.0;
                                totalEff += efx;
                            }
                            else
                            {
                                double.TryParse(prod.Capi.ToString(), out var qty);
                                double.TryParse(prod.Dailyqty.ToString(), out var qtyTarget);
                                totalEff += qty / qtyTarget * 100.0;
                            }

                            lastDate = prod.Data;
                        }
                        totalEff /= daysCount;

                        if (totalEff < Central.LowEff)
                        {
                            prodBarColor = Central.LowColor;
                        }
                        else if (totalEff > Central.LowEff && totalEff < Central.MediumEff)
                        {
                            prodBarColor = Central.MediumColor;
                        }
                        else if (totalEff > Central.MediumEff)
                        {
                            prodBarColor = Central.HighColor;
                        }
                    }
                }

                _objList.Add(new Bar(model.Name, model.Name + '_' + model.Aim,
                    model.StartDate,
                    model.EndDate,
                    model.ProductionStartDate,
                    model.ProductionEndDate,
                    model.DelayStartDate,
                    model.DelayEndDate,
                    barColor, barOverColor,
                    item.RowIndex,
                    false, model.Aim, model.LoadedQty, model.DailyProd, model.ProdQty,
                    model.ProductionEndDate,
                    model.ProductionEndDate.AddDays(+model.ProdOverDays),
                    false, false,
                    model.ClosedByUser,
                    prodBarColor,
                    model.Article, model.Department, model.Launched,
                    model.Operation, model.Idx, model.ParentIdx, model.LoadedQty, model.Members, model.QtyH, model.Id, model.Dvc, model.Rdd));
            }
            //if(_objList[0].Department=="Tessitura")
            //{

            //    _objList = new List<Bar>(_objList.GroupBy(a => Central.ListOfLines.IndexOf()));
            //}
            foreach (var obj in _objList)
            {
                if (!Central.IsResetJobLoader)
                {
                    if (obj.FromTime.Date < Central.DateFrom.Date || obj.FromTime.Date > Central.DateTo.Date) continue;
                }

                _gChart.AddBars(obj.RowText, obj.RowText + "_" + obj.Tag, obj,
                    obj.FromTime, obj.ToTime,
                    obj.ProdFromTime, obj.ProdToTime,
                    obj.DelayFromTime, obj.DelayToTime,
                    obj.Color, obj.HoverColor,
                    obj.Index,
                    obj.IsRoot, obj.Tag, obj.Expanded, obj.Toggle,
                    obj.FixedQty,
                    obj.DailyQty, obj.ProductionQty,
                    obj.ProdOverFromTime,
                    obj.ProdOverToTime,
                    obj.Locked, obj.LockedProd, obj.ClosedOrd,
                    obj.ProdColor, obj.Article, obj.Department, obj.Launched,
                    obj.Operation, obj.Idx, obj.ParentIdx,
                    obj.LoadedQty, obj.Members, obj.QtyH, obj.Id, obj.ToDvc, obj.ToRdd);
            }

            if (Store.Default.sectorId == 7)
            {
                for (int i = 0; i < _gChart.Bars.Count; i++)
                {
                    int x = i + 1;
                    if (x < _gChart.Bars.Count)
                    {
                        var bar1 = _gChart.Bars[i];
                        var bar2 = _gChart.Bars[i + 1];
                      
                    }
                }
            }

            _gChart.Refresh();
        }

        public void AddTimelineObjects()
        {
            SelectedGroupColor();
            SetPrincipalBarIndex(false);
            ByQty = false;
        }

        private void Gantt_DoubleClick(object sender, EventArgs e)
        {
            if (_gChart.MouseOverRowValue == null) return;
            var val = (Bar)_gChart.MouseOverRowValue;
            if (val.Color == Color.Crimson) return;

            var modBefore = JobModel.GetModelIndex(val.RowText, _indexerList, -1);
            if (modBefore != null && modBefore.ProdQty < modBefore.LoadedQty && !modBefore.ClosedByUser
                    && modBefore.ProdQty < modBefore.LoadedQty)
            {
                _tmTip?.Dispose();
                MessageBox.Show("Cannot proceed to '" + val.RowText +
                    "' before you complete '" + modBefore.Name + "'.", Text,
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            TargetOrder = val.RowText;
            TargetLine = val.Tag;
            TargetModelStartDate = val.FromTime;
            TargetModelColor = val.Color;
            TargetClosedByUser = val.ClosedOrd;
            TargetLocked = val.Locked;
            TargetLockedProd = val.LockedProd;
            TargetDepartment = val.Department;
            var start = _gChart.FromDate;
            var end = _gChart.ToDate;

            if (_tmTip != null) _tmTip.Dispose();
            var ci = new ProductionInput();
            ci.StartPosition = FormStartPosition.CenterScreen;
            ci.ShowDialog();
            ci.Dispose();
            TargetOrder = string.Empty;
            _gChart.FromDate = start;
            _gChart.ToDate = end;            
            AddTimelineObjects();
            _gChart.Refresh();
        }
        private void AddGanttContextMenu()
        {
            _ctxMenuStrip = new ContextMenuStrip();
            _ctxMenuStrip.RenderMode = ToolStripRenderMode.System;
            _ctxMenuStrip.ImageScalingSize = new Size(20, 20);
            _ctxMenuStrip.Font = new Font("Tahoma", 9, FontStyle.Regular);

            ToolStripMenuItem tsmi0 = new ToolStripMenuItem();
            ToolStripMenuItem tsmi1 = new ToolStripMenuItem();
            ToolStripMenuItem tsmiGroupOpt = new ToolStripMenuItem();
            ToolStripMenuItem tsmi2 = new ToolStripMenuItem();
            ToolStripMenuItem tsmi3 = new ToolStripMenuItem();

            tsmi0.Text = "Commessa details";
            tsmi0.Image = Properties.Resources.search_16;
            tsmi1.Text = "Commesse da programmare";
            tsmi1.Image = Properties.Resources.programmare_16;
            tsmiGroupOpt.Text = "Options";
            tsmi2.Text = "Fractionate";
            tsmi2.Image = Properties.Resources.reprogram_16;
            tsmi3.Text = "Delete";
            tsmi3.Image = Properties.Resources.trash_16;

            _ctxMenuStrip.Opened += (s, e) =>
                {
                    _ctxActive = true;

                    tsmi2.Enabled = true;
                    tsmi3.Enabled = true;
                };
            _ctxMenuStrip.Closed += (sender, e) =>
            {
                _ctxActive = false;
            };
            tsmi0.Click += (s, e) =>
            {
                var val = (Bar)_gChart.MouseOverRowValue;
                if (val == null)
                {
                    MessageBox.Show("Unreachable field selected.");
                    return;
                }
                TargetOrder = val.RowText;
                TargetLine = val.Tag;
                TargetDepartment = val.Department;

                var cl = new LoadingJob(false);
                cl.UseSingleFilter = true;
                cl.WindowState = FormWindowState.Normal;
                cl.ShowDialog();
                cl.Dispose();
                cl.UseSingleFilter = false;
                TargetOrder = string.Empty;
                TargetLine = string.Empty;
                _gChart.Refresh();
            };
            tsmi1.Click += (s, e) =>
            {
                if (_tmTip != null) _tmTip.Dispose();
                 InsertCommessaProgram();
                
               
            };
            tsmi2.Click += (s, e) =>
            {
                if (_tmTip != null) _tmTip.Dispose();
                if (_gChart.MouseOverRowValue != null)
                {
                    var bar = (Bar)_gChart.MouseOverRowValue;
               
                    if (bar.ClosedOrd || bar.LoadedQty - bar.ProductionQty == 0)
                    {
                        MessageBox.Show("Cannot block a closed order.", "Workflow", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if(CheckOrderHasFractionate(bar.RowText))
                    {
                        MessageBox.Show("Order already fractionated.\nPlease undo work and fractionate correctly.", "Workflow", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    var bo = new FractionateOrder(bar);
                    bo.ShowDialog();
                    bo.Dispose();

                    // Reload chart only in case if user save changes in blocking order
                    if (bo.DialogResult == DialogResult.OK)
                    {
                        _ctxMenuStrip = null;
                        _gChart.Refresh();
                        if(Store.Default.sectorId==7)
                        {
                            Central.GroupForTessitura();
                        }
                        AddTimelineObjects();
                    }
                }
                else
                {
                    // Ignore empty selected field
                }
            };
            tsmi3.Click += (s, e) =>
            {
                _tmTip?.Dispose();
                if (_gChart.MouseOverRowValue == null)
                {
                    MessageBox.Show("Cannot perform the DELETE action from an unreachable field.",
                        "Workflow controller", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    DeleteCommessaProgram();
                }
            };

            _ctxMenuStrip.Items.Add(tsmi0);
            _ctxMenuStrip.Items.Add(tsmi1);
            _ctxMenuStrip.Items.Add(new ToolStripSeparator());
            _ctxMenuStrip.Items.Add(tsmiGroupOpt);
            tsmiGroupOpt.DropDownItems.Add(tsmi2);
            tsmiGroupOpt.DropDownItems.Add(new ToolStripSeparator());
            tsmiGroupOpt.DropDownItems.Add(tsmi3);

            _gChart.ContextMenuStrip = _ctxMenuStrip;
        }

        public static List<string> LineActiveArticlesList = new List<string>();
      
        public void InsertCommessaProgram()
        {
            try
            {
                TargetProgramDate = _gChart.MouseOverColumnDate;

                if (TargetProgramDate == null || TargetProgramDate == DateTime.MinValue)
                {
                    MessageBox.Show("Invalid selection.");
                    return ;
                }
                else
                {
                    var selectedorder = Central.TaskList.Where(a => a.FlowEnd.Date == TargetProgramDate.Date).LastOrDefault();
                    DateTime dborder;
                    using (var ctx = new DataContext(Central.SpecialConnStr))
                    {
                       dborder= ctx.ExecuteQuery<DateTime>("Select flowenddate from objects where ordername={0} and department={1}", selectedorder.Name, selectedorder.Department).FirstOrDefault();
                    }
                    TargetProgramDate = dborder;
                }
                var nextValue = (Bar)_gChart.MouseOverNextValue;
                TargetLine = nextValue.Tag;
                TargetDepartment = nextValue.Department;
                Article = nextValue.Article;

                var lineQuery = from lines in Tables.Lines
                                where lines.Line == TargetLine && lines.Department == TargetDepartment
                                select lines;
                var lineDesc = lineQuery.Any() ? lineQuery.FirstOrDefault().Description : null;

                var sDate = JobModel.GetLineNextDate(TargetLine, TargetDepartment);
                ManualDateTime = sDate;

                var activeLineMembers = Central.TaskList.Where(x => x.Aim == nextValue.Tag && x.Department == nextValue.Department).ToList();
                LineActiveArticlesList = new List<string>();
                foreach (var member in activeLineMembers)
                {
                    if (!LineActiveArticlesList.Contains(member.Article))
                    {
                        LineActiveArticlesList.Add(member.Article);
                    }
                }
                Central.IsProgramare = true;

                OpenCaricoLavoro(true);

                _ctxMenuStrip = null;
                if (TargetOrder == string.Empty)
                {
                    return ;
                }

                var orderQuery = (from ord in Models.Tables.Orders
                                  where ord.NrComanda == TargetOrder
                                  && ord.Department == TargetDepartment
                                  && ord.IsDeleted == false
                                  select ord).SingleOrDefault();

                var exist = Central.TaskList
                    .Where(x => x.Name == orderQuery.NrComanda
                    && x.Department == orderQuery.Department).ToList().Count;

                if (exist > 0)
                {
                    MessageBox.Show("Order already exist as an accepted model.\n" +
                        "Parametarized or cloning anomaly has been occurred.", "Workflow controller",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return ;
                }
                if (orderQuery == null)
                {
                    MessageBox.Show("Order fail.", "Workflow controller", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return ;
                }
                var artQ = (from art in Models.Tables.Articles
                            where art.Id == orderQuery.IdArticol
                            select art).SingleOrDefault();
                if (artQ == null)
                {
                    MessageBox.Show("Article QtyH returns to zero.", "Security check", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return ;
                }
                var jobMod = new JobModel();
                double.TryParse(artQ.Centes.ToString(), out var qtyH);
                if (Store.Default.sectorId == 2 || Store.Default.sectorId == 8) qtyH = TargetQtyH;
                double.TryParse(artQ.Prezzo.ToString(), out var price);
                int.TryParse(orderQuery.Carico.ToString(), out var carico);
                var qty = ByQty ? orderQuery.Cantitate : carico;
                if (ByQty && carico <= 0)
                {
                    qty = orderQuery.Cantitate;
                    ByQty = true;
                }

                var dur = jobMod.CalculateJobDuration(TargetLine, qty, qtyH, TargetDepartment, Members);
                var eDate = ManualDateTime.AddDays(+dur);
                var dailyQty = jobMod.CalculateDailyQty(TargetLine, qtyH, TargetDepartment, Members, qty);
                var loadingJob = new LoadingJob(false);
                loadingJob.InsertNewProgram(TargetOrder, TargetLine, artQ.Articol, orderQuery.Cantitate, qtyH, ManualDateTime, orderQuery.Dvc, orderQuery.Rdd, dur, dailyQty, price, orderQuery.Department, Members, ByManualDate, TargetLaunched);
                using (var ctx = new DataContext(Central.ConnStr))
                {
                    ctx.ExecuteCommand("update comenzi set DataProduzione={0},DataFine={1},Line={2},QtyInstead={3} where NrComanda={4} and department={5}",
                        sDate, eDate,
                        TargetLine, ByQty, TargetOrder, TargetDepartment);
                }
              
                AddTimelineObjects();
                if (_loadingJobForm != null && _clActive)
                {
                    _loadingJobForm.SelectProgrammedCommessa(TargetOrder);
                }
               
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Central.IsProgramare = false;
                _ctxMenuStrip = null;
                MessageBox.Show("Order has been doubled or interrupted from another device.",
                    "Workflow controller",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                
            }
        }
        private void DeleteCommessaProgram()
        {
            if (_gChart.MouseOverRowValue != null)
            {
                var val = (Bar)_gChart.MouseOverRowValue;
                TargetOrder = val.RowText;
                TargetLine = val.Tag;
                TargetDepartment = val.Department;

                var dr = MessageBox.Show("Are you sure you want to delete commessa: " + val.RowText, "Workflow controller", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.No)
                {
                    TargetOrder = string.Empty;
                    TargetLine = string.Empty;
                    TargetDepartment = string.Empty;
                    return;
                }
                var prodComm = (from prod in Models.Tables.Productions
                                where prod.Commessa == TargetOrder
                                && prod.Line == TargetLine && prod.Department == TargetDepartment
                                select prod).ToList();
                if (prodComm.Count > 0)
                {
                    MessageBox.Show("Unable to delete a structured task.",
                        "Workflow controller", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    TargetOrder = string.Empty;
                    TargetLine = string.Empty;
                    TargetDepartment = string.Empty;
                    return;
                }

                var check = CheckOrderSplitStatus(TargetOrder, TargetDepartment);
                var check2 = CheckOrderFractionated(TargetOrder);
                if (check || check2)
                {
                    MessageBox.Show("Unable to delete a splitted/fractionated base.", "Workflow controller", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var checkIsSplitted = (from split in Central.TaskList
                                       where split.Name == TargetOrder && split.Department == TargetDepartment && split.Aim == TargetLine
                                       && split.IsBase == false
                                       select split).SingleOrDefault();

                if (checkIsSplitted != null)
                {
                    var getOrder = (from baseOrd in Central.TaskList
                                    where baseOrd.Name == checkIsSplitted.Name.Split('.')[0] && baseOrd.Department == TargetDepartment
                                    select baseOrd).SingleOrDefault();

                    if (getOrder != null)
                    {
                        var newQty = checkIsSplitted.LoadedQty + getOrder.LoadedQty;

                        var j = new JobModel();
                        var newDur = j.CalculateJobDuration(getOrder.Aim, newQty, getOrder.QtyH, getOrder.Department);
                        var startdate = getOrder.StartDate;
                        var endDate = startdate.AddDays(+newDur);

                        using (var ctx = new System.Data.Linq.DataContext(Central.SpecialConnStr))
                        {
                            ctx.ExecuteCommand("update objects set loadedQty={0}, StartDate={1}, duration={2}, endDate={3} where ordername={4} and aim={5} and department={6}",
                                newQty, JobModel.GetLSpan(startdate), newDur, JobModel.GetLSpan(endDate), getOrder.Name, getOrder.Aim, getOrder.Department);
                        }
                    }
                }

                using (var ctx = new System.Data.Linq.DataContext(Central.SpecialConnStr))
                {
                    ctx.ExecuteCommand("delete from objects where ordername={0} and aim={1} and department={2}",
                        TargetOrder, TargetLine, TargetDepartment);
                }
                using (var ctx = new System.Data.Linq.DataContext(Central.ConnStr))
                {
                    ctx.ExecuteCommand("update comenzi set DataProduzione=null,DataFine=null,Line=null,QtyInstead=null where NrComanda={0}" +
                        " and Department={1}",
                        TargetOrder, TargetDepartment);
                }
                ListOfRemovedOrders.Add(TargetOrder);
                Config.InsertOperationLog("manual_programmingremoval",
                    TargetOrder + "-" + TargetLine + "-" + Store.Default.selDept, "workflowcontroller");
                PopulateFilters();
                _ctxMenuStrip = null;
                _gChart.Refresh();
                AddTimelineObjects();
                TargetOrder = string.Empty;
                TargetLine = string.Empty;
                TargetDepartment = string.Empty;
            }
        }

        private void OpenCaricoLavoro(bool isUpdate = false)
        {
            if (LoadingJobForm == null)
            {
                spContainer.Panel2Collapsed = false;
                spContainer.SplitterDistance = 200;
                var lbl = new Label
                {
                    Text = "Initializing carico lavoro...",
                    AutoSize = true
                };
                foreach (Control c in spContainer.Panel2.Controls)
                {
                    if (c is Form form)
                        if (form != null)
                        {
                            form.Close();
                            form.Dispose();
                        }
                }
                lbl.Location = new Point(spContainer.Panel2.Width / 2 - lbl.Width / 2,
                    spContainer.Panel2.Height / 2 - lbl.Height / 2);
                lbl.Font = new Font("Terminal", 12, FontStyle.Bold);
                spContainer.Panel2.Controls.Add(lbl);
                lbl.BringToFront();
                spContainer.Refresh();
                SuspendLayout();

                LoadingJobForm = new LoadingJob(isUpdate);
                var clr = LoadingJobForm.BackColor;
                LoadingJobForm.Opacity = 0.0;
                LoadingJobForm.FormBorderStyle = FormBorderStyle.None;
                LoadingJobForm.ShowInTaskbar = false;
                LoadingJobForm.ControlBox = false;
                LoadingJobForm.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
                LoadingJobForm.Visible = false;
                LoadingJobForm.Show();
                LoadingJobForm.TopLevel = false;
                spContainer.Panel2.Controls.Add(LoadingJobForm);
                LoadingJobForm.Location = new Point(0, 0);
                LoadingJobForm.Size = spContainer.Panel2.Size;
                LoadingJobForm.Visible = true;
                LoadingJobForm.BackColor = clr;
                btnCallCarico.Text = "";
                btnCallCarico.ImageAlign = ContentAlignment.MiddleCenter;
                btnCallCarico.Image = Properties.Resources.back_3d_icon;
                ResumeLayout(true);
                LoadingJobForm.Opacity = 1.0;
                lbl.Dispose();

                _clActive = true;
            }
            else
            {
                btnCallCarico.ImageAlign = ContentAlignment.MiddleLeft;
                btnCallCarico.Refresh();

                SuspendLayout();
                foreach (Control c in spContainer.Panel2.Controls)
                {
                    if (c is Form form)
                        if (form != null)
                        {
                            form.Close();
                            form.Dispose();
                        }
                }

                LoadingJobForm.Dispose();
                LoadingJobForm = null;
                if (Central.IsProgramare) Central.IsProgramare = false;
                Thread.Sleep(400);

                spContainer.Panel2Collapsed = true;
                ResumeLayout(true);
                Refresh();

                btnCallCarico.Image = null;
                btnCallCarico.Text = "Carico lavoro";

                _clActive = false;
                AddTimelineObjects();
            }
        }

        private void btnCallCarico_Click(object sender, EventArgs e)
        {
            OpenCaricoLavoro();
            AddTimelineObjects();
        }

        private DataGridView _dgvTip = new DataGridView();
        private System.Threading.Timer _tmTip = null;
        private void ShowTableTip(object info)
        { 
            var coord = _gChart.PointToClient(Cursor.Position);

            if (Cursor.Position.X < Parent.Location.X + _gChart.Location.X ||
                Cursor.Position.Y < Parent.Location.Y + _gChart.Location.Y)
            {
                _gChart.MouseOverRowValue = null;
                _gChart.Refresh();
                _tmTip?.Dispose();
                return;
            }

            if (_gChart.MouseOverRowValue == null)
            {
                return;
            }
            var val = (Bar)_gChart.MouseOverRowValue;
            var prodVal = (Bar)_gChart.MouseOverProductionBar;
            if (_ctxActive) return;

            _dgvTip = new TableView();
            _dgvTip.DoubleBuffered(true);
            _dgvTip.BorderStyle = BorderStyle.None;
            _dgvTip.Width = 360;
            _dgvTip.RowHeadersVisible = false;
            _dgvTip.ColumnHeadersVisible = false;

            var model = Central.TaskList.FirstOrDefault(x => x.Aim == val.Tag && x.Name == val.RowText
            && x.Department == val.Department);
            if (model == null) return;

            var sb = new StringBuilder();
            var dt = new DataTable();
            dt.Columns.Add("value");
            dt.Columns.Add("text");
            
            if (_gChart.IsMouseOverProductionBar)
            {
                dt.Rows.Add(new[] { "Commessa", model.Name });
                dt.Rows.Add(new[] { "Produced qty", model.ProdQty.ToString() });
                dt.Rows.Add(new[] { "Residual", (model.LoadedQty - model.ProdQty + model.OverQty).ToString() });
                if (model.OverQty > 0)
                    dt.Rows.Add(new[] { "Over", model.OverQty.ToString() });
            }
            else
            {
                if (val.Color == Color.FromArgb(80, 144, 169) || val.Color == Color.Gray)
                {
                    dt.Rows.Add(new[] { "Commessa", model.Name });
                    dt.Rows.Add(new[] { "Articoli", model.Article });
                    dt.Rows.Add(new[] { "Qty", model.LoadedQty.ToString() });
                    dt.Rows.Add(new[] { "Daily qty", model.DailyProd.ToString() });
                    dt.Rows.Add(new[] { "Data Inizio Produzione", model.StartDate.ToString("dd/MM/yyyy HH:mm") });
                    dt.Rows.Add(new[] { "Data Fine Produzione", model.EndDate.ToString("dd/MM/yyyy HH:mm") });
                    dt.Rows.Add(new[] { "Duration", TimeSpan.FromDays(model.Duration).ToString(@"dd\ hh\:mm")});
                    dt.Rows.Add(new[] { "Days on holiday", model.HolidayRange.ToString() });
                    dt.Rows.Add(new[] { "Members/machines", model.Members.ToString() });
                }
                else if (val.Color == Color.Orange)
                {
                    var delTime = TimeSpan.FromTicks(model.DelayTime);
                    dt.Rows.Add(new[] { "Commessa", model.Name });
                    dt.Rows.Add(new[] { "Delay start", model.EndDate.ToString("dd/MM/yyyy HH:mm") });
                    dt.Rows.Add(new[] { "Delay End ", model.EndDate.AddDays(+delTime.Days)
                        .AddHours(+delTime.Hours).AddMinutes(+delTime.Minutes).ToString("dd/MM/yyyy HH:mm") });
                }
            }
            _dgvTip.DataSource = dt;
            if (_gChart.InvokeRequired)
            {
                _gChart.BeginInvoke((Action)(() =>
                {
                    if (_gChart == null || _dgvTip == null) return;

                    _dgvTip.Location = _gChart.PointToClient(new Point(Cursor.Position.X, Cursor.Position.Y));
                    _gChart.Controls.Add(_dgvTip);
                    _dgvTip.BringToFront();
                    var hg = 0;
                    foreach (DataGridViewRow row in _dgvTip.Rows)
                    {
                        hg += row.Height;
                    }
                    _dgvTip.Height = hg + 1;
                    if (_dgvTip.Columns.Count >= 1)
                    {
                        _dgvTip.Columns[0].Width = 150;
                        _dgvTip.Columns[1].Width = 209;
                        _dgvTip.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }

                }));
            }
            else
            {
                if (_gChart == null || _dgvTip == null) return;

                _dgvTip.Location = _gChart.PointToClient(new Point(Cursor.Position.X, Cursor.Position.Y));
                _gChart.Controls.Add(_dgvTip);
                _dgvTip.BringToFront();
                var hg = 0;
                foreach (DataGridViewRow row in _dgvTip.Rows)
                {
                    hg += row.Height;
                }
                _dgvTip.Height = hg + 1;
                if (_dgvTip.Columns.Count >= 1)
                {
                    _dgvTip.Columns[0].Width = 150;
                    _dgvTip.Columns[1].Width = 209;
                    _dgvTip.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
            }
        }
        private void Gantt_MouseMove(object sender, MouseEventArgs eventArgs)
        {
            if (_ctxActive) return;

            _tmTip?.Dispose();
            _dgvTip?.Dispose();
            TimerCallback tcb = new TimerCallback(ShowTableTip);
            AutoResetEvent are = new AutoResetEvent(true);
            _tmTip = new System.Threading.Timer(tcb, are, 1000, 0);
        }
        private void PopulateFilters()
        {
            cbComm.Items.Clear();
            cbArt.Items.Clear();
            cbComm.Items.Add("");
            cbArt.Items.Add("");
            foreach (var item in Central.TaskList)
            {
                if (!cbComm.Items.Contains(item.Name))
                    cbComm.Items.Add(item.Name);
                if (!cbArt.Items.Contains(item.Article))
                    cbArt.Items.Add(item.Article);
            }
        }

        private void cbComm_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            try
            {
                var cb = (ComboBox)sender;
                vScrollBar1.Value = 0;
                _gChart.ScrollPosition = 0;

                if (cb.SelectedIndex == 0)
                {
                    _gChart.FromDate = JobModel.DateFromLast;
                    _gChart.ToDate = JobModel.DateToLast;
                    _gChart.FilteredRowText = string.Empty;
                }
                else
                {
                    var q = (from model in Central.TaskList
                             where model.Name == cb.Text
                             select model).SingleOrDefault();
                    var month = q.StartDate.Date.Month;
                    var year = q.StartDate.Date.Year;
                    var day = q.StartDate.Date.Day;
                    var sDate = new DateTime(year, month, day).AddDays(-15);
                    var eDate = new DateTime(year, month, DateTime.DaysInMonth(year, month)).AddDays(+15);
                    _gChart.FromDate = sDate;
                    _gChart.ToDate = eDate;
                    _gChart.FilteredRowText = cb.Text;
                }
                _gChart.Refresh();
            }
            catch
            {
                MessageBox.Show("More then one orders found.", "Workflow controller", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
           
        }

        private void cbArt_SelectedIndexChanged(object sender, EventArgs e)
        {
            vScrollBar1.Value = 0;
            _gChart.ScrollPosition = 0;
            var c = new Central();
            c.GetBase();
            AddTimelineObjects();
            _gChart.Refresh();
        }

        private void NavigateBack()
        {
            _gChart.FromDate = _gChart.FromDate.AddDays(-1);
            _gChart.ToDate = _gChart.ToDate.AddDays(-1);
            _gChart.Refresh();
        }
        private void NavigateBackMega()
        {
            _gChart.FromDate = _gChart.FromDate.AddDays(-7);
            _gChart.ToDate = _gChart.ToDate.AddDays(-7);

            _gChart.Refresh();
        }
        private void NavigateForw()
        {
            _gChart.FromDate = _gChart.FromDate.AddDays(+1);
            _gChart.ToDate = _gChart.ToDate.AddDays(+1);
            _gChart.Refresh();
        }
        private void NavigateForwMega()
        {
            _gChart.FromDate = _gChart.FromDate.AddDays(+7);
            _gChart.ToDate = _gChart.ToDate.AddDays(+7);
            _gChart.Refresh();
        }
        private void ZoomIn()
        {
            if (_gChart.ToDate.Subtract(_gChart.FromDate).TotalDays < 3)
            {
                return;
            }
            if (_gChart.BarHeight < 40)
            {
                _gChart.BarHeight++;
                _gChart.Refresh();
            }
            else
            {
                if (_isOutZoomed)
                {
                    panel1.Enabled = false;
                    var tm = new System.Windows.Forms.Timer();
                    tm.Interval = 1000;
                    tm.Enabled = true;
                    tm.Start();
                    tm.Tick += delegate
                        {
                            panel1.Enabled = true;
                            _isOutZoomed = false;
                            btnZoomIn.Focus();
                            tm.Dispose();
                        };
                }
                _gChart.FromDate = _gChart.FromDate.AddDays(+1);
                _gChart.ToDate = _gChart.ToDate.AddDays(-1);
                _gChart.Refresh();
                btnZoomOut.BackColor = Color.FromArgb(235, 235, 235);
            }
        }
        private bool _isOutZoomed = false;
        private void ZoomOut()
        {
            if (_gChart.BarHeight == 28) return;
            _gChart.BarHeight -= 1;
            _gChart.FromDate = _gChart.FromDate.AddDays(-1);
            _gChart.ToDate = _gChart.ToDate.AddDays(+1);
            _gChart.Refresh();
            _isOutZoomed = true;
            if (_gChart.BarHeight < 40)
                btnZoomOut.BackColor = Color.LightBlue;
            else if (_gChart.BarHeight == 40)
                btnZoomIn.BackColor = Color.WhiteSmoke;
        }
        private void HScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.NewValue == e.OldValue)
            {
                return;
            }

            if (e.Type == ScrollEventType.LargeDecrement |
                e.Type == ScrollEventType.LargeIncrement) return;

            if (e.NewValue > e.OldValue)
            {
                _gChart.FromDate = _gChart.FromDate.AddDays(+hScrollBar1.SmallChange);
                _gChart.ToDate = _gChart.ToDate.AddDays(+hScrollBar1.SmallChange);
                _gChart.Refresh();
            }
            else
            {
                _gChart.FromDate = _gChart.FromDate.AddDays(-hScrollBar1.SmallChange);
                _gChart.ToDate = _gChart.ToDate.AddDays(-hScrollBar1.SmallChange);
                _gChart.Refresh();
            }
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            try
            {
                _gChart.ScrollPosition = vScrollBar1.Value;
                _gChart.Refresh();
                _gChart.Invalidate();
            }
            catch
            {
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            if (_gChart.Focused) return;
            NavigateBack();
        }

        private void btnMegaBack_Click(object sender, EventArgs e)
        {
            if (_gChart.Focused) return;
            NavigateBackMega();
        }

        private void btnMegaFow_Click(object sender, EventArgs e)
        {
            if (_gChart.Focused) return;
            NavigateForwMega();
        }

        private void btnFow_Click(object sender, EventArgs e)
        {
            if (_gChart.Focused) return;
            NavigateForw();
        }

        private void btnZoomIn_Click(object sender, EventArgs e)
        {
            if (_gChart.Focused) return;
            ZoomIn();
        }

        private void btnZoomOut_Click(object sender, EventArgs e)
        {
            if (_gChart.Focused) return;
            ZoomOut();
        }

        private void btnDayInfo_Click(object sender, EventArgs e)
        {
            btnDayInfo.BackColor = Color.FromArgb(235, 235, 235);
            _gChart.HideClosedTask = !_gChart.HideClosedTask;
            if (_gChart.HideClosedTask)
            {
                btnDayInfo.BackColor = Color.LightCoral;
            }
            _gChart.Refresh();
        }

        private void btnSchedule_Click(object sender, EventArgs e)
        {

        }

        private void btnHideDelay_Click(object sender, EventArgs e)
        {
            _gChart.HideDelay = !_gChart.HideDelay;
            btnHideDelay.BackColor = Color.FromArgb(235, 235, 235);
            if (_gChart.HideDelay)
            {
                btnHideDelay.BackColor = Color.PowderBlue;
            }
            _gChart.Refresh();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            var b = (Button)sender;
            var f = new LineList();
            f.Location = new Point(b.Parent.Location.X + b.Location.X, b.Height + 1);
            f.ShowDialog();
            f.Dispose();
            SkipLines = ListOfLinesSelected.Count > 0;
            AddTimelineObjects();
        }
        public bool CheckOrderHasFractionate(string order)
        {
            var check1 = order.Split('_');
            if (check1.Count() > 1)
            {
                int index = Convert.ToInt32(check1[1]);
                var check = (from tasks in Central.TaskList
                             where tasks.Name.Contains('_') && Convert.ToInt32(tasks.Name.Split('_')[1]) > index && tasks.Name.Split('_')[0] == order.Split('_')[0] && tasks.Department != "Sartoria"
                             select tasks).FirstOrDefault();
                if (check == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                var check = (from tasks in Central.TaskList
                             where tasks.Name.Contains('_') && tasks.Name.Split('_')[0] == order && tasks.Department != "Sartoria"
                             select tasks).FirstOrDefault();
                if (check == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        public bool CheckOrderFractionated(string order)
        {
            var check = (from tasks in Central.TaskList
                         where tasks.Name.Contains('_') && tasks.Name==order && tasks.Department!="Sartoria"
                         select tasks).SingleOrDefault();
            if(check==null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public bool CheckOrderSplitStatus(string order, string department)
        {
            var check = (from split in Central.TaskList
                         where split.Name.Split('.')[0] == order && split.Department == department && split.IsBase == false
                         select split).SingleOrDefault();
           
            if (check == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void CbArticle_CheckedChanged(object sender, EventArgs e)
        {
            Central.IsArticleSelection = !Central.IsArticleSelection;
            AddTimelineObjects();
        }

        private Holidays _holidays = null;

        private void BtnHolidays_Click(object sender, EventArgs e)
        {
            if (_holidays == null)
            {
                spContainer.Panel2Collapsed = false;
                spContainer.SplitterDistance = 280;
                var lbl = new Label
                {
                    Text = "Initializing holidays...",
                    AutoSize = true
                };
                foreach (Control c in spContainer.Panel2.Controls)
                {
                    if (c is Form form)
                        if (form != null)
                        {
                            form.Close();
                            form.Dispose();
                            form = null;
                        }
                }
                lbl.Location = new Point(spContainer.Panel2.Width / 2 - lbl.Width / 2,
                    spContainer.Panel2.Height / 2 - lbl.Height / 2);
                lbl.Font = new Font("Terminal", 12, FontStyle.Bold);
                spContainer.Panel2.Controls.Add(lbl);
                lbl.BringToFront();
                spContainer.Refresh();
                SuspendLayout();
                _holidays = new Holidays();
                var clr = _holidays.BackColor;
                _holidays.Opacity = 0.0;
                _holidays.FormBorderStyle = FormBorderStyle.None;
                _holidays.ShowInTaskbar = false;
                _holidays.ControlBox = false;
                _holidays.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
                _holidays.Visible = false;
                _holidays.Show();
                _holidays.TopLevel = false;
                spContainer.Panel2.Controls.Add(_holidays);
                _holidays.Location = new Point(0, 0);
                _holidays.Size = spContainer.Panel2.Size;
                _holidays.Visible = true;
                _holidays.BackColor = clr;
                btnHolidays.Text = "";
                ResumeLayout(true);
                _holidays.Opacity = 1.0;
                lbl.Dispose();

                _clActive = true;
            }
            else
            {
                SuspendLayout();
                foreach (Control c in spContainer.Panel2.Controls)
                {
                    if (c is Form form)
                        if (form != null)
                        {
                            form.Close();
                            form.Dispose();
                        }
                }

                _holidays.Dispose();
                _holidays = null;
                btnCallCarico.Image = null;
                btnCallCarico.Text = "Carico lavoro";
                spContainer.Panel2Collapsed = true;
                ResumeLayout(true);
                Refresh();

                _clActive = false;
            }
        }

        private void btnShowEff_Click(object sender, EventArgs e)
        {
            LoadingInfo.ShowLoading();
            LoadingInfo.InfoText = "     Please wait...     ";
            SetPrincipalBarIndex(true);
            ByQty = false;
            LoadingInfo.CloseLoading();
        }

        private void toggleCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            Central.IsQtySelection = !Central.IsQtySelection;
            AddTimelineObjects();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Central.IsActiveOrdersSelection = !Central.IsActiveOrdersSelection;
            AddTimelineObjects();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AddTimelineObjects();
        }

        private void grp1_Click(object sender, EventArgs e)
        {
            Central.GroupIndex = 1;
            Central.GroupForTessitura();
            AddTimelineObjects();
        }

        private void grp2_Click(object sender, EventArgs e)
        {
            Central.GroupIndex = 2;
            Central.GroupForTessitura();
            AddTimelineObjects();
        }

        private void grp3_Click(object sender, EventArgs e)
        {
            Central.GroupIndex = 3;
            Central.GroupForTessitura();
            AddTimelineObjects();
        }

        private void grp4_Click(object sender, EventArgs e)
        {
            Central.GroupIndex = 4;
            Central.GroupForTessitura();
            AddTimelineObjects();
        }

        private void grp5_Click(object sender, EventArgs e)
        {
            Central.GroupIndex = 5;
            Central.GroupForTessitura();
            AddTimelineObjects();
        }

        private void grpback_Click(object sender, EventArgs e)
        {
            Central.GroupIndex -=1;
            if (Central.GroupIndex <= 1) Central.GroupIndex = 1;
            Central.GroupForTessitura();
            AddTimelineObjects();
        }

        private void grpforword_Click(object sender, EventArgs e)
        {
            Central.GroupIndex +=1;
            if (Central.GroupIndex >= 5) Central.GroupIndex = 5;
            Central.GroupForTessitura();
            AddTimelineObjects();
        }

       
        private void SelectedGroupColor()
        {
            
            for(int i=0;i<BtnGroupLst.Count;i++)
            {
                if (i+1==Central.GroupIndex)
                    {
                    BtnGroupLst[i].BackColor = Color.Silver;
                }
                else {
                    BtnGroupLst[i].BackColor = Color.WhiteSmoke;
                }
            }
        }
    }
}