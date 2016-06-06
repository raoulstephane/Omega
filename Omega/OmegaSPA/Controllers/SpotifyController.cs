using OmegaSPA.ModelsSpotify;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json.Linq;

namespace OmegaSPA.Controllers
{
    public class SpotifyController : ApiController
    {
        [Route( "playlists" )]
        public async Task<JObject> GetAllSpotifyPlaylists( string accessToken )
        {
            var allPlaylistsRequest = "https://api.spotify.com/v1/me/playlists";
            WebRequest playlistsRequest = HttpWebRequest.Create( allPlaylistsRequest );

            playlistsRequest.Method = "GET";
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
