using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace Omega.OmegaSPA.GeneralModels
{
    public class AuthenticationToken
    {
        private string accessToken;

        /// <summary>
        /// An access token that can be provided in subsequent calls, for example to Spotify Web API services. 
        /// 
        /// refreshes the token automatically if it has expired
        /// </summary>
        public string access_token
        {
            get
            {
                if (HasExpired)
                    Refresh();

                return accessToken;
            }
            set
            {
                accessToken = value;
            }
        }

        /// <summary>
        /// How the access token may be used: always "Bearer". 
        /// </summary>
        public string token_type { get; set; }

        /// <summary>
        /// The date/time that this token will become invalid
        /// </summary>
        public int expires_in { get; set; }

        /// <summary>
        /// A token that can be sent to the Spotify Accounts service in place of an authorization code. 
        /// (When the access code expires, send a POST request to the Accounts service /api/token endpoint, but 
        /// use this code in place of an authorization code. A new access token and a new refresh token will be returned.) 
        /// </summary>
        public string refresh_token { get; set; }

        /// <summary>
        /// Determines if this token has expired
        /// </summary>
        public bool HasExpired
        {
            get
            {
                //return DateTime.Now > expires_in;
                return false;
            }
        }

        /// <summary>
        /// Updates this token if it has expired
        /// </summary>
        public async void Refresh()
        {
            string spotifyClientId = "52bd6a8d6339464088df06679fc4c96a";
            string spotifyClientSecret = "20c05410d9ae449c8d57dec06b6ba10e";

            //var token = await GeneralModels.Authentication.GetAccessToken( this.RefreshToken );
            var refreshTokenRequest = "https://accounts.spotify.com/api/token";
            WebRequest request = HttpWebRequest.Create( refreshTokenRequest );

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            string body = string.Format( "grant_type=refresh_token&refresh_token={0}&client_id={1}&client_secret={2}"
                , Uri.EscapeDataString( refresh_token ),
                spotifyClientId,
                spotifyClientSecret );
            byte[] bodyBuffer = Encoding.ASCII.GetBytes( body );
            request.ContentLength = bodyBuffer.Length;

            //// Header
            //string authorization = string.Format( "Basic {0}:{1}", spotifyClientId, spotifyClientSecret );
            //authorization = Convert.ToBase64String( Encoding.Unicode.GetBytes( authorization ) );
            //request.Headers.Add( "Authorization", authorization );

            using (Stream requestStream = await request.GetRequestStreamAsync())
            {
                await requestStream.WriteAsync( bodyBuffer, 0, body.Length );
            }
            using (WebResponse response = await request.GetResponseAsync())
            using (Stream responseStream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader( responseStream ))
            {
                // Récupération du token au format text Json
                string jsonToken = reader.ReadToEnd();
                // Enregistrement de ce Json au format AuthenticationToken
                AuthenticationToken token = JsonConvert.DeserializeObject<AuthenticationToken>( jsonToken );
                this.access_token = token.accessToken;
                this.expires_in = token.expires_in;
                this.refresh_token = token.refresh_token;
                this.token_type = token.token_type;
            }
        }
    }
}
