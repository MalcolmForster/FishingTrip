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
                if (time.LowTide != "") { lowTides = string.Concat(lowTides,time.LowTide, " ");}
                if (time.HighTide != "") { highTides = string.Concat(highTides, time.HighTide, " ");}
                waveStrings.Enqueue(string.Concat(time.WavePower, " kJ\n", time.WaveHeight, " m"));
            }

            //Add infomation to an array
            waveInfo[0] = "Wave Power:\nWave Height:";
            for (int i = 1; i < waveInfo.Length; i++)
            {
                if(i < 9 - dayInfo.Value.Count()) { waveInfo[i] = "---------\n--------"; }
                else { waveInfo[i] = waveStrings.Dequeue(); }
            }
            waterTemp = @dayInfo.Value.First().WaterTemperature.Replace("[", "").Replace("]", "");
        }
    }
}
