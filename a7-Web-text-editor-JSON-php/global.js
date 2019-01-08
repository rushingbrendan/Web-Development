/*

  FILE          : global.js
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




//
// FUNCTION NAME: $('#saveButton').click(function() 
// 
// - This function is called when the save button is clicked
// - the text file is saved on the server
//
$('#openButton').click(function() {


    var that = $(this),
    //serialize contents
    contents = that.serialize();

    $.ajax({
            url: 'openFiles.php',
            dataType: 'json',
            type:'post',
            data: {openFileName: $('#openFileNameID').find(':selected').text()},            
            success:function(data){
                if(data.success){


                    //set file name and text box contents
                    document.getElementById('fileContentsInputID').value = data.result;
                    document.getElementById('fileNameInputID').value = data.fileName;

                }
            }
    });
    return false;

});

//
// FUNCTION NAME: $('#saveButton').click(function() 
// 
// - This function is called when the save button is clicked
// - the text file is saved on the server
//

$('#saveButton').click(function() {

    var that = $(this),

    //serialize contents
    contents = that.serialize();

    $.ajax({
            url: 'add.php',
            dataType: 'json',
            type:'post',
            data: {fileName: document.getElementById('fileNameInputID').value, fileContents: document.getElementById('fileContentsInputID').value},
            success:function(data){
                if(data.success == true){

                    //set html elements with data
                    document.getElementById('resultStatus').innerHTML = "File has been saved.";
                }
                else{
                    document.getElementById('resultStatus').innerHTML = "File must have contents to be saved.";
                }
                
            }
    });

    return false;

});

$(document).ready(function()
{	
	 $.ajax({
            url: 'getFiles.php',
            dataType: 'json',
            type:'post',
            data: {loadFiles: 'yes'},
            success:function(data){
                if(data.success){
                    
                    //add all file names found to list in HTML select
					for (var i = 0; i < data.result.length; i++)
					{
						 $("#openFileNameID").append("<option value='"+data.result[i]+"'>"+data.result[i]+"</option");
					}

                }
            }

    });
	
	
	
	return false;
});
