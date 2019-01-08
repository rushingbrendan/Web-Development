/*
*  FILE          : animals.exe
*  PROJECT       : PROG2000 - Web Development: Assignment #3
*  PROGRAMMER    : Brendan Rushing
*  FIRST VERSION : 2018-10-16
*  DESCRIPTION   : This assignment has 2 parts: php server-side and CGI executable server-side
*  
*   This project is the CGI executable server-side. 
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

//INCLUDES
using System;
using System.IO;
using System.Collections;
using System.Text;
using System.Collections.Specialized;
//eo INCLUDES


namespace CGIServerApplication

{

    /// <summary>
    /// Name: CGIServer
    /// Purpose:  to output html page with description and image of users favorite animal
    /// </summary>
    

    class CGIServer

    {
        //animal names available
        public static readonly string[] animalNames = { "Anteater", "Giraffe", "Goose", "Lion", "Moose", "Beaver" };


        [STAThread] //single thread

        static void Main(string[] args)

        {
            
            string userName = "";
            string animalSelection = "";
            bool animalIsValid = false;
            bool nameIsValid = false;

            //get query string from url
            string QueryString = Environment.GetEnvironmentVariable("QUERY_STRING");

            Console.WriteLine("Content-type:text/html\n\n");
            Console.WriteLine("<head><title>Animal Serve Website</title></head>\n");
            Console.WriteLine("<body>\n");
            Console.WriteLine("<h3>Welcome to the Animal Serve Website</h3>\n");
            

            //split query string into seperate strings by =
            string[] values = QueryString.Split('=');

            //split the strings by cutting off & symbol
            values[0] = values[0].Split('&')[0];
            values[1] = values[1].Split('&')[0];
            values[2] = values[2].Split('&')[0];

            //assign to varaibles
            userName = values[1];
            animalSelection = values[2];

            //loop through animalNames to make sure animal selected is valid
            foreach (string currentAnimal in animalNames)
            {
                if (currentAnimal == animalSelection)
                {
                    animalIsValid = true;
                }
            }

            //check to see if name is not empty string
            if (userName.Length != 0)
            {
                nameIsValid = true;
                Console.WriteLine("<h4>{0}, Here is an image and a description of {1}</h4>\n", userName,animalSelection);
            }

            //if variables are valid then ...
            if (animalIsValid && nameIsValid)
            {
                string animalDescription = "";

                //string with location of animal description text file
                string animalDescriptionLocation = "theZoo/";
                animalDescriptionLocation = animalDescriptionLocation + animalSelection;
                animalDescriptionLocation = animalDescriptionLocation + ".txt";
                try
                {   // Open the text file using a stream reader.
                    using (StreamReader sr = new StreamReader(animalDescriptionLocation))
                    {
                        // Read the stream to a string
                        animalDescription = sr.ReadToEnd();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("<p>ERROR - The image could not be read:</p>");

                }

                //table with image and description of animal
                Console.WriteLine("<table border='1'>");
                Console.WriteLine("<tr>");
                Console.WriteLine("<td width='30%' align='center'><b>Animal Image</b></td>");
                Console.WriteLine("<td width='40%' align='center'><b>Animal Description</b></td>");
                Console.WriteLine("</tr>");

                Console.WriteLine("<tr>");
                Console.WriteLine("<td width='30%' align='center'><img src='theZoo/{0}.png' alt='{0}'</td>",animalSelection);
                Console.WriteLine("<td width='40%' align='left'>{0}</td>",animalDescription);
                Console.WriteLine("</tr>");      

                Console.WriteLine("</table>");



            }
            else
            {
                //variables were invalid so print "invalid data"
                Console.WriteLine("<p>Invalid Data!</p>");
            }

            Console.WriteLine("</body></html>");
        }

    }

}