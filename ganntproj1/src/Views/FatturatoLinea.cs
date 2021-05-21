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
        public void ExportToExcel()
        {
            tableView1.MultiSelect = true;
            tableView1.ExportToExcel("Fatturato linea");
            tableView1.MultiSelect = false;
        }

        private int Month { get; set; }

        private int Year { get; set; }

        private bool firstRead = true;

        private void FatturatoLinea_Load(object sender, EventArgs e)
        {
            for (var i = DateTime.Now.Year - 3; i <= DateTime.Now.Year; i++)
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

        private List<int> _listOfTotalIdx = new List<int>();
        public void LoadData()
        {
            var dt = new DataTable();
            tableView1.DataSource = null;

            dt.Columns.Add("data");
            dt.Columns.Add("line");
            dt.Columns.Add("department");
            dt.Columns.Add("price");

            var query = "select convert(date,data,121),line,department,sum(price * capi)price from produzione " +
                        "where datepart(MONTH, data)='" + Month + "' " +
                        "and datepart(YEAR, data)='" + Year + "' and " +
                        "charindex(+ ',' + department + ',', '" + Store.Default.arrDept + "' ) > 0" +
                        "group by convert(date,data,121),line,department " +
                        "order by convert(date,data,121), line, department";
           
            using (var c = new System.Data.SqlClient.SqlConnection(Central.SpecialConnStr))
            {
                var cmd = new System.Data.SqlClient.SqlCommand(query, c);
                c.Open();
                var dr = cmd.ExecuteReader();
                if (dr.HasRows)
                    while (dr.Read())
                    {
                        var newRow = dt.NewRow();
                        newRow[0] = Convert.ToDateTime(dr[0]);
                        newRow[1] = dr[1].ToString();
                        newRow[2] = dr[2].ToString();
                        double.TryParse(dr[3].ToString(), out var price);
                        newRow[3] = price;
                        dt.Rows.Add(newRow);
                    }
                c.Close();
                dr.Close();
            }

            var lst = new List<DataCollection>();
            LoadRangeData(lst);
            foreach (var item in lst)
            {
                var newRow = dt.NewRow();
                newRow[0] = item.Datex;
                newRow[1] = item.Line;
                newRow[2] = item.Department;
                var res = (item.Preventivati * item.Price);
                //if (res > 1000.0)
                //    res /= 2.0;

                if (item.Datex.Date > DateTime.Now.Date)
                {
                    newRow[3] = res;
                }
                
                dt.Rows.Add(newRow);
            }

            var q = "select line,department from lines where " +
                "charindex(+ ',' + department + ',', '" + Store.Default.arrDept + "' ) > 0  order by department,len(line),line";
            var dtLine = new DataTable();
            using (var c = new SqlConnection(Central.SpecialConnStr))
            {
                var cmd = new SqlCommand(q, c);
                c.Open();
                var dr = cmd.ExecuteReader();
                dtLine.Load(dr);
                c.Close();
                dr.Close();
            }
            var tbl = new DataTable();
            tbl.Columns.Add("Calendario");

            if (dtLine.Rows.Count == 0) return;

            var firstDept = dtLine.Rows[0][1].ToString();
            _listOfTotalIdx = new List<int>();
            foreach (DataRow row in dtLine.Rows)
            {
                if (firstDept != row[1].ToString())
                {
                    tbl.Columns.Add("TOTAL " + firstDept);
                    _listOfTotalIdx.Add(tbl.Columns.IndexOf("TOTAL " + firstDept));
                }
                var colName = Store.Default.sectorId == 1 ? row[0].ToString() + row[1].ToString().Split(' ')[1]  : row[0].ToString();
                tbl.Columns.Add(colName);
                firstDept = row[1].ToString();
            }
            tbl.Columns.Add("TOTAL " + firstDept);
            _listOfTotalIdx.Add(tbl.Columns.IndexOf("TOTAL " + firstDept));

            tbl.Columns.Add("TOTAL");
            var totRow = tbl.NewRow();
            totRow[0] = "TOTAL PRICE";
            tbl.Rows.Add(totRow);

            totRow = tbl.NewRow();
            totRow[0] = "TOTAL MEDIA";
            tbl.Rows.Add(totRow);

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
                DataRow dateRow = tbl.NewRow();

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

            foreach (DataRow row in dt.Rows)
            {
                DateTime.TryParse(row.ItemArray.GetValue(0).ToString(), out var dx);
                var line = row.ItemArray.GetValue(1).ToString();
                var depart = Store.Default.sectorId == 1 ? row.ItemArray.GetValue(2).ToString().Split(' ')[1] : row.ItemArray.GetValue(2).ToString();
                double.TryParse(row.ItemArray.GetValue(3).ToString(), out var price);
                var kit = Store.Default.sectorId == 1 ? line + depart : line;

                foreach (DataRow iRow in tbl.Rows)
                {
                    var d = iRow[0].ToString();

                    if (d == dx.ToString("dd/MM"))
                    {
                        double.TryParse(iRow[kit].ToString(), out var p);
                        var g = price; // (p + price);
                        //if (g > 1000.0) g /= 2.0;

                        iRow[kit] = String.Format("{0:0.00}", Math.Round(g, 5));
                    }
                }
            }

            tableView1.DataSource = tbl;
            firstRead = false;

            CalculateTotals();
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
                tableView1.Rows[tot1].Cells[c].Value = "€ " + String.Format("{0:0.00}",Math.Round(t, 2));
            }
            for (var c = 1; c <= tableView1.Columns.Count - 1; c++)
            {
                var t = 0.0;
                for (var r = tot1 + 1; r <= tot2 - 1; r++)
                {
                    double.TryParse(tableView1.Rows[r].Cells[c].Value.ToString(), out var x);
                    t += x;
                }
                tableView1.Rows[tot2].Cells[c].Value = "€ " + String.Format("{0:0.00}", Math.Round(t, 2));
            }
            for (var c = 1; c <= tableView1.Columns.Count - 1; c++)
            {
                var t = 0.0;
                for (var r = tot2 + 1; r <= tot3 - 1; r++)
                {
                    double.TryParse(tableView1.Rows[r].Cells[c].Value.ToString(), out var x);
                    t += x;
                }
                tableView1.Rows[tot3].Cells[c].Value = "€ " + String.Format("{0:0.00}", Math.Round(t, 2));
            }
            for (var c = 1; c <= tableView1.Columns.Count - 1; c++)
            {
                var t = 0.0;
                for (var r = tot3+ 1; r <= tot4 - 1; r++)
                {
                    double.TryParse(tableView1.Rows[r].Cells[c].Value.ToString(), out var x);
                    t += x;
                }
                tableView1.Rows[tot4].Cells[c].Value = "€ " + String.Format("{0:0.00}", Math.Round(t, 2));
            }
           
            var startIdx = 1;
            for (var i = 0; i <= _listOfTotalIdx.Count - 1 ; i++)
            {
                var idx = _listOfTotalIdx[i];
                for (var r = 1; r <= tableView1.Rows.Count - 1; r++)
                {
                    var t = 0.0;
                    for (var c = startIdx; c <= idx - 1; c++)
                    {
                        if (!tableView1.Rows[r].Cells[c].Value.ToString().Contains("€ "))
                        {
                            double.TryParse(tableView1.Rows[r].Cells[c].Value.ToString(), out var x);
                            t += x;
                        }
                        else
                        {
                            double.TryParse(tableView1.Rows[r].Cells[c].Value.ToString().Split(' ')[1], out var x);
                            t += x;
                        }
                    }
                    tableView1.Rows[r].Cells[idx].Value = "€ " + String.Format("{0:0.00}", Math.Round(t, 2));
                }
                startIdx = idx + 1;
            }

            var tot = 0.0;
            for (var r = 2; r <= tableView1.Rows.Count - 1; r++)
            {
                for (var i = 0; i <= _listOfTotalIdx.Count - 1; i++)
                {
                    var idx = _listOfTotalIdx[i];
                    double.TryParse(tableView1.Rows[r].Cells[idx].Value.ToString().Split('€')[1].TrimStart(), out var x);

                    tot += x;
                }
                tableView1.Rows[r].Cells["TOTAL"].Value = "€ " + String.Format("{0:0.00}", Math.Round(tot, 2));
                tot = 0.0;
            }

            for (var c = 1; c <= tableView1.Columns.Count - 1; c++)
            {
                var t = 0.0;
                for (var r = 2; r <= tableView1.Rows.Count - 1; r++)
                {
                    if (tableView1.Rows[r].Index == tot1 || tableView1.Rows[r].Index == tot2 ||
                        tableView1.Rows[r].Index == tot3 || tableView1.Rows[r].Index == tot4) continue;
                
                    if (tableView1.Rows[r].Cells[c].Value.ToString().Contains("€ "))
                    {
                        double.TryParse(tableView1.Rows[r].Cells[c].Value.ToString().Remove(0,2), out var x);
                        t += x;
                    }
                    else
                    {
                        double.TryParse(tableView1.Rows[r].Cells[c].Value.ToString(), out var x);
                        t += x;
                    }
                }
                tableView1.Rows[0].Cells[c].Value = "€ " + String.Format("{0:0.00}", Math.Round(t, 2));
            }

            double.TryParse(tableView1.Rows[0].Cells["TOTAL"].Value.ToString().Split(' ')[1], out var totMed);
            var days = 0;

            foreach (DataGridViewRow row in tableView1.Rows)
            {
                if (row.Cells[0].Value.ToString().Contains("TOT")) continue;

                var day = row.Cells[0].Value.ToString().Split('/')[0];
                var month = row.Cells[0].Value.ToString().Split('/')[1];
                var str = month + "/" + day + "/" + cboYears.Text;
                var check = DateTime.ParseExact(str, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                if (check > Config.MinimalDate && check.DayOfWeek != DayOfWeek.Saturday && row.Cells["TOTAL"].Value.ToString() != "€ 0.00")
                {
                    days++;
                }
            }

            var idxTot = tableView1.Rows[1].Cells["TOTAL"].ColumnIndex;
            tableView1.Rows[1].Cells[idxTot - 3].Value = "Giorni";
            tableView1.Rows[1].Cells[idxTot - 2].Value = days.ToString();
            tableView1.Rows[1].Cells[idxTot - 1].Value = "Media";
            tableView1.Rows[1].Cells[idxTot].Value = "€ " + String.Format("{0:0.00}", Math.Round(totMed / days, 2));
        }
        private void TableView1_DataBindingComp(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            var tv = (TableView)sender;

            tv.RowTemplate.Height = 18;
            tv.Rows[0].DefaultCellStyle.ForeColor = Color.Green;
            tv.Rows[0].DefaultCellStyle.BackColor = Color.Gainsboro;
            tv.Rows[0].DefaultCellStyle.SelectionForeColor = Color.Green;
            tv.Rows[0].DefaultCellStyle.SelectionBackColor = Color.Gainsboro;
            tv.Rows[0].DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8, FontStyle.Bold);
            tv.Rows[0].Frozen = true;
            tv.Rows[0].Height = 30;
            tv.Rows[1].DefaultCellStyle.ForeColor = Color.Green;
            tv.Rows[1].DefaultCellStyle.BackColor = Color.Gainsboro;
            tv.Rows[1].DefaultCellStyle.SelectionForeColor = Color.Green;
            tv.Rows[1].DefaultCellStyle.SelectionBackColor = Color.Gainsboro;
            tv.Rows[1].DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8, FontStyle.Bold);
            tv.Rows[1].Frozen = true;
            tv.Rows[1].Height = 30;

            foreach (DataGridViewRow row in tv.Rows)
            {
                if (row.Index > 1 && row.Cells[0].Value.ToString().Contains("TOTAL"))
                {
                    row.DefaultCellStyle.ForeColor = Color.Red;
                    row.DefaultCellStyle.BackColor = Color.Silver;
                    row.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8, FontStyle.Bold);  
                }
                else if (row.Index > 1 && !row.Cells[0].Value.ToString().Contains("TOTAL"))
                {
                    for (var i = 1; i <= tableView1.Columns.Count - 1; i++)
                    {
                        double.TryParse(row.Cells[i].Value.ToString().Split('%')[0], out var val);
                        if (val < 0.0)
                        {
                            row.Cells[i].Style.ForeColor = Color.Red;
                        }
                    }
                }
            }
            for (var i = 0; i<= _listOfTotalIdx.Count-1; i++)
            {
                var idx = _listOfTotalIdx[i];
                tv.Columns[idx].DefaultCellStyle.BackColor = Color.Gainsboro;
                tv.Columns[idx].HeaderCell.Style.BackColor = Color.FromArgb(50, 52, 68);
            }
            tv.Columns["TOTAL"].DefaultCellStyle.BackColor = Color.LightGray;
            tv.Columns[0].Frozen = true;

            foreach (DataGridViewColumn c in tv.Columns)
            {
                if (c.Index == 0)
                {
                    c.DefaultCellStyle.BackColor = Color.Gainsboro;
                }
                c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            foreach (DataGridViewRow row in tableView1.Rows)
            {
                if (row.Cells[0].Value.ToString().Contains("TOT")) continue;

                var day = row.Cells[0].Value.ToString().Split('/')[0];
                var month = row.Cells[0].Value.ToString().Split('/')[1];
                var str = month + "/" + day + "/" + cboYears.Text;
                var check = DateTime.ParseExact(str, "MM/dd/yyyy", System.Globalization.CultureInfo.CurrentCulture);

                if (check.Date > DateTime.Now.Date)
                {
                    row.DefaultCellStyle.ForeColor = Color.Red;
                }
            }
        }

        private void CbYearAll_CheckedChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadRangeData(List<DataCollection> lst)
        {
            var str = Store.Default.sectorId == 1 ? "Confezione" : "Stiro";

            var lstOfHld = (from hld in Central.ListOfHolidays
                            where hld.Department == str
                            && hld.Year == Year && hld.Month == Month
                            select hld).ToList();
            var hldList = new List<DateTime>();
            foreach (var h in lstOfHld)
            {
                var hDate = new DateTime(h.Year, h.Month, h.Holiday.Day, 0, 0, 0);
                if (hldList.Contains(hDate.Date)) continue;
                hldList.Add(hDate.Date);
            }

            LoadComparationList();
            var dt = new DataTable();
            dt.Columns.Add("date");
            dt.Columns.Add("line");
            dt.Columns.Add("prod");
            dt.Columns.Add("prev");
            dt.Columns.Add("dept");
            dt.Columns.Add("price");

            using (var con = new SqlConnection(Central.SpecialConnStr))
            {
                var cmd = new SqlCommand();
                cmd.CommandText = "get_range_values";
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@deptArr", SqlDbType.NVarChar).Value = Store.Default.arrDept;
                cmd.Parameters.Add("@byHour", SqlDbType.Bit).Value = false;

                con.Open();

                var dr = cmd.ExecuteReader();
                if (dr.HasRows)
                    while (dr.Read())
                    {
                        DateTime.TryParse(dr[0].ToString(), out var start);
                        DateTime.TryParse(dr[1].ToString(), out var end);

                        for (var day = start; day <= end; day = day.AddDays(+1))
                        {
                            if (hldList.Contains(day.Date)) continue;

                            if (day.Month == Month && day.Year == Year)
                            {
                                var check = CheckProductionExist(day, dr[2].ToString(), dr[5].ToString());

                                if (check) continue;
                                if (day.DayOfWeek == DayOfWeek.Saturday || day.DayOfWeek == DayOfWeek.Sunday) continue;
                                if (hldList.Contains(day)) continue;

                                var newRow = dt.NewRow();
                                newRow[0] = day;
                                newRow[1] = dr[2].ToString();
                                newRow[2] = dr[3].ToString();
                                newRow[3] = dr[4].ToString();
                                newRow[4] = dr[5].ToString();
                                newRow[5] = Convert.ToDouble(dr[6]);
                                dt.Rows.Add(newRow);
                            }
                        }
                    }

                con.Close();
            }

            foreach (DataRow row in dt.Rows)
            {
                DateTime.TryParse(row[0].ToString(), out var date);
                double.TryParse(row[2].ToString(), out var produc);
                double.TryParse(row[3].ToString(), out var prev);
                double.TryParse(row[5].ToString(), out var price);

                var checkHoliday = Central.ListOfHolidays.FirstOrDefault(x => x.Holiday.Date == date.Date && x.Line == row[1].ToString());
                if (checkHoliday != null) continue;

                lst.Add(new DataCollection
                    (date, row[1].ToString(), "", produc, prev, Convert.ToInt32(prev), row[4].ToString(), price
                    ));
            }

            _lstCompare.Clear();
        }

        private bool CheckProductionExist(DateTime d, string line, string dept)
        {
            var q = from prod in _lstCompare
                    where Convert.ToDateTime(prod.Datex).Date == d.Date
                    && prod.Line == line && prod.Department == dept
                    select prod;

            var prodList = q.ToList();

            if (prodList.Count > 0) return true;
            else return false;
        }

        private List<DataCollection> _lstCompare = new List<DataCollection>();
        private void LoadComparationList()
        {
            _lstCompare = new List<DataCollection>();
            var q = "select data,line,department from produzione where datepart(month,data)='" + Month + "' and datepart(year,data)='" + Year + "'";
            using (var con = new SqlConnection(Central.SpecialConnStr))
            {
                var cmd = new SqlCommand();
                cmd.CommandText = q;
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;

                con.Open();

                var dr = cmd.ExecuteReader();
                if (dr.HasRows)
                    while (dr.Read())
                    {
                        DateTime.TryParse(dr[0].ToString(), out var date);

                        _lstCompare.Add(new DataCollection(
                            date, dr[1].ToString(), "", 0.0, 0.0, 0, dr[2].ToString(), 0.0));
                    }
                con.Close();
            }
        }
    }
}

