using System;
using System.Diagnostics;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text.Json;
using Microsoft.JSInterop;

namespace FishingTrip.Pages.Shared
{
    public class _Common
    {
        public static string[] nextDays(int num)
        {
            string[] days = new string[num];
            DateTime curDay = DateTime.Today;
            DayOfWeek dayOfWeek = curDay.DayOfWeek;

            days[0] = dayOfWeek.ToString();
            //fills array with the following days of a specified length
            for (int i = 1; i < days.Length; i++)
            {
                DayOfWeek followingDay = curDay.AddDays(i).DayOfWeek;
                days[i] = followingDay.ToString();
            }
            return days;
        }

        private static SqlConnection dbConnect()
        {
            string MyConnectionString = _ServerConnections.main;
            SqlDataReader rdr = null;
            SqlConnection cnn;
            cnn = new SqlConnection(MyConnectionString);

            try
            {
                cnn.Open();
                return cnn;
            } catch
            {
                Console.WriteLine("Failed to connect to database");
                return null;
            }
        }

        private static void closeDB(SqlConnection cnn, SqlDataReader rdr)
        {
            if (rdr != null)
            {
                rdr.Close();
            }
            if (cnn != null)
            {
                cnn.Close();
            }
        }
        private static void dbExecute(string command)
        {
            SqlConnection cnn = dbConnect();

            try
            {
                SqlCommand cmd = new SqlCommand(command, cnn);
            } catch
            {
                Console.WriteLine("Execute command failed");
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

        public static Dictionary<string, Hour[]> getDayInfo(string website, JsonDocument json)
        {
            var jsonDict = new Dictionary<string, JsonDocument>();
            jsonDict = JsonSerializer.Deserialize<Dictionary<string,JsonDocument>>(json);
            var dailyForecast = new Dictionary<string, Hour[]>();

            List<Hour> hourList = new List<Hour>();
            //string[] properties = { "Rain", "Sunset", "Sunrise", "LowTide", "HighTide", "Chilltemp", "WavePower", "WindSpeed", "Temperature", "WaveHeight", "WaveDirection", "WSW", "Winddirection" };

            if (website == "SF")
            {
                foreach (KeyValuePair<string, JsonDocument> kvp in jsonDict)
                {
                    if (kvp.Key != "Spot")
                    {
                        //String here is the Day
                        var hoursForDay = JsonSerializer.Deserialize<Dictionary<string, JsonDocument>>(kvp.Value);
                        hourList.Clear();
                        foreach (KeyValuePair<string, JsonDocument> kvp2 in hoursForDay)
                        {
                            //String here is the time of day, with JsonDocument being information
                            //A new hour object is made and all the values assigned
                            Hour newHour = new Hour();
                            newHour.Time = kvp2.Key;
                            newHour.Rain = kvp2.Value.RootElement.GetProperty("Rain").ToString();
                            newHour.Sunset = kvp2.Value.RootElement.GetProperty("Sunset").ToString();
                            newHour.Sunrise = kvp2.Value.RootElement.GetProperty("Sunrise").ToString();
                            newHour.LowTide = kvp2.Value.RootElement.GetProperty("Low Tide").ToString();
                            newHour.HighTide = kvp2.Value.RootElement.GetProperty("High Tide").ToString();
                            newHour.Chilltemp = kvp2.Value.RootElement.GetProperty("Chill temp").ToString();
                            newHour.WavePower = kvp2.Value.RootElement.GetProperty("Wave Power").ToString();
                            newHour.WindSpeed = kvp2.Value.RootElement.GetProperty("Wind Speed").ToString();
                            newHour.Temperature = kvp2.Value.RootElement.GetProperty("Temperature").ToString();
                            newHour.WaveHeight = kvp2.Value.RootElement.GetProperty("Wave Height").ToString();
                            newHour.WaveDirection = kvp2.Value.RootElement.GetProperty("Wave Direction").ToString();
                            newHour.Winddirection = kvp2.Value.RootElement.GetProperty("Wind direction").ToString();
                            newHour.WaterTemperature = kvp2.Value.RootElement.GetProperty("Sea Temperature").ToString();
                            //Add the hour to the hour list for the day
                            hourList.Add(newHour);
                        }
                        //Convert the daily hours to an array
                        Hour[] hourArray = hourList.ToArray();
                        dailyForecast.Add(kvp.Key, hourArray);
                    }
                    else
                    {
                        Hour[] newHour = new Hour[0];
                        dailyForecast.Add("Not Found", newHour);
                    }
                }
            }
            else if (website == "T4F")
            {
                foreach (KeyValuePair<string, JsonDocument> kvp in jsonDict)
                {
                    if (kvp.Key != "Spot")
                    {
                        //String here is the Day
                        var hoursForDay = JsonSerializer.Deserialize<Dictionary<string, JsonDocument>>(kvp.Value);
                        hourList.Clear();

                        foreach (KeyValuePair<string, JsonDocument> kvp2 in hoursForDay)
                        {
                            if (ForecastFormat.displayTimes.Contains(kvp2.Key))
                            {
                                //String here is the time of day, with JsonDocument being information
                                //A new hour object is made and all the values assigned
                                Hour newHour = new Hour();
                                newHour.Time = kvp2.Key;
                                //newHour.Rain = kvp2.Value.RootElement.GetProperty("Rain").ToString();
                                //newHour.Sunset = kvp2.Value.RootElement.GetProperty("Sunset").ToString();
                                //newHour.Sunrise = kvp2.Value.RootElement.GetProperty("Sunrise").ToString();
                                newHour.LowTide = kvp2.Value.RootElement.GetProperty("Low Tide").ToString();
                                newHour.HighTide = kvp2.Value.RootElement.GetProperty("High Tide").ToString();
                                //newHour.Chilltemp = kvp2.Value.RootElement.GetProperty("Chill temp").ToString();
                                //newHour.WavePower = kvp2.Value.RootElement.GetProperty("Wave Power").ToString();
                                //newHour.WindSpeed = kvp2.Value.RootElement.GetProperty("Wind Speed").ToString();
                                //newHour.Temperature = kvp2.Value.RootElement.GetProperty("Temperature").ToString();
                                newHour.WaveHeight = kvp2.Value.RootElement.GetProperty("Wave Height").ToString();
                                newHour.WaveDirection = kvp2.Value.RootElement.GetProperty("Wave Direction").ToString();
                                //newHour.Winddirection = kvp2.Value.RootElement.GetProperty("Wind direction").ToString();
                                newHour.WaterTemperature = kvp2.Value.RootElement.GetProperty("Sea Temperature").ToString();
                                newHour.FishActivity = kvp2.Value.RootElement.GetProperty("Fish Activity").ToString();
                                //Add the hour to the hour list for the day
                                hourList.Add(newHour);
                            }

                        }
                        //Convert the daily hours to an array
                        Hour[] hourArray = hourList.ToArray();
                        dailyForecast.Add(kvp.Key, hourArray);
                    }
                    else
                    {
                        Hour[] newHour = new Hour[0];
                        dailyForecast.Add("Not Found", newHour);
                    }

                }
            }
            return dailyForecast;
        }

        [JSInvokable]
        public static Dictionary<string, Hour[]> getFavConditions(string website, string spot, string[] days)//days may need to be used later?
        {
            string MyConnectionString = _ServerConnections.linux;
            SqlDataReader rdr = null;
            SqlConnection cnn =new SqlConnection();
            cnn = new SqlConnection(MyConnectionString);
            string query = String.Format("SELECT [dataSF] FROM favForecasts{0} WHERE [spot] = @spot", website);
            SqlCommand cmd = new SqlCommand(query, cnn);

            try
            {
                cmd.Connection.Open();
            } catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            cmd.Parameters.AddWithValue("@spot", spot);

            var dayInfo = new Dictionary<string, Hour[]>();

            if (cnn != null && cnn.State == ConnectionState.Closed)
            {
                //nothin here currently to deal with a failed connection, maybe an HTML.raw output saying fialed to connect?
            } else {
                using (rdr = cmd.ExecuteReader())
                {
                    while(rdr.Read())
                    {                        
                        JsonDocument json = JsonDocument.Parse(rdr.GetString(0));
                        dayInfo = getDayInfo(website,json);
                    }
                }
            }
            
            closeDB(cnn, rdr);
            return dayInfo;
        }

        public static string[] getCheckedDays()
        {
            string[] s = new string[1];
            return s;
        }

        public static string spot_Forecast(string Spot, string[] days) //WILL NEED TO BE USED TO FIND FORECASTS NOT ON THE FAVOURITE LIST
        {
            //This requires the use of the the python webscraper, found in Pages\Shared\Scripts
            string json = "";
            string[] daysShort = new string[days.Length];
            string pyScript = "Pages\\Shared\\scripts\\FSServer.py";

            for (int i = 0; i < days.Length; i++)
            {
                daysShort[i] = days[i].Substring(0, 3);
            }

            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = "Root to python path;";
            //Console.WriteLine(string.Format("{0} request {1} {2}",pyScript, "\"" + Spot + "\"", String.Join(" ", daysShort)));
            start.Arguments = string.Format("{0} request {1} {2}", pyScript, "\"" + Spot + "\"", String.Join(" ", daysShort));
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            Process p = new Process();
            p.StartInfo = start;
            p.Start();
            StreamReader reader = p.StandardOutput;
            json = reader.ReadToEnd();


            //$directory = str_replace("user", "scripts/FSServer.py", getcwd());

            //$command = ("python3 ". $directory." request \"".trim($_POST['fishSpot'])."\" ".join(" ",$_POST['days']));
            //$output = exec($command);

            //// $command = escapeshellcmd("python3 " . $directory." request \"" . trim($_POST['fishSpot']) ."\" ". join(" ",$_POST['days']));
            //// $output = shellexec($command);

            //$decoded = json_decode($output, TRUE);

            //echo $decoded["Wed"];

            return json;
        }

        public static string[] getFavSpots(string uId)
        {
            SqlConnection cnn = dbConnect();
            SqlDataReader rdr = null;

            if (cnn != null)
            {
                string query = "SELECT [FavSpots] FROM [dbo].[AspNetUsers] WHERE Id=@userID;";
                SqlCommand cmd = new SqlCommand(query, cnn);
                try
                {
                    cmd.Parameters.AddWithValue("@userID", uId);
        }
                catch
        {
                    Console.WriteLine("Parameters failed to be added to query");
                }                
                try
                {
                    using (rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            string[] allSpots = rdr.GetString(0).Trim('\'').Split(",");
            
                            return allSpots;
                        }
                    }
                }
                catch
                {
                    Console.WriteLine("getFavSpots failed to execute reader");
                }
            }
            closeDB(cnn, rdr);
            string[] emptyString = new string[0];
            return emptyString;
        }

        public static void add_Favourite(string fav, string uId)
        {
            string[] allSpots = getFavSpots(uId);            

            if(allSpots != null || allSpots.Length > 0)
            {
                bool addFav = true;
                foreach (string s in allSpots)
                {
                    if (s == fav)
                    {
                        addFav = false;
                        Console.WriteLine("This spot already exists in your list of favorites");
                        break;
                    }
                }
                if (addFav == true)
                {
                    string newFavConcat = String.Join(",", allSpots) + fav + ",";

                    SqlConnection conn = dbConnect();
                    string query = "UPDATE [dbo].[AspNetUsers] SET [FavSpots] = @newFavs WHERE Id= @userID;";
                    SqlCommand alterFavs = new SqlCommand(query, conn);
                    alterFavs.Parameters.AddWithValue("@newFavs", newFavConcat);
                    alterFavs.Parameters.AddWithValue("@userID", uId);

                    try
                    {
                        alterFavs.ExecuteNonQuery();
                        Console.WriteLine(fav + " added to favourites");
                    } catch
                    {
                        Console.WriteLine("Something went wrong when updating table");
                    }
                    closeDB(conn, null);
                }
            } else
            {
                Console.WriteLine("No favourites found");
            }
        }
        public static void remove_Favourite(string fav, string uId)
        {
            string[] allSpots = getFavSpots(uId);

            if (allSpots != null || allSpots.Length > 0)
            {
                bool rmvFav = false;
                foreach (string s in allSpots)
                {
                    if (s == fav)
                    {
                        rmvFav = true;
                        Console.WriteLine(fav + " removed from favourites");
                        break;
                    }
                }
                if (rmvFav == true)
                {
                    string newFavConcat = String.Join(",", allSpots).Replace(fav +",","");
                    SqlConnection conn = dbConnect();
                    string query = "UPDATE [dbo].[AspNetUsers] SET [FavSpots] = @newFavs WHERE Id= @userID;";
                    SqlCommand alterFavs = new SqlCommand(query, conn);
                    alterFavs.Parameters.AddWithValue("@newFavs", newFavConcat);
                    alterFavs.Parameters.AddWithValue("@userID", uId);

                    try
                    {
                        alterFavs.ExecuteNonQuery();
                    }
                    catch
                    {
                        Console.WriteLine("Something went wrong when updating table");
                    }
                    closeDB(conn, null);
                } else
                {
                    Console.WriteLine(fav + " not found in favourites");
                }
            }
            else
            {
                Console.WriteLine("No favourites found");
            }
        }
        public static void newForecastDiv(string fishingSpot)
        {

            Console.WriteLine(fishingSpot);

        }
    }
}
