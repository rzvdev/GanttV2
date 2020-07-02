using Microsoft.Office.Interop.Excel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ganntproj1.Views
{
    public partial class CommessaDefect : Form
    {
        private int Month { get; set; }
        private int Year { get; set; }


        private BindingSource _bs = new BindingSource();

        private System.Data.DataTable _dataTable = new System.Data.DataTable();
        public CommessaDefect()
        {
            InitializeComponent();
            
            this.DoubleBuffered(true);
            tableView1.DoubleBuffered(true);
            tableView1.DataBindingComplete += TableView1_DataBindingComp;
            this.tableView1.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvReport_CellPainting);
            this.tableView1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.DgvReport_Scroll);
            this.tableView1.Paint += new System.Windows.Forms.PaintEventHandler(this.dgvReport_Paint);

            cbAbatim.CheckedChanged += (s, events) =>
            {
                LoadData();
            };

            cboMonth.SelectedIndexChanged += (s, ev) =>
            {
                Month = cboMonth.SelectedIndex + 1;
                LoadData();
            };
            for (var i = DateTime.Now.Year - 3; i <= DateTime.Now.Year; i++)
            {
                cboYears.Items.Add(i);
            }
            cboYears.SelectedIndexChanged += (s, ev) =>
            {
                Year = Convert.ToInt32(cboYears.Text);
                LoadData();
            };
            cboYears.SelectedIndex = cboYears.FindString(DateTime.Now.Year.ToString());
            cboMonth.SelectedIndex = DateTime.Now.Month - 1;
        }

        private void LoadData()
        {
            _dataTable = new System.Data.DataTable();
             
            _dataTable.Columns.Add("Num.");
            _dataTable.Columns.Add("Commessa");
            _dataTable.Columns.Add("Articolo");
            _dataTable.Columns.Add("TOTAL commessa");

            _dataTable.Columns.Add("Saldo tessitura_tessitura"); //4
            _dataTable.Columns.Add("Differenza_tessitura");
            _dataTable.Columns.Add("% Mancanti_tessitura");
            _dataTable.Columns.Add("TESS_tessitura"); //7

            _dataTable.Columns.Add("sep_1");

            _dataTable.Columns.Add("Saldo_Confezione A"); //9
            _dataTable.Columns.Add("Capi_Confezione A");
            _dataTable.Columns.Add("TCapi_Confezione A"); //11

            _dataTable.Columns.Add("sep_2");

            _dataTable.Columns.Add("Saldo_Confezione B"); //13
            _dataTable.Columns.Add("Capi_Confezione B");
            _dataTable.Columns.Add("TCapi_Confezione B"); //15

            _dataTable.Columns.Add("sep_3");

            _dataTable.Columns.Add("Saldo_Stiro"); //17
            _dataTable.Columns.Add("Capi_Stiro"); 
            _dataTable.Columns.Add("TCapi_Stiro");
            _dataTable.Columns.Add("CONF"); //20

            _dataTable.Columns.Add("sep_4");
            
            _dataTable.Columns.Add("Tot_capi_mancanti_RIEPIOLOGO");
            _dataTable.Columns.Add("tot_capi_mancanti_perc_RIEPIOLOGO");
            _dataTable.Columns.Add("Tot_Capi_Stiro_RIEPIOLOGO");

            //_dataTable.Columns.Add("sep_5");

            _dataTable.Columns.Add("Tot_Diff_RIEPIOLOGO");
            _dataTable.Columns.Add("Tot_Dif_Perc_RIEPIOLOGO");

            var dt = new System.Data.DataTable();

            var isChiuso = cbAbatim.Checked;

            string stateCondition;
            if (!isChiuso)
            {
                stateCondition = "where comenzi.idstare=2 and comenzi.idsector is not null and DATEPART(MONTH, comenzi.DataLivrare)= '" + Month + "' and DATEPART(YEAR, comenzi.DataLivrare)= '" + Year + "' order by comenzi.DataLivrare desc";
            }
            else
            {
                stateCondition = "where comenzi.idstare<>2 and comenzi.idsector is not null and DATEPART(MONTH, comenzi.DataLivrare)= '" + Month + "' and DATEPART(YEAR, comenzi.DataLivrare)= '" + Year + "' order by comenzi.DataLivrare desc";
            }

            var q = @"select comenzi.nrcomanda,articole.articol,comenzi.cantitate,comenzi.consegnato,comenzi.Tessitura,comenzi.Confezione,comenzi.Department,comenzi.DataLivrare,comenzi.idstare from comenzi 
inner join articole on comenzi.idarticol = articole.id " + stateCondition;

            using (var con = new SqlConnection(Central.ConnStr))
            {
                var cmd = new SqlCommand(q, con);
                con.Open();
                var dr = cmd.ExecuteReader();
                dt.Load(dr);
                con.Close();
                dr.Close();
                cmd = null;
            }

            Hashtable htbl = new Hashtable();
            var idx = 0;

            _dataTable.Rows.Add();


            cbCom.Items.Clear();
            cbAr.Items.Clear();
            cbCom.Items.Add("<Reset>");
            cbAr.Items.Add("<Reset>");

            foreach (DataRow row in dt.Rows)
            {
                var key = "";

                var commessa = row[0].ToString();
                var article = row[1].ToString();
                if (!cbCom.Items.Contains(commessa)) cbCom.Items.Add(commessa);
                if (!cbAr.Items.Contains(article)) cbAr.Items.Add(article);

                int.TryParse(row[2].ToString(), out var qty);
                int.TryParse(row[3].ToString(), out var conseg);

                var tess = row[4].ToString();
                var conf = row[5].ToString();
                var department = row[6].ToString();
                
                var manCanti = - (qty - conseg);
                if (qty == 0) qty = 1;
                var dif = Math.Round(Convert.ToDouble(- (manCanti / qty)), 2);

                key = commessa + article + qty;

                if (htbl.ContainsKey(key))
                {
                    var j = Convert.ToInt32(htbl[key]);

                    if (department == "Stiro")
                    {
                        _dataTable.Rows[j][17] = conseg.ToString();
                        _dataTable.Rows[j][18] = manCanti.ToString();
                        _dataTable.Rows[j][19] = dif.ToString();
                        _dataTable.Rows[j][20] = conf;
                    }
                    else if (department == "Confezione A")
                    {
                        _dataTable.Rows[j][9] = conseg.ToString();
                        _dataTable.Rows[j][10] = manCanti.ToString();
                        _dataTable.Rows[j][11] = dif.ToString();
                    }
                    else if (department == "Confezione B")
                    {
                        _dataTable.Rows[j][13] = conseg.ToString();
                        _dataTable.Rows[j][14] = manCanti.ToString();
                        _dataTable.Rows[j][15] = dif.ToString();
                    }
                }
                else
                {
                    var newRow = _dataTable.NewRow();
                    newRow[0] = (idx + 1).ToString();
                    newRow[1] = commessa;
                    newRow[2] = article;
                    newRow[3] = qty.ToString();

                    for (var i = 4; i <= 7; i++)
                    {
                        newRow[i] = 0;
                    }

                    if (department == "Stiro")
                    {
                        newRow[17] = conseg.ToString();
                        newRow[18] = manCanti.ToString();
                        newRow[19] = dif.ToString();
                        newRow[20] = conf;
                    }
                    else if (department == "Confezione A")
                    {
                        newRow[9] = conseg.ToString();
                        newRow[10] = manCanti.ToString();
                        newRow[11] = dif.ToString();

                    }
                    else if (department == "Confezione B")
                    {
                        newRow[13] = conseg.ToString();
                        newRow[14] = manCanti.ToString();
                        newRow[15] = dif.ToString();
                    }

                    _dataTable.Rows.Add(newRow);
                    htbl.Add(key, idx);
                    idx++;
                }
            }

            _bs = new BindingSource();
            _bs.DataSource = _dataTable;
            tableView1.DataSource = _bs;

            CalculateTotals();
        }

        private void TableView1_DataBindingComp(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (tableView1.Rows.Count <= 0) return;
          
            tableView1.ColumnHeadersHeight = 100;
            
            tableView1.Columns[4].HeaderText = "Saldo\ntessitura";
            tableView1.Columns[5].HeaderText = "Differenza\ntessitura";
            tableView1.Columns[6].HeaderText = "% Mancanti\ntessitura";
            tableView1.Columns[7].HeaderText = "TESS";
            tableView1.Columns[9].HeaderText = "Saldo";
            tableView1.Columns[10].HeaderText = "Capi\nmancanti";
            tableView1.Columns[11].HeaderText = "Total\ncapi\nmancanti in %";
            tableView1.Columns[13].HeaderText = "Saldo";
            tableView1.Columns[14].HeaderText = "Capi\nmancanti";
            tableView1.Columns[15].HeaderText = "Total\ncapi\nmancanti in %";
            tableView1.Columns[17].HeaderText = "Saldo";
            tableView1.Columns[18].HeaderText = "Capi\nmancanti";
            tableView1.Columns[19].HeaderText = "Total\ncapi\nmancanti in %";
            tableView1.Columns[22].HeaderText = "Total\ncapi\nmancanti T+C+L+S";
            tableView1.Columns[23].HeaderText = "TOT\nCAPI\nMANCANTI IN %";
            tableView1.Columns[24].HeaderText = "Capi\ndiff";
            tableView1.Columns[26].HeaderText = "% DIFF";

            foreach (DataGridViewColumn c in tableView1.Columns)
            {                
                if (c.Index < 4)
                {
                    c.HeaderCell.Style.ForeColor = Color.White;
                    c.HeaderCell.Style.Alignment = DataGridViewContentAlignment.BottomLeft;
                    c.HeaderCell.Style.Font = new System.Drawing.Font("Segoe UI", 7, FontStyle.Regular);
                    c.Width = 90;
                    c.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    c.HeaderCell.Style.BackColor = Color.FromArgb(125, 141, 161);
                    c.DefaultCellStyle.BackColor = Color.Gainsboro;
                }
                else
                {
                    if (c.Name.Contains("sep_"))
                    {
                        c.HeaderText = "";
                        c.Width = 10;
                        c.DefaultCellStyle.BackColor = Color.White;
                        c.HeaderCell.Style.BackColor = Color.White;
                    }
                    else
                    {
                        c.Width = 60;
                        c.HeaderCell.Style.ForeColor = Color.White;
                        c.HeaderCell.Style.BackColor = Color.FromArgb(125, 141, 161);
                        c.HeaderCell.Style.Alignment = DataGridViewContentAlignment.BottomLeft;
                        c.HeaderCell.Style.ForeColor = Color.White;
                        c.HeaderCell.Style.Alignment = DataGridViewContentAlignment.BottomLeft;
                        c.HeaderCell.Style.Font = new System.Drawing.Font("Segoe UI", 7, FontStyle.Regular);
                    }
                }
            }
            tableView1.Rows[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tableView1.Rows[0].DefaultCellStyle.BackColor = Color.Gainsboro;
            tableView1.Rows[0].DefaultCellStyle.SelectionBackColor = Color.Gainsboro;
            tableView1.Rows[0].DefaultCellStyle.SelectionForeColor = Color.Black;
            tableView1.Rows[0].Height = 50;
            tableView1.Columns[3].Frozen = true;
            tableView1.Rows[0].Frozen = true;
            tableView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(50, 52, 68);
            tableView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.WhiteSmoke;
            tableView1.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            tableView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tableView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

            tableView1.Columns[0].Width = 35;
        }

        private System.Drawing.Font _headerFont = new System.Drawing.Font("Microsoft Sans Serif", 10, FontStyle.Regular);

        System.Drawing.Rectangle _rect = new System.Drawing.Rectangle();
        private void dgvReport_Paint(object sender, PaintEventArgs e)
        {
            var specBrush = new SolidBrush(Color.FromArgb(125, 141, 161));

            for (int j = 4; j < tableView1.ColumnCount - 1;)
            {
                string txt = tableView1.Columns[j].Name.Split('_')[1].ToUpper();

                if (tableView1.Columns[j].HeaderText == string.Empty || tableView1.Columns[j].Name.Contains("sep_"))
                {
                    j += 1;
                    continue;
                }
                               
                _rect = tableView1.GetCellDisplayRectangle(j, -1, true);
                
                _rect.X += -1;
                _rect.Y = 0;
                if (j < 9)
                {
                    _rect.Width = 240;
                }
                else if (j>=9 && j < 17)
                {
                    _rect.Width = 180;
                }
                else if (j > 16 && j < 21)
                {
                    _rect.Width = 240;
                }
                else if (j > 20)
                {
                    txt = "Riepiologo".ToUpper();
                    _rect.Width = 300;
                }

                _rect.Height = 45;
                e.Graphics.FillRectangle(specBrush, _rect);
                e.Graphics.DrawRectangle(Pens.White, _rect);
                StringFormat format = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };

                e.Graphics.DrawString(txt,
                    new System.Drawing.Font("Segoe UI", 10, FontStyle.Bold),
                    Brushes.White,
                    _rect,
                    format);

                j += 4;
                if (txt == "Riepiologo".ToUpper())
                {
                    return;
                }
            }

            //fill rectangle in frozen cell to avoid graphical bugs
            _rect = tableView1.GetCellDisplayRectangle(0, -1, true);
            int wData = tableView1.GetCellDisplayRectangle(0, -1, true).Width;
            _rect.X += -1;
            _rect.Y = 0;
            _rect.Width = tableView1.Columns[0].Width + 1;
            _rect.Height = 90;
            e.Graphics.FillRectangle(new SolidBrush(tableView1.ColumnHeadersDefaultCellStyle.BackColor), _rect);
            e.Graphics.DrawString("Data",
                tableView1.ColumnHeadersDefaultCellStyle.Font,
                Brushes.White,
                _rect.Width / 2 - 20,
                _rect.Height / 2 - 10);
        }

        private void dgvReport_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex > 0 && e.ColumnIndex < tableView1.ColumnCount - 1)
            {
                _rect = e.CellBounds;
                _rect.Y += e.CellBounds.Height / 2;
                _rect.Height = e.CellBounds.Height / 2;
                e.PaintBackground(_rect, true);
                e.PaintContent(_rect);
                e.Handled = true;
            }
        }

        private void DgvReport_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.NewValue > e.OldValue)
            {
                tableView1.Invalidate(_rect);
            }
            else
            {
                tableView1.Invalidate();
                tableView1.Invalidate(_rect);
            }
        }

        public void ExportToExcel()
        {
            tableView1.MultiSelect = true;
            tableView1.ExportToExcel("Chiusura commesse piu difettato");
            tableView1.MultiSelect = false;
        }

        private void CalculateTotals()
        {
            foreach (DataGridViewColumn c in tableView1.Columns)
            {
                var total = 0;
                if (c.Index < 4) continue;
                if (c.Name.Contains("sep_") || c.HeaderText.Contains("Total") || c.Name.Contains("TESS") || c.Name.Contains("CONF")) continue;

                foreach (DataGridViewRow row in tableView1.Rows)
                {
                    int.TryParse(row.Cells[c.Index].Value.ToString(), out var val);

                    total += val;
                }

                tableView1.Rows[0].Cells[c.Index].Value = total.ToString();
            }


            foreach (DataGridViewColumn c in tableView1.Columns)
            {
         
                if (c.HeaderText.Contains("Total"))
                {
                    double.TryParse(tableView1.Rows[0].Cells[c.Index - 2].Value.ToString(), out var first);
                    double.TryParse(tableView1.Rows[0].Cells[c.Index - 1].Value.ToString(), out var second);

                    var mancPerc = Math.Round(second / first, 2);

                    tableView1.Rows[0].Cells[c.Index].Value = mancPerc.ToString() + "%";
                }
            }
        }

        private void CommessaDefect_Load(object sender, EventArgs e)
        {
           
        }

        private void cbCom_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbCom.SelectedIndex == 0)
            {
                _bs.Filter = null;
                tableView1.DataSource = _bs;
                tableView1.Refresh();
                tableView1.Rows[0].Visible = true;
                return;
            }


            _bs.Filter = string.Format("CONVERT(" + tableView1.Columns[1].DataPropertyName +
                                ", System.String) = '" + cbCom.Text.Replace("'", "''") + "'");

            tableView1.DataSource = _bs;
            tableView1.Rows[0].Visible = false;
            tableView1.Refresh();
        }

        private void cbAr_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbAr.SelectedIndex == 0)
            {
                _bs.Filter = null;
                tableView1.DataSource = _bs;
                tableView1.Refresh();

                tableView1.Rows[0].Visible = true;
                return;
            }

            _bs.Filter = string.Format("CONVERT(" + tableView1.Columns[2].DataPropertyName +
                                ", System.String) = '" + cbAr.Text.Replace("'", "''") + "'");

            tableView1.DataSource = _bs;
            tableView1.Rows[0].Visible = false;
            tableView1.Refresh();
        }
    }
}
