using System;
using System.Diagnostics;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text.Json;

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
            for(int i = 1; i < days.Length; i++)
            {
                DayOfWeek followingDay = curDay.AddDays(i).DayOfWeek;
                days[i] = followingDay.ToString();
            }
            return days;
        }
        private static SqlConnection dbConnect()
        {
            string MyConnectionString = "Insert server details here";
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

        private static void closeDB(SqlConnection cnn,SqlDataReader rdr)
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
            public string WSW { get; set; }
            public string Winddirection { get; set; }
        }

        public static Dictionary<string, Hour[]> getFavConditions(string spot, string[] days)
        {
            string MyConnectionString = "Insert server details here";
            SqlDataReader rdr = null;
            SqlConnection cnn =new SqlConnection();
            cnn = new SqlConnection(MyConnectionString);
            string query = ("SELECT [dataSF] FROM favForecasts WHERE [spot] = @spot");
            SqlCommand cmd = new SqlCommand(query, cnn);

            try
            {
                cmd.Connection.Open();
            } catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            cmd.Parameters.AddWithValue("@spot", spot);

            var working = new Dictionary<string, JsonDocument>();
            var work = new Dictionary<string, object>();

            if (cnn != null && cnn.State == ConnectionState.Closed)
            {
                //nothin here apparently
            } else {
                using (rdr = cmd.ExecuteReader())
                {
                    while(rdr.Read())
                    {
                        JsonDocument json = JsonDocument.Parse(rdr.GetString(0));
                        working = JsonSerializer.Deserialize<Dictionary<string, JsonDocument>>(json);
                    }
                }
            }
            
            Hour[] theDay = new Hour[working.Count()];
            var dict = new Dictionary<string, Hour[]>();

            foreach(KeyValuePair<string,JsonDocument> kvp in working)
            {
                work.Add(kvp.Key,JsonSerializer.Deserialize<Dictionary<string, object>>(json));
            }

            foreach(Hour hour in theDay)
            {

            }



            closeDB(cnn, rdr);
            return dict;
        }

        private static void setFavConditions()
        {

        }

        public static string[] getCheckedDays()
        {
            string[] s = new string[1];

            return s;
        }

        //-------------No longer needed as the Forecast is taken off mysql database, may need to reenable-----------
        //public static string spot_Forecast(string Spot, string[] days)
        //{
        //    //This requires the use of the the python webscraper, found in Pages\Shared\Scripts
        //    string json = "";
        //    string[] daysShort = new string[days.Length];
        //    string pyScript = "Pages\\Shared\\scripts\\FSServer.py";

        //    for(int i = 0; i < days.Length; i++)
        //    {
        //        daysShort[i] = days[i].Substring(0,3);
        //    }

        //    ProcessStartInfo start = new ProcessStartInfo();
        //    start.FileName = "Root to python path;
        //    //Console.WriteLine(string.Format("{0} request {1} {2}",pyScript, "\"" + Spot + "\"", String.Join(" ", daysShort)));
        //    start.Arguments = string.Format("{0} request {1} {2}",pyScript, "\"" + Spot + "\"", String.Join(" ", daysShort));
        //    start.UseShellExecute = false;
        //    start.RedirectStandardOutput = true;
        //    Process p = new Process();
        //    p.StartInfo = start;
        //    p.Start();
        //    StreamReader reader = p.StandardOutput;
        //    json = reader.ReadToEnd();


        //    //$directory = str_replace("user", "scripts/FSServer.py", getcwd());

        //    //$command = ("python3 ". $directory." request \"".trim($_POST['fishSpot'])."\" ".join(" ",$_POST['days']));
        //    //$output = exec($command);

        //    //// $command = escapeshellcmd("python3 " . $directory." request \"" . trim($_POST['fishSpot']) ."\" ". join(" ",$_POST['days']));
        //    //// $output = shellexec($command);

        //    //$decoded = json_decode($output, TRUE);

        //    //echo $decoded["Wed"];

        //    return json;
        //}

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
    }
}
