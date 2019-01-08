<!DOCTYPE html>

<!--
  FILE          : index.php
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

-->
<html>
<head>
<div class="headerTopBar">
<br>
<h1>Brendan & Trevor's JSON Text Editor Web Application</h1>
<!-- CSS FOR HTML -->
<link rel="STYLESHEET" type="text/css" href="css/JSONTextEditor.css">
<hr>
</head>
</div>
<body>

<!-- Form to open file -->
<TABLE border="0" width="80%">
  <TR>
    <!-- File Open Select -->
    <TD width="40%" align="right"><p class="textTitle" >Load Text File</p></TD>
    <TD width="2%">&nbsp;</TD>
    <TD width="38%" align="left">


    <SELECT class="jsonButton" required name="openFileName" id="openFileNameID">
      <OPTION value="">None Selected</OPTION>
    </SELECT>

    &nbsp;&nbsp;&nbsp;<input type="button" id="openButton" class="jsonButton" value="Open File">
    <br>

</TD>

  </TR>
</TABLE>
<!-- END OF Form to open file-->


<!-- INPUT FORM FOR FILE NAME AND CONTENTS-->
<TABLE border="0" width="80%">
  <TR>
      <!-- File Name text input -->
      <TD width="40%" align="right"><p class="textTitle" >File Name </p></TD>
      <TD width="2%">&nbsp;</TD>
      <TD width="38%" align="left"><input type="text" size="82" requried name="fileName" id="fileNameInputID"> </TD>
  </TR>
  <TR>
      <!-- Contents text input -->
      <TD width="40%" align="right"><p class="textTitle" >Text Contents</p></TD>
      <TD width="2%">&nbsp;</TD>
      <TD width="38%" align="left"><textarea name="fileContents" required id="fileContentsInputID" rows="20" cols="80" value=""></textarea> </TD>
  </TR>
  <TR>
      <!-- Submit Button -->
      <TD width="40%" align="right"></TD>
      <TD width="2%">&nbsp;</TD>
      <TD width="38%" align="left"><input type="submit" id="saveButton" class="jsonButton" value="Save File">
    
<div id="resultStatus">
</div></TD>
      
  </TR>
</TABLE>

<!-- error div for error messages -->
<div id="errorDiv"></div>

<!-- END OF INPUT FORM FOR FILE NAME AND CONTENTS-->



<!-- JQuery API -->
<script src="//ajax.googleapis.com/ajax/libs/jquery/1.10.0/jquery.min.js"></script>

<!-- Our Project Global.js script -->
<script src="global.js"></script>

</div>



<?php
  // if add new customer is pressed
  if ($_SERVER['REQUEST_METHOD'] == 'POST')
  {
      //create variables from post data
      $fileNameToOpen = $_POST['openFileName'];

      $fileOpenLocation = getcwd();       //get current working directory
      $fileOpenLocation .= "/MyFiles/";   //append MyFiles folder to the end
      $fileOpenLocation .= $fileNameToOpen;     //append fileName from form      

      echo "file to open: $fileOpenLocation ";


      //get input JSON file contents
      $inputRawFile = file_get_contents($fileOpenLocation);

      $inputJSON = json_decode($inputRawFile, true); // decode the JSON into an associative array

      echo '<pre>' . print_r($inputJSON, true) . '</pre>';

      
  }
?>


</body>
</html>