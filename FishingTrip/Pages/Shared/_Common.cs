using System;
using System.Diagnostics;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

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
            }
            catch
            {
                Console.WriteLine("Failed to connect to database");
                return null;
            }
        }

        private static SqlConnection linuxConnect()
        {
            string MyConnectionString = _ServerConnections.linux;
            SqlConnection cnn = new SqlConnection(MyConnectionString);
            try
            {
                cnn.Open();
                return cnn;
            }
            catch
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
            }
            catch
            {
                Console.WriteLine("Execute command failed");
            }

        }
        //public class Hour
        //{
        //    public string Time { get; set; }
        //    public string Rain { get; set; }
        //    public string Sunset { get; set; }
        //    public string Sunrise { get; set; }
        //    public string LowTide { get; set; }
        //    public string HighTide { get; set; }
        //    public string Chilltemp { get; set; }
        //    public string WavePower { get; set; }
        //    public string WindSpeed { get; set; }
        //    public string Temperature { get; set; }
        //    public string WaveHeight { get; set; }
        //    public string WaveDirection { get; set; }
        //    public string Winddirection { get; set; }
        //    public string WaterTemperature { get; set; }
        //    public string FishActivity { get; set; }
        //}

        public static Dictionary<string, Hour[]> getDayInfo(string website, JsonDocument json)
        {
            //var jsonDict = new Dictionary<string, JsonDocument>();
            var jsonDict = JsonSerializer.Deserialize<Dictionary<string, JsonDocument>>(json);
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
                        Dictionary<string, JsonDocument> hoursForDay = JsonSerializer.Deserialize<Dictionary<string, JsonDocument>>(kvp.Value);
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
            else if (website == "T4F" || website == "FTW")
            {
                foreach (KeyValuePair<string, JsonDocument> kvp in jsonDict)
                {
                    if (kvp.Key != "Spot not found")
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
                                //newHour.FishActivity = kvp2.Value.RootElement.GetProperty("Fish Activity").ToString();
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
                        dailyForecast.Add("Spot not found", newHour);
                    }

                }
            }
            return dailyForecast;
        }

        //public static Dictionary<string, Hour[]> getNewConditions(string website, string spot)//days may need to be used later?
        //{
        //    string MyConnectionString = _ServerConnections.linux; //Connect to the local database
        //    SqlDataReader rdr = null;
        //    SqlConnection cnn = new SqlConnection();
        //    cnn = new SqlConnection(MyConnectionString);
        //    string query = String.Format("SELECT [dataSF] FROM favForecasts{0} WHERE [spot] = @spot", website);
        //    SqlCommand cmd = new SqlCommand(query, cnn);

        //    try
        //    {
        //        cmd.Connection.Open();
        //    }
        //    catch (SqlException ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }

        //    cmd.Parameters.AddWithValue("@spot", spot);

        //    var dayInfo = new Dictionary<string, Hour[]>();

        //    if (cnn != null && cnn.State == ConnectionState.Closed)
        //    {
        //        //nothin here currently to deal with a failed connection, maybe an HTML.raw output saying fialed to connect?
        //    }
        //    else
        //    {
        //        using (rdr = cmd.ExecuteReader())
        //        {
        //            while (rdr.Read())
        //            {
        //                JsonDocument json = JsonDocument.Parse(rdr.GetString(0));
        //                dayInfo = getDayInfo(website, json);
        //            }
        //        }
        //    }
        //    closeDB(cnn, rdr);
        //    return dayInfo;
        //}

        public static Dictionary<string, Hour[]> getForecastConditions(string website, string spot, string uId)//days may need to be used later?
        {
            //string MyConnectionString = _ServerConnections.linux;            
            //SqlConnection cnn = new SqlConnection(MyConnectionString);

            SqlConnection cnn = linuxConnect();
            SqlDataReader rdr = null;
            string query = "";

            if (website == "FTW")
            {
                query = "SELECT [dataSF] FROM [dbo].[searchForecasts] WHERE [spot] = @spot AND UserName = @user";
            } else {
                query = String.Format("SELECT [dataSF] FROM favForecasts{0} WHERE [spot] = @spot", website);
            }

            SqlCommand cmd = new SqlCommand(query, cnn);

            //try
            //{
            //    cmd.Connection.Open();
            //}
            //catch (SqlException ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}

            cmd.Parameters.AddWithValue("@spot", spot);
            cmd.Parameters.AddWithValue("@user", uId);
            var dayInfo = new Dictionary<string, Hour[]>();

            if (cnn != null && cnn.State == ConnectionState.Closed)
            {
                //nothin here currently to deal with a failed connection, maybe an HTML.raw output saying fialed to connect?
            }
            else
            {
                using (rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        //rdr.GetString();
                        try
                        {
                            JsonDocument json = JsonDocument.Parse(rdr.GetString(0));
                            dayInfo = getDayInfo(website, json);
                        }               
                        catch
                        {
                            Hour[] newHour = new Hour[0];
                            dayInfo.Add("ToBeFound", newHour);
                        }                           
                    }
                }
            }

            closeDB(cnn, rdr);
            return dayInfo;
        }

        public static bool taskRunning = false;
        public static bool addFavRunning = false;

        public static async void runTask(string spot, string request, string param)
        {
            if (request == "update")
            {
                addFavRunning = true;
                await Task.Run(() => spot_Forecast_Script(spot, request, param)).ContinueWith(task => { addFavRunning = false; });

            } else if (request == "request"){
                taskRunning = true;
                await Task.Run(() => spot_Forecast_Script(spot, request, param)).ContinueWith(task => { taskRunning = false; });
            }


        }

        public static async Task spot_Forecast_Script(string Spot, string request, string param) //WILL NEED TO BE USED TO FIND FORECASTS NOT ON THE FAVOURITE LIST
        {
            ProcessStartInfo start = new ProcessStartInfo();
            string pyScript = _ServerConnections.fishScraperPythonFile;
            start.FileName = _ServerConnections.pythonExeLoc;

            start.Arguments = string.Format("\"{0}\" {1} \"{2}\" {3}", pyScript, request, Spot, param);
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            Process p = new Process();
            p.StartInfo = start;
            p.Start();
        }

        public static void delSearch(string spot, string uId)
        {
            SqlConnection cnn = linuxConnect();
            string query = "DELETE FROM searchForecasts WHERE [spot] = @spot AND [UserName] = @userID";
            SqlCommand cmd = new SqlCommand(query, cnn);
            cmd.Parameters.AddWithValue("@spot", spot);
            cmd.Parameters.AddWithValue("@userID", uId);

            try
            {
                cmd.ExecuteNonQuery();
            } catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        //This method gets all recently searched spots from the database
        public static string[] getSearchedSpots(string uId)
        {
            SqlDataReader rdr = null;
            SqlConnection cnn = linuxConnect();

            if (cnn != null)
            {
                string query = "SELECT [spot] FROM searchForecasts WHERE [UserName] = @userID";
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
                        List<string> list = new List<string>();
                        while (rdr.Read())
                        {
                            list.Add(rdr.GetString(0));
                        }
                        string[] allSpots = list.ToArray();
                        return allSpots;
                    }
                }
                catch
                {
                    Console.WriteLine("getSearchedSpots failed to execute reader");
                }
            }
            closeDB(cnn, rdr);
            string[] emptyString = new string[0];
            return emptyString;
        }

        public static bool find_Spot_In_Tables(string spotSearched, string uId)
        {
            SqlConnection cnn = linuxConnect();
            SqlDataReader rdr = null;
            bool spotForecastFound = false;
            string[] tablesToSearch = { "favForecastsSF", "favForecastsT4F", "searchForecasts" };

            foreach (string table in tablesToSearch)
            {
                //Searches each of the above tables for the spot
                string query = String.Format("SELECT spot FROM {0} WHERE spot = @spot", table);
                SqlCommand cmd = new SqlCommand(query, cnn);
                cmd.Parameters.AddWithValue("@spot", spotSearched);

                using (rdr = cmd.ExecuteReader()) //execute command
                {
                    if (rdr.Read()) //read response
                    {
                        string[] curSearches = rdr.GetString(0).Trim('\'').Split(","); //trim the results
                        rdr.Close();

                        if (curSearches != null && curSearches.Length > 0) // if results are found, ie does not return 0 results or null
                        {
                            foreach (string s in curSearches) { Console.WriteLine(s); }

                            try // try to copy the result from that table to the searchForecasts table
                            {
                                //curSearches = new string[0];
                                // if the data is found in the seachForecasts table, first check if the current user is already the one who initilised the search,
                                // if they are then don't copy again, send message saying this is already been searched by them
                                // if the user id is different to current user then copy the data with the current user's user ID
                                if (table.Contains("search"))
                                {
                                    string chkQuery = String.Format("SELECT @spot FROM {0} WHERE UserName = @userID", table);
                                    cmd = new SqlCommand(chkQuery, cnn);
                                    cmd.Parameters.AddWithValue("@spot", spotSearched);
                                    cmd.Parameters.AddWithValue("@userID", uId);
                                    spotForecastFound = true;
                                    using (rdr = cmd.ExecuteReader())
                                    {
                                        curSearches = rdr.GetString(0).Trim('\'').Split(",");
                                    }
                                }
                                if (table.Contains("fav") || curSearches.Length == 0)
                                {
                                    string cpyQuery = String.Format("INSERT INTO searchForecasts (spot, date_time, dataSF) SELECT spot, date_time, dataSF FROM {0} WHERE {0}.spot = @ss", table);
                                    string updQuery = "UPDATE searchForecasts SET UserName = @userId WHERE UserName IS NULL";
                                    cmd = new SqlCommand(cpyQuery, cnn);
                                    cmd.Parameters.AddWithValue("@ss", spotSearched);
                                    cmd.ExecuteNonQuery();
                                    cmd.Dispose();
                                    SqlCommand cmd2 = new SqlCommand(updQuery, cnn);
                                    cmd2.Parameters.AddWithValue("@userID", uId);
                                    cmd2.ExecuteNonQuery();
                                    spotForecastFound = true;
                                    closeDB(cnn, rdr);
                                    break;
                                }
                            }
                            catch
                            {
                                Console.WriteLine("Try in add_Search failed");
                            }
                        }
                    }
                }
            }
            closeDB(cnn, rdr);
            return spotForecastFound;
        }

        //public static bool findSpotInTable(string table, string uId)
        //{
        //    SqlConnection cnn = linuxConnect();
        //    SqlDataReader rdr = null;
        //    bool spotForecastFound = false;

        //    bool found = false;



        //    return found;
        //}

        public static void checkDBsForSearch(string spotSearched, string uId) //draft class to check if forecast info exists in the searchForecasts database. Possibly run every 10 seconds etc to recheck for new searches
        {
            SqlConnection cnn = linuxConnect();
            SqlDataReader rdr = null;
            SqlCommand cmd = new SqlCommand();

            string updateTable = "";
            string forecast = null;
            string findDup = null;

            //PLACES TO CHECK searchForecasts (for any user), favForecastsSF, fav ForecastsT4F

            //Checking searchForecasts for any user
            findDup = ("SELECT dataSF FROM searchForecasts WHERE spot = @spot AND dataSF IS NOT NULL and UserName != @user ORDER BY date_time DESC");
            cmd = new SqlCommand(findDup, cnn);
            cmd.Parameters.AddWithValue("@spot", spotSearched);
            cmd.Parameters.AddWithValue("@user", uId);
            forecast = (String)cmd.ExecuteScalar();

            //See if the spot is on the SF table
            if(forecast == null)
            {
                findDup = ("SELECT dataSF FROM favForecastsSF WHERE spot = @spot");
                cmd = new SqlCommand(findDup, cnn);
                cmd.Parameters.AddWithValue("@spot", spotSearched);
                forecast = (String)cmd.ExecuteScalar();
                

                if (forecast != null && (forecast.IndexOf("Not found") != -1))
                {
                    forecast = null;
                }
            }

            // See if the spot is on the T4F table
            if (forecast == null)
            {
                findDup = ("SELECT dataSF FROM favForecastsT4F WHERE spot = @spot");
                cmd = new SqlCommand(findDup, cnn);
                cmd.Parameters.AddWithValue("@spot", spotSearched);
                forecast = (String)cmd.ExecuteScalar();

                if (forecast != null && (forecast.IndexOf("Not found") != -1))
                {
                    forecast = null;
                }
            }

            if (forecast != null)
            {
                //UPDATE WITH THE NEW FORECAST
                updateTable = "UPDATE searchForecasts SET dataSF = @forecast WHERE spot = @spot AND UserName = @user";
                SqlCommand update = new SqlCommand(updateTable, cnn);
                update.Parameters.AddWithValue("@forecast", forecast);
                update.Parameters.AddWithValue("@spot", spotSearched);
                update.Parameters.AddWithValue("@user", uId);
                try
                {
                    update.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("She failed:" + ex);
                }
            }

            //}
            // }
            //bool spot_Added = find_Spot_In_Tables(spotSearched, uId); //See if the spot exists on the table CHANGED TO SEE IF DATA EXISTS FOR THAT SPECIFIC SPOT ON searchForecast
            //string[] arg = { "FTW" };
            //spot_Forecast_Script(spotSearched, arg);
            closeDB(cnn, rdr);
        }

        public static void add_Search(string spotSearched, string uId)
        {
            SqlConnection cnn = linuxConnect();
            SqlDataReader rdr = null;
            string findDuplicate = String.Format("SELECT * FROM searchForecasts WHERE spot = '{0}' AND UserName = '{1}'",spotSearched, uId);
            //Console.WriteLine(findDuplicate);
            SqlCommand fndDup = new SqlCommand(findDuplicate, cnn);
            rdr = fndDup.ExecuteReader();
            if (!rdr.Read()) //read response
            {
                rdr.Close();
                DateTime nowDate = DateTime.Now;
                string qry = String.Format("INSERT INTO searchForecasts (spot, date_time, UserName) VALUES ('{0}', '{1}', '{2}')", spotSearched, nowDate.ToString("yyyy-MM-dd HH:mm:ss.fff"), uId);
                SqlCommand newSearch = new SqlCommand(qry, cnn);
                newSearch.ExecuteNonQuery();
                closeDB(cnn, rdr);                
            }
            else
            {
                Console.WriteLine("User has already searched for " +spotSearched);
                rdr.Close();
            }
            
            closeDB(cnn, rdr);
            checkDBsForSearch(spotSearched, uId);

        }

        // This method returns a list of spots the use has on their favourite list
        public static string[] getFavSpots(string uId)
        {
            SqlConnection cnn = linuxConnect();
            SqlDataReader rdr = null;

            if (cnn != null)
            {
                string query = "SELECT [FavSpots] FROM [dbo].[userFavourites] WHERE Id=@userID;";
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
                        if (!rdr.HasRows)
                        {
                            string addUserFav = "INSERT INTO [dbo].[userFavourites] (Id) VALUES @userID;";
                            SqlCommand addUserToFavs = new SqlCommand(addUserFav, cnn);
                            addUserToFavs.Parameters.AddWithValue("@userID", uId);
                            addUserToFavs.ExecuteNonQuery();                            
                        } else
                        {
                            if (rdr.Read())
                            {
                                string[] allSpots = rdr.GetString(0).Trim('\'').Split(",");
                                //foreach (string s in allSpots)
                                return allSpots;
                            }
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

            if (allSpots != null || allSpots.Length > 0)
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
                    runTask(fav,"update", "");
                    string newFavConcat = String.Join(",", allSpots) + fav + ",";
                    SqlConnection conn = linuxConnect();
                    string query = "UPDATE [dbo].[userFavourites] SET [FavSpots] = @newFavs WHERE Id= @userID;";
                    SqlCommand alterFavs = new SqlCommand(query, conn);
                    alterFavs.Parameters.AddWithValue("@newFavs", newFavConcat);
                    alterFavs.Parameters.AddWithValue("@userID", uId);

                    try
                    {
                        alterFavs.ExecuteNonQuery();
                        Console.WriteLine(fav + " added to favourites");
                    }
                    catch
                    {
                        Console.WriteLine("Something went wrong when updating table");
                    }
                    closeDB(conn, null);                    
                }
            }
            else
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
                    string newFavConcat = String.Join(",", allSpots).Replace(fav + ",", "");
                    SqlConnection conn = linuxConnect();
                    string query = "UPDATE [dbo].[userFavourites] SET [FavSpots] = @newFavs WHERE Id= @userID;";
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
                }
                else
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
