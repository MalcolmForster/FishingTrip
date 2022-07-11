<?php

require_once("checklogin.php");

if($_SERVER['REQUEST_METHOD'] === 'POST') {

  if(isset($_POST['checkSpot'])) {

    // Need to add a check that will ensure that fishing spots only contain letters and numbers to combat SQL injection

    if(isset($_POST['addNewFav'])) {

      $sqlq = "SELECT `favspots` FROM `users` WHERE `Username` = '".trim($_SESSION['username'])."';";
      $results = mysqli_query($link,$sqlq);
      $resultString = mysqli_fetch_row($results);      
      $newString = $resultString[0].trim($_POST['fishSpot']).",";
      //echo "New string is - ".$newString." and its type is ".gettype($newString);
      mysqli_free_result($results);

      $sql = "UPDATE `users` SET `favspots` = ? WHERE `Username` = '".trim($_SESSION['username'])."';";
      altermysqli($link, $sql, array($newString));      
    }

  }

  if(isset($_POST['rmvFav'])) {
    $favs = getFav($link);
    $remove = $_POST['favCurrents'].","; 

    if(strpos($favs, $remove) !== -1) {      
      $newFavs = str_replace($remove, "", $favs);
      $sql = "UPDATE `users` SET `favspots` = ? WHERE `Username` = '".trim($_SESSION['username'])."';";
      //echo "The favourite to remove is $remove</br>The new favourite string will then become $newFavs</br>$sql";
      altermysqli($link, $sql, array($newFavs));
    } else {
      echo ("Something went wrong");
    }

  }

  // By the sounds of it need to use a shell command to run a python script

  $directory = str_replace("user", "scripts/FSServer.py", getcwd());
  
  $command = ("python3 " . $directory." request \"" . trim($_POST['fishSpot']) ."\" ". join(" ",$_POST['days']));
  $output = exec($command);

  // $command = escapeshellcmd("python3 " . $directory." request \"" . trim($_POST['fishSpot']) ."\" ". join(" ",$_POST['days']));
  // $output = shellexec($command);

  $decoded = json_decode($output,TRUE);

  echo $decoded["Wed"];



}

?>

<!DOCTYPE html>

<html lang="en">

<head>
  <meta charset="UTF-8">
  <meta http-equiv="X-UA-Compatible" content="IE=edge">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <title>Fishr</title>
  <link rel="stylesheet" href="../main.css">
  <script defer src="login.js"></script>
</head>

<body class="light-theme">
  <h2>Should I go fishing??</h2></br>
  <I>Yes.....yes you should, and below is why</I>
  <p>This section is designed to allow the fisherman to quickly see all the stats of their favourite fishing spot, limited to weather data within a week. The select days below will be the next occurance of that day.</p>
  <!--

    What to include on this page:
    - Add/remove favourite fishing spots
    - Select single or multiple fishing spots to compare -> checkboxes in option boxes need to be done with JS apparently
    - Quick comparison between these fishing spots already loaded
    - Search any fishing spots and compare to favourites maybe?

  -->

  <form id="checkSpot" method="post" action="<?php echo htmlspecialchars($_SERVER["PHP_SELF"]); ?>">
    <h3>Check conditions at a fishing spot</h3>
    <input type="text" name="fishSpot" placeholder="Find a fishing spot"/> Add to favourites? <input type="checkbox" name="addNewFav"/></br>

      <!-- <form id="favSpots" method="post" action="<?php echo htmlspecialchars($_SERVER["PHP_SELF"]); ?>"> -->
      <?php
        $sqlq = "SELECT `favspots` FROM `users` WHERE `Username` = '".trim($_SESSION['username'])."';";
        $results = mysqli_query($link,$sqlq);
        $resultString = mysqli_fetch_row($results);
        $favSpots = str_getcsv($resultString[0]);
        unset($favSpots[array_search("",$favSpots)]);
        mysqli_free_result($results);
      ?>
      <br>
      <I>Or choose from current favourites</I></br>
      <select name="favCurrents">
        <?php 
        foreach($favSpots as $spot) {
          echo "<option name='option' value='$spot'>$spot</option>";
        }
        
        ?>
      </select><input type='submit' name ='rmvFav' value='Remove Spot from Favourites'/></br></br>
      <p>Select Days:</p>
      
      <ul name = "DayList">
        <li>Monday    <input type="checkbox" name="days[]" value="Mon"/></li>
        <li>Tuesday   <input type="checkbox" name="days[]" value="Tue"/></li>
        <li>Wednesday <input type="checkbox" name="days[]" value="Wed"/></li>
        <li>Thursday  <input type="checkbox" name="days[]" value="Thu"/></li>
        <li>Friday    <input type="checkbox" name="days[]" value="Fri"/></li>
        <li>Saturday  <input type="checkbox" name="days[]" value="Sat"/></li>
        <li>Sunday    <input type="checkbox" name="days[]" value="Sun"/></li>
      </ul>      
      
      <input type='submit' name='checkSpot' value = 'Submit'></br>
      <!-- </form> -->
    

  </form>
  
  <noscript>You need to enable JavaScript to view the full site.</noscript>
  <a href = "usercontrol.php">Return to User homepage</a>
</body>
<br><br>
</html>

<?php

?>