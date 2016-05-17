using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Omega.Crawler
{
    public class DeezerMetadonnees
    {
        public string title { get; set; }
        public Deezerartist artist { get; set; }
        public Deezeralbum album { get; set; }
    }

    public class Deezerartist
    {
        public string name { get; set; }
    }

    public class Deezeralbum
    {
        public string title { get; set; }
    }

    public class DeezerConnect
    {
        public async Task<DeezerMetadonnees> Connect(string id)
        {
            DeezerMetadonnees information = new DeezerMetadonnees();

            WebRequest request = HttpWebRequest.Create("http://api.deezer.com/track/" + id);
            request.Method = "GET";
            request.ContentType = "application/json";
            using (WebResponse response = await request.GetResponseAsync())
            using (Stream responseStream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(responseStream))
            {
                string responseFromServer = reader.ReadToEnd();
                information = JsonConvert.DeserializeObject<DeezerMetadonnees>(responseFromServer);
                return information;
            }  
        }
    }
}