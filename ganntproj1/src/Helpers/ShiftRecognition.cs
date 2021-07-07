using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ganntproj1.src.Helpers
{
    public class ShiftRecognition
    {
        (TimeSpan, TimeSpan) GetShiftSpan()
        {
            var query = "  select min(starttime),max(endtime) from shifts where sectorId = '" + 
                Store.Default.sectorId + "'";
            TimeSpan startTime = new TimeSpan();
            TimeSpan endTime = new TimeSpan();

            using (var c = new SqlConnection(Central.SpecialConnStr))
            {
                var cmd = new SqlCommand(query, c);
                c.Open();
                var dr = cmd.ExecuteReader();
                if (dr.HasRows)
                    while(dr.Read())
                    {
                        TimeSpan.TryParse(dr[0].ToString(), out startTime);
                        TimeSpan.TryParse(dr[1].ToString(), out endTime);
                    }
                c.Close();
                dr.Close();
            }

            return (startTime, endTime);
        }

        public DateTime GetEndTimeInShift(DateTime startDate, DateTime endDate)
        {
            var startShift = GetShiftSpan().Item1;
            var endShift = startShift + TimeSpan.FromHours(GetSectorHour());

            var startDateTs = new TimeSpan(0, startDate.Hour, startDate.Minute, 0, 0);
            var endDateTs = new TimeSpan(0, endDate.Hour, endDate.Minute, 0, 0);
            var hb = endDateTs - startDateTs; //hours between

            if (endDateTs > endShift)
            {
                if (startShift + hb < endShift)
                {
                    endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, startShift.Hours + Math.Abs(hb.Hours),Math.Abs(hb.Minutes), 0, 0).AddDays(+1);
                }
                else
                {
                    var ts = (startShift + hb) - endShift;

                    endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, ts.Hours, ts.Minutes, 0, 0).AddDays(+2);
                }
            }
            else if (endDateTs < startShift)
            {
                var ts = endDateTs + startShift;
                endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, ts.Hours, ts.Minutes, 0, 0);
            }

            return endDate;
        }

        double GetSectorHour()
        {
            var hour = 0.0;
            if (Store.Default.sectorId == 1) hour = Store.Default.confHour;
            else if (Store.Default.sectorId == 2) hour = Store.Default.stiroHour;
            else if (Store.Default.sectorId == 7) hour = Store.Default.tessHour;
            else if (Store.Default.sectorId == 8) hour = Store.Default.sartHour;

            return hour;
        }
    }
}
