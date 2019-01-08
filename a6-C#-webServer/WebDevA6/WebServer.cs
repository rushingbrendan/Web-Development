/*
*  FILE          : WebServer.cs
*  PROJECT       : PROG2000 - Web Development: Assignment #6 - Web Server
*  PROGRAMMER    : Brendan Rushing
*  FIRST VERSION : 2018-11-18
*  DESCRIPTION   :
        This project is a single threaded web server.

        Command line arguments:
            1)  -WebRoot    root folder for the website data
            2)  -WebIP      IP Address of the server
            3)  -webPort    Port Number that the server is listening to
	
*/



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text.RegularExpressions;
using loggingClass;
using System.IO;



namespace WebDevA6
{
    /// <summary>
    /// WebServer class
    /// </summary>
    class WebServer
    {
        string webRoot { get; set; }
        string webIP { get; set; }
        string webPort { get; set; }
        string localFolder { get; set; }
        string htmlResponse { get; set; }
        string htmlOutput { get; set; }
        string fileType { get; set; }
        

        TcpListener server;
        TcpClient client;

        const int EXIT_FAILURE = -1;
        const string TEXT_FILE = ".txt";
        const string HTML_FILE = ".html";
        const string JPEG_FILE = ".jpeg";
        const string JPG_FILE = ".jpg";
        const string GIF_FILE = ".gif";


        const string TEXT_MIME_TYPE = "text/plain";
        const string HTML_MIME_TYPE = "text/html;";
        const string GIF_MIME_TYPE = "image/gif";
        const string JPG_MIME_TYPE = "image/jpeg";


        const string CHAR_SET = "charset=UTF-8";

        const string OK_RESPONSE = "HTTP/1.1 200 OK";
        const string NOT_FOUND_REPONSE = "HTTP/1.1 404 NOT FOUND";
        const string BAD_REQUEST_REPONSE = "HTTP/1.1 400 BAD REQUEST";
        const string UNSUPPORTED_MEDIA_TYPE_REPONSE = "HTTP/1.1 415 UNSUPPORTED MEDIA TYPE";



        static int Main(string[] args)
        {
            
            while (true)
            {
                WebServer myWebServer = new WebServer();

                //PARSE COMMAND LINE ARGUMENTS
                if (!myWebServer.ParseCommandLineArguments(args))
                {
                    //exit program - invalid command line arguments
                    return EXIT_FAILURE;
                }

                //CREATE AND START TCP LISTENER
                if (!myWebServer.CreateAndStartTCPListener())
                {
                    //exit program - could not create and start TCP Listener
                    return EXIT_FAILURE;
                }

                //ACCEPT NEW CLIENT CONNECTION
                myWebServer.AcceptTCPClient();


                //GET MESSAGE FROM CLIENT CONNECTION
                if (!myWebServer.ReadHTMLResponseMessage())
                {
                    //exit program - could not read HTML Response message
                    return EXIT_FAILURE;
                }

                //PERFORM CLIENT REQUEST
                if (!myWebServer.PerformClientRequest())
                {
                    //exit program - could not send HTML message
                    return EXIT_FAILURE;
                    
                }

                //CLOSE WEB SERVER
                if (!myWebServer.CloseWebServer())
                {
                    //exit program - could not close server properly
                    return EXIT_FAILURE;
                }
            }             
        }


        /// <summary>
        /// This method parses the command line arguments and saves them to the class variables
        /// </summary>
        /// <param name="args"></param>
        bool ParseCommandLineArguments(string[] args)
        {
            //PARSE COMMAND LINE ARGUMENTS
            try
            {
                int counter = 0;
                foreach (string arguments in args)
                {
                    string[] parsedArguments = arguments.Split('=');    //split command line arguments by =
                    counter++;  //increment counter

                    switch (counter)
                    {
                        case 1:
                            this.webRoot = parsedArguments[1];
                            break;
                        case 2:
                            this.webIP = parsedArguments[1];
                            break;
                        case 3:
                            this.webPort = parsedArguments[1];
                            break;
                        default:
                            throw new Exception("too many arguments");
                    }
                }
            }
            catch (Exception e)
            {
                Logging.NewLogEvent("Error - invalid command line arguments");
                return false;
            }

            return true;
        }

        /// <summary>
        /// This method creates and starts a TCP listener
        ///     - uses port and IP from class variables
        /// </summary>
        /// <param name="inputWebServer"></param>
        /// <returns></returns>
        bool CreateAndStartTCPListener()
        {
            try
            {
                //create TCPListener class object
                this.server = new TcpListener(System.Net.IPAddress.Loopback, 
                                        Convert.ToInt32(this.webPort));

                //start
                server.Start();
            }
            catch (Exception)
            {
                Logging.NewLogEvent($"Error - Could not start TCP Listener. IP:{this.webIP} " +
                    $"Port:{this.webPort} ");

                return false;
            }
            return true;
        }

        /// <summary>
        /// This method accepts a new TCP client connection
        /// </summary>
        void AcceptTCPClient()
        {
            this.client = this.server.AcceptTcpClient();        
        }

        /// <summary>
        /// This method reads the HTML Response message from Client
        /// </summary>
        /// <returns></returns>
        bool ReadHTMLResponseMessage()
        {
            string inputBuffer = "";

            try
            {
                //use stream reader to read all input data from client
                System.IO.StreamReader reader = new System.IO.StreamReader(client.GetStream());
                while (!reader.EndOfStream)
                {
                    string inputLine = reader.ReadLine();
                    inputBuffer = inputBuffer + inputLine;
                    
                    
                    if (inputLine.Contains("GET"))
                    {
                        //get requested file from client
                        string[] lines = inputLine.Split('/');                        
                        this.localFolder = lines[1];
                        string[] finalLines = lines[1].Split(' ');
                        this.localFolder = finalLines[0];
                        this.localFolder = this.webRoot + "\\" + localFolder;

                        //get requested file type from client
                        string[] fileTypeSplit = this.localFolder.Split('.');
                        this.fileType = "." + fileTypeSplit[1];
                    }

                    if (inputLine == "")
                    {
                        break;
                    }
                    //convert to lower case
                    this.fileType = this.fileType.ToLower();

                }
            }
            catch (Exception)
            {
                Logging.NewLogEvent("Error - could not read message from client");
                return false;
            }

            return true;
        }
        

        /// <summary>
        /// This method performs the client request
        ///     - requested file at file path is read and sent to the client
        /// </summary>
        /// <returns></returns>
        bool PerformClientRequest()
        {
            try
            {
                //SEND 415 FILE TYPE UNSUPPORTED RESPONSE
                if (!CheckIfFileTypeSupported())
                {
                    Send415Response();
                }
                //SEND 404 FILE NOT FOUND RESPONSE
                else if (!CheckIfFileExists())
                {
                    Send404Response();
                }
                //SEND HTML FILE
                else if (this.fileType == HTML_FILE)
                {
                    SendHTMLMessage();
                }
                //SEND TEXT FILE
                else if (this.fileType == TEXT_FILE)
                {
                    SendTextMessage();
                }
                //SEND JPG IMAGE FILE
                else if ((this.fileType == JPEG_FILE) || (this.fileType == JPG_FILE))
                {
                    SendJpegImage();
                }
                //SEND GIF IMAGE FILE
                else if (this.fileType == GIF_FILE)
                {
                    SendGifImage();
                }
                //SEND 500 ERROR
                else
                {
                    Send500Response();
                }
                

            }
            catch (Exception)
            {
                Logging.NewLogEvent("Error - Could not send requested message to client");
                Send500Response();  //send server error message to client
                return false;
            }
            return true;
        }


        /// <summary>
        /// This method closes the web server
        ///     - Client is closed
        ///     - Server is stopped
        /// </summary>
        /// <returns></returns>
        bool CloseWebServer()
        {
            try
            {
                this.client.Close();    //close client
                this.server.Stop();     //stop server
            }
            catch (Exception)
            {
                Logging.NewLogEvent("Error - Could not close webserver properly");
                return false;
            }
            return true;
        }


        /// <summary>
        /// This method sends a 404 NOT FOUND response to client
        /// </summary>
        void Send404Response()
        {
            string htmlBody = "";

            //get 404 html file
            using (StreamReader streamReader = new StreamReader(@"C:/localWebSite/404.html", Encoding.UTF8))
            {
                htmlBody = streamReader.ReadToEnd();
            }

            this.htmlOutput = NOT_FOUND_REPONSE + Environment.NewLine;
            this.htmlOutput += HTML_MIME_TYPE;
            this.htmlOutput += CHAR_SET + Environment.NewLine;
            this.htmlOutput += ("Content-Length: " + htmlBody.Length);
            this.htmlOutput += htmlBody;
            System.IO.StreamWriter Writer = new System.IO.StreamWriter(client.GetStream());
            Writer.Write(htmlOutput);
            Writer.Flush();

            Logging.NewLogEvent(Environment.NewLine + this.htmlOutput);
        }


        /// <summary>
        /// This method sends a 415 UNSUPPORTED MEDIA TYPE response to the client
        ///     - This server supports the following file types:
        ///     - HTML
        ///     - txt
        ///     - jpeg
        ///     - gif
        /// </summary>
        void Send415Response()
        {
            string htmlBody = "";

            this.htmlOutput = UNSUPPORTED_MEDIA_TYPE_REPONSE + Environment.NewLine;
            this.htmlOutput += HTML_MIME_TYPE;
            this.htmlOutput += CHAR_SET + Environment.NewLine;
            this.htmlOutput += ("Content-Length: " + htmlBody.Length);
            this.htmlOutput += htmlBody;
            System.IO.StreamWriter Writer = new System.IO.StreamWriter(client.GetStream());
            Writer.Write(htmlOutput);
            Writer.Flush();

            Logging.NewLogEvent(Environment.NewLine+  this.htmlOutput);    //log send
        }

        /// <summary>
        /// This method sends a 500 SERVER INTERNAL ERROR response to the client
        /// </summary>
        void Send500Response()
        {
            string htmlBody = "";

            //get 404 html file
            //using (StreamReader streamReader = new StreamReader(@"C:/localWebSite/404.html", Encoding.UTF8))
            //{
            //    htmlBody = streamReader.ReadToEnd();
            //}

            //this.htmlOutput = SERVER + Environment.NewLine;
            this.htmlOutput += HTML_MIME_TYPE;
            this.htmlOutput += CHAR_SET + Environment.NewLine;
            this.htmlOutput += ("Content-Length: " + htmlBody.Length);
            this.htmlOutput += htmlBody;
            System.IO.StreamWriter Writer = new System.IO.StreamWriter(client.GetStream());
            Writer.Write(htmlOutput);
            Writer.Flush();

            Logging.NewLogEvent(Environment.NewLine + this.htmlOutput);    //log send
        }

        /// <summary>
        /// This method sends an HTML message to the client
        /// </summary>
        void SendHTMLMessage()
        {
            string htmlHeader = "";
            //READ FILE CONTENTS FROM REQUESTED PATH
            using (StreamReader streamReader = new StreamReader(this.localFolder, Encoding.UTF8))
            {
                this.htmlOutput = streamReader.ReadToEnd();
            }
            System.IO.StreamWriter writer = new System.IO.StreamWriter(client.GetStream());

            htmlHeader = (OK_RESPONSE);
            htmlHeader += (Environment.NewLine);
            htmlHeader += (HTML_MIME_TYPE + CHAR_SET + Environment.NewLine);
            htmlHeader += ("Content-Length: " + this.htmlOutput.Length);
            htmlHeader += (Environment.NewLine);
            htmlHeader += (Environment.NewLine);
            writer.Write(this.htmlOutput);
            writer.Flush();

            Logging.NewLogEvent(Environment.NewLine + htmlHeader);    //log send
        }

        /// <summary>
        /// This method sends a Text message to the client
        /// </summary>
        void SendTextMessage()
        {
            string htmlBody = "";
            //READ FILE CONTENTS FROM REQUESTED PATH
            using (StreamReader streamReader = new StreamReader(this.localFolder, Encoding.UTF8))
            {
                this.htmlOutput = streamReader.ReadToEnd();
            }
            System.IO.StreamWriter writer = new System.IO.StreamWriter(client.GetStream());

            htmlBody = (OK_RESPONSE);
            htmlBody += (Environment.NewLine);
            htmlBody += (TEXT_MIME_TYPE + CHAR_SET + Environment.NewLine);
            htmlBody += ("Content-Length: " + this.htmlOutput.Length);
            htmlBody += (Environment.NewLine);
            htmlBody += (Environment.NewLine);
            writer.Write(htmlBody);
            writer.Write(this.htmlOutput);
            writer.Flush();


            Logging.NewLogEvent(htmlBody); //log send
        }

        /// <summary>
        /// This method sends a jpeg image to the client
        /// </summary>
        void SendJpegImage()
        {

            string outputBuffer = "";
            byte[] imageBytes = File.ReadAllBytes(this.localFolder);

            System.IO.BinaryWriter writer = new System.IO.BinaryWriter(client.GetStream());

            outputBuffer = (OK_RESPONSE + Environment.NewLine);
            outputBuffer += ("Content-Type: " + JPG_MIME_TYPE + Environment.NewLine);
            outputBuffer += ("Content-Length: " + imageBytes.Length);
            outputBuffer += (Environment.NewLine);
            outputBuffer += (Environment.NewLine);

            writer.Write(outputBuffer);
            writer.Write(imageBytes);
            writer.Flush();

            Logging.NewLogEvent(outputBuffer);    //log send
        }

        /// <summary>
        /// This method sends a gif image to the client
        /// </summary>
        void SendGifImage()
        {
            string outputBuffer = "";
            byte[] imageBytes = File.ReadAllBytes(this.localFolder);

            System.IO.BinaryWriter writer = new System.IO.BinaryWriter(client.GetStream());

            outputBuffer = (OK_RESPONSE + Environment.NewLine);
            outputBuffer += ("Content-Type: " + GIF_MIME_TYPE + Environment.NewLine);
            outputBuffer += ("Content-Length: " + imageBytes.Length);
            outputBuffer += (Environment.NewLine);
            outputBuffer += (Environment.NewLine);

            writer.Write(outputBuffer);
            writer.Write(imageBytes);
            writer.Flush();

            Logging.NewLogEvent(outputBuffer);    //log send

        }

        /// <summary>
        /// This method checks if the requested file exists on the server
        /// </summary>
        bool CheckIfFileExists()
        {
            switch (this.fileType)
            {
                //ONLY CHECK SUPPORTED FILE TYPES
                case TEXT_FILE:
                case HTML_FILE:
                case JPEG_FILE:
                case JPG_FILE:
                case GIF_FILE:

                    //CHECK TO SEE IF FILE EXISTS
                    if (File.Exists(this.localFolder))
                    {
                        //YES - RETURN TRUE
                        return true;
                    }
                    break;
                default:
                    return false;
            }
            return false;
        }


        /// <summary>
        /// This method checks if the requested file type is supported by the server
        ///     - HTML
        ///     - txt
        ///     - jpeg
        ///     - gif
        /// </summary>
        bool CheckIfFileTypeSupported()
        {
            switch (this.fileType)
            {
                //ONLY CHECK SUPPORTED FILE TYPES
                case TEXT_FILE:
                case HTML_FILE:
                case JPEG_FILE:
                case JPG_FILE:
                case GIF_FILE:
                    return true;
                    
                default:
                    return false;
            }            
        }
    }
}

