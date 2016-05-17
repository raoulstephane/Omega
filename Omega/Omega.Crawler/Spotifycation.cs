using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Omega.Crawler
{
    public class Spotifycation
    {
        public async Task<string> Search(string title, string artist, string album)
        {
            WebRequest request = HttpWebRequest.Create("https://api.spotify.com/v1/search?q=track%3A"+ title +"+artist%3A"+ artist +"+album%3A"+ album +"&type=track&limit=1");
            request.Method = "GET";
            request.ContentType = "application/json";
            using (WebResponse response = await request.GetResponseAsync())
            using (Stream responseStream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(responseStream))
            {
                string responseFromServer = reader.ReadToEnd();
                JObject rss = JObject.Parse(responseFromServer);
                string rssId = (string)rss["tracks"]["items"][0]["id"];
                Console.WriteLine("Deezer -----> Spotify");
                Console.WriteLine("titre = " + title);
                Console.WriteLine("artiste = " + artist);
                Console.WriteLine("album = " + album);
                Console.WriteLine("----------------------------------------");
                Console.WriteLine("Spotify Id = " + rssId);
                Console.WriteLine("----------------------------------------");
                return rssId;
            }
        }
    }
}
