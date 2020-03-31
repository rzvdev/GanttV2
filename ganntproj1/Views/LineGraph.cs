namespace ganntproj1
{
    using System;
    using System.Data;
    using System.Drawing;
    using System.Windows.Forms;

    /// <summary>
    /// Defines the <see cref="LineGraph" />
    /// </summary>
    public partial class LineGraph : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LineGraph"/> class.
        /// </summary>
        public LineGraph()
        {
            InitializeComponent();
            tblGraph.EnableHeadersVisualStyles = false;
            tblGraph.DoubleBuffered(true);
            tblGraph.RowTemplate.Height = 28;
        }
        /// <summary>
        /// Defines the _dataTable
        /// </summary>
        private DataTable _dataTable = new DataTable();
        /// <summary>
        /// Gets or sets the Month
        /// </summary>
        private int Month { get; set; }
        /// <summary>
        /// Gets or sets the Year
        /// </summary>
        private int Year { get; set; }
        /// <summary>
        /// The OnLoad
        /// </summary>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        protected override void OnLoad(EventArgs e)
        {
            cbYearAll.Checked = false;
            for (var i = DateTime.Now.Year - 3; i <= DateTime.Now.Year; i++)
            {
                cboYears.Items.Add(i);
            }
            cboMonth.SelectedIndexChanged += (s, ev) =>
            {
                if (cbYearAll.Checked) return;
                Month = cboMonth.SelectedIndex + 1;
                LoadGraph();
            };
            cboYears.SelectedIndexChanged += (s, ev) =>
            {
                Year = Convert.ToInt32(cboYears.Text);    
                LoadGraph();  
            };
            cboYears.SelectedIndex = cboYears.FindString(DateTime.Now.Year.ToString());
            cboMonth.SelectedIndex = DateTime.Now.Month - 1;
            base.OnLoad(e);
        }
        /// <summary>
        /// The GetLineEff
        /// </summary>
        /// <param name="month">The month<see cref="int"/></param>
        /// <param name="year">The year<see cref="int"/></param>
        public void GetLineEff(int month, int year)
        {
            try
            {
                using (var c = new System.Data.SqlClient.SqlConnection(Central.SpecialConnStr))
                {
                    var cmd = new System.Data.SqlClient.SqlCommand();
                    cmd.Connection = c;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "drop_temporary_tables";
                    c.Open();
                    cmd.ExecuteNonQuery();
                    c.Close();
                }

                _dataTable = new DataTable();
                string q;
                if (cbYearAll.Checked)
                {
                    q = "create table tmpTable (datas date, line nvarchar(50), qtyH float,members int, abat float, capi int, dept nvarchar(50)) ";
                    q += "insert into tmpTable ";
                    q += "select convert(date,data,101),line,qtyH,members,(cast(abatim as float)/100), ";
                    q += "capi,department from viewproduction ";
                    q += "where DATEPART(YEAR, data)= '" + year + "' ";
                    q += "and charindex(+ ',' + department + ',', '" + Store.Default.arrDept + "' ) > 0 ";
                    q += "order by department,len(line),line,convert(date, data, 101) ";
                    q += "create table tmpSum (datas date,line nvarchar(50),produc float,prevent float,qty int,dept nvarchar(50),cnt int) ";
                    q += "insert into tmpSum ";
                    q += "select datas,line,sum((qtyH * members))producibili, ";
                    q += "sum((qtyH * members * abat))prevent, sum(capi)qty,dept,count(1) from tmpTable ";
                    q += "group by datas,line,dept order by dept,len(line),line,datas ";
                    q += "select datas,line, (produc / cnt * '" + Store.Default.confHour + "')producibili,(prevent / cnt * '" + Store.Default.confHour + "'),qty,dept  from tmpSum ";
                    q += "drop table tmpTable drop table tmpSum";
                }
                else
                {
                    q = "create table tmpTable (datas date, line nvarchar(50), qtyH float,members int, abat float, capi int, dept nvarchar(50)) ";
                    q += "insert into tmpTable ";
                    q += "select convert(date,data,101),line,qtyH,members,(cast(abatim as float)/100), ";
                    q += "capi,department from viewproduction ";
                    q += "where DATEPART(MONTH, data) = '" + month + "' and DATEPART(YEAR, data)= '" + year + "' ";
                    q += "and charindex(+ ',' + department + ',', '" + Store.Default.arrDept + "' ) > 0 ";
                    q += "order by department,len(line),line,convert(date, data, 101) ";
                    q += "create table tmpSum (datas date,line nvarchar(50),produc float,prevent float,qty int,dept nvarchar(50),cnt int) ";
                    q += "insert into tmpSum ";
                    q += "select datas,line,sum((qtyH * members))producibili, ";
                    q += "sum((qtyH * members * abat))prevent, sum(capi)qty,dept,count(1) from tmpTable ";
                    q += "group by datas,line,dept order by dept,len(line),line,datas ";
                    q += "select datas,line, (produc / cnt * '" + Store.Default.confHour + "')producibili,(prevent / cnt * '" + Store.Default.confHour + "'),qty,dept  from tmpSum ";
                    q += "drop table tmpTable drop table tmpSum";
                }
                using (var c = new System.Data.SqlClient.SqlConnection(Central.SpecialConnStr))
                {
                    var cmd = new System.Data.SqlClient.SqlCommand(q, c);
                    c.Open();
                    var dr = cmd.ExecuteReader();
                    _dataTable.Load(dr);
                    c.Close();
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// Gets or sets the MediaEff
        /// </summary>
        private double MediaEff { get; set; }
        /// <summary>
        /// The LoadGraph
        /// </summary>
        public void LoadGraph()
        {
            tblGraph.DataSource = null;
            GetLineEff(Month, Year);
            MediaEff = 0;
            var dt = new DataTable();
            dt.Columns.Add("Linea");
            dt.Columns.Add("Efficienza", typeof(double));
            dt.Columns.Add("0%");
            dt.Columns.Add("25%");
            dt.Columns.Add("50%");
            dt.Columns.Add("75%");
            dt.Columns.Add("100%");
            
            if (_dataTable.Rows.Count <= 0) return;
            var ln =  Store.Default.sectorId == 1 ? _dataTable.Rows[0][1].ToString() + _dataTable.Rows[0][5].ToString().Split(' ')[1] : _dataTable.Rows[0][1].ToString();
            var totEff = 0.0;
            var count = 0;
            var lineCount = 0;
            var newRow = dt.NewRow();
            foreach (DataRow row in _dataTable.Rows)
            {
                newRow = dt.NewRow();
                var arr = row.ItemArray;

                double.TryParse(arr[4].ToString(), out var prodQty);
                double.TryParse(arr[2].ToString(), out var qtyToProd);
                var lnCheck = Store.Default.sectorId == 1 ? arr[1].ToString() + arr[5].ToString().Split(' ')[1] : arr[1].ToString();

                if (ln == lnCheck) //arr[1].ToString() + arr[5].ToString().Split(' ')[1])
                {
                    totEff += (prodQty / qtyToProd * 100);
                    count++;
                }
                else
                {
                    newRow[0] = ln;
                    //var eff = Math.Round(prodQty / qtyToProd * 100, 2);
                    var eff = Math.Round(totEff / count, 2);
                    if (double.IsNaN(eff) || double.IsInfinity(eff)) eff = 0.0;
                    if (eff > 120.0) eff = 120.0;
                    newRow[1] = eff;                              
                    dt.Rows.Add(newRow);
                    MediaEff += eff;
                    totEff = 0.0;
                    lineCount++;
                    count = 0;
                    totEff += (prodQty / qtyToProd * 100);
                    count++;
                }
                ln = lnCheck; // arr[1].ToString() + arr[5].ToString().Split(' ')[1] ;
            }
            newRow = dt.NewRow();
            newRow[0] = ln;
            var lastEff = Math.Round(totEff / count, 2);
            if (double.IsNaN(lastEff) || double.IsInfinity(lastEff)) lastEff = 0.0;
            if (lastEff > 120.0) lastEff = 120.0;
            newRow[1] = lastEff;
            dt.Rows.Add(newRow);
            MediaEff += lastEff;
            lineCount++;

            dt.DefaultView.Sort = "Efficienza DESC";
            dt = dt.DefaultView.ToTable();
            MediaEff /= lineCount;
            var mediaRow = dt.NewRow();
            mediaRow[0] = "media";
            mediaRow[1] = Math.Round(MediaEff, 1);
            dt.Rows.Add(mediaRow);
            mediaRow = dt.NewRow();
            mediaRow[0] = "target";
            mediaRow[1] = 90.0;
            dt.Rows.Add(mediaRow);
            tblGraph.DataSource = dt;

            foreach (DataGridViewColumn c in tblGraph.Columns)
            {
                if (c.Index > 1 && c.Index <= tblGraph.ColumnCount - 1)
                {
                    c.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
            Invalidate();
        }

        /// <summary>
        /// The TblGraph_CellPainting
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="DataGridViewCellPaintingEventArgs"/></param>
        private void TblGraph_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            var tb = (TableView)sender;
            if (tb.Rows.Count <= 1) return;

            if (e.RowIndex >= 0 && e.ColumnIndex >= 2)
            {
                e.Graphics.FillRectangle(Brushes.White, e.CellBounds);
                var graphWidth = tb.Columns[3].Width * 4;
                var startsFromLeft = 182;
                e.Graphics.DrawLine(Pens.Silver, e.CellBounds.X + e.CellBounds.Width - 1,
                        e.CellBounds.Y, e.CellBounds.X + e.CellBounds.Width - 1, e.CellBounds.Y + e.CellBounds.Height);
                var dotPen = new Pen(Brushes.Silver, 1);
                dotPen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDotDot;
                e.Graphics.DrawLine(dotPen, e.CellBounds.X + e.CellBounds.Width / 2,
                e.CellBounds.Y, e.CellBounds.X + e.CellBounds.Width / 2, e.CellBounds.Y + e.CellBounds.Height);
                if (tb.Rows[e.RowIndex].Cells[0].Value.ToString() != "target" && tb.Rows[e.RowIndex].Cells[0].Value.ToString() != "media")
                {
                    double.TryParse(tb.Rows[e.RowIndex].Cells[1].Value.ToString(), out var eff);
                    var barWidth = Convert.ToInt32(graphWidth * eff / 100);
                    var rect = new Rectangle(startsFromLeft, e.CellBounds.Y + 10, barWidth, e.CellBounds.Height - 20);
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(80, 144, 169)), rect);
                    e.Graphics.DrawRectangle(Pens.DimGray, rect);
                    var effText = e.Graphics.MeasureString(eff.ToString() + "%", new Font("Bahnschrift", 10, FontStyle.Regular));
                    if(eff >= 10)
                    {                       
                        e.Graphics.DrawString(eff.ToString() + "%", new Font("Bahnschrift", 10, FontStyle.Regular),
                       Brushes.WhiteSmoke, rect.X + barWidth - effText.Width - 5, e.CellBounds.Y + rect.Height / 2 - effText.Height / 2 + 11);
                    }                   
                }
                float.TryParse((graphWidth * 90 / 100).ToString(), out var medF);
                var medPen = new Pen(Color.Crimson, 3);
                e.Graphics.DrawLine(medPen, startsFromLeft + medF, e.CellBounds.Y, startsFromLeft + medF, e.CellBounds.Y + e.CellBounds.Height);
                medPen.Dispose();
                dotPen.Dispose();
                e.Handled = true;
            }
            if (e.RowIndex >= 0 && e.ColumnIndex == 1)
            {
                if (e.CellStyle.BackColor == Color.LightGoldenrodYellow) return;
                e.Graphics.FillRectangle(Brushes.WhiteSmoke, e.CellBounds.X, e.CellBounds.Y, e.CellBounds.Width, e.CellBounds.Height);
                var rect = new Rectangle(e.CellBounds.X, e.CellBounds.Y + 10, e.CellBounds.Width, e.CellBounds.Height - 20);
                e.Graphics.FillRectangle(new SolidBrush(e.CellStyle.BackColor), rect);
                e.Graphics.DrawRectangle(Pens.DimGray, rect.X,rect.Y,rect.Width - 1, rect.Height);
                var str = e.Value.ToString();
                var cellFont = e.CellStyle.Font;
                var strW = e.Graphics.MeasureString(str, cellFont);
                e.Graphics.DrawString(str + "%", cellFont, Brushes.Black, e.CellBounds.X + (e.CellBounds.Width / 2 - strW.Width / 2),
                    e.CellBounds.Y + (e.CellBounds.Height / 2 - strW.Height / 2) + 4);
                e.Handled = true;
            }
        }

        /// <summary>
        /// The TblGraph_DataBindingComplete
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="DataGridViewBindingCompleteEventArgs"/></param>
        private void TblGraph_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            tblGraph.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            tblGraph.RowTemplate.Height = 35;

            foreach (DataGridViewColumn c in tblGraph.Columns)
            {
                if (c.Index == 0 || c.Index == 1)
                {
                    c.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    c.HeaderCell.Style.Font = new Font("Bahnschrift", 8, FontStyle.Regular);
                    c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    c.DefaultCellStyle.Font = new Font("Bahnschrift", 8, FontStyle.Regular);
                }
                else if (c.Index > 1)
                {
                    c.HeaderCell.Style.Alignment = DataGridViewContentAlignment.BottomRight;
                    c.HeaderCell.Style.Font = new Font("Bahnschrift", 12, FontStyle.Regular);
                    c.HeaderCell.Style.ForeColor = Color.FromArgb(60, 60, 60);
                    if (c.Index == 6) c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }
            tblGraph.Columns[2].HeaderText = "25%";
            tblGraph.Columns[3].HeaderText = "50%";
            tblGraph.Columns[4].HeaderText = "75%";
            tblGraph.Columns[5].HeaderText = "100%";
            tblGraph.Columns[6].HeaderText = "";

            tblGraph.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;

            for (var i = 0; i <= tblGraph.Rows.Count - 1; i++)
            {
                if (tblGraph.Rows[i].Cells[0].Value.ToString() == "target" || tblGraph.Rows[i].Cells[0].Value.ToString() == "media")
                {
                    tblGraph.Rows[i].DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
                    tblGraph.Rows[i].DefaultCellStyle.ForeColor = Color.FromArgb(50,52,68);
                    continue;
                }

                double.TryParse(tblGraph.Rows[i].Cells[1].Value.ToString(), out var eff);

                if (eff > 93.2) tblGraph.Rows[i].Cells[1].Style.BackColor = Color.FromArgb(138, 184, 44);
                if (eff < 93.2 && eff > 83.7) tblGraph.Rows[i].Cells[1].Style.BackColor = Color.Gold;
                if (eff < 83.7 && eff > 72.2) tblGraph.Rows[i].Cells[1].Style.BackColor = Color.Orange;
                if (eff < 72.2) tblGraph.Rows[i].Cells[1].Style.BackColor = Color.OrangeRed;
            }
            tblGraph.Columns[0].Width = 90;
            tblGraph.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tblGraph.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tblGraph.Columns[0].DefaultCellStyle.BackColor = Color.FromArgb(50, 52, 68);
            tblGraph.Columns[0].DefaultCellStyle.ForeColor = Color.WhiteSmoke;
            tblGraph.Columns[1].Width = 90;
            tblGraph.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tblGraph.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            tblGraph.Columns[0].Frozen = true;
            tblGraph.Columns[1].Frozen = true;
        }

        /// <summary>
        /// The TblGraph_SizeChanged
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void TblGraph_SizeChanged(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// The TblGraph_SelectionChanged
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void TblGraph_SelectionChanged(object sender, EventArgs e)
        {
            ((TableView)sender).ClearSelection();
        }
        private void BtnZoomIn_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in tblGraph.Rows)
            {
                if (row.Height >= 100) continue;
                row.Height += 5;             
            }
            tblGraph.Refresh();
        }
        private void BtnZoomOut_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in tblGraph.Rows)
            {
                if (row.Height <= 35) continue;
                row.Height -= 5;
            }
            tblGraph.Refresh();
        }
        private void CbYearAll_CheckedChanged(object sender, EventArgs e)
        {
            LoadGraph();
            label2.ForeColor = default;
            if (cbYearAll.Checked)
            {
                label2.ForeColor = Color.SeaGreen;
            }
        }

        private void Label2_Click(object sender, EventArgs e)
        {

        }
    }
}