<?php

require_once("checklogin.php");

$postTitle = $postBody = "";

if($_SERVER["REQUEST_METHOD"] == "POST") {

  $postTitle = trim($_POST["postTitle"]);
  $postBody = trim($_POST["postBody"]);

  $sql = ("INSERT INTO posts (dateandtime, username, title, body) VALUES (?,?,?,?);");
  
  $date = getCurDate();

  $stmt = mysqli_prepare($link, $sql);
  mysqli_stmt_bind_param($stmt, "ssss", $date, $_SESSION["username"], $postTitle, $postBody);
  mysqli_stmt_execute($stmt);  
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


  <form id = "newUserPost" method="post" action="<?php echo htmlspecialchars($_SERVER["PHP_SELF"]); ?>">
    <label><b>
      Post Title : 
    </b></label></br></br>
    <input type='text' name='postTitle' id ='pTitle' placeholder='Post Title' required>
    <br><br>

    <label><b>
      Post Title : 
    </b></label></br></br>
    <textarea rows='10' cols='50' name='postBody' id ='pBody' placeholder='Enter your post here' required></textarea>
  </br></br>
    <input type='submit'/>
  </form>

  <a href = "usercontrol.php">Return to User homepage</a>

</body>
</html>

<?php

?>