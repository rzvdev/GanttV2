namespace ganntproj1
{
    using System;
    using System.Data;
    using System.Drawing;
    using System.Windows.Forms;

    /// <summary>
    /// Defines the <see cref="Respinte" />
    /// </summary>
    public partial class Respinte : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Respinte"/> class.
        /// </summary>
        public Respinte()
        {
            InitializeComponent();
            tableView1.DoubleBuffered(true);
            tableView1.DataBindingComplete += tableV_dbc;
            tableView1.EnableHeadersVisualStyles = false;
            tableView1.RowTemplate.Height = 18;
        }
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
        /// Checks whether is by date option
        /// </summary>
        private bool CheckByDate { get; set; }

        /// <summary>
        /// The LoadReportByDate
        /// </summary>
        public void LoadReportByDate(bool byDate)
        {
            tableView1.DataSource = null;
            var dt = new DataTable();

            dt.Columns.Add("C/A");  //f ?
            dt.Columns.Add("Comm.");
            dt.Columns.Add("Articolo");
            dt.Columns.Add("TOT Comm.");
            dt.Columns.Add("S/A");      // ?
            dt.Columns.Add("CARICO");   //tot!
            dt.Columns.Add("DIFF.");   //tot!
            dt.Columns.Add("Data Consegna");
            dt.Columns.Add("CONFEZIONE");
            dt.Columns.Add("TESSITURA");
            dt.Columns.Add("DATA D.V.C.");
            dt.Columns.Add("Commenti");

            var totcomm = 0;
            var totcar = 0;
            var totDif = 0;

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
              "comenzi.carico," +
              "comenzi.diff_t," +
              "comenzi.dataLivrare," +
              "comenzi.confezione," +
              "comenzi.tessitura, " +
              "comenzi.dvc,comenzi.note " +
              "from comenzi inner join articole on comenzi.idarticol = articole.id " +
              "where charindex(+',' + Comenzi.department + ',', '" + Store.Default.selDept + "') > 0 and comenzi.dataLivrare between '" + from + "' and '" + to + "' " +
              "and comenzi.Respinte='Yes'";
            }
            else
            {
                query = "select " +
              "comenzi.nrcomanda," +
              "articole.articol," +
              "comenzi.cantitate," +
              "comenzi.carico," +
              "comenzi.diff_t," +
              "comenzi.dataLivrare," +
              "comenzi.confezione," +
              "comenzi.tessitura," +
              "comenzi.dvc,comenzi.note " +
              "from comenzi inner join articole on comenzi.idarticol = articole.id " +
              "where charindex(+',' + Comenzi.department + ',', '" + Store.Default.selDept + "') > 0 and comenzi.Respinte='Yes'";
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

            foreach (DataRow row in tblDb.Rows)
            {
                var newRow = dt.NewRow();

                var arr = row.ItemArray;
                int.TryParse(arr.GetValue(2).ToString(), out var qty);
                int.TryParse(arr.GetValue(3).ToString(), out var carico);
                int.TryParse(arr.GetValue(4).ToString(), out var diff);

                DateTime.TryParse(arr.GetValue(5).ToString(), out var conseg);
                DateTime.TryParse(arr.GetValue(8).ToString(), out var dvc);

                if (carico == 0) carico = 1;
                totcar += carico;
                totcomm += qty;
                totDif += diff;

                newRow[0] = "f";
                newRow[1] = arr.GetValue(0).ToString(); //comm                   
                newRow[2] = arr.GetValue(1).ToString(); //art
                newRow[3] = qty.ToString();
                newRow[4] = ""; //.ToString("dd/MM/yyyy");
                newRow[5] = carico.ToString();
                newRow[6] = diff.ToString();
                newRow[7] = conseg.ToString("dd/MM/yyyy");
                newRow[8] = arr.GetValue(6).ToString();
                newRow[9] = arr.GetValue(7).ToString();
                newRow[10] = dvc.ToString("dd/MM/yyyy");
                newRow[11] = arr.GetValue(9).ToString();

                dt.Rows.Add(newRow);
            }

            //totRow[0] = @"TOTALE";
            totRow[3] = totcomm.ToString();
            totRow[5] = totcar.ToString();
            totRow[6] = totDif.ToString();

            tableView1.DataSource = dt;

            foreach (DataGridViewColumn c in tableView1.Columns)
            {
                c.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

                if (c.Name == "Commenti")
                {
                    c.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }

            CheckByDate = byDate;
        }

        /// <summary>
        /// The ExportToExcel
        /// </summary>
        public void ExportToExcel()
        {
            tableView1.MultiSelect = true;
            tableView1.ExportToExcel("Respinte");
            tableView1.MultiSelect = false;
        }

        /// <summary>
        /// The PrintGrid
        /// </summary>
        public void PrintGrid()
        {
            var lbl = new PictureBox
            {
                Image = Properties.Resources.printing_gif,
                SizeMode = PictureBoxSizeMode.CenterImage,
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Font = new Font("Tahoma", 20, FontStyle.Bold)
            };
            Controls.Add(lbl);
            lbl.BringToFront();

            var subTit = "";
            if (CheckByDate) subTit = "Da: " + Central.DateFrom.ToString("dd/MM/yyyy") + " - A: " + Central.DateTo.ToString("dd/MM/yyyy") + "\n" + "Print date: " + 
                    DateTime.Now.ToString("dd/MM/yyyy");
            else
            {
                subTit = "Print date: " +
                    DateTime.Now.ToString("dd/MM/yyyy");
            }
            var dGvPrinter = new TableViewPrint
            {
                Title = "Report commesse respinte",
                SubTitle = subTit,
                SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip,
                PageNumbers = true,
                PageNumberInHeader = false,
                PorportionalColumns = true,
                HeaderCellAlignment = StringAlignment.Near,
                Footer = "ONLYOU",
                FooterSpacing = 15,
                CellAlignment = StringAlignment.Center,
                ColumnWidth = TableViewPrint.ColumnWidthSetting.DataWidth,
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

            tb.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8, FontStyle.Bold);
            tb.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            //tb.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tb.Columns[0].Width = 30;
            tb.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            tb.Columns[0].DefaultCellStyle.BackColor = Color.Gainsboro;
            tb.Columns[1].DefaultCellStyle.BackColor = Color.Gainsboro;
            tb.Columns[6].DefaultCellStyle.BackColor = Color.Gainsboro;

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
    }
}
