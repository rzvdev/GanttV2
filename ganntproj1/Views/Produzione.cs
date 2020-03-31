using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ganntproj1
    {
    public partial class Produzione : Form
    {
        private Config _config = new Config();

        public Produzione()
        {
            InitializeComponent();

            this.DoubleBuffered(true);
            dgvReport.DoubleBuffered(true);
        }

        private void CaricoLav_Load(object sender, EventArgs e)
        {
            _config.Set_sql_conn(new SqlConnection(_config.ReadSqlConnectionString(1)));

            dgvReport.AllowUserToResizeColumns = false;
            dgvReport.AllowUserToResizeRows = false;

            dgvReport.DataBindingComplete += (s, events) =>
            {
                dgvReport.ColumnHeadersHeight = 80;
                dgvReport.RowHeadersVisible = false;

                for (var i = 2; i <= dgvReport.ColumnCount - 1; i++)
                {
                    if (dgvReport.Columns[i].HeaderText.Split('_')[0] == "Comm")
                    {
                        dgvReport.Columns[i].HeaderText = "Comm";
                        dgvReport.Columns[i].DefaultCellStyle.BackColor = Color.Gainsboro;
                        dgvReport.Columns[i].Width = 70;
                    }
                    else if (dgvReport.Columns[i].HeaderText.Split('_')[0] == "Capi")
                    {
                        dgvReport.Columns[i].HeaderText = "Capi";
                        dgvReport.Columns[i].DefaultCellStyle.BackColor = Color.WhiteSmoke;
                        dgvReport.Columns[i].Width = 70;
                    }                
                }

                //total row
                dgvReport.Rows[0].DefaultCellStyle.ForeColor = Color.Green;
                dgvReport.Rows[0].DefaultCellStyle.BackColor = Color.Gainsboro;
                dgvReport.Rows[0].DefaultCellStyle.SelectionBackColor = Color.Gainsboro;
                dgvReport.Rows[0].DefaultCellStyle.SelectionForeColor = Color.Green;
                dgvReport.Rows[0].Cells[0].Style.ForeColor = Color.Black;
                dgvReport.Rows[0].Cells[0].Style.SelectionBackColor = Color.Gainsboro;
                dgvReport.Rows[0].Cells[0].Style.SelectionForeColor = Color.Black;
                dgvReport.Rows[0].Height = 20;
                dgvReport.Rows[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvReport.Rows[0].Frozen = true;

                dgvReport.Rows[1].DefaultCellStyle.ForeColor = Color.Red;
                dgvReport.Rows[1].DefaultCellStyle.BackColor = Color.Gainsboro;
                dgvReport.Rows[1].DefaultCellStyle.SelectionBackColor = Color.Gainsboro;
                dgvReport.Rows[1].DefaultCellStyle.SelectionForeColor = Color.Red;
                dgvReport.Rows[1].Cells[0].Style.ForeColor = Color.Black;
                dgvReport.Rows[1].Cells[0].Style.SelectionBackColor = Color.Gainsboro;
                dgvReport.Rows[1].Cells[0].Style.SelectionForeColor = Color.Black;
                dgvReport.Rows[1].Height = 20;
                dgvReport.Rows[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvReport.Rows[1].Frozen = true;

                dgvReport.Rows[2].DefaultCellStyle.ForeColor = Color.SteelBlue;
                dgvReport.Rows[2].DefaultCellStyle.BackColor = Color.Gainsboro;
                dgvReport.Rows[2].DefaultCellStyle.SelectionBackColor = Color.Gainsboro;
                dgvReport.Rows[2].DefaultCellStyle.SelectionForeColor = Color.SteelBlue;
                dgvReport.Rows[2].Cells[0].Style.ForeColor = Color.Black;
                dgvReport.Rows[2].Cells[0].Style.SelectionBackColor = Color.Gainsboro;
                dgvReport.Rows[2].Cells[0].Style.SelectionForeColor = Color.Black;
                dgvReport.Rows[2].Height = 20;
                dgvReport.Rows[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvReport.Rows[2].Frozen = true;

                //all columns
                for (var i = 0; i <= dgvReport.Columns.Count - 1; i++)
                {
                    //if (i > 0 && i <= _indexFromTotals - 1) dgvReport.Columns[i].Width = 40;

                    dgvReport.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.BottomLeft;
                    dgvReport.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    //dgvReport.AutoResizeColumn(i, DataGridViewAutoSizeColumnMode.AllCells);
                    if (dgvReport.Columns[i].Name.Split('_')[0] == "sep")
                    {
                        //dgvReport.Columns[i].Visible = false;
                        dgvReport.Columns[i].Width = 5;
                        dgvReport.Columns[i].HeaderCell.Style.BackColor = Color.White;
                        dgvReport.Columns[i].HeaderText = "";
                    }
                    dgvReport.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                for (var i = 1; i<= dgvReport.RowCount - 1; i++)
                {
                    if (dgvReport.Rows[i].Cells[0].Value.ToString() == string.Empty)
                    {
                        dgvReport.Rows[i].DefaultCellStyle.BackColor = Color.White;
                    }
                }

                for (var i = 0; i <= 1; i++)
                {
                    dgvReport.Columns[i].Width = 90;
                    dgvReport.Columns[i].DefaultCellStyle.BackColor = Color.Gainsboro;
                    dgvReport.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.BottomLeft;
                    dgvReport.Columns[i].Frozen = true;
                }               
            };

            dgvReport.Paint += dgvReport_Paint;
            dgvReport.CellPainting += dgvReport_CellPainting;

            LoadReportTable();
        }

        #region Report

        private int _columnRange = 0;
        private int _lines_range = 0;
        //private int _indexFromTotals = 0;

        /// <summary>
        /// 
        /// </summary>
        public void LoadReportTable()
        {
            var table_report = new DataTable();

            var strComm = "Comm";
            var strCapi = "Capi";
            //var strTot = "Tot";

            table_report.Columns.Add("Data");
            table_report.Columns.Add("Tot");
            table_report.Columns.Add("sep_t");

            dgvReport.DataSource = null;

            //add dinamycally columns 
            var con = new SqlConnection(Central.SpecialConnStr);
            var cmd = new SqlCommand("get_data_produzione", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Clear();
            cmd.Parameters.Add("@from_date", SqlDbType.DateTime).Value = Central.DateFrom;
            cmd.Parameters.Add("@to_date", SqlDbType.DateTime).Value = Central.DateTo;
            cmd.Parameters.Add("@deptArr", SqlDbType.NVarChar).Value = Store.Default.arrDept;

            var da = new SqlDataAdapter(cmd);
            var ds = new DataSet();
            da.Fill(ds);
            da.Dispose();

            foreach (DataRow row in ds.Tables[1].Rows)  //counted_max
            {
                _columnRange = !string.IsNullOrEmpty(row[0].ToString()) ? _columnRange = Convert.ToInt32(row[0]) : _columnRange = 0;
            }

            if (_columnRange == 0)
            {
                return;
            }

            var lines = ds.Tables[2].Rows;

            _lines_range = lines.Count;
            int index = 0;

            // Add partitioned columns
            var oldLine = "x";
            foreach (DataRow line in lines)
                for (var c = 1; c <= _columnRange; c++)
                {
                    string newLine = line[0].ToString();
                    var dept = Store.Default.sectorId == 1 ? line[1].ToString().Split(' ')[1] : ' ' + line[2].ToString();
                    newLine += dept;
                    if (oldLine != newLine)
                    {
                        index = 1;
                        table_report.Columns.Add(string.Concat(strComm, "_", newLine, "_", index.ToString()));
                        table_report.Columns.Add(string.Concat(strCapi, "_", newLine, "_", index.ToString()));
                    }
                    else
                    {
                        index++;
                        table_report.Columns.Add(string.Concat(strComm, "_", newLine, "_", index.ToString()));
                        table_report.Columns.Add(string.Concat(strCapi, "_", newLine, "_", index.ToString()));
                    }

                    oldLine = newLine;
                }

            DataTable table_data = ds.Tables[0];

            var totalCapiRow = table_report.NewRow();
            var totalQtyRow = table_report.NewRow();
            totalQtyRow[0] = "Total qty";
            table_report.Rows.Add(totalQtyRow);
            totalCapiRow[0] = "Total capi";
            table_report.Rows.Add(totalCapiRow);
            var medRow = table_report.NewRow();
            medRow[0] = "Medie";
            table_report.Rows.Add(medRow);

            //DataRow repRow = table_report.NewRow();
            var partIndex = 0;
            var lineBefore = string.Empty;
            var lst = new List<string>();

            foreach (DataRow row in table_data.Rows)
            {
                var date = Convert.ToDateTime(row[0]);

                if (lst.Contains(date.ToString("dd/MM"))) continue;

                DataRow repRow = table_report.NewRow();
                repRow[0] = date.ToString("dd/MM");
                table_report.Rows.Add(repRow);
                lst.Add(date.ToString("dd/MM"));

                if (date.DayOfWeek == DayOfWeek.Friday)
                {
                    var weekRow = table_report.NewRow();
                    weekRow[0] = string.Empty;
                    weekRow[1] = string.Empty;
                    table_report.Rows.Add(weekRow);
                }
            }

            foreach (DataRow r in table_data.Rows)
            {
                var date = Convert.ToDateTime(r[0]);
                var comm = r[1].ToString();
                var capi = r[2].ToString();
                var line = r[3].ToString();
                var dept = Store.Default.sectorId == 1 ? r[4].ToString().Split(' ')[1] : ' ' + r[5].ToString();
                line += dept;
             
                foreach (DataRow row in table_report.Rows)
                {                  
                    int lineQty = 0;
          
                    if (date.ToString("dd/MM") != row[0].ToString()) continue;
  
                    if (lineBefore == line)
                    {
                        int.TryParse(table_report.Rows[0][strCapi + "_" + line + "_" + "1"].ToString(), out lineQty);
                        int.TryParse(capi, out var q);
                        partIndex++;
                        lineQty += q;
                        row[strComm + "_" + line + "_" + partIndex.ToString()] = comm;
                        row[strCapi + "_" + line + "_" + partIndex.ToString()] = capi.ToString();
                        table_report.Rows[0][strCapi + "_" + line + "_" + "1"] = lineQty.ToString();
                        table_report.Rows[2][strCapi + "_" + line + "_" + "1"] = lineQty.ToString();
                    }
                    else
                    {
                        int.TryParse(capi, out var q);
                        int.TryParse(table_report.Rows[0][strCapi + "_" + line + "_" + "1"].ToString(), out lineQty);
                        lineQty += q;
                        partIndex = 1;
                        row[strComm + "_" + line + "_" + partIndex.ToString()] = comm;
                        row[strCapi + "_" + line + "_" + partIndex.ToString()] = capi.ToString();
                        table_report.Rows[0][strCapi + "_" + line + "_" + "1"] = lineQty.ToString();
                        table_report.Rows[2][strCapi + "_" + line + "_" + "1"] = lineQty.ToString();
                    }
                }

                lineBefore = line;
            }
                
            if (dgvReport.DataSource != null) dgvReport.DataSource = null;
            dgvReport.DataSource = table_report;

            var days = 0;

            foreach (DataGridViewRow row in dgvReport.Rows)
            {
                if (row.Index < 3) continue;
                if (row.Cells[0].Value.ToString() == string.Empty) continue;

                var day = row.Cells[0].Value.ToString().Split('/')[0];
                var month = row.Cells[0].Value.ToString().Split('/')[1];
                var str = month + "/" + day + "/" + Central.DateTo.Year;
                var check = DateTime.ParseExact(str, "MM/dd/yyyy", System.Globalization.CultureInfo.CurrentCulture);

                if (check > Config.MinimalDate)
                {
                    days++;
                }
            }

            //calculate totals_hor
            for (var r = 1; r <= dgvReport.Rows.Count - 1; r++)
            {
                var qty = 0;
                for (var c = 2; c <= dgvReport.Columns.Count - 1; c++)
                {
                    var colName = dgvReport.Columns[c].Name.Split('_')[0];
                    if (colName != strCapi) continue;
                    int.TryParse(dgvReport.Rows[r].Cells[c].Value.ToString(), out var q);
                    qty += q;
                }
                if (dgvReport.Rows[r].Cells[0].Value.ToString() != string.Empty)
                {
                    dgvReport.Rows[r].Cells[1].Value = string.Format("{0:#,##0}", qty);
                }                
            }
            // calculate totals
            var capiTot = 0;
            for (var c = 2; c <= dgvReport.Columns.Count - 1; c++)
            {
                var total = 0;
                if (dgvReport.Columns[c].HeaderText != strCapi) continue;
                for (var r = 3; r <= dgvReport.Rows.Count - 1; r++)
                {
                    if (dgvReport.Rows[r].Cells[c].Value != DBNull.Value)
                    {
                        total += Convert.ToInt32(dgvReport.Rows[r].Cells[c].Value);                      
                    }
                }
                capiTot += total;
                dgvReport.Rows[1].Cells[c].Value = 
                    string.Format("{0:#,##0}", total);
                if (dgvReport.Rows[2].Cells[c].Value.ToString() != string.Empty)
                {
                    int.TryParse(dgvReport.Rows[2].Cells[c].Value.ToString(), out var med);
                    dgvReport.Rows[2].Cells[c].Value = string.Format("{0:#,##0}", med / days);
                }
            }
            dgvReport.Rows[1].Cells[1].Value = string.Format("{0:#,##0}", capiTot);

            dgvReport.Rows[2].Cells[1].Value = string.Format("{0:#,##0}", capiTot / days);

            rowHeight = 0;
            foreach (DataGridViewRow r in dgvReport.Rows)
            {
                rowHeight += r.Height;
            }
            rowHeight += dgvReport.ColumnHeadersHeight;
            rowHeight -= 14;
        }
        private int rowHeight = 0;
        private int Factorial(int n)
        // recursive version
        {
            if (n == 1) return n;
            else return n * Factorial(n - 1);
        }

        #endregion Report

        public void PrintGrid()
        {
            TableViewPrint dGVPrinter = new TableViewPrint();
            dGVPrinter.Title = "Carico lavoro";
            dGVPrinter.SubTitle = DateTime.Now.ToShortDateString();
            dGVPrinter.SubTitleFormatFlags = System.Drawing.StringFormatFlags.LineLimit | System.Drawing.StringFormatFlags.NoClip;
            dGVPrinter.PageNumbers = true;
            dGVPrinter.PageNumberInHeader = false;
            dGVPrinter.PorportionalColumns = true;
            dGVPrinter.HeaderCellAlignment = System.Drawing.StringAlignment.Near;
            dGVPrinter.Footer = "ONLYOU";
            dGVPrinter.FooterSpacing = 15;
            dGVPrinter.PageSettings.Landscape = true;
            dGVPrinter.PrintDataGridView(dgvReport);
        }

        private Font _headerFont = new Font("Microsoft Sans Serif", 10, FontStyle.Regular);

        Rectangle _rect = new Rectangle();
        private void dgvReport_Paint(object sender, PaintEventArgs e)
        {
            var specBrush = new SolidBrush(SystemColors.Control);
            for (int j = 0; j < dgvReport.ColumnCount - 1;)
            {
                string txt;
                int dynamicWdt;

                if (j == 0)
                {
                    txt = "ANALISI CONSEGNE";
                    dynamicWdt = 2;
                }
                else
                {
                    if (dgvReport.Columns[j].Name.Split('_')[0] == "sep")
                    {
                        j++;
                        continue;
                    }
                    else
                    {
                        txt = dgvReport.Columns[j].Name.Split('_')[1];
                        dynamicWdt = _columnRange * 2;
                    }
                }

                _rect = dgvReport.GetCellDisplayRectangle(j, -1, true);
                int w2 = dgvReport.GetCellDisplayRectangle(j, -1, true).Width;

                _rect.X += -1;
                _rect.Y = 1;

                _rect.Width = (w2 * dynamicWdt) - 1;
                _rect.Height = 40;

                e.Graphics.FillRectangle(specBrush, _rect);
              
                var format = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };

                var brsh = Brushes.Black;
                _headerFont = dgvReport.ColumnHeadersDefaultCellStyle.Font;

                if (txt == "ANALISI CONSEGNE")
                {
                    brsh = Brushes.White;
                    e.Graphics.FillRectangle(new SolidBrush(dgvReport.ColumnHeadersDefaultCellStyle.BackColor), _rect);
                }
                else
                {
                    var ln = txt.Substring(0, 5);
                    var numb = txt.Remove(0, 5);
                    txt = ln + " " + numb;
                }

                e.Graphics.DrawString(txt,
                    _headerFont,
                    brsh,
                    _rect,
                    format);

                if (txt == "ANALISI CONSEGNE")
                {
                    j += 2;
                }
                 
                else
                {
                    j += (_columnRange * 2);

                    e.Graphics.DrawLine(Pens.Gray, _rect.X + _rect.Width, _rect.Y, _rect.X + _rect.Width, rowHeight);
                }
                e.Graphics.DrawLine(Pens.Gray, _rect.X + _rect.Width, _rect.Y, _rect.X + _rect.Width, rowHeight);              
            }
            //e.Graphics.DrawLine(Pens.Red, _rect.X, 0, _rect.Width,0);
        }
        private void dgvReport_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.NewValue > e.OldValue)
            {
                dgvReport.Invalidate(_rect);
                dgvReport.Invalidate();                
            }
            else
            {
                dgvReport.Invalidate(_rect);
                dgvReport.Invalidate();              
            }
        }
        private void dgvReport_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
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

            if (e.RowIndex > -1)
            {
                if (dgvReport.Rows[e.RowIndex].Cells[0].Value.ToString() == string.Empty)
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(161, 161, 161)), e.CellBounds.X, e.CellBounds.Y, e.CellBounds.Width, e.CellBounds.Height);
                    e.Graphics.FillRectangle(Brushes.White, e.CellBounds.X, e.CellBounds.Y, e.CellBounds.Width, e.CellBounds.Height - 1);
                    e.Handled = true;
                }
            }
        }

        private void DgvReport_Scroll_1(object sender, ScrollEventArgs e)
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

        public void ExportToExcel()
        {
            dgvReport.MultiSelect = true;
            dgvReport.ExportToExcel("Produzione");
            dgvReport.MultiSelect = false;
        }
    }
}

