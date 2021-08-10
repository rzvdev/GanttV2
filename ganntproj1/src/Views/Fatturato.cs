namespace ganntproj1
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;

    public partial class Fatturato : Form
    {
        private const string StrPrev = "Fatturato Preventivo";

        private const string StrEff = "Fatturato Effetivo";

        private const string StrDeltaValor = "Delta Valoare";

        private const string StrPercent = "%";

        public Fatturato()
        {
            InitializeComponent();
            dgvReport.DoubleBuffered(true);
            dgvReport.DataBindingComplete += dgvReport_DataBindingCom;
        }

        private void Fatturato_Load(object sender, EventArgs e)
        {
            LoadData();
        }

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
            cmd.Parameters.Add("@to_date", SqlDbType.DateTime).Value = Central.DateTo.AddDays(1);
            cmd.Parameters.Add("@deptArr", SqlDbType.NVarChar).Value = Store.Default.arrDept;
            cmd.Parameters.Add("@useAbat", SqlDbType.Bit).Value = cbAcconto.Checked;

            var hour = 0.0;
            var hourW = 0.0;
            switch (Store.Default.sectorId)
            {
                case 1:
                    hour = Store.Default.confHour;
                    hourW = Store.Default.confHourW;
                    break;
                case 2:
                    hour = Store.Default.stiroHour;
                    hourW = Store.Default.stioHourW;
                    break;
                case 7:
                    hour = Store.Default.tessHour;
                    hourW = Store.Default.tessHourW;
                    break;
                case 8:
                    hour = Store.Default.sartHour;
                    hourW = Store.Default.sartHourW;
                    break;
            }

            cmd.Parameters.Add("@useHours", SqlDbType.Float).Value = hour;
            cmd.Parameters.Add("@useHoursW", SqlDbType.Float).Value = hourW;

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
            }

            var totRow = tblRep.NewRow();
            totRow[0] = "TOTAL";
            tblRep.Rows.Add(totRow);
            var dateBefore = DateTime.MinValue;
            var totX = 1;
            tblDays.DefaultView.Sort = "datex ASC";
            DataRow lastrow = null;
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

                if (day.DayOfWeek == DayOfWeek.Saturday)
                {
                    DataRow tRow = tblRep.NewRow();
                    tRow[0] = "TOTAL " + totX.ToString();
                    totX++;
                    tblRep.Rows.Add(tRow);
                }
                else if (lastrow != null)
                {
                    var date1 = Convert.ToDateTime(lastrow[0]);
                    if (date1.DayOfWeek == DayOfWeek.Friday && day.DayOfWeek == DayOfWeek.Monday)
                    {
                        DataRow tRow = tblRep.NewRow();
                        tRow[0] = "TOTAL " + totX.ToString();
                        totX++;
                        tblRep.Rows.InsertAt(tRow, tblRep.Rows.Count - 1);
                    }
                }
                lastrow = xRow;
               
            }
            if (totX>1 && tblRep.Rows.Count>=5 && totX<7)
            {
                DataRow tRowf = tblRep.NewRow();
                tRowf[0] = "TOTAL " + totX.ToString();
                totX++;
                tblRep.Rows.Add(tRowf);
            }
            dgvReport.DataSource = tblRep;
            GetTotals();
        }

        private string FindT(string prefx, string target)
        {
            return string.Format("{0}{1}{2}", prefx, "_", target);
        }

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
                row.Cells[3].Value = Math.Round(tPrice,1);
                row.Cells[4].Value = dif;
                row.Cells[5].Value = eff.ToString();
            }

            var tot1 = 0; var tot2 = 0; var tot3 = 0; var tot4 = 0; var tot5 = 0; var tot6 = 0;

            foreach (DataGridViewRow row in dgvReport.Rows)
            {
                if (row.Cells[0].Value.ToString() == "TOTAL 1") tot1 = row.Index;
                else if (row.Cells[0].Value.ToString() == "TOTAL 2") tot2 = row.Index;
                else if (row.Cells[0].Value.ToString() == "TOTAL 3") tot3 = row.Index;
                else if (row.Cells[0].Value.ToString() == "TOTAL 4") tot4 = row.Index;
                else if (row.Cells[0].Value.ToString() == "TOTAL 5") tot5 = row.Index;
                else if (row.Cells[0].Value.ToString() == "TOTAL 6") tot6 = row.Index;

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
            for (var c = 1; c <= dgvReport.Columns.Count - 1; c++)
            {
                if (dgvReport.Columns[c].Name.Contains(StrDeltaValor) || dgvReport.Columns[c].Name.Contains(StrPercent)) continue;
                var t = 0.0;
                for (var r = tot4 + 1; r <= tot5 - 1; r++)
                {
                    double.TryParse(dgvReport.Rows[r].Cells[c].Value.ToString(), out var x);
                    t += x;
                }
                dgvReport.Rows[tot5].Cells[c].Value = Math.Round(t, 2).ToString();
            }

            for (var c = 1; c <= dgvReport.Columns.Count - 1; c++)
            {
                if (dgvReport.Columns[c].Name.Contains(StrDeltaValor) || dgvReport.Columns[c].Name.Contains(StrPercent)) continue;
                var t = 0.0;
                for (var r = tot5 + 1; r <= tot6 - 1; r++)
                {
                    double.TryParse(dgvReport.Rows[r].Cells[c].Value.ToString(), out var x);
                    t += x;
                }
                dgvReport.Rows[tot6].Cells[c].Value = Math.Round(t, 2).ToString();
            }

            foreach (DataGridViewColumn col in dgvReport.Columns)
            {
                if (col.Name.Split('_')[0] != "Fatturato Preventivo" ||
                 col.HeaderText == string.Empty) continue;
                var tQty = 0.0;
                var tPrice = 0.0;
                foreach (DataGridViewRow row in dgvReport.Rows)
                {
                    if (row.Cells[0].Value.ToString().Contains("TOTAL")) continue;
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

        private Rectangle _rect = new Rectangle();

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

        private void dgvReport_DataBindingCom(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewColumn dgvc in dgvReport.Columns)
            {
                if (dgvc.Name.Split('_')[0] != "sep")
                {
                    dgvc.HeaderText = dgvc.HeaderText.Split('_')[0];
                    dgvc.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvc.Width = 65;
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
                //if (dgvr.Index == 0)
                //{
                //    dgvr.DefaultCellStyle.BackColor = Color.Gainsboro;
                //    dgvr.DefaultCellStyle.SelectionBackColor = Color.Gainsboro;
                //    dgvr.DefaultCellStyle.ForeColor = Color.Red;
                //    dgvr.DefaultCellStyle.SelectionForeColor = Color.Red; 
                //    dgvr.DefaultCellStyle.Font = new Font(Font, FontStyle.Bold);
                //    dgvr.Frozen = true;
                //}
                if (dgvr.Cells[0].Value.ToString().Contains("TOTAL"))
                {
                    dgvr.DefaultCellStyle.BackColor = Color.Gainsboro;
                    dgvr.DefaultCellStyle.SelectionBackColor = Color.Gainsboro;
                    dgvr.DefaultCellStyle.SelectionForeColor = Color.Red;
                    dgvr.DefaultCellStyle.Font = new Font(Font, FontStyle.Bold);
                    dgvr.DefaultCellStyle.ForeColor = Color.Red;
                }
            }

            dgvReport.Columns[0].Width = 100;
            dgvReport.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvReport.ColumnHeadersHeight = 90;
            dgvReport.Columns[0].DefaultCellStyle.BackColor = Color.Gainsboro;
            dgvReport.Columns[0].DefaultCellStyle.SelectionBackColor = Color.Gainsboro;
            dgvReport.Columns[0].Frozen = true;
            dgvReport.Columns[3].DefaultCellStyle.BackColor = Color.Gold;
        }

        internal class LineTotals
        {
            public LineTotals()
            {
            }

            public LineTotals(string line, double q, double p)
            {
                Line = line;
                Qty = q;
                Price = p;
            }

            public string Line { get; set; }

            public double Qty { get; set; }

            public double Price { get; set; }
        }

        private void DgvReport_Scroll(object sender, ScrollEventArgs e)
        {
           
            if (e.NewValue > e.OldValue)
            {                
                dgvReport.Invalidate(_rect);
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
        }

        private double GetPrice(string order, string line)
        {
            var p = 0.0;
            var q = (from art in Central.TaskList
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

        private void CbAcconto_CheckedChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        public void ExportToExcel()
        {
            dgvReport.MultiSelect = true;
            dgvReport.ExportToExcel("Fatturato");
            dgvReport.MultiSelect = false;
        }
    }
}