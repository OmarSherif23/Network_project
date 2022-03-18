using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HTTPServer
{
    public enum RequestMethod
    {
        GET,
        POST,
        HEAD
    }

    public enum HTTPVersion
    {
        HTTP10,
        HTTP11,
        HTTP09
    }

    class Request
    {
        string[] requestLines;
        RequestMethod method;
        public string relativeURI;
        Dictionary<string, string> headerLines;

        public Dictionary<string, string> HeaderLines
        {
            get { return headerLines; }
        }

        HTTPVersion httpVersion;
        string requestString;
        string[] contentLines;

        public Request(string requestString)
        {
            this.requestString = requestString;
        }
        /// <summary>
        /// Parses the request string and loads the request line, header lines and content, returns false if there is a parsing error
        /// 
        /// </summary>
        /// <returns>True if parsing succeeds, false otherwise.</returns>

        public bool ParseRequest()
        {
            //TODO: parse the receivedRequest using the \r\n delimeter   

            // check that there is atleast 3 lines: Request line, Host Header, Blank line (usually 4 lines with the last empty line for empty content)

            // Parse Request line

            // Validate blank line exists

            // Load header lines into HeaderLines dictionary

            //TODO: parse the receivedRequest using the \r\n delimeter   
            contentLines = requestString.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            // check that there is atleast 3 lines: Request line, Host Header, Blank line (usually 4 lines with the last empty line for empty content)
            if (contentLines.Length < 3)
            {
                return false;
            }
            // Parse Request line
            requestLines = contentLines[0].Split(' ');//method uri version
            if (!(this.ParseRequestLine()))
                return false;
            if (!(this.ValidateBlankLine()))
                return false;
            if (!(this.LoadHeaderLines()))
                return false;

            return true;
        }

        private bool ParseRequestLine()
        {
            if (requestLines.Length < 3)
            {
                return false;
            }
            if (requestLines[2] == "HTTP/0.9")
            {
                httpVersion = HTTPVersion.HTTP09;
            }
            else if (requestLines[2] == "HTTP/1.0")
            {
                httpVersion = HTTPVersion.HTTP10;
            }
            else if (requestLines[2] == "HTTP/1.1")
            {
                httpVersion = HTTPVersion.HTTP11;
            }
            else
            {
                return false;
            }

        
            switch (requestLines[0].ToUpper())
            {
                case "GET":
                    method = RequestMethod.GET;
                    break;
                case "POST":
                    method = RequestMethod.POST;
                    break;
                case "HEAD":
                    method = RequestMethod.HEAD;
                    break;
                default:
                    return false;
            }

            relativeURI = requestLines[1];
            return ValidateIsURI(relativeURI);
        }

        private bool ValidateIsURI(string uri)
        {
            return Uri.IsWellFormedUriString(uri, UriKind.RelativeOrAbsolute);
        }

        private bool LoadHeaderLines()
        {
            
            headerLines = new Dictionary<string, string>();
            
                if (contentLines[1].Contains(":"))
                {
                    string[] splitch = { ": " };
                    string[] request1 = contentLines[1].Split(splitch, StringSplitOptions.None);
                    headerLines.Add(request1[0], request1[1]);
                }
                else  return false;
            
            return true;
        }

        private bool ValidateBlankLine()
        {
            if (contentLines[(contentLines.Length - 2)] == "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
