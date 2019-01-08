/*
*  FILE          : hiLowForm.aspx.cs
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

//INCLUDES
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
//eo INCLUDES


namespace webDevA5
{
    //Class Name: hiLowForm
    //Purpose: To build the page elements for the form page
    public partial class hiLowForm : System.Web.UI.Page
    {

        /*!
        *  FUNCTION : Page_Init
        *  
        *  TYPE: Protected
        *  
        *  DESCRIPTION : This function is called to initialize the page
        *
        *  PARAMETERS : object sender, EventArgs e
        *
        *  RETURNS : none
        */
        protected void Page_Init(object sender, EventArgs e)
        {

        }


        /*!
        *  FUNCTION : Page_Load
        *  
        *  TYPE: Protected
        *  
        *  DESCRIPTION : This function is called after the page is initialized to load the items for the page
        *
        *  PARAMETERS : object sender, EventArgs e
        *
        *  RETURNS : none
        */
        protected void Page_Load(object sender, EventArgs e)
        {
            PageTitle.Text = "<h1>Brendan & Josh's High - Low Game </h1>";
            Submit1.Text = "Submit";
            userTextInput.Style.Add("display", "block");

            blankEntryError.Visible = false;


            System.Text.StringBuilder displayValues = new System.Text.StringBuilder();
            System.Collections.Specialized.NameValueCollection postedValues = Request.Form;

            string currentPostedData = "";

            //STEP THROUGH ALL OF THE POSTED KEYS
            String nextKey;
            for (int i = 0; i < postedValues.AllKeys.Length; i++)
            {
                nextKey = postedValues.AllKeys[i];
                if (nextKey.Substring(0, 2) != "__")
                {
                    displayValues.Append(postedValues[i]);
                    //we want the 5th item which is the first key after the enviroment variables
                    currentPostedData = postedValues[5];
                }
            }


            //GET HIGHLOWGAME DATA CLASS FROM VIEWSTATE
            var HighLowGameUserData = (HighLowGame)ViewState["UserData"];


            if ((HighLowGameUserData != null))
            {
                //USERNAME HAS NOT BEEN ENTERED
                if (HighLowGameUserData.UserName == "")
                {
                    //Check if the submitted info would be blank if spaces were removed
                    if ((HighLowGameUserData.UserName = currentPostedData.ToString().Replace(" ", string.Empty)) != "")
                    {

                        //RESET BACKGROUND COLOR                    
                        PageBody.Attributes.Add("style", "background-color: grey");

                        //SAVE USERNAME TO USERDATA


                        //PROMPT THE USER
                        UserHeading.Text = $"<h1>Hello {HighLowGameUserData.UserName}, </h1>";
                        userSummaryText.Text = "<h3>Please enter your max number:</h3>";


                        //CLEAR THE INPUT TEXT BOX
                        userTextInput.Text = "";

                        //CHANGE VALIDATORS
                        RegularExpressionValidator1.ValidationExpression = "^[0-9]*$";
                        RegularExpressionValidator1.ErrorMessage = "You can only enter numbers";


                    } else
                    {
                        //Show an error if only whitespace was submitted
                        blankEntryError.Visible = true;
                    }
                }
                //MAX NUMBER HAS NOT BEEN ENTERED
                else if (HighLowGameUserData.MaxNumber == 0)
                {
                    //Remove all spaces from user input
                    var tempNum = currentPostedData.ToString().Replace(" ", string.Empty);

                    if (tempNum != "") {
                        //SAVE MAX NUMBER TO USERDATA
                        HighLowGameUserData.MaxNumber = Convert.ToInt32(tempNum);

                        //SET CORRECT NUMBER
                        HighLowGameUserData.SetCorrectNumber();

                        //PROMPT THE USER
                        UserHeading.Text = $"<h1>Hello {HighLowGameUserData.UserName}, </h1>";
                        userSummaryText.Text = $"<p class='number'>{HighLowGameUserData.MinNumber} - {HighLowGameUserData.MaxNumber}" +
                            $" <br> <h3>Please enter your guess:</p>";


                        //CLEAR THE INPUT TEXT BOX
                        userTextInput.Text = "";
                    }
                    else
                    {
                        //Show an error if only whitespace was submitted
                        blankEntryError.Visible = true;
                    }

                }
                //CHECK CURRENT GUESS
                else if ((HighLowGameUserData.CorrectNumber != 0) && (HighLowGameUserData.MaxNumber != 0))
                {
                    //Remove all spaces from user input
                    var tempNum = currentPostedData.ToString().Replace(" ", string.Empty);
                    if (tempNum != "")
                    {
                        int CurrentGuess = Convert.ToInt32(currentPostedData.ToString());

                        //IF USER DID NOT GUESS CORRECT NUMBER
                        if (CurrentGuess != HighLowGameUserData.CorrectNumber)
                        {
                            //ADJUST MIN AND MAX NUMBERS 
                            if ((CurrentGuess < HighLowGameUserData.MaxNumber) && (CurrentGuess > HighLowGameUserData.CorrectNumber))
                            {
                                HighLowGameUserData.MaxNumber = CurrentGuess;
                            }
                            else if ((CurrentGuess > HighLowGameUserData.MinNumber) && (CurrentGuess < HighLowGameUserData.CorrectNumber))
                            {
                                HighLowGameUserData.MinNumber = CurrentGuess;
                            }

                            //PROMPT THE USER
                            UserHeading.Text = $"<h1>Hello {HighLowGameUserData.UserName}, </h1>";
                            userSummaryText.Text = $"<p class='number'>{HighLowGameUserData.MinNumber} - {HighLowGameUserData.MaxNumber}" +
                                $" <br> <h3>Please enter your guess:</p>";


                            //CLEAR THE INPUT TEXT BOX
                            userTextInput.Text = "";
                        }
                        //USER GUESSED THE CORRECT NUMBER
                        else
                        {
                            //CHANGE BUTTON TEXT
                            Submit1.Text = "Play Again";

                            //PROMPT THE USER
                            UserHeading.Text = $"<h1>Hello {HighLowGameUserData.UserName}, </h1>";
                            userSummaryText.Text = $"<h2>You Win!! You guessed the number!! <br>";

                            //CHANGE BACKGROUND COLOR          
                            PageBody.Attributes.Add("style", "background-color: #808000");


                            userTextInput.Text = HighLowGameUserData.UserName;
                            userTextInput.Style.Add("display", "none");


                            //RESET VALUES FOR NEW GAME
                            HighLowGameUserData.MinNumber = 1;
                            HighLowGameUserData.MaxNumber = 0;
                            HighLowGameUserData.UserName = "";

                            //RESET REGEX
                            RegularExpressionValidator1.ValidationExpression = "^[a-zA-Z ]+$";

                        }
                    }
                    else
                    {
                        //Show an error if only whitespace was submitted
                        blankEntryError.Visible = true;
                    }
                }


                //SAVE CLASS TO VIEWSTATE
                ViewState["UserData"] = HighLowGameUserData;

            }





            //IS THIS THE FIRST TIME WEBPAGE HAS BEEN OPENED?
            if (IsPostBack == false)
            {
                //YES - MAKE CLASS
                HighLowGame myHighLow = new HighLowGame();

                //SAVE CLASS TO VIEWSTATE
                ViewState["UserData"] = myHighLow;

                //PROMPT USER FOR FIRST TIME
                UserHeading.Text = "<h1>Hello</h1>";
                userSummaryText.Text = "<h3>Please enter your name:</h3>";
                

            }

        }


        /*!
        *  FUNCTION : Button1_Click
        *  
        *  TYPE: Protected
        *  
        *  DESCRIPTION : This function is called when the pbutton is clicked
        *
        *  PARAMETERS : object sender, EventArgs e
        *
        *  RETURNS : none
        */
        protected void Button1_Click(object sender, EventArgs e)
        {


        }

    }
}