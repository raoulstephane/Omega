using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omega.DataManager
{
    public class PlaylistEntity : TableEntity
    {
        public string OwnerId { get; set; }
        public string PlaylistId { get; set; }
        public List<TrackEntity> Tracks { get; set; }
        public string Name { get; set; }
        public string Cover { get; set; }

        public PlaylistEntity( string ownerId, string playlistId, List<TrackEntity> tracks, string name, string cover )
        {
            PartitionKey = ownerId;
            RowKey = playlistId;
            OwnerId = ownerId;
            PlaylistId = playlistId;
            Tracks = tracks;
            Name = name;
            Cover = cover;
        }

        public PlaylistEntity() { }
    }
}
