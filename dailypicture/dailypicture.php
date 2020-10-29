<?php
 include ("dailypictureheader.php");
?>

<?php
 $mostrecentpicture=false;
 $picture_date = $_GET['dp_date'];
 if($picture_date == 0) {
 	$mostrecentpicture=true;
 	$picture_date = date(Ymd);
 }
 $yyyy = substr($picture_date, 0,4);
 $mm = substr($picture_date, 4,2);
 $dd = substr($picture_date, 6,2);

 $picture_file = 'images'.$yyyy.'/'. (string)$picture_date. '.JPG';
 if($mostrecentpicture) {
  	if (! file_exists($picture_file)) { //if the current date's picture is not published, change the date to yesterday
  		$date_to_display  = mktime(0, 0, 0, date("m"), date("d")-1, date("Y"));
		$picture_date = date(Ymd, $date_to_display);
 		$picture_file = 'images'.$yyyy.'/'. (string)$picture_date. '.JPG';
		$yyyy = substr($picture_date, 0,4);
		$mm = substr($picture_date, 4,2);
		$dd = substr($picture_date, 6,2);

		if (! file_exists($picture_file)) { //if yesterday's picture is not published, change the date to the day before yesterday
			$date_to_display  = mktime(0, 0, 0, date("m"), date("d")-2, date("Y"));
			$picture_date = date(Ymd, $date_to_display);
 			$picture_file = 'images'.$yyyy.'/'. (string)$picture_date. '.JPG';
			$yyyy = substr($picture_date, 0,4);
			$mm = substr($picture_date, 4,2);
			$dd = substr($picture_date, 6,2);

			if (! file_exists($picture_file)) { //if the day before yesterday's picture is not published, change the date to the day before that
				$date_to_display  = mktime(0, 0, 0, date("m"), date("d")-3, date("Y"));
				$picture_date = date(Ymd, $date_to_display);
				$picture_file = 'images'.$yyyy.'/'. (string)$picture_date. '.JPG';
				$yyyy = substr($picture_date, 0,4);
				$mm = substr($picture_date, 4,2);
				$dd = substr($picture_date, 6,2);


				if (! file_exists($picture_file)) { //if the day before the day before yesterday is not published, change the date to 1-1-2006
					$picture_date = 20060101;
					$picture_file = 'images'.$yyyy.'/'. (string)$picture_date. '.JPG';
					$yyyy = substr($picture_date, 0,4);
					$mm = substr($picture_date, 4,2);
					$dd = substr($picture_date, 6,2);
				}
			}
		}
 	}
 }

 $picture_file_date_yyyymmdd = date("Ymd", mktime(0, 0, 0, $mm, $dd, $yyyy));
 $nextImageYear = $yyyy;
 if($mm==12 && $dd==31) {
 	$nextImageYear = $yyyy+1;
 }
 $picture_file_next = 'images'.$nextImageYear.'/'. (String)(date("Ymd", mktime(0, 0, 0, $mm, $dd+1, $yyyy))). '.JPG';
 $picture_file_previous = 'images'.$yyyy.'/'. (String)(date("Ymd", mktime(0, 0, 0, $mm, $dd-1, $yyyy))). '.JPG';
 $picture_file_bonus = 'images'.$yyyy.'/'. (string)$picture_date. '_bonus.JPG';
 $tomorrow_date = (String)(date("Ymd", mktime(0, 0, 0, $mm, $dd+1, $yyyy)));
 $yesterday_date =(String)(date("Ymd", mktime(0, 0, 0, $mm, $dd-1, $yyyy)));
?>
    <br>
&nbsp;</p>
	<ul>
	<?php
		//if there is a picture for the next day, create a Next link.  Else a blank row.
		if (file_exists($picture_file_next)) {
   			echo "<li><a href=\"dailypicture.php?dp_date=",($tomorrow_date),"\">Next</a></li>";
		} else {
   			echo "<br />";
		}

		//if the picture date is January 1, 2006, link to 12-31-2005 picture page for previous.
		//Else create a PHP link to the previous page.
		if ($picture_date==20060101) {
				echo "<li><a href=\"/dailypicture/2005/12-dec/20051231_JPG.html\">Previous</a></li>";
		} else {
				echo "<li><a href=\"dailypicture.php?dp_date=",($yesterday_date),"\">Previous</a></li>";
		}

	?>
		<!--the calendar link is created as yyyy_mm.html based on the picture date being requested -->
		<!--<li><a href="<? echo (string)$yyyy,'/',(string)$yyyy, '_', (string)$mm; ?>.html">Calendar</a><br>&nbsp;</li>-->
		<li><a href="<? echo "calendar.php?currentYear=".(string)$yyyy."&currentMonth=".(string)$mm; ?>">Calendar</a><br>&nbsp;</li>
		<li><a href="index.html">Daily Picture Home</a><br>&nbsp;</li>
	</ul>
    </div>

<div id="tree">
<p align="center">
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

        } else {
        	echo "<font size=\"3\">This date's picture has not yet been published</font><br></p>";
        	echo "<p align=\"center\"><img border=\"0\" src=\"nopicture.JPG\"></p>";
        }
    ?>
</div>
</body>

</html>
