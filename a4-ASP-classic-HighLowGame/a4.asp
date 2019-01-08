<!DOCTYPE html>
<!-- 
  FILE          : a04.asp
  PROJECT       : PROG2000 - Web Development: Assignment #4 - Hi-Low game in ASP
  PROGRAMMER    : Brendan Rushing
  FIRST VERSION : 2018-11-01
  DESCRIPTION   :   Hi-Lo game written with server-side written in ASP
					Inputs are validated in javascript
					Forms are written in HTML
                    The user is asked for a username and then a max number guess
                    The user then precedes to guess the random number and the range is shown.
                    The user is shown a message and the background is changed after the game is won.
                    The user can then play the game again by pressing the Play again button.
-->

<html lang="en" xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="utf-8" />
    <title>WDD - Hi-Lo Game with ASP server-side</title>

	<link rel="STYLESHEET" type="text/css" href="css/a4StyleSheet.css">

	<script type="text/javascript" src="js/validateInput.js"></script>

	<h1>Guess The Secret Number!</h1>


</head>
<body>
<%
' ASP VARIABLES:
'  - maxNumber - user provides max number for hi-low game
'  - minNumber - min number for hi-low game is 1
'  - correctNumber - this number is randomized based on the user given max number
'  - userName - this name is provided by the user
dim maxNumber
dim correctNumber
dim currentGuess
dim minNumber
dim userName

maxNumber=0
correctNumber=0
currentGuess=0
minNumber=0
userName=""

'REQUEST ASP VARIABLES FROM ENVIROMENT VARIABLES PROVIDED BY FORM POST
maxNumber=Request("maxNumber")
currentGuess=Request("currentGuess")
correctNumber=Request("correctNumber")
minNumber=Request("minNumber")
userName=Request("userName")

'cast variables to int
maxNumber = CInt(maxNumber)
minNumber = CInt(minNumber)
currentGuess = CInt(currentGuess)
correctNumber = CInt(correctNumber)


'change max and min numbers if guess is within ranges
if (currentGuess <> 0) then
if (currentGuess < maxNumber) and (currentGuess > correctNumber) then
maxNumber=currentGuess
elseif (currentGuess > minNumber) and (currentGuess < correctNumber) then
minNumber=currentGuess
end if
end if


'calculate correct number with randomize if max number is given
if ((correctNumber = 0) and (maxNumber <> 0)) then
Randomize
correctNumber=(Int((maxNumber-2+1)*Rnd+2))	'random number between 2 and max number
end if

'user did not guess correct number, tell them
if ((currentGuess <> correctNumber) and (currentGuess <> 0))then
response.Write("<p class='wrongGuess'> YOU DID NOT GUESS THE CORRECT NUMBER!!! </p>")
end if

%>



<%
'STAGE #1 
' userName has not been given, prompt the user to enter it
'reset background color
if (userName = "") then
%>
<body style="background-color:white">
</body>

<form name="hiLoForm" action="a4.asp" method="post" onsubmit="return validateLetters(userName.value);">
<p class="userPrompt">Enter Username: <input type="text" required id="userInput" name="userName" placeholder="" size="20"  />
<input type="text" id="minNumberID" name="minNumber" placeholder="" size="20" value="1" style="display:none" /> 
    &nbsp;&nbsp;&nbsp;<input class="button" type="submit" value="Submit"  /></p>
    <br/>
        <p class="errorMessage" id="inputError"  ><i>&nbsp</i></p>
</form>



<% 
'STAGE #2
' maxNumber has not been given, prompt the user to enter it
elseif (maxNumber = 0) then
%>
<p class="userPrompt"><%=userName%>, </p>
<form name="hiLoForm" action="a4.asp" method="post" onsubmit="return validateNumbers(maxNumber.value);">
<p class="userPrompt"> Enter Maximum Number: <input type="text" required id="userInput" name="maxNumber" placeholder="" size="20"  />
<input type="text" id="userNameID" name="userName" placeholder="" size="20" value="<%=userName%>" style="display:none" />
<input type="text" id="minNumberID" name="minNumber" placeholder="" size="20" value="1" style="display:none" />
    &nbsp;&nbsp;&nbsp;<input class="button" type="submit" value="Submit"  /></p>
    <br/>
        <p class="errorMessage" id="inputError"  ><i>&nbsp</i></p>
</form>


<%
'STAGE #3
'Prompt user for guess
elseif (currentGuess <> correctNumber) then
%>

<p class="number"><%=minNumber%> to <%=maxNumber%></p>
<p class="userPrompt"><%=userName%>, </p>
<form name="hiLoForm" action="a4.asp" method="post" onsubmit="return validateNumbers(currentGuess.value);">
<p class="userPrompt">
Enter Guess: <input type="text" id="userInput" required name="currentGuess" placeholder="" size="20"  />
<input type="text" id="maxNumberID" name="maxNumber" placeholder="" size="20" value="<%=maxNumber%>" style="display:none" />
<input type="text" id="minNumberID" name="minNumber" placeholder="" size="20" value="<%=minNumber%>" style="display:none" />
<input type="text" id="userNameID" name="userName" placeholder="" size="20" value="<%=userName%>" style="display:none" />
<input type="text" id="correctGuessID" name="correctNumber" placeholder="" size="20" value="<%=correctNumber%>" style="display:none" />
    &nbsp;&nbsp;&nbsp;<input class="button" type="submit" value="Submit"  /></p>
    <br/>
    <div class='errorMessage' id="inputError" ><i>&nbsp</i></div>
</form>

<%
'STAGE #4
'User has guessed correct number
'change background color
'clear variables
'post for again with username saved
elseif (currentGuess = correctNumber) then 
correctNumber=0
currentGuess=0
maxNumber=0
minNumber=0
%>
<h1>YOU WIN! YOU GUESSED THE RIGHT NUMBER!</h1>
<body style="background-color:yellow;">
</body>

<form name="hiLoForm" action="a4.asp" method="post">
<input type="text" id="userInput" name="currentGuess" placeholder="" size="20" value="<%=currentGuess%>" style="display:none" />
<input type="text" id="maxNumberID" name="maxNumber" placeholder="" size="20" value="<%=maxNumber%>" style="display:none" />
<input type="text" id="minNumberID" name="minNumber" placeholder="" size="20" value="<%=minNumber%>" style="display:none" />
<input type="text" id="userNameID" name="userName" placeholder="" size="20" value="<%=userName%>" style="display:none" />
<input type="text" id="correctGuessID" name="correctNumber" placeholder="" size="20" value="<%=correctNumber%>" style="display:none" />
    &nbsp;&nbsp;&nbsp;<input class="winningButton" type="submit" value="Play Again" onsubmit="submit" />
    <br/>
</form>



<%
end if
%>

</body>
</html>