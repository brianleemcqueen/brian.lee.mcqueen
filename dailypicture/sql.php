<?php
 include ("dailypictureheader.php");
?>

	<?php
		if(file_exists($picture_file)) {
			$databasename = "dailypicture";
			$username = "dailypicture";
			$password = "lemons91";
			$hostname = "mysql87.secureserver.net";
			$dbh = mysql_connect($hostname, $username, $password) or die("Unable to connect to MySQL");
			$selected = mysql_select_db($databasename,$dbh) or die("Could not select from database.  Contact us for help.");

			$sql = "SELECT title, comments, case when comments2 is null then '<br />' else comments2 end as comments2, comments_bonus FROM picture_comments WHERE date = '".$picture_date."'";
			
			
			$result = mysql_query($sql);
			//while ($row = mysql_fetch_array($result,MYSQL_ASSOC)) {
			$row = mysql_fetch_array($result,MYSQL_ASSOC);
			echo "<font size=\"3\">".$row{'title'}."</font><br />".$row{'comments'}."<br />".$row{'comments2'}."</p>";
			echo "<p align=\"center\"><img border=\"0\" src=\"",$picture_file,"\"></p>";

			if(file_exists($picture_file_bonus)) {
				echo "<p align=\"center\"><b><span style=\"background-color: #FFFF00\">BONUS PIC!!!!</span><br></p>";
			    echo "<p align=\"center\"><img border=\"0\" src=\"",$picture_file_bonus,"\"><br />",$row{'comments_bonus'},"</b></p>";
			}

			//}

			mysql_close($dbh);

</div>
</body>

</html>
