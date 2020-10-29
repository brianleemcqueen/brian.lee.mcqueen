<?php
 include ("dailypictureheader.php");
?>

	<ul>
		<li><a href="../index_amburg.html">Home</a></li>
		<li><a href="index.html">Daily Picture Home</a></li>
	</ul>
</div>

<div id="tree" style="border-style: solid; border-width: 0; padding-left: 4; padding-right: 4; padding-top: 1; padding-bottom: 1">
	Enter Daily Picture Information<br />

	<form id="topfive" name="topfive"   action="dailypicture_processor.php" method="post">
	<table border="10" cellpadding="5" bordercolor="blue">
	<tr>
	  <td align="center" style="color: #6666CC; font-family: 'Comic Sans MS',Arial"><big><b>Enter Daily Picture Information Here</b></big></td></tr>
	<tr><td style="color: #6666CC; font-family: 'Comic Sans MS',Arial">
	<table border="40"  bordercolorlight="yellow" bordercolordark="red" cellpadding="25" cellspacing="0">
		<tbody>
			<tr>
				<td style="color: #6666CC; font-family: 'Comic Sans MS',Arial">
				<table border="0" cellpadding="0" cellspacing="0">
					<tbody>
						<tr>
							<td align="center" style="color: #6666CC; font-family: 'Comic Sans MS',Arial">Date (yyyymmdd)</td>
							<td style="color: #6666CC; font-family: 'Comic Sans MS',Arial">&nbsp;</td>
							<td style="color: #6666CC; font-family: 'Comic Sans MS',Arial"><input type="text" id="dp_name" name="dp_name" size="55" maxlength="49"></td>
						</tr>
						<tr>
							<td style="color: #6666CC; font-family: 'Comic Sans MS',Arial">&nbsp;</td>
							<td style="color: #6666CC; font-family: 'Comic Sans MS',Arial">&nbsp;</td>
							<td style="color: #6666CC; font-family: 'Comic Sans MS',Arial">&nbsp;</td>
						</tr>
						<tr>
							<td align="center" style="color: #6666CC; font-family: 'Comic Sans MS',Arial">Title</td>
							<td style="color: #6666CC; font-family: 'Comic Sans MS',Arial">&nbsp;</td>
							<td style="color: #6666CC; font-family: 'Comic Sans MS',Arial"><input type="text" id="title" name="title" size="55"
								maxlength="49"></td>
						</tr>
						<tr>
							<td align="center" style="color: #6666CC; font-family: 'Comic Sans MS',Arial">Comments</td>
							<td style="color: #6666CC; font-family: 'Comic Sans MS',Arial">&nbsp;</td>
							<td style="color: #6666CC; font-family: 'Comic Sans MS',Arial"><input type="text" id="comments" name="comments" size="55"
								maxlength="149"></td>
						</tr>
						<tr>
							<td align="center" style="color: #6666CC; font-family: 'Comic Sans MS',Arial">Comments 2</td>
							<td style="color: #6666CC; font-family: 'Comic Sans MS',Arial">&nbsp;</td>
							<td style="color: #6666CC; font-family: 'Comic Sans MS',Arial"><input type="text" id="comments2" name="comments2" size="55"
								maxlength="149"></td>
						</tr>
						<tr>
							<td align="center" style="color: #6666CC; font-family: 'Comic Sans MS',Arial">Comments Bonus</td>
							<td style="color: #6666CC; font-family: 'Comic Sans MS',Arial">&nbsp;</td>
							<td style="color: #6666CC; font-family: 'Comic Sans MS',Arial"><input type="text" id="comments_bonus" name="comments_bonus" size="55"
								maxlength="149"></td>
						</tr>
						<tr>
							<td style="color: #6666CC; font-family: 'Comic Sans MS',Arial"></td>
							<td style="color: #6666CC; font-family: 'Comic Sans MS',Arial">&nbsp;</td>
							<td style="color: #6666CC; font-family: 'Comic Sans MS',Arial">
								<input type="submit" value="Submit This Entry" name="submit_dailypicture">
								<input type="reset" value="Reset The Form" name="reset_dailypicture">
							</td>
						</tr>
					</tbody>
				</table>
				</td>
			</tr>
		</tbody>
	</table>
	</td></tr>
	</table>
	</form>
	<p>&nbsp;</p>

	    <p>&nbsp;</p>
	    <p>&nbsp;
</div>

<br /><br /><br /><br />
<p /><p />

</body>

</html>