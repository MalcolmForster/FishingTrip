using static FishingTrip.Pages.Shared._Common;

namespace FishingTrip.Pages.Shared
{
    public class ForecastFormat
    {
        public string lowTides = "";
        public string highTides = "";
        public string waterTemp = "";        
        public static string[] displayTimes = {"0 AM","3 AM","6 AM","9 AM","12 PM","3 PM","6 PM","9 PM"};
        private Queue<string> waveStrings = new Queue<string>();
        public string[] waveInfo = new string[displayTimes.Length+1];
        //This class is meant to return the information to a page about the forecast of a fishing spot from both Surf-Forecast and Tides4Fishing
        public ForecastFormat(KeyValuePair<string, Hour[]> dayInfo) //Constructor for the _ForecastFormat
        {
            // Concat strings for high and low tides
            foreach (Hour time in dayInfo.Value)
            {
                if (time.LowTide != "" && time.LowTide +" " != lowTides) { lowTides = string.Concat(lowTides,time.LowTide, " ");}
                if (time.HighTide != "" && time.HighTide +" " != highTides) { highTides = string.Concat(highTides, time.HighTide, " ");}
                if (time.WavePower != null)
                {
                    waveStrings.Enqueue(String.Format("{0} kj / {1} m", time.WavePower, time.WaveHeight));
                }
                else
                {
                    waveStrings.Enqueue(String.Format("{0} m", time.WaveHeight));
                }                    
                    //string.Concat(time.WavePower, " kJ", time.WaveHeight, " m"));;
            }
            //Add infomation to an array
            waveInfo[0] = "Wave Power/Height:";
            for (int i = 1; i < waveInfo.Length; i++)
            {
                if(i < 9 - dayInfo.Value.Count()) { waveInfo[i] = "---------"; }
                else { waveInfo[i] = waveStrings.Dequeue(); }
            }
            waterTemp = @dayInfo.Value.First().WaterTemperature.Replace("[", "").Replace("]", "");
        }
    }

    public class ForecastDiv
    {
        public string searchSpot { get; set; }
        public string rawDiv;
        public ForecastDiv(KeyValuePair<string, Hour[]> dayInfo)
        {
            //this.searchSpot = searchSpot;
            ForecastFormat forecastDiv = new ForecastFormat(dayInfo);
            List<string> times = new List<string>();
            List<string> waveInfo = new List<string>();

            foreach (string time in ForecastFormat.displayTimes)
            {
                times.Add("<th>" + time + " </th>");
            }
            foreach (string s in forecastDiv.waveInfo)
            {
                waveInfo.Add("<td>" + s + " </td>");
            }

            string concatTimes = String.Concat(times.ToArray());
            string concatWaveInfo = String.Concat(waveInfo.ToArray());

            rawDiv = @"<div name = 'forecastDiv' class='forecastDiv' style='display:none' id='" + dayInfo.Key + "'>" +
                        "<h2 style = 'display: inline-block;'>" + dayInfo.Key + ": " +
                        "</h2><label>Low tides: " + forecastDiv.lowTides + "</label>" +
                        "<label>, High tides: " + forecastDiv.highTides + "</label>" +
                        "<label>, Sea Temperature:" + forecastDiv.waterTemp + "</label>" +
                        "<table class=\"forecastTable\"><tr><th></th>" + concatTimes + "</tr><tr>" +
                        concatWaveInfo+"</tr></table></div>";
        }
    }    
}
