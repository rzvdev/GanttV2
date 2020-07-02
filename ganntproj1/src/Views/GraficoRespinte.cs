using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ganntproj1.Views
{
    public partial class GraficoRespinte : Form
    {
        private System.Data.DataTable _dataTable;
        private System.Data.DataTable _dataTableGraph;
        private System.Data.DataTable _dt;
        private bool _isChiuse;
        private BindingSource _bs = new BindingSource();
        public GraficoRespinte()
        {
            _dataTable = new System.Data.DataTable();
            InitializeComponent();
            tblRespinte.DoubleBuffered(true);

            LoadaDataFromServer();
            CreateSituationContolReport();
            CreateGraphReport();

            cbAbatim.CheckedChanged += (s, e) =>
            {
                LoadaDataFromServer();
                CreateSituationContolReport();
                CreateGraphReport();
            };
        }

        private void AddSituationContolColumns()
        {
            _dataTable = new System.Data.DataTable();
            _dataTable.Columns.Add("Id");
            _dataTable.Columns.Add("Num.");
            _dataTable.Columns.Add("Commessa");
            _dataTable.Columns.Add("Articolo");
            _dataTable.Columns.Add("Linea");
            _dataTable.Columns.Add("Acconto/Saldo");
            _dataTable.Columns.Add("Capi\ncontrollati");
            _dataTable.Columns.Add("DataConsegna", typeof(string));
            _dataTable.Columns.Add("Accetata/Rispinta");
            _dataTable.Columns.Add("DataDiControlo", typeof(string));
            _dataTable.Columns.Add("Motivo");
        }

        public void LoadaDataFromServer()
        {
            if (tblRespinte.DataSource != null) tblRespinte.DataSource = null;
            _isChiuse = cbAbatim.Checked;

            var from = $"{Central.DateFrom.Year}-{Central.DateFrom.Month}-{Central.DateFrom.Day}";
            var to = $"{Central.DateTo.Year}-{Central.DateTo.Month}-{Central.DateTo.Day}";

            string qAddit;
            if (_isChiuse)
            {
                qAddit = " and sc.Stare_IT = 'Chiusa' ";
            }
            else
            {
                qAddit = " and sc.Stare_IT <> 'Chiusa' ";
            }

            var q = "select c.id,nrcomanda,a.Articol ,Line,sc.Stare_IT,Consegnato,Respinte,convert(date,DataLivrare,110),Respinte,convert(date,DateControlled,110),Motivo,c.department " +
                "from comenzi c inner join Articole a on c.IdArticol = a.Id inner join StareCmd sc on c.IdStare = sc.id " +
                "where dataLivrare between '" + from + "' and '" + to + "'" + qAddit + " and  charindex(+',' + c.department + ',', '" + Store.Default.selDept + "') > 0 and line is not null " +
                "order by convert(date,DataLivrare,110) desc";

            _dt = new System.Data.DataTable();
            using (var c = new SqlConnection(Central.ConnStr))
            {
                var cmd = new SqlCommand(q, c);

                c.Open();
                var dr = cmd.ExecuteReader();
                _dt.Load(dr);
                c.Close();
                dr.Close();
                cmd = null;
            }           
        }

        public void CreateSituationContolReport()
        {
            if (_dt.Rows.Count == 0) return;

            AddSituationContolColumns();

            cbCom.Items.Clear();
            cbAr.Items.Clear();
            cbLin.Items.Clear();

            cbCom.Items.Add("<Reset>");
            cbAr.Items.Add("<Reset>");
            cbLin.Items.Add("<Reset>");

            var i = 0;

            _dataTable.Rows.Add();  //row for total res
            _dataTable.Rows.Add();  //row for totals

            var totResQty = 0.0;
            var totQty = 0.0;
            var percentage = 0.0;

            foreach (DataRow row in _dt.Rows)
            {
                i++;

                var newRow = _dataTable.NewRow();

                var id = row[0].ToString();
                var commessa = row[1].ToString();
                var article = row[2].ToString();
                var line = row[3].ToString();
                var state = row[4].ToString();
                var conseg = row[5].ToString();
                var carico = row[6].ToString();     //respinte

                DateTime.TryParse(row[7].ToString(), out var dateConseg);

                string respinte;
                double.TryParse(conseg, out var consegQty);
                double.TryParse(carico, out var qty);

                if (string.IsNullOrEmpty(row[8].ToString()))
                {
                    respinte = "Accetata";
                    totQty += consegQty;
                }
                else
                {
                    respinte = "Respinta";
                    totResQty += qty;
                    totQty += consegQty;
                }

                percentage = Math.Round(totResQty / totQty, 2);

                DateTime.TryParse(row[9].ToString(), out var dateControlled);
                var motivo = row[10].ToString();

                //add department character to the line
                var department = row[11].ToString().Split(' ');
                //if (department.Length > 0) line += " " + department[1];
                if (!cbCom.Items.Contains(commessa)) cbCom.Items.Add(commessa);
                if (!cbAr.Items.Contains(article)) cbAr.Items.Add(article);
                if (!cbLin.Items.Contains(line)) cbLin.Items.Add(line);

                newRow[0] = id;
                newRow[1] = i.ToString();
                newRow[2] = commessa;
                newRow[3] = article;
                newRow[4] = line;
                newRow[5] = state;
                newRow[6] = conseg;
                newRow[7] = dateConseg != DateTime.MinValue ? dateConseg.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture) : string.Empty;
                newRow[8] = respinte;
                newRow[9] = dateControlled != DateTime.MinValue ? dateControlled.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture) : string.Empty;
                newRow[10] = motivo;

                _dataTable.Rows.Add(newRow);
            }

            _dataTable.Rows[0][6] = totResQty.ToString();
            _dataTable.Rows[1][6] = totQty.ToString();
            _dataTable.Rows[1][8] = percentage.ToString() + "%";

            _bs = new BindingSource();
            _bs.DataSource = _dataTable;
            tblRespinte.DataSource = _bs;

            tblRespinte.Columns[0].Visible = false;
            tblRespinte.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            tblRespinte.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tblRespinte.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

            tblRespinte.Columns[1].Width = 35;
            tblRespinte.Columns[9].Width = 100;
            tblRespinte.Columns[10].Width = 400;

            tblRespinte.Columns[9].DefaultCellStyle.BackColor = Color.LightYellow;
            tblRespinte.Columns[10].DefaultCellStyle.BackColor = Color.LightYellow;
            

            for (var c = 0; c <= 4; c++)
            {
                tblRespinte.Columns[c].DefaultCellStyle.BackColor = Color.Gainsboro;
                tblRespinte.Columns[c].Frozen = true;
            }

            foreach (DataGridViewRow row in tblRespinte.Rows)
            {

                if (row.Cells[8].Value.ToString() == "Respinta")
                {
                    row.Cells[8].Style.BackColor = Color.LightCoral;
                    row.Cells[8].Style.ForeColor = Color.Red;
                }
                else if (row.Cells[8].Value.ToString() == "Accetata")
                {
                    row.Cells[8].Style.BackColor = Color.FromArgb(192, 255, 192);
                    row.Cells[8].Style.ForeColor = Color.DarkGreen;

                    if (row.Cells[10].Value.ToString() != string.Empty)
                    {
                        row.Cells[10].Style.BackColor = Color.LightPink;
                        row.Cells[10].Style.ForeColor = Color.Red;
                    }
                }
                if (row.Index <= 1)
                {
                    row.DefaultCellStyle.SelectionBackColor = Color.Gainsboro;
                    row.DefaultCellStyle.BackColor = Color.Gainsboro;
                    row.Frozen = true;

                    if (row.Index == 0)
                    {
                        row.Cells[5].Value = "Res.";
                        row.Cells[6].Style.BackColor = Color.Red;
                        row.Cells[6].Style.SelectionBackColor = Color.Red;
                        row.Cells[6].Style.ForeColor = Color.White;
                        row.Height = 20;
                    }
                    else if (row.Index == 1)
                    {
                        row.Cells[5].Value = "Total";
                        row.Cells[6].Style.BackColor = Color.Red;
                        row.Cells[6].Style.SelectionBackColor = Color.Red;
                        row.Cells[6].Style.ForeColor = Color.White;
                        row.Height = 20;
                    }
                }
            }
        }

        private void AddGraphColumns(System.Data.DataTable dt)
        {
            dt.Columns.Add("Num");
            dt.Columns.Add("Linea");
            dt.Columns.Add("CapiControllati");
            dt.Columns.Add("CapiRespinti");
            dt.Columns.Add("Respinte %",typeof(string));
            dt.Columns.Add("Respinte", typeof(double));
            dt.Columns.Add("Department");
            dt.Columns.Add("0%");
            dt.Columns.Add("25%");
            dt.Columns.Add("50%");
            dt.Columns.Add("75%");
            dt.Columns.Add("100%");
        }

        public void CreateGraphReport()
        {            
            var lbl = new System.Windows.Forms.Label();
            var tblGraph = new TableView();
            var lst = new List<string>();
            var i = 0;

            pnGraphs.Controls.Clear();

            foreach (Control item in pnGraphs.Controls.OfType<System.Windows.Forms.Label>())
            {
                pnGraphs.Controls.Remove(item);
            }
            foreach (Control item in pnGraphs.Controls.OfType<TableView>())
            {
                pnGraphs.Controls.Remove(item);
            }

            if (_dt.Rows.Count <= 0) return;

            _dataTableGraph = new System.Data.DataTable();
            AddGraphColumns(_dataTableGraph);

            foreach (DataRow row in _dt.Rows)
            {
                i++;
                var newRow = _dataTableGraph.NewRow();

                double.TryParse(row[5].ToString(), out var capiControllati);
                double.TryParse(row[6].ToString(), out var respinti);
                    
                var resPercentage = capiControllati != 0 ? Math.Round(respinti / capiControllati * 100.0, 1) : 0;

                newRow[0] = i.ToString();
                newRow[1] = row[3].ToString();
                newRow[2] = capiControllati.ToString();
                newRow[3] = respinti.ToString();
                newRow[4] = resPercentage.ToString() + "%";
                newRow[5] = resPercentage;
                newRow[6] = row[11].ToString();

                _dataTableGraph.Rows.Add(newRow);

                if (lst.Contains(row[11].ToString())) continue;
                lst.Add(row[11].ToString());
            }

            var posX = 2;
            var posY = 5;
            var tblHeight = 0;
            var ir = 0;
            
            foreach (var dept in lst)
            {
                lbl = new System.Windows.Forms.Label()
                {
                    Font = new System.Drawing.Font("Segoe UI", 12, FontStyle.Regular),
                    Location = new System.Drawing.Point(posX, posY + tblHeight),
                    ForeColor = Color.Black,
                    BackColor = SystemColors.Control,
                    AutoSize = false,
                    Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right,
                    Height = 30,
                    Width = tabPage2.Width - 10,
                    TextAlign = ContentAlignment.MiddleLeft,
                };

                lbl.Text = "Respinte " + dept;

                tblGraph = new TableView
                {
                    Location = new System.Drawing.Point(posX, lbl.Height + posY + tblHeight)
                };
                tblGraph.BackgroundColor = Color.White;
                tblGraph.CellPainting += TblGraph_CellPainting;
                tblGraph.DataBindingComplete += TblGraph_BindingComplete;
                tblGraph.Width = tabPage2.Width - 10;
                tblGraph.Name = lbl.Text.Replace(' ', '_');
                tblGraph.Scroll += (s, e) =>
                {
                    if (e.ScrollOrientation == ScrollOrientation.VerticalScroll)
                    {
                        tblGraph.Invalidate();
                    }
                };
                tblGraph.EnableHeadersVisualStyles = false;
                tblGraph.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
                
                var tot1 = 0.0;
                var tot2 = 0.0;
                var deptTable = new System.Data.DataTable();
                AddGraphColumns(deptTable);
                var totRow = deptTable.NewRow();
                deptTable.Rows.Add(totRow);
                foreach (DataRow row in _dataTableGraph.Rows)
                {
                    if (row[6].ToString() != dept) continue;

                    ir++;
                    var newRow = deptTable.NewRow();
                    newRow[0] = ir.ToString();
                    newRow[1] = row[1].ToString();
                    newRow[2] = row[2].ToString();
                    newRow[3] = row[3].ToString();
                    newRow[4] = row[4].ToString();
                    newRow[5] = Convert.ToDouble(row[5]);
                    newRow[6] = row[6].ToString();

                    double.TryParse(row[2].ToString(), out var t1);
                    double.TryParse(row[3].ToString(), out var t2);
                    tot1 += t1;
                    tot2 += t2;

                    deptTable.Rows.Add(newRow);
                }

                deptTable.Rows[0][2] = tot1.ToString();
                deptTable.Rows[0][3] = tot2.ToString();
                deptTable.Rows[0][4] = Math.Round(tot2 / tot1, 2).ToString() + "%";
                
                tblGraph.Height = 300;
                tblGraph.DataSource = deptTable;
                tblGraph.DoubleBuffered(true);

                pnGraphs.Controls.Add(lbl);
                pnGraphs.Controls.Add(tblGraph);
                tblHeight = tblGraph.Height + lbl.Height + 15;
            }
        }

        private void TblGraph_BindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            var tb = (TableView)sender;
            
            if (tb.Columns.Count <= 0) return;
            
            tb.Columns[0].Width = 30;
            tb.ColumnHeadersHeight = 30;

            for (var g = 0; g<= 6; g++)
            {
                if (g > 0) tb.Columns[g].Width = 50; else tb.Columns[g].DefaultCellStyle.BackColor = Color.FromArgb(192, 255, 192);

                tb.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                tb.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft;
                tb.Columns[g].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            for (var g = 7; g <= tb.ColumnCount - 1; g++)
            {
                tb.Columns[g].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                tb.Columns[g].HeaderCell.Style.Alignment = DataGridViewContentAlignment.BottomLeft;
                tb.Columns[g].HeaderCell.Style.Font = new System.Drawing.Font("Bahnschrift", 12, FontStyle.Regular);
                tb.Columns[g].HeaderCell.Style.ForeColor = Color.White;
                tb.Columns[g].HeaderCell.Style.BackColor = Color.FromArgb(125, 141, 161);
            }

            tb.Columns[5].Visible = false;
            tb.Columns[6].Visible = false;

            foreach (DataGridViewRow row in tb.Rows)
            {
                if (row.Index == 0) continue;
               
                row.Height = 24;
                
                if (row.Cells[5].Value.ToString() == "0")
                {
                    row.DefaultCellStyle.BackColor = Color.FromArgb(192, 255, 192);
                    row.Cells[3].Style.ForeColor = Color.DarkGreen;
                    row.Cells[4].Style.ForeColor = Color.DarkGreen;
                }
                else
                {
                    for (var i = 1; i<= 4; i++)
                    {
                        row.Cells[i].Style.BackColor = Color.FromArgb(255, 198, 199);
                    }
                    row.Cells[3].Style.ForeColor = Color.Red;
                    row.Cells[4].Style.ForeColor = Color.Red;
                }
            }

            tb.Rows[0].Frozen = true;
            tb.Rows[0].DefaultCellStyle.BackColor = Color.FromArgb(125, 141, 161);
            tb.Rows[0].DefaultCellStyle.ForeColor = Color.White;
            tb.Rows[0].DefaultCellStyle.SelectionBackColor = Color.FromArgb(125, 141, 161);
            tb.Rows[0].DefaultCellStyle.SelectionForeColor = Color.White;
        }

        private void TblGraph_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            var tb = (TableView)sender;
            if (tb.Rows.Count <= 0) return;

            if (e.RowIndex >= 1 && e.ColumnIndex >= 7)
            {
                e.Graphics.FillRectangle(Brushes.White, e.CellBounds);
                var graphWidth = tb.Columns[8].Width * 4;

                var sfl = 0;
                for (var i = 0; i<= 6; i++)
                {
                    if (tb.Columns[i].Visible)
                    {
                        sfl += tb.Columns[i].Width;
                    }
                }

                var startsFromLeft = sfl + 2;
                e.Graphics.DrawLine(Pens.Silver, e.CellBounds.X + e.CellBounds.Width - 1, e.CellBounds.Y, e.CellBounds.X + e.CellBounds.Width - 1, e.CellBounds.Y + e.CellBounds.Height);
                var dotPen = new Pen(Brushes.Silver, 1);
                dotPen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDotDot;
                e.Graphics.DrawLine(dotPen, e.CellBounds.X + e.CellBounds.Width / 2,
                e.CellBounds.Y, e.CellBounds.X + e.CellBounds.Width / 2, e.CellBounds.Y + e.CellBounds.Height);

                double.TryParse(tb.Rows[e.RowIndex].Cells[5].Value.ToString(), out var eff);
                var barWidth = Convert.ToInt32(graphWidth * eff / 100);
                var rect = new System.Drawing.Rectangle(startsFromLeft, e.CellBounds.Y + 3, barWidth, e.CellBounds.Height - 11);
                var rectShadow = new System.Drawing.Rectangle(startsFromLeft + 1, e.CellBounds.Y + 4, barWidth, e.CellBounds.Height - 10);

                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(60,134,169)), rectShadow);
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(80, 144, 169)), rect);

                var effText = e.Graphics.MeasureString(eff.ToString() + "%", new System.Drawing.Font("Bahnschrift", 10, FontStyle.Regular));

                if (eff >= 10)
                {
                    e.Graphics.DrawString(eff.ToString() + "%", new System.Drawing.Font("Bahnschrift", 10, FontStyle.Regular),
                   Brushes.White, rect.X + barWidth - effText.Width - 5, e.CellBounds.Y + rect.Height / 2 - effText.Height / 2 + 4);
                }

                dotPen.Dispose();
                e.Handled = true;
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (TabPage page in tabControl1.TabPages)
            {
                page.Invalidate();
            }
        }

        private void tblRespinte_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }

        private void tblRespinte_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 2) return;

            int.TryParse(tblRespinte.Rows[e.RowIndex].Cells[0].Value.ToString(), out var id);
            DateTime.TryParse(tblRespinte.Rows[e.RowIndex].Cells[9].Value.ToString(), out var data);
            var motivo = tblRespinte.Rows[e.RowIndex].Cells[10].Value.ToString();
            var frm = new RespinteControlloInput(id,data,motivo);
            frm.StartPosition = FormStartPosition.CenterScreen;
            if (frm.ShowDialog() == DialogResult.OK)
            {
                tblRespinte.Rows[e.RowIndex].Cells[9].Value = frm.Dates.ToString("dd/MM/yyyy");
                tblRespinte.Rows[e.RowIndex].Cells[10].Value = frm.Motivo;
            }
        }

        private void tblRespinte_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {           
        }

        private void cbCom_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbCom.SelectedIndex == 0)
            {
                _bs.Filter = null;
                tblRespinte.DataSource = _bs;
                tblRespinte.Refresh();
                return;
            }

            _bs.Filter = string.Format("CONVERT(" + tblRespinte.Columns[2].DataPropertyName +
                                ", System.String) = '" + cbCom.Text.Replace("'", "''") + "'");

            tblRespinte.DataSource = _bs;
            tblRespinte.Refresh();
        }

        private void cbAr_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbAr.SelectedIndex == 0)
            {
                _bs.Filter = null;
                tblRespinte.DataSource = _bs;
                tblRespinte.Refresh();
                return;
            }

            _bs.Filter = string.Format("CONVERT(" + tblRespinte.Columns[3].DataPropertyName +
                                ", System.String) = '" + cbAr.Text.Replace("'", "''") + "'");

            tblRespinte.DataSource = _bs;
            tblRespinte.Refresh();
        }

        private void cbLin_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbLin.SelectedIndex == 0)
            {
                _bs.Filter = null;
                tblRespinte.DataSource = _bs;
                tblRespinte.Refresh();
                return;
            }

            _bs.Filter = string.Format("CONVERT(" + tblRespinte.Columns[4].DataPropertyName +
                                ", System.String) = '" + cbLin.Text.Replace("'", "''") + "'");

            tblRespinte.DataSource = _bs;
            tblRespinte.Refresh();
        }
    }
}
