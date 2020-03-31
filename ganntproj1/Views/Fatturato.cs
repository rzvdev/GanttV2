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
            dgvReport.DataSource = null;
            var tblRep = new DataTable();
            tblRep.Columns.Add("Data");
            tblRep.Columns.Add("sep_data");
            var con = new SqlConnection(Central.SpecialConnStr);
            var cmd = new SqlCommand("get_data_fatturato", con);//72
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Clear();
            cmd.Parameters.Add("@from_date", SqlDbType.DateTime).Value = Central.DateFrom;
            cmd.Parameters.Add("@to_date", SqlDbType.DateTime).Value = Central.DateTo;
            cmd.Parameters.Add("@deptArr", SqlDbType.NVarChar).Value = Store.Default.arrDept;
            cmd.Parameters.Add("@useAbat", SqlDbType.Bit).Value = cbAcconto.Checked;
            cmd.Parameters.Add("@useHours", SqlDbType.Float).Value = Store.Default.confHour;

            var da = new SqlDataAdapter(cmd);
            var ds = new DataSet();
            da.Fill(ds);
            da.Dispose();
            var tblData = ds.Tables[0];
            var tblLines = ds.Tables[1];
            var tblDays = ds.Tables[2];

            if (tblData.Rows.Count == 0 || tblLines.Rows.Count == 0) return;

            tblRep.Columns.Add(FindT(StrPrev, "t"));
            tblRep.Columns.Add(FindT(StrEff, "t"));
            tblRep.Columns.Add(FindT(StrDeltaValor, "t"));
            tblRep.Columns.Add(FindT(StrPercent, "t"));
            tblRep.Columns.Add(FindT("sep", "t"));

            var sCount = 0;
            foreach (DataRow row in tblLines.Rows)
            {
                sCount++;
                var l = row[0].ToString();
                var dept = Store.Default.sectorId == 1 ? row[1].ToString().Split(' ')[1] : ' ' + row[2].ToString();
                l += dept;
                tblRep.Columns.Add(FindT(StrPrev, l), typeof(double));
                tblRep.Columns.Add(FindT(StrEff, l), typeof(double));
                tblRep.Columns.Add(FindT(StrDeltaValor, l), typeof(double));
                tblRep.Columns.Add(FindT(StrPercent, l), typeof(string));
                //if (sCount == 2)
                //{
                //    tblRep.Columns.Add(FindT("sep", l));
                //    sCount = 0;
                //}
            }

            var totRow = tblRep.NewRow();
            totRow[0] = "total";
            tblRep.Rows.Add(totRow);
            var dateBefore = DateTime.MinValue;
            var totX = 1;
            foreach (DataRow xRow in tblDays.Rows)
            {               
                DateTime.TryParse(xRow[0].ToString(), out var day);               
                DataRow nRow = tblRep.NewRow();
                nRow[0] = day.ToString("dd/MM", System.Globalization.CultureInfo.InvariantCulture);
                              
                foreach (DataRow row in tblData.Rows)
                {
                    var newDate = Convert.ToDateTime(row[0]);
                    if (newDate != day) continue;

                    var line = row[1].ToString();
                    var dept = Store.Default.sectorId == 1 ? row[4].ToString().Split(' ')[1] : ' ' + row[6].ToString();
                    line += dept;
                    double.TryParse(row[2].ToString(), out var price);
                    double.TryParse(row[3].ToString(), out var capi);
                    double.TryParse(row[5].ToString(), out var prev);

                    if (prev == 0) prev = 1;
                    if (capi == 0) capi = 1;
                    prev = Math.Round(prev, 1);
                    capi = Math.Round(capi, 1);

                    var delta = Math.Round(capi - prev, 1);
                    var percentage = Math.Round(capi / prev * 100, 2);

                    if (dateBefore == newDate)
                    {
                        double.TryParse(nRow[FindT(StrPrev, line)].ToString(), out var preventivo);
                        nRow[FindT(StrPrev, line)] = Math.Round(prev + preventivo, 1).ToString();
                        double.TryParse(nRow[FindT(StrEff, line)].ToString(), out var effetivo);
                        nRow[FindT(StrEff, line)] = Math.Round(capi + effetivo, 1).ToString();
                        nRow[FindT(StrDeltaValor, line)] = Math.Round(delta, 1).ToString();
                        nRow[FindT(StrPercent, line)] = Math.Round(percentage, 1).ToString() + StrPercent;
                    }
                    else
                    {
                        nRow[FindT(StrPrev, line)] = prev.ToString();
                        nRow[FindT(StrEff, line)] = capi.ToString();
                        nRow[FindT(StrDeltaValor, line)] = Math.Round(delta, 1).ToString();
                        nRow[FindT(StrPercent, line)] = Math.Round(percentage, 1).ToString() + StrPercent;
                    }
                    dateBefore = newDate;
                }

                tblRep.Rows.Add(nRow);

                if (day.DayOfWeek == DayOfWeek.Friday)
                {
                    DataRow tRow = tblRep.NewRow();
                    tRow[0] = "TOT " + totX.ToString();
                    totX++;
                    tblRep.Rows.Add(tRow);
                }               
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
                var dif = Math.Round(tPrice - tQty, 1).ToString();
                if (tQty == 0) tQty = 1.0;
                if (tPrice == 0) tPrice = 1.0;
                var eff = Math.Round(tPrice / tQty * 100, 1).ToString() + "%";
                var c = dgvReport.ColumnCount;
                row.Cells[2].Value = Math.Round(tQty,1);
                row.Cells[3].Value = Math.Round(tPrice,1);//.ToString();
                row.Cells[4].Value = dif;//.ToString();
                row.Cells[5].Value = eff.ToString();
            }

            var tot1 = 0; var tot2 = 0; var tot3 = 0; var tot4 = 0;

            foreach (DataGridViewRow row in dgvReport.Rows)
            {
                if (row.Cells[0].Value.ToString() == "TOT 1") tot1 = row.Index;
                else if (row.Cells[0].Value.ToString() == "TOT 2") tot2 = row.Index;
                else if (row.Cells[0].Value.ToString() == "TOT 3") tot3 = row.Index;
                else if (row.Cells[0].Value.ToString() == "TOT 4") tot4 = row.Index;
            }

            for (var c = 1; c <= dgvReport.Columns.Count - 1; c++)
            {
                if (dgvReport.Columns[c].Name.Contains(StrDeltaValor) || dgvReport.Columns[c].Name.Contains(StrPercent)) continue;
                var t = 0.0;
                for (var r = 1; r <= tot1 - 1; r++)
                {
                    double.TryParse(dgvReport.Rows[r].Cells[c].Value.ToString(), out var x);
                    t += x;
                }
                dgvReport.Rows[tot1].Cells[c].Value = Math.Round(t, 2).ToString();
            }
            for (var c = 1; c <= dgvReport.Columns.Count - 1; c++)
            {
                if (dgvReport.Columns[c].Name.Contains(StrDeltaValor) || dgvReport.Columns[c].Name.Contains(StrPercent)) continue;
                var t = 0.0;
                for (var r = tot1 + 1; r <= tot2 - 1; r++)
                {
                    double.TryParse(dgvReport.Rows[r].Cells[c].Value.ToString(), out var x);
                    t += x;
                }
                dgvReport.Rows[tot2].Cells[c].Value = Math.Round(t, 2).ToString();
            }
            for (var c = 1; c <= dgvReport.Columns.Count - 1; c++)
            {
                if (dgvReport.Columns[c].Name.Contains(StrDeltaValor) || dgvReport.Columns[c].Name.Contains(StrPercent)) continue;
                var t = 0.0;
                for (var r = tot2 + 1; r <= tot3 - 1; r++)
                {
                    double.TryParse(dgvReport.Rows[r].Cells[c].Value.ToString(), out var x);
                    t += x;
                }
                dgvReport.Rows[tot3].Cells[c].Value = Math.Round(t, 2).ToString();
            }
            for (var c = 1; c <= dgvReport.Columns.Count - 1; c++)
            {
                if (dgvReport.Columns[c].Name.Contains(StrDeltaValor) || dgvReport.Columns[c].Name.Contains(StrPercent)) continue;
                var t = 0.0;
                for (var r = tot3 + 1; r <= tot4 - 1; r++)
                {
                    double.TryParse(dgvReport.Rows[r].Cells[c].Value.ToString(), out var x);
                    t += x;
                }
                dgvReport.Rows[tot4].Cells[c].Value = Math.Round(t, 2).ToString();
            }

            foreach (DataGridViewColumn col in dgvReport.Columns)
            {
                if (col.Name.Split('_')[0] != "Fatturato Preventivo" ||
                 col.HeaderText == string.Empty) continue;
                var tQty = 0.0;
                var tPrice = 0.0;
                foreach (DataGridViewRow row in dgvReport.Rows)
                {
                    if (row.Cells[0].Value.ToString().Contains("TOT")) continue;
                    double.TryParse(row.Cells[col.Index].Value.ToString(), out var val);
                    double.TryParse(row.Cells[col.Index + 1].Value.ToString(), out var pVal);
                    tQty += val;
                    tPrice += pVal;
                }
                var diff = Math.Round(tPrice - tQty, 1);
                if (tQty == 0) tQty = 1;
                var eff = Math.Round(Convert.ToDouble(tPrice / tQty) * 100, 1);
                var c = col.Index;
                dgvReport.Rows[0].Cells[c].Value = Math.Round(tQty, 1).ToString();
                dgvReport.Rows[0].Cells[c + 1].Value = Math.Round(tPrice, 1).ToString();
                dgvReport.Rows[0].Cells[c + 2].Value = Math.Round(diff, 1).ToString();
                dgvReport.Rows[0].Cells[c + 3].Value = Math.Round(eff, 1).ToString() + "%";
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
            var specBrush = new SolidBrush(SystemColors.Control);
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
                    //var s = Store.Default.selDept.Split(' ')[1];

                    txt = ln + " " + n;
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
                    dgvReport.ColumnHeadersDefaultCellStyle.Font,
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
            e.Graphics.FillRectangle(new SolidBrush(dgvReport.ColumnHeadersDefaultCellStyle.BackColor), _rect);
            e.Graphics.DrawString("Data", 
                dgvReport.ColumnHeadersDefaultCellStyle.Font,
                Brushes.White, 
                _rect.Width / 2 - 20,
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
                    //dgvc.HeaderCell.Style.BackColor = Color.LightBlue;
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
                    dgvr.DefaultCellStyle.BackColor = Color.Gainsboro;
                    dgvr.DefaultCellStyle.SelectionBackColor = Color.Gainsboro;
                    dgvr.DefaultCellStyle.ForeColor = Color.Red;
                    dgvr.DefaultCellStyle.SelectionForeColor = Color.Red;
                    dgvr.Frozen = true;
                }
                if (dgvr.Cells[0].Value.ToString().Contains("TOT"))
                {
                    dgvr.DefaultCellStyle.BackColor = Color.Gainsboro;
                    dgvr.DefaultCellStyle.SelectionBackColor = Color.Gainsboro;
                    dgvr.DefaultCellStyle.SelectionForeColor = Color.Black;
                    //foreach (DataGridViewColumn c in dgvReport.Columns)
                    //{
                    //    if (c.HeaderText == string.Empty)
                    //    {
                    //        dgvr.Cells[c.Index].Style.BackColor = Color.White;
                    //        dgvr.Cells[c.Index].Style.SelectionBackColor = Color.White;
                    //    }
                    //}
                }
                //else if (dgvr.Cells[0].Value.ToString() == string.Empty)
                //{
                //    dgvr.DefaultCellStyle.BackColor = Color.Gainsboro;
                //    dgvr.DefaultCellStyle.SelectionBackColor = Color.Gainsboro;
                //    dgvr.Height = 5;
                //}
                //else
                //{
                //    dgvr.Cells[0].Style.BackColor = Color.Gainsboro;
                //}
            }

            dgvReport.Columns[0].Width = 100;
            dgvReport.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvReport.ColumnHeadersHeight = 90;
            dgvReport.Columns[0].DefaultCellStyle.BackColor = Color.Gainsboro;
            dgvReport.Columns[0].DefaultCellStyle.SelectionBackColor = Color.Gainsboro;
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
                //dgvReport.Invalidate();
            }
            else
            {
                dgvReport.Invalidate(_rect);
                dgvReport.Invalidate();
            }

            if (e.Type == ScrollEventType.EndScroll)
            {
                dgvReport.Invalidate();
            }
            //dgvReport.Invalidate(true);
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