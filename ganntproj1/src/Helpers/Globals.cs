using System;

namespace ganntproj1
    {
    public class Globals
    {
        private static string order;
        public static string GetOrder() => order;
        public static void SetOrder(string value) => order = value;
        private static string line;
        public static string GetLine() => line;
        public static void SetLine(string value) => line = value;
        private static string article;
        public static string GetArticle() => article;
        public static void SetArticle(string value) => article = value;
        private static DateTime programationDate;
        public static DateTime GetProgramationDate() => programationDate;
        public static void SetProgramationDate(DateTime value) => programationDate = value;
        public static DateTime GetNextWeekday(DateTime start, DayOfWeek day)
        {
            int daysToAdd = ((int)day - (int)start.DayOfWeek + 7) % 7;
            return start.AddDays(daysToAdd);
        }
    }
}
