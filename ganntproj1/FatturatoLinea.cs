using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ganntproj1
{
    public partial class FatturatoLinea : Form
    {
        public FatturatoLinea()
        {
            InitializeComponent();
            tableView1.DoubleBuffered(true);
            tableView1.DataBindingComplete += TableView1_DataBindingComp;
            tableView1.EnableHeadersVisualStyles = false;
            tableView1.RowTemplate.Height = 18;
        }

        /// <summary>
        /// Gets or sets the Month
        /// </summary>
        private int Month { get; set; }

        /// <summary>
        /// Gets or sets the Year
        /// </summary>
        private int Year { get; set; }

        /// <summary>
        /// Defines the firstRead
        /// </summary>
        private bool firstRead = true;

        private void FatturatoLinea_Load(object sender, EventArgs e)
        {
            for (var i = DateTime.Now.Year - 2; i <= DateTime.Now.Year + 1; i++)
            {
                cboYears.Items.Add(i);
            }

            cboMonth.SelectedIndexChanged += (s, ev) =>
            {
                Month = cboMonth.SelectedIndex + 1;
                if (!firstRead)
                {
                    LoadData();
                }
            };

            cboYears.SelectedIndexChanged += (s, ev) =>
            {
                Year = Convert.ToInt32(cboYears.Text);
                if (!firstRead)
                {
                    LoadData();
                }
            };

            cboYears.SelectedIndex = cboYears.FindString(DateTime.Now.Year.ToString());
            cboMonth.SelectedIndex = DateTime.Now.Month - 1;
        }

        public void LoadData()
        {
            var dt = new DataTable();
            var query = "";

            if (cbYearAll.Checked)
            {
                query = "select data,line,department,sum(price * capi) from produzione " +
                        "where datepart(YEAR,data)='" + Year + "' " +
                        "group by data,line,department " +
                        "order by data, line, department";
            }
            else
            {
                query = "select data,line,department,sum(price * capi) from produzione " +
                        "where datepart(MONTH, data)='" + Month + "' and datepart(YEAR,data)='" + Year + "' " +
                        "group by data,line,department " +
                        "order by data, line, department";
            }

            //query = "select data,line,department,sum(price * capi) from produzione " +
            //            "where datepart(MONTH, data)='" + Month + "' and datepart(YEAR,data)='" + Year + "' " +
            //            "group by data,line,department " +
            //            "order by data, line, department";

            using (var c = new System.Data.SqlClient.SqlConnection(Central.SpecialConnStr))
            {
                var cmd = new System.Data.SqlClient.SqlCommand(query, c);
                c.Open();
                var dr = cmd.ExecuteReader();
                dt.Load(dr);
                c.Close();
                dr.Close();
            }

            var lineList = (from line in ObjectModels.Tables.Lines
                            orderby Convert.ToInt32(line.Line.Remove(0, 5))
                            select new { line.Line, line.Department }).ToList();

            var tbl = new DataTable();
            tbl.Columns.Add("Calendario");
            foreach (var item in lineList)
            {
                tbl.Columns.Add(item.Line + item.Department.Split(' ')[1]);
            }
            tbl.Columns.Add("TOTAL");
            var totRow = tbl.NewRow();
            totRow[0] = "TOTAL PRICE";
            tbl.Rows.Add(totRow);

            var weekRow = tbl.NewRow();
            //var htbl = new System.Collections.Hashtable();
            //var idx = 0;
            var dateRow = tbl.NewRow();
            List<DateTime> list = new List<DateTime>();
            DateTime date = new DateTime(Year, Month, 1);
            do
            {
                list.Add(date);
                date = date.AddDays(1);
            }
            while
            (date.Month == Month);

            var x = 0;
            foreach (var d in list)
            {
                dateRow = tbl.NewRow();
                if (cbYearAll.Checked)
                {
                    dateRow[0] = d.ToString("dd");
                    tbl.Rows.Add(dateRow);
                }
                else
                {
                    dateRow[0] = d.ToString("dd/MM");
                    tbl.Rows.Add(dateRow);
                    if (d.DayOfWeek == DayOfWeek.Sunday)
                    {
                        dateRow = tbl.NewRow();
                        x++;
                        dateRow[0] = "TOTAL " + x.ToString();
                        tbl.Rows.Add(dateRow);
                    }
                }
            }

            foreach (DataRow row in dt.Rows)
            {
                DateTime.TryParse(row.ItemArray.GetValue(0).ToString(), out var dx);
                var line = row.ItemArray.GetValue(1).ToString();
                var depart = row.ItemArray.GetValue(2).ToString().Split(' ')[1];
                double.TryParse(row.ItemArray.GetValue(3).ToString(), out var price);
                var kit = line + depart;

                foreach (DataRow iRow in tbl.Rows)
                {
                    var d = iRow[0].ToString();

                    if (cbYearAll.Checked)
                    {
                        if (d == dx.ToString("dd"))
                        {
                            iRow[kit] = price;
                        }
                    }
                    else
                    {
                        if (d == dx.ToString("dd/MM"))
                        {
                            iRow[kit] = price;
                        }
                    }
              
                }
            }
            tableView1.DataSource = tbl;
            firstRead = false;
            if (cbYearAll.Checked == false)
            {
                CalculateTotals();
            }            
        }

        private void CalculateTotals()
        {
            var tot1 = 0; var tot2 = 0; var tot3 = 0; var tot4 = 0;

            foreach (DataGridViewRow row in tableView1.Rows)
            {
                if (row.Cells[0].Value.ToString() == "TOTAL 1") tot1 = row.Index;
                else if (row.Cells[0].Value.ToString() == "TOTAL 2") tot2 = row.Index;
                else if (row.Cells[0].Value.ToString() == "TOTAL 3") tot3 = row.Index;
                else if (row.Cells[0].Value.ToString() == "TOTAL 4") tot4 = row.Index;
            }
            for (var c = 1; c <= tableView1.Columns.Count - 1; c++)
            {
                var t = 0.0;
                for (var r = 1; r <= tot1 - 1; r++)
                {
                    double.TryParse(tableView1.Rows[r].Cells[c].Value.ToString(), out var x);
                    t += x;
                }
                tableView1.Rows[tot1].Cells[c].Value = "€ " + Math.Round(t, 2).ToString();
            }
            for (var c = 1; c <= tableView1.Columns.Count - 1; c++)
            {
                var t = 0.0;
                for (var r = tot1 + 1; r <= tot2 - 1; r++)
                {
                    double.TryParse(tableView1.Rows[r].Cells[c].Value.ToString(), out var x);
                    t += x;
                }
                tableView1.Rows[tot2].Cells[c].Value = "€ " + Math.Round(t, 2).ToString();
            }
            for (var c = 1; c <= tableView1.Columns.Count - 1; c++)
            {
                var t = 0.0;
                for (var r = tot2 + 1; r <= tot3 - 1; r++)
                {
                    double.TryParse(tableView1.Rows[r].Cells[c].Value.ToString(), out var x);
                    t += x;
                }
                tableView1.Rows[tot3].Cells[c].Value = "€ " + Math.Round(t, 2).ToString();
            }
            for (var c = 1; c <= tableView1.Columns.Count - 1; c++)
            {
                var t = 0.0;
                for (var r = tot3+ 1; r <= tot4 - 1; r++)
                {
                    double.TryParse(tableView1.Rows[r].Cells[c].Value.ToString(), out var x);
                    t += x;
                }
                tableView1.Rows[tot4].Cells[c].Value = "€ " + Math.Round(t, 2).ToString();
            }
            for (var c = 1; c <= tableView1.Columns.Count - 1; c++)
            {
                var t = 0.0;
                for (var r = 1; r <= tableView1.Rows.Count - 1; r++)
                {
                    if (tableView1.Rows[r].Index == tot1 || tableView1.Rows[r].Index == tot2 ||
                        tableView1.Rows[r].Index == tot3 || tableView1.Rows[r].Index == tot4) continue;

                    double.TryParse(tableView1.Rows[r].Cells[c].Value.ToString(), out var x);
                    t += x;
                }
                tableView1.Rows[0].Cells[c].Value = "€ " + Math.Round(t, 2).ToString();
            }
        }
        private void TableView1_DataBindingComp(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            var tv = (TableView)sender;

            tv.RowTemplate.Height = 18;
            tv.Rows[0].DefaultCellStyle.ForeColor = Color.Green;
            tv.Rows[0].DefaultCellStyle.BackColor = Color.White;
            tv.Rows[0].DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8, FontStyle.Bold);
            tv.Rows[0].Frozen = true;
            tv.Rows[0].Height = 30;

            foreach (DataGridViewRow row in tv.Rows)
            {
                if (row.Index > 0 && row.Cells[0].Value.ToString().Contains("TOTAL"))
                {
                    row.DefaultCellStyle.ForeColor = Color.Red;
                    row.DefaultCellStyle.BackColor = Color.Silver;
                    row.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8, FontStyle.Bold);
                }
                //DateTime.TryParse(row.Cells[0].Value.ToString() + "/" + Year.ToString(), out var g);
                //if (g == DateTime.MinValue) continue;
                //DateTime.TryParse(g.ToString("dd/MM/yyyy"), out var dx);
                //if (dx.DayOfWeek == DayOfWeek.Saturday || dx.DayOfWeek == DayOfWeek.Sunday)
                //{
                //    row.DefaultCellStyle.BackColor = Color.DarkSeaGreen;
                //    row.DefaultCellStyle.SelectionBackColor = Color.DarkSeaGreen;
                //    row.DefaultCellStyle.SelectionForeColor = Color.Black;
                //}
            }
        }

        private void CbYearAll_CheckedChanged(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
