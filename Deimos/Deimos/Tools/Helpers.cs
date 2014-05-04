using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Deimos
{
    class Helpers
    {
        public string FileGetContents(string url)
        {
            try
            {
                WebRequest request = WebRequest.Create(url) as HttpWebRequest;

                request.Method = "GET";
                request.ContentType = "application/json";
                request.Timeout = 20000;

                string responseContent = "";
                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    using (var reader =
                        new StreamReader(response.GetResponseStream()))
                    {
                        responseContent = reader.ReadToEnd();
                        // Prevent memory leak
                        reader.Dispose();
                        request = null;
                    }
                }
                return responseContent;
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}
