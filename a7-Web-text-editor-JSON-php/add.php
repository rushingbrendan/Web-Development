<?php

/*
  FILE          : add.php
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

//CHANGE HEADER TYPE SO THAT WE ARE OUTPUTTING CORRECT CONTENT TYPE
header('Content-type: text/javascript');

//guilty before (false)
$json = array(

    'success' => false,
    'fileName' => 0,
    'fileContents' => 0

);
    //proven innocent (true)
if(isset($_POST['fileName'], $_POST['fileContents'])){

    $fileName       =   $_POST['fileName'];    
    $fileContents   =   $_POST['fileContents'];

    //set success to true -- override inital value of false
    $json['success'] = true;



    $json['fileName'] = $fileName;    
    $json['fileContents'] = $fileContents;

}
else
{
    $json['success'] = false;
}

$fileSaveLocation = getcwd();       //get current working directory
$fileSaveLocation .= "/MyFiles/";   //append MyFiles folder to the end
$fileSaveLocation .= $fileName;     //append fileName from form
$fileSaveLocation .= ".txt";       //append .JSON file type


echo json_encode($json);

$json_data = json_encode($json);
file_put_contents($fileSaveLocation, $fileContents);


?>


