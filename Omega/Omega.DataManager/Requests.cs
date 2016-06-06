using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Omega.DataManager
{
    public class Requests
    {
        public CloudTable ConnectCleanTrackTable()
        {
            // Retrieve the storage account from the connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
            CloudConfigurationManager.GetSetting("StorageConnectionString"));

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Retrieve a reference to the table.
            CloudTable table = tableClient.GetTableReference("CleanTrack");

            return table;
        }

        public void AddSongCleanTrack(MetaDonnees meta, string trackId, string title, string source, string AlbumName, string popularity)
        {
            CloudTable table = ConnectCleanTrackTable();
            string name = source + trackId;
            if (GetSongCleanTrack(name).Id == null)
            {
                CleanTrack track = new CleanTrack(trackId, source);
                track.Id = trackId;
                track.Source = source;
                track.Title = title;
                track.Danceability = meta.danceability;
                track.Loudness = meta.loudness;
                track.Mode = meta.mode;
                track.Speechiness = meta.speechiness;
                track.Acousticness = meta.acousticness;
                track.Instrumentalness = meta.instrumentalness;
                track.Liveness = meta.liveness;
                track.Vanlence = meta.valence;
                track.Tempo = meta.tempo;
                track.AlbumName = AlbumName;
                track.Popularity = popularity;

                // Create the TableOperation object that inserts the customer entity.
                TableOperation insertOperation = TableOperation.Insert(track);

                // Execute the insert operation.
                table.Execute(insertOperation);
            }
        }

        public CleanTrack GetSongCleanTrack(string trackIdSource)
        {
            CleanTrack ct = new CleanTrack(); ;

            CloudTable table = ConnectCleanTrackTable();

            // Create a retrieve operation that takes a customer entity.
            TableOperation retrieveOperation = TableOperation.Retrieve<CleanTrack>("", trackIdSource);

            // Execute the retrieve operation.
            TableResult retrievedResult = table.Execute(retrieveOperation);

            // Print the phone number of the result.
            if (retrievedResult.Result != null)
                ct = (CleanTrack)retrievedResult.Result;

            return ct;
        }

        public void UpdateCleanTrack(MetaDonnees meta, string trackId, string title, string source, string AlbumName, string popularity)
        {
            CloudTable table = ConnectCleanTrackTable();

            // Create a retrieve operation that takes a customer entity.
            TableOperation retrieveOperation = TableOperation.Retrieve<CleanTrack>("", source + trackId);

            // Execute the operation.
            TableResult retrievedResult = table.Execute(retrieveOperation);

            // Assign the result to a CustomerEntity object.
            CleanTrack updateEntity = (CleanTrack)retrievedResult.Result;

            if (updateEntity != null)
            {
                CleanTrack track = new CleanTrack(trackId, source);
                    updateEntity.Id = trackId;
                    updateEntity.Source = source;
                    updateEntity.Title = title;
                    updateEntity.Danceability = meta.danceability;
                    updateEntity.Loudness = meta.loudness;
                    updateEntity.Mode = meta.mode;
                    updateEntity.Speechiness = meta.speechiness;
                    updateEntity.Acousticness = meta.acousticness;
                    updateEntity.Instrumentalness = meta.instrumentalness;
                    updateEntity.Liveness = meta.liveness;
                    updateEntity.Vanlence = meta.valence;
                    updateEntity.Tempo = meta.tempo;
                    updateEntity.AlbumName = AlbumName;
                    updateEntity.Popularity = popularity;

                // Create the Replace TableOperation.
                TableOperation updateOperation = TableOperation.Replace(updateEntity);

                // Execute the operation.
                table.Execute(updateOperation);
            }
        }
    }
}