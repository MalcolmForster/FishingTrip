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

<body class="light-theme">
  <h2>Fishing History</h2>
  <p>Display and sort your past fishing trips and show off your successes!</p><br>
  <div>
  <h3>Past Fishing Trips</h3>
  <?php

  $sqlcmd = "SELECT * FROM `posts` WHERE `username` = '" . trim($_SESSION["username"]. "' AND `tripDate` IS NOT NULL");

  if ($result = mysqli_query($link, $sqlcmd)) {
    $rowCount = mysqli_num_rows($result);
    echo "<h4>Recorded Fishing Days: " . $rowCount . "</h4>";
    if ($rowCount > 0) {
      while($row = mysqli_fetch_assoc($result)) {
        echo $row["title"] . " at " . $row["dateandtime"] . " gave " . $row["totalFish"] . " fish caught including " . $row["bigFish"] . ".<br>Other notes: " . $row["body"] . "<br><br>";
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
  <a href = "usercontrol.php">Return to User homepage</a>
</body>
<br><br>
</html>


