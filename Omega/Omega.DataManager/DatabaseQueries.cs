using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;

namespace Omega.DataManager
{
    public class DatabaseQueries
    {
        static readonly CloudQueueClient queueClient;
        static readonly CloudStorageAccount storageAccount;
        static readonly CloudTableClient tableClient;
        static readonly CloudTable tableUser;
        static readonly CloudTable tablePlaylist;
        static readonly CloudTable tableTrack;
        static readonly CloudQueue queue;

        static DatabaseQueries()
        {
            // Retrieve the storage account from the connection string.
            storageAccount = CloudStorageAccount.Parse(
               CloudConfigurationManager.GetSetting( "StorageConnectionString" ) );

            // Create the table client.
            tableClient = storageAccount.CreateCloudTableClient();

            // Create the CloudTables objects that represent the different tables.
            tableUser = tableClient.GetTableReference( "User" );
            tableTrack = tableClient.GetTableReference( "Track" );
            tablePlaylist = tableClient.GetTableReference( "Playlist" );

            queueClient = storageAccount.CreateCloudQueueClient();
            queue = queueClient.GetQueueReference("myqueue");
            queue.CreateIfNotExistsAsync();
        }

        public static bool IsUserPresentInBase( string email )
        {
            TableOperation retrieveUser = TableOperation.Retrieve<UserEntity>( string.Empty, email );
            TableResult retrievedUser = tableUser.Execute( retrieveUser );

            UserEntity u = (UserEntity)retrievedUser.Result;
            return (u != null && u.SpotifyId != null);
        }

        public static string GetFacebookAccessTokenByEmail( string email )
        {
            TableOperation retrieveUserOperation = TableOperation.Retrieve<UserEntity>( string.Empty, email );
            TableResult retrievedUser = tableUser.Execute( retrieveUserOperation );

            UserEntity e = (UserEntity)retrievedUser.Result;

            return e.FacebookAccessToken;
        }

        public static string GetFacebookIdByEmail( string email )
        {
            TableOperation retrieveUserOperation = TableOperation.Retrieve<UserEntity>( string.Empty, email );
            TableResult retrievedUser = tableUser.Execute( retrieveUserOperation );

            UserEntity e = (UserEntity)retrievedUser.Result;

            return e.FacebookId;
        }

        public static string GetSpotifyAccessTokenByEmail( string email )
        {
            TableOperation retrieveUserOperation = TableOperation.Retrieve<UserEntity>( string.Empty, email );
            TableResult retrievedUser = tableUser.Execute( retrieveUserOperation );

            UserEntity e = (UserEntity)retrievedUser.Result;

            return e.SpotifyAccessToken;
        }

        public static string GetSpotifyUserIdByEmail( string email )
        {
            TableOperation retrieveUserOperation = TableOperation.Retrieve<UserEntity>( string.Empty, email );
            TableResult retrievedUser = tableUser.Execute( retrieveUserOperation );

            UserEntity e = (UserEntity)retrievedUser.Result;

            return e.SpotifyId;
        }

        public static void InsertOrUpdateUserBySpotify( UserEntity spotifyUser )
        {
            // Create a retrieve operation that takes a customer entity.
            TableOperation retrieveOperation = TableOperation.Retrieve<UserEntity>( string.Empty, spotifyUser.RowKey );

            // Execute the retrieve operation.
            TableResult retrievedResult = tableUser.Execute( retrieveOperation );
            UserEntity retrievedUser = (UserEntity)retrievedResult.Result;

            if (retrievedResult.Result != null && (retrievedUser.SpotifyId != spotifyUser.SpotifyId || retrievedUser.SpotifyAccessToken != spotifyUser.SpotifyAccessToken ))
            {
                retrievedUser.SpotifyRefreshToken = spotifyUser.SpotifyRefreshToken;
                retrievedUser.SpotifyAccessToken = spotifyUser.SpotifyAccessToken;
                retrievedUser.SpotifyId = spotifyUser.SpotifyId;

                TableOperation updateOperation = TableOperation.Replace( retrievedUser );
                tableUser.Execute( updateOperation );
            }
            else if (retrievedUser == null)
            {
                TableOperation insertOperation = TableOperation.Insert( spotifyUser );
                tableUser.Execute( insertOperation );
            }
        }

        public static void InsertOrUpdateUserByDeezer(UserEntity deezerUser)
        {
            // Create a retrieve operation that takes a customer entity.
            TableOperation retrieveOperation = TableOperation.Retrieve<UserEntity>(string.Empty, deezerUser.RowKey);

            // Execute the retrieve operation.
            TableResult retrievedResult = tableUser.Execute(retrieveOperation);
            UserEntity retrievedUser = (UserEntity)retrievedResult.Result;

            if (retrievedResult.Result != null && retrievedUser.DeezerId != deezerUser.DeezerId)
            {
                //retrievedUser.DeezerRefreshToken = deezerUser.SpotifyRefreshToken;
                retrievedUser.DeezerAccessToken = deezerUser.SpotifyAccessToken;
                retrievedUser.DeezerId = deezerUser.SpotifyId;

                TableOperation updateOperation = TableOperation.Replace(retrievedUser);
            }
            else if (retrievedUser == null)
            {
                TableOperation insertOperation = TableOperation.Insert(deezerUser);
                tableUser.Execute(insertOperation);
            }
        }
        
        public static void InsertOrUpdateUserByFacebook( UserEntity facebookUser )
        {
            // Create a retrieve operation that takes a customer entity.
            TableOperation retrieveOperation = TableOperation.Retrieve<UserEntity>( string.Empty, facebookUser.RowKey );

            // Execute the retrieve operation.
            TableResult retrievedResult = tableUser.Execute( retrieveOperation );
            UserEntity retrievedUser = (UserEntity)retrievedResult.Result;

            if (retrievedResult.Result != null && (retrievedUser.FacebookId != facebookUser.FacebookId || retrievedUser.FacebookAccessToken != facebookUser.FacebookAccessToken))
            {
                retrievedUser.FacebookId = facebookUser.FacebookId;
                retrievedUser.FacebookAccessToken = facebookUser.FacebookAccessToken;

                TableOperation updateOperation = TableOperation.Replace( retrievedUser );
                tableUser.Execute( updateOperation );
            }
            else if (retrievedUser == null)
            {
                TableOperation insertOperation = TableOperation.Insert( facebookUser );
                tableUser.Execute( insertOperation );
            }
        }

        public static void InsertSpotifyTrack(string userId, string playlistId, string trackId, string title, string albumName, string popularity, string cover )
        {
            TableOperation retrieveTrackOperation = TableOperation.Retrieve<TrackEntity>( userId, "s:" +playlistId+ ":" +trackId );
            
            TableResult retrievedResult = tableTrack.Execute( retrieveTrackOperation );
            if (retrievedResult == null)
            {
                TableBatchOperation batchOperation = new TableBatchOperation();
                TrackEntity t = new TrackEntity( "s", userId, playlistId, trackId, title, albumName, popularity, cover );
                batchOperation.Insert( t );
                tableTrack.ExecuteBatch( batchOperation );
            }
            CloudQueueMessage message = new CloudQueueMessage("s:" + trackId);
            queue.AddMessageAsync(message);
        }

        public static void InsertSpotifyPlaylist(PlaylistEntity p)
        {
            TableOperation retrievePlaylistOperation = TableOperation.Retrieve<PlaylistEntity>( p.PartitionKey, p.RowKey );

            TableResult retrievedResult = tablePlaylist.Execute( retrievePlaylistOperation );
            if (retrievedResult.Result == null)
            {
                TableBatchOperation batchOperation = new TableBatchOperation();
                batchOperation.Insert( p );
                tablePlaylist.ExecuteBatch( batchOperation );
            }
        }

        public static List<PlaylistEntity>GetAllPlaylistFromOwner( string email )
        {
            List<PlaylistEntity> allPlaylistsFromOwner = new List<PlaylistEntity>();

            TableQuery<PlaylistEntity> queryPlaylists = new TableQuery<PlaylistEntity>().Where(
                    TableQuery.GenerateFilterCondition( "PartitionKey", QueryComparisons.Equal, DatabaseQueries.GetSpotifyUserIdByEmail(email) ) );

            foreach( PlaylistEntity p in tablePlaylist.ExecuteQuery( queryPlaylists ))
            {
                List<TrackEntity> allTracksInPlaylist = new List<TrackEntity>();

                TableQuery<TrackEntity> rangeQueryTracks = new TableQuery<TrackEntity>().Where(
                    TableQuery.CombineFilters(
                        TableQuery.GenerateFilterCondition( "PartitionKey", QueryComparisons.Equal, GetSpotifyUserIdByEmail(email) ),
                        TableOperators.And,
                        TableQuery.GenerateFilterCondition( "PlaylistId", QueryComparisons.Equal, p.PlaylistId ) ) );

                foreach( TrackEntity t in tableTrack.ExecuteQuery( rangeQueryTracks ))
                {
                    allTracksInPlaylist.Add( t );
                }
                p.Tracks = allTracksInPlaylist;
                allPlaylistsFromOwner.Add( p );
            }

            return allPlaylistsFromOwner;
        }
    }
}
