using Omega.DataManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omega.Model
{
    public class Agreggation
    {
        Requests cr = new Requests();
        public List<string> Agreggate(List<string> playlist)
        {
            List<string> organisedPlaylist = new List<string>();
            Dictionary<string, double> organised = new Dictionary<string, double>();
            foreach (string musique in playlist)
            {
                CleanTrack analysedSong = cr.GetSongCleanTrack(musique);
                if (!organised.ContainsKey(analysedSong.Id))
                {
                    
                    organised.Add(analysedSong.Id, double.Parse(analysedSong.Danceability.Replace(".",",")));
                }
            }
            var items = from pair in organised
                        orderby pair.Value descending
                        select pair;
            foreach (KeyValuePair<string,double> track in items)
            {
                organisedPlaylist.Add(track.Key);
            }
            return organisedPlaylist;
        }
    }
}
