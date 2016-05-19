using System.Web.Http;
using Microsoft.Azure; // Namespace for CloudConfigurationManager 
using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Table; // Namespace for Table storage types
using Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.AspNet.Identity;

namespace Omega.App_Start
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Configuration et services API Web

            // Itinéraires de l'API Web
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Retrieve the storage account from the connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting( "StorageConnectionString" ) );

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Retrieve a reference to the table.
            CloudTable table = tableClient.GetTableReference( "Playlist" );
            // Create the table if it doesn't exist.
            table.CreateIfNotExists();

            table = tableClient.GetTableReference( "RowTrack" );
            table.CreateIfNotExists();

            table = tableClient.GetTableReference( "User" );
            table.CreateIfNotExists();
        }

        public static void ConfigureAuth( IAppBuilder app )
        {
            // Enable the application to use a cookie to store information for the signed in user
            app.UseCookieAuthentication( new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString( "/Account/Login" )
            } );

            // Use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie( DefaultAuthenticationTypes.ExternalCookie );
            

            app.UseFacebookAuthentication(
               appId: "263290184003311",
               appSecret: "c534799048294b0566e010a10a3ea67f" );
            

            app.UseSpotifyAuthentication(
                clientId: "52bd6a8d6339464088df06679fc4c96a",
                clientSecret: "20c05410d9ae449c8d57dec06b6ba10e" );

            app.UseDeezerAuthentication(
                 appId: "180182",
                 secretKey: "5a75ec4d6615ec1617ad46bc2410ae88" );
        }

    }
}
