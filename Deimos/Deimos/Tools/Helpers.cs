using Microsoft.Xna.Framework;
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

        public string Truncate(string str, int maxlength, string end = "")
        {
            if (str.Length > maxlength)
            {
                str = str.Substring(0, maxlength - end.Length) + end;
            }

            return str;
        }

        public Color ColorBetween(Color min, Color max)
        {
            return Microsoft.Xna.Framework.Color.Lerp(min, max, Float());
        }

        public Color ColorWithAlpha()
        {
            return new Color(Float(), Float(), Float(), Float());
        }

        public Color Color()
        {
            return new Color(Float(), Float(), Float());
        }

        static Random random = new Random();
        public Random Random
        {
            get { return random; }
        }

        public float Float()
        {
            return (float)Random.NextDouble();
        }

        public float FloatBetween(float min, float max)
        {
            return min + (float)(Random.NextDouble() * (max - min));
        }

        public int IntBetween(int min, int max)
        {
            return min + (int)(Random.NextDouble() * (max - min));
        }

        public bool Boolean()
        {
            return Random.Next(2) == 0;
        }
    }
}
