using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Blob; // Namespace for Blob storage types

namespace Omega.DataManager
{
    public class DatabaseQueries
    {
        static readonly CloudStorageAccount storageAccount;
        static readonly CloudTableClient tableClient;
        static readonly CloudTable tableUser;

        static DatabaseQueries()
        {
            // Retrieve the storage account from the connection string.
            storageAccount = CloudStorageAccount.Parse(
               CloudConfigurationManager.GetSetting( "StorageConnectionString" ) );

            // Create the table client.
            tableClient = storageAccount.CreateCloudTableClient();

            // Create the CloudTables objects that represent the different tables.
            tableUser = tableClient.GetTableReference( "User" );
            //CloudTable tableUser = tableClient.GetTableReference( "people" );
            //CloudTable tableUser = tableClient.GetTableReference( "people" );
        }

        //public static void InsertOrUpdateUserBySpotify( string email )
        //{
        //    // Create a retrieve operation that takes a customer entity.
        //    TableOperation retrieveOperation = TableOperation.Retrieve<UserEntity>( string.Empty, email );

        //    // Execute the retrieve operation.
        //    TableResult retrievedResult = tableUser.Execute( retrieveOperation );
        //    UserEntity retrievedUser = (UserEntity)retrievedResult.Result;

        //    if (retrievedResult.Result != null && retrievedUser.SpotifyRefreshToken != user.SpotifyRefreshToken)
        //    {
        //        retrievedUser.SpotifyRefreshToken = user.SpotifyRefreshToken;
        //        retrievedUser.SpotifyAccessToken = user.SpotifyAccessToken;
        //        retrievedUser.SpotifyId = user.SpotifyId;

        //        TableOperation updateOperation = TableOperation.Replace( retrievedUser );
        //    }
        //    else if (retrievedUser == null)
        //    {
        //        TableOperation insertOperation = TableOperation.Insert( user );
        //    }
        //}
        public static void InsertOrUpdateUserBySpotify( string email, string spotifyId, string spotifyAccessToken, string spotifyRefreshToken )
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<UserEntity>( string.Empty, email );

            // Execute the retrieve operation.
            TableResult retrievedResult = tableUser.Execute( retrieveOperation );
            UserEntity retrievedUser = (UserEntity)retrievedResult.Result;

            if (retrievedResult.Result != null && retrievedUser.SpotifyRefreshToken != spotifyRefreshToken)
            {
                retrievedUser.SpotifyRefreshToken = spotifyRefreshToken;
                retrievedUser.SpotifyAccessToken = spotifyAccessToken;
                retrievedUser.SpotifyId = spotifyId;

                TableOperation updateOperation = TableOperation.Replace( retrievedUser );
            }
            else if (retrievedUser == null)
            {
                UserEntity user = new UserEntity( email, spotifyId, spotifyAccessToken, spotifyRefreshToken );
                TableOperation insertOperation = TableOperation.Insert( user );
                tableUser.Execute( insertOperation );
            }
        }
    }
}
