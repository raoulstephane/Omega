using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace OmegaSPA.ModelsSpotify
{
    public class SpotifyPlaylist
    {
        public string ownerId { get; set; }
        public string Id { get; set; }
        public List<SpotifyTrack> Tracks { get; set; }
        public string name { get; set; }

        //public List<Image> images { get; set; }
    }
}