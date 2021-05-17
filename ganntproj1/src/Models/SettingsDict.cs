namespace ganntproj1.src.Models
{ 
    public class SettingsDict
    {
        public string Key { get; set; }
        public double Value1 { get; set; }
        public double Value2 { get; set; }

        public SettingsDict(string key, double v1, double v2)
        {
            Key = key;
            Value1 = v1;
            Value2 = v2;
        }
    }
}
