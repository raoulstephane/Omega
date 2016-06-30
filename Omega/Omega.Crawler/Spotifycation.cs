using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Omega.Crawler
{
    public class Spotifycation
    {
        public async Task<string> Search(string title, string artist, string album)
        {
            StringBuilder builder = new StringBuilder("https://api.spotify.com/v1/search?q=");
            builder.Append("track%3A" + title);
            if (!String.IsNullOrEmpty(artist))
            {
                builder.Append("+artist%3A" + artist);
            }
            if (!String.IsNullOrEmpty(album))
            {
                builder.Append("+album%3A" + album);
            }
            builder.Append("&type=track&limit=1");

            WebRequest request = HttpWebRequest.Create(builder.ToString());
            request.Method = "GET";
            request.ContentType = "application/json";
            using (WebResponse response = await request.GetResponseAsync())
            using (Stream responseStream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(responseStream))
            {
                string responseFromServer = reader.ReadToEnd();
                JObject rss = JObject.Parse(responseFromServer);
                if ((string)rss["tracks"]["total"] == "0")
                {
                    return "";
                }             
                string rssId = (string)rss["tracks"]["items"][0]["id"];
                return rssId;
            }
        }
    }
}