namespace ganntproj1
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;

    /// <summary>
    /// Defines the <see cref="Mensile" />
    /// </summary>
    public partial class Mensile : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Mensile"/> class.
        /// </summary>
        public Mensile(string mode)
        {
            InitializeComponent();
            tableView1.DoubleBuffered(true);
            tableView1.DataBindingComplete += TableView1_DataBindingComp;
            tableView1.EnableHeadersVisualStyles = false;
            tableView1.RowTemplate.Height = 18;
            Mode = mode;
        }

        /// <summary>
        /// Defines the dc
        /// </summary>
        private readonly DataCollection dc = new DataCollection();

        /// <summary>
        /// Gets or sets the Mode
        /// </summary>
        public string Mode { get; set; }

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

        /// <summary>
        /// The OnLoad
        /// </summary>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        protected override void OnLoad(EventArgs e)
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

            cboYears.SelectedIndex = cboYears.FindString(DateTime.Now.Year.ToString());
            cboMonth.SelectedIndex = DateTime.Now.Month - 1;


            //LoadDataMensile();

            base.OnLoad(e);
        }

        /// <summary>
        /// The ListOfTypes
        /// </summary>
        /// <returns>The <see cref="List{string}"/></returns>
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

        /// <summary>
        /// The ListOfTypesEff
        /// </summary>
        /// <returns>The <see cref="List{string}"/></returns>
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

        /// <summary>
        /// The GetDataMensile
        /// </summary>
        /// <param name="month">The month<see cref="int"/></param>
        /// <param name="year">The year<see cref="int"/></param>
        /// <returns>The <see cref="List{DataCollection}"/></returns>
        public List<DataCollection> GetData(int month, int year)
        {
            var q = "create table tmpTable (datas date, line nvarchar(50), qtyH float,members int, abat float,startHour float,endHour float, capi int) ";
                    q += "insert into tmpTable ";
            q += "select convert(date,data,101),line,qtyH,members,(cast(abatim as float)/100), ";
            q += "case when datepart(hour,data) = 7 then 7 else datepart(hour,dateadd(minute,5,data)) end, "; //--startTime
            q += "case when datepart(hour,data) = 7 and datepart(hour,times) > 15 then 14.5 else ";
            q += "case when datepart(hour,data) > 7 and datepart(hour,times) < 15 then 14.5 else datepart(hour,dateadd(minute,1,times)) end end, "; //--endTime
            q += "capi from viewproduction ";
            q += "where DATEPART(MONTH, data) = '" + month + "' and DATEPART(YEAR, data)= '" + year + "' ";
            q += "order by len(line),line,convert(date, data, 101) ";
            q += "select datas,line,sum((qtyH * members * (endHour - startHour)))producibili, ";
            q += "sum((qtyH * members * (endHour - startHour) * abat))prevent, sum(capi)qty from tmpTable ";
            q += "group by datas,line order by len(line),line,datas ";
            q += "drop table tmpTable";

            //var q = "create table tmpTable (datas date, line nvarchar(50), qtyH float,members int, abat float,startHour int,endHour int, capi int) ";
            //q += "insert into tmpTable ";
            //q += "select convert(date,data,101),line,qtyH,members,(cast(abatim as float)/100),datepart(hour,data), ";
            //q += "case when datepart(hour,times) > 15 then 15 else datepart(hour,times) end, capi from viewproduction ";
            //q += "where DATEPART(MONTH, data) = '" + month + "' and DATEPART(YEAR, data)= '" + year + "' ";
            //q += "order by len(line),line,convert(date, data, 101) ";
            //q += "select datas,line,sum((qtyH * members * (endHour - startHour-0.5)))producibili, " +
            //    "sum((qtyH * members * (endHour - startHour-0.5) * abat))prevent, sum(capi)qty from tmpTable ";
            //q += "group by datas,line order by len(line),line,datas ";
            //q += "drop table tmpTable";

            var lst = new List<DataCollection>();

            if (Mode == "mens")
            {
                using (var c = new System.Data.SqlClient.SqlConnection(Central.SpecialConnStr))
                {
                    var cmd = new System.Data.SqlClient.SqlCommand(q, c);
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
                                lst.Add(new DataCollection(
                                    dt,
                                    dr[1].ToString(), type, produc, prevent, qty)
                                    );
                            }
                        }
                    c.Close();
                    dr.Close();
                }
            }
            else
            {
                using (var c = new System.Data.SqlClient.SqlConnection(Central.SpecialConnStr))
                {
                    var cmd = new System.Data.SqlClient.SqlCommand(q, c);
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
                                lst.Add(new DataCollection(
                                    dt,
                                    dr[1].ToString(), type, produc, prevent, qty)
                                    );
                            }
                        }
                    c.Close();
                    dr.Close();
                }
            }
            

            //Mode = mode;
            return lst;
        }

        ///// <summary>
        ///// The GetDataEff
        ///// </summary>
        ///// <param name="month">The month<see cref="int"/></param>
        ///// <param name="year">The year<see cref="int"/></param>
        ///// <returns>The <see cref="List{DataCollection}"/></returns>
        //public List<DataCollection> GetDataEff(int month, int year)
        //{

        //    //DATE , LINE, QTY, PRODUCIBILY
        //    var q = "";
        //     q += "select convert(date, data, 101),line,sum(cast(capi as int))qty, sum(members * qtyH * (15 - datepart(hour, data) - 0.5))qtyH from produzione " +
        //            "where DATEPART(MONTH, data) = '" + month + "' and datepart(year, data)= '" + year + "' " +
        //            "group by convert(date, data, 101),line " +
        //            " order by len(line),line, convert(date, data, 101)";

        //    /*use more query elements*/

        //    var lst = new List<DataCollection>();
        //    using (var c = new System.Data.SqlClient.SqlConnection(Central.SpecialConnStr))
        //    {
        //        var cmd = new System.Data.SqlClient.SqlCommand(q, c);
        //        c.Open();
        //        var dr = cmd.ExecuteReader();
        //        if (dr.HasRows)
        //            while (dr.Read())
        //            {
        //                foreach (var type in ListOfTypesEff())
        //                {
        //                    if (_cboType.SelectedIndex > 0 && _cboType.Text != type) continue;
        //                    DateTime.TryParse(dr[0].ToString(), out var dt);
        //                    double.TryParse(dr[2].ToString(), out var qty);
        //                    double.TryParse(dr[3].ToString(), out var qtyH);
        //                    //double.TryParse(dr[4].ToString(), out var qht);

        //                    lst.Add(new DataCollection(
        //                        dt,
        //                        dr[1].ToString(), type, qty, qtyH, dc.MaxEff, dc.WorkEff, dc.DiffEff)
        //                        );
        //                }

        //            }
        //        c.Close();
        //        dr.Close();
        //    }
        //    Mode = "eff";
        //    return lst;
        //}

        /// <summary>
        /// The LoadDataMensile
        /// </summary>
        public void LoadDataMensile()
        {
            tableView1.DataSource = null;
            var month = cboMonth.SelectedIndex + 1;
            int.TryParse(cboYears.Text, out var year);
            var dt = new DataTable();
            /*DATA*/
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
            //add row with weekday first character
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
            if (data.Count > 0)
            {
                lastLine = data.First().Line;
            }
            var htbl = new System.Collections.Hashtable();
            var idx = tpMax + 2;
            var tmpIdx = 0;
            foreach (var item in data)
            {
                var hKey = string.Empty;
                hKey = item.Line + item.Type;
                var dateIdx = item.Datex.ToString("yyyy-MM-dd");
               
                //double.TryParse(item.Members.ToString(), out var memb);
                //double.TryParse(item.QtyH.ToString(), out var qH);
                //double.TryParse(item.Abatim.ToString(), out var ab);
                //ab /= 100;
                var cProducibili = Math.Round(item.Producibili,0);
                var cPreventivati = Math.Round(item.Preventivati,0);

                if (htbl.Contains(hKey))
                {
                    var j = Convert.ToInt32(htbl[hKey]);

                    switch (item.Type)
                    {
                        case "CAPI PRODUCIBILI":
                            dt.Rows[j][dateIdx] = cProducibili.ToString();
                            int.TryParse(dt.Rows[1][dateIdx].ToString(), out var q);
                            dt.Rows[1][dateIdx] = (q + cProducibili).ToString();
                            break;
                        case "CAPI PREVENTIVATI":
                            dt.Rows[j][dateIdx] = cPreventivati.ToString();
                            double.TryParse(dt.Rows[2][dateIdx].ToString(), out var q1);
                            dt.Rows[2][dateIdx] = (q1 + cPreventivati).ToString();
                            break;
                        case "CAPI PRODOTI":
                            dt.Rows[j][dateIdx] = item.Qty.ToString();
                            int.TryParse(dt.Rows[3][dateIdx].ToString(), out var q2);
                            dt.Rows[3][dateIdx] = (q2 + item.Qty).ToString();
                            break;
                        case "DIFF PROD-PREVENT":
                            dt.Rows[j][dateIdx] = (item.Qty - cPreventivati).ToString();
                            double.TryParse(dt.Rows[4][dateIdx].ToString(), out var q3);
                            dt.Rows[4][dateIdx] = (q3 + (item.Qty - cPreventivati)).ToString();
                            break;
                        case "DIFF PROD-PRODUCI":
                            dt.Rows[j][dateIdx] = (item.Qty - cProducibili).ToString();
                            double.TryParse(dt.Rows[5][dateIdx].ToString(), out var q4);
                            dt.Rows[5][dateIdx] = (q4 + (item.Qty - cProducibili)).ToString();
                            break;
                    }
                }
                else
                {
                    if (lastLine != item.Line)
                    {
                        emptyRow = dt.NewRow();
                        dt.Rows.Add(emptyRow);
                        idx++;
                        //after first block of types
                        //increase tmpIdx for each indexed splitter
                        if (idx > 12) tmpIdx++;  
                    }
                    lastLine = item.Line;
                    var newRow = dt.NewRow();
                    //add line string in the first row when new line start
                    if (_cboType.SelectedIndex <= 0 &&
                        (idx - tmpIdx) % tpMax == 2) newRow[0] = item.Line; //(idx - tmpMaxIdx) remove indexed splitter  
                    else if (_cboType.SelectedIndex > 0) newRow[0] = item.Line;
                    newRow[1] = item.Type;
                    switch (item.Type)
                    {
                        case "CAPI PRODUCIBILI":
                            newRow[dateIdx] = cProducibili.ToString();
                            int.TryParse(dt.Rows[1][dateIdx].ToString(), out var q1);
                            dt.Rows[1][dateIdx] = (q1 + cProducibili).ToString();
                            break;
                        case "CAPI PREVENTIVATI":
                            newRow[dateIdx] = cPreventivati.ToString();
                            double.TryParse(dt.Rows[2][dateIdx].ToString(), out var q2);
                            dt.Rows[2][dateIdx] = (q2 + cPreventivati).ToString();
                            break;
                        case "CAPI PRODOTI":
                            newRow[dateIdx] = item.Qty.ToString();
                            int.TryParse(dt.Rows[3][dateIdx].ToString(), out var q3);
                            dt.Rows[3][dateIdx] = (q3 + item.Qty).ToString();
                            break;
                        case "DIFF PROD-PREVENT":
                            newRow[dateIdx] = (item.Qty - cPreventivati).ToString();
                            double.TryParse(dt.Rows[4][dateIdx].ToString(), out var q4);
                            dt.Rows[4][dateIdx] = (q4 + (item.Qty - cPreventivati)).ToString();
                            break;
                        case "DIFF PROD-PRODUCI":
                            newRow[dateIdx] = (item.Qty - cProducibili).ToString();
                            double.TryParse(dt.Rows[5][dateIdx].ToString(), out var q5);
                            dt.Rows[5][dateIdx] = (q5 + (item.Qty - cProducibili)).ToString();
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

        /// <summary>
        /// The LoadEff
        /// </summary>
        public void LoadEff()
        {
            tableView1.DataSource = null;
            var month = cboMonth.SelectedIndex + 1;
            int.TryParse(cboYears.Text, out var year);
            var dt = new DataTable();
            /*DATA*/
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
            //add row with weekday first character
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
            //var totRow = dt.NewRow();
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
                lastLine = data.First().Line;
            }
            var htbl = new System.Collections.Hashtable();
            var idx = tpMax + 2;
            var tmpIdx = 0;
            foreach (var item in data)
            {
                var maxEff = 100.0;
                var workEff = Math.Round((item.Qty / item.Producibili) * 100.0,1);
                if (double.IsNaN(workEff) || double.IsInfinity(workEff)) workEff = 0.0;
                var diffEff = Math.Round(workEff - maxEff, 1);

                var hKey = string.Empty;
                hKey = item.Line + item.Type;
                var dateIdx = item.Datex.ToString("yyyy-MM-dd");
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
                    if (lastLine != item.Line)
                    {
                        emptyRow = dt.NewRow();
                        dt.Rows.Add(emptyRow);
                        idx++;
                        if (idx > 7) tmpIdx++;
                    }
                    lastLine = item.Line;
                    var newRow = dt.NewRow();
                    //add line string in the first row when is new line
                    if (_cboType.SelectedIndex <= 0 &&
                        (idx - tmpIdx) % tpMax == 2) newRow[0] = item.Line;
                    else if (_cboType.SelectedIndex > 0) newRow[0] = item.Line;
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
            cbAbatim.Enabled = false;
            CreateFilter(ListOfTypesEff());
        }

        /// <summary>
        /// The CalculateTotals
        /// </summary>
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
                var tot = 0.0;
                for (var c = 2; c <= tableView1.Columns.Count - 2; c++)
                {

                    var cd = tableView1.Columns[c].Index;
                    if (cd != idx1 && cd != idx2 && cd != idx3 && cd != idx4) continue;
                    double.TryParse(tableView1.Rows[r].Cells[c].Value.ToString(), out var t);
                    tot += t;
                }
                tableView1.Rows[r].Cells["TOTAL"].Value = 
                    Math.Round(tot, 0).ToString();
            }
        }

        /// <summary>
        /// The CalculateTotalsEff
        /// </summary>
        private void CalculateTotalsEff()
        {
            for (var c = 2; c <= tableView1.Columns.Count - 1; c++)
            {
                var tot = 0;
                var x = 0;
                for (var r = 6; r <= tableView1.Rows.Count - 1; r++)
                {
                    if (tableView1.Rows[r].Cells[1].Value.ToString() != "EFFICIENZA CON") continue;
                    int.TryParse(tableView1.Rows[r].Cells[c].Value.ToString().Split('%')[0], out var t);
                    if (t == 0) continue;
                    tot += t;
                    x++;
                }
                double.TryParse(tot.ToString(), out var totDoub);
                double.TryParse(x.ToString(), out var xt);
                var res = Math.Round(totDoub / xt,1);
                string strRes;
                if (double.IsNaN(res) || double.IsInfinity(res)) strRes = "";
                else strRes = res.ToString() + "%";
                tableView1.Rows[2].Cells[c].Value = strRes;
            }
            for (var c = 2; c <= tableView1.Columns.Count - 1; c++)
            {
                var tot = 0.0;
                var x = 0;
                for (var r = 6; r <= tableView1.Rows.Count - 1; r++)
                {
                    if (tableView1.Rows[r].Cells[1].Value.ToString() != "DIFFERENZA") continue;
                    double.TryParse(tableView1.Rows[r].Cells[c].Value.ToString().Split('%')[0], out var t);
                    if (t == 0) continue;
                    tot += t;
                    x++;
                }
                var res = Math.Round(tot / x, 2);
                string strRes;
                if (double.IsNaN(res) || double.IsInfinity(res)) strRes = "";
                else strRes = res.ToString() + "%";
                tableView1.Rows[3].Cells[c].Value = strRes;
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

            //caluclate horizontal total value
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

        /// <summary>
        /// The CbAcconto_CheckedChanged
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void CbAcconto_CheckedChanged(object sender, EventArgs e)
        {
            LoadDataMensile();
        }

        /// <summary>
        /// The TableView1_DataBindingComp
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="DataGridViewBindingCompleteEventArgs"/></param>
        private void TableView1_DataBindingComp(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (tableView1.Rows.Count <= 0) return;
            tableView1.Rows[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tableView1.Rows[0].DefaultCellStyle.BackColor = Color.White;
            tableView1.Rows[0].DefaultCellStyle.SelectionBackColor = Color.White;
            tableView1.Rows[0].DefaultCellStyle.SelectionForeColor = Color.Black;
            tableView1.Rows[0].Frozen = true;
            tableView1.Columns[0].DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 10, FontStyle.Bold);

            tableView1.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            tableView1.ColumnHeadersHeight = 50;

            foreach (DataGridViewRow row in tableView1.Rows)
            {
                if (row.Cells[1].Value.ToString() == "CAPI PRODOTI")
                {
                    row.DefaultCellStyle.BackColor = Color.FromArgb(138, 184, 44);
                }

                if (Mode == "mens" && row.Index > 0 && row.Index <= 5)
                {
                    //row.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    row.DefaultCellStyle.BackColor = Color.WhiteSmoke;
                    row.DefaultCellStyle.ForeColor = Color.Black;
                    row.DefaultCellStyle.SelectionBackColor = Color.WhiteSmoke;
                    row.DefaultCellStyle.SelectionForeColor = Color.Black;
                    row.Frozen = true;
                }
            }

            foreach (DataGridViewColumn c in tableView1.Columns)
            {
                if (c.Name.Contains("SETT"))
                {
                    c.DefaultCellStyle.BackColor = Color.LightSteelBlue;
                    c.HeaderCell.Style.ForeColor = Color.Red;
                    c.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 7, FontStyle.Regular);

                }
                c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                DateTime.TryParse(c.Name, out var dx);
                if (dx == DateTime.MinValue) continue;
                c.HeaderText = dx.ToString("dd-MMM",
                    System.Globalization.CultureInfo.InvariantCulture);
                if (dx.DayOfWeek == DayOfWeek.Saturday || dx.DayOfWeek == DayOfWeek.Sunday)
                {
                    c.HeaderCell.Style.BackColor = Color.Yellow;
                    c.DefaultCellStyle.BackColor = Color.Yellow;
                }
            }
            tableView1.Columns[0].Frozen = true;
            tableView1.Columns[1].Frozen = true;
            tableView1.RowTemplate.Height = 18;
        }

        /// <summary>
        /// The TableView1_CellPainting
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="DataGridViewCellPaintingEventArgs"/></param>
        private void TableView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (Mode == "mens" && e.RowIndex > 5 || e.RowIndex > 1)
            {
                if (tableView1.Rows[e.RowIndex].Cells[0].Value.ToString() == string.Empty &&
                 tableView1.Rows[e.RowIndex].Cells[1].Value.ToString() == string.Empty)
                {
                    e.Graphics.FillRectangle(Brushes.White, e.CellBounds.X, e.CellBounds.Y, e.CellBounds.Width, e.CellBounds.Height);
                    e.Handled = true;
                }
            }
            if (e.RowIndex > 0 && e.ColumnIndex > 1 && e.ColumnIndex <= tableView1.ColumnCount - 1)
            {
                if (tableView1.Columns[e.ColumnIndex].HeaderText.Contains("^ SETT"))
                {
                    if (tableView1.Rows[e.RowIndex].Cells[0].Value.ToString() != string.Empty ||
                 tableView1.Rows[e.RowIndex].Cells[1].Value.ToString() != string.Empty)
                    {
                        e.Graphics.FillRectangle(Brushes.LightSteelBlue,
                            e.CellBounds.X, e.CellBounds.Y, e.CellBounds.Width, e.CellBounds.Height);
                        e.Graphics.DrawRectangle(Pens.Black,
                            e.CellBounds.X, e.CellBounds.Y, e.CellBounds.Width - 1, e.CellBounds.Height - 1);
                        e.Graphics.DrawString(e.Value.ToString(), new Font("Microsoft Sans Serif", 8),
                            Brushes.Black, e.CellBounds.X + 8, e.CellBounds.Y + (e.CellBounds.Height / 2 - 7));
                        e.Handled = true;
                    }
                }
            }
        }

        /// <summary>
        /// The ExportToExcel
        /// </summary>
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

        /// <summary>
        /// The PrintGrid
        /// </summary>
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

            var dGvPrinter = new DGVPrinter
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

        /// <summary>
        /// The TableView1_SelectionChanged
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void TableView1_SelectionChanged(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Defines the _filterCreated
        /// </summary>
        private bool _filterCreated = false;

        /// <summary>
        /// Defines the _cboType
        /// </summary>
        private ComboBox _cboType = new ComboBox();

        /// <summary>
        /// The CreateFilter
        /// </summary>
        /// <param name="lst">The lst<see cref="List{string}"/></param>
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
    }

    /// <summary>
    /// Defines the <see cref="DataCollection" />
    /// </summary>
    public class DataCollection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataCollection"/> class.
        /// </summary>
        public DataCollection()
        {
        }

        public double Producibili { get; set; }
        public double Preventivati { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataCollection"/> class.
        /// </summary>
        /// <param name="date">The date<see cref="DateTime"/></param>
        /// <param name="line">The line<see cref="string"/></param>
        /// <param name="type">The type<see cref="string"/></param>
        /// <param name="memb">The memb<see cref="int"/></param>
        /// <param name="qh">The qh<see cref="double"/></param>
        /// <param name="abatim">The abatim<see cref="int"/></param>
        /// <param name="qtyToprod">The qtyToprod<see cref="int"/></param>
        /// <param name="dQty">The dQty<see cref="int"/></param>
        /// <param name="qtyProdIn">The qtyProdIn<see cref="double"/></param>
        public DataCollection(DateTime date, string line, string type,
            double produc, double prev, int qtyToprod)
        {
            Datex = date;
            Line = line;
            Type = type;
            Producibili = produc;
            Preventivati = prev;
            Qty = qtyToprod;
        }

        ///// <summary>
        ///// Initializes a new instance of the <see cref="DataCollection"/> class.
        ///// </summary>
        ///// <param name="date">The date<see cref="DateTime"/></param>
        ///// <param name="line">The line<see cref="string"/></param>
        ///// <param name="type">The type<see cref="string"/></param>
        ///// <param name="qty">The qty<see cref="double"/></param>
        ///// <param name="memb">The memb<see cref="int"/></param>
        ///// <param name="qh">The qh<see cref="double"/></param>
        ///// <param name="maxEff">The maxEff<see cref="double"/></param>
        ///// <param name="workeff">The workeff<see cref="double"/></param>
        ///// <param name="diffEff">The diffEff<see cref="double"/></param>
        //public DataCollection(DateTime date, string line, string type, double qty, double qtyH, double maxEff, double workeff, double diffEff)
        //{
        //    Datex = date;
        //    Line = line;
        //    Type = type;
        //    QtyH = qtyH;
        //    MaxEff = maxEff;
        //    WorkEff = workeff;
        //    DiffEff = diffEff;
        //    QtyProdEff = qty;
        //}

        /// <summary>
        /// Defines the _qtyToProd
        /// </summary>
        internal double _qtyToProd;

        /// <summary>
        /// Gets or sets the QtyToProdIn
        /// </summary>
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

        /// <summary>
        /// Defines the _maxEff
        /// </summary>
        internal double _maxEff;

        /// <summary>
        /// Defines the _diffEff
        /// </summary>
        internal double _diffEff;

        /// <summary>
        /// Gets or sets the MaxEff
        /// </summary>
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

        /// <summary>
        /// Defines the _workEff
        /// </summary>
        private double _workEff;

        /// <summary>
        /// Gets or sets the WorkEff
        /// </summary>
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
                _workEff = q / (m * qh * 7.5) * 100.0;
            }
        }

        /// <summary>
        /// Gets or sets the DiffEff
        /// </summary>
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

        /// <summary>
        /// Gets or sets the Datex
        /// </summary>
        public DateTime Datex { get; set; }

        /// <summary>
        /// Gets or sets the Line
        /// </summary>
        public string Line { get; set; }

        /// <summary>
        /// Gets or sets the Type
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the Members
        /// </summary>
        public int Members { get; set; }

        /// <summary>
        /// Gets or sets the QtyH
        /// </summary>
        public double QtyH { get; set; }

        /// <summary>
        /// Gets or sets the Abatim
        /// </summary>
        public int Abatim { get; set; }

        /// <summary>
        /// Gets or sets the QtyToProd
        /// </summary>
        public int Qty { get; set; }

        /// <summary>
        /// Gets or sets the QtyProdEff
        /// </summary>
        public double QtyProdEff { get; set; }

        /// <summary>
        /// Defines the _dailyQty
        /// </summary>
        private double _dailyQty;

        /// <summary>
        /// Gets or sets the DailyQty
        /// </summary>
        public double DailyQty
        {
            get
            {
                return _dailyQty;
            }
            set
            {
                double.TryParse(Members.ToString(), out var m);
                //double.TryParse(Abatim.ToString(), out var a);
                _dailyQty = m * 7.5 * QtyH;
            }
        }
    }
}
