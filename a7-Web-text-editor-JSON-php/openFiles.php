
<?php

/*
  FILE          : openFiles.php
  PROJECT       : PROG2000 - Web Development
  PROGRAMMER    : Brendan Rushing & Trevor Allain
  FIRST VERSION : 2018-12-05
  DESCRIPTION   :   

  - This web app uses PHP for serverside communication
  - The web page is built with HTML and Javascript

This application is a text web application

    - Capture the data using Javascript
    - Use the JQuery AJAX method to pass it to our php page
    - Return a JSON encoded string

*/


    header('Content-Type: application/json');
	
	$json = array(

    'success' => false,
    'result' => 0,
    'fileName' =>0


);



    $result = array();

    if (isset($_POST['openFileName']))
	{
        $fileNameToOpen = $_POST['openFileName'];
        $fileOpenLocation = getcwd();       //get current working directory
        $fileOpenLocation .= "/MyFiles/";   //append MyFiles folder to the end
        $fileOpenLocation .= $fileNameToOpen;     //append fileName from form      
  
        
        //get input JSON file contents
        $inputRawFile = file_get_contents($fileOpenLocation);
        $json['success'] = true;
        $json['result'] = $inputRawFile;
        $json['fileName'] = $fileNameToOpen;

        echo json_encode($json);
		}								
?>