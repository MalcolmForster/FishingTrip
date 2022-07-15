using System;
using System.Diagnostics;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace FishingTrip.Pages.Shared
{
    public class _Common
    {      
        private static string dbCommEx(string command)
        {
            string MyConnectionString = "Server=(localdb)\\MSSQLLocalDB;Database=aspnet-FishingTrip-53bc9b9d-9d6a-45d4-8429-2a2761773502;user=FishingTrip;password=FishingTrip123;";
            SqlDataReader rdr = null;
            SqlConnection cnn;
            cnn = new SqlConnection(MyConnectionString);

            try
            {
                cnn.Open();

                SqlCommand cmd = new SqlCommand(command, cnn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Console.WriteLine(rdr[0]);
                }


            }
            finally
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

        }
        public static string spot_Forecast(string Spot)
        {
            dbCommEx("");
            string json = "";

              //$directory = str_replace("user", "scripts/FSServer.py", getcwd());
  
              //$command = ("python3 ". $directory." request \"".trim($_POST['fishSpot'])."\" ".join(" ",$_POST['days']));
              //$output = exec($command);

              //// $command = escapeshellcmd("python3 " . $directory." request \"" . trim($_POST['fishSpot']) ."\" ". join(" ",$_POST['days']));
              //// $output = shellexec($command);

              //$decoded = json_decode($output, TRUE);

              //echo $decoded["Wed"];

            return json;
        }
        public static List<string> user_Favorites()
        {
            List<string> favSpots = new List<string>();
            

            //$sqlq = "SELECT `favspots` FROM `users` WHERE `Username` = '".trim($_SESSION['username'])."';";

            //$results = mysqli_query($link,$sqlq);
            //$resultString = mysqli_fetch_row($results);
            //$favSpots = str_getcsv($resultString[0]);
            //unset($favSpots[array_search("",$favSpots)]);
            //mysqli_free_result($results);

            return favSpots;
        }

        public static void add_Favourite(string fav, string uId)
        {
            dbCommEx("SELECT [FavSpots] FROM [dbo].[AspNetUsers] WHERE Id='" + uId + "';");
            



            //$sqlq = "SELECT `favspots` FROM `users` WHERE `Username` = '".trim($_SESSION['username'])."';";
            //$results = mysqli_query($link,$sqlq);
            //$resultString = mysqli_fetch_row($results);      
            //$newString = $resultString[0].trim($_POST['fishSpot']).",";
            ////echo "New string is - ".$newString." and its type is ".gettype($newString);
            //mysqli_free_result($results);

            //$sql = "UPDATE `users` SET `favspots` = ? WHERE `Username` = '".trim($_SESSION['username'])."';";
            //altermysqli($link, $sql, array($newString));
        }

        public static void remove_Favourite(string fav)
        {
            dbCommEx("");

            //    $favs = getFav($link);
            //    $remove = $_POST['favCurrents'].",";

            //if (strpos($favs, $remove) !== -1)
            //{      
            //    $newFavs = str_replace($remove, "", $favs);
            //    $sql = "UPDATE `users` SET `favspots` = ? WHERE `Username` = '".trim($_SESSION['username'])."';";
            //    //echo "The favourite to remove is $remove</br>The new favourite string will then become $newFavs</br>$sql";
            //    altermysqli($link, $sql, array($newFavs));
            //}
            //else
            //{
            //    echo("Something went wrong");
            //}
        }
    }
}
