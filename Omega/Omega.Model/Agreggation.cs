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
            foreach (string musique in playlist)
            {
                CleanTrack analysedSong = cr.GetSongCleanTrack(musique);
                if (!organisedPlaylist.Contains(analysedSong.Id))
                {
                    organisedPlaylist.Add(analysedSong.Id);
                }
            }
            return organisedPlaylist;
        }
    }
}
