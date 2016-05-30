using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace OmegaSPA.ModelsSpotify
{
    public class SpotifyPlaylist
    {
        public string id { get; set; }

        public List<SpotifyTrack> tracks { get; set; }

        public string name { get; set; }

        // public List<Image> images { get; set; }

        public string ownerId { get; set; }
    }
}