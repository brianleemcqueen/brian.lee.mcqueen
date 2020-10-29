<?php
 include ("dailypictureheader.php");
?>
	<ul>
		<li><a href="../index_amburg.html">MT Home</a></li>
		<li><a href="index.html">DP Home</a></li>
<?php

	if(! is_numeric($currentMonth) || ! is_numeric($currentYear) || ! checkdate ( $currentMonth, 1, $currentYear)) {
		$currentMonth = date(m);
		$currentYear = date(Y);
	}
	echo	"<li><a href='2007/index.html'>2007</a></li>";
	echo	"<li><a href='2006/index.html'>2006</a></li>";
	echo	"<li><a href='2005/index.html'>2005</a><br>&nbsp;</li>";
//	echo	"<li class='currentpagecolor'><span class='currentpagecolor'>January</span></li>";
	echo "<li><a href='calendar.php?currentYear=".$currentYear."&currentMonth=1'>January</a></li>";
	echo "<li><a href='calendar.php?currentYear=".$currentYear."&currentMonth=2'>February</a></li>";
	echo "<li><a href='calendar.php?currentYear=".$currentYear."&currentMonth=3'>March</a></li>";
	echo "<li><a href='calendar.php?currentYear=".$currentYear."&currentMonth=4'>April</a></li>";
	echo "<li><a href='calendar.php?currentYear=".$currentYear."&currentMonth=5'>May</a></li>";
	echo "<li><a href='calendar.php?currentYear=".$currentYear."&currentMonth=6'>June</a></li>";
	echo "<li><a href='calendar.php?currentYear=".$currentYear."&currentMonth=7'>July</a></li>";
	echo "<li><a href='calendar.php?currentYear=".$currentYear."&currentMonth=8'>August</a></li>";
	echo "<li><a href='calendar.php?currentYear=".$currentYear."&currentMonth=9'>September</a></li>";
	echo "<li><a href='calendar.php?currentYear=".$currentYear."&currentMonth=10'>October</a></li>";
	echo "<li><a href='calendar.php?currentYear=".$currentYear."&currentMonth=11'>November</a></li>";
	echo "<li><a href='calendar.php?currentYear=".$currentYear."&currentMonth=12'>December</a></li>";


	$databasename = "dailypicture";
	$username = "dailypicture";
	$password = "lemons91";
	$hostname = "mysql87.secureserver.net";
	$dbh = mysql_connect($hostname, $username, $password) or die("Unable to connect to MySQL");
	$selected = mysql_select_db($databasename,$dbh) or die("Could not select from database.  Contact us for help.");

	$sql = "SELECT date, title FROM `picture_comments` WHERE year(date)=".$currentYear." AND month(date) = ".$currentMonth." ORDER BY date ";
	$sql2 = "SELECT mmdd, event FROM `events` WHERE (yyyy='*' OR yyyy='".$currentYear."') AND mmdd LIKE ('".date("m", mktime(0, 0, 0, $currentMonth, 1, $currentYear))."%') ORDER BY mmdd";
	//echo $sql2;
	$titles = array(0 => 0);
	$events = array(0 => 0);

	$result = mysql_query($sql);
	while ($row = mysql_fetch_array($result,MYSQL_ASSOC)) {
		$titles[]=$row{'title'};
	}

	$result = mysql_query($sql2);
	$current_mmdd = "";
	$previous_mmdd = "";
	$previous_event = "";
	$current_event = "";
	while ($row = mysql_fetch_array($result,MYSQL_ASSOC)) {
		$current_mmdd=$row{'mmdd'};
		$current_event=$row{'event'};
		if($current_mmdd == $previous_mmdd) {
			$events[$current_mmdd] = $previous_event."/".$current_event;
		} else {
			$events[$current_mmdd]=$current_event;
		}
		$previous_event = $current_event;
		$previous_mmdd = $current_mmdd;

	}

	mysql_close($dbh);


?>
	</ul>
</div>

<div id="tree" style="width: 630; height: 83">
<center>
<table style="font-family: Comic Sans MS; border-collapse: collapse;" bordercolor="#111111" cellspacing="0" cellpadding="0" width="650" bgcolor="#fad605" border="1">
	<tr>
		<td valign="top" align="right" bgcolor="#ffffff" colspan="7">
			<p align="center"><b><font size="5"><?php echo date("F Y", mktime(0, 0, 0, $currentMonth, 1, $currentYear)); ?></font></b></p>
		</td>
	</tr>
	<tr>
		<td valign="top" width="100" align="center" bgcolor="#FFFFFF">Sun</td>
		<td valign="top" width="100" align="center" bgcolor="#FFFFFF">Mon</td>
		<td valign="top" width="100" align="center" bgcolor="#FFFFFF">Tue</td>
		<td valign="top" width="100" align="center" bgcolor="#FFFFFF">Wed</td>
		<td valign="top" width="100" align="center" bgcolor="#FFFFFF">Thu</td>
		<td valign="top" width="100" align="center" bgcolor="#FFFFFF">Fri</td>
		<td valign="top" width="100" align="center" width="14%" bgcolor="#FFFFFF">Sat</td>
	</tr>
<?php
	//for help with the date function go here:  http://phpbuilder.com/manual/en/function.date.php

	$previousMonth = $currentMonth - 1;
	$previousYear = $currentYear;
	if($currentMonth == 1) {
		$previousYear = $currentYear-1;
		$previousMonth = 12;
	}

	$nextMonth = $currentMonth + 1;
	$nextYear = $currentYear;
	if($currentMonth == 12) {
		$nextMonth = 1;
		$nextYear = $currentYear + 1;
	}

	$firstDay_yyyymmdd = date("Ymd", mktime(0, 0, 0, $currentMonth, 1, $currentYear)); //first day of the month in yyyymmdd format
	$firstDay_text = date("D", mktime(0, 0, 0, $currentMonth, 1, $currentYear)); //day of the week - Sun thru Sat
	$daysInMonth = date("t", mktime(0, 0, 0, $currentMonth, 1, $currentYear));  //number of days in the month
	$daysInPreviousMonth = date("t", mktime(0, 0, 0, $previousMonth, 1, $previousYear)); //number of days in the previous month

	//The number of days to put on the calendar prior to the first of the month
	$leadDays = 0;
	if($firstDay_text=="Mon") {
		$leadDays = 1;
	} else if($firstDay_text=="Tue") {
		$leadDays = 2;
	} else if($firstDay_text=="Wed") {
		$leadDays = 3;
	} else if($firstDay_text=="Thu") {
		$leadDays = 4;
	} else if($firstDay_text=="Fri") {
		$leadDays = 5;
	} else if($firstDay_text=="Sat") {
		$leadDays = 6;
	}

	$postDays = 0; //number of days in the next month to process (value set at the end of the current month for loop)

	echo "<tr>";
	$dayCounter = 0;


	if($leadDays != 0) {
		//build any days from the previous month
		$firstDate = $daysInPreviousMonth-($leadDays-1);

		for ( $previousDays = $firstDate; $previousDays <= $daysInPreviousMonth; $previousDays++) {
			$title = "";  //decided not to print a title for future months.  If we change our mind, this is the variable;
			$dpDate = date("Ymd", mktime(0, 0, 0, $previousMonth, $previousDays, $previousYear)); //day to build - in yyyymmdd format

			echo "<td valign='top' bgcolor='#ffffff' align='right' width='14%'>".$previousDays;
			echo 	"<table  width='100'  cellspacing='2' align='center' border='0'>";
			echo			"<tr><td valign='middle' align='center' height='96'>";

			if(file_exists("images".$previousYear."/".$dpDate."_BW.gif")) {
				echo			"<a href='dailypicture.php?dp_date=".$dpDate."'>";
				echo			"<img border='0' src='images".$previousYear."/".$dpDate."_BW.gif'></a>";
				echo			"</td></tr><tr><td valign='middle' align='center'>";
				echo			"<font size='2'>".$title."</font><br>";
			} else {
				echo			"&nbsp;</td></tr><tr><td>&nbsp;";
			}
			echo			"</td></tr>";
			echo		"</table>";
			echo	"</td>";
		} //end previous days for loop
		$dayCounter = $leadDays;
	} //end previous days if statement
	$loopCounter=0;
	for ( $days = 1; $days <= $daysInMonth; $days++) {
		$loopCounter++;
		if($dayCounter == 7) {
			//if 7 days have been created, start a new row (a.k.a. week)
			echo "</tr><tr>";
			$dayCounter = 0;
		}

		$title = "Happy Birthday";
		$dpDate = date("Ymd", mktime(0, 0, 0, $currentMonth, $days, $currentYear)); //day to build - in yyyymmdd format
		echo "<td valign='top' align='right' width='14%'><font size='1'>".$events[date("md", mktime(0, 0, 0, $currentMonth, $days, $currentYear))]."</font>".$days;
		echo 	"<table  width='100'  cellspacing='2' align='center' border='0'>";
		echo			"<tr><td valign='middle' align='center' height='96'>";

		if(file_exists("images".$currentYear."/".$dpDate.".gif")) {
			echo			"<a href='dailypicture.php?dp_date=".$dpDate."'>";
			echo			"<img border='0' src='images".$currentYear."/".$dpDate.".gif'></a>";
			echo			"</td></tr><tr><td valign='middle' align='center'>";
			echo			"<font size='2'>".$titles[$loopCounter]."</font><br>";
		} else {
			echo			"&nbsp;</td></tr><tr><td>&nbsp;";
		}
		echo			"</td></tr>";
		echo		"</table>";
		echo	"</td>";

		$dayCounter ++;

		if($days == $daysInMonth) {
			//calculate number of days for next month to process
			$postDays = 7-$dayCounter;
		}

	} //end calendar build for loop


	//build the days for the next month
	if($postDays > 0) {

		for ( $nextMonthDays = 1; $nextMonthDays <= $postDays; $nextMonthDays++) {
			$title = "";  //decided not to print a title for future months.  If we change our mind, this is the variable;
			$dpDate = date("Ymd", mktime(0, 0, 0, $nextMonth, $nextMonthDays, $nextYear)); //day to build - in yyyymmdd format

			echo "<td valign='top' bgcolor='#ffffff' align='right' width='14%'>".$nextMonthDays;
			echo 	"<table  width='100'  cellspacing='2' align='center' border='0'>";
			echo			"<tr><td valign='middle' align='center' height='96'>";

			if(file_exists("images".$nextYear."/".$dpDate."_BW.gif")) {
				echo			"<a href='dailypicture.php?dp_date=".$dpDate."'>";
				echo			"<img border='0' src='images".$nextYear."/".$dpDate."_BW.gif'></a>";
				echo			"</td></tr><tr><td valign='middle' align='center'>";
				echo			"<font size='2'>".$title."</font><br>";
			} else {
				echo			"&nbsp;</td></tr><tr><td>&nbsp;";
			}
			echo			"</td></tr>";
			echo		"</table>";
			echo	"</td>";
		} //end next month days for loop

	}// end if($postDays > 0) {

?>

</tr>

</table>
</center>
<p align="center">&nbsp;</p>
</div>
</body>

</html>