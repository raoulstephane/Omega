using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Omega
{
    public class SpotifyConnection
    {
        string clientId = "a67128607ff24b1a985abf54aa395bcf";
        string secretId = "94df6a759eb04573ba20e75249c82889";
        string redirectUri = "localhost:8888/callback";

        private String GetUri()
        {
            StringBuilder builder = new StringBuilder("https://accounts.spotify.com/authorize/?");
            builder.Append("client_id=" + clientId);
            builder.Append("&response_type=code");
            builder.Append("&redirect_uri=" + redirectUri);
            return builder.ToString();
        }

        public async Task Connect()
        {
            string code = null;
            string state = null;

            var body = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("code", code),
                new KeyValuePair<string, string>("redirect_uri", redirectUri)
            };
            WebRequest request = HttpWebRequest.Create("https://api.spotify.com/v1/audio-features/06AKEBrKUckW0KREUWRnvT");
            request.Method = "POST";
            request.Headers.Add("Authorization", string.Format("Bearer {0}", ""));
            request.ContentType = "application/json";
            using (WebResponse response = await request.GetResponseAsync())
            using (Stream responseStream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(responseStream))
            {

            }
        }
    }
}