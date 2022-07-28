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

        public ForecastDiv(string searchSpot)
        {
            this.searchSpot = searchSpot;
        }


        public static void forecastDiv()
        {

        }
    }
    
}
