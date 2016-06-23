using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OmegaSPA.ModelsDeezer
{
    public class DeezerUser
    {
        public DeezerUser(string email, string spotifyId, string accessToken)
        {
            Email = email;
            DeezerId = DeezerId;
            DeezerAccessToken = accessToken;
           // DeezerRefreshToken = refreshToken;
        }

        //The email of the Deezer user
        public string Email { get; set; }

        //The id of the Deezer user
        public string DeezerId { get; set; }

        //The Access Token of the Deezer User
        public string DeezerAccessToken { get; set; }

        //The Refresh Token of the Deezer User
      //  public string DeezerRefreshToken { get; set; }
    }
}