using System;
using System.Diagnostics;
using System.Web;
using System.Data;
using System.Data.SqlClient;

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
            string MyConnectionString = "Server=(localdb)\\MSSQLLocalDB;Database=aspnet-FishingTrip-53bc9b9d-9d6a-45d4-8429-2a2761773502;user=FishingTrip;password=FishingTrip123;";
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

      /*  public static string spot_Forecast(string Spot)
        {

            //This requires the use of the the python webscraper, found in Pages\Shared\Scripts


            dbExecute("");
            string json = "";

            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = "C: \\Users\\Malcolm Forster\\AppData\\Local\\Microsoft\\WindowsApps\\PythonSoftwareFoundation.Python.3.9_qbz5n2kfra8p0\\python.exe";
            start.Arguments = string.Format("request"{ 0}
            Spot;



            //$directory = str_replace("user", "scripts/FSServer.py", getcwd());

            //$command = ("python3 ". $directory." request \"".trim($_POST['fishSpot'])."\" ".join(" ",$_POST['days']));
            //$output = exec($command);

            //// $command = escapeshellcmd("python3 " . $directory." request \"" . trim($_POST['fishSpot']) ."\" ". join(" ",$_POST['days']));
            //// $output = shellexec($command);

            //$decoded = json_decode($output, TRUE);

            //echo $decoded["Wed"];

            return json;
        } */

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
