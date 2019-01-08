<!DOCTYPE html>

<!--
*  FILE          : animals.php
*  PROJECT       : PROG2000 - Web Development: Assignment #3
*  PROGRAMMER    : Brendan Rushing
*  FIRST VERSION : 2018-10-16
*  DESCRIPTION   : This assignment has 2 parts: php server-side and CGI executable server-side
*  
*   This project is the php server-side script
*   
*   This program is called after a html webform is submitted using GET method with the following:
*       1) username
*       2) favorite animal selection
*       
*   This program reads the variables from the url (GET method). 
*   
*   The program uses the user's favorite animal selection to grab the contents of a text file and
*   picture of the animal and puts the information in a table for the user.
*  
*   References:
*   //moose text: https://en.wikipedia.org/wiki/Moose
*   //lion text: https://en.wikipedia.org/wiki/Lion
*   //giraffe text: https://en.wikipedia.org/wiki/Giraffe
*   //beaver text: https://en.wikipedia.org/wiki/Beaver
*   //goose text: https://en.wikipedia.org/wiki/Goose
*   //anteater text: https://en.wikipedia.org/wiki/Anteater
*   
*   //moose pic: https://www.alaskawildlife.org/product/adopt-a-moose/
*   //lion pic: https://www.the-star.co.ke/news/2018/05/20/male-lion-nicknamed-lipstick-found-dead-in-maasai-mara_c1761108
*   //giraffe pic: https://animals.sandiegozoo.org/animals/giraffe
*   //beaver pic: https://en.wikipedia.org/wiki/Beaver
*   //goose pic: https://www.google.ca/url?sa=i&rct=j&q=&esrc=s&source=images&cd=&cad=rja&uact=8&ved=2ahUKEwizt8GNmIneAhUrp4MKHQ1VAfwQjRx6BAgBEAU&url=https%3A%2F%2Fwww.earthtouchnews.com%2Fwtf%2Fwtf%2Fgreen-woodpecker-tongues-are-so-long-they-wrap-around-their-skulls&psig=AOvVaw2T3ECBkfkDiTvFDYKIlqbo&ust=1539717949398602
*   //anteater pic: http://mentalfloss.com/article/64660/15-majestic-facts-about-anteater
*/
-->

<html lang="en" xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="utf-8" />
    <head><title>Animal Serve Website</title></head>
</head>
<body>



</body>
<h3>Welcome to the Animal Serve Website</h3>

<?php 
	$userName = $_GET['name'];	//get username from url
	$animalSelection = $_GET['animalSelection'];	//get animal selection from url
	$animalDescription = "theZoo/" . $animalSelection . ".txt";	//build link to txt file from animal name


	if (($userName == '') or ($animalSelection =='')){		//check if either variable is empty
		
		echo "<p>ERROR - URL string is invalid</p>";
		echo "</html>";
	}
?>





<h4><?php echo $userName ?>, Here is an image and a description of <?php echo $animalSelection ?></h4>

<table border='1'>
	<tr>	
		<td width='40%' align='center'><b>Animal Image</b></td>
		<td width='50%' align='center'><b>Animal Description</b></td>
	</tr>
	<tr>
         <td width='40%' align='center'><img src='theZoo/<?php echo $animalSelection ?>.png' alt='{0}'</td>	<!-- display image -->
         <td width='50%' align='left'> 
		 <?php
$myfile = fopen($animalDescription, "r") or die("Unable to open file!");	//use php to empty text file of animal into table
echo fread($myfile,filesize($animalDescription));
fclose($myfile);
?>


</td>
	</tr>

</table>


</html>