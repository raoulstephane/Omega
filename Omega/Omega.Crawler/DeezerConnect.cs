using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Omega.Crawler
{
    public class DeezerConnect
    {
        public async Task<Track> Connect(string id)
        {
            Track track = new Track();

            WebRequest request = HttpWebRequest.Create("http://api.deezer.com/track/" + id);
            request.Method = "GET";
            request.ContentType = "application/json";
            using (WebResponse response = await request.GetResponseAsync())
            using (Stream responseStream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(responseStream))
            {
                string responseFromServer = reader.ReadToEnd();
                JObject rss = JObject.Parse(responseFromServer);
                track.AlbumName = (string)rss["album"]["title"];
                track.Artist = (string)rss["artist"]["name"];
                track.Title = (string)rss["title"];
                return track;
            }  
        }
    }
}