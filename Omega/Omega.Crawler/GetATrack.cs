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
    public class GetATrack
    {
        public async Task<Track> GetTrack(string id)
        {
            Track track = new Track();
            WebRequest request = HttpWebRequest.Create("https://api.spotify.com/v1/tracks/" + id);
            request.Method = "GET";
            request.ContentType = "application/json";
            using (WebResponse response = await request.GetResponseAsync())
            using (Stream responseStream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(responseStream))
            {
                string responseFromServer = reader.ReadToEnd();
                JObject rss = JObject.Parse(responseFromServer);
                track.AlbumName = (string)rss["album"]["name"];
                track.Popularity = (string)rss["popularity"];
                track.Title = (string)rss["name"];

                return track;
            }
        }
    }
}
