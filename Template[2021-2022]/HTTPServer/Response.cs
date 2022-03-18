using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HTTPServer
{

    public enum StatusCode
    {
        OK = 200,
        InternalServerError = 500,
        NotFound = 404,
        BadRequest = 400,
        Redirect = 301
    }

    class Response
    {
        string responseString;
        public string ResponseString
        {
            get
            {
                return responseString;
            }
        }
        StatusCode code;
        List<string> headerLines = new List<string>();
        public Response(StatusCode code, string contentType, string content, string redirectoinPath)
        {
            // TODO: Add headlines (Content-Type, Content-Length,Date, [location if there is redirection])


            this.code = code;

            string returned_status_line = GetStatusLine(code);
            string returned_contentType = "Content-Type: " + contentType ;
            string content_Length = "Content-Length: " + content.Length ;
            string date = "Date: " + DateTime.Now ;

            headerLines.Add(returned_contentType);
            headerLines.Add(content_Length);
            headerLines.Add(date);



            // TODO: Create the request string
            if (redirectoinPath!=String.Empty)
            {
                string location = "Location: " + redirectoinPath ;
                headerLines.Add(location);
            }
            string header_line_section = string.Empty;
            foreach (string line in headerLines)
            {
                header_line_section += line + "\r\n" ;
            }
            responseString = returned_status_line + header_line_section + "\r\n" + content + "\r\n";

        }

        private string GetStatusLine(StatusCode code)
        {
            // TODO: Create the response status line and return it
            string statusLine = string.Empty;

            if((int)code == 200)
            {
                statusLine = "OK";
            }

            if ((int)code == 500)
            {
                statusLine = "InternalServerError";
            }

            if ((int)code == 404)
            {
                statusLine = "NotFound";
            }

            if ((int)code == 400)
            {
                statusLine = "BadRequest";
            }

            if ((int)code == 301)
            {
                statusLine = "Redirect";
            }
            string statue = "HTTP/1.1" + (int)code + "" + statusLine + "\r\n";
            return statue;
        }
    }
}