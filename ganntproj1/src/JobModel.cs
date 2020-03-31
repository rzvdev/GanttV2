namespace ganntproj1
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Defines the <see cref="JobModel" />
    /// </summary>
    public class JobModel
    {
        /// <summary>
        /// Gets or sets the DateFromLast
        /// </summary>
        public static DateTime DateFromLast { get; set; }

        /// <summary>
        /// Gets or sets the DateToLast
        /// </summary>
        public static DateTime DateToLast { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="JobModel"/> class.
        /// </summary>
        public JobModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JobModel"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="aim">The aim.</param>
        /// <param name="article">The article.</param>
        /// <param name="stateId">The state identifier.</param>
        /// <param name="loadedQty">The loaded qty.</param>
        /// <param name="qtyH">The qty h.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="dvc">The DVC.</param>
        /// <param name="rdd">The RDD.</param>
        /// <param name="prodStart">The product start.</param>
        /// <param name="prodEnd">The product end.</param>
        /// <param name="dailyProd">The daily product.</param>
        /// <param name="prodQty">The product qty.</param>
        /// <param name="overQty">The over qty.</param>
        /// <param name="prodOverDays">The product over days.</param>
        /// <param name="delayTs">The delayTs<see cref="TimeSpan"/></param>
        /// <param name="prodOverTs">The prodOverTs<see cref="TimeSpan"/></param>
        /// <param name="locked">The locked<see cref="bool"/></param>
        /// <param name="holiday">The holiday<see cref="int"/></param>
        /// <param name="closedord">The closedord<see cref="bool"/></param>
        /// <param name="artPrice">The artPrice<see cref="double"/></param>
        /// <param name="hasProd">The hasProd<see cref="bool"/></param>
        /// <param name="lockedProd">The lockedProd<see cref="bool"/></param>
        /// <param name="delayStart">The delayStart<see cref="DateTime"/></param>
        /// <param name="delayEnd">The delayEnd<see cref="DateTime"/></param>
        /// <param name="prodDone">The prodDone<see cref="bool"/></param>
        public JobModel(string name, string aim, string article, int stateId, int loadedQty, double qtyH, DateTime startDate, int duration,
            DateTime endDate, DateTime dvc, DateTime rdd, DateTime prodStart, DateTime prodEnd, int dailyProd,
            int prodQty, int overQty, int prodOverDays, long delayTs, long prodOverTs,
            bool locked, int holiday, bool closedord, double artPrice, bool hasProd, bool lockedProd,
            DateTime delayStart, DateTime delayEnd, bool prodDone, bool isbase, double newQh, double newPrice, string dept)
        {
            Name = name;
            Aim = aim;
            Article = article;
            StateId = stateId;
            LoadedQty = loadedQty;
            QtyH = qtyH;
            StartDate = startDate;
            Duration = duration;
            EndDate = endDate;
            Dvc = dvc;
            Rdd = rdd;
            ProductionStartDate = prodStart;
            ProductionEndDate = prodEnd;
            DailyProd = dailyProd;
            //Index = index;
            ProdQty = prodQty;
            OverQty = overQty;
            ProdOverDays = prodOverDays;
            DelayTime = delayTs;
            ProdOverTime = prodOverTs;
            Locked = locked;
            HolidayRange = holiday;
            ClosedByUser = closedord;
            ArtPrice = artPrice;
            HasProduction = hasProd;
            IsLockedProduction = lockedProd;
            DelayStartDate = delayStart;
            DelayEndDate = delayEnd;
            ProductionDone = prodDone;
            IsBase = isbase;
            NewQtyh = newQh;
            NewPrice = newPrice;
            Department = dept;
        }

        /// <summary>
        /// Gets or sets the Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Aim
        /// </summary>
        public string Aim { get; set; }

        /// <summary>
        /// Gets or sets the Article
        /// </summary>
        public string Article { get; set; }

        /// <summary>
        /// Gets or sets the StateId
        /// </summary>
        public int StateId { get; set; }

        /// <summary>
        /// Gets or sets the LoadedQty
        /// </summary>
        public int LoadedQty { get; set; }

        /// <summary>
        /// Gets or sets the QtyH
        /// </summary>
        public double QtyH { get; set; }

        /// <summary>
        /// Gets or sets the StartDate
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the Duration
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// Gets or sets the EndDate
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets the Dvc
        /// </summary>
        public DateTime Dvc { get; set; }

        /// <summary>
        /// Gets or sets the Rdd
        /// </summary>
        public DateTime Rdd { get; set; }

        /// <summary>
        /// Gets or sets the ProductionStartDate
        /// </summary>
        public DateTime ProductionStartDate { get; set; }

        /// <summary>
        /// Gets or sets the ProductionEndDate
        /// </summary>
        public DateTime ProductionEndDate { get; set; }

        /// <summary>
        /// Gets or sets the DailyProd
        /// </summary>
        public int DailyProd { get; set; }

        /// <summary>
        /// Gets or sets the Index
        /// </summary>
        //public int Index { get; set; }

        /// <summary>
        /// Gets or sets the ProdQty
        /// </summary>
        public int ProdQty { get; set; }

        /// <summary>
        /// Gets or sets the OverQty
        /// </summary>
        public int OverQty { get; set; }

        /// <summary>
        /// Gets or sets the ProdOverDays
        /// </summary>
        public int ProdOverDays { get; set; }

        /// <summary>
        /// Gets or sets the DelayTime
        /// </summary>
        public long DelayTime { get; set; }

        /// <summary>
        /// Gets or sets the ProdOverTime
        /// </summary>
        public long ProdOverTime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Locked
        /// </summary>
        public bool Locked { get; set; }

        /// <summary>
        /// Gets or sets the HolidayRange
        /// </summary>
        public int HolidayRange { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether ClosedByUser
        /// </summary>
        public bool ClosedByUser { get; set; }

        /// <summary>
        /// Gets or sets the ArtPrice
        /// </summary>
        public double ArtPrice { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether HasProduction
        /// </summary>
        public bool HasProduction { get; set; }

        /// <summary>
        /// Gets or sets the DelayStartDate
        /// </summary>
        public DateTime DelayStartDate { get; set; }

        /// <summary>
        /// Gets or sets the DelayEndDate
        /// </summary>
        public DateTime DelayEndDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether IsLockedProduction
        /// </summary>
        public bool IsLockedProduction { get; set; }

        /// <summary>
        /// Gets or sets the LockDate
        /// </summary>
        public DateTime LockDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether ProductionDone
        /// </summary>
        public bool ProductionDone { get; set; }

        public bool IsBase { get; set; }

        public double NewQtyh { get; set; }

        public double NewPrice { get; set; }

        public string Department { get; set; }

        /// <summary>
        /// The GetDaysInRange
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public IEnumerable<DateTime> GetDaysInRange(DateTime from,
                                                    DateTime to)
        {
            for (var day = from.Date; day.Date <= to.Date; day = day.AddDays(+1))
                yield return day;
        }

        /// <summary>
        /// The GetDaysInRangeOf
        /// </summary>
        /// <param name="from">The from<see cref="DateTime"/></param>
        /// <param name="to">The to<see cref="DateTime"/></param>
        /// <returns>The <see cref="IEnumerable{DateTime}"/></returns>
        public static IEnumerable<DateTime> GetDaysInRangeOf(DateTime from,
                                                    DateTime to)
        {
            for (var day = from.Date; day.Date <= to.Date; day = day.AddDays(+1))
                yield return day;
        }

        /// <summary>
        /// The SkipDateRange
        /// </summary>
        /// <param name="dateFrom">The dateFrom<see cref="DateTime"/></param>
        /// <param name="dateTo">The dateTo<see cref="DateTime"/></param>
        /// <returns>The <see cref="int"/></returns>
        public static int SkipDateRange(DateTime dateFrom,
                                    DateTime dateTo)
        {
            var lstOfHld = (from hld in Central.ListOfHolidays
                            select hld).ToList();
            var hldList = new List<DateTime>();
            foreach (var h in lstOfHld)
            {
                var dt = new DateTime(h.Year, h.Month, h.Holiday.Day, 0, 0, 0);
                hldList.Add(dt);
            }
            var i = 0;
            for (var day = dateFrom.Date; day.Date <= dateTo.Date; day = day.AddDays(+1))
            {
                var dd = new DateTime(day.Year, day.Month, day.Day, 0, 0, 0);
                if (hldList.Contains(dd))
                {
                    i += 1;
                    dateTo = dateTo.AddDays(+1);
                }
                else
                {
                    if (dd.DayOfWeek == DayOfWeek.Saturday || dd.DayOfWeek == DayOfWeek.Sunday)
                    {
                        i += 1;
                        dateTo = dateTo.AddDays(+1);
                    }
                }
            }
            return i;
        }

        /// <summary>
        /// The SkipDateRange
        /// </summary>
        /// <param name="dateFrom">The dateFrom<see cref="DateTime"/></param>
        /// <param name="dateTo">The dateTo<see cref="DateTime"/></param>
        /// <param name="aim">The aim<see cref="string"/></param>
        /// <returns>The <see cref="int"/></returns>
        public static int SkipDateRange(DateTime dateFrom,
                                    DateTime dateTo, string aim)
        {
            var lstOfHld = (from hld in Central.ListOfHolidays
                            where hld.Line == aim
                            select hld).ToList();
            var hldList = new List<DateTime>();
            foreach (var h in lstOfHld)
            {
                var dt = new DateTime(h.Year, h.Month, h.Holiday.Day, 0, 0, 0);
                hldList.Add(dt);
            }
            var i = 0;
            for (var day = dateFrom.Date; day.Date <= dateTo.Date; day = day.AddDays(+1))
            {
                var dd = new DateTime(day.Year, day.Month, day.Day, 0, 0, 0);
                if (hldList.Contains(dd))
                {
                    i += 1;
                    dateTo = dateTo.AddDays(+1);
                }
                else
                {
                    if (dd.DayOfWeek == DayOfWeek.Saturday || dd.DayOfWeek == DayOfWeek.Sunday)
                    {
                        i += 1;
                        dateTo = dateTo.AddDays(+1);
                    }
                }
            }
            return i;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long GetLSpan(DateTime dateTime)
        {
            return dateTime.Subtract(Config.MinimalDate).Ticks;
        }
        /// <summary>
        /// The CalculateDailyQty
        /// </summary>
        /// <param name="aim"></param>
        /// <param name="qtyH"></param>
        /// <returns></returns>
        public double CalculateDailyQty(string aim, double qtyH, string department)
        {
            var linesQuery = from lines in Models.Tables.Lines
                             where lines.Line == aim && lines.Department == department
                             select lines;
            var lineMembers = linesQuery.Select(x => x.Members).SingleOrDefault();
            var lineAbatimento = linesQuery.Select(x => x.Abatimento).SingleOrDefault();
            if (lineMembers == 0) lineMembers = 1;
            var abatimento = 0.0;
            if (lineAbatimento > 0.0)
                abatimento = Math.Round(Convert.ToDouble(lineAbatimento) / 100, 2);

            var h = 0.0;
            if (Store.Default.sectorId == 1) h = Store.Default.confHour; else h = Store.Default.stiroHour;

            return Math.Round(lineMembers * qtyH * h * abatimento, 0);
        }

        /// <summary>
        /// The CalculateJobDuration
        /// </summary>
        /// <param name="aim"></param>
        /// <param name="qty"></param>
        /// <param name="qtyH"></param>
        /// <returns></returns>
        public int CalculateJobDuration(string aim,
                                     int qty,
                                     double qtyH, string department)
        {
            var linesQuery = from lines in Models.Tables.Lines
                             where lines.Line == aim && lines.Department == department
                             select lines;
            var lineMembers = linesQuery.Select(x => x.Members).SingleOrDefault();
            var lineAbatimento = linesQuery.Select(x => x.Abatimento).SingleOrDefault();
            if (lineMembers == 0) lineMembers = 1;
            var abatimento = 0.0;
            if (lineAbatimento > 0.0)
                abatimento = Math.Round(Convert.ToDouble(lineAbatimento) / 100, 2);
            if (qty == 0) return 0;

            var h = 0.0;
            if (Store.Default.sectorId == 1) h = Store.Default.confHour; else h = Store.Default.stiroHour;

            var duration = Math.Round
                (Convert.ToDouble(qty / (lineMembers * qtyH * h * abatimento)), 0);
            if (duration <= 0)
            {
                duration = 1;
            }
            int.TryParse(Math.Round(duration, 0).ToString(), out var d);
            return d;
        }
        /// <summary>
        /// The GetJobContinum
        /// </summary>
        /// <param name="job"></param>
        /// <param name="aim"></param>
        /// <returns></returns>
        public object[] GetJobContinum(string job,
            string aim, string dept)
        {
            object[] obj = new object[] { };
            var production = from prod in Models.Tables.Productions
                             where prod.Commessa == job
                             && prod.Line == aim && prod.Department == dept
                             orderby prod.Data
                             select prod;
            //check production insertments to get start-end date
            var prodStart = production.ToList().Count > 0 ?
                production.ToList().First().Data : new DateTime();
            var prodEnd = production.ToList().Count > 0 ?
                production.ToList().Last().Times : new DateTime();
            if (production.ToList().Count == 0)
            {
                return new object[] {prodStart, prodEnd,
                0,0,0, new TimeSpan(0,0,0,0,0), new TimeSpan(0,0,0,0,0)};   // neutral array
            }
            var prodOverDays = 0.0;
            var lastDate = DateTime.MinValue;
            var startShift = Central.ShiftFrom.Hours;       //get user inserted shift (start-time)
            var endShift = Central.ShiftTo.Hours;
            var shiftRange = Central.ShiftTo.Subtract(Central.ShiftTo).TotalHours;  // get shift duration
            /*
             * decalre production dynamic parameters
             */
            var dymDailyQty = 0;    //collects all qty values that must be produced
            var dymQty = 0.0;   //collects all qty-values that are produced
            var dymOverQty = 0.0;
            var dymAlertTime = new TimeSpan(0, 0, 0, 0, 0);
            var dymOverTime = new TimeSpan(0, 0, 0, 0, 0);
            foreach (var prod in production)
            {
                if (lastDate != prod.Data.Date)
                {
                    int.TryParse(prod.Dailyqty.ToString(), out var q);
                    dymDailyQty += q;
                }
                lastDate = prod.Data.Date;
                dymQty += prod.Capi;
            }
            var restQty = 0.0;
            restQty = (dymDailyQty - dymQty);
            if (restQty <= 0)
                dymAlertTime = new TimeSpan(0, 0, 0, 0, 0);
            else
            //get containing hours and days in shift-range
            {
                var r = Convert.ToInt32(restQty / 60); //get hours
                var dd = r / 8; //get days
                var hh = r % 8; //get hours containing in rest                
                if (Math.Floor(Convert.ToDecimal(dd)) == 0) dd = 0;
                dymAlertTime = new TimeSpan(dd, startShift + hh, 0, 0, 0);
                if (dd == 0 && hh == 0)
                    dymAlertTime = new TimeSpan(0, 0, 0, 0, 0);
            }
            obj = new object[]
            {
                prodStart, prodEnd,
                dymQty,prodOverDays,
                dymOverQty,
                dymOverTime,dymAlertTime
            };
            DateTime.TryParse(prodEnd.ToString(), out var endDate);           
            using (var context = new System.Data.Linq.DataContext(Central.SpecialConnStr))
            {
                // delete existing records
                context.ExecuteCommand("update objects set " +
                    "startprod={0},endprod={1},prodqty={2},delayts={3}, delaystart={4},delayend={5} " +
                    "where ordername={6} and aim={7} and department={8}", 
                    GetLSpan(prodStart),
                    GetLSpan(endDate), 
                    dymQty,
                    dymAlertTime.Ticks,
                    GetLSpan(endDate),
                    GetLSpan(endDate) + dymAlertTime.Ticks,
                    job,aim, dept);
            }
            return obj;
        }
        /// <summary>
        /// The GetModelIndex
        /// </summary>
        /// <param name="model"></param>
        /// <param name="lst"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static JobModel GetModelIndex(string model, List<Index> lst, int index)
        {
            JobModel jMod = default;
            foreach (var item in lst)
            {
                if (item.ObjText != model) continue;
                var jIndex = lst
                    .SingleOrDefault(x => x.RowIndex == item.RowIndex &&
                    x.ObjIndex == item.ObjIndex + index);
                if (jIndex != null)
                    jMod = Central.ListOfModels
                        .SingleOrDefault(x => x.Name == jIndex.ObjText &&
                        x.Aim == jIndex.ObjAim && x.Department == jIndex.ObjDept);
                else
                    jMod = null;
            }
            return jMod;
        }
        /// <summary>
        /// The GetIndexAfterLock
        /// </summary>
        /// <param name="model">The model<see cref="string"/></param>
        /// <param name="lst">The lst<see cref="List{Index}"/></param>
        /// <param name="rIndex">The rIndex<see cref="int"/></param>
        /// <param name="oIndex">The oIndex<see cref="int"/></param>
        /// <returns>The <see cref="JobModel"/></returns>
        public static JobModel GetIndexAfterLock(string model, List<Index> lst, int rIndex, int oIndex)
        {
            JobModel jMod = default;
            foreach (var item in lst)
            {
                if (item.ObjText != model) continue;
                var jIndex = lst
                    .SingleOrDefault(x => x.RowIndex == item.RowIndex + rIndex &&
                    x.ObjIndex == oIndex);
                if (jIndex != null)
                    jMod = Central.ListOfModels
                        .SingleOrDefault(x => x.Name == jIndex.ObjText &&
                        x.Aim == jIndex.ObjAim && x.Department == jIndex.ObjDept);
                else
                    jMod = null;
            }
            return jMod;
        }

        /// <summary>
        /// The GetLineLastDate
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static DateTime GetLineLastDate(string line, string dept)
        {
            var lng = 0L;
            var q = "select max(enddate) from objects where aim='" + line + "' and department='" + dept + "'";
            using (var c = new System.Data.SqlClient.SqlConnection(Central.SpecialConnStr))
            {
                var cmd = new System.Data.SqlClient.SqlCommand(q, c);
                c.Open();
                var dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        long.TryParse(dr[0].ToString(), out lng);
                    }
                }
                c.Close();
            }

            var date = Config.MinimalDate.AddTicks(lng);
            return date;
        }

        /// <summary>
        /// The CalculateDailyQty
        /// </summary>
        /// <param name="aim"></param>
        /// <param name="qtyH"></param>
        /// <returns></returns>
        public double GetPrice(string article)
        {
            var artQuery = from art in Models.Tables.Articles
                           where art.Articol == article && art.Idsector == 1
                           select art;

            if (artQuery != null)
            {
                var price = artQuery.Select(x => x.Prezzo).SingleOrDefault();
                double.TryParse(price.ToString(), out var p);
                return p;
            }
            else
            {
                return 0.0;
            }
        }       
    }

    /// <summary>
    /// Defines the <see cref="Index" />
    /// </summary>
    public class Index
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Index"/> class.
        /// </summary>
        public Index()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Index"/> class.
        /// </summary>
        /// <param name="rowIndex">Index of the row.</param>
        /// <param name="objIndex">Index of the object.</param>
        /// <param name="objText">The object text.</param>
        /// <param name="objAim">The object aim.</param>
        public Index(int rowIndex, int objIndex, string objText, string objAim, string objDept)
        {
            RowIndex = rowIndex;
            ObjIndex = objIndex;
            ObjText = objText;
            ObjAim = objAim;
            ObjDept = objDept;
        }

        /// <summary>
        /// Gets or sets the RowIndex
        /// </summary>
        public int RowIndex { get; set; }

        /// <summary>
        /// Gets or sets the ObjIndex
        /// </summary>
        public int ObjIndex { get; set; }

        /// <summary>
        /// Gets or sets the ObjText
        /// </summary>
        public string ObjText { get; set; }

        /// <summary>
        /// Gets or sets the ObjAim
        /// </summary>
        public string ObjAim { get; set; }

        public string ObjDept { get; set; }
    }
    /// <summary>
    /// Defines the <see cref="LineHolidaysEmbeded" />
    /// </summary>
    public class LineHolidaysEmbeded
    {
        /// <summary>
        /// Gets or sets the Line
        /// </summary>
        public string Line { get; set; }

        /// <summary>
        /// Gets or sets the HolidayArray
        /// </summary>
        public string HolidayArray { get; set; }

        /// <summary>
        /// Gets or sets the Holiday
        /// </summary>
        public DateTime Holiday { get; set; }

        /// <summary>
        /// Gets or sets the Month
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        /// Gets or sets the Year
        /// </summary>
        public int Year { get; set; }

        public string Department { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineHolidaysEmbeded"/> class.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="hld">Gets datetime from an array</param>
        /// <param name="m"></param>
        /// <param name="y"></param>
        public LineHolidaysEmbeded(string line, DateTime hld, int m, int y)
        {
            Line = line;
            Holiday = hld;
            Month = m;
            Year = y;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineHolidaysEmbeded"/> class.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="hld">Gets an array of dates</param>
        /// <param name="m"></param>
        /// <param name="y"></param>
        public LineHolidaysEmbeded(string line, string hld, int m, int y, string dept)
        {
            Line = line;
            HolidayArray = hld;
            Month = m;
            Year = y;
            Department = dept;
        }

        public LineHolidaysEmbeded(string line, DateTime hld, int m, int y, string dept)
        {
            Line = line;
            Holiday = hld;
            Month = m;
            Year = y;
            Department = dept;
        }
    }
}
