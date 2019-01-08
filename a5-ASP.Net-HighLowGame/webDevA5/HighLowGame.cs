/*
*  FILE          : HighLowGame.cs
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
//eo INCLUDES


namespace webDevA5
{
    //Class Name: HighLowGame
    //Purpose: To store data for the HighLowGame
    [Serializable]
    public class HighLowGame
    {

        //VARIABLES
        public string UserName { get; set; }
        public int MinNumber { get; set; }
        public int MaxNumber { get; set; }
        public int CorrectNumber { get; set; }


        /*!
        *  FUNCTION : HighLowGame
        *  
        *  TYPE: Public
        *  
        *  DESCRIPTION : This function is the default constructor for the HighLowGame class
        *
        *  PARAMETERS : none
        *
        *  RETURNS : none
        */
        public HighLowGame()
        {
            this.MinNumber = 1; //set min number to 1
            this.MaxNumber = 0;
            this.CorrectNumber = 0;
            this.UserName = "";
        }


        /*!
        *  FUNCTION : SetCorrectNumber
        *  
        *  TYPE: Public
        *  
        *  DESCRIPTION : This function is called to set the correct random number after the maxNumber is set
        *
        *  PARAMETERS : none
        *
        *  RETURNS : none
        */
        public void SetCorrectNumber()
        {
           
            //GET RANDOM NUMBER BETWEEN 1 AND MAXNUMBER
            Random r = new Random();
            this.CorrectNumber = r.Next(1, this.MaxNumber);

        }


    }
}