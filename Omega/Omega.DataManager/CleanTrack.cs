using Microsoft.WindowsAzure.Storage.Table;

namespace Omega.DataManager
{
    public class CleanTrack : TableEntity
    {
        public CleanTrack(string id, string source)
        {
            this.PartitionKey = "";
            this.RowKey = source + ":"+ id;
        }

        public CleanTrack() { }

        public string Artist { get; set; }

        public string Title { get; set; }

        public string Danceability { get; set; }

        public string Energy { get; set; }

        public string Loudness { get; set; }

        public string Mode { get; set; }

        public string Speechiness { get; set; }

        public string Acousticness { get; set; }

        public string Instrumentalness { get; set; }

        public string Liveness { get; set; }

        public string Valence { get; set; }

        public string Tempo { get; set; }

        public string Id { get; set; }

        public string DeezerId { get; set; }

        public string Source { get; set; }

        public string AlbumName { get; set; }

        public string Popularity { get; set; }
    }
}

