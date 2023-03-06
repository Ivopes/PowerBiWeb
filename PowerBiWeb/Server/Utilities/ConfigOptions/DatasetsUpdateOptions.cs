namespace PowerBiWeb.Server.Utilities.ConfigOptions
{
    public class DatasetsUpdateOptions
    {
        public const string DatasetsUpdate = "DatasetsUpdate";
        public UpdateFrequency UpdateFrequency { get; set; }
        public bool Enabled { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        private int _hour;
        public int Hour 
        {
            get => _hour;
            set => _hour = Math.Clamp(value, 0, 23);
        }
    }
    public enum UpdateFrequency
    {
        Hour = 0,
        Day = 1,
        Week = 2
    }
}
