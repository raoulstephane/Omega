using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Omega.DataManager
{
    public class Requests
    {
        public CloudTable ConnectCleanTrackTable()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
            CloudConfigurationManager.GetSetting("StorageConnectionString"));

            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            CloudTable table = tableClient.GetTableReference("CleanTrack");

            return table;
        }

        public void AddSongCleanTrack(MetaDonnees meta,string artist, string deezerId, string trackId, string title, string source, string AlbumName, string popularity)
        {
            source = source.Substring(0, 1);
            CloudTable table = ConnectCleanTrackTable();
            string name = source +":"+ trackId;

            if (GetSongCleanTrack(name).Id == null)
            {
                CleanTrack track = new CleanTrack(trackId, source);
                track.Artist = artist;
                track.DeezerId = deezerId;
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
                track.Valence = meta.valence;
                track.Tempo = meta.tempo;
                track.AlbumName = AlbumName;
                track.Popularity = popularity;
                track.Energy = meta.energy;

                TableOperation insertOperation = TableOperation.Insert(track);

                table.Execute(insertOperation);
            }
        }

        public CleanTrack GetSongCleanTrack(string trackIdSource)
        {
            CleanTrack ct = new CleanTrack();

            CloudTable table = ConnectCleanTrackTable();

            TableOperation retrieveOperation = TableOperation.Retrieve<CleanTrack>("", trackIdSource);

            TableResult retrievedResult = table.Execute(retrieveOperation);

            if (retrievedResult.Result != null)
                ct = (CleanTrack)retrievedResult.Result;

            return ct;
        }

        public void UpdateCleanTrack(MetaDonnees meta, string trackId, string title, string source, string AlbumName, string popularity)
        {
            CloudTable table = ConnectCleanTrackTable();

            TableOperation retrieveOperation = TableOperation.Retrieve<CleanTrack>("", source + ":" + trackId);

            TableResult retrievedResult = table.Execute(retrieveOperation);

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
                    updateEntity.Valence = meta.valence;
                    updateEntity.Tempo = meta.tempo;
                    updateEntity.AlbumName = AlbumName;
                    updateEntity.Popularity = popularity;
                    updateEntity.Energy = meta.energy;

                TableOperation updateOperation = TableOperation.Replace(updateEntity);

                table.Execute(updateOperation);
            }
        }

        public void DeleteTrack(string trackIdSource)
        {
            CleanTrack ct = new CleanTrack();

            CloudTable table = ConnectCleanTrackTable();

            TableOperation retrieveOperation = TableOperation.Retrieve<CleanTrack>("", trackIdSource);

            TableResult retrievedResult = table.Execute(retrieveOperation);

            CleanTrack deleteEntity = (CleanTrack)retrievedResult.Result;

            if (deleteEntity != null)
            {
                TableOperation deleteOperation = TableOperation.Delete(deleteEntity);
                table.Execute(deleteOperation);
            }
        }
    }
}