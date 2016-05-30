using Newtonsoft.Json;
using SpotifyWebAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using test;

namespace OmegaSPA
{
    public class AuthorizationHelper
    {
        public static string spotifyClientId = "52bd6a8d6339464088df06679fc4c96a";
        public static string spotifyClientSecret = "94df6a759eb04573ba20e75249c82889";
        //public static string secret = clientId + ":" + clientSecret;
        //public static string secretBase64 = Convert.ToBase64String( Encoding.UTF8.GetBytes( secret ) );
        public static string redirectUri = "http://localhost:51707/Account/Login/callback";
        public static string scope = "user-read-private user-read-email";

        // autorisation
        public static async Task<string> GetAuthorizeCode()
        {
            string code;
            var authorizationEndpoint =
                    "https://accounts.spotify.com/authorize" +
                    "?response_type=code" +
                    "&client_id=" + Uri.EscapeDataString( spotifyClientId ) +
                    "&redirect_uri=" + Uri.EscapeDataString( redirectUri ) +
                    "&scope=" + Uri.EscapeDataString( scope );

            WebRequest request = HttpWebRequest.Create( authorizationEndpoint );
            using (WebResponse response = await request.GetResponseAsync())
            using (Stream responseStream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader( responseStream ))
            {
                code = reader.ReadToEnd();
            }
            return code;
        }

        //// Une fois le code recus
        //public async Task<AuthenticationToken> GetAccessToken( string code )
        //{
        //    Dictionary<string, string> postData = new Dictionary<string, string>();
        //    postData.Add( "grant_type", "authorization_code" );
        //    postData.Add( "code", code );
        //    postData.Add( "redirect_uri", redirectUri );
        //    postData.Add( "client_id", clientId );
        //    postData.Add( "client_secret", clientSecret );

        //    var json = await HttpHelper.Post( "https://accounts.spotify.com/api/token", postData );
        //    var obj = JsonConvert.DeserializeObject<AccessToken>( json, new JsonSerializerSettings
        //    {
        //        TypeNameHandling = TypeNameHandling.All,
        //        TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple
        //    } );
        //    Console.WriteLine( obj.access_token );
        //    Console.ReadKey();
        //    return obj.ToPOCO();
        //}
    }
}