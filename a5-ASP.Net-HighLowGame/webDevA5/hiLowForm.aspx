<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="hiLowForm.aspx.cs" Inherits="webDevA5.hiLowForm" %>

<!DOCTYPE html>

<%-- 
    
    /*
*  FILE          : hiLowForm.aspx
*  PROJECT       : PROG2000 - Assignment #5 - ASP.NET High-Low Game
*  PROGRAMMER    : Brendan Rushing & Josh Rogers
*  FIRST VERSION : 2018-11-13
*  DESCRIPTION   :
  DESCRIPTION   :   Hi-Lo game written with server-side written in ASP.NET
					Inputs are validated with ASP.NET server-side validation functions
					Forms are written in ASP HTML
                    The user is asked for a username and then a max number guess
                    The user then precedes to guess the random number and the range is shown.
                    The user is shown a message and the background is changed after the game is won.
                    The user can then play the game again by pressing the Play again button.              
*	
*/
    
    
    
--%>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <link rel="STYLESHEET" type="text/css" href="css/a5StyleSheet.css"/>


    <title>Brendan Rushing & Josh Roger's ASP.NET HIGH - LOW Game</title>

</head>
<body id="PageBody" runat="server">


    <form id="highLowForm" class="userForm" runat="server">

         <%-- Page Title Text --%>
        <asp:Label ID="PageTitle" runat="server"></asp:Label>

         <%-- User Heading Text --%>
        <asp:Label ID="UserHeading" runat="server"></asp:Label>
        
         <%-- User Prompt Text --%>
        <div class="userPrompt"> 
        <asp:Label ID="userSummaryText" class="userPrompt" runat="server"></asp:Label>
        </div>

         <%-- User text input box --%>
        <div class="inputBox">
        <asp:textbox id="userTextInput" required="true" runat="server" value="" 
            class="inputBox" Height="40px" autofocus="true"  ></asp:textbox>
            <br />
        </div>
        


         <%-- Submit button --%>
        <asp:button id="Submit1" type="submit" value="Submit" runat="server" class="button" Text="Submit" Height="40px" Width="169px"></asp:button>

            
        <%-- Regex validator for letters only --%>
        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" required="true"
            validateEmptyText="false" CssClass="errorMessage"
            runat="server" validationexpression ="^[a-zA-Z]+$" ControlToValidate="userTextInput"
            ErrorMessage="Name can only be letters"></asp:RegularExpressionValidator>

        <%-- Error if user tries to submit only whitespace --%>
        <div runat="server" id="blankEntryError"  class="errorMessage">Input field cannot be left blank</div>


    </form>
    </body>





</html>
