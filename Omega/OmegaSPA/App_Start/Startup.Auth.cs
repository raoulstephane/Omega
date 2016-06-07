﻿using System;
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
using System.Threading.Tasks;

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

            // Supprimer les commentaires des lignes suivantes pour autoriser la connexion avec des fournisseurs de connexions tiers
            app.UseFacebookAuthentication(
                appId: "263290184003311",
                appSecret: "c534799048294b0566e010a10a3ea67f" );


            app.UseSpotifyAuthentication(
                clientId: "52bd6a8d6339464088df06679fc4c96a",
                clientSecret: "20c05410d9ae449c8d57dec06b6ba10e" );

            SpotifyAuthenticationOptions spotifyAuthOptions = new SpotifyAuthenticationOptions
            {
                ClientId = "52bd6a8d6339464088df06679fc4c96a",
                ClientSecret = "20c05410d9ae449c8d57dec06b6ba10e",
                CallbackPath = new PathString( "/Account/callback" ),
                Provider = new SpotifyAuthenticationProvider
                {
                    OnAuthenticated = async c =>
                    {
                        // c.Identity.Claims to retrieve claims
                        int userId = await CreateUser( c );
                        c.Identity.AddClaim( new System.Security.Claims.Claim( "http://omega.fr:user_id", userId.ToString() ) );
                    }
                }
            };

            // spotifyAuthOptions.Scope.Add( "email" ); // if email is needed.

            app.UseSpotifyAuthentication( spotifyAuthOptions );


            app.UseDeezerAuthentication(
                 appId: "176241",
                 secretKey: "176241270f8a9c20d1c8f31baa6e32ec3871a9" );
        }

        Task<int> CreateUser( SpotifyAuthenticatedContext c )
        {
            throw new NotImplementedException();
        }
    }
}
