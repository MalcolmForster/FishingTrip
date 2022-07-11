<?php

require_once("checklogin.php");

if($_SERVER['REQUEST_METHOD'] == "POST") {
  
  //Request a friendship
  if(isset($_POST['submit'])) {

      $accUser = trim($_POST["friendRequest"]);
      $intUser = trim($_SESSION["username"]);

      if($accUser != $intUser) {

        $sqlChk = "SELECT username FROM users WHERE username = ?;";

        if($chkstmt = mysqli_prepare($link, $sqlChk)) {
          mysqli_stmt_bind_param($chkstmt,"s", $param_accUser);
          $param_accUser = $accUser;

          if(mysqli_stmt_execute($chkstmt)) {
            mysqli_stmt_store_result($chkstmt);
            if(mysqli_stmt_num_rows($chkstmt) == 1) {
              //Check if already have a friend request
              $sqlChkFnd = "SELECT intUser,accUser FROM relationships WHERE intUser = ? AND accUser = ?;";
              $Astmt = mysqli_stmt_num_rows(mysqli($link, $sqlChkFnd, array($param_accUser,$intUser)));
              $Bstmt = mysqli_stmt_num_rows(mysqli($link, $sqlChkFnd, array($intUser,$param_accUser)));

              //mysqli_stmt_attr_get(

              if (($Astmt || $Bstmt) == 0) {

                $sqlInsert = "INSERT INTO relationships (intDate, intUser, accUser) VALUES (?,?,?);";            
                date_default_timezone_set('Pacific/Auckland');
                $date = date("d-m-y h:i:s");
                $stmt = mysqli_prepare($link, $sqlInsert);
                mysqli_stmt_bind_param($stmt, "sss", $date, $intUser, $accUser);
                mysqli_stmt_execute($stmt);  
                echo "You have added " . $accUser;
                mysqli_stmt_close($stmt);    

              } else {
                echo "Already friends or already sent friend request";
              }
            } else {
              echo "Friend not found :(";
            }
          }
        }
      } else {
        echo "Don't be sad";
      }
  

  } else {  
    $postnames = array_keys($_POST);

    //Change the friendship status    
    foreach ($postnames as $posts) {
      $addFnd = 0;
      $intFriend = substr($posts, 3);

      if (strpos($posts, 'acc') == 0) {
        $addFnd = 1;
        echo "You added " . $intFriend;
      } elseif (strpos($posts, 'dec') == 0) {
        echo "You declined " . $intFriend;
      }      

      $chgFndState = "UPDATE relationships SET accDate = ?, friends = ? WHERE intUser = \"".$intFriend."\" AND accUser = \"".$_SESSION["username"]."\";";

      $curDate = getCurDate();

      $fndState = mysqli_prepare($link, $chgFndState);
      mysqli_stmt_bind_param($fndState, 'si', $curDate, $addFnd);
      mysqli_stmt_execute($fndState);
      mysqli_stmt_close($fndState);
    }
  }  
}
?>

<form name="requests" method= "POST" action="<?php echo htmlspecialchars($_SERVER["PHP_SELF"]);?>">
<?php

$user = trim($_SESSION["username"]);
$fndReqs = "SELECT accUser, intUser, intDate FROM relationships WHERE accUser = ? AND accDate IS NULL";
$stmt = mysqli($link, $fndReqs, array($user));

if(mysqli_stmt_num_rows($stmt) > 0) {
  echo "You have friend requests from:</br>";
  mysqli_stmt_bind_result($stmt, $accUser, $intUser, $intDate);
  while(mysqli_stmt_fetch($stmt)) {
    echo $intUser . " on " . $intDate." <button name=\"acc".$intUser."\">Accept</button> <button name=\"dec".$intUser."\">Decline</button></br>";
  }
} else {
  echo "No friend requests";
}
mysqli_stmt_close($stmt);
?></form>


<!DOCTYPE html>

<html lang="en">

<head>
  <meta charset="UTF-8">
  <meta http-equiv="X-UA-Compatible" content="IE=edge">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <title>Fishr</title>
  <link rel="stylesheet" href="../main.css">
  <!-- <script defer src="../login.js"></script> -->
</head>

<body class="light-theme">
  <h2>Friends</h2>
  <p>

    <?php    
    $fndFriends = "SELECT accUser, intUser FROM relationships WHERE (intUser = ? OR accUser = ?) AND friends = 1;";
    $stmt = mysqli($link, $fndFriends, array($_SESSION["username"],$_SESSION["username"]));

    if(mysqli_stmt_num_rows($stmt) > 0) {
      mysqli_stmt_bind_result($stmt, $accUser, $intUser);
      while(mysqli_stmt_fetch($stmt)) {
        echo str_ireplace($_SESSION["username"],"",$intUser.$accUser)."</br>"; 
      }
    } else {
      echo "No friends found";
    }
    mysqli_stmt_close($stmt); 
    ?>

    </p>
  <noscript>You need to enable JavaScript to view the full site.</noscript>

  <form name="addFriend" method= "POST" action="<?php echo htmlspecialchars($_SERVER["PHP_SELF"]); ?>">
    <h3>Add new friend below:</h3>
    <input type= "text" name ="friendRequest" placeholder="Insert friend's name"/>
    <input type= "submit" name ='submit'/>   
  </form>
  
  <a href = "blocks.php">Blocks</a>  
  <a href = "usercontrol.php">Return to User homepage</a>
</body>
<br><br>
</html>