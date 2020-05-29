/*
 * Copyright(c) 2017-2019 Necton Inc.All Rights Reserved.
 *
 * @NECTON_LICENSE_HEADER_START@
 *
 * This file contains Original Code and/or Modifications of Original Code
* as defined in and that are subject to the Apple Public Source License
 * Version 2.0 (the 'License'). You may not use this file except in
 * compliance with the License.Please obtain a copy of the License at
 * http://www.opensource.necton.com/ncsl/ and read it before using this
 * file.
 *
 * The Original Code and all software distributed under the License are
 * distributed on an 'AS IS' basis, WITHOUT WARRANTY OF ANY KIND, EITHER
 * EXPRESS OR IMPLIED, AND APPLE HEREBY DISCLAIMS ALL SUCH WARRANTIES,
 * INCLUDING WITHOUT LIMITATION, ANY WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE, QUIET ENJOYMENT OR NON-INFRINGEMENT.
 * Please see the License for the specific language governing rights and
 * limitations under the License.
 *
 * @NECTON_LICENSE_HEADER_END@
 */

/*
 * sslKeyExchange.c - Support for key exchange and server key exchange
 */
namespace ganntproj1
{
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

    /// <summary>
    /// Defines the <see cref="Workflow" />
    /// </summary>
    public partial class Workflow : Form
    {
        //
        //Charting
        //
        /// <summary>
        /// Defines the _gChart
        /// </summary>
        private readonly Ganttogram _gChart = new Ganttogram();

        /// <summary>
        /// Defines the _objList
        /// </summary>
        private List<Bar> _objList = new List<Bar>();

        /// <summary>
        /// Defines the _clonedIndexes
        /// </summary>
        private List<Index> _clonedIndexes = new List<Index>();

        /// <summary>
        /// Defines the _lstExpanded
        /// </summary>
        private readonly List<string> _lstExpanded = new List<string>();

        /// <summary>
        /// Defines the _ctxMenuStrip
        /// </summary>
        private ContextMenuStrip _ctxMenuStrip = new ContextMenuStrip();

        /// <summary>
        /// Defines the _ctxActive
        /// </summary>
        private bool _ctxActive = false;

        /// <summary>
        /// Defines the _clActive
        /// </summary>
        private bool _clActive = false;

        //
        //Runtime
        //
        /// <summary>
        /// Defines the _config
        /// </summary>
        private readonly Config _config = new Config();

        /// <summary>
        /// Gets or sets a value indicating whether IsTreeState
        /// </summary>
        private bool IsTreeState { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether HoldCollapsedNodes
        /// </summary>
        private bool HoldCollapsedNodes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether ByQty
        /// </summary>
        public static bool ByQty { get; set; }

        /// <summary>
        /// Gets or sets the TargetOrder
        /// </summary>
        public static string TargetOrder { get; set; }

        /// <summary>
        /// Gets or sets the TargetLine
        /// </summary>
        public static string TargetLine { get; set; }
        /// <summary>
        /// Gets or sets the target department
        /// </summary>
        public static string TargetDepartment { get; set; }
        /// <summary>
        /// Gets or sets the TargetProgramDate
        /// </summary>
        public static DateTime TargetProgramDate { get; set; }

        /// <summary>
        /// Gets or sets the TargetModelStartDate
        /// </summary>
        public static DateTime TargetModelStartDate { get; set; }

        /// <summary>
        /// Gets or sets the TargetModelColor
        /// </summary>
        public static Color TargetModelColor { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether TargetClosedByUser
        /// </summary>
        public static bool TargetClosedByUser { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether TargetLocked
        /// </summary>
        public static bool TargetLocked { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether TargetLockedProd
        /// </summary>
        public static bool TargetLockedProd { get; set; }

        /// <summary>
        /// Gets or sets the ListOfRemovedOrders
        /// </summary>
        public static List<string> ListOfRemovedOrders { get; set; }

        /// <summary>
        /// Gets or sets the DefaultAim
        /// </summary>
        private string DefaultAim { get; set; }

        private string DefaultDept { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether IsDelayHidden
        /// </summary>
        public static bool IsDelayHidden { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Workflow"/> class.
        /// </summary>
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
            //_gChart.MouseLeave += (s, e) =>
            //{
            //    _gChart.Refresh();
            //};
            _config.Set_sql_conn(new SqlConnection
                (Config.GetOlyConn().Connection.ConnectionString));
            spContainer.Panel2Collapsed = true;

            FormClosing += (s, e) =>
            {
                _gChart?.Dispose();
                _tmTip?.Dispose();
                GC.Collect();
            };
        }

        /// <summary>
        /// The ProduzioneGantt_Load
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void ProduzioneGantt_Load(object sender, EventArgs e)
        {
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

        /// <summary>
        /// The LoadDataWithDateChange
        /// </summary>
        public void LoadDataWithDateChange()
        {
            _gChart.FromDate = Central.DateFrom;
            _gChart.ToDate = Central.DateTo;
            SkipLines = false;
            _gChart._barHeight = 40;
            ListOfLinesSelected = new List<string>();
            AddTimelineObjects();
        }

        /// <summary>
        /// Defines the _indexerList
        /// </summary>
        private List<Index> _indexerList = new List<Index>();

        /// <summary>
        /// The SetPrincipalBarIndex
        /// </summary>
        private void SetPrincipalBarIndex(bool effColor)
        {
            _gChart.Bars = new List<BarProperty>();
            _gChart.HeaderList = new List<Header>();
            _objList = new List<Bar>();
            //_gChart.FilteredRowText = string.Empty;
            //_gChart.FilteredRowAtt = string.Empty;

            var jobModel = new JobModel();
            var c = new Central();
            c.GetBase();
            c.GetProductionColor();
            if (Central.ListOfModels.Count <= 0) return;
            _indexerList = new List<Index>();
            DefaultAim = Central.ListOfModels.First().Aim;
            DefaultDept = Central.ListOfModels.First().Department;

            var islocked = false;

            /*Indexing of gantt objects*/
            var rowIdx = 0;
            var elementIdx = 0;
            foreach (var model in Central.ListOfModels)
            {
                if (SkipLines && !ListOfLinesSelected.Contains(model.Aim)) continue;
                if (model.Aim == DefaultAim && model.Department == DefaultDept && !islocked)
                {
                    _indexerList.Add(new Index(rowIdx, elementIdx, model.Name, model.Aim, model.Department));
                    elementIdx++;
                }
                else
                {
                    elementIdx = 0;
                    rowIdx++;
                    _indexerList.Add(new Index(rowIdx, elementIdx, model.Name, model.Aim, model.Department));
                    elementIdx++;
                }
                DefaultAim = model.Aim;
                DefaultDept = model.Department;
                islocked = model.Locked;
            }
            //if (cbComm.SelectedIndex > 0) _gChart.FilteredRowText = cbComm.Text;

            var timeToMoveForward = 0.0;
            var timeToMoveBack = 0.0;
            var objLockIndex = 0;
            foreach (var item in _indexerList)
            // Specifies the index for each model added to the controller
            {
                timeToMoveForward = 0.0;
                timeToMoveBack = 0.0;
                var model = Central.ListOfModels.SingleOrDefault(x => x.Name == item.ObjText && x.Aim == item.ObjAim && x.Department == item.ObjDept);
                var modelBefore = JobModel.GetModelIndex(model.Name, _indexerList, -1);
                var lockedModelBefore = JobModel.GetIndexAfterLock(model.Name, _indexerList, -1, objLockIndex);
                if (modelBefore != null)
                {
                    var delayBefore = TimeSpan.FromTicks(modelBefore.DelayTime);//.Days, modelBefore.DelayTime.Hours, modelBefore.DelayTime.Minutes,
                    //delayBeforeSeconds, modelBefore.DelayTime.Milliseconds);
                    var modelStartDate = model.StartDate;
                    var tck = 0.0;
                    if (modelBefore.DelayEndDate == DateTime.MinValue) tck = 0;
                    else tck = modelBefore.DelayEndDate.Subtract(modelBefore.EndDate).TotalDays;
                    var before = modelBefore.EndDate.AddDays(tck);
                    timeToMoveForward = before.Subtract(model.StartDate).TotalDays;
                    timeToMoveBack = modelStartDate.Subtract(before).TotalDays;
                    if (timeToMoveForward < 0.0) timeToMoveForward = 0.0;
                    if (timeToMoveBack < 0.0) timeToMoveBack = 0.0;
                }
                else if (lockedModelBefore != null && model.Aim == lockedModelBefore.Aim && model.Department == lockedModelBefore.Department)
                {
                    timeToMoveBack = 0;
                    timeToMoveForward = 0;
                    var d = model.Duration;
                    if (lockedModelBefore.ProdQty > 0)
                    {
                        if (model.ProdQty == 0)
                        {
                            model.StartDate = lockedModelBefore.ProductionEndDate.AddMinutes(+2);
                            model.EndDate = model.StartDate.AddDays(+d);
                        }
                        else
                        {
                            model.StartDate = model.ProductionStartDate;
                            model.EndDate = model.StartDate.AddDays(+d);
                        }
                    }
                    else
                    {
                        model.StartDate = lockedModelBefore.StartDate;
                        model.EndDate = model.StartDate.AddDays(+d);
                    }
                }
                var isClosed = model.ClosedByUser;
                var jobEnd = model.EndDate.AddDays(+timeToMoveForward).AddDays(-timeToMoveBack);
                var h = JobModel.SkipDateRange(model.StartDate.AddDays(+timeToMoveForward).AddDays(-timeToMoveBack), jobEnd.AddMinutes(-1), model.Aim);
                jobEnd = jobEnd.AddDays(+h);
                var prodEnd = new DateTime(model.ProductionEndDate.Year, model.ProductionEndDate.Month, model.ProductionEndDate.Day,
                    model.ProductionEndDate.Hour, model.ProductionEndDate.Minute, model.ProductionEndDate.Second);
                var delayTs = TimeSpan.FromTicks(model.DelayTime);
                var prodQty = model.ProdQty;
                var spCarico = model.LoadedQty;
                var delEnd = jobEnd.AddDays(delayTs.Days).AddHours(+delayTs.Hours);
                var delayStart = new DateTime(jobEnd.Year, jobEnd.Month, jobEnd.Day, jobEnd.Hour, jobEnd.Minute, jobEnd.Second);
                var delayEnd = delEnd; //jobEnd.AddDays(delayTs.Days).AddHours(+delayTs.Hours).AddMinutes(delayTs.Minutes).AddSeconds(delayTs.Seconds);
                if (prodQty > 0)
                {
                    if (isClosed && prodEnd.Date <= jobEnd.Date)
                    //closed model
                    {
                        jobEnd = prodEnd; //new DateTime(prodEnd.Year, prodEnd.Month, prodEnd.Day,
                                          //prodEnd.Hour, prodEnd.Minute, prodEnd.Second, prodEnd.Millisecond);
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
                    //finished model
                    {
                        jobEnd = prodEnd; // new DateTime(prodEnd.Year, prodEnd.Month, prodEnd.Day,
                                          //prodEnd.Hour, prodEnd.Minute, prodEnd.Second, prodEnd.Millisecond);
                        delayTs = new TimeSpan(0, 0, 0, 0, 0);
                        delayStart = Config.MinimalDate;
                        delayEnd = Config.MinimalDate;
                    }
                    else if (!isClosed && prodQty < spCarico && jobEnd < prodEnd)
                    {
                        //var pe = prodEnd.Subtract(jobEnd.AddMinutes(+1));
                        //delayTs = new TimeSpan(0, 0, 0, 0, 0);
                        delayStart = jobEnd.AddMinutes(+1);
                        delayEnd = prodEnd;
                    }
                    else if (!isClosed && prodQty < spCarico && delayTs != new TimeSpan(0, 0, 0, 0, 0))// || delayTs.Hours > 0)
                    {
                        var j = delayEnd; //jobEnd.AddDays(+delayTs.Days).AddHours(+delayTs.Hours);
                        delayStart = jobEnd.AddMinutes(+1);
                        if (j.Hour > 15)
                        {
                            var dh = 7 + (j.Hour - 15);
                            delayTs = new TimeSpan(delayTs.Days + 1, dh,
                                59, 59, 59);
                            delayEnd = new DateTime(delayEnd.Year, delayEnd.Month, delayEnd.Day, 0, 0, 0, 0);
                            delayEnd = delayEnd.AddDays(+1).AddHours(+dh).AddMinutes(+59).AddSeconds(+59);
                        }
                        else if (j.Hour < 7)
                        {
                            var cs = 7 + j.Hour;
                            delayTs = new TimeSpan(delayTs.Days, cs,
                                59, 59, 59);
                            delayEnd = new DateTime(delayEnd.Year, delayEnd.Month, delayEnd.Day, 0, 0, 0, 0);
                            delayEnd = delayEnd.AddHours(+delayTs.Hours).AddMinutes(+59).AddSeconds(+59);
                        }
                        var delInc = JobModel.SkipDateRange(delayStart, delayEnd, model.Aim);

                        //var si = JobModel.SkipWeekendRange(delayStart, delayEnd);
                        delayTs = new TimeSpan(delayTs.Days + delInc, delayTs.Hours, 0, 0, 0);
                        delayEnd = delayEnd.AddDays(+delInc);
                    }
                }

                // Update job model
                var query = (from md in Central.ListOfModels
                             where md.Name == model.Name
                             select md)
                             .Update(st =>
                             {
                                 st.StartDate = model.StartDate.AddDays(+timeToMoveForward).AddDays(-timeToMoveBack);
                                 st.EndDate = jobEnd.AddMinutes(-1);
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

                //get efficiency color

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

                // Insert objects into chart object list
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
                    model.Locked, model.IsLockedProduction,
                    model.ClosedByUser,
                    prodBarColor,
                    model.Article, model.Department));

                if (model.Locked || model.IsLockedProduction)
                // get last index of the locked model
                {
                    var idp = _indexerList.Where(x => x.ObjText == model.Name &&
                    x.ObjAim == model.Aim && x.ObjDept == model.Department).SingleOrDefault();
                    objLockIndex = idp.ObjIndex;
                }
            }

            foreach (var obj in _objList)
            {
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
                    obj.ProdColor, obj.Article, obj.Department);
            }
            _gChart.Refresh();
        }

        /// <summary>
        /// The AddTimelineObjects
        /// </summary>
        public void AddTimelineObjects()
        {
            SetPrincipalBarIndex(false);
            ByQty = false;
        }

        /// <summary>
        /// The Gantt_DoubleClick
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void Gantt_DoubleClick(object sender, EventArgs e)
        {
            if (_gChart.MouseOverRowValue == null) return;
            var val = (Bar)_gChart.MouseOverRowValue;
            if (val.Color == Color.Crimson) return;

            var modBefore = JobModel.GetModelIndex(val.RowText, _indexerList, -1);
            if (modBefore != null && modBefore.ProdQty < modBefore.LoadedQty && !modBefore.ClosedByUser
                    && modBefore.ProdQty < modBefore.LoadedQty && !modBefore.Locked)
            {
                _tmTip?.Dispose();
                MessageBox.Show("Cannot proceed to '" + val.RowText +
                    "' before you lock or complete '" + modBefore.Name + "'.", Text,
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
            //var frmTransp = new Form
            //{
            //    //Size = this.ClientRectangle.Size,
            //    BackColor = Color.Black,
            //    Opacity = 0.7,
            //    FormBorderStyle = FormBorderStyle.None,
            //    WindowState = FormWindowState.Maximized,
            //    ShowIcon = false,
            //    ShowInTaskbar = false
            //};
            ////_gChart.Refresh();
            //frmTransp.Show();
            if (_tmTip != null) _tmTip.Dispose();
            var ci = new ProductionInput();
            ci.StartPosition = FormStartPosition.CenterScreen;
            ci.ShowDialog();
            ci.Dispose();
            //frmTransp.Close();
            TargetOrder = string.Empty;
            _gChart.FromDate = start;
            _gChart.ToDate = end;
            //var c = new Central();
            //c.GetBase(null);               
            AddTimelineObjects();
            _gChart.Refresh();

            //ci.FormClosing += (s, ev) =>
            //{

            //};
            //frmTransp.Click += (s, ev) =>
            //{
            //    ci.Close();
            //    frmTransp.Close();
            //};
        }
        /// <summary>
        /// The AddGanttContextMenu
        /// </summary>
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
            ToolStripMenuItem tsmi4 = new ToolStripMenuItem();

            tsmi0.Text = "Commessa details";
            tsmi0.Image = Properties.Resources.search_16;
            tsmi1.Text = "Commesse da programmare";
            tsmi1.Image = Properties.Resources.programmare_16;
            tsmiGroupOpt.Text = "Options";
            tsmi2.Text = "Split commessa";
            tsmi2.Image = Properties.Resources.split_16;
            tsmi3.Text = "Delete";
            tsmi3.Image = Properties.Resources.trash_16;

            _ctxMenuStrip.Opened += (s, e) =>
                {
                    _ctxActive = true;

                    tsmi4.Enabled = true;
                    tsmi2.Enabled = true;
                    tsmi3.Enabled = true;
                    if (_gChart.MouseOverRowValue != null)
                        if (((Bar)_gChart.MouseOverRowValue).Locked)
                        {
                            tsmi4.Text = "Unlock";
                            tsmi4.Image = Properties.Resources.unlock_16;
                        }
                        else
                        {
                            tsmi4.Text = "Lock";
                            tsmi4.Image = Properties.Resources.lock_16;
                        }
                    else
                    {
                        tsmi4.Text = "Lock";
                        tsmi4.Image = Properties.Resources.lock_16;
                        tsmi4.Enabled = false;

                        tsmi2.Enabled = false;
                        tsmi3.Enabled = false;
                    }
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
                    var val = (Bar)_gChart.MouseOverRowValue;
                    TargetOrder = val.RowText;
                    TargetLine = val.Tag;
                    TargetDepartment = val.Department;
                    var start = _gChart.FromDate;
                    var end = _gChart.ToDate;
                    var frmTransp = new Form
                    {
                        Size = this.ClientRectangle.Size,
                        BackColor = Color.Black,
                        Opacity = 0.4,
                        FormBorderStyle = FormBorderStyle.None,
                        WindowState = FormWindowState.Maximized,
                        ShowIcon = false,
                        ShowInTaskbar = false
                    };
                    frmTransp.Show();
                    var si = new Split(TargetOrder, TargetLine, TargetDepartment);
                    si.StartPosition = FormStartPosition.CenterScreen;
                    si.Show();
                    si.FormClosing += (se, ev) =>
                    {
                        frmTransp.Close();
                        AddTimelineObjects();
                        _gChart.FromDate = start;
                        _gChart.ToDate = end;
                        TargetOrder = string.Empty;
                        TargetLine = string.Empty;
                        TargetDepartment = string.Empty;
                        _gChart.Refresh();
                    };
                    frmTransp.Click += (se, ev) =>
                    {
                        si.Close();
                        frmTransp.Close();
                    };
                }
                else
                {
                    //show important message
                    MessageBox.Show("Cannot perform the SPLIT query from an unreachable field.",
                        "Workflow controller", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            };
            tsmi3.Click += (s, e) =>
            {
                _tmTip?.Dispose();
                if (_gChart.MouseOverRowValue == null)
                {
                    //show important message
                    MessageBox.Show("Cannot perform the DELETE query from an unreachable field.",
                        "Workflow controller", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    DeleteCommessaProgram();
                }
            };
            tsmi4.Click += (s, e) =>
            {
                if (_gChart.MouseOverRowValue == null)
                {
                    //show important message
                    MessageBox.Show("Cannot perform the LOCK query from an unreachable field.",
                        "Workflow controller", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    var val = (Bar)_gChart.MouseOverRowValue;
                    TargetOrder = val.RowText;
                    TargetLine = val.Tag;
                    TargetDepartment = val.Department;
                    using (var ctx = new System.Data.Linq.DataContext(Central.SpecialConnStr))
                    {
                        var isLockedStatus = val.Locked;
                        if (isLockedStatus)
                        {
                            var fLock = false;
                            var modLock = JobModel.GetIndexAfterLock(val.RowText, _indexerList, 1, 0);
                            if (modLock != null && modLock.ProdQty > 0)
                            {
                                fLock = true;
                            }
                            ctx.ExecuteCommand("update objects set locked={0},lockedprod={1} where ordername={2} and aim={3}",
                                fLock, false, TargetOrder, TargetLine);
                        }
                        else
                        {

                            var lockDate = DateTime.Now.Subtract(Config.MinimalDate).Ticks;
                            ctx.ExecuteCommand("update objects set locked={0},lockedprod={1},lockdate={2} where ordername={3} and aim={4}",
                                true, true, lockDate, TargetOrder, TargetLine);
                        }
                    }
                    //var c = new Central();
                    //c.GetBase(null);
                    AddTimelineObjects();
                    _gChart.Refresh();
                    TargetOrder = string.Empty;
                    TargetLine = string.Empty;
                }
            };

            _ctxMenuStrip.Items.Add(tsmi0);
            _ctxMenuStrip.Items.Add(tsmi1);
            _ctxMenuStrip.Items.Add(new ToolStripSeparator());
            _ctxMenuStrip.Items.Add(tsmiGroupOpt);
            tsmiGroupOpt.DropDownItems.Add(tsmi2);
            tsmiGroupOpt.DropDownItems.Add(tsmi4);
            tsmiGroupOpt.DropDownItems.Add(new ToolStripSeparator());
            tsmiGroupOpt.DropDownItems.Add(tsmi3);

            _gChart.ContextMenuStrip = _ctxMenuStrip;
        }

        /// <summary>
        /// Defines the LineActiveArticlesList
        /// </summary>
        public static List<string> LineActiveArticlesList = new List<string>();

        /// <summary>
        /// The InsertCommessaProgram
        /// </summary>
        public void InsertCommessaProgram()
        {
            var loadingJob = new LoadingJob(true)
            {
                WindowState = FormWindowState.Normal
            };
            try
            {
                //get mouse over date
                TargetProgramDate = _gChart.MouseOverColumnDate;

                if (TargetProgramDate == null || TargetProgramDate == DateTime.MinValue)
                {
                    MessageBox.Show("Invalid selection.");
                    return;
                }
                // Gets dynamic value that is preeceding, next or greater index of the cursor
                // If row has two or more, always will be the last
                var nextValue = (Bar)_gChart.MouseOverNextValue;
                // Finds the aim of the found cursor
                //var nextValueModel = Central.ListOfModels.Where(x =>
                //x.Name == nextValue.RowText && x.Aim == nextValue.Tag && x.Department == nextValue.Department).SingleOrDefault();
                TargetLine = nextValue.Tag;
                TargetDepartment = nextValue.Department;
                var sDate = JobModel.GetLineLastDate(TargetLine, TargetDepartment);
                var activeLineMembers = Central.ListOfModels.Where(x => x.Aim == nextValue.Tag && x.Department == nextValue.Department).ToList();
                LineActiveArticlesList = new List<string>();
                foreach (var member in activeLineMembers)
                {
                    if (!LineActiveArticlesList.Contains(member.Article))
                    {
                        LineActiveArticlesList.Add(member.Article);
                    }
                }
                Central.IsProgramare = true;
                loadingJob.Text = "Carico lavoro - Commessa da programmare (" + TargetLine + ")";
                loadingJob.ShowDialog();
                loadingJob.Dispose();
                Central.IsProgramare = false;
                _ctxMenuStrip = null;
                if (TargetOrder == string.Empty)
                // Check if user did not select an order
                {
                    return;
                }
                /* Insert new order into objects table */
                var orderQuery = (from ord in Models.Tables.Orders
                                  where ord.NrComanda == TargetOrder
                                  && ord.Department == TargetDepartment
                                  select ord).SingleOrDefault();
                var exist = Central.ListOfModels.Where(x => x.Name == orderQuery.NrComanda && x.Department == orderQuery.Department).ToList();
                if (exist.Count > 0)
                {
                    MessageBox.Show("Order already exist as an accepted model.\n" +
                        "Parametarized or cloning anomaly has been occurred.", "Workflow controller",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (orderQuery == null)
                // Check if orders query doesn't get some of the records
                {
                    MessageBox.Show("Order fail.", "Workflow controller", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                var artQ = (from art in Models.Tables.Articles
                            where art.Id == orderQuery.IdArticol
                            select art).SingleOrDefault();
                if (artQ == null)
                {
                    MessageBox.Show("Article QtyH returns to zero.", "Security check", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                var jobMod = new JobModel();
                double.TryParse(artQ.Centes.ToString(), out var qtyH);
                double.TryParse(artQ.Prezzo.ToString(), out var price);
                int.TryParse(orderQuery.Carico.ToString(), out var carico);
                var qty = ByQty ? orderQuery.Cantitate : carico;
                if (ByQty && carico <= 0)
                {
                    //select total qty automatic
                    qty = orderQuery.Cantitate;
                    ByQty = true;
                }
                var dur = jobMod.CalculateJobDuration(TargetLine, qty, qtyH, TargetDepartment);
                var eDate = sDate.AddDays(+dur);
                var dailyQty = jobMod.CalculateDailyQty(TargetLine, qtyH, TargetDepartment);
                int.TryParse(dailyQty.ToString(), out var dq);
                loadingJob.InsertNewProgram(TargetOrder, TargetLine, artQ.Articol, orderQuery.Cantitate, qtyH, sDate, dur, dq, price, orderQuery.Department);
                using (var ctx = new System.Data.Linq.DataContext(Central.ConnStr))
                // update job aim
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
            }
            catch
            {
                Central.IsProgramare = false;
                _ctxMenuStrip = null;
                MessageBox.Show("Unreachable field selected.",
                    "Workflow controller",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }
        /// <summary>
        /// The DeleteCommessaProgram
        /// </summary>
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
                // Tests to see does order has production insertments
                var prodComm = (from prod in Models.Tables.Productions
                                where prod.Commessa == TargetOrder
                                && prod.Line == TargetLine && prod.Department == TargetDepartment
                                select prod).ToList();
                if (prodComm.Count > 0)
                // Disallow user to delete order which has production insertments
                {
                    MessageBox.Show("Unable to delete a structured task.",
                        "Workflow controller", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    TargetOrder = string.Empty;
                    TargetLine = string.Empty;
                    TargetDepartment = string.Empty;
                    return;
                }

                var check = CheckOrderSplitStatus(TargetOrder, TargetDepartment);
                if (check)
                {
                    MessageBox.Show("Unable to delete a splitted base.", "Workflow controller", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var checkIsSplitted = (from split in Central.ListOfModels
                                       where split.Name == TargetOrder && split.Department == TargetDepartment && split.Aim == TargetLine
                                       && split.IsBase == false
                                       select split).SingleOrDefault();

                if (checkIsSplitted != null)
                {
                    var getOrder = (from baseOrd in Central.ListOfModels
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
                        // update job aim
                        {
                            ctx.ExecuteCommand("update objects set loadedQty={0}, StartDate={1}, duration={2}, endDate={3} where ordername={4} and aim={5} and department={6}",
                                newQty, JobModel.GetLSpan(startdate), newDur, JobModel.GetLSpan(endDate), getOrder.Name, getOrder.Aim, getOrder.Department);
                        }
                    }
                }

                /* Delete(reset) order sturucture */
                using (var ctx = new System.Data.Linq.DataContext(Central.SpecialConnStr))
                // update job aim
                {
                    ctx.ExecuteCommand("delete from objects where ordername={0} and aim={1} and department={2}",
                        TargetOrder, TargetLine, TargetDepartment);
                }
                using (var context = new System.Data.Linq.DataContext(Central.ConnStr))
                {
                    // delete existing records
                    context.ExecuteCommand("update comenzi set DataProduzione=null,DataFine=null,Line=null,QtyInstead=null where NrComanda={0} and line={1} and department={2}",
                        TargetOrder, TargetLine, TargetDepartment);
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
        /// <summary>
        /// The cbDvc_CheckedChanged
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void cbDvc_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
                IsTreeState = true;
            else
                if (!HoldCollapsedNodes)
                _lstExpanded.Clear();
            IsTreeState = false;
            AddTimelineObjects();
        }

        /// <summary>
        /// The myCheckBox1_CheckedChanged
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        //private void myCheckBox1_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (((CheckBox)sender).Checked)
        //    {
        //        HoldCollapsedNodes = true;

        //        if (IsTreeState) cbTree.Checked = false;
        //    }
        //    else
        //    {
        //        HoldCollapsedNodes = false;
        //        cbTree.Checked = true;
        //    }
        //}

        /// <summary>
        /// Defines the _loadingJobForm
        /// </summary>
        private LoadingJob _loadingJobForm = null;

        /// <summary>
        /// The btnCallCarico_Click
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void btnCallCarico_Click(object sender, EventArgs e)
        {
            if (_loadingJobForm == null)
            {
                spContainer.Panel2Collapsed = false;
                spContainer.SplitterDistance = 280;
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
                _loadingJobForm = new LoadingJob(false);
                var clr = _loadingJobForm.BackColor;
                _loadingJobForm.Opacity = 0.0;
                _loadingJobForm.FormBorderStyle = FormBorderStyle.None;
                _loadingJobForm.ShowInTaskbar = false;
                _loadingJobForm.ControlBox = false;
                _loadingJobForm.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
                _loadingJobForm.Visible = false;
                _loadingJobForm.Show();
                _loadingJobForm.TopLevel = false;
                spContainer.Panel2.Controls.Add(_loadingJobForm);
                _loadingJobForm.Location = new Point(0, 0);
                _loadingJobForm.Size = spContainer.Panel2.Size;
                _loadingJobForm.Visible = true;
                _loadingJobForm.BackColor = clr;
                btnCallCarico.Text = "";
                btnCallCarico.ImageAlign = ContentAlignment.MiddleCenter;
                btnCallCarico.Image = Properties.Resources.back_3d_icon;
                ResumeLayout(true);
                _loadingJobForm.Opacity = 1.0;
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

                _loadingJobForm.Dispose();
                _loadingJobForm = null;
                Thread.Sleep(400);

                spContainer.Panel2Collapsed = true;
                ResumeLayout(true);
                Refresh();

                btnCallCarico.Image = null;
                btnCallCarico.Text = "Carico lavoro";

                _clActive = false;
            }
        }
        /// <summary>
        /// Defines the _dgvTip
        /// </summary>
        private DataGridView _dgvTip = new DataGridView();
        /// <summary>
        /// Defines the _tmTip
        /// </summary>
        private System.Threading.Timer _tmTip = null;
        /// <summary>
        /// The ShowLabelTip
        /// </summary>
        /// <param name="info">The info<see cref="object"/></param>
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

            var model = Central.ListOfModels.SingleOrDefault(x => x.Aim == val.Tag && x.Name == val.RowText 
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
                    dt.Rows.Add(new[] { "Duration", model.Duration.ToString() });
                    dt.Rows.Add(new[] { "Days on holiday", model.HolidayRange.ToString() });
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
        /// <summary>
        /// The Gantt_MouseMove
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="eventArgs">The eventArgs<see cref="MouseEventArgs"/></param>
        private void Gantt_MouseMove(object sender, MouseEventArgs eventArgs)
        {
            if (_ctxActive) return;

            _tmTip?.Dispose();
            _dgvTip?.Dispose();
            TimerCallback tcb = new TimerCallback(ShowTableTip);
            AutoResetEvent are = new AutoResetEvent(true);
            _tmTip = new System.Threading.Timer(tcb, are, 1000, 0);
        }
        /// <summary>
        /// The PopulateFilters
        /// </summary>
        private void PopulateFilters()
        {
            cbComm.Items.Clear();
            cbArt.Items.Clear();
            cbComm.Items.Add("");
            cbArt.Items.Add("");
            foreach (var item in Central.ListOfModels)
            {
                if (!cbComm.Items.Contains(item.Name))
                    cbComm.Items.Add(item.Name);
                if (!cbArt.Items.Contains(item.Article))
                    cbArt.Items.Add(item.Article);
            }
        }

        /// <summary>
        /// The cbComm_SelectedIndexChanged_1
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
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
                    var q = (from model in Central.ListOfModels
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

        /// <summary>
        /// The cbArt_SelectedIndexChanged
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void cbArt_SelectedIndexChanged(object sender, EventArgs e)
        {
            vScrollBar1.Value = 0;
            _gChart.ScrollPosition = 0;
            var c = new Central();
            c.GetBase();
            AddTimelineObjects();
            _gChart.Refresh();
        }

        /// <summary>
        /// The NavigateBack
        /// </summary>
        private void NavigateBack()
        {
            _gChart.FromDate = _gChart.FromDate.AddDays(-1);
            _gChart.ToDate = _gChart.ToDate.AddDays(-1);
            _gChart.Refresh();
        }
        /// <summary>
        /// The NavigateBackMega
        /// </summary>
        private void NavigateBackMega()
        {
            _gChart.FromDate = _gChart.FromDate.AddDays(-7);
            _gChart.ToDate = _gChart.ToDate.AddDays(-7);

            _gChart.Refresh();
        }
        /// <summary>
        /// The NavigateForw
        /// </summary>
        private void NavigateForw()
        {
            _gChart.FromDate = _gChart.FromDate.AddDays(+1);
            _gChart.ToDate = _gChart.ToDate.AddDays(+1);
            _gChart.Refresh();
        }
        /// <summary>
        /// The NavigateForwMega
        /// </summary>
        private void NavigateForwMega()
        {
            _gChart.FromDate = _gChart.FromDate.AddDays(+7);
            _gChart.ToDate = _gChart.ToDate.AddDays(+7);
            _gChart.Refresh();
        }
        /// <summary>
        /// The ZoomIn
        /// </summary>
        private void ZoomIn()
        {
            if (_gChart.ToDate.Subtract(_gChart.FromDate).TotalDays < 3)
            {
                return;
            }
            if (_gChart._barHeight < 40)
            {
                _gChart._barHeight++;
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
        /// <summary>
        /// Defines the _isOutZoomed
        /// </summary>
        private bool _isOutZoomed = false;
        /// <summary>
        /// The ZoomOut
        /// </summary>
        private void ZoomOut()
        {
            if (_gChart._barHeight == 28) return;
            _gChart._barHeight -= 1;
            _gChart.FromDate = _gChart.FromDate.AddDays(-1);
            _gChart.ToDate = _gChart.ToDate.AddDays(+1);
            _gChart.Refresh();
            _isOutZoomed = true;
            if (_gChart._barHeight < 40)
                btnZoomOut.BackColor = Color.LightBlue;
            else if (_gChart._barHeight == 40)
                btnZoomIn.BackColor = Color.WhiteSmoke;
        }
        /// <summary>
        /// The HScrollBar1_Scroll
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="ScrollEventArgs"/></param>
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

        /// <summary>
        /// The vScrollBar1_Scroll
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="ScrollEventArgs"/></param>
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
                // ignored
            }
        }

        /// <summary>
        /// The btnBack_Click
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void btnBack_Click(object sender, EventArgs e)
        {
            if (_gChart.Focused) return;
            NavigateBack();
        }

        /// <summary>
        /// The btnMegaBack_Click
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void btnMegaBack_Click(object sender, EventArgs e)
        {
            if (_gChart.Focused) return;
            NavigateBackMega();
        }

        /// <summary>
        /// The btnMegaFow_Click
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void btnMegaFow_Click(object sender, EventArgs e)
        {
            if (_gChart.Focused) return;
            NavigateForwMega();
        }

        /// <summary>
        /// The btnFow_Click
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void btnFow_Click(object sender, EventArgs e)
        {
            if (_gChart.Focused) return;
            NavigateForw();
        }

        /// <summary>
        /// The btnZoomIn_Click
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void btnZoomIn_Click(object sender, EventArgs e)
        {
            if (_gChart.Focused) return;
            ZoomIn();
        }

        /// <summary>
        /// The btnZoomOut_Click
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void btnZoomOut_Click(object sender, EventArgs e)
        {
            if (_gChart.Focused) return;
            ZoomOut();
        }

        /// <summary>
        /// The btnDayInfo_Click
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void btnDayInfo_Click(object sender, EventArgs e)
        {
            btnDayInfo.BackColor = Color.FromArgb(235, 235, 235);
            // _gChart.RectangleSelectorActivated = !_gChart.RectangleSelectorActivated;
            _gChart.HideClosedTask = !_gChart.HideClosedTask;
            if (_gChart.HideClosedTask)
            {
                btnDayInfo.BackColor = Color.LightCoral;
            }
            _gChart.Refresh();
        }

        /// <summary>
        /// The btnSchedule_Click
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void btnSchedule_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// The btnHideDelay_Click
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
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

        /// <summary>
        /// The WorkflowController_Paint
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="PaintEventArgs"/></param>
        private void WorkflowController_Paint(object sender, PaintEventArgs e)
        {
        }

        /// <summary>
        /// The WorkflowController_KeyDown
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="KeyEventArgs"/></param>
        private void WorkflowController_KeyDown(object sender, KeyEventArgs e)
        {
        }

        /// <summary>
        /// Gets or sets the ListOfLinesSelected
        /// </summary>
        public static List<string> ListOfLinesSelected { get; set; }

        /// <summary>
        /// Defines the SkipLines
        /// </summary>
        public static bool SkipLines = false;

        /// <summary>
        /// The Button1_Click
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
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

        public bool CheckOrderSplitStatus(string order, string department)
        {
            var check = (from split in Central.ListOfModels
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
    }
}