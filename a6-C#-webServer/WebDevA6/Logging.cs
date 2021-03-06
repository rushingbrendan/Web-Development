﻿/*
*  FILE          : Logging.cs
*  PROJECT       : INFO2180 - Software Quality - EMS Term Project
*  PROGRAMMER    : Brendan Rushing
*  FIRST VERSION : 2018-11-09
*  DESCRIPTION   :
*	This is the Logging class for the EMS Term Project
*	
*	This class creates a log folder where the .exe for the project is located.
*	
*	This class then creates a .log file for each day
*	
*	The public function "NewLogEvent" is used by all other classes to log events to the log file.
*	The function requires the eventdetails string to be passed.
*	The function uses StackFrame to find the className and methodName that the loggingEvent was called from.
*	
*/


//INCLUDE FILES
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using loggingClass;
using System.IO;
using System.Runtime.CompilerServices;
using System.Diagnostics;
//eo INCLUDE FILES


namespace loggingClass
{
    /// <summary>
    /// This is the Logging class for the EMS Term Project.
    /// This class creates a log folder where the .exe for the project is located.
    /// This class then creates a .log file for each day
    /// 
    /// The public function "NewLogEvent" is used by all other classes to log events to the log file.
    /// </summary>
    public static class Logging
    {



        /// <summary>
        /// This function builds a string for the current file name based on the date
        /// </summary>
        /// <returns>string: fileName in format: "ems.YYYY-MM-DD.log"
        /// </returns>
        static private string GetCurrentFileName()
        {
            string fileName = "WebServer";
            fileName = fileName + DateTime.Today.ToString("yyyy-MM-dd");
            fileName = fileName + ".log";

            return fileName;
        }


        /// <summary>
        /// This function builds a string for the current time for the event details line
        /// </summary>
        /// <returns>string: currentTime in format: "YYYY-MM-DD hh:mm:ss"</returns>
        static private string GetCurrentDateTimeForLoggingEvent()
        {
            string currentTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");

            return currentTime;
        }


        /// <summary>
        /// This function builds a string for the complete event details for the file line
        /// </summary>
        /// <param name="inputEventDetails"></param>
        /// <returns></returns>
        static private string BuildEventString(string inputEventDetails)
        {
            string eventString = "";

            //build eventString 
            eventString = GetCurrentDateTimeForLoggingEvent();
            eventString = eventString + " - " + inputEventDetails + Environment.NewLine;

            return eventString;
        }


        /// <summary>
        /// This function writes the event details string to the .log file
        /// </summary>
        /// <param name="inputEventDetails"></param>
        static public void NewLogEvent(string inputEventDetails)
        {
            string eventString = BuildEventString(inputEventDetails);   //build event string
            string fileName = GetCurrentFileName();                     //build file name string
            var directory = Directory.GetCurrentDirectory() + @"\log";    //get current directory

            try
            {   //attempt to write string to file until it is available

                if (!Directory.Exists(directory))  // if it doesn't exist, create
                {
                    Directory.CreateDirectory(directory);
                }

                // use Path.Combine to combine 2 strings to a path
                File.AppendAllText(Path.Combine(directory, fileName), eventString);

            }
            catch (Exception e)
            {
                //cant really do anything with the exception since this is the logging class
            }
        }
    }
}
