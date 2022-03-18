using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace HTTPServer
{
    class Server
    {
        Socket serverSocket;

        public Server(int portNumber, string redirectionMatrixPath)
        {
            //TODO: call this.LoadRedirectionRules passing redirectionMatrixPath to it
            //TODO: initialize this.serverSocket
            this.LoadRedirectionRules(redirectionMatrixPath);
            IPEndPoint ipEnd = new IPEndPoint(IPAddress.Any, portNumber);
            this.serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(ipEnd);
        }

        public void StartServer()
        {
            // TODO: Listen to connections, with large backlog.
            // TODO: Accept connections in while loop and start a thread for each connection on function "Handle Connection"
            this.serverSocket.Listen(100);
            while (true)
            {
                Socket client = serverSocket.Accept();
                
                Console.WriteLine("New client accepted : {0}", client.RemoteEndPoint);
                Thread new_thread = new Thread(new ParameterizedThreadStart(HandleConnection));
                new_thread.Start(client);
                //TODO: accept connections and start thread for each accepted connection.

            }
        }

        public void HandleConnection(object obj)
        {
            // TODO: Create client socket 
            // set client socket ReceiveTimeout = 0 to indicate an infinite time-out period

            // TODO: receive requests in while true until remote client closes the socket.


            Socket client_socket = (Socket)obj;
            client_socket.ReceiveTimeout = 0;

            while (true)
            {
                try
                {
                    // TODO: Receive request

                    // TODO: break the while loop if receivedLen==0

                    // TODO: Create a Request object using received request string

                    // TODO: Call HandleRequest Method that returns the response

                    // TODO: Send Response back to client

                    byte[] data = new byte[1024];
                    int recieved_length = client_socket.Receive(data);
                    if (recieved_length == 0)
                    {
                        break;
                    }
                    string x = Encoding.ASCII.GetString(data, 0, recieved_length);
                    Request request = new Request(x);
                    Response response = this.HandleRequest(request);
                    client_socket.Send(Encoding.ASCII.GetBytes(response.ResponseString));
                }
                catch (Exception ex)
                {
                    // TODO: log exception using Logger class
                    Logger.LogException(ex);
                }
            }
            client_socket.Close();
            // TODO: close client socket
        }
        Response HandleRequest(Request request)
        {
            string content;
            try
            {
                //TODO: check for bad request 

                //TODO: map the relativeURI in request to get the physical path of the resource.

                //TODO: check for redirect

                //TODO: check file exists

                //TODO: read the physical file

                // Create OK response
                if (!request.ParseRequest())
                {
                    content = LoadDefaultPage(Configuration.BadRequestDefaultPageName);
                    return new Response(StatusCode.BadRequest, "html", content, "");
                }

                //TODO: map the relativeURI in request to get the physical path of the resource.
                string phyPath = Configuration.RootPath + "\\" + request.relativeURI;

                //TODO: check for redirect
                string redirectionPath = GetRedirectionPagePathIFExist(request.relativeURI);//new path
                if (!String.IsNullOrEmpty(redirectionPath))//redirection
                {
                    //content = LoadDefaultPage(Configuration.RedirectionDefaultPageName);
                    phyPath = Configuration.RootPath + "\\" + redirectionPath;
                    content = File.ReadAllText(phyPath);
                    return new Response(StatusCode.Redirect, "html", content, redirectionPath);
                }

                //TODO: check file exists
                bool exist = File.Exists(phyPath);
                if (!exist)
                {
                    content = LoadDefaultPage(Configuration.NotFoundDefaultPageName);
                    return new Response(StatusCode.NotFound, "html", content, "");
                }

                //TODO: read the physical file
                content = File.ReadAllText(phyPath);

                // Create OK response
                return new Response(StatusCode.OK, "html", content, "");
            }
            catch (Exception ex)
            {
                // TODO: log exception using Logger class
                // TODO: in case of exception, return Internal Server Error.
                // TODO: log exception using Logger class
                Logger.LogException(ex);
                // TODO: in case of exception, return Internal Server Error. 
                content = LoadDefaultPage(Configuration.InternalErrorDefaultPageName);
                return new Response(StatusCode.InternalServerError, "html", content, "");
            }
           
        }

        private string GetRedirectionPagePathIFExist(string relativePath)
        {
            // using Configuration.RedirectionRules return the redirected page path if exists else returns empty
            string RedirectionPath  = string.Empty;
            if (Configuration.RedirectionRules.ContainsKey(relativePath)) {
                RedirectionPath = Configuration.RedirectionRules[relativePath];
                return RedirectionPath;
            }
            return string.Empty;
        }

        private string LoadDefaultPage(string defaultPageName)
        {
            string filePath = Configuration.RootPath + '\\' + defaultPageName;
            // TODO: check if filepath not exist log exception using Logger class and return empty string

           // try 
           // { 
                if (!File.Exists(filePath)){
                Logger.LogException(new FileNotFoundException("cannot find the file", filePath));
                return string.Empty;
            }
            //}
            ////catch (Exception ex)
            ////{
            ////    Logger.LogException(ex);
            ////    return string.Empty;
            ////}
            // else read file and return its content
            return File.ReadAllText(filePath);
    
        }

        private void LoadRedirectionRules(string filePath)
        {
            try
            {
                // TODO: using the filepath paramter read the redirection rules from file 
                // then fill Configuration.RedirectionRules dictionary
                string[] RulesArr = File.ReadAllLines(filePath);
                Configuration.RedirectionRules = new Dictionary<string, string>();
                // then fill Configuration.RedirectionRules dictionary 
                for (int i = 0; i < RulesArr.Length; i++)
                {
                    string[] rule = RulesArr[i].Split(',');
                    Configuration.RedirectionRules.Add(rule[0], rule[1]);
                }

            }
            catch (Exception ex)
            {
                // TODO: log exception using Logger class
                Logger.LogException(ex);
                Environment.Exit(1);
            }
        }
    }
}