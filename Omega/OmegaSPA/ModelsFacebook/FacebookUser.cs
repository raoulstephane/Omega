namespace OmegaSPA.ModelsFacebook
{
    public class FacebookUser
    {
        public FacebookUser( string email, string facebookId, string accessToken )
        {
            Email = email;
            FacebookId = facebookId;
            AccessToken = accessToken;
        }

        public string Email { get; set; }
        public string FacebookId { get; set; }
        public string AccessToken { get; set; }
    }
}