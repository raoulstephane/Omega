using Microsoft.WindowsAzure.Storage.Table;

namespace Omega.DataManager
{
    public class UserEntity : TableEntity
    {
        public UserEntity() { }

        public UserEntity( string email, string spotifyId, string spotifyAccessToken, string spotifyRefreshToken )
        {
            PartitionKey = string.Empty;
            RowKey = email;
            SpotifyId = spotifyId;
            SpotifyAccessToken = spotifyAccessToken;
            SpotifyRefreshToken = spotifyRefreshToken;
        }

        public UserEntity( UserEntity user )
        {
            PartitionKey = string.Empty;
            RowKey = user.Email;
            FacebookId = user.FacebookId;
            FacebookAccessToken = user.FacebookAccessToken;
            DeezerId = user.DeezerId;
            SpotifyId = user.SpotifyId;
            SpotifyAccessToken = user.SpotifyAccessToken;
            SpotifyRefreshToken = user.SpotifyRefreshToken;
        }

        public string Email
        {
            get { return RowKey; }
            set { RowKey = value; }
        }
        public string SpotifyId { get; set; }
        public string DeezerId { get; set; }
        public string FacebookId { get; set; }
        public string FacebookAccessToken { get; set; }
        public string SpotifyAccessToken { get; set; }
        public string SpotifyRefreshToken { get; set; }
    }
}
