using static FishingTrip.Pages.Shared._Common;

namespace FishingTrip.Pages.Shared
{
    public class SpotForecast
    {
        public string spotName;
        //private string userID;
        public Dictionary<string, Hour[]> dayForecasts = new Dictionary<string, Hour[]>();
        private string info = "Spot not found"; //To be returned if not info found, perhaps change to null instead
        //DayForecast[] spotWeek = new DayForecast[7];
        public Dictionary<string, Hour[]> spotWeek = new Dictionary<string, Hour[]>();

        public SpotForecast(char arg, string spot, string uId) //arg - s = searched Spot, f = favourite Spot
        {
            spotName = spot;
            if (arg == 's')
            {
                spotWeek = getForecastConditions("FTW", spot, uId);
            }
            else if (arg == 'f')
            {
                spotWeek = getForecastConditions("SF", spot, uId);
                if (spotWeek.Keys.First() == "ToBeFound")
                {
                    spotWeek = getForecastConditions("T4F", spot, uId);

                }
            }
        }
    }

    public class Hour
    {
        public string Time { get; set; }
        public string Rain { get; set; }
        public string Sunset { get; set; }
        public string Sunrise { get; set; }
        public string LowTide { get; set; }
        public string HighTide { get; set; }
        public string Chilltemp { get; set; }
        public string WavePower { get; set; }
        public string WindSpeed { get; set; }
        public string Temperature { get; set; }
        public string WaveHeight { get; set; }
        public string WaveDirection { get; set; }
        public string Winddirection { get; set; }
        public string WaterTemperature { get; set; }
        public string FishActivity { get; set; }
    }

}
