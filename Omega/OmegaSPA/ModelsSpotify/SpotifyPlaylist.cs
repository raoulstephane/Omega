using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace OmegaSPA.ModelsSpotify
{
    public class SpotifyPlaylist
    {
        public string OwnerId { get; set; }
        public string Id { get; set; }
        public List<SpotifyTrack> Tracks { get; set; }
        public string Name { get; set; }

        public SpotifyPlaylist(string ownerId, string id, List<SpotifyTrack> tracks, string name )
        {
            OwnerId = ownerId;
            Id = id;
            Tracks = tracks;
            Name = name;
        }

        //public List<Image> images { get; set; }
    }
}