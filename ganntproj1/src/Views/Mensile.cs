namespace ganntproj1
{
    using ganntproj1.Models;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Linq;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;

    public partial class Mensile : Form
    {
        public Mensile(string mode)
        {
            InitializeComponent();
            tableView1.DoubleBuffered(true);
            tableView1.DataBindingComplete += TableView1_DataBindingComp;
            tableView1.EnableHeadersVisualStyles = false;
            tableView1.RowTemplate.Height = 18;
            Mode = mode;
        }

        private readonly DataCollection dc = new DataCollection();

        public string Mode { get; set; }

        private int Month { get; set; }

        private int Year { get; set; }

        private bool firstRead = true;

        private bool Ore { get; set; }

        protected override void OnLoad(EventArgs e)
        {            
            cboMonth.SelectedIndexChanged += (s, ev) =>
            {
                Month = cboMonth.SelectedIndex + 1;
                if (!firstRead)
                {
                    _filterCreated = false;
                    _cboType?.Dispose();
                    if (Mode == "mens")
                    {
                        LoadDataMensile();
                    }
                    else
                    {
                        LoadEff();
                    }
                }
            };

            for (var i = DateTime.Now.Year - 3; i <= DateTime.Now.Year; i++)
            {
                cboYears.Items.Add(i);
            }
            cboYears.SelectedIndexChanged += (s, ev) =>
            {
                Year = Convert.ToInt32(cboYears.Text);
                if (!firstRead)
                {
                    _filterCreated = false;
                    _cboType?.Dispose();
                    if (Mode == "mens")
                    {
                        LoadDataMensile();
                    }
                    else
                    {
                        LoadEff();
                    }
                }
            };

            toggleCheckBox1.CheckedChanged += (s, ev) =>
            {
                Ore = toggleCheckBox1.Checked;
                if (Mode == "mens")
                {
                    LoadingInfo.ShowLoading();
                    LoadingInfo.InfoText = Ore ? "Loading by ORE" : "Loading data mensile";
                    LoadDataMensile();
                    LoadingInfo.CloseLoading();
                }
            };

            cboYears.SelectedIndex = cboYears.FindString(DateTime.Now.Year.ToString());
            cboMonth.SelectedIndex = DateTime.Now.Month - 1;
            toggleCheckBox1.Checked = false;

            if (Mode != "mens")
            {
                toggleCheckBox1.Visible = false;
            }


            base.OnLoad(e);
        }

        private List<string> ListOfTypesMensile()
        {
            var lst = new List<string>
            {
                "CAPI PRODUCIBILI",
                "CAPI PREVENTIVATI",
                "CAPI PRODOTI",
                "DIFF PROD-PREVENT",
                "DIFF PROD-PRODUCI"
            };

            return lst;
        }

        private List<string> ListOfTypesEff()
        {
            var lst = new List<string>
            {
                "EFFICIENZA PRE",
                "EFFICIENZA CON",
                "DIFFERENZA"
            };
            return lst;
        }

        public List<DataCollection> GetData(int month, int year)
        {
            try
            {
                using (var c = new SqlConnection(Central.SpecialConnStr))
                {
                    var cmd = new SqlCommand();
                    cmd.Connection = c;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "drop_temporary_tables";
                    c.Open();
                    cmd.ExecuteNonQuery();
                    c.Close();
                }

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

                var q = "";

                if (!Ore)
                {
                    q = "create table tmpTable (datas date, line nvarchar(50), qtyH float,members int, abat float, capi int, dept nvarchar(50)) ";
                    q += "insert into tmpTable ";
                    q += "select convert(date,data,101),line,qtyH,members,case when @paramAb = 0 then (cast(abatim as float)/100) else 1 end, ";
                    q += "capi,department from viewproduction ";
                    q += "where DATEPART(MONTH, data) = '" + month + "' and DATEPART(YEAR, data)= '" + year + "' ";
                    q += "and charindex(+ ',' + department + ',', '" + Store.Default.arrDept + "' ) > 0 ";
                    q += "order by department,len(line),line,convert(date, data, 101) ";
                    q += "create table tmpSum (datas date,line nvarchar(50),produc float,prevent float,qty int,dept nvarchar(50),cnt int) ";
                    q += "insert into tmpSum ";
                    q += "select datas,line,sum((qtyH * members))producibili, ";
                    q += "sum((qtyH * members * abat))prevent, sum(capi)qty,dept,count(1) from tmpTable ";
                    q += "group by datas,line,dept order by dept,len(line),line,datas ";
                    q += "select datas,line, (produc / cnt * " +
                        "case when DATEPART(DW, datas) <> 7 THEN '" +
                          hour + "' ELSE '" + hourW + "' END)producibili," +
                        "(prevent / cnt * case when DATEPART(DW, datas) <> 7 THEN '" +
                          hour + "' ELSE '" + hourW + "' END),qty,dept from tmpSum ";
                    q += "drop table tmpTable drop table tmpSum";
                }
                else
                {
                    q = "create table tmpTable (datas date, line nvarchar(50), qtyH float,members int, abat float, capi int, dept nvarchar(50)) ";
                    q += "insert into tmpTable ";
                    q += "select convert(date,data,101),line,qtyH,members,case when @paramAb = 0 then (cast(abatim as float)/100) else 1 end, ";
                    q += "capi,department from viewproduction ";
                    q += "where DATEPART(MONTH, data) = '" + month + "' and DATEPART(YEAR, data)= '" + year + "' ";
                    q += "and charindex(+ ',' + department + ',', '" + Store.Default.arrDept + "' ) > 0 ";
                    q += "order by department,len(line),line,convert(date, data, 101) ";
                    q += "create table tmpSum (datas date,line nvarchar(50),produc float,prevent float,qty int,dept nvarchar(50),cnt int) ";
                    q += "insert into tmpSum ";
                    q += "select datas,line,sum(members) as producibili, ";
                    q += "sum(members * abat) as prevent, sum(capi / qtyH)qty,dept,count(1) from tmpTable ";
                    q += "group by datas,line,dept order by dept,len(line),line,datas ";
                    q += "select datas,line, (produc / cnt * " +
                        "case when DATEPART(DW, datas) <> 7 THEN '" +
                          hour + "' ELSE '" + hourW + "' END)producibili," +
                        "(prevent / cnt * case when DATEPART(DW, datas) <> 7 THEN '" +
                          hour + "' ELSE '" + hourW + "' END),qty,dept from tmpSum ";
                    q += "drop table tmpTable drop table tmpSum";
                }

                var lst = new List<DataCollection>();
                if (Mode == "mens")
                {
                    using (var c = new SqlConnection(Central.SpecialConnStr))
                    {
                        var cmd = new SqlCommand(q, c);
                        cmd.Parameters.Add("@paramAb", SqlDbType.BigInt).Value = cbAbatim.Checked;
                        c.Open();
                        var dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                            while (dr.Read())
                            {
                                foreach (var type in ListOfTypesMensile())
                                {
                                    if (_cboType.SelectedIndex > 0 && _cboType.Text != type) continue;
                                    DateTime.TryParse(dr[0].ToString(), out var dt);
                                    double.TryParse(dr[2].ToString(), out var produc);
                                    double.TryParse(dr[3].ToString(), out var prevent);
                                    int.TryParse(dr[4].ToString(), out var qty);
                                    var dpt = dr[5].ToString();
                                    lst.Add(new DataCollection(
                                        dt,
                                        dr[1].ToString(), type, produc, prevent, qty, dpt, 0.0)
                                        );
                                }
                            }
                        c.Close();
                        dr.Close();
                    }
                }
                else
                {
                    using (var c = new SqlConnection(Central.SpecialConnStr))
                    {
                        var cmd = new SqlCommand(q, c);
                        cmd.Parameters.Add("@paramAb", SqlDbType.BigInt).Value = cbAbatim.Checked;
                        c.Open();
                        var dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                            while (dr.Read())
                            {
                                foreach (var type in ListOfTypesEff())
                                {
                                    if (_cboType.SelectedIndex > 0 && _cboType.Text != type) continue;

                                    DateTime.TryParse(dr[0].ToString(), out var dt);
                                    double.TryParse(dr[2].ToString(), out var produc);
                                    double.TryParse(dr[3].ToString(), out var prevent);
                                    int.TryParse(dr[4].ToString(), out var qty);
                                    var dpt = dr[5].ToString();
                                    lst.Add(new DataCollection(
                                        dt,
                                        dr[1].ToString(), type, produc, prevent, qty, dpt,0.0)
                                        );
                                }
                            }
                        c.Close();
                        dr.Close();
                    }
                }

                return lst;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return new List<DataCollection>();
            }           
        }
        public void LoadDataMensile()
        {
            tableView1.DataSource = null;
            var month = cboMonth.SelectedIndex + 1;
            int.TryParse(cboYears.Text, out var year);
            var dt = new DataTable();
            dt.Columns.Add("Linea");
            dt.Columns.Add("Type");
            var tpMax = ListOfTypesMensile().Count;
            var i = 1;
            for (var date = new DateTime(Year, Month, 1);
                date <= new DateTime(Year, Month, DateTime.DaysInMonth(Year, Month)); date = date.AddDays(+1))
            {
                var col = new DataColumn()
                {
                    ColumnName = date.ToString("yyyy-MM-dd",
                    System.Globalization.CultureInfo.CurrentCulture),
                };
                col.DefaultValue = null;
                dt.Columns.Add(col.ColumnName, typeof(string));
                if (date.DayOfWeek == DayOfWeek.Sunday)
                {
                    dt.Columns.Add(i.ToString() + "^ SETT");
                    i++;
                }
            }
            dt.Columns.Add("TOTAL");
            var weekDayRow = dt.NewRow();
            foreach (DataColumn dcol in dt.Columns)
            {
                DateTime.TryParse(dcol.ColumnName, out var day);
                if (day == DateTime.MinValue) continue;

                weekDayRow[day.ToString("yyyy-MM-dd")] = day.ToString("ddd").Remove(1, 2);
            }
            dt.Rows.Add(weekDayRow);
            var x = 0;
            foreach (var item in ListOfTypesMensile())
            {
                DataRow totRow = dt.NewRow();
                if (x == 0) totRow[0] = "TOTALE";
                totRow[1] = item;
                dt.Rows.Add(totRow);
                x++;
            }
            var emptyRow = dt.NewRow();
            dt.Rows.Add(emptyRow);

            var lastLine = "";
            var data = GetData(month, year);
            LoadRangeData(data);
            if (data.Count > 0)
            {
                lastLine = Store.Default.sectorId == 1 ? data.First().Line + data.First().Department.Split(' ')[1] : data.First().Line;
            }
            var htbl = new System.Collections.Hashtable();
            var idx = tpMax + 2;
            var tmpIdx = 0;

            var lineQuery = (from lines in Tables.Lines
                            select lines).ToList();

            foreach (var item in data)
            {
                var hKey = string.Empty;
                var line = Store.Default.sectorId == 1 ? (item.Line + item.Department.Split(' ')[1]) : item.Line;

                hKey = line + item.Type;
                var dateIdx = item.Datex.ToString("yyyy-MM-dd");
               
                var cProducibili = Math.Round(item.Producibili,0);
                var cPreventivati = Math.Round(item.Preventivati,0);

                var lineDesc = lineQuery.Where(l => l.Line == item.Line && l.Department == item.Department).FirstOrDefault(); 

                if (htbl.Contains(hKey))
                {
                    var j = Convert.ToInt32(htbl[hKey]);

                    switch (item.Type)
                    {
                        case "CAPI PRODUCIBILI":
                            dt.Rows[j][dateIdx] = cProducibili.ToString();
                            break;
                        case "CAPI PREVENTIVATI":
                            dt.Rows[j][dateIdx] = cPreventivati.ToString();
                            break;
                        case "CAPI PRODOTI":
                            dt.Rows[j][dateIdx] = item.Qty.ToString();
                            break;
                        case "DIFF PROD-PREVENT":
                            dt.Rows[j][dateIdx] = (item.Qty - cPreventivati).ToString();
                            break;
                        case "DIFF PROD-PRODUCI":
                            dt.Rows[j][dateIdx] = (item.Qty - cProducibili).ToString();
                            break;
                    }
                }
                else
                {
                    if (lastLine != line)
                    {
                        emptyRow = dt.NewRow();
                        dt.Rows.Add(emptyRow);
                        idx++;
                        if (idx > 12) tmpIdx++;  
                    }
                    lastLine = line;
                    var newRow = dt.NewRow();

                    if (_cboType.SelectedIndex <= 0 && (idx - tmpIdx) % tpMax == 2)
                        newRow[0] = line;
                    else if (_cboType.SelectedIndex <= 0 && (idx - tmpIdx) % tpMax == 3)
                        newRow[0] = lineDesc.Description;
                    else if (_cboType.SelectedIndex > 0)
                        newRow[0] = lineDesc.Description;

                    newRow[1] = item.Type;
                    switch (item.Type)
                    {
                        case "CAPI PRODUCIBILI":
                            newRow[dateIdx] = cProducibili.ToString();
                            break;
                        case "CAPI PREVENTIVATI":
                            newRow[dateIdx] = cPreventivati.ToString();
                            break;
                        case "CAPI PRODOTI":
                            newRow[dateIdx] = item.Qty.ToString();
                            break;
                        case "DIFF PROD-PREVENT":
                            newRow[dateIdx] = (item.Qty - cPreventivati).ToString();
                            break;
                        case "DIFF PROD-PRODUCI":
                            newRow[dateIdx] = (item.Qty - cProducibili).ToString();
                            break;
                    }
                    dt.Rows.Add(newRow);
                    htbl.Add(hKey, idx);
                    idx++;
                }
            }
            tableView1.DataSource = dt;
            firstRead = false;
            CalculateTotalsMens();
            foreach (DataGridViewColumn c in tableView1.Columns)
            {
                c.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
            CreateFilter(ListOfTypesMensile());
        }

        public void LoadEff()
        {
            tableView1.DataSource = null;
            var month = cboMonth.SelectedIndex + 1;
            int.TryParse(cboYears.Text, out var year);
            var dt = new DataTable();
            dt.Columns.Add("Linea");
            dt.Columns.Add("Type");
            var tpMax = ListOfTypesEff().Count;
            var i = 1;
            for (var date = new DateTime(Year, Month, 1);
                date <= new DateTime(Year, Month, DateTime.DaysInMonth(Year, Month)); date = date.AddDays(+1))
            {
                var col = new DataColumn()
                {
                    ColumnName = date.ToString("yyyy-MM-dd",
                    System.Globalization.CultureInfo.CurrentCulture),
                };
                col.DefaultValue = null;
                dt.Columns.Add(col.ColumnName, typeof(string));
                if (date.DayOfWeek == DayOfWeek.Sunday)
                {
                    dt.Columns.Add(i.ToString() + "^ SETT");
                    i++;
                }
            }
            dt.Columns.Add("TOT");
            var weekDayRow = dt.NewRow();
            foreach (DataColumn dcol in dt.Columns)
            {
                DateTime.TryParse(dcol.ColumnName, out var day);
                if (day == DateTime.MinValue) continue;

                weekDayRow[day.ToString("yyyy-MM-dd")] = day.ToString("ddd",
                    System.Globalization.CultureInfo.InvariantCulture).Remove(1, 2);
            }
            dt.Rows.Add(weekDayRow);
            var x = 0;
            foreach (var item in ListOfTypesEff())
            {
                DataRow totRow = dt.NewRow();
                if (x == 0) totRow[0] = "TOTALE";
                totRow[1] = item;
                dt.Rows.Add(totRow);
                x++;
            }
            var emptyRow = dt.NewRow();
            dt.Rows.Add(emptyRow);
            var lastLine = "";
            var data = GetData(month, year);
            if (data.Count > 0)
            {
                lastLine = Store.Default.sectorId == 1 ? data.First().Line + data.First().Department.Split(' ')[1] : data.First().Line;
            }
            var htbl = new System.Collections.Hashtable();
            var idx = tpMax + 2;
            var tmpIdx = 0;

            var lineQuery = (from lines in Tables.Lines
                             select lines).ToList();


            foreach (var item in data)
            {
                var maxEff = 100.0;
                var workEff = Math.Round((item.Qty / item.Producibili) * 100.0,1);
                if (double.IsNaN(workEff) || double.IsInfinity(workEff)) workEff = 0.0;
                var diffEff = Math.Round(workEff - maxEff, 1);
                var hKey = string.Empty;
                var line = Store.Default.sectorId == 1 ? (item.Line + item.Department.Split(' ')[1]) : item.Line;

                hKey = line + item.Type;             
                var dateIdx = item.Datex.ToString("yyyy-MM-dd");

                var lineDesc = lineQuery.Where(l => l.Line == item.Line && l.Department == item.Department).FirstOrDefault();

                if (htbl.Contains(hKey))
                {
                    var j = Convert.ToInt32(htbl[hKey]);
                    switch (item.Type)
                    {
                        case "EFFICIENZA PRE":
                            dt.Rows[j][dateIdx] = maxEff.ToString() + "%";
                            dt.Rows[1][dateIdx] = "100%";
                            break;
                        case "EFFICIENZA CON":
                            dt.Rows[j][dateIdx] = workEff.ToString() + "%";
                            break;
                        case "DIFFERENZA":
                            dt.Rows[j][dateIdx] = diffEff.ToString() + "%";
                            break;
                    }
                }
                else
                {
                    if (lastLine != line)     
                    {
                        emptyRow = dt.NewRow();
                        dt.Rows.Add(emptyRow);
                        idx++;
                        if (idx > 7) tmpIdx++;
                    }
                    lastLine = line;
                    var newRow = dt.NewRow();
                    if (_cboType.SelectedIndex <= 0 && (idx - tmpIdx) % tpMax == 2) 
                        newRow[0] = line;
                    else if (_cboType.SelectedIndex <= 0 && (idx - tmpIdx) % tpMax == 0)
                        newRow[0] = lineDesc.Description;
                    else if (_cboType.SelectedIndex > 0) newRow[0] = lineDesc.Description;

                    newRow[1] = item.Type;
                    switch (item.Type)
                    {
                        case "EFFICIENZA PRE":
                            newRow[dateIdx] = maxEff.ToString() + "%";
                            dt.Rows[1][dateIdx] = "100%";
                            break;
                        case "EFFICIENZA CON":
                            newRow[dateIdx] = workEff.ToString() + "%";
                            break;
                        case "DIFFERENZA":
                            newRow[dateIdx] = diffEff.ToString() + "%";
                            break;
                    }
                    dt.Rows.Add(newRow);
                    htbl.Add(hKey, idx);
                    idx++;
                }
            }

            tableView1.DataSource = dt;
            firstRead = false;
            CalculateTotalsEff();
            foreach (DataGridViewColumn c in tableView1.Columns)
            {
                c.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
            CreateFilter(ListOfTypesEff());
        }

        private void CalculateTotalsMens()
        {
            var idx1 = tableView1.Columns["1^ SETT"].Index;
            var idx2 = tableView1.Columns["2^ SETT"].Index;
            var idx3 = tableView1.Columns["3^ SETT"].Index;
            var idx4 = tableView1.Columns["4^ SETT"].Index;

            for (var r = 1; r <= tableView1.Rows.Count - 1; r++)
            {
                var row = tableView1.Rows[r];
                var tot = 0.0;
                for (var x = 2; x < idx1; x++)
                {
                    double.TryParse(row.Cells[x].Value.ToString(), out var t);
                    tot += t;
                }
                row.Cells[idx1].Value = Math.Round(tot, 1).ToString();
            }
            for (var r = 1; r <= tableView1.Rows.Count - 1; r++)
            {
                var row = tableView1.Rows[r];
                var tot = 0.0;
                for (var x = idx1 + 1; x < idx2; x++)
                {
                    double.TryParse(row.Cells[x].Value.ToString(), out var t);
                    tot += t;
                }
                row.Cells[idx2].Value = Math.Round(tot, 1).ToString();
            }
            for (var r = 1; r <= tableView1.Rows.Count - 1; r++)
            {
                var row = tableView1.Rows[r];
                var tot = 0.0;
                for (var x = idx2 + 1; x < idx3; x++)
                {
                    double.TryParse(row.Cells[x].Value.ToString(), out var t);
                    tot += t;
                }
                row.Cells[idx3].Value = Math.Round(tot, 1).ToString();
            }
            for (var r = 1; r <= tableView1.Rows.Count - 1; r++)
            {
                var row = tableView1.Rows[r];
                var tot = 0.0;
                for (var x = idx3 + 1; x < idx4; x++)
                {
                    double.TryParse(row.Cells[x].Value.ToString(), out var t);
                    tot += t;
                }
                row.Cells[idx4].Value = Math.Round(tot, 1).ToString();
            }
            for (var r = 1; r<= tableView1.Rows.Count - 1; r++)
            {
                var tot = 0;
                for (var c = 2; c <= tableView1.Columns.Count - 2; c++)
                {
                    var cd = tableView1.Columns[c].Index;
                    if (cd == idx1 || cd == idx2 || cd == idx3 || cd == idx4) continue;
                    int.TryParse(tableView1.Rows[r].Cells[c].Value.ToString(), out var t);
                    tot += t;
                }
                tableView1.Rows[r].Cells["TOTAL"].Value = tot.ToString();
            }

            for (var c = 2; c <= tableView1.Columns.Count - 1; c++)
            {
                var totProd = 0;
                var totPrev = 0;
                var totQty = 0;

                for (var r = 6; r <= tableView1.Rows.Count - 1; r++)
                {
                    if (tableView1.Rows[r].Cells[1].Value.ToString() == "CAPI PRODUCIBILI")
                    {
                        int.TryParse(tableView1.Rows[1].Cells[c].Value.ToString().Split('%')[0], out var tot);
                        int.TryParse(tableView1.Rows[r].Cells[c].Value.ToString().Split('%')[0], out var t);
                        if (t == 0) continue;

                        tableView1.Rows[1].Cells[c].Value = (tot + t).ToString();
                        totProd += t;
                    }
                    else if (tableView1.Rows[r].Cells[1].Value.ToString() == "CAPI PREVENTIVATI")
                    {
                        int.TryParse(tableView1.Rows[2].Cells[c].Value.ToString().Split('%')[0], out var tot);
                        int.TryParse(tableView1.Rows[r].Cells[c].Value.ToString().Split('%')[0], out var t);
                        if (t == 0) continue;

                        tableView1.Rows[2].Cells[c].Value = (tot + t).ToString();
                        totPrev += t;
                    }
                    else if (tableView1.Rows[r].Cells[1].Value.ToString() == "CAPI PRODOTI")
                    {
                        int.TryParse(tableView1.Rows[3].Cells[c].Value.ToString().Split('%')[0], out var tot);
                        int.TryParse(tableView1.Rows[r].Cells[c].Value.ToString().Split('%')[0], out var t);
                        if (t == 0) continue;

                        tableView1.Rows[3].Cells[c].Value = (tot + t).ToString();
                        totQty += t;
                    }
                }
                if (totPrev != 0 || totProd != 0)
                {
                    tableView1.Rows[4].Cells[c].Value = (totQty - totPrev).ToString();
                    tableView1.Rows[5].Cells[c].Value = (totQty - totProd).ToString();
                }
            }
        }

        private void CalculateTotalsEff()
        {
            for (var c = 2; c <= tableView1.Columns.Count - 1; c++)
            {
                var tot = 0.0;
                var x = 0;
                for (var r = 6; r <= tableView1.Rows.Count - 1; r++)
                {
                    if (tableView1.Rows[r].Cells[1].Value.ToString() != "EFFICIENZA CON") continue;
                    double.TryParse(tableView1.Rows[r].Cells[c].Value.ToString().Split('%')[0], out var t);
                    if (t == 0.0) continue;
                    tot += t;
                    x++;
                }
                double.TryParse(tot.ToString(), out var totDoub);
                double.TryParse(x.ToString(), out var xt);
                var res = Math.Round(totDoub / xt,1);
                string strRes;
                if (double.IsNaN(res) || double.IsInfinity(res)) strRes = "";
                else strRes = res.ToString() + "%";
                var diff = Math.Round(100.0 - res, 1);
                string strDiff;
                if (double.IsNaN(diff) || double.IsInfinity(diff)) strDiff = "";
                else strDiff = diff.ToString() + "%";

                tableView1.Rows[2].Cells[c].Value = strRes;
                tableView1.Rows[3].Cells[c].Value = strDiff;
            }

            var idx1 = tableView1.Columns["1^ SETT"].Index;
            var idx2 = tableView1.Columns["2^ SETT"].Index;
            var idx3 = tableView1.Columns["3^ SETT"].Index;
            var idx4 = tableView1.Columns["4^ SETT"].Index;

            for (var r = 1; r <= tableView1.Rows.Count - 1; r++)
            {
                var row = tableView1.Rows[r];
                var tot = 0.0;
                var ct = 0;
                for (var x = 2; x < idx1; x++)
                {
                    double.TryParse(row.Cells[x].Value.ToString().Split('%')[0], out var t);
                    if (t != 0) ct++;
                    tot += t;
                }
                var result = Math.Round(tot / ct, 1);
                if (double.IsNaN(result) || double.IsInfinity(result)) result = 0.0;
                row.Cells[idx1].Value = result.ToString() + "%";
            }
            for (var r = 1; r <= tableView1.Rows.Count - 1; r++)
            {
                var row = tableView1.Rows[r];
                var tot = 0.0;
                var ct = 0;
                for (var x = idx1 + 1; x < idx2; x++)
                {
                    double.TryParse(row.Cells[x].Value.ToString().Split('%')[0], out var t);
                    if (t != 0) ct++;
                    tot += t;
                }
                var result = Math.Round(tot / ct, 1);
                if (double.IsNaN(result) || double.IsInfinity(result)) result = 0.0;
                row.Cells[idx2].Value = result.ToString() + "%";
            }
            for (var r = 1; r <= tableView1.Rows.Count - 1; r++)
            {
                var row = tableView1.Rows[r];
                var tot = 0.0;
                var ct = 0;
                for (var x = idx2 + 1; x < idx3; x++)
                {
                    double.TryParse(row.Cells[x].Value.ToString().Split('%')[0], out var t);
                    if (t != 0) ct++;
                    tot += t;
                }
                var result = Math.Round(tot / ct, 1);
                if (double.IsNaN(result) || double.IsInfinity(result)) result = 0.0;
                row.Cells[idx3].Value = result.ToString() + "%";
            }
            for (var r = 1; r <= tableView1.Rows.Count - 1; r++)
            {
                var row = tableView1.Rows[r];
                var tot = 0.0;
                var ct = 0;
                for (var x = idx3 + 1; x < idx4; x++)
                {
                    double.TryParse(row.Cells[x].Value.ToString().Split('%')[0], out var t);
                    if (t != 0) ct++;
                    tot += t;
                }
                var result = Math.Round(tot / ct, 1);
                if (double.IsNaN(result) || double.IsInfinity(result)) result = 0.0;
                row.Cells[idx4].Value = result.ToString() + "%";
            }

            for (var r = 1; r <= tableView1.Rows.Count - 1; r++)
            {
                var tot = 0.0;
                var x = 0;
                for (var c = 2; c<= tableView1.ColumnCount-1;c++)
                {
                    DateTime.TryParse(tableView1.Columns[c].Name, out var dt);
                    if (dt == DateTime.MinValue) continue;
                    double.TryParse(tableView1.Rows[r].Cells[c].Value.ToString().Split('%')[0], out var eff);
                    if (eff != 0) x++;
                    tot += eff;
                }
                tableView1.Rows[r].Cells["TOT"].Value =Math.Round(tot/ x, 1).ToString() + "%";
            }
        }

        private void CbAcconto_CheckedChanged(object sender, EventArgs e)
        {
            LoadDataMensile();
        }

        private void TableView1_DataBindingComp(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (tableView1.Rows.Count <= 0) return;
            tableView1.Rows[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tableView1.Rows[0].DefaultCellStyle.BackColor = Color.Gainsboro;
            tableView1.Rows[0].DefaultCellStyle.SelectionBackColor = Color.Gainsboro;
            tableView1.Rows[0].DefaultCellStyle.SelectionForeColor = Color.Black;
            tableView1.Rows[0].Frozen = true;
            tableView1.Columns[0].DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 10, FontStyle.Bold);
            tableView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(50, 52, 68);
            tableView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.WhiteSmoke;

            tableView1.ColumnHeadersHeight = 50;
            foreach (DataGridViewRow row in tableView1.Rows)
            {
                if (row.Cells[1].Value.ToString() == "CAPI PRODOTI")
                {
                    row.DefaultCellStyle.BackColor = Color.LightSteelBlue;
                }
                if (Mode == "mens" && row.Index > 0 && row.Index <= 5)
                {
                    row.DefaultCellStyle.BackColor = Color.Gainsboro;
                    row.DefaultCellStyle.SelectionBackColor = Color.Gainsboro;
                    row.DefaultCellStyle.SelectionForeColor = Color.Green;
                    row.Frozen = true;
                }
                else if (Mode == "eff" && row.Index > 0 && row.Index <= 3)
                {
                    row.DefaultCellStyle.BackColor = Color.Gainsboro;
                    row.DefaultCellStyle.SelectionBackColor = Color.Gainsboro;
                    row.DefaultCellStyle.SelectionForeColor = Color.Green;
                    row.Frozen = true;
                }
                if (Mode == "mens" && row.Index > 0)
                {
                    for (var i = 2; i <= tableView1.Columns.Count - 1; i++)
                    {
                        int.TryParse(row.Cells[i].Value.ToString(), out var val);
                        if (val < 0)
                        {
                            row.Cells[i].Style.ForeColor = Color.Red;
                        }
                    }
                }
                else if (Mode == "eff" && row.Index > 0)
                {
                    for (var i = 2; i <= tableView1.Columns.Count - 1; i++)
                    {
                        double.TryParse(row.Cells[i].Value.ToString().Split('%')[0], out var val);
                        if (val < 0.0)
                        {
                            row.Cells[i].Style.ForeColor = Color.Red;
                        }
                    }
                }
            }
            foreach (DataGridViewColumn c in tableView1.Columns)
            {
                c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                DateTime.TryParse(c.Name, out var dx);
                if (dx == DateTime.MinValue) continue;
                c.HeaderText = dx.ToString("dd-MMM",
                    System.Globalization.CultureInfo.InvariantCulture);
                if (dx.DayOfWeek == DayOfWeek.Saturday || dx.DayOfWeek == DayOfWeek.Sunday)
                {
                    c.HeaderCell.Style.BackColor = Color.FromArgb(125, 141, 161);
                    c.DefaultCellStyle.BackColor = Color.Gainsboro;
                }
            }

            tableView1.Columns[0].DefaultCellStyle.BackColor = Color.Gainsboro;
            tableView1.Columns[1].DefaultCellStyle.BackColor = Color.Gainsboro;
            tableView1.Columns[0].Frozen = true;
            tableView1.Columns[1].Frozen = true;
            tableView1.RowTemplate.Height = 18;

            foreach (DataGridViewColumn c in tableView1.Columns)
            {
                DateTime.TryParse(c.Name, out var dt);

                if (dt > DateTime.Now)
                {
                    c.DefaultCellStyle.ForeColor = Color.Red;
                }

                if (c.HeaderText.Contains("^ SETT"))
                {
                    c.DefaultCellStyle.BackColor = Color.FromArgb(125, 141, 161); ;
                }
                
            }
        }

        private void TableView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (Mode == "mens" && e.RowIndex > 5 || e.RowIndex > 1)
            {
                if (tableView1.Rows[e.RowIndex].Cells[0].Value.ToString() == string.Empty &&
                 tableView1.Rows[e.RowIndex].Cells[1].Value.ToString() == string.Empty)
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(161, 161, 161)), e.CellBounds.X, e.CellBounds.Y, e.CellBounds.Width, e.CellBounds.Height);
                    e.Graphics.FillRectangle(Brushes.White, e.CellBounds.X, e.CellBounds.Y, e.CellBounds.Width, e.CellBounds.Height - 1);
                    e.Handled = true;
                }
            }
        }

        public void ExportToExcel()
        {
            tableView1.MultiSelect = true;
            if (Mode == "mens")
            {
                tableView1.ExportToExcel("Report Produzione Mensile");
            }
            else
            {
                tableView1.ExportToExcel("Report Effizienza per Linea");
            }
            tableView1.MultiSelect = false;
        }

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

            string tit;
            string subTit;

            if (Mode == "mens")
            {
                tit = "Report Produzione Mensile";
                subTit = cboMonth.Text + "-" + cboYears.Text + "\n" + "Print date: " + DateTime.Now.ToString("dd/MM/yyyy");
            }
            else
            {
                tit = "Report Effizienza per Linea";
                subTit = cboMonth.Text + "-" + cboYears.Text + "\n" + "Print date: " + DateTime.Now.ToString("dd/MM/yyyy");
            }

            var dGvPrinter = new TableViewPrint
            {
                Title = tit,
                SubTitle = subTit,
                SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip,
                PageNumbers = true,
                PageNumberInHeader = false,
                PorportionalColumns = true,
                HeaderCellAlignment = StringAlignment.Near,
                Footer = "ONLYOU",
                FooterSpacing = 15,
                CellAlignment = StringAlignment.Center,
            };
            dGvPrinter.PageSettings.Landscape = true;
            dGvPrinter.PrintDataGridView(tableView1);
            Controls.Remove(lbl);
            lbl.Dispose();
        }

        private void TableView1_SelectionChanged(object sender, EventArgs e)
        {
        }

        private bool _filterCreated = false;

        private ComboBox _cboType = new ComboBox();

        private void CreateFilter(List<string> lst)
        {
            if (tableView1.Rows.Count <= 0) return;
            if (_cboType.SelectedIndex > 0) return;
            if (_filterCreated)
            {
                var headerRect = tableView1.GetColumnDisplayRectangle(1, true);
                _cboType.Location = new Point(headerRect.Location.X + 1, 50 - _cboType.Height - 2);
                _cboType.Size = new Size(headerRect.Width - 3, tableView1.ColumnHeadersHeight);
                _cboType.DropDownWidth = 200;
            }
            else
            {
                _cboType = new ComboBox
                {
                    Name = tableView1.Columns[1].Name,
                    BackColor = Color.White,
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    Font = new Font("Microsoft Sans Serif", 10, FontStyle.Regular),
                    Parent = tableView1,
                    DropDownWidth = 200,
                };
                _cboType.Items.Add("<Reset>");
                foreach (var item in lst)
                {
                    _cboType.Items.Add(item);
                }

                tableView1.Controls.Add(_cboType);

                var headerRect = tableView1.GetColumnDisplayRectangle(1, true);
                _cboType.Location = new Point(headerRect.Location.X + 1, 50 - _cboType.Height - 2);
                _cboType.Size = new Size(headerRect.Width - 3, tableView1.ColumnHeadersHeight);
                _filterCreated = true;
            }
            _cboType.SelectedIndexChanged += (s, ev) =>
            {
                if (Mode == "mens")
                {
                    LoadDataMensile();
                }
                else
                {
                    LoadEff();
                }
            };
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

            var tcb = toggleCheckBox1.Checked ? 1 : 0;

            using (var con = new SqlConnection(Central.SpecialConnStr))
            {
                var cmd = new SqlCommand();
                cmd.CommandText = "get_range_values";
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@deptArr", SqlDbType.NVarChar).Value = Store.Default.arrDept;
                cmd.Parameters.Add("@byHour", SqlDbType.Bit).Value = tcb;

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
                foreach (var type in ListOfTypesMensile())
                {
                    DateTime.TryParse(row[0].ToString(), out var date);
                    double.TryParse(row[2].ToString(), out var produc);
                    double.TryParse(row[3].ToString(), out var prev);
                    double.TryParse(row[5].ToString(), out var price);

                    var checkHoliday = Central.ListOfHolidays.FirstOrDefault(x => x.Holiday.Date == date.Date && x.Line == row[1].ToString());
                    if (checkHoliday != null) continue;

                    lst.Add(new DataCollection
                        (date, row[1].ToString(),type, produc, prev, 0, row[4].ToString(), price
                        ));
                }
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
                            date, dr[1].ToString(),"", 0.0, 0.0,0, dr[2].ToString(), 0.0));
                    }
                con.Close();
            }            
        }
    }

    public class DataCollection
    {
        public DataCollection()
        {
        }

        public double Producibili { get; set; }
        public double Preventivati { get; set; }

        public DataCollection(DateTime date, string line, string type,
            double produc, double prev, int qtyToprod, string dpt, double price)
        {
            Datex = date;
            Line = line;
            Type = type;
            Producibili = produc;
            Preventivati = prev;
            Qty = qtyToprod;
            Department = dpt;
            Price = price;
        }

        internal double _qtyToProd;

        public double QtyToProdIn
        {
            get
            {
                return _qtyToProd;
            }
            set
            {
                double.TryParse(Members.ToString(), out var m);
                double.TryParse(Abatim.ToString(), out var a);
                _qtyToProd = m * 7.5 * QtyH * (a / 100);
            }
        }

        internal double _maxEff;

        internal double _diffEff;

        public double MaxEff
        {
            get
            {
                return _maxEff;
            }
            set
            {
                _maxEff = Math.Round(100.0, 1);
            }
        }

        private double _workEff;

        public double WorkEff
        {
            get
            {
                return _workEff;
            }
            set
            {
                double.TryParse(Members.ToString(), out var m);
                double.TryParse(QtyProdEff.ToString(), out var q);
                double.TryParse(QtyH.ToString(), out var qh);
                var hour = 0.0;
                var hourW = 0.0;
                switch (Store.Default.sectorId) {
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
                _workEff = q / (m * qh * hour) * 100.0;
            }
        }

        public double DiffEff
        {
            get
            {
                return _diffEff;
            }
            set
            {
                _diffEff = Math.Round(WorkEff - MaxEff, 1);
            }
        }

        public DateTime Datex { get; set; }

        public string Line { get; set; }

        public string Type { get; set; }

        public int Members { get; set; }

        public double QtyH { get; set; }

        public int Abatim { get; set; }

        public int Qty { get; set; }

        public double QtyProdEff { get; set; }

        private double _dailyQty;

        public double DailyQty
        {
            get
            {
                return _dailyQty;
            }
            set
            {
                double.TryParse(Members.ToString(), out var m);
                _dailyQty = m * 7.5 * QtyH;
            }
        }
        
        public string Department { get; set; }

        public double Price { get; set; }
                
    }
}