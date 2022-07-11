<?php

require_once("checklogin.php");

$postTitle = $postBody = "";

if($_SERVER["REQUEST_METHOD"] == "POST") {

  $tripTitle = trim($_POST["tripTitle"]);
  $tripLocation = trim($_POST["tripLoc"]);
  $tripBody = trim($_POST["tripBody"]);
  $tripFC = trim($_POST["tripFC"]);
  $tripBF = trim($_POST["tripBF"]);
  $tripDay = trim($_POST["tripDay"]);
  $tripTime = trim($_POST["tripTime"]);

  $postTitle = $tripTitle." at ".$tripLocation;
  $tripDate = $tripDay." ".$tripTime;

  $sql = ("INSERT INTO posts (dateandtime, username, title, body, tripDate, totalFish, bigFish) VALUES (?,?,?,?,?,?,?);");
  
  $date = getCurDate();

  $stmt = mysqli_prepare($link, $sql);
  mysqli_stmt_bind_param($stmt, "sssssis", $date, $_SESSION["username"], $postTitle, $tripBody, $tripDate, $tripFC, $tripBF);
  mysqli_stmt_execute($stmt);

  echo "Fishing trip at $tripLocation added";

  // header("location: ");
  
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
  <h2>New Post</h2>
  <p> Use the below app to get all information about your fishing location in one place!</p>
  <noscript>You need to enable JavaScript to view the full site.</noscript>

  <form id = "newUserTrip" method="post" action="<?php echo htmlspecialchars($_SERVER["PHP_SELF"]); ?>">
    <label><b>
      Fishing Trip Title: 
    </b></label></br></br>  
    <input type='text' name='tripTitle' id ='tTitle' placeholder='Trip Title' required>
    <br><br>

    <label><b>
      Trip Location 
    </b></label></br></br>
    <input type='text' name='tripLoc' id ='tLoc' placeholder='Trip Location' required>
    <?php
      $sqlq = "SELECT `favspots` FROM `users` WHERE `Username` = '".trim($_SESSION['username'])."';";
      $results = mysqli_query($link,$sqlq);
      $resultString = mysqli_fetch_row($results);
      $favSpots = str_getcsv($resultString[0]);
      unset($favSpots[array_search("",$favSpots)]);
      mysqli_free_result($results);
      ?>
    <br>
    
    <select name="favCurrents" method ="post" action="<?php echo htmlspecialchars($_SERVER["PHP_SELF"]); ?>">
      <option name='placeHolder'value='placeHolder'>Or select From Favourites</option>
    <?php 
    foreach($favSpots as $spot) {
      echo "<option name='option'value='$spot'>$spot</option>";
    }
    
    ?>
    </select>
    </br></br>

    <label><b>
      Fishing Trip Details:
    </b></label></br>
    <Label>Time and Date:</label><input type='date' name='tripDay' id ='tDay' required/><input type='time' name='tripTime' id ='tTime' required/>
    </br>
    <textarea rows='10' cols='50' name='tripBody' id ='tBody' placeholder='Enter details of your trip' required></textarea>
    </br></br>

    <label><b>
      Total Fish Caught:
    </b></label></br></br>
    <input type='number' name='tripFC' id ='tFC' placeholder='Total Number Caught' required></textarea>
    </br></br>

    <label><b>
      Big Fish Caught (one on each line):
    </b></label></br></br>
    <textarea rows='10' cols='50' name='tripBF' id ='tBF' placeholder='List all the big fish from your trip!'></textarea>
    </br></br>

    <input type='submit'/>
  </form>

  <a href = "usercontrol.php">Return to User homepage</a>

</body>
</html>

<?php

?>