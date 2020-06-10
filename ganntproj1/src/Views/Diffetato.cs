namespace ganntproj1
{
    using System;
    using System.Data;
    using System.Data.Linq;
    using System.Drawing;
    using System.Windows.Forms;

    /// <summary>
    /// Defines the <see cref="Diffetato" />
    /// </summary>
    public partial class Diffetato : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Diffetato"/> class.
        /// </summary>
        public Diffetato()
        {
            InitializeComponent();
            tableView1.DoubleBuffered(true);
            tableView1.DataBindingComplete += tableV_dbc;
            tableView1.EnableHeadersVisualStyles = false;
            tableView1.RowTemplate.Height = 18;
            panel1.Visible = false;
            if (Store.Default.sectorId == 2) panel1.Visible = true;

            cbAbatim.CheckedChanged += (s, events) =>
            {
                LoadReportByDateStiro(CheckByDate);
            };
        }


        /// <summary>
        /// Checks whether is by date selection
        /// </summary>
        private bool CheckByDate { get; set; }

        /// <summary>
        /// The OnLoad
        /// </summary>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        protected override void OnLoad(EventArgs e)
        {
            //LoadReportByDate();
            base.OnLoad(e);
        }

        /// <summary>
        /// The LoadReportByDate
        /// </summary>
        public void LoadReportByDate(bool byDate)
        {
            tableView1.DataSource = null;
            var dt = new DataTable();

            dt.Columns.Add("C/A");
            dt.Columns.Add("Comm.");
            dt.Columns.Add("Articolo");
            dt.Columns.Add("TOT Comm.");
            dt.Columns.Add("Data Arr.");
            dt.Columns.Add("S/A");
            dt.Columns.Add("CARICO");
            dt.Columns.Add("DIFF");
            dt.Columns.Add("Data Consegna");
            dt.Columns.Add("TESS.");
            dt.Columns.Add("CAPI ROTTI");
            dt.Columns.Add("%");
            dt.Columns.Add("RAMM CONF");
            dt.Columns.Add("RAMM TESS");
            dt.Columns.Add("DIFF TESS", typeof(double));
            dt.Columns.Add("% tess", typeof(double));
            dt.Columns.Add("DIFF CONF", typeof(double));
            dt.Columns.Add("% conf", typeof(double));
            dt.Columns.Add("TOTAL", typeof(double));
            dt.Columns.Add("TOT %", typeof(double));

            var totcomm = 0.0;
            var totcar = 0.0;
            var tottess = 0.0;
            var totconf = 0.0;
            var totEffTess = 0.0;
            var totEffConf = 0.0;

            var c1 = 0.0;
            var c2 = 0.0;
            var c3 = 0.0;
            var c4 = 0.0;

            var tblDb = new DataTable();

            string query;
            var from = $"{Central.DateFrom.Year}-{Central.DateFrom.Month}-{Central.DateFrom.Day}";
            var to = $"{Central.DateTo.Year}-{Central.DateTo.Month}-{Central.DateTo.Day}";

            if (byDate)
            {

                query = "select " +
             "comenzi.nrcomanda," +
             "articole.articol," +
             "comenzi.cantitate," +
             "comenzi.dataLansare," +
             "comenzi.carico," +
             "comenzi.dataLivrare," +
             "comenzi.tessitura," +
             "comenzi.ram_conf," +
             "comenzi.ram_tess," +
             "comenzi.def_conf," +
             "comenzi.def_tess, comenzi.diff_t " +
             "from comenzi inner join articole on comenzi.idarticol = articole.id " +
             "where charindex(+',' + Comenzi.department + ',', '" + Store.Default.selDept + "') > 0 and comenzi.dataLivrare between '" + from + "' and '" + to + "'";
            }
            else
            {
                query = "select " +
             "comenzi.nrcomanda," +
             "articole.articol," +
             "comenzi.cantitate," +
             "comenzi.dataLansare," +
             "comenzi.carico," +
             "comenzi.dataLivrare," +
             "comenzi.tessitura," +
             "comenzi.ram_conf," +
             "comenzi.ram_tess," +
             "comenzi.def_conf," +
             "comenzi.def_tess, comenzi.diff_t " +
             "from comenzi inner join articole on comenzi.idarticol = articole.id " +
             "where charindex(+',' + Comenzi.department + ',', '" + Store.Default.selDept + "') > 0";
            }

            using (var conn = new System.Data.SqlClient.SqlConnection(Central.ConnStr))
            {
                var cmd = new System.Data.SqlClient.SqlCommand(query, conn);
                conn.Open();
                var dr = cmd.ExecuteReader();
                tblDb.Load(dr);
                conn.Close();
                conn.Close();
            }

            var totRow = dt.NewRow();
            dt.Rows.Add(totRow);

            var tot = 0.0;
            var totEff = 0.0;
            foreach (DataRow row in tblDb.Rows)
            {
                var newRow = dt.NewRow();

                var arr = row.ItemArray;
                double.TryParse(arr.GetValue(2).ToString(), out var qty);
                double.TryParse(arr.GetValue(4).ToString(), out var carico);
                double.TryParse(arr.GetValue(9).ToString(), out var defConf);
                double.TryParse(arr.GetValue(10).ToString(), out var defTess);
                double.TryParse(arr.GetValue(7).ToString(), out var ramConf);
                double.TryParse(arr.GetValue(8).ToString(), out var ramTess);

                DateTime.TryParse(arr.GetValue(3).ToString(), out var arrivo);
                DateTime.TryParse(arr.GetValue(5).ToString(), out var conseg);

                if (carico == 0) carico = 1;
                totcar += carico;
                totcomm += qty;
                totconf += defConf;
                tottess += defTess;

                newRow[0] = "f";
                newRow[1] = arr.GetValue(0).ToString(); //comm                   
                newRow[2] = arr.GetValue(1).ToString(); //art
                newRow[3] = qty.ToString();
                newRow[4] = arrivo.ToString("dd/MM/yyyy");
                newRow[5] = "";
                newRow[6] = carico.ToString();
                newRow[7] = arr.GetValue(11).ToString();
                newRow[8] = conseg.ToString("dd/MM/yyyy");
                newRow[9] = arr.GetValue(6).ToString(); //tess
                newRow[10] = (ramConf + ramTess).ToString();
                newRow[11] = Math.Round(Convert.ToDouble((ramConf + ramTess) / carico), 3).ToString();
                newRow[12] = ramConf.ToString();
                newRow[13] = ramTess.ToString();
                newRow[14] = defTess.ToString();
                newRow[15] = Math.Round(defTess / carico * 100, 2).ToString();
                newRow[16] = defConf.ToString();
                newRow[17] = Math.Round(defConf / carico * 100, 2).ToString();
                newRow[18] = (defTess + defConf).ToString();
                newRow[19] = Math.Round(((defTess + defConf) / carico) * 100, 2).ToString();

                totEffTess += (double)newRow[15]; //defTess / carico *100;
                totEffConf += (double)newRow[17]; //defConf / carico * 100;
                tot += (double)newRow[18];
                totEff += (double)newRow[19];

                double.TryParse(newRow[10].ToString(), out var capiRoti);
                double.TryParse(newRow[11].ToString(), out var percRot);
                double.TryParse(newRow[12].ToString(), out var ramConfx);
                double.TryParse(newRow[13].ToString(), out var ramTessX);
                c1 += capiRoti;
                c2 += percRot;
                c3 += ramConfx;
                c4 += ramTessX;

                dt.Rows.Add(newRow);
            }

            //totRow[0] = "";
            totRow[3] = totcomm.ToString();
            totRow[6] = totcar.ToString();
            totRow[7] = (totcomm - totcar).ToString();
            totRow[10] = c1.ToString();
            totRow[11] = c2.ToString();
            totRow[12] = c3.ToString();
            totRow[13] = c4.ToString();
            totRow[14] = tottess.ToString();
            totRow[15] = Math.Round(totEffTess, 2).ToString();
            totRow[16] = totconf.ToString();
            totRow[17] = Math.Round(totEffConf, 2).ToString();
            totRow[18] = Math.Round(tot, 2).ToString();
            totRow[19] = Math.Round(totEff, 2).ToString();

            tableView1.DataSource = dt;

            for (var c = 0; c <= tableView1.Columns.Count - 1; c++)
            {
                if (tableView1.Columns[c].Index <= 9)
                {
                    tableView1.Columns[c].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }
                else
                {
                    tableView1.Columns[c].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }                
            }

            CheckByDate = byDate;
        }

        public void LoadReportByDateStiro(bool byDate)
        {
            tableView1.DataSource = null;
            var dt = new DataTable();
         
            string query;
            var from = $"{Central.DateFrom.Year}-{Central.DateFrom.Month}-{Central.DateFrom.Day}";
            var to = $"{Central.DateTo.Year}-{Central.DateTo.Month}-{Central.DateTo.Day}";
            var isChiuso = cbAbatim.Checked;

            string stateCondition;
            if (!isChiuso)
            {
                stateCondition = " and c.idstare=2";
            }
            else
            {
                stateCondition = " and c.idstare<>2";
            }

            if (byDate)
            {
                query = @"  select c.id,c.nrcomanda,a.articol,c.consegnato,c.dataLansare,c.carico,c.datalivrare,cd.tessitura,cd.Confezione,cd.Stiro,
  cd.MateriaPrima,cd.Taglio,cd.stampa,cd.Tintoria,cd.ApplicazioneAccessori
  from comenzi c 
  inner join articole a on c.IdArticol = a.Id
  left join ComenziDiffetato cd on c.Id = cd.NrComandaId
  where c.Department='stiro' and c.datalivrare between @from and @to" + stateCondition;
            }
            else
            {
                query = @"  select c.id,c.nrcomanda,a.articol,c.consegnato,c.dataLansare,c.carico,c.datalivrare,cd.tessitura,cd.Confezione,cd.Stiro,
  cd.MateriaPrima,cd.Taglio,cd.stampa,cd.Tintoria,cd.ApplicazioneAccessori
  from comenzi c 
  inner join articole a on c.IdArticol = a.Id
  left join ComenziDiffetato cd on c.Id = cd.NrComandaId
  where c.Department='stiro'" + stateCondition;
            }

            using (var conn = new System.Data.SqlClient.SqlConnection(Central.ConnStr))
            {
                var cmd = new System.Data.SqlClient.SqlCommand(query, conn);
                if (byDate)
                {
                    cmd.Parameters.Add("@from", SqlDbType.DateTime).Value = from;
                    cmd.Parameters.Add("@to", SqlDbType.DateTime).Value = to;
                }
                conn.Open();
                var dr = cmd.ExecuteReader();
                dt.Load(dr);
                conn.Close();
                conn.Close();
            }


            //tableView1.DataSource = dt;
            var tblView = new DataTable();
            tblView.Columns.Add("Id");
            tblView.Columns.Add("C/A");
            tblView.Columns.Add("Comm.");
            tblView.Columns.Add("Articolo");
            tblView.Columns.Add("Saldo");
            tblView.Columns.Add("Data Arr.");
            tblView.Columns.Add("Carico");
            tblView.Columns.Add("Data Consegna");

            tblView.Columns.Add("Tessitura");
            tblView.Columns.Add("tessitura_p");
            tblView.Columns.Add("Confezione");
            tblView.Columns.Add("Confezione_p");
            tblView.Columns.Add("Stiro");
            tblView.Columns.Add("Stiro_p");
            tblView.Columns.Add("Materia\nPrima");
            tblView.Columns.Add("MateriaPrima_p");
            tblView.Columns.Add("Taglio");
            tblView.Columns.Add("Taglio_p");
            tblView.Columns.Add("Stampa");
            tblView.Columns.Add("stampa_p");
            tblView.Columns.Add("Tintoria");
            tblView.Columns.Add("Tintoria_p");
            tblView.Columns.Add("Applicazione\nAccessori");
            tblView.Columns.Add("Applicazione\nAccessori_p");
            //tblView.Columns.Add("TOT");
            //tblView.Columns.Add("TOT_p");

            var totRow = tblView.NewRow();
            tblView.Rows.Add(totRow);
            var i = 0;

            foreach (DataRow row in dt.Rows)
            {
                i++;
                var newRow = tblView.NewRow();

                var arr = row.ItemArray;

                DateTime.TryParse(arr[4].ToString(), out var dt1);
                DateTime.TryParse(arr[6].ToString(), out var dt2);

                newRow[0] = arr[0].ToString();
                newRow[1] = i.ToString();
                newRow[2] = arr[1].ToString();
                newRow[3] = arr[2].ToString();
                newRow[4] = arr[3].ToString();
                newRow[5] = dt1.ToString("dd/MM/yyyy");
                newRow[6] = arr[5].ToString();
                newRow[7] = dt2.ToString("dd/MM/yyyy");

                double.TryParse(arr[7].ToString(), out var v1);
                double.TryParse(arr[8].ToString(), out var v2);
                double.TryParse(arr[9].ToString(), out var v3);
                double.TryParse(arr[10].ToString(), out var v4);
                double.TryParse(arr[11].ToString(), out var v5);
                double.TryParse(arr[12].ToString(), out var v6);
                double.TryParse(arr[13].ToString(), out var v7);
                double.TryParse(arr[14].ToString(), out var v8);

                double.TryParse(arr[3].ToString(), out var totCom);

                newRow[8] = arr[7].ToString();
                newRow[9] = Math.Round(v1 / totCom, 2).ToString() + "%"; 
                newRow[10] = arr[8].ToString();
                newRow[11] = Math.Round(v2 / totCom, 2).ToString() + "%";
                newRow[12] = arr[9].ToString();
                newRow[13] = Math.Round(v3 / totCom, 2).ToString() + "%";
                newRow[14] = arr[10].ToString();
                newRow[15] = Math.Round(v4 / totCom, 2).ToString() + "%";
                newRow[16] = arr[11].ToString();
                newRow[17] = Math.Round(v5 / totCom, 2).ToString() + "%";
                newRow[18] = arr[12].ToString();
                newRow[19] = Math.Round(v6 / totCom, 2).ToString() + "%";
                newRow[20] = arr[13].ToString();
                newRow[21] = Math.Round(v7 / totCom, 2).ToString() + "%";
                newRow[22] = arr[14].ToString();
                newRow[23] = Math.Round(v8 / totCom, 2).ToString() + "%";

                tblView.Rows.Add(newRow);
            }

            tableView1.DataSource = tblView;

            for (var c = 0; c <= tableView1.Columns.Count - 1; c++)
            {
                if (tableView1.Columns[c].Index <= 9)
                {
                    tableView1.Columns[c].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }
                else
                {
                    tableView1.Columns[c].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }

            var totCons = 0;
            for (var r = 1; r <= tableView1.RowCount - 1; r++)
            {
                int.TryParse(tableView1.Rows[r].Cells[4].Value.ToString(), out var s);
                totCons += s;
            }
            tableView1.Rows[0].Cells[4].Value = totCons.ToString();

            for (var c = 8; c <= tableView1.ColumnCount - 1; c += 2)
            {
                totCons = 0;
                for (var r = 1; r <= tableView1.RowCount - 1; r++)
                {
                    int.TryParse(tableView1.Rows[r].Cells[c].Value.ToString(), out var s);
                    totCons += s;
                }

                tableView1.Rows[0].Cells[c].Value = totCons.ToString();
            }

            for (var c = 9; c <= tableView1.ColumnCount - 1; c += 2)
            {
                double.TryParse(tableView1.Rows[0].Cells[4].Value.ToString(), out var s);
                double.TryParse(tableView1.Rows[0].Cells[c - 1].Value.ToString(), out var x);

                tableView1.Columns[c].HeaderText = Math.Round(x / s * 100, 2).ToString() + "%";
            }

            CheckByDate = byDate;           
        }

        /// <summary>
        /// The ExportToExcel
        /// </summary>
        public void ExportToExcel()
        {
            tableView1.MultiSelect = true;
            tableView1.ExportToExcel("Diffetato");
            tableView1.MultiSelect = false;
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

            var subTit = "";
            if (CheckByDate) subTit = "Da: " + Central.DateFrom.ToString("dd/MM/yyyy") + " - A: " + Central.DateTo.ToString("dd/MM/yyyy") + "\n"+ "Print date: " +
                    DateTime.Now.ToString("dd/MM/yyyy");
            else
            {
                subTit = "Print date: " +
                    DateTime.Now.ToString("dd/MM/yyyy");
            }

            var dGvPrinter = new TableViewPrint
            {
                Title = "Report diffetato",
                SubTitle = subTit, // DateTime.Now.ToString("dd-MM-yyyy"),
                SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip,
                PageNumbers = true,
                PageNumberInHeader = false,
                PorportionalColumns = true,
                HeaderCellAlignment = StringAlignment.Near,
                Footer = "ONLYOU",
                FooterSpacing = 15,
                CellAlignment = StringAlignment.Center,
                ColumnWidth = TableViewPrint.ColumnWidthSetting.DataWidth
            };
            dGvPrinter.PageSettings.Landscape = true;
            dGvPrinter.PrintDataGridView(tableView1);
            Controls.Remove(lbl);
            lbl.Dispose();
        }

        /// <summary>
        /// The tableV_dbc
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="DataGridViewBindingCompleteEventArgs"/></param>
        private void tableV_dbc(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            var tb = (TableView)sender;
            tb.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            if (Store.Default.sectorId != 2)
            {
                tb.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8, FontStyle.Bold);
                tb.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                if (Store.Default.sectorId == 2) return;
                tb.Columns["C/A"].DefaultCellStyle.BackColor = Color.Gainsboro;
                tb.Columns["Comm."].DefaultCellStyle.BackColor = Color.Gainsboro;
                tb.Columns["DIFF"].DefaultCellStyle.BackColor = Color.Gainsboro;
                tb.Columns[0].Width = 30;
                tb.Rows[0].DefaultCellStyle.BackColor = Color.White;
                tb.Rows[0].Height = 28;
                tb.Rows[0].DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8, FontStyle.Bold);
                tb.Rows[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                tb.Rows[0].DefaultCellStyle.ForeColor = Color.Crimson;
                tb.Rows[0].DefaultCellStyle.BackColor = Color.Gainsboro;
                tb.Rows[0].DefaultCellStyle.SelectionBackColor = Color.Gainsboro;
                tb.Rows[0].DefaultCellStyle.SelectionForeColor = Color.Crimson;
                tb.Rows[0].Frozen = true;
                tb.RowTemplate.Height = 18;
            }
            else
            {
                tableView1.Columns[0].Visible = false;
                tableView1.Rows[0].DefaultCellStyle.ForeColor = Color.Crimson;

                for (var c = 9; c <= tableView1.ColumnCount - 1; c += 2)
                {
                    tableView1.Columns[c].HeaderCell.Style.Alignment = DataGridViewContentAlignment.BottomCenter;
                    tableView1.Columns[c].HeaderCell.Style.Font = new Font("Microsoft Sans Serif", 14, FontStyle.Bold);
                }
            }

        }

        private void tableView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int.TryParse(tableView1.Rows[e.RowIndex].Cells[0].Value.ToString(), out var id);
            var f = new DiffetatoInput(id); 
            
            if (f.ShowDialog() == DialogResult.OK)
            {
                LoadReportByDateStiro(CheckByDate);
            }            
        }
    }
}
