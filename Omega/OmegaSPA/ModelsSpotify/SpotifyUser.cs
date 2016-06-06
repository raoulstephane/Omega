using Omega.OmegaSPA.GeneralModels;

namespace OmegaSPA.GeneralModels
{
    public class SpotifyUser
    {
        public SpotifyUser(string email, string spotifyId, AuthenticationToken authenticationToken )
        {
            Email = email;
            SpotifyId = spotifyId;
            SpotifyAccessToken = authenticationToken.access_token;
            SpotifyRefreshToken = authenticationToken.refresh_token;
        }

        public string Email { get; set; }
        public string SpotifyId { get; set; }
        public string SpotifyAccessToken { get; set; }
        public string SpotifyRefreshToken { get; set; }
    }
}