using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Security.Claims;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using System.Collections.Generic;
using Omega.DataManager;

namespace OmegaSPA.Controllers
{
    public class SpotifyController : ApiController
    {
        /// <summary>
        /// Insert the tracks of the current playlist in the table Track if they aren't in already.
        /// </summary>
        /// <param name="urlRequest"></param>
        /// <param name="accessToken"></param>
        /// <param name="userdId"></param>
        /// <param name="playlistId"></param>
        /// <returns>Returns all the tracks of the playlist.</returns>
        private async Task<List<TrackEntity>> GetAllTracksInPlaylists(string urlRequest, string accessToken, string userdId, string playlistId, string cover )
        {
            List<TrackEntity> tracksInPlaylist = new List<TrackEntity>();

            WebRequest getTracksInPlaylistRequest = HttpWebRequest.Create( urlRequest );
            getTracksInPlaylistRequest.Method = "GET";
            getTracksInPlaylistRequest.Headers.Add( "Authorization", string.Format( "Bearer {0}", accessToken ) );
            using (WebResponse responseTracksInPlaylist = await getTracksInPlaylistRequest.GetResponseAsync())
            using (Stream responseStreamTracksInPlaylist = responseTracksInPlaylist.GetResponseStream())
            using (StreamReader readerTracksInPlaylists = new StreamReader( responseStreamTracksInPlaylist ))
            {
                string allTracksInPlaylistString = readerTracksInPlaylists.ReadToEnd();
                JObject allTracksInPlaylistJson = JObject.Parse( allTracksInPlaylistString );
                JArray allTracksInPlaylistArray = (JArray)allTracksInPlaylistJson["items"];

                for( int i = 0; i < allTracksInPlaylistArray.Count; i++)
                {
                    string trackTitle = (string)allTracksInPlaylistJson["items"][i]["track"]["name"];
                    string trackId = (string)allTracksInPlaylistJson["items"][i]["track"]["id"];
                    string albumName = (string)allTracksInPlaylistJson["items"][i]["track"]["album"]["name"];
                    string trackPopularity = (string)allTracksInPlaylistJson["items"][i]["track"]["popularity"];
                    string coverAlbum = (string)allTracksInPlaylistJson["items"][i]["track"]["album"]["images"][0]["url"];

                    DatabaseQueries.InsertSpotifyTrack( userdId, playlistId, trackId, trackTitle, albumName, trackPopularity, coverAlbum );
                    tracksInPlaylist.Add( new TrackEntity( "s", userdId, playlistId, trackId, trackTitle, albumName, trackPopularity, coverAlbum ) );
                }
            }
            return tracksInPlaylist;
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
            using (StreamReader reader = new StreamReader(responseStream))
            {
                string currentUserJson = reader.ReadToEnd();
                JObject rss = JObject.Parse(currentUserJson);
                currentUserEmail = (string)rss["email"];
            }
            return currentUserEmail;
        }

        [Route( "Spotify/playlists" )]
        public async Task<string> GetAllSpotifyPlaylists()
        {
            var allPlaylistsRequest = "https://api.spotify.com/v1/me/playlists";
            WebRequest playlistsRequest = HttpWebRequest.Create( allPlaylistsRequest );
            playlistsRequest.Method = "GET";

            ClaimsIdentity claimsIdentity = await this.Request.GetOwinContext().Authentication.GetExternalIdentityAsync( DefaultAuthenticationTypes.ExternalCookie );
            Claim claim = claimsIdentity.Claims.Single( c => c.Type == "http://omega.fr:spotify_access_token" );
            string accessToken = claim.Value;

            playlistsRequest.Headers.Add( "Authorization", string.Format( "Bearer {0}", accessToken ) );

            JObject allPlaylistsJson;
            string allplaylist;

            using (WebResponse responseAllPlaylists = await playlistsRequest.GetResponseAsync())
            using (Stream responseStreamAllPlaylists = responseAllPlaylists.GetResponseStream())
            using (StreamReader readerAllPlaylists = new StreamReader( responseStreamAllPlaylists ))
            {
                string allPlaylistsString = readerAllPlaylists.ReadToEnd();

                allPlaylistsJson = JObject.Parse( allPlaylistsString );
                JArray allPlaylistsArray = (JArray)allPlaylistsJson["items"];
                List<PlaylistEntity> listOfPlaylists = new List<PlaylistEntity>();

                for (int i = 0; i < allPlaylistsArray.Count; i++)
                {
                    var playlist = allPlaylistsArray[i];

                    string requestTracksInPlaylist = (string)playlist["tracks"]["href"];

                    string idOwner = (string)playlist["owner"]["id"];
                    string name = (string)playlist["name"];
                    string idPlaylist = (string)playlist["id"];
                    string coverPlaylist = (string)playlist["images"][0]["url"];

                    PlaylistEntity p = new PlaylistEntity( idOwner, idPlaylist, await GetAllTracksInPlaylists(requestTracksInPlaylist, accessToken, idOwner, idPlaylist, coverPlaylist), name, coverPlaylist);
                    DatabaseQueries.InsertSpotifyPlaylist( p );
                    listOfPlaylists.Add( p );
                }
                allplaylist = JsonConvert.SerializeObject( listOfPlaylists );
                //allPlaylistsJson = JObject.Parse( allPlaylistsString );
            }
            //return allPlaylistsJson;
            return allplaylist;
        }
    }
}
