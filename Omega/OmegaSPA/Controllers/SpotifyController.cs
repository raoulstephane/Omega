using System.Net;
using System.Web.Http;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Security.Claims;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace OmegaSPA.Controllers
{
    public class SpotifyController : ApiController
    {
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return this.AuthenticationManager;

            }
        }

        public static async Task<string> GetCurrentUserEmail(string accessToken )
        {
            var currentUserRequest = "https://api.spotify.com/v1/me";
            WebRequest userRequest = HttpWebRequest.Create( currentUserRequest );
            userRequest.Method = "GET";
            userRequest.Headers.Add( "Authorization", string.Format( "Bearer {0}", accessToken ) );
            userRequest.ContentType = "application/json";

            string currentUserEmail;

            using (WebResponse response = await userRequest.GetResponseAsync())
            using (Stream responseStream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader( responseStream ))
            {
                string currentUserJson = reader.ReadToEnd();
                JObject rss = JObject.Parse( currentUserJson );
                currentUserEmail = (string)rss["email"];
                //string rssId = (string)rss["id"];
                //SpotifyUser spotifyUser = new SpotifyUser( rssEmail, rssId, token );
                //DatabaseQueries.InsertOrUpdateUserBySpotify( rssEmail, rssId, token.access_token, token.refresh_token );
            }
            return currentUserEmail;
        }

        [Route( "api/Spotify/playlists" )]
        public async Task<JObject> GetAllSpotifyPlaylists()
        {
            var allPlaylistsRequest = "https://api.spotify.com/v1/me/playlists";
            WebRequest playlistsRequest = HttpWebRequest.Create( allPlaylistsRequest );
            playlistsRequest.Method = "GET";

            ClaimsIdentity claimsIdentity = await this.AuthenticationManager.GetExternalIdentityAsync( DefaultAuthenticationTypes.ExternalCookie );
            Claim claim = claimsIdentity.Claims.Single( c => c.Type == "http://omega.fr:access_token" );
            string accessToken = claim.Value;

            playlistsRequest.Headers.Add( "Authorization", accessToken );

            JObject allPlaylistsJson;

            using (WebResponse response = await playlistsRequest.GetResponseAsync())
            using (Stream responseStream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader( responseStream ))
            {
                string allPlaylistsJsonString = reader.ReadToEnd();
                allPlaylistsJson = JObject.Parse( allPlaylistsJsonString );
            }
            return allPlaylistsJson;
        }
    }
}
