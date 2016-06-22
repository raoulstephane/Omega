using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin;
using OmegaSPA.Models;
using OmegaSPA.Providers;
using Owin.Security.Providers.Spotify;
using Omega;
using Owin.Security.Providers.Spotify.Provider;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Security.Claims;
using Microsoft.Owin.Security.Facebook;
using OmegaSPA.ModelsSpotify;
using OmegaSPA.ModelsFacebook;
using Facebook;

namespace OmegaSPA
{
    public partial class Startup
    {
        // Autoriser l'application à utiliser OAuthAuthorization. Vous pouvez ensuite sécuriser vos API Web
        static Startup()
        {
            PublicClientId = "web";

            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString( "/Token" ),
                AuthorizeEndpointPath = new PathString( "/Account/Authorize" ),
                Provider = new ApplicationOAuthProvider( PublicClientId ),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays( 14 ),
                AllowInsecureHttp = true
            };
        }

        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        public static string PublicClientId { get; private set; }

        // Pour plus d’informations sur la configuration de l’authentification, rendez-vous sur http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth( IAppBuilder app )
        {
            // Configurer le contexte de base de données, le gestionnaire des utilisateurs et le gestionnaire des connexions pour utiliser une instance unique par demande
            app.CreatePerOwinContext( ApplicationDbContext.Create );
            app.CreatePerOwinContext<ApplicationUserManager>( ApplicationUserManager.Create );
            app.CreatePerOwinContext<ApplicationSignInManager>( ApplicationSignInManager.Create );

            // Autoriser l'application à utiliser un cookie pour stocker des informations pour l’utilisateur connecté
            app.UseCookieAuthentication( new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString( "/Account/Login" ),
                Provider = new CookieAuthenticationProvider
                {
                    // Permet à l'application de valider le timbre de sécurité quand l'utilisateur se connecte.
                    // Cette fonction de sécurité est utilisée quand vous changez un mot de passe ou ajoutez une connexion externe à votre compte.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes( 20 ),
                        regenerateIdentity: ( manager, user ) => user.GenerateUserIdentityAsync( manager ) )
                }
            } );
            // Utilisez un cookie pour stocker temporairement des informations sur une connexion utilisateur à un fournisseur de connexions tiers
            app.UseExternalSignInCookie( DefaultAuthenticationTypes.ExternalCookie );

            // Permet à l'application de stocker temporairement les informations utilisateur lors de la vérification du second facteur dans le processus d'authentification à 2 facteurs.
            app.UseTwoFactorSignInCookie( DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes( 5 ) );

            // Permet à l'application de mémoriser le second facteur de vérification de la connexion, un numéro de téléphone ou un e-mail par exemple.
            // Lorsque vous activez cette option, votre seconde étape de vérification pendant le processus de connexion est mémorisée sur le poste à partir duquel vous vous êtes connecté.
            // Ceci est similaire à l'option RememberMe quand vous vous connectez.
            app.UseTwoFactorRememberBrowserCookie( DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie );

            // Autoriser l’application à utiliser des jetons de support pour authentifier les utilisateurs
            app.UseOAuthBearerTokens( OAuthOptions );

            FacebookAuthenticationOptions facebookOption = new FacebookAuthenticationOptions
            {
                AppId = "263290184003311",
                AppSecret = "c534799048294b0566e010a10a3ea67f",
                CallbackPath = new PathString( "/signin-facebook" ),
                Provider = new FacebookAuthenticationProvider
                {
                    OnAuthenticated = async c =>
                    {
                        FacebookClient fbClient = new FacebookClient( c.AccessToken );
                        dynamic fbUser = fbClient.Get( "me?fields=id,name, email" );
                        JObject facebookUserJson = JObject.FromObject( fbUser );
                        string email = (string)facebookUserJson["email"];

                        UserStorage.CreateUser( new FacebookUser( email, c.Id, c.AccessToken ) );

                        c.Identity.AddClaim( new Claim( "http://omega.fr:user_email", email ) );
                        c.Identity.AddClaim( new Claim( "http://omega.fr:facebook_access_token", c.AccessToken ) );
                    }
                }
            };
            app.UseFacebookAuthentication( facebookOption );

            

            SpotifyAuthenticationOptions spotifyAuthOptions = new SpotifyAuthenticationOptions
            {
                ClientId = "52bd6a8d6339464088df06679fc4c96a",
                ClientSecret = "20c05410d9ae449c8d57dec06b6ba10e",
                Provider = new SpotifyAuthenticationProvider
                {
                    OnAuthenticated = async c =>
                    {
                        // c.Identity.Claims to retrieve claims

                        var currentUserRequest = "https://api.spotify.com/v1/me";
                        WebRequest userRequest = HttpWebRequest.Create( currentUserRequest );
                        userRequest.Method = "GET";
                        userRequest.Headers.Add( "Authorization", string.Format( "Bearer {0}", c.AccessToken ) );
                        userRequest.ContentType = "application/json";

                        string currentUserEmail;

                        using (WebResponse response = await userRequest.GetResponseAsync())
                        using (Stream responseStream = response.GetResponseStream())
                        using (StreamReader reader = new StreamReader( responseStream ))
                        {
                            string currentUserJson = reader.ReadToEnd();
                            JObject rss = JObject.Parse( currentUserJson );
                            currentUserEmail = (string)rss["email"];
                        }
                        
                        UserStorage.CreateUser( new SpotifyUser( currentUserEmail, c.Id, c.AccessToken, c.RefreshToken ) );

                        c.Identity.AddClaim( new Claim( "http://omega.fr:user_email", currentUserEmail ) );
                        c.Identity.AddClaim( new Claim( "http://omega.fr:spotify_access_token", c.AccessToken ) );
                        c.Identity.AddClaim( new Claim( "http://omega.fr:spotify_refresh_token", c.RefreshToken ) );
                    }
                }
            };
            spotifyAuthOptions.Scope.Add( "user-read-email" );// if email is needed.
            spotifyAuthOptions.Scope.Add( "playlist-read-private" );
            spotifyAuthOptions.Scope.Add( "playlist-read-collaborative" );
            spotifyAuthOptions.Scope.Add( "user-library-read" );


            app.UseSpotifyAuthentication( spotifyAuthOptions );


            app.UseDeezerAuthentication(
                 appId: "180182",
                 secretKey: "5a75ec4d6615ec1617ad46bc2410ae88" );
        }
    }
}
