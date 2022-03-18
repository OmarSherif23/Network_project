﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HTTPServer
{
    class Program
    {
        static void Main(string[] args)
        {
            // TODO: Call CreateRedirectionRulesFile() function to create the rules of redirection 
            CreateRedirectionRulesFile();
            string redirect_path = Path.GetFullPath("redirectionRules.txt");
            //Start server
            // 1) Make server object on port 1000
            Server server = new Server(1000, redirect_path);
            // 2) Start Server
            server.StartServer();
        }

        static void CreateRedirectionRulesFile()
        {
            StreamWriter stream_path = new StreamWriter("redirectionRules.txt");
            // TODO: Create file named redirectionRules.txt
            // each line in the file specify a redirection rule
            // example: "aboutus.html,aboutus2.html"
            // means that when making request to aboustus.html,, it redirects me to aboutus2
            stream_path.WriteLine("aboutus.html,aboutus2.html");
            stream_path.Close();
        }
         
    }
}
