using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;
using Omega.DataManager.Models;
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
        static readonly CloudTable tableFacebookUser;
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
            tableFacebookUser = tableClient.GetTableReference( "FacebookUser" );

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

        public static string GetEmailByFacebookId( string facebookId )
        {
            TableOperation retrieveUser = TableOperation.Retrieve<FacebookUserEntity>( string.Empty, facebookId );
            TableResult retrievedFacebookUser = tableFacebookUser.Execute( retrieveUser );
            
            FacebookUserEntity f = (FacebookUserEntity)retrievedFacebookUser.Result;
            if (f == null)
                return null;
            else
                return f.Email;
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
            // Create a retrieve operation that takes a User entity.
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

            TableOperation retrieveFacebookUserOperation = TableOperation.Retrieve<FacebookUserEntity>( string.Empty, facebookUser.FacebookId );
            TableResult retrievedFacebookUserResult = tableFacebookUser.Execute( retrieveFacebookUserOperation );
            FacebookUserEntity retrievedFacebookUser = (FacebookUserEntity)retrievedFacebookUserResult.Result;
            if(retrievedFacebookUser == null)
            {
                FacebookUserEntity fUser = new FacebookUserEntity( facebookUser.FacebookId, facebookUser.Email );
                TableOperation insertFacebookUser = TableOperation.Insert( fUser );
                tableFacebookUser.Execute( insertFacebookUser );
            }
        }

        public static void InsertSpotifyTrack(string userId, string playlistId, string trackId, string title, string albumName, string popularity, int duration, string cover )
        {
            TableOperation retrieveTrackOperation = TableOperation.Retrieve<TrackEntity>( userId, "s:" +playlistId+ ":" +trackId );
            
            TableResult retrievedResult = tableTrack.Execute( retrieveTrackOperation );
            TrackEntity retrievedTrack = (TrackEntity)retrievedResult.Result;
            if (retrievedTrack == null)
            {
                TableBatchOperation batchOperation = new TableBatchOperation();
                TrackEntity t = new TrackEntity( "s", userId, playlistId, trackId, title, albumName, popularity, duration, cover );
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

        public static List<PlaylistEntity>GetAllPlaylistsFromOwner( string email )
        {
            List<PlaylistEntity> allPlaylists = new List<PlaylistEntity>();

            TableQuery<PlaylistEntity> queryPlaylists = new TableQuery<PlaylistEntity>().Where(
                TableQuery.GenerateFilterCondition( "PartitionKey", QueryComparisons.Equal, GetSpotifyUserIdByEmail( email ) ) );

            foreach (PlaylistEntity p in tablePlaylist.ExecuteQuery( queryPlaylists ))
            {
                List<TrackEntity> allTracks = new List<TrackEntity>();

                TableQuery<TrackEntity> queryTracks = new TableQuery<TrackEntity>().Where(
                TableQuery.GenerateFilterCondition( "PartitionKey", QueryComparisons.Equal, GetSpotifyUserIdByEmail( email ) ) );

                foreach (TrackEntity track in tableTrack.ExecuteQuery( queryTracks ))
                {
                    if(track.PlaylistId == p.PlaylistId)
                        allTracks.Add( track );
                }
                p.Tracks = allTracks;
                allPlaylists.Add( p );
            }
            return allPlaylists;
        }
    }
}
