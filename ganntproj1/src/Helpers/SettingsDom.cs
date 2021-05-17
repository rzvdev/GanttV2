using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ganntproj1.src.Models;

namespace ganntproj1.src.Helpers
{
    public class SettingsDom
    {
        public void UpdateSettingsHours(string department, string value1, string value2)
        {
            using (var con = new SqlConnection(Central.SpecialConnStr))
            {
                var cmd = new SqlCommand
                {
                    CommandText = $"update settings set settingvalue='{value1}',settingvalue2='{value2}' where settingkey='{department}'",
                    Connection = con,
                    CommandType = CommandType.Text
                };
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
                    case "Stiro" :
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
                        double.TryParse(dr[1].ToString(), out var v1);
                        double.TryParse(dr[2].ToString(), out var v2); 
                        lst.Add(new SettingsDict(key, v1, v2));
                    }

                con.Close();
            }

            return lst;
        }
    }
}
