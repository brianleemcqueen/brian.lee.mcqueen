<?php
 include ("dailypictureheader.php");
?>

	<ul>
		<li><a href="../index_amburg.html">Home</a></li>
		<li><a href="index.html">Daily Picture Home</a></li>
	</ul>
</div>

<div id="tree" style="border-style: solid; border-width: 0; padding-left: 4; padding-right: 4; padding-top: 1; padding-bottom: 1">
<!-- process the insert -->
	<?php
		$databasename = "dailypicture";
		$username = "dailypicture";
		$password = "lemons91";
		$hostname = "mysql87.secureserver.net";
		$dbh = mysql_connect($hostname, $username, $password) or die("Unable to connect to MySQL");
		$selected = mysql_select_db($databasename,$dbh) or die("Could not select from database.  Contact us for help.");

		mysql_query("delete from picture_comments where date='$dp_name'");
	
	

	if (mysql_query("insert into picture_comments values('$dp_name', '$title', '$comments', '$comments2', '$comments_bonus')")) {
	  print "Info Entered.  <a href='dailypicture.php'>click here to see the most recent daily picture</a>";
	}
	else {
		  print "Failed to insert record";
	}
	mysql_close($dbh);
?>

    <p>&nbsp;</div>

<br /><br /><br /><br />
<p /><p />

</body>

</html>