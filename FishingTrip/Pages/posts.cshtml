<?php

require_once("checklogin.php");

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

  <div>
  <I>Posts by ____NEED TO ADD USERNAME HERE______</I>
  <?php

  $sqlcmd = "SELECT * FROM Posts WHERE 'username' = ".$_SESSION["username"]." ORDER BY dateandtime DESC";

  if ($result = mysqli_query($link, $sqlcmd)) {
    $rowCount = mysqli_num_rows($result);

    if ($rowCount > 0) {
      //Order results according to the posted date, latest first
      $pagenum = 1;

      while($row = mysqli_fetch_assoc($result)) {
        if($row["title"] != NULL) {

          echo "<h3>".$row["title"] . "</h3>Posted on " . $row["dateandtime"] . "<br>";

          if($row["tripDate"] != NULL) {
            echo "<I>A Fishing trip on " . $row["tripDate"] . ". Catching a total of ". $row["totalFish"]. " fish.</I><br>";
          }
          echo $row["body"] . "</br>";
          if($row["bigFish"] != NULL) {
            echo "<I>Sizeable fish were:</I><br>".$row["bigFish"] ."</br>";
            
          }
          echo "</br>";
        }

      }

    }
  

    mysqli_free_result($result);

  } else {

    die(mysqli_error($link));

  }

  mysqli_close($link);


  ?>
  </div> <br>
  <noscript>You need to enable JavaScript to view the full site.</noscript>
  <a href = "../index.php">Return to User homepage</a>
</body>
<br><br>
</html>


