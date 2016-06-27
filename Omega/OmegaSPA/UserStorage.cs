using Omega.DataManager;
using OmegaSPA.ModelsFacebook;
using OmegaSPA.ModelsSpotify;
using System.Threading.Tasks;

namespace OmegaSPA
{
    public class UserStorage
    {
        public static void CreateUser( SpotifyUser sUser )
        {
            UserEntity u = new UserEntity();
            u.Email = sUser.Email;
            u.SpotifyId = sUser.SpotifyId;
            u.SpotifyAccessToken = sUser.SpotifyAccessToken;
            u.SpotifyRefreshToken = sUser.SpotifyRefreshToken;

            DatabaseQueries.InsertOrUpdateUserBySpotify( u );
        }

        public static void CreateUser( FacebookUser fUser )
        {
            UserEntity u = new UserEntity();
            u.Email = fUser.Email;
            u.FacebookId = fUser.FacebookId;
            u.FacebookAccessToken = fUser.AccessToken;

            DatabaseQueries.InsertOrUpdateUserByFacebook( u );
        }
        //static Task DeleteAsync( UserEntity user )
        //{

        //}
        //static Task<UserEntity> FindByIdAsync( string email )
        //{

        //}
        //static Task<UserEntity> FindByNameAsync( string userName )
        //{

        //}
        //static Task UpdateAsync( UserEntity user )
        //{

        //}
    }
}