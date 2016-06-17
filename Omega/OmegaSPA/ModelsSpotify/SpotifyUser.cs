using Omega.OmegaSPA.GeneralModels;

namespace OmegaSPA.ModelsSpotify
{
    public class SpotifyUser
    {
        public SpotifyUser(string email, string spotifyId, string accessToken, string refreshToken )
        {
            Email = email;
            SpotifyId = spotifyId;
            SpotifyAccessToken = accessToken;
            SpotifyRefreshToken = refreshToken;
        }

        public string Email { get; set; }
        public string SpotifyId { get; set; }
        public string SpotifyAccessToken { get; set; }
        public string SpotifyRefreshToken { get; set; }
    }
}