using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage;
using Microsoft.Azure;
using Omega.DataManager;

namespace OmegaSPA
{
    public static class WebApiConfig
    {
        public static void Register( HttpConfiguration config )
        {
            System.Diagnostics.Debugger.Launch();
            // Configuration et services Web API
            // Configurez Web API pour utiliser uniquement l’authentification par jeton de support.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add( new HostAuthenticationFilter( OAuthDefaults.AuthenticationType ) );

            // Utilisez la casse mixte pour les données JSON.
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            // Routes Web API
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
    }
}
