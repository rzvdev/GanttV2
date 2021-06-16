using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using ganntproj1.src.Models;

namespace ganntproj1.src.Helpers
{
    public class SettingsDom
    {
        public void UpdateSettingsHours(string department, double value1, double value2)
        {
            var q = @"
update settings set settingvalue=@value1, settingvalue2=@value2 where settingkey=@department
";
            using (var con = new SqlConnection(Central.SpecialConnStr))
            {
                var cmd = new SqlCommand
                {
                    CommandText = q,
                    Connection = con,
                    CommandType = CommandType.Text
                };

                cmd.Parameters.Add("@value1", SqlDbType.Float).Value = value1;
                cmd.Parameters.Add("@value2", SqlDbType.Float).Value = value2;
                cmd.Parameters.Add("@department", SqlDbType.NVarChar).Value = department;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public void SaveHoursToSettings()
        {
            var lst = GetSettingsHours();

            foreach (var item in lst)
            {
                switch (item.Key)
                {
                    case "Confezione":
                        Store.Default.confHour = item.Value1;
                        Store.Default.confHourW = item.Value2;
                        break;
                    case "Stiro":
                        Store.Default.stiroHour = item.Value1;
                        Store.Default.stioHourW = item.Value2;
                        break;
                    case "Tessitura":
                        Store.Default.tessHour = item.Value1;
                        Store.Default.tessHourW = item.Value2;
                        break;
                    case "Sartoria":
                        Store.Default.sartHour = item.Value1;
                        Store.Default.sartHourW = item.Value2;
                        break;
                }
            }

            Store.Default.Save();
        }

        private List<SettingsDict> GetSettingsHours()
        {
            var lst = new List<SettingsDict>();

            using (var con = new SqlConnection(Central.SpecialConnStr))
            {
                var cmd = new SqlCommand
                {
                    CommandText = "select settingkey,settingvalue,settingvalue2 from settings",
                    Connection = con,
                    CommandType = CommandType.Text
                };

                con.Open();
                var dr = cmd.ExecuteReader();
                if (dr.HasRows)
                    while (dr.Read())
                    {
                        var key = dr[0].ToString();
                        double.TryParse(dr[1].ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var v1);
                        double.TryParse(dr[2].ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var v2);
                        lst.Add(new SettingsDict(key, v1, v2));
                    }

                con.Close();
            }

            return lst;
        }
    }
}
