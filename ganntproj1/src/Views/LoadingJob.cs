namespace ganntproj1
{
    using ganntproj1.Models;
    using ganntproj1.Views;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Security.Cryptography;
    using System.Text;
    using System.Windows.Forms;

    /// <summary>
    /// Defines the <see cref="LoadingJob" />
    /// </summary>
    public partial class LoadingJob : Form
    {
        /// <summary>
        /// Defines the _config
        /// </summary>
        private readonly Config _config = new Config();

        /// <summary>
        /// The FillGridDelegate
        /// </summary>
        /// <param name="reader">The reader<see cref="SqlDataReader"/></param>
        private delegate void FillGridDelegate(SqlDataReader reader);

        ///// <summary>
        ///// Defines the _connection
        ///// </summary>
        //private SqlConnection _connection;

        /// <summary>
        /// Defines the _tableCarico
        /// </summary>
        private DataTable _tableCarico = new DataTable();

        /// <summary>
        /// Defines the _bs
        /// </summary>
        private BindingSource _bs = new BindingSource();

        /// <summary>
        /// Defines the _acsc
        /// </summary>
        private AutoCompleteStringCollection _acsc = new AutoCompleteStringCollection();

        /// <summary>
        /// Defines the _acscArt
        /// </summary>
        private AutoCompleteStringCollection _acscArt = new AutoCompleteStringCollection();

        /// <summary>
        /// Defines the _ascsLine
        /// </summary>
        private AutoCompleteStringCollection _ascsLine = new AutoCompleteStringCollection();

        /// <summary>
        /// Defines the _listOfAcceptedOrder
        /// </summary>
        private List<string> _listOfAcceptedOrder = new List<string>();

        /// <summary>
        /// Defines the _listOfOrdersWithNotes
        /// </summary>
        private List<string> _listOfOrdersWithNotes = new List<string>();

        /// <summary>
        /// Defines the _listOfLinesComplete
        /// </summary>
        private List<string> _listOfLinesComplete = new List<string>();

        //
        //Runtime
        //
        /// <summary>
        /// Gets or sets a value indicating whether UseSingleFilter
        /// </summary>
        public bool UseSingleFilter { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether IsUpdating
        /// </summary>
        public bool IsUpd { get; set; }

        /// <summary>
        /// Gets a value indicating whether HasOrderSelected
        /// </summary>
        public static bool HasOrderSelected { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadingJob"/> class.
        /// </summary>
        public LoadingJob(bool updMode)
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;          
            IsUpd = updMode;
            dgvReport.DoubleBuffered(true);
        }

        /// <summary>
        /// The CreateDataTable
        /// </summary>
        private void CreateDataTable()
        {
            _tableCarico = new DataTable();

            _tableCarico.Columns.Add("Flag", typeof(Image));
            _tableCarico.Columns["Flag"].DefaultValue = null;
            _tableCarico.Columns.Add("Commessa");
            _tableCarico.Columns.Add("Articol");
            _tableCarico.Columns.Add("Linea");  //3
            _tableCarico.Columns.Add("Qty");
            _tableCarico.Columns.Add("Carico"); //5
            _tableCarico.Columns.Add("Diff_t");
            _tableCarico.Columns.Add("Lans");
            _tableCarico.Columns.Add("Prod");
            _tableCarico.Columns.Add("Fin");
            _tableCarico.Columns.Add("Liv");
            _tableCarico.Columns.Add("Rdd");    //11
            _tableCarico.Columns.Add("Dvc");
            _tableCarico.Columns.Add("Minuti"); //13
            _tableCarico.Columns.Add("Giorni");
            _tableCarico.Columns.Add("Tess");
            _tableCarico.Columns.Add("Conf");
            _tableCarico.Columns.Add("Respinte");
            _tableCarico.Columns.Add("Conseg");
            _tableCarico.Columns.Add("Diff_c");
            _tableCarico.Columns.Add("Department"); //20
            _tableCarico.Columns.Add("Note");
            _tableCarico.Columns.Add("IdState");
            _tableCarico.Columns.Add("Ritardo");
            _tableCarico.Columns.Add("FlagA", typeof(Image));
            _tableCarico.Columns["FlagA"].DefaultValue = null;
            _tableCarico.Columns.Add("FlagB", typeof(string));
            //_tableCarico.Columns["FlagB"].DefaultValue = null;
            _tableCarico.Columns.Add("DataInizio Prod");
            _tableCarico.Columns.Add("DataFine Prod");
            _tableCarico.Columns.Add("CaricoTrigger"); //28
            _tableCarico.Columns.Add("TempStat"); //29
            _tableCarico.Columns.Add("orderId"); //30
            _tableCarico.Columns.Add("Ramm Tess"); //31
            _tableCarico.Columns.Add("Ramm Conf");
            _tableCarico.Columns.Add("Dft Tess");
            _tableCarico.Columns.Add("Dft Conf"); //34
            _tableCarico.Columns.Add("TempoTotStaz");
            _tableCarico.Columns.Add("RitardoMedia");   //36
            _tableCarico.Columns.Add("Prezzo"); //37
        }

        /// <summary>
        /// Defines the _lstOfListedArts
        /// </summary>
        private List<string> _lstOfListedArts = new List<string>();

        /// <summary>
        /// The OnLoad
        /// </summary>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        protected override void OnLoad(EventArgs e)
        {
            _lstOfListedArts = new List<string>();
            HasOrderSelected = false;

            LoadCaricoLavoroInternal();

            if (Central.IsProgramare)
            {
                pnArticles.Visible = true;
                lblModelsTitle.Visible = true;
                var leftObjLocation = 8;
                Geometry _geometry = new Geometry();

                foreach (var actArt in Workflow.LineActiveArticlesList)
                {
                    foreach (DataRow row in _tableCarico.Rows)
                    {
                        var getArt = Workflow.LineActiveArticlesList.Where(x => x == row[2].ToString()).SingleOrDefault();
                        if (getArt == null) continue;

                        if (row[2].ToString() != actArt.Trim()) continue;
                        var aim = row[3].ToString();
                       
                        DateTime.TryParse(row[11].ToString(), out var rdd);
                        var strRdd = rdd != DateTime.MinValue ? rdd.ToString("dd/MM") : "no rdd";
                        var strQty = row[4].ToString();
                        var strCarico = row[5].ToString();

                        int.TryParse(row[4].ToString(), out var qty);
                        int.TryParse(row[5].ToString(), out var carico);
                        double.TryParse(row[13].ToString(), out var qtyH);
                        var q = carico;
                        if (q <= 0) q = qty;
                        if (qtyH < 1.0 || q <= 0) continue;
                        var bcolor = Color.FromArgb(27, 98, 124);
                        if (carico <= 0 && qty > 0) bcolor = Color.DarkSlateBlue;                       
                        var lbl = new Label
                        {
                            Name = row[1].ToString() + "_" + row[2].ToString(),
                            BackColor = bcolor,
                            ForeColor = Color.White,
                            Width = 160,
                            Height = 65,
                            Left = leftObjLocation,
                            Top = 8
                        };
                        var name = lbl.Name.Split('_')[0];
                        var art = lbl.Name.Split('_')[1];
                        var jMod = new JobModel();
                        var dur = jMod.CalculateJobDuration(Workflow.TargetLine, qty, qtyH, Workflow.TargetDepartment);
                        lbl.Paint += (s, pe) =>
                        {
                            //draw commessa + rdd title
                            pe.Graphics.DrawString("Comm. ",
                                new Font("Tahoma", 9, FontStyle.Regular), new SolidBrush(lbl.ForeColor), 5, 5);
                            pe.Graphics.DrawString("      " + name,
                              new Font("Tahoma", 9, FontStyle.Bold), new SolidBrush(lbl.ForeColor), 25, 5);
                            pe.Graphics.DrawString("                      RDD",
                        new Font("Tahoma", 9, FontStyle.Regular), new SolidBrush(lbl.ForeColor), 25, 5);
                            //draws article
                            pe.Graphics.DrawString(art,
                                new Font("tahoma", 9, FontStyle.Bold), new SolidBrush(lbl.ForeColor), 5, 25);
                            //draws other info
                            pe.Graphics.DrawString(qty.ToString() + "-" + dur.ToString() + "gg" + "         " + strRdd,
                                new Font("tahoma", 9, FontStyle.Bold), new SolidBrush(lbl.ForeColor), 5, 45);
                            var pen = new Pen(new SolidBrush(Color.Silver), 1);
                            using (GraphicsPath path = _geometry.RoundedRectanglePath(new Rectangle(-1, -1, lbl.Width + 1, lbl.Height + 1), 5))
                            {
                                SmoothingMode old = pe.Graphics.SmoothingMode;
                                pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                                pe.Graphics.DrawPath(new Pen(Brushes.LightGray, 4), _geometry.RoundedRectanglePath(new Rectangle(-1, -1, lbl.Width, lbl.Height), 5));

                                lbl.Region = new Region(path);
                                pe.Graphics.SmoothingMode = old;
                            }
                        };
                        lbl.MouseEnter += (s, mv) =>
                        {
                            if (lbl.BackColor == Color.DarkSlateBlue)
                            {
                                lbl.BackColor = Color.LightSlateGray;
                            }
                            else
                            {
                                lbl.BackColor = Color.SteelBlue;
                            }
                            foreach (DataGridViewRow dRow in dgvReport.Rows)
                            {
                                if (dRow.Cells[1].Value.ToString() == lbl.Name.Split('_')[0])
                                    dRow.DefaultCellStyle.BackColor = Color.White;
                                else
                                    dRow.DefaultCellStyle.BackColor = Color.FromArgb(235, 235, 235);
                            }
                        };
                        lbl.MouseLeave += (s, mv) =>
                        {
                            lbl.BackColor = bcolor;
                            foreach (DataGridViewRow dRow in dgvReport.Rows)
                            {
                                dRow.DefaultCellStyle.BackColor = Color.FromArgb(235, 235, 235);
                            }
                        };
                        lbl.Click += (s, cv) =>
                        {
                            if (!IsUpd) return;
                            var byQty = false;
                            if (((Label)s).BackColor == Color.LightSlateGray)
                            {
                                byQty = true;
                            }
                            else
                            {
                                var dg = MessageBox.Show("Do you want to program by total qty?", "Produzione gantt", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                if (dg == DialogResult.Yes)
                                {
                                    byQty = true;
                                }
                            }
                            Workflow.ByQty = byQty;
                            Workflow.TargetOrder = lbl.Name.Split('_')[0];
                            Close();
                        };
                        pnArticles.Controls.Add(lbl);
                        lbl.BringToFront();
                        lbl.Refresh();
                        _lstOfListedArts.Add(art);
                        leftObjLocation += lbl.Width + 3;
                    }
                }
            }
            //dgvReport.DataBindingComplete += (s, ev) =>
            //{
              
            //};
            AddHeaderButtons();
            base.OnLoad(e);
        }

        /// <summary>
        /// The LoadCaricoLavoro
        /// </summary>
        public void LoadCaricoLavoro()
        {
            Workflow.ListOfRemovedOrders.Clear();
            LoadCaricoLavoroInternal();
        }

        /// <summary>
        /// The LoadCaricoLavoroInternal
        /// </summary>
        private void LoadCaricoLavoroInternal()
        {
            RemoveGridControls();
            _listOfAcceptedOrder = new List<string>();
            _listOfOrdersWithNotes = new List<string>();
            _listOfLinesComplete = new List<string>();
            foreach (var item in Central.ListOfModels)
                _listOfAcceptedOrder.Add(item.Name);
            foreach (var item in _lstOfSplittedOrd)
                if (_listOfAcceptedOrder.Contains(item)) _listOfAcceptedOrder.Remove(item);
            var sector = ""; // " charindex(+',' + Comenzi.department + ',', '" + Store.Default.selDept + "') > 0"		  //Store.Default.selDept;
            var from = $"{Central.DateFrom.Year}-{Central.DateFrom.Month}-{Central.DateFrom.Day}";
            var to = $"{Central.DateTo.Year}-{Central.DateTo.Month}-{Central.DateTo.Day}";
            Central.IdStateArray.Clear();
            if (!Central.IsAcconto && !Central.IsSaldo && !Central.IsChiuso)
                Central.IdStateArray.Append(",1,2,3,4,"); // new array element (4-commessa da programmare) 
            else
            {
                if (Central.IsAcconto) Central.IdStateArray.Append(",1");
                if (Central.IsSaldo) Central.IdStateArray.Append(",3");
                if (Central.IsChiuso) Central.IdStateArray.Append(",2");
                if (Central.IsProgramare) Central.IdStateArray.Append(",4");
                Central.IdStateArray.Append(",");
            }
            var stateId = " and charindex(+ ',' + cast(Comenzi.IdStare as nvarchar(20)) + ',', '"
                + Central.IdStateArray.ToString() + "') > 0";
            var queryCondition = "";
            if (!Central.IsResetJobLoader)
                queryCondition = "where Comenzi.dataLivrare between '" +
                from + "' and '" + to + "' and charindex(+',' + Comenzi.department + ',', '" + Store.Default.selDept + "') > 0 and  Comenzi.isdeleted='0'" +
                sector +  //"'" +
                stateId + " order by Comenzi.dataLivrare DESC";
            else
                queryCondition = "where charindex(+',' + Comenzi.department + ',', '" + Store.Default.selDept + "') > 0 and  Comenzi.isdeleted='0'" +
                sector + 
                stateId + " order by Comenzi.dataLansare DESC";
            if (Central.IsDvc)
                queryCondition = "where Comenzi.DVC between '" +
                from + "' and '" +
                to + "' and charindex(+',' + Comenzi.department + ',', '" + Store.Default.selDept + "') > 0 and  Comenzi.isdeleted='0'" +
                sector + 
                stateId + " order by Comenzi.DVC";
            else if (Central.IsRdd)
                queryCondition = "where Comenzi.Rdd between '" +
                from + "' and '" + to + "' and charindex(+',' + Comenzi.department + ',', '" + Store.Default.selDept + "') > 0 and  Comenzi.isdeleted='0'" +
                sector + "" +
                stateId + " order by case when Comenzi.Rdd is null then 1 else 0 end, Comenzi.Rdd";
            else if (Central.IsProgramare)
            {
                if (IsUpd)
                {
                    queryCondition = "where Comenzi.DataProduzione is null and Comenzi.department='" + Workflow.TargetDepartment + "' " +
                                      "and Comenzi.IdStare<>2 and  Comenzi.isdeleted='0' or Comenzi.Line is null and Comenzi.department='" + Workflow.TargetDepartment + "' " +
                                      "and Comenzi.IdStare<>2 and  Comenzi.isdeleted='0' or Comenzi.IdStare=4 and Comenzi.department='" + Workflow.TargetDepartment + "' " +
                                      "and Comenzi.IdStare<>2 and Comenzi.DataProduzione is null " +
                                      "and Comenzi.Line is null and Comenzi.isdeleted=0 " +
                                      "order by case when Comenzi.Rdd is null then 1 else 0 end, Comenzi.Rdd";
                }
                else
                {
                    queryCondition = "where Comenzi.DataProduzione is null and charindex(+',' + Comenzi.department + ',', '" + Store.Default.selDept + "') > 0 " +
                        "and Comenzi.IdStare<>2 and  Comenzi.isdeleted='0' or Comenzi.Line is null and charindex(+',' + Comenzi.department + ',', '" + Store.Default.selDept + "') > 0 " +
                        "and Comenzi.IdStare<>2 and  Comenzi.isdeleted='0' or Comenzi.IdStare=4 and charindex(+',' + Comenzi.department + ',', '" + Store.Default.selDept + "') > 0 " +
                        "and Comenzi.IdStare<>2 and Comenzi.DataProduzione is null " +
                        "and Comenzi.Line is null and  Comenzi.isdeleted='0' " +
                        "order by case when Comenzi.Rdd is null then 1 else 0 end, Comenzi.Rdd";
                }
            }
            if (UseSingleFilter)
            {
                queryCondition = "where Comenzi.NrComanda='" + Workflow.TargetOrder + "' "
                    + "and comenzi.line='" + Workflow.TargetLine + "' and comenzi.department='" + Workflow.TargetDepartment + "'";
            }
            var query = "select " +
                     "Comenzi.NrComanda," +      //0    
                     "Articole.Articol," +       //1    
                     "Comenzi.Line," +           //2
                     "Comenzi.Cantitate," +      //3
                     "Comenzi.Carico," +         //4    tiny
                     "Comenzi.Diff_t," +         //5    tiny
                     "Comenzi.DataLansare," +    //6    dt
                     "Comenzi.DataProduzione," + //7    dt
                     "Comenzi.DataFine," +       //8    dt
                     "Comenzi.DataLivrare," +    //9    dt
                     "Comenzi.RDD," +            //10    dt
                     "Comenzi.DVC," +            //11   dt
                     "Articole.Centes," +         //12   tiny
                     "Comenzi.GiorniLavorati as Gironi, " + //13   tiny
                     "Comenzi.Tessitura as Tess," +      //14 
                     "Comenzi.Confezione as Conf," +     //15
                     "Comenzi.Respinte," +       //16
                     "Comenzi.Consegnato as Conseg, " +     //17
                     "Comenzi.Diff_c," +         //18
                     "Comenzi.Department," +     //19
                     "Comenzi.Note," +
                     "Comenzi.IdStare," +
                     "Comenzi.CaricoTrigger, " +
                     "Comenzi.Id, " +
                     "Comenzi.ram_tess," +
                     "Comenzi.ram_conf," +
                     "Comenzi.def_tess," +
                     "Comenzi.def_conf, " +
                     "Articole.Prezzo " +
                     "from Comenzi" +
                     " inner join Articole on Comenzi.IdArticol = Articole.Id " + queryCondition;

            _lstOfSplittedOrd = new List<string>();
            _lstOfSplittedOrd = GetSplittedOrders();
            var table = new DataTable();
            using (var con = new SqlConnection(Central.ConnStr))
            {
                var command = new SqlCommand(query, con);              
                con.Open();
                var dr = command.ExecuteReader();
                table.Load(dr);
                con.Close();
                dr.Close();
                command = null;
            }          
            CreateDataTable();
            foreach (DataRow row in table.Rows)
            {
                if (!string.IsNullOrEmpty(row[20].ToString()))
                // Stores orders with note declared
                {
                    _listOfOrdersWithNotes.Add(row[0].ToString());
                }
                //store orders with lines produc tion complete into list
                //var daysToComplete = Store.Default.daysToFinish;
                //DateTime.TryParse(row[8].ToString(), out var dataFine);
                //var boundCom = DateTime.Now.AddDays(+daysToComplete);
                //var dtFine = new DateTime(dataFine.Year, dataFine.Month, dataFine.Day);
                //var dtBound = new DateTime(boundCom.Year, boundCom.Month, boundCom.Day);
                //if (dtBound == dtFine)
                //{
                //    _listOfLinesComplete.Add(row[0].ToString());
                //}
                var newRow = _tableCarico.NewRow();
                var artFixRow = _tableCarico.NewRow();
                var job = row[0].ToString();
                var line = row[2].ToString();
                var dept = row[19].ToString();
                var jobSpostStart = "";
                var jobSpostEnd = "";
                var jobModel = Central.ListOfModels.SingleOrDefault(x => x.Name == job && x.Aim == line && x.Department == dept);
                var prodStart = DateTime.MinValue;
                if (jobModel != null)
                {
                    jobSpostStart = UniParseDateTime(jobModel.StartDate.ToString());
                    jobSpostEnd = UniParseDateTime(jobModel.DelayEndDate).ToString();
                    if (jobModel.DelayEndDate == DateTime.MinValue || jobModel.DelayEndDate == Config.MinimalDate)
                    {
                        jobSpostEnd = UniParseDateTime(jobModel.EndDate);
                    }
                    prodStart = jobModel.StartDate;
                }
                newRow[0] = ReturnImageByState(row[0].ToString());
                newRow[1] = job;
                newRow[2] = row[1].ToString();
                newRow[3] = row[2].ToString(); //line

                int.TryParse(row[3].ToString(), out var qty);
                int.TryParse(row[4].ToString(), out var carico);
                int.TryParse(row[5].ToString(), out var diff);

                newRow[4] = qty;
                newRow[5] = carico;
                newRow[6] = diff;

                newRow[7] = UniParseDateTime(row[6]);
                newRow[8] = UniParseDateTime(row[7]); //prod
                newRow[9] = UniParseDateTime(row[8]); //fine
                newRow[10] = UniParseDateTime(row[9]);    //cons
                newRow[11] = UniParseDateTime(row[10]);
                newRow[12] = UniParseDateTime(row[11]);
                newRow[13] = row[12].ToString();
                newRow[14] = row[13].ToString();
                newRow[15] = row[14].ToString();    //tess
                newRow[16] = row[15].ToString();    //conf
                newRow[17] = row[16].ToString();
                newRow[18] = row[17].ToString();
                newRow[19] = row[18].ToString();
                newRow[20] = row[19].ToString();
                newRow[21] = row[20].ToString();
                newRow[22] = row[21].ToString();
                newRow[24] = ReturnImageByNote(row[0].ToString());
                //newRow[25] = row[22].ToString();
                DateTime.TryParse(row[6].ToString(), out var arrivo);
                DateTime.TryParse(row[7].ToString(), out var start);
                DateTime.TryParse(row[8].ToString(), out var end);

                if (start != DateTime.MinValue && end != DateTime.MinValue)
                {
                    var duration = end.Subtract(start).Days;
                    var strDuration = duration > 0 ? duration.ToString() : "1";
                    newRow[25] = strDuration; // ReturnImageByCompletetion(row[0].ToString());
                }
                else
                    newRow[25] = "";

                newRow[26] = jobSpostStart;
                newRow[27] = jobSpostEnd;
                newRow[28] = row[22].ToString();

                if (arrivo != DateTime.MinValue && prodStart != DateTime.MinValue)
                {
                    var duration = prodStart.Date.Subtract(arrivo.Date).Days;

                    DateTime.TryParse(row[9].ToString(), out var dataCons);
                    DateTime.TryParse(row[6].ToString(), out var dataArrivo);

                    var totDuration = dataCons.Date.Subtract(dataArrivo.Date).Days;
                    newRow[29] = duration.ToString();

                    newRow[35] = totDuration.ToString();

                    var timeDuration = end.Subtract(start).Days;
                    if (timeDuration <= 0) timeDuration = 1;
                    if (jobModel.DelayStartDate != Config.MinimalDate && jobModel.DelayEndDate != Config.MinimalDate)
                    {
                        var delayDuration = jobModel.DelayEndDate.Subtract(jobModel.DelayStartDate).Days;
                        newRow[36] = (timeDuration + delayDuration).ToString();
                    }                    
                }
                else newRow[29] = "";

                newRow[30] = row[23].ToString();
                newRow[31] = row[24].ToString();
                newRow[32] = row[25].ToString();
                newRow[33] = row[26].ToString();
                newRow[34] = row[27].ToString();
                newRow[37] = row[28].ToString();

                //calculate ritardo
                DateTime.TryParse(row[9].ToString(), out var dtCons);
                DateTime.TryParse(row[10].ToString(), out var dtRdd);
                if (dtCons != DateTime.MinValue && dtRdd != DateTime.MinValue)
                {
                    var ritardo = dtCons.Subtract(dtRdd).TotalDays;
                    var strRitardo = ritardo > 0 ? Math.Floor(ritardo).ToString() : string.Empty;
                    newRow[23] = strRitardo;
                }
                _tableCarico.Rows.Add(newRow);
            }
            _bs = new BindingSource { DataSource = _tableCarico };
            if (dgvReport.DataSource != null) dgvReport.DataSource = null;           
            dgvReport.DataSource = _bs;
            dgvReport.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvReport.Columns[0].Width = 35;
            dgvReport.Columns[0].HeaderText = "State";
            dgvReport.Columns[0].HeaderCell.Style.BackColor = Color.FromArgb(50, 52, 68);
            dgvReport.Columns["FlagA"].Width = 35;
            dgvReport.Columns["FlagA"].HeaderText = "Info";
            dgvReport.Columns["FlagA"].HeaderCell.Style.BackColor = Color.FromArgb(50, 52, 68);
            dgvReport.Columns["FlagA"].DisplayIndex = 1;
            dgvReport.Columns["FlagB"].Width = 35;
            dgvReport.Columns["FlagB"].HeaderText = "Time";
            dgvReport.Columns["FlagB"].HeaderCell.Style.BackColor = Color.FromArgb(50, 52, 68);
            dgvReport.Columns["FlagB"].DisplayIndex = 2;
            dgvReport.Columns["TempStat"].DisplayIndex = 11;
            dgvReport.Columns["TempStat"].HeaderText = "Tempo Staz\n" + string.Format("{0:#,##0}", GetTotals()[3]);
            dgvReport.Columns["TempStat"].HeaderCell.Style.BackColor = Color.FromArgb(50, 52, 68);
            dgvReport.Columns["TempStat"].Width = 60;

            dgvReport.Columns["TempoTotStaz"].DisplayIndex = 14;
            dgvReport.Columns["TempoTotStaz"].HeaderText = "Tot Staz\n" + string.Format("{0:#,##0}", GetTotals()[4]);
            dgvReport.Columns["TempoTotStaz"].HeaderCell.Style.BackColor = Color.FromArgb(50, 52, 68);

            dgvReport.Columns["RitardoMedia"].DisplayIndex = 15;
            dgvReport.Columns["RitardoMedia"].HeaderText = "Ritardo media\n" + string.Format("{0:#,##0}", GetTotals()[5]);
            dgvReport.Columns["RitardoMedia"].HeaderCell.Style.BackColor = Color.FromArgb(50, 52, 68);
            //dgvReport.Columns["TempoTotStaz"].Width = 60;

            dgvReport.Columns[1].HeaderText = "Commessa";
            dgvReport.Columns[1].Frozen = true;
            dgvReport.Columns[1].HeaderCell.Style.BackColor = Color.FromArgb(50, 52, 68);
            dgvReport.Columns[2].HeaderText = "Aricoli";
            dgvReport.Columns[2].Frozen = true;
            dgvReport.Columns[2].HeaderCell.Style.BackColor = Color.FromArgb(50, 52, 68);
            dgvReport.Columns[3].HeaderText = "Linea";
            dgvReport.Columns[3].Frozen = true;
            dgvReport.Columns[3].HeaderCell.Style.BackColor = Color.FromArgb(50, 52, 68);
            dgvReport.Columns[3].Width = 70;
            dgvReport.Columns[4].HeaderText = "Qty\n\n" + string.Format("{0:#,##0}", GetTotals()[0]);
            dgvReport.Columns[4].Width = 60;
            dgvReport.Columns[5].HeaderText = "Carico\n\n" + string.Format("{0:#,##0}", GetTotals()[1]);
            dgvReport.Columns[5].Width = 60;
            dgvReport.Columns[6].HeaderText = "Diff\n\n" + string.Format("{0:#,##0}", GetTotals()[2]);
            dgvReport.Columns[6].Width = 60;
            dgvReport.Columns[7].HeaderText = "Data Arrivo";
            dgvReport.Columns[8].HeaderText = "DataInizio Prev";
            dgvReport.Columns[9].HeaderText = "DataFine Prev";
            dgvReport.Columns[10].HeaderText = "Data Consegna";
            dgvReport.Columns[13].HeaderText = "Capi/\nOra\n" + GetTotals()[6].ToString();
            dgvReport.Columns[14].Visible = false;
            //dgvReport.Columns[16].Visible = false;
            dgvReport.Columns[22].Visible = false;
            dgvReport.Columns[26].Visible = false;
            dgvReport.Columns[27].Visible = false;
            dgvReport.Columns["Ritardo"].HeaderText = "Ritardo\n\n" + string.Format("{0:#,##0}", GetTotals()[7]);
            dgvReport.Columns["Ritardo"].HeaderCell.Style.BackColor = Color.FromArgb(50, 52, 68);
            dgvReport.Columns["CaricoTrigger"].Visible = false;
            dgvReport.Columns[26].DisplayIndex = 11;
            dgvReport.Columns[27].DisplayIndex = 15;
            dgvReport.Columns[37].DisplayIndex = 22;
            if (Store.Default.selSector == "Confezione")
            {
                dgvReport.Columns[15].Visible = true;
                dgvReport.Columns[16].Visible = false;
            }
            else
            {
                dgvReport.Columns[15].Visible = false;
                dgvReport.Columns[16].Visible = true;
            }

            for (var x = 7; x <= 12; x++)
            {
                dgvReport.Columns[x].Width = 70;
            }
            dgvReport.Columns[8].Width = 85;
            for (var x = 13; x <= dgvReport.Columns.Count - 1; x++)
            {
                dgvReport.Columns[x].Width = 50;
            }

            dgvReport.Columns[20].Width = 90;
            dgvReport.Columns[21].Width = 300;

            for (var i = 4; i <= 12; i++)
                dgvReport.Columns[i].HeaderCell.Style.BackColor = Color.FromArgb(50,52,68);

            dgvReport.Columns[26].HeaderCell.Style.BackColor = Color.FromArgb(50, 52, 68);
            dgvReport.Columns[27].HeaderCell.Style.BackColor = Color.FromArgb(50, 52, 68);
            dgvReport.Columns[26].Width = 100;
            dgvReport.Columns[27].Width = 100;
            dgvReport.Columns[31].HeaderText = "Ramm\nTess";
            dgvReport.Columns[32].HeaderText = "Ramm\nConf";
            dgvReport.Columns[33].HeaderText = "Dft\nTess";
            dgvReport.Columns[34].HeaderText = "Dft\nConf";
            dgvReport.Columns["orderId"].Visible = false;

            for (int i = 0; i < dgvReport.Rows.Count; i++)
            {

                if (i % 2 == 0)
                {
                    dgvReport.Rows[i].DefaultCellStyle.BackColor = System.Drawing.Color.Gainsboro;
                }
            }

            if (dgvReport.Rows.Count <= 0) return;
            for (var i = 0; i <= dgvReport.Rows.Count - 1; i++)
            {
                if (dgvReport.Rows[i].Cells[22].Value.ToString() == "2")
                    dgvReport.Rows[i].DefaultCellStyle.BackColor = Color.LightYellow;

                if (dgvReport.Rows[i].Cells[22].Value.ToString() == "1")
                    dgvReport.Rows[i].Cells["Carico"].Style.ForeColor = Color.Red;
                else 
                    dgvReport.Rows[i].Cells["Carico"].Style.ForeColor = Color.DarkGreen;

                if (!string.IsNullOrEmpty(dgvReport.Rows[i].Cells[23].Value.ToString()))
                {
                    dgvReport.Rows[i].Cells[23].Style.BackColor = Color.LightCoral;
                    dgvReport.Rows[i].Cells[23].Style.ForeColor = Color.White;
                }
                bool.TryParse(dgvReport.Rows[i].Cells[28].Value.ToString(), out var carTrg);
                if (carTrg)
                {
                    dgvReport.Rows[i].Cells[7].Style.ForeColor = Color.Red;
                }
            }
            dgvReport.Columns["Ritardo"].DisplayIndex = 15;
            dgvReport.Columns["Ritardo"].Width = 60;

            _acsc = new AutoCompleteStringCollection();
            _acscArt = new AutoCompleteStringCollection();
            _ascsLine = new AutoCompleteStringCollection();
            CreateFilter();
        }
        /// <summary>
        /// Defines the _lstOfSplittedOrd
        /// </summary>
        private List<string> _lstOfSplittedOrd = new List<string>();
        /// <summary>
        /// The GetSplittedOrders
        /// </summary>
        /// <returns>The <see cref="List{string}"/></returns>
        private List<string> GetSplittedOrders()
        {
            var query = from split in Central.ListOfModels
                        where split.IsBase == false
                        select split;
            var lstOfSplit = query.ToList();
            var lst = new List<string>();
            foreach (var str in lstOfSplit)
            {
                if (!lst.Contains(str.Name.Trim())) lst.Add(str.Name.Trim());
            }
            return lst;
        }
        /// <summary>
        /// The ReturnImageByState
        /// </summary>
        /// <param name="ord">The ord<see cref="string"/></param>
        /// <returns>The <see cref="Image"/></returns>
        private Image ReturnImageByState(string ord)
        {
            Image img1 = Properties.Resources.order_split_flag_16;
            Image img2 = Properties.Resources.trash_16;
            Image img3 = Properties.Resources.tick_icon_16;
            Bitmap bmp = new Bitmap(24, 24); //empty
            Image img = bmp;
            if (_lstOfSplittedOrd.Contains(ord + ".1")) img = img1;
            else if (Workflow.ListOfRemovedOrders.Contains(ord)) img = img2;
            else if (_listOfAcceptedOrder.Contains(ord)) img = img3;
            return img;
        }
        /// <summary>
        /// The ReturnImageByNote
        /// </summary>
        /// <param name="ord">The ord<see cref="string"/></param>
        /// <returns>The <see cref="Image"/></returns>
        private Image ReturnImageByNote(string ord)
        {
            Image img1 = Properties.Resources.exclamation_16;
            Bitmap bmp = new Bitmap(24, 24);
            Image img = bmp;
            if (_listOfOrdersWithNotes.Contains(ord)) img = img1;
            return img;
        }
        /// <summary>
        /// The ReturnImageByCompletetion
        /// </summary>
        /// <param name="ord">The ord<see cref="string"/></param>
        /// <returns>The <see cref="Image"/></returns>
        private Image ReturnImageByCompletetion(string ord)
        {
            Image img1 = Properties.Resources.inform_16;
            Bitmap bmp = new Bitmap(24, 24);
            Image img = bmp;
            if (_listOfLinesComplete.Contains(ord)) img = img1;
            return img;
        }
        /// <summary>
        /// The UniParseDateTime
        /// </summary>
        /// <param name="obj">The obj<see cref="object"/></param>
        /// <returns>The <see cref="string"/></returns>
        private string UniParseDateTime(object obj)
        {
            DateTime.TryParse(obj.ToString(), out var dt);
            string tmpStr;
            if (dt == DateTime.MinValue)
            {
                tmpStr = "";
            }
            else
            {
                var uniDt = new DateTime(dt.Year, dt.Month, dt.Day);

                tmpStr = uniDt.ToString("dd/MM/yyyy",
                    CultureInfo.InvariantCulture);
            }
            return tmpStr;
        }
        /// <summary>
        /// The PrintGrid
        /// </summary>
        public void PrintGrid()
        {
            var lbl = new PictureBox();
            lbl.Image = Properties.Resources.printing_gif;
            lbl.SizeMode = PictureBoxSizeMode.CenterImage;
            lbl.Dock = DockStyle.Fill;
            lbl.BackColor = Color.White;
            lbl.Font = new Font("Tahoma", 20, FontStyle.Bold);
            Controls.Add(lbl);
            lbl.BringToFront();
            RemoveFilters();
            for (var i = 13; i <= dgvReport.ColumnCount - 1; i++)
            {
                dgvReport.Columns[i].Visible = false;
            }
            dgvReport.Columns[26].Visible = false;
            dgvReport.Columns[27].Visible = false;
            dgvReport.Columns["Ritardo"].Visible = true;
            dgvReport.Columns[29].Visible = true;
            dgvReport.Columns["Dvc"].Visible = false;
            var dGvPrinter = new TableViewPrint
            {
                Title = "Carico lavoro",
                SubTitle = DateTime.Now.ToShortDateString(),
                SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip,
                PageNumbers = true,
                PageNumberInHeader = false,
                PorportionalColumns = true,
                HeaderCellAlignment = StringAlignment.Near,
                Footer = "ONLYOU",
                FooterSpacing = 15,
                CellAlignment = StringAlignment.Center,
            };
            dGvPrinter.PageSettings.Landscape = true;
            dGvPrinter.PrintDataGridView(dgvReport);
            for (var i = 13; i <= dgvReport.ColumnCount - 1; i++)
            {
                dgvReport.Columns[i].Visible = true;
            }
            dgvReport.Columns["Dvc"].Visible = true;
            dgvReport.Columns["IdState"].Visible = false;
            dgvReport.Columns[26].Visible = false;
            dgvReport.Columns[27].Visible = false;
            dgvReport.Columns[28].Visible = false;
            dgvReport.Columns[29].Visible = true;
            dgvReport.Columns[30].Visible = false;

            AddHeaderButtons();
            CreateFilter();
            Controls.Remove(lbl);
            lbl.Dispose();
        }
        private int _hidden = -1;
        public void HideLightColumns()
        {
            if (dgvReport.Columns.Count < 12) return;
            var idx = dgvReport.Columns[12].Index;

            switch (_hidden)
            {
                case -1:
                    for (var i = idx + 1; i <= dgvReport.Columns.Count - 1; i++)
                    {
                        //if (dgvReport.Columns[i].HeaderText == "Info" || dgvReport.Columns[i].HeaderText == "Time") continue;
                        if (dgvReport.Columns[i].HeaderCell.Style.BackColor != Color.FromArgb(50, 52, 68))
                        {
                            dgvReport.Columns[i].Visible = false;
                        }
                      
                    }
                    _hidden = 0;
                    break;
                case 0:
                    for (var i = idx + 1; i <= dgvReport.Columns.Count - 1; i++)
                    {
                        if (dgvReport.Columns[i].HeaderCell.Style.BackColor != Color.FromArgb(50, 52, 68))
                        {
                            dgvReport.Columns[i].Visible = true;
                        }
                    }
                    dgvReport.Columns[14].Visible = false;
                    //dgvReport.Columns[16].Visible = false;
                    dgvReport.Columns[22].Visible = false;
                    dgvReport.Columns[26].Visible = false;
                    dgvReport.Columns[27].Visible = false;
                    if (Store.Default.selSector == "Confezione")
                    {
                        dgvReport.Columns[15].Visible = true;
                        dgvReport.Columns[16].Visible = false;
                    }
                    else
                    {
                        dgvReport.Columns[15].Visible = false;
                        dgvReport.Columns[16].Visible = true;
                    }
                    _hidden = -1;
                    break;
            }
        }

        /// <summary>
        /// The RemoveFilters
        /// </summary>
        private void RemoveFilters()
        {
            _filterCreated = false;
            for (int i = dgvReport.Controls.Count - 1; i >= 0; i--)
            {
                if ((dgvReport.Controls[i] is TextBox filter) && (filter.Name != "XXXXXXX"))
                {
                    dgvReport.Controls.RemoveAt(i);
                    filter.Dispose();
                }
            }
        }
        /// <summary>
        /// Defines the _filterCreated
        /// </summary>
        private bool _filterCreated;
        /// <summary>
        /// Defines the _txtArt
        /// </summary>
        private TextBox _txtArt;
        /// <summary>
        /// Defines the _txtLin
        /// </summary>
        private TextBox _txtLin;
        /// <summary>
        /// The CreateFilter
        /// </summary>
        private void CreateFilter()
        {
            if (dgvReport.Rows.Count <= 0) return;
            _acsc.Clear();
            _ascsLine.Clear();
            _acscArt.Clear();
            foreach (DataGridViewRow row in dgvReport.Rows)
            {
                _acsc.Add(row.Cells[1].Value.ToString());
                _acscArt.Add(row.Cells[2].Value.ToString());
                _ascsLine.Add(row.Cells[3].Value.ToString());
            }
            if (_filterCreated) return;
            SuspendLayout();
            var txtComm = new TextBox
            {
                Name = dgvReport.Columns[1].Name,
                BorderStyle = BorderStyle.None,
                BackColor = Color.White,
                Font = new Font("Microsoft Sans Serif", 10, FontStyle.Regular),
                Parent = dgvReport
            };
            _txtArt = new TextBox
            {
                Name = dgvReport.Columns[2].Name,
                BorderStyle = BorderStyle.None,
                BackColor = Color.White,
                Font = new Font("Microsoft Sans Serif", 10, FontStyle.Regular),
                Parent = dgvReport
            };
            _txtLin = new TextBox
            {
                Name = dgvReport.Columns[3].Name,
                BorderStyle = BorderStyle.None,
                BackColor = Color.White,
                Font = new Font("Microsoft Sans Serif", 10, FontStyle.Regular),
                Parent = dgvReport
            };

            dgvReport.Controls.Add(_txtLin);
            dgvReport.Controls.Add(_txtArt);
            dgvReport.Controls.Add(txtComm);
            var headerRect = dgvReport.GetColumnDisplayRectangle(1, true);
            txtComm.Location = new Point(headerRect.Location.X + 1, 50 - txtComm.Height - 2);
            txtComm.Size = new Size(headerRect.Width - 3, dgvReport.ColumnHeadersHeight);
            headerRect = dgvReport.GetColumnDisplayRectangle(2, true);
            _txtArt.Location = new Point(headerRect.Location.X + 1, 50 - _txtArt.Height - 2);
            _txtArt.Size = new Size(headerRect.Width - 3, dgvReport.ColumnHeadersHeight);
            headerRect = dgvReport.GetColumnDisplayRectangle(3, true);
            _txtLin.Location = new Point(headerRect.Location.X + 1, 50 - _txtLin.Height - 2);
            _txtLin.Size = new Size(headerRect.Width - 3, dgvReport.ColumnHeadersHeight);
            ResumeLayout(true);
            txtComm.TextChanged += (s, e) =>
            {
                _btnClearFilter = new Button();
                if (txtComm.Controls.Contains(_btnClearFilter))
                {
                    txtComm.Controls.Remove(_btnClearFilter);
                    _btnClearFilter.Dispose();
                }
                if (txtComm.Text != string.Empty)
                {
                    AddClearFilterButton(txtComm, "btncom");
                    txtComm.Controls.Add(_btnClearFilter);
                }
                RemoveGridControls();
                CollectFiltersQuery();
                _bs.Filter = _strFilter; //string.Format("CONVERT(" + dgvReport.Columns[txtComm.Name]?.DataPropertyName +
                                         //", System.String) like '%" + txtComm.Text.Replace("'", "''") +
                                         //"%'");
                dgvReport.DataSource = _bs;
                dgvReport.Refresh();
            };
            _txtArt.TextChanged += (s, e) =>
            {
                if (_txtArt.Controls.Contains(_btnClearFilter))
                {
                    _txtArt.Controls.Remove(_btnClearFilter);
                    _btnClearFilter.Dispose();
                }
                if (_txtArt.Text != string.Empty)
                {
                    AddClearFilterButton(_txtArt, "btnart");
                    _txtArt.Controls.Add(_btnClearFilter);
                }
                RemoveGridControls();
                CollectFiltersQuery();
                _bs.Filter = _strFilter;// string.Format("CONVERT(" + dgvReport.Columns[_txtArt.Name]?.DataPropertyName +
                //                                ", System.String) like '%" + _txtArt.Text.Replace("'", "''") +
                //                                "%'");
                dgvReport.DataSource = _bs;
                dgvReport.Refresh();
            };
            _txtLin.TextChanged += (s, e) =>
            {
                if (_txtLin.Controls.Contains(_btnClearFilter))
                {
                    _txtLin.Controls.Remove(_btnClearFilter);
                    _btnClearFilter.Dispose();
                }
                if (_txtLin.Text != string.Empty)
                {
                    AddClearFilterButton(_txtLin, "btnlin");
                    _txtLin.Controls.Add(_btnClearFilter);
                }
                RemoveGridControls();
                CollectFiltersQuery();

                _bs.Filter = _strFilter; // string.Format("CONVERT(" + dgvReport.Columns[_txtLin.Name]?.DataPropertyName +
                                         //     ", System.String) = '{0}'", _txtLin.Text.Replace("'", "''"));
                dgvReport.DataSource = _bs;
                dgvReport.Refresh();
            };

            _filterCreated = true;

            for (var i = dgvReport.Controls.Count - 1; i >= 0; i--)
            {
                if (!(dgvReport.Controls[i] is TextBox txt)) continue;

                if (txt.Name == dgvReport.Columns[1].Name)
                {
                    txt.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    txt.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    txt.AutoCompleteCustomSource = _acsc;
                }
                else if (txt.Name == dgvReport.Columns[2].Name)
                {
                    txt.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    txt.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    txt.AutoCompleteCustomSource = _acscArt;
                }
                else if (txt.Name == dgvReport.Columns[3].Name)
                {
                    txt.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    txt.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    txt.AutoCompleteCustomSource = _ascsLine;
                }
            }
        }
        /// <summary>
        /// Defines the _strFilter
        /// </summary>
        private string _strFilter;
        /// <summary>
        /// The CollectFiltersQuery
        /// </summary>
        private void CollectFiltersQuery()
        {
            var i = 0;
            _strFilter = "";
            foreach (var c in dgvReport.Controls)
            {
                if (c is TextBox txt)
                {
                    if (txt.Text != string.Empty)
                    {
                        i++;

                        if (i == 1)
                        {
                            _strFilter = string.Format("CONVERT(" + dgvReport.Columns[txt.Name].DataPropertyName +
                                ", System.String) = '" + txt.Text.Replace("'", "''") + "'");
                        }
                        else
                        {
                            var str = " and CONVERT(" + dgvReport.Columns[txt.Name].DataPropertyName +
                                ", System.String) = '" + txt.Text.Replace("'", "''") + "'";
                            _strFilter += str;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// The SendMessage
        /// </summary>
        /// <param name="hWnd">The hWnd<see cref="IntPtr"/></param>
        /// <param name="msg">The msg<see cref="int"/></param>
        /// <param name="wp">The wp<see cref="IntPtr"/></param>
        /// <param name="lp">The lp<see cref="IntPtr"/></param>
        /// <returns>The <see cref="IntPtr"/></returns>
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);
        /// <summary>
        /// Defines the _btnClearFilter
        /// </summary>
        private Button _btnClearFilter;
        /// <summary>
        /// The AddClearFilterButton
        /// </summary>
        /// <param name="txt">The txt<see cref="TextBox"/></param>
        /// <param name="name">The name<see cref="string"/></param>
        private void AddClearFilterButton(TextBox txt, string name)
        {
            _btnClearFilter = new Button
            {
                Size = new Size(25, txt.ClientSize.Height + 2)
            };
            _btnClearFilter.Location = new Point(txt.ClientSize.Width - _btnClearFilter.Width, -1);
            //_btnClearFilter.Image = Properties.Resources.cleanse;
            _btnClearFilter.Cursor = Cursors.Default;
            _btnClearFilter.FlatStyle = FlatStyle.Flat;
            _btnClearFilter.FlatAppearance.BorderSize = 0;
            _btnClearFilter.FlatAppearance.MouseDownBackColor = Color.SkyBlue;
            _btnClearFilter.BackColor = default;
            _btnClearFilter.Font = new Font("Microsoft Sans Serif", 8, FontStyle.Bold);
            _btnClearFilter.ForeColor = Color.FromArgb(60, 60, 60);
            _btnClearFilter.Text = "X";
            _btnClearFilter.Name = name;
            _btnClearFilter.Parent = txt;
            _btnClearFilter.Click += (s, e) =>
            {
                txt.Clear();
            };

            SendMessage(txt.Handle, 0xd3, (IntPtr)2, (IntPtr)(_btnClearFilter.Width << 16));
        }
        /// <summary>
        /// The dgvReport_CellDoubleClick
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="DataGridViewCellEventArgs"/></param>
        private void dgvReport_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (!IsUpd)
            {
                if (e.ColumnIndex != 1) return;

                var order = dgvReport.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                var id = dgvReport.Rows[e.RowIndex].Cells["orderId"].Value.ToString();

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
                var ci = new Carico(order, id);
                ci.StartPosition = FormStartPosition.CenterScreen;
                ci.Show();
                ci.TopMost = true;
                ci.FormClosing += (s, ev) =>
                {
                    frmTransp.Close();
                };
                frmTransp.Click += (s, ev) =>
                {
                    ci.Close();
                    frmTransp.Close();
                };
                return;
            }
            var selectedOrder = dgvReport.Rows[e.RowIndex].Cells[1].Value.ToString();

            int.TryParse(dgvReport.Rows[e.RowIndex].Cells[4].Value.ToString(), out var qty);
            int.TryParse(dgvReport.Rows[e.RowIndex].Cells[5].Value.ToString(), out var carico);
            double.TryParse(dgvReport.Rows[e.RowIndex].Cells[13].Value.ToString(), out var qtyH);

            if (qtyH < 1.0)
            {
                MessageBox.Show("QtyH zero value isn't allowed for programmation.");
                return;
            }
            if (selectedOrder == null || string.IsNullOrEmpty(selectedOrder))
            {
                MessageBox.Show("Selection anomaly has been detected.");
                return;
            }
            var byQty = false;
            var dg = MessageBox.Show("Do you want to program by total qty?", "Produzione gantt", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dg == DialogResult.Yes) byQty = true;

            if (!byQty)
            {
                if (carico <= 0)
                {
                    MessageBox.Show("Carico zero value isn't allowed for programmation.");
                    return;
                }
            }
            else
            {
                if (qty <= 0)
                {
                    MessageBox.Show("Qty zero value isn't allowed for programmation.");
                    return;
                }
            }

            Workflow.ByQty = byQty;
            Workflow.TargetOrder = selectedOrder;

            Close();
        }

        /// <summary>
        /// Defines the _cbLineChange
        /// </summary>
        private ComboBox _cbLineChange;

        /// <summary>
        /// Defines the _dtProdChange
        /// </summary>
        private DateTimePicker _dtProdChange;

        /// <summary>
        /// Defines the _txtInput
        /// </summary>
        private TextBox _txtInput;

        /// <summary>
        /// Defines the _txtOnEnter
        /// </summary>
        private string _txtOnEnter;

        /// <summary>
        /// Defines the _orderToUpdate
        /// </summary>
        private string _orderToUpdate;

        /// <summary>
        /// Defines the _line
        /// </summary>
        private string _line;

        private string _department;

        /// <summary>
        /// Defines the _rowIdx, _cellIdx
        /// </summary>
        private int _rowIdx, _cellIdx;

        /// <summary>
        /// Defines the _btnShowProd
        /// </summary>
        private Button _btnShowProd;

        /// <summary>
        /// Defines the _btnShowFin
        /// </summary>
        private Button _btnShowFin;

        /// <summary>
        /// The dgvReport_CellClick
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="DataGridViewCellEventArgs"/></param>
        private void dgvReport_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex <= -1) return;

            RemoveGridControls();

            _orderToUpdate = dgvReport.Rows[e.RowIndex].Cells[1].Value.ToString();
            _line = dgvReport.Rows[e.RowIndex].Cells[3].Value.ToString();
            _department = dgvReport.Rows[e.RowIndex].Cells[20].Value.ToString();

            if (e.ColumnIndex == 0)
            {
                if ((from split in Central.ListOfModels
                     where split.Name == _orderToUpdate
                     select split).ToList().Count > 0)
                {
                    var sph = new SplitHistory(_orderToUpdate, _department);
                    sph.IsFromSplit = false;
                    sph.StartPosition = FormStartPosition.CenterScreen;
                    sph.ShowDialog();
                    sph.Dispose();
                }

                //if (_listOfAcceptedOrder.Contains(_orderToUpdate) && !_lstOfSplittedOrd.Contains(_orderToUpdate))
                //{
                //    var inf = new MyMessage("Status", "Accepted in the system");
                //    //inf.InfoText = "Accepted in the system.";
                //    inf.MessageIcon = ReturnImageByState(_orderToUpdate);
                //    inf.Show();
                //}

                if (Workflow.ListOfRemovedOrders.Contains(_orderToUpdate) && !_lstOfSplittedOrd.Contains(_orderToUpdate))
                {
                    var inf = new MyMessage("Status", "Removed from the system, but still is programmable.");
                    inf.MessageIcon = ReturnImageByState(_orderToUpdate);
                    inf.Show();
                }
            }

            if (e.ColumnIndex == 24)
            {
                var info = dgvReport.Rows[e.RowIndex].Cells[21].Value.ToString();
                if (!_listOfOrdersWithNotes.Contains(_orderToUpdate)) return;

                var inf = new MyMessage("Note", info);
                inf.MessageIcon = ReturnImageByNote(_orderToUpdate);
                inf.Show();
            }

            if (e.ColumnIndex == 25)
            {
                if (!_listOfLinesComplete.Contains(_orderToUpdate)) return;

                var line = dgvReport.Rows[e.RowIndex].Cells[3].Value.ToString();
                var days = Store.Default.daysToFinish.ToString();

                var inf = new MyMessage("Produzione", days + " more days left the " + line + " complete order:" + _orderToUpdate);
                inf.MessageIcon = ReturnImageByCompletetion(_orderToUpdate);
                inf.Show();
            }

            if (e.ColumnIndex == 3)
            {
                var lineTxt = "";

                if (!string.IsNullOrEmpty(dgvReport.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()))
                {
                    lineTxt = dgvReport.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                }

                _cbLineChange = new ComboBox();
                var dept = dgvReport.Rows[e.RowIndex].Cells[20].Value.ToString();

                var linesList = (from lines in Models.Tables.Lines
                                 where lines.Department == dept
                                 select lines);

                foreach (var item in linesList)
                {
                    _cbLineChange.Items.Add(item.Description);
                }
                _cbLineChange.SelectedIndex = _cbLineChange.FindString(lineTxt);
                _cbLineChange.Font = new Font("Microsoft Sans Serif", 10);
                dgvReport.Controls.Add(_cbLineChange);
                Rectangle Rectangle = dgvReport.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                _cbLineChange.Size = new Size(Rectangle.Width, Rectangle.Height);
                _cbLineChange.Location = new Point(Rectangle.X, Rectangle.Y);
                _cbLineChange.SelectedIndexChanged += _cbLineChange_SelectedIndexChanged;
                _cbLineChange.Visible = true;
                _cbLineChange.DropDownWidth = 100;
                _cbLineChange.DropDownHeight = 300;
                _cbLineChange.BackColor = Color.LightYellow;
                
                _rowIdx = e.RowIndex;
                _cellIdx = e.ColumnIndex;
                _cbLineChange.Focus();
                _cbLineChange.KeyDown += (s, g) =>
                {
                    if (g.KeyCode == Keys.Escape)
                    {
                        dgvReport.Controls.Remove(_cbLineChange);
                        _cbLineChange?.Dispose();
                    }
                };
            }

            if (e.ColumnIndex == 10) //(e.ColumnIndex == 8 || e.ColumnIndex == 10)
            {
                var dt = DateTime.Now;

                if (!string.IsNullOrEmpty(dgvReport.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()))
                {
                    dt = DateTime.ParseExact(dgvReport.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                }

                _dtProdChange = new DateTimePicker();
                _dtProdChange.Format = DateTimePickerFormat.Custom;
                _dtProdChange.CustomFormat = "dd/MM/yyyy";
                _dtProdChange.Font = new Font("Microsoft Sans Serif", 9);
                _dtProdChange.Value = dt;
                dgvReport.Controls.Add(_dtProdChange);
                Rectangle Rectangle = dgvReport.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                _dtProdChange.Size = new Size(Rectangle.Width, Rectangle.Height);
                _dtProdChange.Location = new Point(Rectangle.X, Rectangle.Y);
                _dtProdChange.ValueChanged += _dtpProdChange_ValueChange;
                _dtProdChange.Visible = true;
                _dtProdChange.Focus();
                _rowIdx = e.RowIndex;
                _cellIdx = e.ColumnIndex;
                _dtProdChange.KeyDown += (s, g) =>
                {
                    if (g.KeyCode == Keys.Escape)
                    {
                        dgvReport.Controls.Remove(_dtProdChange);
                        _dtProdChange?.Dispose();
                    }
                };
            }
            if (e.ColumnIndex == 17)
            {
                _txtInput = new TextBox();
                _txtInput.Text = dgvReport.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                dgvReport.Controls.Add(_txtInput);
                Rectangle Rectangle = dgvReport.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                _txtInput.Size = new Size(Rectangle.Width, Rectangle.Height);
                _txtInput.Location = new Point(Rectangle.X, Rectangle.Y);
                _txtInput.Font = new Font("Microsoft Sans Serif", 8);
                _txtInput.Visible = true;
                _rowIdx = e.RowIndex;
                _cellIdx = e.ColumnIndex;
                _txtOnEnter = _txtInput.Text;
                _txtInput.Tag = e.ColumnIndex;
                _txtInput.KeyDown += (s, g) =>
                {
                    if (g.KeyCode == Keys.Escape) _txtInput?.Dispose();
                };
            }
            if (e.ColumnIndex == 15 || e.ColumnIndex == 16 || e.ColumnIndex == 18 || e.ColumnIndex == 21 ||
                e.ColumnIndex == 31 || e.ColumnIndex == 32 || e.ColumnIndex == 33 || e.ColumnIndex == 34)
            {
                _txtInput = new TextBox();
                _txtInput.Text = dgvReport.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                dgvReport.Controls.Add(_txtInput);
                Rectangle Rectangle = dgvReport.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                _txtInput.Size = new Size(Rectangle.Width, Rectangle.Height);
                _txtInput.Location = new Point(Rectangle.X, Rectangle.Y);
                _txtInput.Font = new Font("Microsoft Sans Serif", 8);
                _txtInput.Visible = true;
                _rowIdx = e.RowIndex;
                _cellIdx = e.ColumnIndex;
                _txtOnEnter = _txtInput.Text;
                _txtInput.Tag = e.ColumnIndex;
                _txtInput.KeyDown += (s, g) =>
                {
                    if (g.KeyCode == Keys.Escape)
                    {
                        RemoveGridControls();
                        //  _txtInput?.Dispose();
                    }
                };
            }
            
            _txtInput?.Focus();
        }

        /// <summary>
        /// The dgvReport_CellEndEdit
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="DataGridViewCellEventArgs"/></param>
        private void dgvReport_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var comm = dgvReport.Rows[e.RowIndex].Cells[1].Value.ToString();
            if (e.ColumnIndex == 22)
            {
                MessageBox.Show(comm + " Note");
            }
        }

        /// <summary>
        /// The dgvReport_Scroll
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="ScrollEventArgs"/></param>
        private void dgvReport_Scroll(object sender, ScrollEventArgs e)
        {
            if (dgvReport.Controls.Contains(_cbLineChange))
            {
                Rectangle Rectangle = dgvReport.GetCellDisplayRectangle(_cellIdx, _rowIdx, true);
                _cbLineChange.Size = new Size(Rectangle.Width, Rectangle.Height);
                _cbLineChange.Location = new Point(Rectangle.X, Rectangle.Y);
            }
            if (dgvReport.Controls.Contains(_dtProdChange))
            {
                Rectangle Rectangle = dgvReport.GetCellDisplayRectangle(_cellIdx, _rowIdx, true);
                _dtProdChange.Size = new Size(Rectangle.Width, Rectangle.Height);
                _dtProdChange.Location = new Point(Rectangle.X, Rectangle.Y);
            }
            if (dgvReport.Controls.Contains(_txtInput))
            {
                Rectangle Rectangle = dgvReport.GetCellDisplayRectangle(_cellIdx, _rowIdx, true);
                _txtInput.Size = new Size(Rectangle.Width, Rectangle.Height);
                _txtInput.Location = new Point(Rectangle.X, Rectangle.Y);
            }
            if (dgvReport.Controls.Contains(_btnShowProd))
            {
                Rectangle Rectangle = dgvReport.GetCellDisplayRectangle(8, -1, true);
                //_btnShowProd.Size = new Size(30,20);
                _btnShowProd.Location = new Point(Rectangle.X + Rectangle.Width - 30, Rectangle.Y + Rectangle.Height - 20);
            }
            if (dgvReport.Controls.Contains(_btnShowFin))
            {
                Rectangle Rectangle = dgvReport.GetCellDisplayRectangle(9, -1, true);
                // _btnShowFin.Size = new Size(30, 20);
                _btnShowFin.Location = new Point(Rectangle.X + Rectangle.Width - 30, Rectangle.Y + Rectangle.Height - 20);
            }

            dgvReport.Invalidate();
            _btnShowFin.Invalidate();
            _btnShowProd.Invalidate();
        }

        /// <summary>
        /// The AddHeaderButtons
        /// </summary>
        private void AddHeaderButtons()
        {
            for (int i = dgvReport.Controls.Count - 1; i >= 0; i--)
            {
                if ((dgvReport.Controls[i] is Button ctrl) && (ctrl.Name != "XXXXXXX"))
                {
                    dgvReport.Controls.RemoveAt(i);
                    ctrl.Dispose();
                }
            }

            _btnShowProd = new Button();
            _btnShowProd.Text = "►";
            dgvReport.Controls.Add(_btnShowProd);
            Rectangle Rectangle = dgvReport.GetCellDisplayRectangle(8, -1, true);
            _btnShowProd.Size = new Size(30, 20);
            _btnShowProd.TextAlign = ContentAlignment.MiddleCenter;
            _btnShowProd.Location = new Point(Rectangle.X + Rectangle.Width - 30, Rectangle.Y + Rectangle.Height - 20);
            _btnShowProd.Font = new Font("Microsoft Sans Serif", 10);
            _btnShowProd.Visible = true;
            _btnShowProd.FlatStyle = FlatStyle.Flat;
            _btnShowProd.BackColor = Color.FromArgb(50, 52, 68);
            _btnShowProd.ForeColor = Color.WhiteSmoke;
            _btnShowProd.FlatAppearance.BorderSize = 0;

            _btnShowProd.Click += (s, e) =>
            {
                if (dgvReport.Columns[26].Visible)
                {
                    dgvReport.Columns[26].Visible = false;
                    _btnShowProd.Text = "►";

                    if (_btnShowFin != null)
                    {
                        Rectangle rect = dgvReport.GetCellDisplayRectangle(9, -1, true);
                        _btnShowFin.Size = new Size(30, 20);
                        _btnShowFin.Location = new Point(rect.X + rect.Width - 30, rect.Y + rect.Height - 20);
                    }
                }
                else
                {
                    dgvReport.Columns[26].Visible = true;
                    _btnShowProd.Text = "◄";

                    if (_btnShowFin != null)
                    {
                        Rectangle rect = dgvReport.GetCellDisplayRectangle(9, -1, true);
                        _btnShowFin.Size = new Size(30, 20);
                        _btnShowFin.Location = new Point(rect.X + rect.Width - 30, rect.Y + rect.Height - 20);
                    }
                }
            };

            _btnShowFin = new Button();
            _btnShowFin.Text = "►";
            dgvReport.Controls.Add(_btnShowFin);
            Rectangle = dgvReport.GetCellDisplayRectangle(9, -1, true);
            _btnShowFin.Size = new Size(30, 20);
            _btnShowFin.TextAlign = ContentAlignment.MiddleCenter;
            _btnShowFin.Location = new Point(Rectangle.X + Rectangle.Width - 30, Rectangle.Y + Rectangle.Height - 20);
            _btnShowFin.Font = new Font("Microsoft Sans Serif", 10);
            _btnShowFin.Visible = true;
            _btnShowFin.FlatStyle = FlatStyle.Flat;
            _btnShowFin.BackColor = Color.FromArgb(50, 52, 68);
            _btnShowFin.ForeColor = Color.WhiteSmoke;
            _btnShowFin.FlatAppearance.BorderSize = 0;

            _btnShowFin.Click += (s, e) =>
            {
                if (dgvReport.Columns[27].Visible)
                {
                    dgvReport.Columns[27].Visible = false;
                    _btnShowFin.Text = "►";
                }
                else
                {
                    dgvReport.Columns[27].Visible = true;
                    _btnShowFin.Text = "◄";
                }
            };
        }

        /// <summary>
        /// The SelectProgrammedCommessa
        /// </summary>
        /// <param name="target">The target<see cref="string"/></param>
        public void SelectProgrammedCommessa(string target)
        {
            LoadCaricoLavoroInternal();
            var idx = 0;
            foreach (DataGridViewRow row in dgvReport.Rows)
                if (row.Cells[1].Value.ToString() == target)
                {
                    idx = row.Index;
                    row.Selected = true;
                }

            if (idx > 1)
                dgvReport.FirstDisplayedScrollingRowIndex = idx - 2;
            else
                dgvReport.FirstDisplayedScrollingRowIndex = idx;
        }

        /// <summary>
        /// The RemoveGridControls
        /// </summary>
        private void RemoveGridControls()
        {
            if (dgvReport.Controls.Contains(_cbLineChange))
            {
                dgvReport.Controls.Remove(_cbLineChange);
            }
            if (dgvReport.Controls.Contains(_dtProdChange))
            {
                dgvReport.Controls.Remove(_dtProdChange);
            }
            if (dgvReport.Controls.Contains(_txtInput))
            {
                if (_txtInput.Text == _txtOnEnter)
                {
                    dgvReport.Controls.Remove(_txtInput);

                    return;
                }
                var dr = MessageBox.Show("Do you want to save changes?", "Carico lavoro", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    var tmpStr = "";

                    if (_txtInput.Tag.ToString() == "15")
                    {
                        tmpStr = "Tessitura";
                    }
                    else if (_txtInput.Tag.ToString() == "16")
                    {
                        tmpStr = "Confezione";
                    }
                    else if (_txtInput.Tag.ToString() == "17")
                    {
                        tmpStr = "Respinte";
                    }
                    else if (_txtInput.Tag.ToString() == "18")
                    {
                        tmpStr = "Consegnato";
                    }
                    else if (_txtInput.Tag.ToString() == "21")
                    {
                        tmpStr = "Note";
                    }
                    else if (_txtInput.Tag.ToString() == "31")
                    {
                        tmpStr = "Ram_tess";
                    }
                    else if (_txtInput.Tag.ToString() == "32")
                    {
                        tmpStr = "Ram_conf";
                    }
                    else if (_txtInput.Tag.ToString() == "33")
                    {
                        tmpStr = "Def_tess";
                    }
                    else if (_txtInput.Tag.ToString() == "34")
                    {
                        tmpStr = "Def_conf";
                    }
                    using (var con = new System.Data.Linq.DataContext(Central.ConnStr))
                    {
                        con.ExecuteCommand("update Comenzi set " + tmpStr + "={0} where NrComanda={1}", _txtInput.Text, _orderToUpdate);
                    }
                    dgvReport.Rows[_rowIdx].Cells[_cellIdx].Value = _txtInput.Text;
                    dgvReport.Rows[_rowIdx].Cells[24].Value = Properties.Resources.exclamation_16;
                    dgvReport.Controls.Remove(_txtInput);
                }
                else
                {
                    dgvReport.Controls.Remove(_txtInput);
                }
            }
        }
        /// <summary>
        /// Gets or sets the StartDateValue
        /// </summary>
        public static DateTime StartDateValue { get; set; }

        /// <summary>
        /// The _cbLineChange_SelectedIndexChanged
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="eventArgs">The eventArgs<see cref="EventArgs"/></param>
        private void _cbLineChange_SelectedIndexChanged(object sender, EventArgs eventArgs)
        {
            var cb = (ComboBox)sender;

            double.TryParse(dgvReport.CurrentRow.Cells[13].Value.ToString(), out var qtyH);

            int.TryParse(dgvReport.CurrentRow.Cells[4].Value.ToString(), out var totalQty);
            int.TryParse(dgvReport.CurrentRow.Cells[5].Value.ToString(), out var carico);

            int.TryParse(dgvReport.CurrentRow.Cells["IdState"].Value.ToString(), out var stateNum);
            var article = dgvReport.CurrentRow.Cells[2].Value.ToString();
            var dept = dgvReport.CurrentRow.Cells["Department"].Value.ToString();

            var lineInsteadDescripton = (from lines in Tables.Lines
                                         where lines.Description == cb.Text && lines.Department == dept
                                         select lines).SingleOrDefault().Line;

            if (stateNum == 2)
            {
                MessageBox.Show("Programmation of an order with 'Chiuso' status can harm current display of the workflow.\n\nAction won't be performed.", "Stop", MessageBoxButtons.OKCancel, MessageBoxIcon.Stop);
                return;
            }
            //check if all aprameters are greater zero
            if (qtyH < 1.0)
            {
                MessageBox.Show("Capi/ora (" + qtyH.ToString() + ") is not accepted as an unity.", "Programming job", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            var exist = Central.ListOfModels.Where(x => x.Name == _orderToUpdate && x.Department == dept).ToList();
            if (exist.Count > 0)
            {
                MessageBox.Show("Order already exist as an accepted model.\n" +
                    "Parametarized or cloning anomaly has been occurred.", "Workflow controller",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var byQty = false;
            var dg = MessageBox.Show("Do you want to program by total qty?", "Produzione gantt", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dg == DialogResult.Yes) byQty = true;

            var d = JobModel.GetLineLastDate(lineInsteadDescripton, dept);

            int qty;
            if (byQty)
            {
                //check qty are greater zero
                if (totalQty == 0)
                {
                    MessageBox.Show("Qty must be greater zero!", "Programming job", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                qty = totalQty;
            }
            else
            {
                //check if all aprameters are greater zero
                if (carico == 0)
                {
                    MessageBox.Show("Carico must be greater zero!", "Programming job", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                qty = carico;
            }
            var j = new JobModel();

            
            var jobDuration = j.CalculateJobDuration(lineInsteadDescripton, qty, qtyH, dept);    //production duration
            int.TryParse(j.CalculateDailyQty(lineInsteadDescripton, qtyH, dept).ToString(), out var dailyQty);
            var price = j.GetPrice(article);
            DateTime startDate;
            DateTime endDate;

            if (d == Config.MinimalDate || d == DateTime.MinValue)
            {
                //MessageBox.Show("Please choose order start date.", "Produzione gantt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                var inputDate = new InputDate();
                inputDate.ShowDialog();
                inputDate.Dispose();
                startDate = StartDateValue;
                if (StartDateValue == DateTime.MinValue) return;
                endDate = StartDateValue.AddDays(+jobDuration);
            }
            else
            {
                startDate = d;
                endDate = d.AddDays(+jobDuration);
            }
            if (endDate == DateTime.MinValue)
            {
                MessageBox.Show("Some error has ocurred.");
                return;
            }
            using (var ctx = new System.Data.Linq.DataContext(Central.ConnStr))
            // update job aim
            {
                ctx.ExecuteCommand("update comenzi set DataProduzione={0},DataFine={1},Line={2}," +
                    "QtyInstead={3},Duration={4} where NrComanda={5} and Department={6}",
                    startDate, endDate,
                    cb.Text, byQty, jobDuration, _orderToUpdate, dept);
            }
            dgvReport.CurrentCell.Value = cb.Text;
            dgvReport.CurrentRow.Cells[8].Value = UniParseDateTime(startDate);
            dgvReport.CurrentRow.Cells[9].Value = UniParseDateTime(endDate);
            dgvReport.CurrentRow.Cells[0].Value =
                Properties.Resources.tick_icon_16;

            InsertNewProgram(_orderToUpdate, cb.Text, article, qty, qtyH, startDate, jobDuration, dailyQty, price,dept);
        }

        /// <summary>
        /// The _dtpProdChange_ValueChange
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void _dtpProdChange_ValueChange(object sender, EventArgs e)
        {
            var dtp = (DateTimePicker)sender;
            var dr = MessageBox.Show("Do you want to upadte to 'Chiuso'?", "Carico lavoro", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                using (var con = new System.Data.Linq.DataContext(Central.ConnStr))
                {
                    con.ExecuteCommand("update Comenzi set " + "DataLivrare={0}, IdStare={1} where NrComanda={2}", dtp.Value, 2, _orderToUpdate);
                }
                dgvReport.Rows[_rowIdx].Cells[_cellIdx].Value = UniParseDateTime(_dtProdChange.Value.ToString());
                dgvReport.Controls.Remove(dtp);
            }
            else
            {
                using (var con = new System.Data.Linq.DataContext(Central.ConnStr))
                {
                    con.ExecuteCommand("update Comenzi set " + "DataLivrare" + "={0} where NrComanda={1}", dtp.Value, _orderToUpdate);
                }
                dgvReport.Rows[_rowIdx].Cells[_cellIdx].Value = UniParseDateTime(_dtProdChange.Value.ToString());
                dgvReport.Controls.Remove(dtp);
            }
        }

        /// <summary>
        /// The tmsiSplitCommessa_Click
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void tmsiSplitCommessa_Click(object sender, EventArgs e)
        {
            IEnumerable<JobModel> modelQuery;

            if (UseSingleFilter)
            {
                modelQuery = from model in Central.ListOfModels
                             where model.Name == Workflow.TargetOrder
                             && model.Aim == Workflow.TargetLine
                             select model;
            }
            else
            {
                modelQuery = from model in Central.ListOfModels
                             where model.Name == _orderToUpdate
                             && model.Aim == _line
                             select model;

                Workflow.TargetOrder = _orderToUpdate;
                Workflow.TargetLine = _line;
            }
            if (modelQuery.ToList().Count <= 0)
            {
                MessageBox.Show("The unfinished structure or anomaly has been detected.",
                    "Workflow controller",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }
            var selIdx = dgvReport.CurrentCell.RowIndex;
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

            var si = new Split(_orderToUpdate, _line, _department);
            si.StartPosition = FormStartPosition.CenterScreen;
            si.Show();

            si.FormClosing += (s, ev) =>
            {
                //clear gantt parameters
                Workflow.TargetOrder = string.Empty;
                Workflow.TargetLine = string.Empty;

                frmTransp.Close();
            };

            frmTransp.Click += (s, ev) =>
            {
                si.Close();
                frmTransp.Close();
            };
        }

        /// <summary>
        /// The DgvReport_CellMouseEnter
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="DataGridViewCellEventArgs"/></param>
        private void DgvReport_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvReport.Rows[e.RowIndex].Cells["Linea"].Value.ToString() == string.Empty) return;
                var dept = dgvReport.Rows[e.RowIndex].Cells["Department"].Value.ToString();
                var ln = (from line in Models.Tables.Lines
                          where line.Line == dgvReport.Rows[e.RowIndex].Cells["Linea"].Value.ToString() && line.Department == dept
                          select line);
                var sb = new StringBuilder();
                foreach (var itemp in ln)
                {
                    sb.AppendLine("Nr. Persone: " + itemp.Members);
                    sb.AppendLine("Efficienza: " + itemp.Abatimento.ToString());
                }
                dgvReport.Rows[e.RowIndex].Cells["Linea"].ToolTipText = sb.ToString();
            }
            catch
            {
            }
        }

        /// <summary>
        /// The GetTotals
        /// </summary>
        /// <returns>The <see cref="object[]"/></returns>
        private object[] GetTotals()
        {
            var totQ = 0;
            var totC = 0;
            var totD = 0;
            var tempoStazMedia = 0.0;
            var tempoTotStazMedia = 0.0;
            var ritardoMedia = 0.0;
            var mediaCapiHour = 0.0;
            var ritardo = 0;

            var tszCount = 0.0;
            var tszTotCount = 0.0;
            var rmCount = 0.0;
            var capiHourCount = 0.0;
            var ritardocount = 0;

            foreach (DataGridViewRow row in dgvReport.Rows)
            {
                int.TryParse(row.Cells[4].Value.ToString(), out var q);
                int.TryParse(row.Cells[5].Value.ToString(), out var c);
                int.TryParse(row.Cells[6].Value.ToString(), out var d);
                
                totQ += q;
                totC += c;
                totD += d;
                
                int.TryParse(row.Cells[29].Value.ToString(), out var tz);
                int.TryParse(row.Cells[35].Value.ToString(), out var totTz);
                int.TryParse(row.Cells[36].Value.ToString(), out var totRm);
                double.TryParse(row.Cells["Minuti"].Value.ToString(), out var totCapiHour);
                int.TryParse(row.Cells["Ritardo"].Value.ToString(), out var ritardoc);

                if (tz != 0)
                {
                    tszCount++;
                    tempoStazMedia += tz;
                }
                if (totTz != 0)
                {
                    tszTotCount++;
                    tempoTotStazMedia += totTz;
                }
                if (totRm != 0)
                {
                    rmCount++;
                    ritardoMedia += totRm;
                }
                if (totCapiHour != 0)
                {
                    capiHourCount+=1;
                    mediaCapiHour += totCapiHour;
                }
                if (ritardoc != 0)
                {
                    ritardocount++;
                    ritardo += ritardoc;
                }
            }

            if (tempoStazMedia <= 0)
            {
                tempoStazMedia = 0;
            }
            else
            {
                tempoStazMedia /= tszCount;
            }

            if (tempoTotStazMedia <= 0)
            {
                tempoTotStazMedia = 0;
            }
            else
            {
                tempoTotStazMedia /= tszTotCount;
            }
            if (ritardoMedia <= 0)
            {
                ritardoMedia = 0;
            }
            else
            {
                ritardoMedia /= rmCount;
            }
            if (mediaCapiHour <= 0.0) 
                mediaCapiHour = 0.0; 
            else 
                mediaCapiHour /= capiHourCount;

            if (ritardo < 0)
                ritardo = 0;
            else
            {
                if (ritardocount == 0) ritardocount = 1;
                ritardo /= ritardocount;
            }

            return new object[] { totQ, totC, totD, 
                Convert.ToInt32(tempoStazMedia), 
                Convert.ToInt32(tempoTotStazMedia),Convert.ToInt32(ritardoMedia), Math.Round(mediaCapiHour, 2 ), ritardo};
        }
        /// <summary>
        /// The DgvReport_CellPainting
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="DataGridViewCellPaintingEventArgs"/></param>
        private void DgvReport_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
                
        }
        /// <summary>
        /// The ResetToolStripMenuItem_Click
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void ResetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selIdx = dgvReport.CurrentCell.RowIndex;
            if (dgvReport.Rows.Count == 0)
            {
                selIdx = 0;
            }

            if (string.IsNullOrEmpty(dgvReport.Rows[selIdx].Cells[3].Value.ToString()))
            {
                return;
            }
            if (_lstOfSplittedOrd.Contains(_orderToUpdate))
            {
                MessageBox.Show("Cannot reset splitted order.",
                    "Splitted task",
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            var modelQuery = (from model in Central.ListOfModels
                              where model.Name == _orderToUpdate
                              select model).ToList();
            foreach (var model in modelQuery)
            {
                if (model.HasProduction || model.ClosedByUser)
                {
                    MessageBox.Show("Not possible to reset order which has interactive state.\n" +
                        "Please check workflow status for more information.",
                        "Workflow controller",
                        MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            }
            var dept = dgvReport.Rows[selIdx].Cells["Department"].Value.ToString();
            using (var ctx = new System.Data.Linq.DataContext(Central.ConnStr))
            // update job aim
            {
                ctx.ExecuteCommand("update comenzi set DataProduzione=null,DataFine=null,Line=null,QtyInstead=null where NrComanda={0}" +
                    " and Department={1}",
                    _orderToUpdate, dept);
            }
            var line = dgvReport.Rows[selIdx].Cells[3].Value.ToString();
            using (var ctx = new System.Data.Linq.DataContext(Central.SpecialConnStr))
            // update job aim
            {
                ctx.ExecuteCommand("delete from objects where ordername={0} and aim={1} and department={2}",
                    _orderToUpdate, line, dept);
            }
            Config.InsertOperationLog("manual_programmingremoval", _orderToUpdate + "-" + line + "-" + 
                Store.Default.selDept, "caricolavoro");

            //var query = from ord in ObjectModels.Tables.Orders
            //            where ord.NrComanda == _orderToUpdate
            //            select ord;
            //foreach (var item in query)
            //{
            //    item.Line = null;
            //    item.DataProduzione = null;
            //    item.DataFine = null;
            //}
            //Config.GetOlyConn().SubmitChanges();
            var menu = new Central();
            menu.GetBase();
            dgvReport.Rows[selIdx].Cells[3].Value = null;
            dgvReport.Rows[selIdx].Cells[8].Value = null;
            dgvReport.Rows[selIdx].Cells[9].Value = null;
            if (_listOfAcceptedOrder.Contains(_orderToUpdate))
                _listOfAcceptedOrder.Remove(_orderToUpdate);
            dgvReport.CurrentRow.Cells[0].Value =
                ReturnImageByState(_orderToUpdate);
        }

        /// <summary>
        /// The LoadExcelData
        /// </summary>
        public void LoadExcelData()
        {
            var lbl = new Label();
            lbl.Image = Properties.Resources.icons_excel_filled_32;
            lbl.ImageAlign = ContentAlignment.MiddleCenter;
            lbl.Dock = DockStyle.Fill;
            lbl.BackColor = Color.White;
            lbl.Font = new Font("Tahoma", 18, FontStyle.Bold);
            lbl.TextAlign = ContentAlignment.MiddleCenter;
            lbl.Text = "\n\n\n" + "Preparing Excel worksheet... Please wait";
            Controls.Add(lbl);
            lbl.BringToFront();

            var dt = new DataTable();
            dt.Columns.Add("Unità Produttiva Riassegnata");
            dt.Columns.Add("Lotto");
            dt.Columns.Add("Articolo");
            dt.Columns.Add("Collezione");
            dt.Columns.Add("LINEA");
            dt.Columns.Add("Data arrivo");
            dt.Columns.Add("Note Commessa");
            dt.Columns.Add("Data prevista consegna");
            dt.Columns.Add("Data RDD");
            dt.Columns.Add("Finezza");
            dt.Columns.Add("NOTA_FASE");
            dt.Columns.Add("Capi commessa");
            dt.Columns.Add("Carico");
            dt.Columns.Add("Capi Rientrati OK (ASS)");
            dt.Columns.Add("Capi Assegnati -ORE-");
            dt.Columns.Add("Diff");

            var totRow = dt.NewRow();
            dt.Rows.Add(totRow);

            foreach (DataGridViewRow row in dgvReport.Rows)
            {
                var tess = row.Cells["Tess"].Value.ToString();
                var comm = row.Cells["Commessa"].Value.ToString();
                var art = row.Cells["Articol"].Value.ToString();
                var line = row.Cells["Linea"].Value.ToString();

                string rdd;
                string conseg;
                string arrivo;

                if (row.Cells["Rdd"].Value.ToString() == string.Empty)
                {
                    rdd = string.Empty;
                }
                else
                {
                    rdd = DateTime.ParseExact(row.Cells["Rdd"].Value.ToString(), "dd/MM/yyyy", 
                        System.Globalization.CultureInfo.InvariantCulture).ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                }

                if (row.Cells["Liv"].Value.ToString() == string.Empty)
                {
                    conseg = string.Empty;
                }
                else
                {
                    conseg = DateTime.ParseExact(row.Cells["Liv"].Value.ToString(), "dd/MM/yyyy",
                        System.Globalization.CultureInfo.InvariantCulture).ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                }
                if (row.Cells["Lans"].Value.ToString() == string.Empty)
                {
                    arrivo = string.Empty;
                }
                else
                {
                    arrivo = DateTime.ParseExact(row.Cells["Lans"].Value.ToString(), "dd/MM/yyyy",
                        System.Globalization.CultureInfo.InvariantCulture).ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                }
                var note = row.Cells["Note"].Value.ToString();
                int.TryParse(row.Cells["Qty"].Value.ToString(), out var qty);
                int.TryParse(row.Cells["Carico"].Value.ToString(), out var carico);
                var collection = Collection(art, 0);
                var finest = Collection(art, 1);
                int.TryParse(row.Cells["Diff_t"].Value.ToString(), out var diff);
                double.TryParse(row.Cells["Minuti"].Value.ToString(), out var capiOra);
                var assegnOre = 0.0;
                var difOre = 0.0;
                if (capiOra > 0 && qty > 0)
                {
                    assegnOre = Math.Round(Convert.ToDouble(qty) / capiOra, 0);
                    difOre = Math.Round(Convert.ToDouble(diff) / capiOra, 0);
                }
                var newRow = dt.NewRow();
                newRow[0] = tess; //a
                newRow[1] = comm;
                newRow[2] = art;
                newRow[3] = collection;
                newRow[4] = line;
                newRow[5] = arrivo;     //f
                newRow[6] = tess;
                newRow[7] = conseg;     //h
                newRow[8] = rdd;    //i
                newRow[9] = finest;
                newRow[10] = note;
                newRow[11] = qty;  //l
                newRow[12] = carico;
                newRow[13] = diff;
                newRow[14] = assegnOre;
                newRow[15] = difOre; //p
                dt.Rows.Add(newRow);
            }
            Microsoft.Office.Interop.Excel._Application appExcel = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel._Workbook workbook = appExcel.Workbooks.Add(Type.Missing);
            Microsoft.Office.Interop.Excel._Worksheet worksheet = null;
            var fileName = DateTime.Now.ToString("mm-ss-ff") +
                " Programma Un. Prod. Riassegnata fase CF " + DateTime.Now.ToString("dd-MM-yyyy") +
                " OLIKNITCF";
            worksheet = workbook.ActiveSheet;
            worksheet.Name = "Riassegnata fase CF";
            var crIdx = 1;
            var ccIdx = 1;
            for (var i = 0; i <= dt.Rows.Count - 1; i++)
            {
                DataRow r = dt.Rows[i];
                for (var c = 0; c <= dt.Columns.Count - 1; c++)
                {
                    if (crIdx == 2)
                        worksheet.Cells[crIdx, ccIdx] = dt.Columns[c].ColumnName;
                    else
                    {
                        worksheet.Cells[crIdx, ccIdx] = r.ItemArray.GetValue(c).ToString();
                    }
                    ccIdx++;
                }
                ccIdx = 1;
                crIdx++;
            }
            var xlCells = worksheet.Cells;
            Microsoft.Office.Interop.Excel.Range entireRow = xlCells.EntireRow;
            xlCells.Range["A:P"].AutoFilter(Microsoft.Office.Interop.Excel.XlAutoFilterOperator.xlFilterValues);
            xlCells.Range["A2:P2"].RowHeight = 80;
            xlCells.Range["A2:P2"].Font.Bold = true;
            xlCells.Range["A1:P1"].RowHeight = 50;
            xlCells.Range["A1:P1"].Font.Color = Color.Red;
            xlCells.Range["A1:P1"].Font.Bold = true;
            xlCells.Range["A2:P2"].Interior.Color = Color.Yellow;
            var totalRow = worksheet.Range["A3"].End[Microsoft.Office.Interop.Excel.XlDirection.xlDown].Row;
            worksheet.Range["L1"].Formula = "=SUBTOTAL(9,L3:L" + totalRow.ToString() + ")";
            worksheet.Range["M1"].Formula = "=SUBTOTAL(9,M3:M" + totalRow.ToString() + ")";
            worksheet.Range["N1"].Formula = "=SUBTOTAL(9,N3:N" + totalRow.ToString() + ")";
            worksheet.Range["O1"].Formula = "=SUBTOTAL(9,O3:O" + totalRow.ToString() + ")";
            worksheet.Range["P1"].Formula = "=SUBTOTAL(9,P3:P" + totalRow.ToString() + ")";
            xlCells.Range["I3:I" + totalRow.ToString()].Interior.Color = Color.FromArgb(217, 226, 243);
            xlCells.Range["I3:I" + totalRow.ToString()].Borders.Color = Color.Silver;
            xlCells.Range["A:P"].EntireColumn.AutoFit();
            xlCells.Range["A2:P2"].Borders.Color = Color.Silver;
            xlCells.Range["A1"].Value = fileName;
            xlCells.Range["A1"].Font.Color = Color.Black;
            xlCells.Range["A1"].Font.Size = 20;
            xlCells.Range["A1"].Font.Bold = true;
            xlCells.Range["A1:H1"].Merge();
            //f,h,i
            xlCells.Range["F3"].EntireColumn.NumberFormat = "dd-MM-yyyy";
            xlCells.Range["H3"].EntireColumn.NumberFormat = "dd-MM-yyyy";
            xlCells.Range["I3"].EntireColumn.NumberFormat = "dd-MM-yyyy";
            var saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            saveDialog.FilterIndex = 1;
            saveDialog.FileName = fileName;
            if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                workbook.SaveAs(saveDialog.FileName);
            }
            appExcel.Quit();
            workbook = null;
            appExcel = null;                      
            try
            {
                System.Diagnostics.Process.Start(saveDialog.FileName);               
            }
            catch
            {
            }
            finally
            {
                lbl.Dispose();
            }
        }

        /// <summary>
        /// The Collection
        /// </summary>
        /// <param name="a">The a<see cref="string"/></param>
        /// <param name="part">The part<see cref="int"/></param>
        /// <returns>The <see cref="string"/></returns>
        private string Collection(string a, int part)
        {
            var str = "";
            var q = (from art in Models.Tables.Articles
                     where art.Articol == a
                     select new { art.Finete, art.Stagione });
            var lst = q.ToList();
            if (lst.Count == 0) return string.Empty;
            foreach (var l in lst) str = l.Stagione + "_" + l.Finete;

            return str.Split('_')[part];
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            HideLightColumns();
        }

        public void InsertNewProgram(string order,string line,string article,int qty,double qtyH, DateTime startDate,int duration, int dailyQty, double price, string dept) 
        {
            if (Store.Default.selDept == ",Stiro,")
            {
                var id = 0;

                var op = new OperationProgram();
                if (op.ShowDialog() == DialogResult.OK)
                {
                    id = op.OperationId;
                }

                var artId = 0;
                var artQuery = "select id from Articole where articol='" + article + "' and idsector=2";
                using (var c = new SqlConnection(Central.ConnStr))
                {
                    var cmd = new SqlCommand(artQuery, c);
                    c.Open();
                    var dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            int.TryParse(dr[0].ToString(), out artId);
                        }
                    }
                    c.Close();
                }

                var operatQuery = "select Centes from OperatiiArticol where idOperatie='" + id.ToString() + "' and IdSector=2" +
                    " and idarticol='" + artId + "'";

                using (var c = new SqlConnection(Central.ConnStr))
                {
                    var cmd = new SqlCommand(operatQuery, c);
                    c.Open();
                    var dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            double.TryParse(dr[0].ToString(), out qtyH);
                        }
                    }
                    c.Close();
                }

                if (qtyH == 0.0)
                {
                    MessageBox.Show("QtyH is not valid, or operation isn't defined");
                }
            }

            var q = "insert into objects (ordername,aim,article,stateid,loadedqty,qtyh,startdate,duration,enddate,dvc,rdd,startprod,endprod,dailyprod,prodqty, " +
               "overqty,prodoverdays,delayts,prodoverts,locked,holiday,closedord,artprice,hasprod,lockedprod,delaystart,delayend,doneprod,base,department) values " +
               "(@param1,@param2,@param3,@param4,@param5,@param6,@param7,@param8,@param9,@param10," +
               "@param11,@param12,@param13,@param14,@param15,@param16,@param17,@param18,@param19," +
               "@param20,@param21,@param22,@param23,@param24,@param25,@param26,@param27,@param28,@param29,@param30)";

            var eDate = startDate.AddDays(+duration);

            var lineDesc = (from lines in Tables.Lines
                       where lines.Description == line && lines.Department == dept
                       select lines).SingleOrDefault();

            var lineDescription = lineDesc != null ? lineDesc.Line : line;
            using (var con = new System.Data.SqlClient.SqlConnection(Central.SpecialConnStr))
            {
                var cmd = new System.Data.SqlClient.SqlCommand(q, con);
                cmd.Parameters.Add("@param1", SqlDbType.NVarChar).Value = order;
                cmd.Parameters.Add("@param2", SqlDbType.NVarChar).Value = lineDescription;
                cmd.Parameters.Add("@param3", SqlDbType.NVarChar).Value = article;
                cmd.Parameters.Add("@param4", SqlDbType.Int).Value = 1;
                cmd.Parameters.Add("@param5", SqlDbType.Int).Value = qty;
                cmd.Parameters.Add("@param6", SqlDbType.Float).Value = qtyH;
                cmd.Parameters.Add("@param7", System.Data.SqlDbType.BigInt).Value = JobModel.GetLSpan(startDate);
                cmd.Parameters.Add("@param8", System.Data.SqlDbType.Int).Value = duration;
                cmd.Parameters.Add("@param9", System.Data.SqlDbType.BigInt).Value = JobModel.GetLSpan(eDate);
                cmd.Parameters.Add("@param10", System.Data.SqlDbType.BigInt).Value = 0;
                cmd.Parameters.Add("@param11", System.Data.SqlDbType.BigInt).Value = 0;
                cmd.Parameters.Add("@param12", System.Data.SqlDbType.BigInt).Value = 0;
                cmd.Parameters.Add("@param13", System.Data.SqlDbType.BigInt).Value = 0;
                cmd.Parameters.Add("@param14", System.Data.SqlDbType.Int).Value = dailyQty;
                cmd.Parameters.Add("@param15", System.Data.SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@param16", System.Data.SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@param17", System.Data.SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@param18", System.Data.SqlDbType.BigInt).Value = 0;
                cmd.Parameters.Add("@param19", System.Data.SqlDbType.BigInt).Value = 0;
                cmd.Parameters.Add("@param20", System.Data.SqlDbType.Bit).Value = false;
                cmd.Parameters.Add("@param21", System.Data.SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@param22", System.Data.SqlDbType.Bit).Value = false;
                cmd.Parameters.Add("@param23", System.Data.SqlDbType.Float).Value = price;
                cmd.Parameters.Add("@param24", System.Data.SqlDbType.Bit).Value = false;
                cmd.Parameters.Add("@param25", System.Data.SqlDbType.Bit).Value = false;
                cmd.Parameters.Add("@param26", System.Data.SqlDbType.BigInt).Value = 0;
                cmd.Parameters.Add("@param27", System.Data.SqlDbType.BigInt).Value = 0;
                cmd.Parameters.Add("@param28", System.Data.SqlDbType.Bit).Value = false;
                cmd.Parameters.Add("@param29", System.Data.SqlDbType.Bit).Value = true;
                cmd.Parameters.Add("@param30", System.Data.SqlDbType.NVarChar).Value = dept;

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            Config.InsertOperationLog("manual_programming", order + "-" + line + "-" + dept, "caricolavoro");
        }
    }
}
