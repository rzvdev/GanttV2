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
    using System.ComponentModel;
    using System.Data.Linq;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using System.Xml.Linq;

    /// <summary>
    /// Defines the <see cref="Central" />
    /// </summary>
    public partial class Central : Form
    {
        /// <summary>
        /// Defines the SettingsSys
        /// </summary>
        public enum SettingsSys
        {
            /// <summary>
            /// Defines the Department
            /// </summary>
            Department,

            /// <summary>
            /// Defines the Shift
            /// </summary>
            Shift,

            /// <summary>
            /// Defines the Line
            /// </summary>
            Line,

            /// <summary>
            /// Defines the Backup
            /// </summary>
            Backup,

            /// <summary>
            /// Defines the Server
            /// </summary>
            Server,

            /// <summary>
            /// Defines the Completed
            /// </summary>
            Completed,
        }

        /// <summary>
        /// Defines the _config
        /// </summary>
        private readonly Config _config = new Config();

        //public static string ConnStr = "data source=192.168.96.17;initial catalog=ONLYOU; User ID=sa; password=onlyouolimpias;";
        //public static string SpecialConnStr = "data source=192.168.96.17;initial catalog=Ganttproj; User ID=sa; password=onlyouolimpias;";
        /// <summary>
        /// Defines the ConnStr
        /// </summary>
        public static string ConnStr = "data source=quant\\quantnew;initial catalog=Ganttproj; User=boris; password=Provajder@123;";
        /// <summary>
        /// Defines the SpecialConnStr
        /// </summary>
        public static string SpecialConnStr = "data source=quant\\quantnew;initial catalog=Ganttproj; User=boris; password=Provajder@123;";
        /// <summary>
        /// Gets or sets the ListOfModels
        /// </summary>
        public static List<JobModel> ListOfModels { get; set; }
        //private List<TreeNode> ListOfNodes { get; set; }
        //private int CurrentNavIndex { get; set; }
        /// <summary>
        /// 
        /// Gets the DateFrom
        /// </summary>
        public static DateTime DateFrom { get; private set; }
        /// <summary>
        /// Gets the DateTo
        /// </summary>
        public static DateTime DateTo { get; private set; }
        /// <summary>
        /// Gets the ShiftFrom
        /// </summary>
        public static TimeSpan ShiftFrom { get; private set; }
        /// <summary>
        /// Gets the ShiftTo
        /// </summary>
        public static TimeSpan ShiftTo { get; private set; }
        /// <summary>
        /// Gets a value indicating whether IsDvc
        /// </summary>
        public static bool IsDvc { get; private set; }
        /// <summary>
        /// Gets a value indicating whether IsRdd
        /// </summary>
        public static bool IsRdd { get; private set; }
        /// <summary>
        /// Gets a value indicating whether IsAcconto
        /// </summary>
        public static bool IsAcconto { get; private set; }
        /// <summary>
        /// Gets a value indicating whether IsSaldo
        /// </summary>
        public static bool IsSaldo { get; private set; }
        /// <summary>
        /// Gets a value indicating whether IsChiuso
        /// </summary>
        public static bool IsChiuso { get; private set; }
        /// <summary>
        /// Gets or sets a value indicating whether IsProgramare
        /// </summary>
        public static bool IsProgramare { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether IsResetJobLoader
        /// </summary>
        public static bool IsResetJobLoader { get; set; }
        /// <summary>
        /// Gets or sets the SettingsCompleted
        /// </summary>
        public static SettingsSys SettingsCompleted { get; set; }
        /// <summary>
        /// Gets the IdStateArray
        /// </summary>
        public static System.Text.StringBuilder IdStateArray { get; private set; }
        /// <summary>
        /// Gets or sets the SectorsArray from project default setting
        /// </summary>
        /// <remarks>The <see cref="System.Text.StringBuilder"/>
        /// <see cref="Properties.Settings.arrSectors"/></remarks>
        public static System.Text.StringBuilder SectorsArray { get; set; }
        /// <summary>
        /// Gets a value indicating whether IsArticleSelection
        /// </summary>
        public static bool IsArticleSelection { get; private set; }
        //console drivers
        /// <summary>
        /// The GetConsoleWindow
        /// </summary>
        /// <returns>The <see cref="IntPtr"/></returns>
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();
        /// <summary>
        /// The ShowWindow
        /// </summary>
        /// <param name="hWnd">The hWnd<see cref="IntPtr"/></param>
        /// <param name="nCmdShow">The nCmdShow<see cref="int"/></param>
        /// <returns>The <see cref="bool"/></returns>
        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        /// <summary>
        /// Defines the SwHide
        /// </summary>
        private const int SwHide = 0;
        /// <summary>
        /// 
        /// </summary>
        public static List<ArticlesUnit> _artList = new List<ArticlesUnit>();       
        /// <summary>
        /// 
        /// </summary>
        public static List<LinesUnit> _lineList = new List<LinesUnit>();
        //private const int SwShow = 5;
        /// <summary>
        /// The Main
        /// </summary>
        /// <param name="args">The args<see cref="string[]"/></param>
        [STAThread]
        private static void Main(string[] args)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Central"/> class.
        /// </summary>
        public Central()
        {
            InitializeComponent();
            this.SetBevel(false);
            DoubleBuffered = true;
        }

        /// <summary>
        /// The Menu_Load
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void Menu_Load(object sender, EventArgs e)
        {
            //pbStatus.Style = ProgressBarStyle.Marquee;
            //pbStatus.MarqueeAnimationSpeed = 10;
            lblStatus.Text = Store.Default.selDept + "/" + Store.Default.selDept;
            lblDepartment.Text = Store.Default.selDept + "/" + Store.Default.selDept;
            var arrDept = Store.Default.arrDept.Split(',');
            foreach (var dpt in arrDept)
            {
                if (dpt == string.Empty) continue;
                cbDept.Items.Add(dpt);
            }
            if (cbDept.Items.Count > 0)
            {
                cbDept.SelectedIndex = cbDept.FindString(Store.Default.selDept);
            }

            IdStateArray = new System.Text.StringBuilder();
            IsDvc = false;
            IsChiuso = false;    //3
            IsResetJobLoader = true;
            IsAcconto = true;   //2
            IsSaldo = true; //1
            cbAcconto.Checked = true;
            cbSaldo.Checked = true;
            pbReload.Image = Properties.Resources.refresh_total_32;
            IsArticleSelection = false;
            //if (Store.Default.sector == string.Empty || Store.Default.selDept == string.Empty)
            //{
            //    lblStatus.Text = "No department definition";
            //}
            //else
            //{
            //    lblStatus.Text = "Ready";
            //    lblDepartment.Text = Store.Default.selDept + "/" + Store.Default.selDept;
            //    //if (Store.Default.sector == "Confezione")
            //    //{
            //    //    Store.Default.selDept = 1;
            //    //}
            //    //else Store.Default.id_sector = 2;

            //    Store.Default.Save();
            //}
            var currentDate = DateTime.Now;
            dtpFrom.Value = new DateTime(currentDate.Year, currentDate.Month, 1);
            dtpTo.Value = dtpFrom.Value.AddMonths(1).AddDays(-1);
            DateFrom = new DateTime(dtpFrom.Value.Year, dtpFrom.Value.Month, dtpFrom.Value.Day);
            DateTo = new DateTime(dtpTo.Value.Year, dtpTo.Value.Month, dtpTo.Value.Day);
            JobModel.DateFromLast = DateFrom;
            JobModel.DateToLast = DateTo;
            pnDockBar.DoubleBuffered(true);
            pnDockBar.BackColor = Color.WhiteSmoke;
            pnDockBar.Width = 200;
            WorkflowController.ListOfRemovedOrders = new List<string>();
            CreateMenuTree();
            GetBase(null);
            if (string.IsNullOrEmpty(Store.Default.selShift)) // == string.Empty)
            {
                MessageBox.Show("Shift must be configured.", "Settings", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SettingsCompleted = SettingsSys.Shift;
                var settings = new Settings();
                settings.ShowDialog();
                settings.Dispose();
            }
            btnCarico.PerformClick();
            treeMenu.Width = 0;
            var console = GetConsoleWindow();
            ShowWindow(console, SwHide);
            pbReload.MouseEnter += delegate
            {
                pbReload.BackColor = Color.Gainsboro;
                button1.BackColor = Color.Gainsboro;
            };
            pbReload.MouseLeave += delegate
            {
                pbReload.BackColor = Color.FromArgb(235, 235, 235);
                button1.BackColor = Color.FromArgb(235, 235, 235);
            };
            SettingsCompleted = SettingsSys.Completed;
            lblRefreshTitle.Text = "Commesse in lavoro/ commesse da programmare";

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
        }

        /// <summary>
        /// The LoadShifts
        /// </summary>
        private void LoadShifts()
        {
            var q = "select starttime,endtime from shifts where shift='" + Store.Default.selShift + "'";
            using (var c = new System.Data.SqlClient.SqlConnection(SpecialConnStr))
            {
                var cmd = new System.Data.SqlClient.SqlCommand(q, c);
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

            lblShiftInfo.Text = "Turno: " + ShiftFrom.ToString() + " - " + ShiftTo.ToString();           
        }

        /// <summary>
        /// Gets or sets the ListOfHolidays
        /// </summary>
        public static List<LineHolidaysEmbeded> ListOfHolidays { get; set; }

        /// <summary>
        /// The LoadHolidays
        /// </summary>
        private void LoadHolidays()
        {
            var lst = new List<LineHolidaysEmbeded>();
            ListOfHolidays = new List<LineHolidaysEmbeded>();
            var q = "select line,hdate,month,year from holidays order by year, month," +
                "cast(SUBSTRING(line, 6, len(line)) as int)";
            using (var con = new System.Data.SqlClient.SqlConnection(SpecialConnStr))
            {
                var cmd = new System.Data.SqlClient.SqlCommand(q, con);
                con.Open();
                var dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        lst.Add(new LineHolidaysEmbeded(dr[0].ToString(),
                            dr[1].ToString(), Convert.ToInt32(dr[2]), Convert.ToInt32(dr[3])));
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
                    DateTime.TryParse(items, out var dt);
                    var holiDay = new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0);
                    //var dt = Convert.ToDateTime(items);
                    ListOfHolidays.Add(new LineHolidaysEmbeded(
                        item.Line, dt, item.Month, item.Year));
                }
            }
        }
        private void LoadArticlesUnit()
        {
            int idSect = 0;
            if (Store.Default.selDept == "Confezione B") idSect = 1;

            var query = (from art in ObjectModels.Tables.Articles
                        where art.IdSector == idSect
                        select art);

            _artList = new List<ArticlesUnit>();
            foreach (var item in query)
            {
                double.TryParse(item.Centes.ToString(), out var cent);
                double.TryParse(item.Prezzo.ToString(), out var price);
                _artList.Add(new ArticlesUnit(item.Articol, cent,price));
            }
        }
        private void LoadLinesUnit()
        {
            var query = (from lin in ObjectModels.Tables.Lines
                         where lin.Department == Store.Default.selDept
                         select lin);

            _lineList = new List<LinesUnit>();
            foreach (var item in query)
            {
                int.TryParse(item.Members.ToString(), out var cent);
                double.TryParse(item.Abatimento.ToString(), out var abat);
                _lineList.Add(new LinesUnit(item.Line, cent,  Math.Round(abat / 100, 2)));
            }
        }
        /// <summary>
        /// The GetBase
        /// </summary>
        /// <param name="info">The info<see cref="object"/></param>
        public void GetBase(object info)
        {
            Cursor = Cursors.WaitCursor;
            Config.SetGanttConn(new DataContext(_config.ReadSqlConnectionString(1)));
            Config.SetOlyConn(new DataContext(_config.ReadSqlConnectionString(1)));
            LoadShifts();
            LoadHolidays();
            AddModels(false);
            var lst = from models in ListOfModels
                      orderby Convert.ToInt32(models.Aim.Remove(0, 5)),
                      models.StartDate
                      select models;
            ListOfModels = lst.ToList();
            foreach (var itm in ListOfModels)
            {
                Console.WriteLine(itm.ToString());
            }
            Cursor = Cursors.Default;
        }

        /// <summary>
        /// The AddModels
        /// </summary>
        private void AddModels(bool updateProduction)
        {
            LoadArticlesUnit();
            LoadLinesUnit();

            var tbl = new System.Data.DataTable();
            var q = "select * from [dbo].[viewobjects] where department='" + Store.Default.selDept + "'";
            using (var con = new System.Data.SqlClient.SqlConnection(SpecialConnStr))
            {
                var cmd = new System.Data.SqlClient.SqlCommand(q, con);
                con.Open();
                var dr = cmd.ExecuteReader();
                tbl.Load(dr);
                con.Close();
                dr.Close();
            }

            ListOfModels = new List<JobModel>();
            var jb = new JobModel();
            foreach (System.Data.DataRow row in tbl.Rows)
            {
                var name = row[1].ToString();
                var aim = row[2].ToString();
                if (updateProduction)
                    jb.GetJobContinum(name, aim);

                var article = row[3].ToString();
                int.TryParse(row[4].ToString(), out var stateId);
                int.TryParse(row[5].ToString(), out var qty);
                double.TryParse(row[6].ToString(), out var qtyH);
                long.TryParse(row[7].ToString(), out var startDate);
                int.TryParse(row[8].ToString(), out var duration);
                long.TryParse(row[9].ToString(), out var endDate);
                long.TryParse(row[10].ToString(), out var dvc);
                long.TryParse(row[11].ToString(), out var rdd);
                long.TryParse(row[12].ToString(), out var startProd);
                long.TryParse(row[13].ToString(), out var endProd);
                int.TryParse(row[14].ToString(), out var qtyDaily);
                int.TryParse(row[15].ToString(), out var qtyProd);
                int.TryParse(row[16].ToString(), out var qtyOver);
                int.TryParse(row[17].ToString(), out var prodOverDays);
                long.TryParse(row[18].ToString(), out var delayTime);
                long.TryParse(row[19].ToString(), out var prodOverTime);
                bool.TryParse(row[20].ToString(), out var locked);
                int.TryParse(row[21].ToString(), out var holiday);
                bool.TryParse(row[22].ToString(), out var closedOrd);
                double.TryParse(row[23].ToString(), out var artPrice);
                bool.TryParse(row[24].ToString(), out var hasProd);
                bool.TryParse(row[25].ToString(), out var lockedProd);
                long.TryParse(row[26].ToString(), out var delayStart);
                long.TryParse(row[27].ToString(), out var delayEnd);
                bool.TryParse(row[28].ToString(), out var doneProd);
                long.TryParse(row[29].ToString(), out var lockDate);
                bool.TryParse(row[30].ToString(), out var based);
                var dept = row[31].ToString();

                var nArt = (from art in _artList
                            where art.Article == article
                            select art).SingleOrDefault();
                var nLine = (from line in _lineList
                             where line.Line == aim
                             select line).SingleOrDefault();

                var startDt = Config.MinimalDate.AddTicks(startDate);
                var endDt = Config.MinimalDate; //.AddTicks(endDate);
                var dvcDt = Config.MinimalDate.AddTicks(dvc);
                var rddDt = Config.MinimalDate.AddTicks(rdd);
                var startProdDt = Config.MinimalDate.AddTicks(startProd);
                var endProdDt = Config.MinimalDate.AddTicks(endProd);
                var delayStartDt = Config.MinimalDate.AddTicks(delayStart);
                var delayEndDt = Config.MinimalDate.AddTicks(delayEnd);

                if (updateProduction)
                {
                    using (var context = new DataContext(SpecialConnStr))
                    {
                        // delete existing records
                        context.ExecuteCommand("update produzione set " +
                            "qtyH={0},price={1},members={2},abatim={3} " +
                            "where commessa={4} and line={5}",
                            nArt.QtyH, nArt.Price, nLine.Memebers, (nLine.Abatimento * 100), name, aim);
                    }
                }

                var isClosed = false;
                if (closedOrd)
                {
                    isClosed = closedOrd;
                }
                else
                {
                    isClosed = qtyProd >= qty ? true : false;
                }

                //var newDailyQty = 0.0;
                //var newDuration = 0.0;
                var dq = 0; 
                var drt = 0;
                //if (nArt.QtyH != qtyH)
                //{
                //    if (!isClosed)
                //    {
                //        newDailyQty = Math.Round((nLine.Memebers * nArt.QtyH * 7.5 * nLine.Abatimento), 0);
                //        newDuration = Math.Round(Convert.ToDouble(qty) / newDailyQty, 0);
                //    }
                //    else if (isClosed && nArt.QtyH < qtyH)
                //    {
                //        newDailyQty = Math.Round((nLine.Memebers * nArt.QtyH * 7.5 * nLine.Abatimento), 0);
                //        newDuration = Math.Round(Convert.ToDouble(qty) / newDailyQty, 0);
                //    }
                //    int.TryParse(newDailyQty.ToString(), out dq);
                //    int.TryParse(newDuration.ToString(), out drt);
                //    endDt = startDt.AddDays(+newDuration);
                //}
                //else
                //{
                    endDt = Config.MinimalDate.AddTicks(endDate);
                    drt = duration;
                    dq = qtyDaily;
                //}                 
                ListOfModels.Add(new JobModel(name, aim, article, stateId, qty, qtyH, startDt, drt, endDt, dvcDt, rddDt, startProdDt, endProdDt,
                    dq, qtyProd, qtyOver, prodOverDays, delayTime, prodOverTime,
                    locked, holiday, isClosed, artPrice, hasProd, lockedProd,
                    delayStartDt, delayEndDt, doneProd, based, qtyH, artPrice,dept));

                if (pbStatus.Value < pbStatus.Maximum)
                {
                    pbStatus.Value += 1;
                }
                else
                {
                    pbStatus.Value = pbStatus.Maximum;
                }                            
                lblProgress.Text = pbStatus.Value * 100 / pbStatus.Maximum + "%";
                statusStrip1.Refresh();
            }

            //IQueryable<ObjectModels.Orders> jobModels = null;
            //if (!IsResetJobLoader)
            //{
            //    jobModels =
            //        from job in ObjectModels.Tables.Orders
            //        where job.Line != null && job.Cantitate > 0 && job.DataProduzione != null
            //        && job.DataLivrare >= DateFrom && job.DataLivrare <= DateTo
            //        && job.Department == Store.Default.selDept
            //        select job;
            //}
            //else
            //{
            //    jobModels =
            //        from job in ObjectModels.Tables.Orders
            //        where job.Line != null && job.Cantitate > 0 && job.DataProduzione != null
            //        && job.Department == Store.Default.selDept
            //        select job;
            //}
            //pbStatus.Style = ProgressBarStyle.Blocks;
            //pbStatus.Maximum = jobModels.ToList().Count;
            //pbStatus.Minimum = 0;
            //pbStatus.Value = 0;
            //foreach (var job in jobModels)
            //// Pass through all orginial jobs
            //{
            //    var jobMod = new JobModel();
            //    var order = job.NrComanda;

            //    //if (!IsResetJobLoader && startDt <= DateFrom) continue; 
            //    var article =
            //        (from art in ObjectModels.Tables.Articles
            //         where art.Id == job.IdArticol
            //         select art).SingleOrDefault();
            //    if (article == null) continue;

            //    var rdd = (!string.IsNullOrEmpty(job.Rdd.ToString())) ?
            //        new DateTime(Convert.ToDateTime(job.Rdd).Year,
            //        Convert.ToDateTime(job.Rdd).Month, Convert.ToDateTime(job.Rdd).Day) : DateTime.MinValue;
            //    var dvc = (!string.IsNullOrEmpty(job.Dvc.ToString())) ?
            //        new DateTime(Convert.ToDateTime(job.Dvc).Year,
            //            Convert.ToDateTime(job.Dvc).Month, Convert.ToDateTime(job.Dvc).Day) : DateTime.MinValue;

            //    double.TryParse(article.Prezzo.ToString(), out double artPrice);    //price

            //    var capiOra =
            //        (from art in ObjectModels.Tables.Articles
            //         where art.Articol == article.Articol && art.IdSector == 1
            //         select art.Centes).ToList();
            //    var qtyH = 0.0;
            //    // Tests to see does qty per hour has some value
            //    foreach (var c in capiOra)
            //    {
            //        double.TryParse(c.ToString(), out var capi);
            //        qtyH += capi;
            //    }
            //    if (qtyH <= 0) continue;
            //    var splitList =
            //        (from splitJob in ObjectModels.Tables.ProductionSplits
            //         where splitJob.Commessa == order
            //         select splitJob).ToList();
            //    var slc = splitList.Count;
            //    if (slc <= 0)
            //    {
            //        int.TryParse(job.Carico.ToString(), out var spCarico);
            //        int.TryParse(job.Cantitate.ToString(), out var spCantitate);
            //        if (spCarico <= 0) spCarico = spCantitate;
            //        if (job.QtyInstead != null && Convert.ToBoolean(job.QtyInstead) == true) spCarico = spCantitate;
            //        if (spCarico <= 0) continue;
            //        var spLine = job.Line;  //line
            //        DateTime.TryParse(job.DataProduzione.ToString(), out var startDt);
            //        var jobStart = new DateTime(startDt.Year, startDt.Month, startDt.Day); //production start date                 
            //        if (!IsResetJobLoader && jobStart < DateFrom) continue;
            //        //double.TryParse(job.Duration.ToString(), out var jD);
            //        //var jobDuration = jD;
            //        //if (jobDuration <= 0) jobDuration = jobMod.CalculateJobDuration(spLine, spCarico, qtyH);    //production duratio                                      
            //        int.TryParse(jobMod.CalculateDailyQty(spLine, qtyH).ToString(), out var spDailyQty);
            //        var jobDuration = Math.Round(Convert.ToDouble(spCarico) / spDailyQty, 0);
            //        if (jobDuration <= 0 || double.IsNaN(jobDuration) || double.IsInfinity(jobDuration)) continue;
            //        var jobEnd = jobStart.AddDays(+jobDuration);
            //        var production = jobMod.GetJobContinum(order, spLine);
            //        DateTime.TryParse(production[0].ToString(), out var prodStart);
            //        DateTime.TryParse(production[1].ToString(), out var prodEnd);
            //        int.TryParse(production[2].ToString(), out var prodQty);
            //        int.TryParse(production[3].ToString(), out var prodOverDays);
            //        int.TryParse(production[4].ToString(), out var prodOverQty);
            //        TimeSpan.TryParse(production[5].ToString(), out var prodOverTs);
            //        TimeSpan.TryParse(production[6].ToString(), out var delayTs);
            //        if (prodQty <= 0) delayTs = new TimeSpan(0, 0, 0, 0, 0);
            //        var prodOverDuration = prodEnd.AddDays(+prodOverDays);

            //        //Tests to see lock status from database
            //        var lockStat = from locked in ObjectModels.Tables.OrderLocks
            //                       where locked.Commessa == job.NrComanda &&
            //                       locked.Line == job.Line
            //                       select locked;

            //        //var lockList = lockStat.ToList();
            //        var isLocked = false;
            //        var isLockedProd = false;
            //        //var lockDate = Config.MinimalDate;
            //        //foreach (var item in lockList)
            //        //{
            //        //    isLocked = item.Islock;
            //        //    isLockedProd = item.Islockprod;
            //        //    lockDate = item.Lockdate;
            //        //}
            //        //var lockDays = 0;
            //        var delayStart = Config.MinimalDate;
            //        var delayEnd = Config.MinimalDate;
            //        //if (isLocked && isLockedProd && lockDate > Config.MinimalDate;)
            //        //{
            //        //    lockDays = DateTime.Now.Date.Subtract(lockDate.Date).Days;
            //        //    delayStart = jobEnd;
            //        //    delayEnd = jobEnd.AddDays(+lockDays);
            //        //    delayTs = new TimeSpan(lockDays, 0, 0, 0, 0);

            //        //    var si = JobModel.SkipDateRange(delayStart, delayEnd, spLine);
            //        //    delayTs = new TimeSpan(delayTs.Days + si, 23, 59, 59);
            //        //    delayEnd = delayEnd.AddDays(si);
            //        //}
            //        ////Tests to see manual or auto closed parameter to recalculate job endDate
            //        //var closeStat = (from closes in ObjectModels.Tables.OrderCloses
            //        //                 where closes.Commessa == job.NrComanda
            //        //                 && closes.Line == job.Line
            //        //                 select closes).ToList();
            //        var isClosed = false;
            //        var hasProduction = false;
            //        //if (closeStat.Count > 0) isClosed = true;
            //        //if (prodQty > 0) hasProduction = true;
            //        var prodDone = false;
            //        //if (prodQty == spCarico) prodDone = true;

            //        ListOfModels.Add(new JobModel(order, spLine, article.Articol, job.IdStare,
            //            spCarico, qtyH,
            //            jobStart,
            //            Convert.ToInt32(jobDuration),
            //            jobEnd, dvc, rdd, prodStart, prodEnd,
            //            spDailyQty,
            //            prodQty, prodOverQty, prodOverDays,
            //            delayTs.Ticks,
            //            prodOverTs.Ticks, isLocked, 0, isClosed,
            //            artPrice, hasProduction, isLockedProd,
            //            delayStart, delayEnd, prodDone,true,qtyH,artPrice));
            //    }
            //    else
            //    // Recalculate orders based on split
            //    {
            //        foreach (var sJob in splitList)
            //        {
            //            var spCarico = sJob.Qty;
            //            if (spCarico <= 0) continue;
            //            var spOrder = sJob.Commessa;
            //            var spLine = sJob.Line;
            //            var start = (DateTime)sJob.Startdate;
            //            var jobStart = new DateTime(start.Year, start.Month, start.Day);
            //            var spDuration = jobMod.CalculateJobDuration(spLine, spCarico, qtyH);
            //            if (double.IsNaN(spDuration) || double.IsInfinity(spDuration)) continue;
            //            var jobEnd = jobStart.AddDays(+spDuration);
            //            if (spDuration == 0) spDuration = 1;
            //            var spDailyQty = Convert.ToInt32(spCarico / spDuration);
            //            var production = jobMod.GetJobContinum(spOrder, spLine);
            //            DateTime.TryParse(production[0].ToString(), out var prodStart);
            //            DateTime.TryParse(production[1].ToString(), out var prodEnd);
            //            int.TryParse(production[2].ToString(), out var prodQty);
            //            int.TryParse(production[3].ToString(), out var prodOverDays);
            //            int.TryParse(production[4].ToString(), out var prodOverQty);
            //            TimeSpan.TryParse(production[5].ToString(), out var prodOverTs);
            //            TimeSpan.TryParse(production[6].ToString(), out var delayTs);
            //            if (prodQty <= 0) delayTs = new TimeSpan(0, 0, 0, 0, 0);
            //            var prodOverDuration = prodEnd.AddDays(+prodOverDays);
            //            var lockStat = from locked in ObjectModels.Tables.OrderLocks
            //                           where locked.Commessa == job.NrComanda &&
            //                           locked.Line == job.Line
            //                           select locked;
            //            var lockList = lockStat.ToList();
            //            var isLocked = false;
            //            var isLockedProd = false;
            //            var lockDate = DateTime.MinValue;
            //            foreach (var item in lockList)
            //            {
            //                isLocked = item.Islock;
            //                isLockedProd = item.Islockprod;
            //                lockDate = item.Lockdate;
            //            }
            //            var lockDays = 0;
            //            var delayStart = DateTime.MinValue;
            //            var delayEnd = DateTime.MinValue;
            //            if (isLocked && isLockedProd && lockDate > DateTime.MinValue)
            //            {
            //                lockDays = DateTime.Now.Date.Subtract(lockDate.Date).Days;
            //                delayStart = jobEnd;
            //                delayEnd = jobEnd.AddDays(+lockDays);
            //                delayTs = new TimeSpan(lockDays, 0, 0, 0, 0);
            //                var si = JobModel.SkipWeekendRange(delayStart, delayEnd);
            //                delayTs = new TimeSpan(delayTs.Days + si, 0, 0, 0, 0);
            //                delayEnd = delayEnd.AddDays(si);
            //            }
            //            //Tests to see manual or auto closed parameter to recalculate job endDate
            //            var closeStat = (from closes in ObjectModels.Tables.OrderCloses
            //                             where closes.Commessa == job.NrComanda
            //                             && closes.Line == job.Line
            //                             select closes).ToList();
            //            var isClosed = false;
            //            if (closeStat.Count > 0) isClosed = true;
            //            var hasProduction = false;
            //            if (prodQty > 0) hasProduction = true;
            //            var prodDone = false;
            //            if (prodQty == spCarico) prodDone = true;

            //            ListOfModels.Add(new JobModel(spOrder, spLine, article.Articol, job.IdStare,
            //                spCarico, qtyH,
            //                jobStart, Convert.ToInt32(spDuration), jobEnd,
            //                dvc, rdd, prodStart, prodEnd,
            //                spDailyQty,
            //                prodQty, prodOverQty, prodOverDays,
            //                delayTs.Ticks, prodOverTs.Ticks,
            //                isLocked, 0, isClosed, artPrice, hasProduction, isLockedProd,
            //                delayStart, delayEnd, prodDone,false,qtyH,artPrice));
            //        }
            //    }
            //}

            //var q = "insert into objects (ordername,aim,article,stateid,loadedqty,qtyh,startdate,duration,enddate,dvc,rdd,startprod,endprod,dailyprod,prodqty, " +
            //   "overqty,prodoverdays,delayts,prodoverts,locked,holiday,closedord,artprice,hasprod,lockedprod,delaystart,delayend,doneprod,base) values " +
            //   "(@param1,@param2,@param3,@param4,@param5,@param6,@param7,@param8,@param9,@param10," +
            //   "@param11,@param12,@param13,@param14,@param15,@param16,@param17,@param18,@param19," +
            //   "@param20,@param21,@param22,@param23,@param24,@param25,@param26,@param27,@param28,@param29)";

            //foreach (var item in ListOfModels)
            //{
            //    using (var con = new System.Data.SqlClient.SqlConnection(SpecialConnStr))
            //    {
            //        var dStart = item.DelayStartDate;
            //        var dEnd = item.DelayEndDate;

            //        if (item.DelayStartDate == DateTime.MinValue)
            //        {
            //            dStart = DateTime.MinValue;
            //            dEnd = DateTime.MinValue;
            //        }
            //        var cmd = new System.Data.SqlClient.SqlCommand(q, con);
            //        cmd.Parameters.Add("@param1", System.Data.SqlDbType.NVarChar).Value = item.Name;
            //        cmd.Parameters.Add("@param2", System.Data.SqlDbType.NVarChar).Value = item.Aim;
            //        cmd.Parameters.Add("@param3", System.Data.SqlDbType.NVarChar).Value = item.Article;
            //        cmd.Parameters.Add("@param4", System.Data.SqlDbType.Int).Value = item.StateId;
            //        cmd.Parameters.Add("@param5", System.Data.SqlDbType.Int).Value = item.LoadedQty;
            //        cmd.Parameters.Add("@param6", System.Data.SqlDbType.Float).Value = item.QtyH;
            //        cmd.Parameters.Add("@param7", System.Data.SqlDbType.BigInt).Value = item.StartDate.Subtract(Config.MinimalDate).Ticks;
            //        cmd.Parameters.Add("@param8", System.Data.SqlDbType.Int).Value = item.Duration;
            //        cmd.Parameters.Add("@param9", System.Data.SqlDbType.BigInt).Value = item.EndDate.Subtract(Config.MinimalDate).Ticks;
            //        cmd.Parameters.Add("@param10", System.Data.SqlDbType.BigInt).Value = item.Dvc.Subtract(Config.MinimalDate).Ticks;
            //        cmd.Parameters.Add("@param11", System.Data.SqlDbType.BigInt).Value = item.Rdd.Subtract(Config.MinimalDate).Ticks;
            //        cmd.Parameters.Add("@param12", System.Data.SqlDbType.BigInt).Value = item.ProductionStartDate.Subtract(Config.MinimalDate).Ticks;
            //        cmd.Parameters.Add("@param13", System.Data.SqlDbType.BigInt).Value = item.ProductionEndDate.Subtract(Config.MinimalDate).Ticks;
            //        cmd.Parameters.Add("@param14", System.Data.SqlDbType.Int).Value = item.DailyProd;
            //        cmd.Parameters.Add("@param15", System.Data.SqlDbType.Int).Value = item.ProdQty;
            //        cmd.Parameters.Add("@param16", System.Data.SqlDbType.Int).Value = item.OverQty;
            //        cmd.Parameters.Add("@param17", System.Data.SqlDbType.Int).Value = item.ProdOverDays;
            //        cmd.Parameters.Add("@param18", System.Data.SqlDbType.BigInt).Value = item.DelayTime;
            //        cmd.Parameters.Add("@param19", System.Data.SqlDbType.BigInt).Value = item.ProdOverTime;
            //        cmd.Parameters.Add("@param20", System.Data.SqlDbType.Bit).Value = item.Locked;
            //        cmd.Parameters.Add("@param21", System.Data.SqlDbType.Int).Value = item.HolidayRange;
            //        cmd.Parameters.Add("@param22", System.Data.SqlDbType.Bit).Value = item.ClosedByUser;
            //        cmd.Parameters.Add("@param23", System.Data.SqlDbType.Float).Value = item.ArtPrice;
            //        cmd.Parameters.Add("@param24", System.Data.SqlDbType.Bit).Value = item.HasProduction;
            //        cmd.Parameters.Add("@param25", System.Data.SqlDbType.Bit).Value = item.IsLockedProduction;
            //        cmd.Parameters.Add("@param26", System.Data.SqlDbType.BigInt).Value = dStart.Subtract(Config.MinimalDate).Ticks;
            //        cmd.Parameters.Add("@param27", System.Data.SqlDbType.BigInt).Value = dEnd.Subtract(Config.MinimalDate).Ticks;
            //        cmd.Parameters.Add("@param28", System.Data.SqlDbType.Bit).Value = item.ProductionDone;
            //        cmd.Parameters.Add("@param29", System.Data.SqlDbType.Bit).Value = item.IsBase;

            //        con.Open();
            //        cmd.ExecuteNonQuery();
            //        con.Close();
            //    }
            //}

            pbStatus.Value = 0;
            lblProgress.Text = "";
            statusStrip1.Refresh();
        }

        /// <summary>
        /// The CreateMenuTree
        /// </summary>
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

            treeMenu.BeginUpdate();
            treeMenu.Nodes.Add(root);
            treeMenu.EndUpdate();

            treeMenu.AfterSelect += (sender, eventArgs) =>
            {
                //LoadingInfo.InfoText = "Please wait...";
                //LoadingInfo.ShowLoading();

                pnNavi.Enabled = false;
                foreach (var f in MdiChildren)
                {
                    f.Close();
                    f.Dispose();
                }
                ClearReflectedHandlers();

                lblNode.Text = "";
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

                    var frm = new LoadingJobController
                    {
                        MdiParent = this
                    };

                    frm.Show();
                    frm.WindowState = FormWindowState.Minimized;
                    frm.WindowState = FormWindowState.Maximized;

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

                        GetBase(null);
                        frm.LoadCaricoLavoro();
                    };
                    lblRefreshGlobal.Click += (se, e) =>
                    {
                        if (IsResetJobLoader)
                        {
                            ResetStateFilters();
                            IsResetJobLoader = false;
                        }
                        GetBase(null);
                        frm.LoadCaricoLavoro();
                        pnReload.Visible = false;
                        pbReload.Image = Properties.Resources.reset_total_32;
                        lblRefreshTitle.Text = "Commesse sul selezione data";
                    };
                    lblResetGlobal.Click += (se, e) =>
                    {
                        IsResetJobLoader = true;
                        IsAcconto = true;
                        IsSaldo = true;
                        cbAcconto.Checked = true;
                        cbSaldo.Checked = true;
                        GetBase(null);
                        frm.LoadCaricoLavoro();
                        pnReload.Visible = false;
                        pbReload.Image = Properties.Resources.refresh_total_32;
                        lblRefreshTitle.Text = "Commesse in lavoro/ commesse da programmare";
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
                    cbArticle.Click += (se, e) =>
                    {
                        IsArticleSelection = !IsArticleSelection;
                    };
                    btnCarico.Text = "Carico lavoro     ►";
                    btnCarico.BackColor = Color.Yellow;
                    //btnCarico.FlatStyle = FlatStyle.Standard;
                }

                if (treeMenu.SelectedNode == node2)
                {
                    if (IsResetJobLoader)
                    {
                        ResetStateFilters();
                    }

                    var frm = new WorkflowController();
                    frm.MdiParent = this;
                    frm.Show();
                    frm.WindowState = FormWindowState.Minimized;
                    frm.WindowState = FormWindowState.Maximized;

                    pbReload.Click += (se, e) =>
                    {
                        //GetBase(null);
                        frm.LoadDataWithDateChange();
                    };
                    lblRefreshGlobal.Click += (se, e) =>
                    {
                        IsResetJobLoader = false;
                        //GetBase(null);
                        frm.LoadDataWithDateChange();
                        pnReload.Visible = false;
                        pbReload.Image = Properties.Resources.reset_total_32;
                        lblRefreshTitle.Text = "Commesse sul selezione data";
                    };
                    lblResetGlobal.Click += (se, e) =>
                    {
                        IsResetJobLoader = true;
                        //GetBase(null);
                        frm.LoadDataWithDateChange();
                        pnReload.Visible = false;
                        pbReload.Image = Properties.Resources.refresh_total_32;
                        lblRefreshTitle.Text = "Commesse in lavoro/ commesse da programmare";
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
                    cbArticle.Click += (se, e) =>
                    {
                        //LoadingInfo.ShowLoading();
                        IsArticleSelection = !IsArticleSelection;
                        frm.AddTimelineObjects();
                        //LoadingInfo.CloseLoading();
                    };
                    btnProduzioneGantt.Text = "Produzione gantt     ►";
                    btnProduzioneGantt.BackColor = Color.Yellow;
                    //btnProduzioneGantt.FlatStyle = FlatStyle.Standard;
                }

                if (treeMenu.SelectedNode == node3)
                {
                    if (IsResetJobLoader)
                    {
                        ResetStateFilters();
                    }
                    var frm = new Produzione();
                    frm.MdiParent = this;
                    frm.Show();
                    frm.WindowState = FormWindowState.Minimized;
                    frm.WindowState = FormWindowState.Maximized;
                    btnProduzione.Text = "Produzione     ►";
                    btnProduzione.BackColor = Color.Yellow;
                    //btnProduzione.FlatStyle = FlatStyle.Flat;
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

                    var frm = new Fatturato();
                    frm.MdiParent = this;
                    frm.Show();
                    frm.WindowState = FormWindowState.Minimized;
                    frm.WindowState = FormWindowState.Maximized;
                    btnFatturato.Text = "Fatturato     ►";
                    btnFatturato.BackColor = Color.Yellow;
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
                    frm.MdiParent = this;
                    frm.Show();
                    frm.WindowState = FormWindowState.Minimized;
                    frm.WindowState = FormWindowState.Maximized;
                    frm.LoadReportByDate(!IsResetJobLoader);
                    btnDiffetato.Text = "Diffetato     ►";
                    btnDiffetato.BackColor = Color.Yellow;
                    //btnDiffetato.FlatStyle = FlatStyle.Flat;
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
                        lblRefreshTitle.Text = "Commesse sul selezione data";
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
                        lblRefreshTitle.Text = "Commesse in lavoro/ commesse da programmare";
                    };
                }
                if (treeMenu.SelectedNode == node6)
                {
                    if (IsResetJobLoader) ResetStateFilters();

                    var frm = new Respinte();
                    frm.MdiParent = this;
                    frm.Show();
                    frm.WindowState = FormWindowState.Minimized;
                    frm.WindowState = FormWindowState.Maximized;
                    frm.LoadReportByDate(!IsResetJobLoader);
                    btnRespinte.Text = "Respinte     ►";
                    btnRespinte.BackColor = Color.Yellow;
                    //btnRespinte.FlatStyle = FlatStyle.Flat;

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
                        lblRefreshTitle.Text = "Commesse sul selezione data";
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
                        lblRefreshTitle.Text = "Commesse in lavoro/ commesse da programmare";
                    };
                }
                if (treeMenu.SelectedNode == node7)
                {
                    if (IsResetJobLoader) ResetStateFilters();
                    SuspendLayout();
                    var frm = new Mensile("mens")
                    {
                        MdiParent = this
                    };
                    frm.Show();
                    frm.WindowState = FormWindowState.Minimized;
                    frm.WindowState = FormWindowState.Maximized;
                    frm.LoadDataMensile();
                    btnMensile.Text = "Produzione Mensile    ►";
                    btnMensile.BackColor = Color.Yellow;
                    ResumeLayout(true);
                    //btnMensile.FlatStyle = FlatStyle.Flat;
                    btnExcel.Click += (se, e) =>
                    {
                        frm.ExportToExcel();
                    };
                    pbPrint.Click += (se, e) =>
                    {
                        frm.PrintGrid();
                    };
                }
                if (treeMenu.SelectedNode == node8)
                {
                    if (IsResetJobLoader) ResetStateFilters();
                    SuspendLayout();
                    var frm = new Mensile("eff")
                    {
                        MdiParent = this
                    };
                    frm.Show();
                    frm.WindowState = FormWindowState.Minimized;
                    frm.WindowState = FormWindowState.Maximized;
                    frm.LoadEff();
                    btnEffizLinea.Text = "Effizienza/linea    ►";
                    btnEffizLinea.BackColor = Color.Yellow;
                    //btnEffizLinea.FlatStyle = FlatStyle.Flat;
                    ResumeLayout(true);
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

                    var frm = new LineGraph
                    {
                        MdiParent = this
                    };
                    frm.Show();
                    frm.WindowState = FormWindowState.Minimized;
                    frm.WindowState = FormWindowState.Maximized;
                    frm.LoadGraph();
                    btnGraphEffLinea.Text = "Grafico di efficienza/linea ►";
                    btnGraphEffLinea.BackColor = Color.Yellow;
                    //btnGraphEffLinea.FlatStyle = FlatStyle.Flat;
                }
                if (treeMenu.SelectedNode == node10)
                {
                    if (IsResetJobLoader) ResetStateFilters();

                    var frm = new FatturatoLinea
                    {
                        MdiParent = this
                    };
                    frm.Show();
                    frm.WindowState = FormWindowState.Minimized;
                    frm.WindowState = FormWindowState.Maximized;
                    frm.LoadData();
                    btnFatturatoLinea.Text = "Fatturato per Linea  ►";
                    btnFatturatoLinea.BackColor = Color.Yellow;
                    //btnGraphEffLinea.FlatStyle = FlatStyle.Flat;
                    btnExcel.Click += (se, e) =>
                    {
                        
                    };
                    pbPrint.Click += (se, e) =>
                    {
                        
                    };
                }

                //sets the selection title
                foreach (char c in "System\\" + treeMenu.SelectedNode.Text)
                {
                    lblNode.Text += c.ToString();
                    System.Threading.Thread.Sleep(2);
                    lblNode.Refresh();
                }

                pnNavi.Enabled = true;
                treeMenu.Enabled = true;
                // Tests to see does user select node from treeMenu
                if (eventArgs.Action != TreeViewAction.Unknown) return;
                if (!_fromNavigation)
                {
                    if (!listBox1.Items.Contains(treeMenu.SelectedNode))
                        listBox1.Items.Add(treeMenu.SelectedNode);

                    listBox1.SelectedIndex = listBox1.Items.Count - 1;
                }
                btnBack.Enabled = true;
            };
            treeMenu.BeginUpdate();
            treeMenu.CollapseAll();
            treeMenu.BeforeExpand += CheckForCheckedChildrenHandler;
            treeMenu.ExpandAll();
            treeMenu.BeforeExpand -= CheckForCheckedChildrenHandler;
            treeMenu.EndUpdate();
       
        }

        /// <summary>
        /// The ResetStateFilters
        /// </summary>
        private void ResetStateFilters()
        {
            cbAcconto.Checked = false;
            cbSaldo.Checked = false;
            cbChiuso.Checked = false;
            //cbProgrammare.Checked = false;
            IsAcconto = false;
            IsSaldo = false;
            IsChiuso = false;
        }

        /// <summary>
        /// The CheckForCheckedChildrenHandler
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="TreeViewCancelEventArgs"/></param>
        private static void CheckForCheckedChildrenHandler(object sender,
            TreeViewCancelEventArgs e)
        {
            if (!HasCheckedChildNodes(e.Node)) e.Cancel = true;
        }

        /// <summary>
        /// The HasCheckedChildNodes
        /// </summary>
        /// <param name="node">The node<see cref="TreeNode"/></param>
        /// <returns>The <see cref="bool"/></returns>
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

        /// <summary>
        /// The pictureBox1_Click
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
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
                pnDockBar.Width = 200;
                ResumeLayout(true);
            }
        }

        /// <summary>
        /// The pictureBox2_Click
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            var set = new Settings();
            set.FormClosing += (se, ee) =>
            {
                lblStatus.Text = Store.Default.selDept + "/" + Store.Default.selDept;
                lblDepartment.Text = Store.Default.selDept + "/" + Store.Default.selDept;
            };
            statusStrip1.Refresh();
            ShowPopup(set);
        }

        /// <summary>
        /// The dtpFrom_ValueChanged
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            DateFrom = new DateTime(dtpFrom.Value.Year, dtpFrom.Value.Month, dtpFrom.Value.Day);
        }

        /// <summary>
        /// The dtpTo_ValueChanged
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            DateTo = new DateTime(dtpTo.Value.Year, dtpTo.Value.Month, dtpTo.Value.Day);
        }

        /// <summary>
        /// The ShowPopup
        /// </summary>
        /// <param name="frm">The frm<see cref="Form"/></param>
        private void ShowPopup(Form frm)
        {
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.ShowDialog(this);
        }

        /// <summary>
        /// The btnSync_ButtonClick
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void btnSync_ButtonClick(object sender, EventArgs e)
        {
            if (Store.Default.backupData)
            {
                BackUpDataOnSync();
            }
            try
            {
                Cursor = Cursors.WaitCursor;
                Config.SetGanttConn(new DataContext(_config.ReadSqlConnectionString(1)));
                Config.SetOlyConn(new DataContext(_config.ReadSqlConnectionString(2)));
                LoadShifts();
                LoadHolidays();
                AddModels(true);
                var lst = from models in ListOfModels
                          orderby Convert.ToInt32(models.Aim.Remove(0, 5)),
                          models.StartDate
                          select models;
                ListOfModels = lst.ToList();
                foreach (var itm in ListOfModels)
                {
                    Console.WriteLine(itm.ToString());
                }
                Cursor = Cursors.Default;
            }
            catch
            {
                Cursor = Cursors.Default;
                MessageBox.Show("Sectors potential error.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// The btnFilters_Click
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void btnFilters_Click(object sender, EventArgs e)
        {
            var flag = pnChecks.Visible;
            pnChecks.Visible = !flag;
        }

        /// <summary>
        /// Defines the WmParentnotify
        /// </summary>
        private const int WmParentnotify = 0x210;

        /// <summary>
        /// Defines the WmLbuttondown
        /// </summary>
        private const int WmLbuttondown = 0x201;

        /// <summary>
        /// The WndProc
        /// </summary>
        /// <param name="m">The m<see cref="Message"/></param>
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
            base.WndProc(ref m);
        }

        /// <summary>
        /// The ClearReflectedHandlers
        /// </summary>
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
                    case PictureBox pb when pb.Name != "pbMenu" && pb.Name != "pbSettings":
                        {
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
                }           
        }

        /// <summary>
        /// The pbMenu_MouseEnter
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void pbMenu_MouseEnter(object sender, EventArgs e)
        {
            var btn = (PictureBox)sender;
            btn.BackColor = Color.Gainsboro;
        }

        /// <summary>
        /// The pbMenu_MouseLeave
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void pbMenu_MouseLeave(object sender, EventArgs e)
        {
            var btn = (PictureBox)sender;
            btn.BackColor = Color.FromArgb(235, 235, 235);
        }

        /// <summary>
        /// The btnProgram_Click
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void btnProgram_Click(object sender, EventArgs e)
        {
            treeMenu.SelectedNode = treeMenu.Nodes[0];
            treeMenu.Select();

            foreach (Control c in pnDockBar.Controls)
            {
                if (c is Button b && b.Tag.ToString() == 1.ToString() && b.Name != "btnProgram")
                {
                    c.Visible = !c.Visible;

                    if (!c.Visible)
                    {
                        btnProgram.Text = "► Produzione gantt";
                    }
                    else
                    {
                        btnProgram.Text = "▼ Produzione gantt";
                        if (c.Visible)
                        {
                            btnCarico.PerformClick();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// The btnCarico_Click
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void btnCarico_Click(object sender, EventArgs e)
        {
            _fromNavigation = false;
            treeMenu.SelectedNode = treeMenu.Nodes[0].Nodes[0];
            treeMenu.Select();
        }

        /// <summary>
        /// The btnProduzioneGantt_Click
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void btnProduzioneGantt_Click(object sender, EventArgs e)
        {
            _fromNavigation = false;
            treeMenu.SelectedNode = treeMenu.Nodes[0].Nodes[1];
            treeMenu.Select();
        }

        /// <summary>
        /// The BtnProduzione_Click
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void BtnProduzione_Click(object sender, EventArgs e)
        {
            _fromNavigation = false;
            treeMenu.SelectedNode = treeMenu.Nodes[0].Nodes[2];
            treeMenu.Select();
        }

        /// <summary>
        /// The BtnFatturato_Click
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void BtnFatturato_Click(object sender, EventArgs e)
        {
            _fromNavigation = false;
            treeMenu.SelectedNode = treeMenu.Nodes[0].Nodes[3];
            treeMenu.Select();
        }

        /// <summary>
        /// The BtnDiffetato_Click
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void BtnDiffetato_Click(object sender, EventArgs e)
        {
            _fromNavigation = false;
            treeMenu.SelectedNode = treeMenu.Nodes[0].Nodes[4];
            treeMenu.Select();
        }

        /// <summary>
        /// The BtnRespinte_Click
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
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

        /// <summary>
        /// The ResetMenuCommands
        /// </summary>
        private void ResetMenuCommands()
        {
            btnCarico.Text = "Carico lavoro";
            btnCarico.BackColor = Color.WhiteSmoke;
            btnCarico.FlatStyle = FlatStyle.Standard;

            btnProduzioneGantt.Text = "Produzione gantt";
            btnProduzioneGantt.BackColor = Color.WhiteSmoke;
            btnProduzioneGantt.FlatStyle = FlatStyle.Standard;

            btnProduzione.Text = "Produzione";
            btnProduzione.BackColor = Color.WhiteSmoke;
            btnProduzione.FlatStyle = FlatStyle.Standard;

            btnFatturato.Text = "Fatturato";
            btnFatturato.BackColor = Color.WhiteSmoke;
            btnFatturato.FlatStyle = FlatStyle.Standard;

            btnDiffetato.Text = "Diffetato";
            btnDiffetato.BackColor = Color.WhiteSmoke;
            btnDiffetato.FlatStyle = FlatStyle.Standard;

            btnRespinte.Text = "Respinte";
            btnRespinte.BackColor = Color.WhiteSmoke;
            btnRespinte.FlatStyle = FlatStyle.Standard;

            btnMensile.Text = "Produzione Mensile";
            btnMensile.BackColor = Color.WhiteSmoke;
            btnMensile.FlatStyle = FlatStyle.Standard;

            btnEffizLinea.Text = "Effizienza/linea";
            btnEffizLinea.BackColor = Color.WhiteSmoke;
            btnEffizLinea.FlatStyle = FlatStyle.Standard;

            btnGraphEffLinea.Text = "Grafico di efficienza/linea";
            btnGraphEffLinea.BackColor = Color.WhiteSmoke;
            btnGraphEffLinea.FlatStyle = FlatStyle.Standard;

            btnFatturatoLinea.Text = "Fatturato per linea";
            btnFatturatoLinea.BackColor = Color.WhiteSmoke;
            btnFatturatoLinea.FlatStyle = FlatStyle.Standard;
        }

        /// <summary>
        /// The BackUpDataOnSync
        /// </summary>
        private void BackUpDataOnSync()
        {
            var xEle = new XElement("ProduzioneGantt",
              from models in ListOfModels
              select new XElement("Model",
              new XAttribute("Order", models.Name),
              new XElement("QtyH", models.QtyH),
              //new XElement("Persons", models.MembersCount),
              new XElement("StartDate", models.StartDate),
              //new XElement("Abatimento", models.BreakdownEff),
              new XElement("Duration", models.Duration),
              new XElement("EndDate", models.EndDate),
              new XElement("LoadQty", models.LoadedQty),
              //new XElement("Delay", models.Delay),
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

        /// <summary>
        /// The button1_Click
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void button1_Click(object sender, EventArgs e)
        {
            pnReload.Visible = true;
            pnReload.Focus();
        }

        /// <summary>
        /// The pnReload_Leave
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void pnReload_Leave(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Defines the _geometry
        /// </summary>
        private Geometry _geometry = new Geometry();

        /// <summary>
        /// The btnReloadItem_Paint
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="PaintEventArgs"/></param>
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

        /// <summary>
        /// The pnReload_Paint
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="PaintEventArgs"/></param>
        private void pnReload_Paint(object sender, PaintEventArgs e)
        {
            using (GraphicsPath path = _geometry.RoundedRectanglePath(new Rectangle(0, 0, pnReload.Width, pnReload.Height), 10))
            {
                SmoothingMode old = e.Graphics.SmoothingMode;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.DrawPath(new Pen(Brushes.DimGray, 1), _geometry.RoundedRectanglePath(new Rectangle(-1, -1, pnReload.Width, pnReload.Height), 10));
                e.Graphics.FillPath(new SolidBrush(Color.FromArgb(50, 255, 255, 255)), path);

                pnReload.Region = new Region(path);
                e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            }
        }

        /// <summary>
        /// The label11_Paint
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="PaintEventArgs"/></param>
        private void label11_Paint(object sender, PaintEventArgs e)
        {

            var btn = (Label)sender;

            using (GraphicsPath path = _geometry.RoundedRectanglePath(new Rectangle(0, 0, btn.Width, btn.Height), 30))
            {
                SmoothingMode old = e.Graphics.SmoothingMode;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.DrawPath(new Pen(Brushes.DimGray, 1), _geometry.RoundedRectanglePath(new Rectangle(-1, -1, btn.Width, btn.Height), 30));

                btn.Region = new Region(path);
                e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            }
        }

        /// <summary>
        /// The btnResetItem_MouseEnter
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void btnResetItem_MouseEnter(object sender, EventArgs e)
        {
            lblResetGlobal.BackColor = Color.Gainsboro;
        }

        /// <summary>
        /// The btnResetItem_MouseLeave
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void btnResetItem_MouseLeave(object sender, EventArgs e)
        {
            lblResetGlobal.BackColor = Color.White;
        }

        /// <summary>
        /// The btnReloadItem_MouseEnter
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void btnReloadItem_MouseEnter(object sender, EventArgs e)
        {
            lblRefreshGlobal.BackColor = Color.Gainsboro;
        }

        /// <summary>
        /// The btnReloadItem_MouseLeave
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void btnReloadItem_MouseLeave(object sender, EventArgs e)
        {
            lblRefreshGlobal.BackColor = Color.White;
        }

        /// <summary>
        /// The lblResetGlobal_MouseEnter
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void lblResetGlobal_MouseEnter(object sender, EventArgs e)
        {
            lblResetGlobal.BackColor = Color.Gainsboro;
        }

        /// <summary>
        /// The lblResetGlobal_MouseLeave
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void lblResetGlobal_MouseLeave(object sender, EventArgs e)
        {

            lblResetGlobal.BackColor = Color.WhiteSmoke;
        }

        /// <summary>
        /// The lblRefreshGlobal_MouseEnter
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void lblRefreshGlobal_MouseEnter(object sender, EventArgs e)
        {
            lblRefreshGlobal.BackColor = Color.Gainsboro;
        }

        /// <summary>
        /// The lblRefreshGlobal_MouseLeave
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void lblRefreshGlobal_MouseLeave(object sender, EventArgs e)
        {
            lblRefreshGlobal.BackColor = Color.WhiteSmoke;
        }

        /// <summary>
        /// The button2_Click
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void button2_Click(object sender, EventArgs e)
        {
            if (!btnCarico.Visible) return;

            btnProgram.PerformClick();

            ClearReflectedHandlers();

            var frm = new HolidaysController();
            frm.MdiParent = this;
            frm.WindowState = FormWindowState.Maximized;
            frm.Show();

            lblNode.Text = "Vacanze";
        }

        /// <summary>
        /// Defines the _fromNavigation
        /// </summary>
        internal bool _fromNavigation = false;

        /// <summary>
        /// The BtnForward_Click
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
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

            //listBox1.SelectedIndex += 1; // listBox1.SelectedIndex + 1;
            treeMenu.SelectedNode = (TreeNode)listBox1.SelectedItem;
        }

        /// <summary>
        /// The BtnBack_Click
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
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

        /// <summary>
        /// The FindLastNode
        /// </summary>
        /// <param name="x">The x<see cref="TreeNode"/></param>
        /// <returns>The <see cref="TreeNode"/></returns>
        private TreeNode FindLastNode(TreeNode x)
        {
            return x;
        }

        private void BtnGraphEffLinea_Click(object sender, EventArgs e)
        {

        }

        private void CbDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            Store.Default.selDept = cbDept.Text;
            Store.Default.Save();
        }
    }
}
