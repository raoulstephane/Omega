using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Omega.Crawler
{
    public class SpotifyToDeezer
    {
        public async Task<string> GetDeezerId(string title, string artist)
        {
            string deezerId = "";

            WebRequest request = HttpWebRequest.Create("https://api.deezer.com/search?q=artist:'"+artist+"' track:'"+title+"'");
            request.Method = "GET";
            request.ContentType = "application/json";
            using (WebResponse response = await request.GetResponseAsync())
            using (Stream responseStream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(responseStream))
            {
                string responseFromServer = reader.ReadToEnd();
                JObject rss = JObject.Parse(responseFromServer);
                if ((string)rss["total"] == "0")
                    Console.WriteLine("No equivalent Deezer");
                else
                    deezerId = (string)rss["data"][0]["id"];
            }

            return deezerId;
        } 
    }
}
