using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OmegaSPA.ModelsDeezer
{
    public class DeezerPlaylist
    {
        //The id of the owner of the Deezer playlist
        public string OwnerId { get; set; }

        //The id of the Deezer playlist
        public string Id { get; set; }

        //List of Deezer tracks
        public List<DeezerTrack> Tracks { get; set; }

        //The name of the playlist
        public string Name { get; set; }

        public DeezerPlaylist(string ownerId, string id, List<DeezerTrack> tracks, string name)
        {
            OwnerId = ownerId;
            Id = id;
            Tracks = tracks;
            Name = name;
        }
    }
}