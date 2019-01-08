
<?php

/*
  FILE          : getFiles.php
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
    'result' => 0


);


    $result = array();

    if (isset($_POST['loadFiles']))
	{
		switch ($_POST['loadFiles'])
		{
			case 'yes':
				
				$dir = getcwd();
				$dir .= "/MyFiles/";
                $files = scandir($dir);
                
                unset($files[0], $files[1]);
                $files=array_values($files);
				
				$json['result'] = $files;
				$json['success'] = true;


				
				$result = $json;
				break;
				
			default:
               $json['success'] = false;
			   $json['result'] = 'file load error';
			   
			   $result = $json;
               break;
		}
			
			
		
	}
	
	
	
    echo json_encode($result);

?>