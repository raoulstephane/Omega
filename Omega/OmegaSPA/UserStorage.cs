using Omega.DataManager;

namespace OmegaSPA
{
    public class UserStorage
    {
        public static UserEntity CreateUser( string email, string spotifyId, string spotifyAccessToken, string spotifyRefreshToken )
        {
            UserEntity u = new UserEntity( email, spotifyId, spotifyAccessToken, spotifyRefreshToken );
            DatabaseQueries.InsertOrUpdateUserBySpotify( email, spotifyId, spotifyAccessToken, spotifyRefreshToken );
            return u;
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