using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System;

namespace Omega.Crawler
{
    public class CredentialAuth
    {
        public async Task<MetaDonnees> TrackMetadonnee(string songId)
        {
            string token = await GetAccessToken();
            MetaDonnees information = new MetaDonnees();

            WebRequest request = HttpWebRequest.Create("https://api.spotify.com/v1/audio-features/" + songId);
            request.Method = "GET";
            request.Headers.Add("Authorization", string.Format("Bearer {0}", token));
            request.ContentType = "application/json";
            using (WebResponse response = await request.GetResponseAsync())
            using (Stream responseStream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(responseStream))
            {
                string responseFromServer = reader.ReadToEnd();
                information = JsonConvert.DeserializeObject<MetaDonnees>(responseFromServer);
                Console.WriteLine(responseFromServer);
                return information;
            }
        }

        public async Task<string> GetAccessToken()
        {
            SpotifyToken token = new SpotifyToken();

            string postString = string.Format("grant_type=client_credentials");
            byte[] byteArray = Encoding.UTF8.GetBytes(postString);

            string url = "https://accounts.spotify.com/api/token";

            WebRequest request = WebRequest.Create(url);
            request.Method = "POST";
            request.Headers.Add("Authorization", "Basic YTY3MTI4NjA3ZmYyNGIxYTk4NWFiZjU0YWEzOTViY2Y6OTRkZjZhNzU5ZWIwNDU3M2JhMjBlNzUyNDljODI4ODk=");
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;
            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
                using (WebResponse response = await request.GetResponseAsync())
                using (Stream responseStream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(responseStream))
                {
                    string responseFromServer = reader.ReadToEnd();
                    token = JsonConvert.DeserializeObject<SpotifyToken>(responseFromServer);

                }
            }
            return token.access_token;
        }
    }

}

