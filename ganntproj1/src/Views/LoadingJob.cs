namespace ganntproj1
{
    using ganntproj1.Models;
    using ganntproj1.Properties;
    using ganntproj1.src.Helpers;
    using ganntproj1.src.Views;
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

    public partial class LoadingJob : Form
    {
        private readonly Config _config = new Config();

        private delegate void FillGridDelegate(SqlDataReader reader);

        private DataTable _tableCarico = new DataTable();

        private AutoCompleteStringCollection _acsc = new AutoCompleteStringCollection();

        private AutoCompleteStringCollection _acscArt = new AutoCompleteStringCollection();

        private AutoCompleteStringCollection _ascsLine = new AutoCompleteStringCollection();

        private AutoCompleteStringCollection _ascsTest = new AutoCompleteStringCollection();

        private List<string> _listOfAcceptedOrder = new List<string>();

        private List<string> _listOfOrdersWithNotes = new List<string>();

        private List<string> _listOfLinesComplete = new List<string>();

        public bool UseSingleFilter { get; set; }

        public bool IsUpd { get; set; }

        public static bool HasOrderSelected { get; private set; }

        public LoadingJob(bool updMode)
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;          
            IsUpd = updMode;
            dgvReport.DoubleBuffered(true);

            dgvReport.DataBindingComplete += (s, ev) =>
            {
                if (dgvReport.Columns.Count > 37)
                {
                    dgvReport.Columns[38].DefaultCellStyle.Font = new Font("Verdana", 8, FontStyle.Bold);
                    dgvReport.Columns[38].Width = 20;
                    dgvReport.Columns[38].DefaultCellStyle.BackColor = Color.WhiteSmoke;
                }  
            };
        }

        private void CreateDataTable()
        {
            _tableCarico = new DataTable();

            _tableCarico.Columns.Add("Flag", typeof(Image));
            _tableCarico.Columns["Flag"].DefaultValue = null;
            _tableCarico.Columns.Add("Commessa");
            _tableCarico.Columns.Add("Articol");
            _tableCarico.Columns.Add("Linea");  
            _tableCarico.Columns.Add("Qty");
            _tableCarico.Columns.Add("Carico"); 
            _tableCarico.Columns.Add("Diff_t");
            _tableCarico.Columns.Add("Lans");
            _tableCarico.Columns.Add("Prod");
            _tableCarico.Columns.Add("Fin");
            _tableCarico.Columns.Add("Liv");
            _tableCarico.Columns.Add("Rdd");    
            _tableCarico.Columns.Add("Dvc");
            _tableCarico.Columns.Add("Minuti"); 
            _tableCarico.Columns.Add("Giorni");
            _tableCarico.Columns.Add("Tess");
            _tableCarico.Columns.Add("Conf");
            _tableCarico.Columns.Add("Respinte");
            _tableCarico.Columns.Add("Conseg");
            _tableCarico.Columns.Add("Diff_c");
            _tableCarico.Columns.Add("Department"); 
            _tableCarico.Columns.Add("Note");
            _tableCarico.Columns.Add("IdState");
            _tableCarico.Columns.Add("Ritardo");
            _tableCarico.Columns.Add("FlagA", typeof(Image));
            _tableCarico.Columns["FlagA"].DefaultValue = null;
            _tableCarico.Columns.Add("FlagB", typeof(string));
            _tableCarico.Columns.Add("DataInizio Prod");
            _tableCarico.Columns.Add("DataFine Prod");
            _tableCarico.Columns.Add("CaricoTrigger"); 
            _tableCarico.Columns.Add("TempStat"); 
            _tableCarico.Columns.Add("orderId"); 
            _tableCarico.Columns.Add("Ramm Tess"); 
            _tableCarico.Columns.Add("Ramm Conf");
            _tableCarico.Columns.Add("Dft Tess");
            _tableCarico.Columns.Add("Dft Conf"); 
            _tableCarico.Columns.Add("TempoTotStaz");
            _tableCarico.Columns.Add("RitardoMedia");   
            _tableCarico.Columns.Add("Prezzo"); 
            _tableCarico.Columns.Add("FlagC", typeof(string));
            _tableCarico.Columns["FlagC"].DefaultValue = "+";
        }

        private List<string> _lstOfListedArts = new List<string>();

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
                            pe.Graphics.DrawString("Comm. ",
                                new Font("Tahoma", 9, FontStyle.Regular), new SolidBrush(lbl.ForeColor), 5, 5);
                            pe.Graphics.DrawString("      " + name,
                              new Font("Tahoma", 9, FontStyle.Bold), new SolidBrush(lbl.ForeColor), 25, 5);
                            pe.Graphics.DrawString("                      RDD",
                        new Font("Tahoma", 9, FontStyle.Regular), new SolidBrush(lbl.ForeColor), 25, 5);
                            pe.Graphics.DrawString(art,
                                new Font("tahoma", 9, FontStyle.Bold), new SolidBrush(lbl.ForeColor), 5, 25);
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

                            if (Store.Default.sectorId == 8)
                            {

                                foreach (DataGridViewRow dRow in dgvReport.Rows)
                                {
                                    if (dRow.Cells[1].Value.ToString() == lbl.Name.Split('_')[0])
                                    {
                                        dgvReport.CurrentCell = dRow.Cells[0];
                                    }
                                }

                                return;
                            }
                            else
                            {
                                var isParent = CheckOrderHasChildren(art);
                                if (isParent)
                                {
                                    MessageBox.Show("Cannot program from main order.");
                                    return;
                                }
                            }

                            Workflow.TargetOrder = lbl.Name.Split('_')[0];

                            var programDialog = new ProgramationControl(Workflow.TargetOrder, Workflow.TargetLine, Workflow.TargetDepartment, 
                                Workflow.ManualDateTime,Workflow.TargetProgramDate, Workflow.Article, qty, carico);
                         
                            if (programDialog.ShowDialog() != DialogResult.Cancel)
                            {
                                Workflow.ByManualDate = programDialog.UseManualDate;
                                Workflow.Members = programDialog.Members;
                                Workflow.ManualDateTime = programDialog.DateTimes;
                                Workflow.ByQty = programDialog.ByTotalQty;
                                Workflow.TargetQtyH = programDialog.QtyH;
                                Workflow.TargetLaunched = programDialog.Launched;

                                Close();
                            }
                            else
                            {
                                return;
                            }
                        };
                        pnArticles.Controls.Add(lbl);
                        lbl.BringToFront();
                        lbl.Refresh();
                        _lstOfListedArts.Add(art);
                        leftObjLocation += lbl.Width + 3;
                    }
                }
            }

            AddHeaderButtons();
            base.OnLoad(e);
        }

        public void LoadCaricoLavoro()
        {
            Workflow.ListOfRemovedOrders.Clear();
            LoadCaricoLavoroInternal();
        }

        private void LoadCaricoLavoroInternal()
        {
            RemoveGridControls();
            _listOfAcceptedOrder = new List<string>();
            _listOfOrdersWithNotes = new List<string>();
            _listOfLinesComplete = new List<string>();
            foreach (var item in Central.TaskList)
                _listOfAcceptedOrder.Add(item.Name);
            foreach (var item in _lstOfSplittedOrd)
                if (_listOfAcceptedOrder.Contains(item)) _listOfAcceptedOrder.Remove(item);
            var sector = "";              		  
            var from = $"{Central.DateFrom.Year}-{Central.DateFrom.Month}-{Central.DateFrom.Day}";
            var to = $"{Central.DateTo.Year}-{Central.DateTo.Month}-{Central.DateTo.Day}";
            Central.IdStateArray.Clear();
            if (!Central.IsAcconto && !Central.IsSaldo && !Central.IsChiuso)
                Central.IdStateArray.Append(",1,2,3,4,");        
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
                sector +   
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
                var strFilter = "";
                if (Store.Default.arrivoOrder)
                {
                    strFilter = "Comenzi.DataLansare";
                }
                else
                {
                    strFilter = "Comenzi.Rdd";
                }

                if (IsUpd)
                {
                    queryCondition = "where Comenzi.DataProduzione is null and Comenzi.department='" + Workflow.TargetDepartment + "' " +
                                      "and Comenzi.IdStare=4 and  Comenzi.isdeleted='0' or Comenzi.Line is null and Comenzi.department='" + Workflow.TargetDepartment + "' " +
                                      "and Comenzi.IdStare=4 and  Comenzi.isdeleted='0' and Comenzi.department='" + Workflow.TargetDepartment + "' " +
                                      "and Comenzi.IdStare=4 and Comenzi.DataProduzione is null " +
                                      "and Comenzi.Line is null and Comenzi.isdeleted=0 " + 
                                      "order by case when " + strFilter + " is null then 1 else 0 end, " + strFilter;
                }
                else
                {
                    queryCondition = "where Comenzi.DataProduzione is null and charindex(+',' + Comenzi.department + ',', '" + Store.Default.selDept + "') > 0 " +
                        "and Comenzi.IdStare=4 and Comenzi.isdeleted='0' or Comenzi.Line is null and charindex(+',' + Comenzi.department + ',', '" + Store.Default.selDept + "') > 0 " +
                        "and Comenzi.IdStare=4 and Comenzi.isdeleted='0' and charindex(+',' + Comenzi.department + ',', '" + Store.Default.selDept + "') > 0 " +
                        "and Comenzi.IdStare=4 and Comenzi.DataProduzione is null " +
                        "and Comenzi.Line is null and Comenzi.isdeleted='0' " +
                        "order by case when " + strFilter + " is null then 1 else 0 end, " + strFilter;
                }
            }
            if (UseSingleFilter)
            {
                queryCondition = "where Comenzi.NrComanda='" + Workflow.TargetOrder + "' "
                    + "and comenzi.line='" + Workflow.TargetLine + "' and comenzi.department='" + Workflow.TargetDepartment + "'";
            }

            var strLine = IsUpd ? "case when Comenzi.Line is not null then null else Comenzi.Line end as Line," : "Comenzi.Line,";

            var query = "select " +
                     "Comenzi.NrComanda," +          
                     "Articole.Articol," +
                     strLine +           
                     "Comenzi.Cantitate," +      
                     "Comenzi.Carico," +             
                     "Comenzi.Diff_t," +             
                     "Comenzi.DataLansare," +        
                     "Comenzi.DataProduzione," +     
                     "Comenzi.DataFine," +           
                     "Comenzi.DataLivrare," +        
                     "Comenzi.RDD," +                
                     "Comenzi.DVC," +               
                     "Articole.Centes," +            
                     "Comenzi.GiorniLavorati as Gironi, " +    
                     "Comenzi.Tessitura as Tess," +       
                     "Comenzi.Confezione as Conf," +     
                     "Comenzi.Respinte," +       
                     "Comenzi.Consegnato as Conseg, " +     
                     "Comenzi.Diff_c," +         
                     "Comenzi.Department," +     
                     "Comenzi.Note," +
                     "Comenzi.IdStare," +
                     "Comenzi.CaricoTrigger, " +
                     "Comenzi.Id, " +
                     "Comenzi.ram_tess," +
                     "Comenzi.ram_conf," +
                     "Comenzi.def_tess," +
                     "Comenzi.def_conf, " +
                     "Articole.Prezzo, " +
                     "Comenzi.Programmed as programmed " +
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
            var totCons = 0;
            var totDifC = 0;

            foreach (DataRow row in table.Rows)
            {
                if (!string.IsNullOrEmpty(row[20].ToString()))
                {
                    _listOfOrdersWithNotes.Add(row[0].ToString());
                }
                
                int.TryParse(row[17].ToString(), out var conseg);
                int.TryParse(row[18].ToString(), out var difc);
                totCons += conseg;
                totDifC += difc;
                
                var newRow = _tableCarico.NewRow();
                var artFixRow = _tableCarico.NewRow();
                var job = row[0].ToString();
                var line = row[2].ToString();
                var dept = row[19].ToString();
                var jobSpostStart = "";
                var jobSpostEnd = "";
                var jobModel = Central.TaskList.LastOrDefault(x => x.Name == job && x.Aim == line && x.Department == dept);

                if (IsUpd && jobModel != null) return;

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
                bool.TryParse(row["programmed"].ToString(), out var programmed);
                newRow[0] = ReturnImageByState(row[0].ToString(),programmed);
                newRow[1] = job;
                newRow[2] = row[1].ToString();
                newRow[3] = GetDescriptionInsteadOfLine(line, dept);   

                int.TryParse(row[3].ToString(), out var qty);
                int.TryParse(row[4].ToString(), out var carico);
                int.TryParse(row[5].ToString(), out var diff);

                newRow[4] = qty;
                newRow[5] = carico;
                newRow[6] = diff;

                newRow[7] = UniParseDateTime(row[6]);
                newRow[8] = UniParseDateTime(row[7]); 
                newRow[9] = UniParseDateTime(row[8]); 
                newRow[10] = UniParseDateTime(row[9]);    
                newRow[11] = UniParseDateTime(row[10]);
                newRow[12] = UniParseDateTime(row[11]);
                newRow[13] = row[12].ToString();
                newRow[14] = row[13].ToString();
                newRow[15] = row[14].ToString();    
                newRow[16] = row[15].ToString();    
                newRow[17] = row[16].ToString();
                newRow[18] = row[17].ToString();
                newRow[19] = row[18].ToString();
                newRow[20] = row[19].ToString();
                newRow[21] = row[20].ToString();
                newRow[22] = row[21].ToString();
                newRow[24] = ReturnImageByNote(row[0].ToString());
                DateTime.TryParse(row[6].ToString(), out var arrivo);
                DateTime.TryParse(row[7].ToString(), out var start);
                DateTime.TryParse(row[8].ToString(), out var end);

                if (start != DateTime.MinValue && end != DateTime.MinValue)
                {
                    var duration = end.Subtract(start).Days;
                    var strDuration = duration > 0 ? duration.ToString() : "1";
                    newRow[25] = strDuration;  
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

            if (dgvReport.DataSource != null) dgvReport.DataSource = null;           
            dgvReport.DataSource = _tableCarico;

            dgvReport.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvReport.Columns["Flag"].Width = 35;
            dgvReport.Columns["Flag"].HeaderText = "State";
            dgvReport.Columns["Flag"].HeaderCell.Style.BackColor = Color.FromArgb(50, 52, 68);
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

            dgvReport.Columns[38].HeaderText = "";
            dgvReport.Columns[38].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            dgvReport.Columns[38].HeaderCell.Style.BackColor = Color.FromArgb(50, 52, 68);
            dgvReport.Columns[38].DisplayIndex = 0;
            
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
            dgvReport.Columns[18].HeaderText = "Conseg.\n\n" + totCons.ToString();
            dgvReport.Columns[19].HeaderText = "Diff_c\n\n" + totDifC.ToString();

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
            dgvReport.Columns["FlagC"].DefaultCellStyle.BackColor = Color.White;
            dgvReport.Columns[18].Width = 60;
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

            if (Store.Default.sectorId != 8)
            {
                dgvReport.Columns[38].Visible = false;
            }
            else
            {
                dgvReport.Columns[38].Width = 20;
                dgvReport.Columns[38].DefaultCellStyle.Font = new Font("Verdana", 8, FontStyle.Bold);
                dgvReport.Columns[38].DefaultCellStyle.ForeColor = Color.Black;
            }

            _acsc = new AutoCompleteStringCollection();
            _acscArt = new AutoCompleteStringCollection();
            _ascsLine = new AutoCompleteStringCollection();

            CreateFilter();
        }
        private List<string> _lstOfSplittedOrd = new List<string>();
        private List<string> GetSplittedOrders()
        {
            var query = from split in Central.TaskList
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
        private Image ReturnImageByState(string ord, bool programmed = false)
        {
            Image img1 = Resources.order_split_flag_16;
            Image img2 = Resources.trash_16;
            Image img3 = Resources.tick_icon_16;
            Bitmap bmp = new Bitmap(24, 24); 
            Image img = bmp;

            if (!programmed)
            {
                if (_lstOfSplittedOrd.Contains(ord + ".1")) img = img1;
                else if (Workflow.ListOfRemovedOrders.Contains(ord)) img = img2;
                else if (_listOfAcceptedOrder.Contains(ord)) img = img3;
                return img;
            }
            else
            {
                return img3;
            }
        }
        private Image ReturnImageByNote(string ord)
        {
            Image img1 = Properties.Resources.exclamation_16;
            Bitmap bmp = new Bitmap(24, 24);
            Image img = bmp;
            if (_listOfOrdersWithNotes.Contains(ord)) img = img1;
            return img;
        }
        private Image ReturnImageByCompletetion(string ord)
        {
            Image img1 = Properties.Resources.inform_16;
            Bitmap bmp = new Bitmap(24, 24);
            Image img = bmp;
            if (_listOfLinesComplete.Contains(ord)) img = img1;
            return img;
        }

        private Image ReturnImageByExpandBullet()
        {
            Image img1 = Properties.Resources.bullet_toggle_plus;
            return img1;
        }

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
        private bool _filterCreated;
        private TextBox _txtArt;
        private TextBox _txtLin;
        private TextBox _txtTess;

        private ComboBox _cbNote;
        private void CreateFilter()
        {
            if (dgvReport.Rows.Count <= 0) return;
            _acsc.Clear();
            _ascsLine.Clear();
            _acscArt.Clear();
            _ascsTest.Clear();

            if (!dgvReport.Controls.Contains(_cbNote))
            {
                _cbNote = new ComboBox
                {
                    Name = dgvReport.Columns["Note"].Name,
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    BackColor = Color.White,
                    Font = new Font("Microsoft Sans Serif", 10, FontStyle.Regular),
                    Parent = dgvReport
                };
                _cbNote.Items.Clear();
                _cbNote.Items.Add("<Reset>");

                dgvReport.Controls.Add(_cbNote);
            }

            if (!dgvReport.Controls.Contains(_txtTess))
            {
                _txtTess = new TextBox
                {
                    Name = dgvReport.Columns["Tess"].Name,
                    BorderStyle = BorderStyle.None,
                    BackColor = Color.White,
                    Font = new Font("Microsoft Sans Serif", 10, FontStyle.Regular),
                    Parent = dgvReport
                };
                
                dgvReport.Controls.Add(_txtTess);
            }

            foreach (DataGridViewRow row in dgvReport.Rows)
            {
                _acsc.Add(row.Cells[1].Value.ToString());
                _acscArt.Add(row.Cells[2].Value.ToString());
                _ascsLine.Add(row.Cells[3].Value.ToString());
                _ascsTest.Add(row.Cells[15].Value.ToString());
                
                if (_cbNote.Items.Contains(row.Cells[21].Value.ToString())) continue;
                _cbNote.Items.Add(row.Cells[21].Value.ToString());
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


            headerRect = dgvReport.GetColumnDisplayRectangle(15, true);
            _txtTess.Location = new Point(headerRect.Location.X + 1, 50 - _txtTess.Height - 2);
            _txtTess.Size = new Size(headerRect.Width - 3, dgvReport.ColumnHeadersHeight);

            headerRect = dgvReport.GetColumnDisplayRectangle(21, true);
            _cbNote.Location = new Point(headerRect.Location.X + 1, 50 - _cbNote.Height - 2);
            _cbNote.Size = new Size(headerRect.Width - 3, dgvReport.ColumnHeadersHeight);

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
                _tableCarico.DefaultView.RowFilter = _strFilter;
                dgvReport.DataSource = _tableCarico;
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

                _tableCarico.DefaultView.RowFilter = _strFilter;
                dgvReport.DataSource = _tableCarico;
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

                _tableCarico.DefaultView.RowFilter = _strFilter;
                dgvReport.DataSource = _tableCarico;
                dgvReport.Refresh();
            };
            _txtTess.TextChanged += (s, e) =>
            {
                if (_txtTess.Controls.Contains(_btnClearFilter))
                {
                    _txtTess.Controls.Remove(_btnClearFilter);
                    _btnClearFilter.Dispose();
                }
                if (_txtTess.Text != string.Empty)
                {
                    AddClearFilterButton(_txtTess, "btnTess");
                    _txtTess.Controls.Add(_btnClearFilter);
                }
                RemoveGridControls();
                CollectFiltersQuery();

                _tableCarico.DefaultView.RowFilter = _strFilter;
                dgvReport.DataSource = _tableCarico;
                dgvReport.Refresh();
            };
            _cbNote.SelectedIndexChanged += (s, e) =>
            {
                if (_cbNote.SelectedIndex == 0)
                {
                    _tableCarico.DefaultView.RowFilter = _strFilter;
                    dgvReport.DataSource = _tableCarico;
                    dgvReport.Refresh();
                    return;
                }

                var strF = string.Format("CONVERT(" + dgvReport.Columns[_cbNote.Name].DataPropertyName +
                                ", System.String) = '" + _cbNote.Text.Replace("'", "''") + "'");
                _tableCarico.DefaultView.RowFilter = _strFilter;
                dgvReport.DataSource = _tableCarico;
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
                else if (txt.Name == dgvReport.Columns[15].Name)
                {
                    txt.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    txt.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    txt.AutoCompleteCustomSource = _ascsTest;
                }
            }
        }

        private string _strFilter = string.Empty;

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
                                ", System.String) like '%" + txt.Text.Replace("'", "''") + "%'");
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
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);
        private Button _btnClearFilter;
        private void AddClearFilterButton(TextBox txt, string name)
        {
            _btnClearFilter = new Button
            {
                Size = new Size(25, txt.ClientSize.Height + 2)
            };
            _btnClearFilter.Location = new Point(txt.ClientSize.Width - _btnClearFilter.Width, -1);
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
            var art = dgvReport.Rows[e.RowIndex].Cells[2].Value.ToString();

            var isParent = CheckOrderHasChildren(art);

            if (Store.Default.sectorId == 8 && dgvReport.CurrentRow.Cells["FlagC"].Value.ToString() != string.Empty && isParent)
            {
                MessageBox.Show("Main field cannot be programmed.");
                return;
            }

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

            var programDialog = new ProgramationControl(Workflow.TargetOrder, Workflow.TargetLine, 
                Workflow.TargetDepartment, Workflow.TargetProgramDate,DateTime.MinValue,art, qty, carico);

            if (programDialog.ShowDialog() != DialogResult.Cancel)
            {
                Workflow.ByManualDate = programDialog.UseManualDate;
                Workflow.Members = programDialog.Members;
                Workflow.ManualDateTime = programDialog.DateTimes;

                byQty = programDialog.ByTotalQty;

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
                Workflow.TargetLaunched = programDialog.Launched;

                if (Store.Default.sectorId == 2) Workflow.TargetQtyH = programDialog.QtyH;

                Close();
            }
                      
        }

        private ComboBox _cbLineChange;
        private DateTimePicker _dtProdChange;
        private TextBox _txtInput;
        private string _txtOnEnter;
        private string _orderToUpdate;
        private string _line;
        private string _department;
        private int _rowIdx, _cellIdx;
        private Button _btnShowProd;
        private Button _btnShowFin;
        private DataTable _tmpTbl = new DataTable();

        private void dgvReport_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex <= -1) return;

            RemoveGridControls();

            _orderToUpdate = dgvReport.Rows[e.RowIndex].Cells[1].Value.ToString();
            _line = dgvReport.Rows[e.RowIndex].Cells[3].Value.ToString();
            _department = dgvReport.Rows[e.RowIndex].Cells[20].Value.ToString();

            if (e.ColumnIndex == 0)
            {
                if ((from split in Central.TaskList
                     where split.Name == _orderToUpdate
                     select split).ToList().Count > 0)
                {
                    var sph = new SplitHistory(_orderToUpdate, _department);
                    sph.IsFromSplit = false;
                    sph.StartPosition = FormStartPosition.CenterScreen;
                    sph.ShowDialog();
                    sph.Dispose();
                }

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
                _cbLineChange.DropDownWidth = 200;
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

            if (e.ColumnIndex == 10)       
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
                    }
                };
            }

            _txtInput?.Focus();

            if (e.ColumnIndex == 0 && Store.Default.sectorId == 8 && dgvReport.Rows[e.RowIndex].Cells[38].Value.ToString() == "+" 
                || e.ColumnIndex == 0 && Store.Default.sectorId == 8 && dgvReport.Rows[e.RowIndex].Cells[38].Value.ToString() == "-")
            {

                var isprog = false;
                var q = "select programmed from comenzi where nrcomanda=@param1 and department=@param2";
                using (var c = new SqlConnection(Central.ConnStr))
                {
                    var cmd = new SqlCommand(q, c);
                    cmd.Parameters.Add("@param1", SqlDbType.NVarChar).Value = dgvReport.Rows[e.RowIndex].Cells[1].Value.ToString();
                    cmd.Parameters.Add("@param2", SqlDbType.NVarChar).Value = "Sartoria";

                    c.Open();
                    var dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                        while (dr.Read()) bool.TryParse(dr[0].ToString(), out isprog);
                    c.Close();
                    dr.Close();
                    cmd = null;
                }

                var mark = "MARK";
                if (isprog) mark = "UNMARK";

                var msg = MessageBox.Show("Do you want to " + mark + " this order as programmed?", "Carico lavoro", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (msg == DialogResult.Yes)
                {
                    using (var ctx = new System.Data.Linq.DataContext(Central.ConnStr))
                    {
                        ctx.ExecuteCommand("update comenzi set Programmed={0} where NrComanda={1} and Department={2}", !isprog,
                            dgvReport.Rows[e.RowIndex].Cells[1].Value.ToString(), "Sartoria");
                    }

                    dgvReport.Rows[e.RowIndex].Cells[0].Value = ReturnImageByState("", !isprog);
                }
            }

            if (e.ColumnIndex == 38 && Store.Default.sectorId == 8)
            {
                if (dgvReport.Rows[e.RowIndex].Cells[38].Value.ToString() == "+")
                {
                    if (_strFilter != string.Empty && dgvReport.Rows.Count > 1)
                    {
                        MessageBox.Show("To expand this order, just one record must be filtered.","Carico lavoro",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var art = dgvReport.Rows[e.RowIndex].Cells[2].Value.ToString();
                    int.TryParse(dgvReport.Rows[e.RowIndex].Cells[4].Value.ToString(), out var totalQty);
                    int.TryParse(dgvReport.Rows[e.RowIndex].Cells[5].Value.ToString(), out var carico);

                    var q = "select round(cast(6000 / sum(Op.Centes) as float),2), Op.GroupName from OperatiiArticol Op " +
                        "inner join Articole Art on Art.Articol='" + art + "' " +
                        "where Op.IdArticol = Art.Id and groupName is not null " +
                        "group by Op.GroupName";

                    _tmpTbl = new DataTable();

                    if (dgvReport.Rows[e.RowIndex].Cells[38].Value.ToString() == "+")
                    {
                        using (var con = new SqlConnection(Central.ConnStr))
                        {
                            var cmd = new SqlCommand(q, con);
                            con.Open();
                            var dr = cmd.ExecuteReader();
                            if (!dr.HasRows) return;
                            _tmpTbl.Load(dr);
                            con.Close();
                            dr.Close();
                        }
                    }

                    if (_tmpTbl.Rows.Count == 0) return;

                    var idx = 1;
                    
                    foreach (DataRow row in _tmpTbl.Rows)
                    {
                        var capiOra = row[0].ToString();
                        var group = row[1].ToString();

                        var newRow = _tableCarico.NewRow();

                        for (var i = 0; i <= _tableCarico.Columns.Count - 1; i++)
                        {
                            if (i == 38) continue;
                            if (_tableCarico.Columns[i].ColumnName == "Flag" || _tableCarico.Columns[i].ColumnName == "FlagA") continue;
                            newRow[i] = _tableCarico.Rows[e.RowIndex][i].ToString();
                        }

                        var ord = dgvReport.Rows[e.RowIndex].Cells[1].Value.ToString() + "_" + idx.ToString();

                        newRow[0] = ReturnImageByState(ord);                                               
                        newRow[1] = ord;
                        newRow[2] = art;

                        var orderFromGant = (from orders in Central.TaskList
                                             where orders.Name == ord && orders.Department == "Sartoria"
                                             select orders).SingleOrDefault();

                        if (orderFromGant != null)
                        {
                            var lineInsteadDescripton = (from lines in Tables.Lines
                                                         where lines.Line == orderFromGant.Aim && lines.Department == "Sartoria"
                                                         select lines).SingleOrDefault().Description;

                            newRow["Linea"] = lineInsteadDescripton;
                            newRow["Prod"] = UniParseDateTime(orderFromGant.StartDate);
                            newRow["Fin"] = UniParseDateTime(orderFromGant.EndDate);
                        }
                        else
                        {
                            newRow["Linea"] = string.Empty;
                            newRow["Prod"] = string.Empty;
                            newRow["Fin"] = string.Empty;
                        }

                        newRow[3] = group;
                        newRow[4] = totalQty;
                        newRow[5] = carico;
                        newRow[13] = capiOra;
                        newRow[24] = ReturnImageByNote(dgvReport.Rows[e.RowIndex].Cells[3].Value.ToString());
                        newRow[38] = string.Empty;

                        if (dgvReport.Rows.Count == idx)
                        {
                            _tableCarico.Rows.InsertAt(newRow, _tableCarico.Rows.Count + idx);
                            dgvReport.Rows[0 + idx].DefaultCellStyle.BackColor = Color.FromArgb(200, 255, 200);
                            idx++;
                        }
                        else
                        {
                            _tableCarico.Rows.InsertAt(newRow, e.RowIndex + idx);
                            dgvReport.Rows[e.RowIndex + idx].DefaultCellStyle.BackColor = Color.FromArgb(200, 255, 200);
                            idx++;
                        }                       
                    }

                    dgvReport.Rows[e.RowIndex].Cells[38].Value = "-";
                }
                else if (dgvReport.Rows[e.RowIndex].Cells[38].Value.ToString() == "-")
                {
                    var ord = dgvReport.Rows[e.RowIndex].Cells[1].Value.ToString();
                    var art = dgvReport.Rows[e.RowIndex].Cells[2].Value.ToString();

                    dgvReport.BeginEdit(true);
                    var listOfInt = new List<int>(0);
                    foreach (DataGridViewRow item in dgvReport.Rows)
                    {
                        if (item.Cells[1].Value.ToString().Split('_')[0] == ord && item.Cells[2].Value.ToString() == art
                            && item.DefaultCellStyle.BackColor == Color.FromArgb(200, 255, 200))
                        {
                            listOfInt.Add(item.Index);
                        }
                    }

                    if (listOfInt.Count > 0)
                    {
                        var index = listOfInt.First();
                        for (var i = 0; i <= listOfInt.Count - 1; i++)
                        {
                            dgvReport.Rows.RemoveAt(index);
                        }
                    }

                    dgvReport.EndEdit();

                    dgvReport.Rows[e.RowIndex].Cells[38].Value = "+";
                } 
            }
        }

        private void dgvReport_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var comm = dgvReport.Rows[e.RowIndex].Cells[1].Value.ToString();
            if (e.ColumnIndex == 22)
            {
                MessageBox.Show(comm + " Note");
            }
        }

        private void dgvReport_Scroll(object sender, ScrollEventArgs e)
        {
            try
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
                    _btnShowProd.Location = new Point(Rectangle.X + Rectangle.Width - 30, Rectangle.Y + Rectangle.Height - 20);
                    _btnShowProd.Invalidate();
                }
                if (dgvReport.Controls.Contains(_btnShowFin))
                {
                    Rectangle Rectangle = dgvReport.GetCellDisplayRectangle(9, -1, true);
                    _btnShowFin.Location = new Point(Rectangle.X + Rectangle.Width - 30, Rectangle.Y + Rectangle.Height - 20);
                    _btnShowFin.Invalidate();
                }

                if (dgvReport.Controls.Contains(_txtTess))
                {
                    var headerRect = dgvReport.GetColumnDisplayRectangle(15, true);
                    _txtTess.Location = new Point(headerRect.Location.X + 1, 50 - _txtTess.Height - 2);
                    _txtTess.Size = new Size(headerRect.Width - 3, dgvReport.ColumnHeadersHeight);
                }

                if (dgvReport.Controls.Contains(_cbNote))
                {
                    var headerRect = dgvReport.GetColumnDisplayRectangle(21, true);
                    _cbNote.Location = new Point(headerRect.Location.X + 1, 50 - _cbNote.Height - 2);
                    _cbNote.Size = new Size(headerRect.Width - 3, dgvReport.ColumnHeadersHeight);
                }

                dgvReport.Invalidate(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }      
        }

        private void AddHeaderButtons()
        {
            if (IsUpd) return;

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
        public static DateTime StartDateValue { get; set; }

        private void _cbLineChange_SelectedIndexChanged(object sender, EventArgs eventArgs)
        {
            var cb = (ComboBox)sender;

            var art = dgvReport.CurrentRow.Cells[2].Value.ToString();

            var isParent = CheckOrderHasChildren(art);

            if (Store.Default.sectorId == 8 && dgvReport.CurrentRow.Cells["FlagC"].Value.ToString() != string.Empty && isParent)
            {
                MessageBox.Show("Main field cannot be programmed.");
                return;
            }

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
            if (qtyH < 1.0)
            {
                MessageBox.Show("Capi/ora (" + qtyH.ToString() + ") is not accepted as an unity.", "Programming job", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            var exist = Central.TaskList.Where(x => x.Name == _orderToUpdate && x.Department == dept).ToList();
            if (exist.Count > 0)
            {
                MessageBox.Show("Order already exist as an accepted model.\n" +
                    "Parametarized or cloning anomaly has been occurred.", "Workflow controller",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var byQty = false;

            var d = JobModel.GetLineNextDate(lineInsteadDescripton, dept);

            var programDialog = new ProgramationControl(_orderToUpdate, cb.Text, dept, d,Workflow.TargetProgramDate, article, totalQty, carico, qtyH);

            if (programDialog.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            byQty = programDialog.ByTotalQty;
            d = programDialog.DateTimes;
            var members = programDialog.Members;
            var manualdate = programDialog.UseManualDate;
            var launched = programDialog.Launched;

            if (Store.Default.sectorId == 2)
            {
                qtyH = programDialog.QtyH;
            }
            else if (Store.Default.sectorId == 8)
            {
                qtyH = programDialog.QtyHSartoria;
            }
 
            int qty;
            if (byQty)
            {
                if (totalQty == 0)
                {
                    MessageBox.Show("Qty must be greater zero!", "Programming job", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                qty = totalQty;
            }
            else
            {
                if (carico == 0)
                {
                    MessageBox.Show("Carico must be greater zero!", "Programming job", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                qty = carico;
            }

            var j = new JobModel();

            var jobDuration = j.CalculateJobDuration(lineInsteadDescripton, qty, qtyH, dept, members);
            var dailyQty = j.CalculateDailyQty(lineInsteadDescripton, qtyH, dept, members, qty);
            var price = j.GetPrice(article);
            DateTime startDate;
            DateTime endDate;

            if (d == Config.MinimalDate || d == DateTime.MinValue)
            {
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

            var lineDesc = (from lines in Tables.Lines
                            where lines.Description == cb.Text && lines.Department == dept
                            select lines).SingleOrDefault();

            if (Store.Default.sectorId == 8 && dgvReport.CurrentRow.Cells[38].Value.ToString() != string.Empty 
                || Store.Default.sectorId != 8)
            {
                using (var ctx = new System.Data.Linq.DataContext(Central.ConnStr))
                {
                    ctx.ExecuteCommand("update comenzi set DataProduzione={0},DataFine={1},Line={2}," +
                        "QtyInstead={3},Duration={4},IdStare=1 where NrComanda={5} and Department={6}",
                        startDate, endDate,
                        lineDesc.Line, byQty, jobDuration, _orderToUpdate, dept);
                }
            }

            dgvReport.CurrentCell.Value = cb.Text;
            dgvReport.CurrentRow.Cells[8].Value = UniParseDateTime(startDate);
            dgvReport.CurrentRow.Cells[9].Value = UniParseDateTime(endDate);
            dgvReport.CurrentRow.Cells[0].Value = Properties.Resources.tick_icon_16;

            InsertNewProgram(_orderToUpdate, cb.Text, article, qty, qtyH, startDate, jobDuration, dailyQty, price, dept,members,manualdate, launched);
           
        }

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

        private void tmsiSplitCommessa_Click(object sender, EventArgs e)
        {
            IEnumerable<JobModel> modelQuery;

            if (UseSingleFilter)
            {
                modelQuery = from model in Central.TaskList
                             where model.Name == Workflow.TargetOrder
                             && model.Aim == Workflow.TargetLine
                             select model;
            }
            else
            {
                modelQuery = from model in Central.TaskList
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

        private void DgvReport_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (Store.Default.sectorId != 8) return;

            if (e.RowIndex > -1 && e.ColumnIndex == 38)
            {
                if (dgvReport.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != string.Empty)
                {
                    var rect = new RectangleF(e.CellBounds.X, e.CellBounds.Y, e.CellBounds.Width, e.CellBounds.Height);
                    e.Graphics.FillRectangle(new SolidBrush(e.CellStyle.BackColor), rect);
                    
                    if (dgvReport.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "+")
                    {
                        e.Graphics.DrawImage(Resources.bullet_toggle_plus, rect.X - 6, rect.Y - 6);
                    }
                    else if (dgvReport.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "-")
                    {
                        e.Graphics.DrawImage(Resources.bullet_toggle_minus, rect.X - 6, rect.Y - 6);
                    }

                    e.Graphics.DrawLine(Pens.Silver, e.CellBounds.X, e.CellBounds.Y + e.CellBounds.Height - 1,
                        e.CellBounds.X + e.CellBounds.Width, e.CellBounds.Y + e.CellBounds.Height - 1);

                    e.Handled = true;
                }
            }
        }

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
            var modelQuery = (from model in Central.TaskList
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
            {
                ctx.ExecuteCommand("update comenzi set DataProduzione=null,DataFine=null,Line=null,QtyInstead=null where NrComanda={0}" +
                    " and Department={1}",
                    _orderToUpdate, dept);
            }
            var line = dgvReport.Rows[selIdx].Cells[3].Value.ToString();
            using (var ctx = new System.Data.Linq.DataContext(Central.SpecialConnStr))
            {
                ctx.ExecuteCommand("delete from objects where ordername={0} and aim={1} and department={2}",
                    _orderToUpdate, line, dept);
            }
            Config.InsertOperationLog("manual_programmingremoval", _orderToUpdate + "-" + line + "-" + 
                Store.Default.selDept, "caricolavoro");

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
                newRow[0] = tess; 
                newRow[1] = comm;
                newRow[2] = art;
                newRow[3] = collection;
                newRow[4] = line;
                newRow[5] = arrivo;     
                newRow[6] = tess;
                newRow[7] = conseg;     
                newRow[8] = rdd;    
                newRow[9] = finest;
                newRow[10] = note;
                newRow[11] = qty;  
                newRow[12] = carico;
                newRow[13] = diff;
                newRow[14] = assegnOre;
                newRow[15] = difOre; 
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

        public void InsertNewProgram(string order,string line,string article,int qty,double qtyH, DateTime startDate,double duration, int dailyQty, double price, string dept, int members, bool manualDate, bool launched, int idx = 1) 
        {
            
                var checkShift = new ShiftRecognition();

                var orderOrigin = order.Contains("_") ? order.Split('_')[0] : order;

                var q = "insert into objects (ordername,aim,article,stateid,loadedqty,qtyh,startdate,duration,enddate,dvc,rdd,startprod,endprod,dailyprod,prodqty, " +
                   "overqty,prodoverdays,delayts,prodoverts,locked,holiday,closedord,artprice,hasprod,lockedprod,delaystart,delayend,doneprod,base,department," +
                   "membersnr,manualDate,abatimen,launched, idx, orderorigin) values " +
                   "(@param1,@param2,@param3,@param4,@param5,@param6,@param7,@param8,@param9,@param10," +
                   "@param11,@param12,@param13,@param14,@param15,@param16,@param17,@param18,@param19," +
                   "@param20,@param21,@param22,@param23,@param24,@param25,@param26,@param27,@param28,@param29,@param30,@param31,@param32,@param33,@param34, @param35, @param36)";

                var durationTicks = TimeSpan.FromDays(duration).Ticks;
                var eDate = startDate.AddTicks(durationTicks);
                eDate = new DateTime(eDate.Year, eDate.Date.Month, eDate.Day, eDate.Hour, eDate.Minute, 0, 0);

                eDate = checkShift.GetEndTimeInShift(startDate, eDate);

                var lineDesc = (from lines in Tables.Lines
                                where lines.Description == line && lines.Department == dept
                                select lines).SingleOrDefault();

                var lineDescription = lineDesc != null ? lineDesc.Line : line;
                var lineAbatimen = lineDesc != null ? lineDesc.Abatimento : 0;

                using (var con = new SqlConnection(Central.SpecialConnStr))
                {
                    var cmd = new SqlCommand(q, con);
                    cmd.Parameters.Add("@param1", SqlDbType.NVarChar).Value = order;
                    cmd.Parameters.Add("@param2", SqlDbType.NVarChar).Value = lineDescription;
                    cmd.Parameters.Add("@param3", SqlDbType.NVarChar).Value = article;
                    cmd.Parameters.Add("@param4", SqlDbType.Int).Value = 1;
                    cmd.Parameters.Add("@param5", SqlDbType.Int).Value = qty;
                    cmd.Parameters.Add("@param6", SqlDbType.Float).Value = qtyH;
                    cmd.Parameters.Add("@param7", SqlDbType.DateTime).Value = startDate; //Config.MinimalDate.AddTicks(startDate.Ticks).Ticks;
                    cmd.Parameters.Add("@param8", SqlDbType.Float).Value = duration;
                    cmd.Parameters.Add("@param9", SqlDbType.DateTime).Value = eDate;//Config.MinimalDate.AddTicks(eDate.Ticks).Ticks;
                    cmd.Parameters.Add("@param10", SqlDbType.DateTime).Value = DBNull.Value;
                    cmd.Parameters.Add("@param11", SqlDbType.DateTime).Value = DBNull.Value;
                    cmd.Parameters.Add("@param12", SqlDbType.DateTime).Value = DBNull.Value;
                    cmd.Parameters.Add("@param13", SqlDbType.DateTime).Value = DBNull.Value;
                    cmd.Parameters.Add("@param14", SqlDbType.Int).Value = dailyQty;
                    cmd.Parameters.Add("@param15", SqlDbType.Int).Value = 0;
                    cmd.Parameters.Add("@param16", SqlDbType.Int).Value = 0;
                    cmd.Parameters.Add("@param17", SqlDbType.Int).Value = 0;
                    cmd.Parameters.Add("@param18", SqlDbType.BigInt).Value = 0;
                    cmd.Parameters.Add("@param19", SqlDbType.BigInt).Value = 0;
                    cmd.Parameters.Add("@param20", SqlDbType.Bit).Value = false;
                    cmd.Parameters.Add("@param21", SqlDbType.Int).Value = 0;
                    cmd.Parameters.Add("@param22", SqlDbType.Bit).Value = false;
                    cmd.Parameters.Add("@param23", SqlDbType.Float).Value = price;
                    cmd.Parameters.Add("@param24", SqlDbType.Bit).Value = false;
                    cmd.Parameters.Add("@param25", SqlDbType.Bit).Value = false;
                    cmd.Parameters.Add("@param26", SqlDbType.DateTime).Value = DBNull.Value;
                    cmd.Parameters.Add("@param27", SqlDbType.DateTime).Value = DBNull.Value;
                    cmd.Parameters.Add("@param28", SqlDbType.Bit).Value = false;
                    cmd.Parameters.Add("@param29", SqlDbType.Bit).Value = true;
                    cmd.Parameters.Add("@param30", SqlDbType.NVarChar).Value = dept;
                    cmd.Parameters.Add("@param31", SqlDbType.Int).Value = members;
                    cmd.Parameters.Add("@param32", SqlDbType.Bit).Value = manualDate;
                    cmd.Parameters.Add("@param33", SqlDbType.Int).Value = lineAbatimen;
                    cmd.Parameters.Add("@param34", SqlDbType.Bit).Value = launched;
                    cmd.Parameters.Add("@param35", SqlDbType.Bit).Value = idx;
                    cmd.Parameters.Add("@param36", SqlDbType.NVarChar).Value = orderOrigin;

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }

                Config.InsertOperationLog("manual_programming", order + "-" + line + "-" + dept, "caricolavoro");
            var w = new Workflow();
            w.reset.PerformClick();
        }

        private string GetDescriptionInsteadOfLine(string line, string dept)
        {
            if (string.IsNullOrEmpty(line) || string.IsNullOrEmpty(dept))
            {
                return string.Empty;
            }

            var description = (from lines in Central.ListOfLines
                               where lines.Line == line && lines.Department == dept
                               select lines).SingleOrDefault()?.Description;

            return description;
        }

        private bool CheckOrderHasChildren(string art)
        {
            var q = "select round(cast(6000 / sum(Op.Centes) as float),2), Op.GroupName from OperatiiArticol Op " +
               "inner join Articole Art on Art.Articol='" + art + "' " +
               "where Op.IdArticol = Art.Id and groupName is not null " +
               "group by Op.GroupName";

            var tmpTable = new DataTable();

            using (var con = new SqlConnection(Central.ConnStr))
            {
                var cmd = new SqlCommand(q, con);
                con.Open();
                var dr = cmd.ExecuteReader();
                tmpTable.Load(dr);
                con.Close();
                dr.Close();
            }

            return tmpTable.Rows.Count > 0;
        }
    }
}
