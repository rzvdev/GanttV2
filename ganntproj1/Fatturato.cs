namespace ganntproj1
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;

    /// <summary>
    /// Defines the <see cref="Fatturato" />
    /// </summary>
    public partial class Fatturato : Form
    {
        /// <summary>
        /// Defines the StrPrev
        /// </summary>
        private const string StrPrev = "Fatturato Preventivo";

        /// <summary>
        /// Defines the StrEff
        /// </summary>
        private const string StrEff = "Fatturato Effetivo";

        /// <summary>
        /// Defines the StrDeltaValor
        /// </summary>
        private const string StrDeltaValor = "Delta Valoare";

        /// <summary>
        /// Defines the StrPercent
        /// </summary>
        private const string StrPercent = "%";

        /// <summary>
        /// Initializes a new instance of the <see cref="Fatturato"/> class.
        /// </summary>
        public Fatturato()
        {
            InitializeComponent();
            dgvReport.DoubleBuffered(true);
            dgvReport.DataBindingComplete += dgvReport_DataBindingCom;
        }

        /// <summary>
        /// The Fatturato_Load
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void Fatturato_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        /// <summary>
        /// The LoadData
        /// </summary>
        public void LoadData()
        {
            var tblRep = new DataTable();
            tblRep.Columns.Add("Data");
            tblRep.Columns.Add("sep_data");
            var con = new SqlConnection(Central.SpecialConnStr);
            var cmd = new SqlCommand("get_data_fatturato", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Clear();
            cmd.Parameters.Add("@from_date", SqlDbType.DateTime).Value = Central.DateFrom;
            cmd.Parameters.Add("@to_date", SqlDbType.DateTime).Value = Central.DateTo;
            var da = new SqlDataAdapter(cmd);
            var ds = new DataSet();
            da.Fill(ds);
            da.Dispose();
            var tblData = ds.Tables[0];
            var tblLines = ds.Tables[1];

            if (tblData.Rows.Count == 0 || tblLines.Rows.Count == 0) return;

            var sCount = 0;
            foreach (DataRow row in tblLines.Rows)
            {
                sCount++;
                var l = row[0].ToString();
                tblRep.Columns.Add(FindT(StrPrev, l), typeof(double));
                tblRep.Columns.Add(FindT(StrEff, l), typeof(double));
                tblRep.Columns.Add(FindT(StrDeltaValor, l), typeof(double));
                tblRep.Columns.Add(FindT(StrPercent, l), typeof(string));
                if (sCount == 2)
                {
                    tblRep.Columns.Add(FindT("sep", l));
                    sCount = 0;
                }
            }
            if (sCount == 1) tblRep.Columns.Add(FindT("sep", "t"));
            tblRep.Columns.Add(FindT(StrPrev, "t"));
            tblRep.Columns.Add(FindT(StrEff, "t"));
            tblRep.Columns.Add(FindT(StrDeltaValor, "t"));
            tblRep.Columns.Add(FindT(StrPercent, "t"));

            var lstOfTotals = new List<LineTotals>();
            var dateBefore = DateTime.MinValue;
            var lastLine = tblData.Rows[0][1].ToString();
            var newRow = tblRep.NewRow();
            var totRow = tblRep.NewRow();
            totRow[0] = "medie";
            tblRep.Rows.Add(totRow);

            foreach (DataRow row in tblData.Rows)
            {
                var newDate = Convert.ToDateTime(row[0]);
                var line = row[1].ToString();
                var order = row[4].ToString();
                var qty = 0.0;

                double.TryParse(row[2].ToString(), out var price);               
                double.TryParse(row[3].ToString(), out var capi);

                qty = GetPreventivo(line, order, cbAcconto.Checked, price,newDate);
                //capi *= GetPrice(order, line);

                if (qty == 0) qty = 1;
                if (capi == 0) capi = 1;
                qty = Math.Round(qty, 1);
                capi = Math.Round(capi, 1);

                var delta = Math.Round(capi - qty, 1);
                var percentage = Math.Round(Convert.ToDouble((capi / qty) * 100), 1).ToString() + StrPercent;
                var matches = lstOfTotals
                    .Where(p => string.Equals
                    (p.Line, line, StringComparison.CurrentCulture)).ToList();

                if (matches.Count > 0)
                {
                    var query = (from t in lstOfTotals
                                 where t.Line == line
                                 select t)
                             .Update(i =>
                             {
                                 i.Qty += qty;
                                 i.Price += capi;
                             });
                }
                else
                    lstOfTotals.Add(
                        new LineTotals(line, qty, capi));

                if (dateBefore == newDate)
                {
                    double.TryParse(newRow[FindT(StrPrev, line)].ToString(), out var preventivo);
                    newRow[FindT(StrPrev, line)] = Math.Round(qty + preventivo, 1).ToString();
                    double.TryParse(newRow[FindT(StrEff, line)].ToString(), out var effetivo);
                    newRow[FindT(StrEff, line)] = Math.Round(capi + effetivo, 1).ToString();
                    newRow[FindT(StrDeltaValor, line)] = Math.Round(delta,1).ToString();
                    newRow[FindT(StrPercent, line)] = percentage;
                }
                else
                {
                    //tblRep.Rows.Add(newRow);
                    newRow = tblRep.NewRow();
                    newRow[0] = newDate.ToString("dd/MM");
                    newRow[FindT(StrPrev, line)] = qty.ToString();
                    newRow[FindT(StrEff, line)] = capi.ToString();
                    newRow[FindT(StrDeltaValor, line)] = Math.Round(delta, 1).ToString();
                    newRow[FindT(StrPercent, line)] = percentage;
                    tblRep.Rows.Add(newRow);

                    var nextWeekday = Globals.GetNextWeekday(newDate, DayOfWeek.Friday);

                    if (newDate >= nextWeekday)
                    {
                        totRow = tblRep.NewRow();
                        totRow[0] = "Total";
                        foreach (var item in lstOfTotals)
                        {
                            totRow[FindT(StrPrev, item.Line)] = Math.Round(item.Qty,1);
                            totRow[FindT(StrEff, item.Line)] = Math.Round(item.Price,1);
                        }
                        tblRep.Rows.Add(totRow);
                        var emptyRow = tblRep.NewRow();
                        emptyRow[0] = "";
                        tblRep.Rows.Add(emptyRow);
                        qty = 0.0;
                        capi = 0.0;
                        lstOfTotals.Clear();
                    }
                }
                dateBefore = newDate;
            }
            foreach (var item in lstOfTotals)
            {
                double.TryParse(totRow[FindT(StrPrev, item.Line)].ToString(),
                    out var curQty);
                double.TryParse(totRow[FindT(StrEff, item.Line)].ToString(),
                    out var curPrice);
                totRow[FindT(StrPrev, item.Line)] = Math.Round((curQty + item.Qty),1).ToString();
                totRow[FindT(StrEff, item.Line)] =
                    Math.Round((curPrice + item.Price),1).ToString();
            }
            dgvReport.DataSource = tblRep;
            GetTotals();
        }

        /// <summary>
        /// The FindT
        /// </summary>
        /// <param name="prefx">The prefx<see cref="string"/></param>
        /// <param name="target">The target<see cref="string"/></param>
        /// <returns>The <see cref="string"/></returns>
        private string FindT(string prefx, string target)
        {
            return string.Format("{0}{1}{2}", prefx, "_", target);
        }

        /// <summary>
        /// The GetTotals
        /// </summary>
        private void GetTotals()
        {
            foreach (DataGridViewRow row in dgvReport.Rows)
            {
                var tQty = 0.0;
                var tPrice = 0.0;
                if (string.IsNullOrEmpty(row.Cells[0].Value.ToString())) continue;
                foreach (DataGridViewColumn col in dgvReport.Columns)
                {
                    if (col.Name.Split('_')[0] != "Fatturato Preventivo" ||
                    col.HeaderText == string.Empty) continue;
                    double.TryParse(row.Cells[col.Index].Value.ToString(), out var val);
                    double.TryParse(row.Cells[col.Index + 1].Value.ToString(), out var pVal);
                    tQty += val;
                    tPrice += pVal;
                }
                var dif = (tPrice - tQty).ToString();
                if (tQty == 0) tQty = 1.0;
                if (tPrice == 0) tPrice = 1.0;
                var eff = Math.Round(tPrice / tQty * 100, 1).ToString() + "%";
                var c = dgvReport.ColumnCount;
                row.Cells[c - 4].Value = tQty;
                row.Cells[c - 3].Value = tPrice;//.ToString();
                row.Cells[c - 2].Value = dif;//.ToString();
                row.Cells[c - 1].Value = eff.ToString();
            }

            foreach (DataGridViewColumn col in dgvReport.Columns)
            {
                if (col.Name.Split('_')[0] != "Fatturato Preventivo" ||
                    col.HeaderText == string.Empty) continue;
                var tQty = 0.0;
                var tPrice = 0.0;
                foreach (DataGridViewRow row in dgvReport.Rows)
                {
                    if (row.Cells[0].Value.ToString() == "Total")
                    {
                        double.TryParse(row.Cells[col.Index].Value.ToString(), out var val);
                        double.TryParse(row.Cells[col.Index + 1].Value.ToString(), out var pVal);
                        tQty += val;
                        tPrice += pVal;
                    }
                }
                var diff = Math.Round(tPrice - tQty, 1);
                if (tQty == 0) tQty = 1;
                var eff = Math.Round(Convert.ToDouble(tPrice / tQty) * 100, 1);
                var c = col.Index;
                dgvReport.Rows[0].Cells[c].Value = Math.Round(tQty, 1);//.ToString();
                dgvReport.Rows[0].Cells[c + 1].Value = Math.Round(tPrice, 1);//.ToString();
                dgvReport.Rows[0].Cells[c + 2].Value = diff;//.ToString();
                dgvReport.Rows[0].Cells[c + 3].Value = eff.ToString() + "%";
            }
        }

        /// <summary>
        /// Defines the _rect
        /// </summary>
        private Rectangle _rect = new Rectangle();

        /// <summary>
        /// The DgvReport_Paint
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="PaintEventArgs"/></param>
        private void DgvReport_Paint(object sender, PaintEventArgs e)
        {
            var specBrush = Brushes.Gold;
            for (int j = 1; j < dgvReport.ColumnCount - 1;)
            {               
                if (dgvReport.Columns[j].HeaderText == string.Empty)
                {
                    j += 1;
                    continue;
                }
                string txt = dgvReport.Columns[j].Name.Split('_')[1];               
                if (txt == "t") txt = "TOTALE";
                else
                {
                    var ln = txt.Substring(0, 5);
                    var n = txt.Remove(0, 5);
                    var s = Store.Default.selDept.Split(' ')[1];

                    txt = ln + " " + n + s;
                }
                _rect = dgvReport.GetCellDisplayRectangle(j, -1, true);
                int w2 = dgvReport.GetCellDisplayRectangle(j, -1, true).Width;
                _rect.X += -1;
                _rect.Y = 0;
                _rect.Width = w2 * 4;
                _rect.Height = 45;
                e.Graphics.FillRectangle(specBrush, _rect);
                e.Graphics.DrawRectangle(Pens.White, _rect);
                StringFormat format = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };

                e.Graphics.DrawString(txt,
                    new Font("Microsoft Sans Serif", 10, FontStyle.Regular),
                    Brushes.Black,
                    _rect,
                    format);
             
                j += 4;
            }

            //fill rectangle in frozen cell to avoid graphical bugs
            _rect = dgvReport.GetCellDisplayRectangle(0, -1, true);
            int wData = dgvReport.GetCellDisplayRectangle(0, -1, true).Width;
            _rect.X += -1;
            _rect.Y = 0;
            _rect.Width = dgvReport.Columns[0].Width + 1;
            _rect.Height = 90;
            e.Graphics.FillRectangle(specBrush, _rect);
            e.Graphics.DrawString("DATA", 
                new Font("Microsoft Sans Serif", 10, FontStyle.Regular), 
                Brushes.Black, 
                _rect.X + 2,
                _rect.Height / 2 - 10);
        }

        /// <summary>
        /// The DgvReport_CellPainting
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="DataGridViewCellPaintingEventArgs"/></param>
        private void DgvReport_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex > 0 && e.ColumnIndex < dgvReport.ColumnCount - 1)
            {
                _rect = e.CellBounds;
                _rect.Y += e.CellBounds.Height / 2;
                _rect.Height = e.CellBounds.Height / 2;
                e.PaintBackground(_rect, true);
                e.PaintContent(_rect);
                e.Handled = true;
            }
        }

        /// <summary>
        /// The dgvReport_DataBindingCom
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="DataGridViewBindingCompleteEventArgs"/></param>
        private void dgvReport_DataBindingCom(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewColumn dgvc in dgvReport.Columns)
            {
                if (dgvc.Name.Split('_')[0] != "sep")
                {
                    dgvc.HeaderText = dgvc.HeaderText.Split('_')[0];
                    dgvc.HeaderCell.Style.BackColor = Color.LightBlue;
                    dgvc.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvc.Width = 60;
                    dgvc.HeaderCell.Style.Alignment = DataGridViewContentAlignment.BottomLeft;
                    if (dgvc.Name.Split('_')[0] == StrEff)
                    {
                        dgvc.DefaultCellStyle.BackColor = Color.Gainsboro;
                    }
                }
                else
                {
                    dgvc.HeaderText = "";
                    dgvc.DefaultCellStyle.BackColor = Color.White;
                    dgvc.DefaultCellStyle.SelectionBackColor = Color.White;
                    dgvc.HeaderCell.Style.BackColor = Color.White;
                    dgvc.Width = 5;
                }
            }
            foreach (DataGridViewRow dgvr in dgvReport.Rows)
            {
                if (dgvr.Index == 0)
                {
                    dgvr.DefaultCellStyle.BackColor = Color.White;
                    dgvr.DefaultCellStyle.SelectionBackColor = Color.White;
                    dgvr.DefaultCellStyle.ForeColor = Color.Red;
                    dgvr.DefaultCellStyle.SelectionForeColor = Color.Red;
                    dgvr.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8, FontStyle.Bold);
                    dgvr.Cells[0].Style.ForeColor = Color.Black;
                    dgvr.Cells[0].Style.Font = new Font("Micorost Sans Serif", 8, FontStyle.Regular);
                    dgvr.Frozen = true;
                }
                if (dgvr.Cells[0].Value.ToString() == "Total")
                {
                    dgvr.DefaultCellStyle.BackColor = Color.Gainsboro;
                    dgvr.DefaultCellStyle.SelectionBackColor = Color.Gainsboro;
                    dgvr.DefaultCellStyle.SelectionForeColor = Color.Black;
                    foreach (DataGridViewColumn c in dgvReport.Columns)
                    {
                        if (c.HeaderText == string.Empty)
                        {
                            dgvr.Cells[c.Index].Style.BackColor = Color.White;
                            dgvr.Cells[c.Index].Style.SelectionBackColor = Color.White;
                        }
                    }
                }
                else if (dgvr.Cells[0].Value.ToString() == string.Empty)
                {
                    dgvr.DefaultCellStyle.BackColor = Color.White;
                    dgvr.DefaultCellStyle.SelectionBackColor = Color.White;
                    dgvr.Height = 5;
                }
                else
                {
                    if (dgvr.Cells[0].Value.ToString() == "medie")
                    {
                        dgvr.Cells[0].Style.BackColor = Color.White;
                    }
                    else
                    {
                        dgvr.Cells[0].Style.BackColor = Color.Gold;
                    }
                }
            }

            dgvReport.Columns[0].Width = 50;
            dgvReport.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvReport.Columns[0].HeaderCell.Style.BackColor = Color.Gold;
            dgvReport.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dgvReport.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvReport.GridColor = Color.White;
            dgvReport.ColumnHeadersHeight = 90;
            dgvReport.Columns[0].Frozen = true;
        }

        /// <summary>
        /// Defines the <see cref="LineTotals" />
        /// </summary>
        internal class LineTotals
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="LineTotals"/> class.
            /// </summary>
            public LineTotals()
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="LineTotals"/> class.
            /// </summary>
            /// <param name="line">The line<see cref="string"/></param>
            /// <param name="q">The q<see cref="double"/></param>
            /// <param name="p">The p<see cref="double"/></param>
            public LineTotals(string line, double q, double p)
            {
                Line = line;
                Qty = q;
                Price = p;
            }

            /// <summary>
            /// Gets or sets the Line
            /// </summary>
            public string Line { get; set; }

            /// <summary>
            /// Gets or sets the Qty
            /// </summary>
            public double Qty { get; set; }

            /// <summary>
            /// Gets or sets the Price
            /// </summary>
            public double Price { get; set; }
        }

        /// <summary>
        /// The DgvReport_Scroll
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="ScrollEventArgs"/></param>
        private void DgvReport_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.NewValue > e.OldValue)
            {
                dgvReport.Invalidate(_rect);
            }
            else
            {
                dgvReport.Invalidate();
                dgvReport.Invalidate(_rect);
            }
        }

        /// <summary>
        /// The GetPrice
        /// </summary>
        /// <param name="order">The order<see cref="string"/></param>
        /// <param name="line">The line<see cref="string"/></param>
        /// <returns>The <see cref="double"/></returns>
        private double GetPrice(string order, string line)
        {
            var p = 0.0;
            var q = (from art in Central.ListOfModels
                     where art.Name == order && art.Aim == line
                     select art).ToList();

            if (q.Count == 0)
            {
                return 1;
            }
            else
            {
                foreach (var item in q)
                {
                    double.TryParse(item.ArtPrice.ToString(), out p);
                }
            }

            return p;
        }

        /// <summary>
        /// The GetPreventivo
        /// </summary>
        /// <param name="line">The line<see cref="string"/></param>
        /// <param name="order">The order<see cref="string"/></param>
        /// <param name="byHund">The byHund<see cref="bool"/></param>
        /// <returns>The <see cref="double"/></returns>
        private double GetPreventivo(string line, string order, bool byHund, double price, DateTime fromDate)
        {
            var retVal = 0.0;
            var persons = 0;
            var abatim = 0.0;
            var qtyH = 0.0;
            //var price = GetPrice(order, line);

            var p = (from lin in ObjectModels.Tables.Lines
                     where lin.Line == line
                     select lin).ToList();

            foreach (var item in p)
            {
                persons = item.Members;
                double.TryParse(item.Abatimento.ToString(), out abatim);
                abatim /= 100;
            }

            var models = (from mod in Central.ListOfModels
                          where mod.Aim == line && mod.Name == order
                          select new { mod.QtyH }).ToList();

            foreach (var item in models)
            {
                double.TryParse(item.QtyH.ToString(), out qtyH);
            }
            var prodQ = from prod in ObjectModels.Tables.Productions
                        where prod.Commessa == order && prod.Line == line &&
                        Convert.ToDateTime(prod.Times).Date == fromDate.Date
                        select prod;

            var timers = new DateTime();
            foreach (var item in prodQ)
            {
                DateTime.TryParse(item.Times.ToString(), out timers);
            }

            var hX = 8.0;
            if ((timers.Hour - fromDate.Hour) < 8)
            {
                hX -= (timers.Hour - fromDate.Hour);
            }

            if (byHund)
                retVal = Math.Round(
                    persons * qtyH * 7.5 * price, 0);
            else
                retVal = Math.Round(
                    persons * qtyH * 7.5 * price * abatim, 0);

            if (hX < 8)
            {
                retVal = (retVal / 8.0 * hX);
            }

            return Math.Round(retVal,0);
        }

        /// <summary>
        /// The CbAcconto_CheckedChanged
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void CbAcconto_CheckedChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        /// <summary>
        /// The ExportToExcel
        /// </summary>
        public void ExportToExcel()
        {
            dgvReport.MultiSelect = true;
            dgvReport.ExportToExcel("Fatturato");
            dgvReport.MultiSelect = false;
        }
    }
}
