using OmegaSPA.ModelsSpotify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using SpotifyWebAPI;

namespace OmegaSPA.Controllers
{
    public class SpotifyController : ApiController
    {
        public async Task<Page<Playlist>> GetAllPlaylists()
        {
            string authorizationCode = await AuthorizationHelper.GetAuthorizeCode();

            var authenticationToken = new AuthenticationToken()
            {
                AccessToken = "NgCXRK...MzYjw",
                ExpiresOn = DateTime.Now.AddSeconds( 3600 ),
                RefreshToken = "dfagC...fd43x",
                TokenType = "Bearer"
            };

            // get the user you just logged in with
            var user = await SpotifyWebAPI.User.GetCurrentUserProfile( authenticationToken );

            // get this persons playlists
            var playlists = await user.GetPlaylists( authenticationToken );

            return playlists;
        }
    }
}
