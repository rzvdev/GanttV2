namespace ganntproj1
{
    using Microsoft.Office.Interop.Excel;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class JobModel
    {
        public static DateTime DateFromLast { get; set; }

        public static DateTime DateToLast { get; set; }

        public JobModel()
        {
        }

        public JobModel(string name, string aim, string article, int stateId, int loadedQty, double qtyH, DateTime startDate, double duration,
            DateTime endDate, DateTime dvc, DateTime rdd, DateTime prodStart, DateTime prodEnd, int dailyProd,
            int prodQty, int overQty, int prodOverDays, long delayTs, long prodOverTs,
            bool locked, int holiday, bool closedord, double artPrice, bool hasProd, bool lockedProd,
            DateTime delayStart, DateTime delayEnd, bool prodDone, bool isbase, double newQh, double newPrice, string dept, 
            int workingdays, int members, bool manualDate, int abatimen, bool launched)
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
            WorkingDays = workingdays;
            Members = members;
            ManualDate = manualDate;
            Abatimen = abatimen;
            Launched = launched;
        }

        public string Name { get; set; }

        public string Aim { get; set; }

        public string Article { get; set; }

        public int StateId { get; set; }

        public int LoadedQty { get; set; }

        public double QtyH { get; set; }

        public DateTime StartDate { get; set; }

        public double Duration { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime Dvc { get; set; }

        public DateTime Rdd { get; set; }

        public DateTime ProductionStartDate { get; set; }

        public DateTime ProductionEndDate { get; set; }

        public int DailyProd { get; set; }

        public int ProdQty { get; set; }

        public int OverQty { get; set; }

        public int ProdOverDays { get; set; }

        public long DelayTime { get; set; }

        public long ProdOverTime { get; set; }

        public bool Locked { get; set; }

        public int HolidayRange { get; set; }

        public bool ClosedByUser { get; set; }

        public double ArtPrice { get; set; }

        public bool HasProduction { get; set; }

        public DateTime DelayStartDate { get; set; }

        public DateTime DelayEndDate { get; set; }

        public bool IsLockedProduction { get; set; }

        public DateTime LockDate { get; set; }

        public bool ProductionDone { get; set; }

        public bool IsBase { get; set; }

        public double NewQtyh { get; set; }

        public double NewPrice { get; set; }

        public string Department { get; set; }

        public int WorkingDays { get; set; }

        public int Members { get; set; }

        public bool ManualDate { get; set; }

        public int Abatimen { get; set; }

        public bool Launched { get; set; }

        public IEnumerable<DateTime> GetDaysInRange(DateTime from,
                                                    DateTime to)
        {
            for (var day = from.Date; day.Date <= to.Date; day = day.AddDays(+1))
                yield return day;
        }

        public static IEnumerable<DateTime> GetDaysInRangeOf(DateTime from,
                                                    DateTime to)
        {
            for (var day = from.Date; day.Date <= to.Date; day = day.AddDays(+1))
                yield return day;
        }

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

        public static int SkipDateRange(DateTime dateFrom,
                                    DateTime dateTo, string aim, string department)
        {
            var lstOfHld = (from hld in Central.ListOfHolidays
                            where hld.Line == aim && hld.Department == department
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

        public static long GetLSpan(DateTime dateTime)
        {
            return dateTime.Subtract(Config.MinimalDate).Ticks;
        }

        public int CalculateDailyQty(string aim, double qtyH, string department, int members = 0, int qty = 0)
        {
            var linesQuery = from lines in Models.Tables.Lines
                             where lines.Line == aim && lines.Department == department
                             select lines;
            var lineMembers = linesQuery.Select(x => x.Members).SingleOrDefault();
            if (lineMembers == 0) lineMembers = 1;

            if (members != lineMembers && members > 0) lineMembers = members;

            var lineAbatimento = linesQuery.Select(x => x.Abatimento).SingleOrDefault();
            var abatimento = 0.0;
            if (lineAbatimento > 0.0)
                abatimento = Math.Round(Convert.ToDouble(lineAbatimento) / 100, 2);

            var h = 0.0;
            if (Store.Default.sectorId == 1) h = Store.Default.confHour;
            else if (Store.Default.sectorId == 2) h = Store.Default.stiroHour;
            else if (Store.Default.sectorId == 7) h = Store.Default.tessHour;
            else if (Store.Default.sectorId == 8) h = Store.Default.sartHour;

            var dailyQty = 0;

            if (Store.Default.sectorId != 7)
            {
                dailyQty = Convert.ToInt32(lineMembers * qtyH * h * abatimento);
            }
            else
            {
                dailyQty = qty / members;
            }

            return dailyQty;
        }

        public double CalculateJobDuration(string aim,
                                     int qty,
                                     double qtyH, string department, int members = 0)
        {
            var linesQuery = from lines in Models.Tables.Lines
                             where lines.Line == aim && lines.Department == department
                             select lines;

            var lineMembers = linesQuery.Select(x => x.Members).SingleOrDefault();
            if (lineMembers == 0) lineMembers = 1;
            if (members != lineMembers && members > 0) lineMembers = members;

            var lineAbatimento = linesQuery.Select(x => x.Abatimento).SingleOrDefault();

            var abatimento = 0.0;
            if (lineAbatimento > 0.0)
                abatimento = Math.Round(Convert.ToDouble
                    (lineAbatimento) / 100, 2);

            if (qty == 0) return 0;

            var h = 0.0;
            if (Store.Default.sectorId == 1) h = Store.Default.confHour;
            else if (Store.Default.sectorId == 2) h = Store.Default.stiroHour;
            else if (Store.Default.sectorId == 7) h = Store.Default.tessHour;
            else if (Store.Default.sectorId == 8) h = Store.Default.sartHour;

            var duration = 0.0;
            
            if (Store.Default.sectorId != 7)
            {
                duration = Convert.ToDouble(qty / (lineMembers * qtyH * h * abatimento));
            }
            else
            {
                var dailyQty = (qty / lineMembers) / qtyH;
                duration = dailyQty * abatimento;
            }

            return duration;
        }

        public object[] GetJobContinum(string job,
            string aim, string dept)
        {
            object[] obj = new object[] { };
            var production = from prod in Models.Tables.Productions
                             where prod.Commessa == job
                             && prod.Line == aim && prod.Department == dept
                             orderby prod.Data
                             select prod;

            var jobMod = Central.ListOfModels.SingleOrDefault(x => x.Name == job && x.Aim == aim && x.Department == dept);
            var jobModEndProduction = jobMod.ProductionEndDate.Date;
            int.TryParse(jobMod.WorkingDays.ToString(), out var jobWorkingDays);

            var prodStart = production.ToList().Count > 0 ?
                production.ToList().First().Data : new DateTime();
            var prodEnd = production.ToList().Count > 0 ?
                production.ToList().Last().Times : new DateTime();
            if (production.ToList().Count == 0)
            {
                return new object[] {prodStart, prodEnd,
                0,0,0, new TimeSpan(0,0,0,0,0), new TimeSpan(0,0,0,0,0)};     
            }
            var prodOverDays = 0.0;
            var lastDate = DateTime.MinValue;
            var startShift = Central.ShiftFrom.Hours;           
            var endShift = Central.ShiftTo.Hours;
            var shiftRange = Central.ShiftTo.Subtract(Central.ShiftTo).TotalHours;     
            var dymDailyQty = 0;           
            var dymQty = 0.0;        
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
            {
                var hour = 0;
                switch (Store.Default.sectorId)
                {
                    case 1:
                        hour = Convert.ToInt32(Store.Default.confHour);
                        break;
                    case 2:
                        hour = Convert.ToInt32(Store.Default.stiroHour);
                        break;
                    case 7:
                        hour = Convert.ToInt32(Store.Default.tessHour);
                        break;
                    case 8:
                        hour = Convert.ToInt32(Store.Default.sartHour);
                        break;
                }

                var r = Convert.ToInt32(restQty / 60);  
                var dd = r / hour;  
                var hh = r % hour;                       
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
            if (endDate.Date > jobModEndProduction) jobWorkingDays += 1;
            using (var context = new System.Data.Linq.DataContext(Central.SpecialConnStr))
            {
                context.ExecuteCommand("update objects set " +
                    "startprod={0},endprod={1},prodqty={2},delayts={3}, delaystart={4},delayend={5}, workingdays={9} " +
                    "where ordername={6} and aim={7} and department={8}", 
                    GetLSpan(prodStart),
                    GetLSpan(endDate), 
                    dymQty,
                    dymAlertTime.Ticks,
                    GetLSpan(endDate),
                    GetLSpan(endDate) + dymAlertTime.Ticks,
                    job,aim, dept, jobWorkingDays);
            }
            return obj;
        }

        public static JobModel GetModelIndex(string modelText, List<Index> lst, int index)
        {
            JobModel jMod = default;
            foreach (var item in lst)
            {
                if (item.ObjText != modelText) continue;
                var jIndex = lst.LastOrDefault(x => x.RowIndex == item.RowIndex && x.ObjIndex == item.ObjIndex + index);
                if (jIndex != null)
                    jMod = Central.ListOfModels
                        .FirstOrDefault(x => x.Name == jIndex.ObjText &&
                        x.Aim == jIndex.ObjAim && x.Department == jIndex.ObjDept);
                else
                    jMod = null;
            }
            return jMod;
        }

        public static JobModel GetIndexAfterLock(string model, List<Index> lst, int rIndex, int oIndex)
        {
            JobModel jMod = default;
            foreach (var item in lst)
            {
                if (item.ObjText != model) continue;
                var jIndex = lst.LastOrDefault(x => x.RowIndex == item.RowIndex + rIndex && x.ObjIndex == oIndex);
                if (jIndex != null)
                    jMod = Central.ListOfModels
                        .FirstOrDefault(x => x.Name == jIndex.ObjText &&
                        x.Aim == jIndex.ObjAim && x.Department == jIndex.ObjDept);
                else
                    jMod = null;
            }
            return jMod;
        }

        public static DateTime GetLineLastDate(string line, string dept)
        {
            var query = from models in Central.ListOfModels
                        where models.Aim == line && models.Department == dept
                        orderby models.DelayEndDate
                        select models;

            var lst = query.ToList();

            if (lst.Count > 0)
            {
                DateTime lastDate;
                if (lst.Last().DelayEndDate > Config.MinimalDate)
                {
                    lastDate = lst.Last().DelayEndDate;
                }
                else
                {
                    lastDate = lst.Last().EndDate;
                }

                return lastDate;
            }
            else
            {
                return Config.MinimalDate;
            }
        }

        public double GetPrice(string article)
        {
            var artQuery = from art in Models.Tables.Articles
                           where art.Articol == article && art.Idsector == Store.Default.sectorId && art.IsDeleted != true
                           select art;

            if (artQuery != null)
            {
                var price = artQuery.Select(x => x.Prezzo).FirstOrDefault();
                double.TryParse(price.ToString(), out var p);
                return p;
            }
            else
            {
                return 0.0;
            }
        }       
    }

    public class Index
    {
        public Index()
        {
        }

        public Index(int rowIndex, int objIndex, string objText, string objAim, string objDept)
        {
            RowIndex = rowIndex;
            ObjIndex = objIndex;
            ObjText = objText;
            ObjAim = objAim;
            ObjDept = objDept;
        }

        public int RowIndex { get; set; }

        public int ObjIndex { get; set; }

        public string ObjText { get; set; }

        public string ObjAim { get; set; }

        public string ObjDept { get; set; }
    }

    public class LineHolidaysEmbeded
    {
        public string Line { get; set; }

        public string HolidayArray { get; set; }

        public DateTime Holiday { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

        public string Department { get; set; }

        public LineHolidaysEmbeded(string line, DateTime hld, int m, int y)
        {
            Line = line;
            Holiday = hld;
            Month = m;
            Year = y;
        }

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
