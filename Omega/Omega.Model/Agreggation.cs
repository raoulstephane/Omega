using Newtonsoft.Json.Linq;
using Omega.DataManager;
using System.Collections.Generic;
using System.Linq;

namespace Omega.Model
{
    public class Agreggation
    {
        Requests cr = new Requests();
        public JArray Agreggate(string playlists)
        {
            List<string> organisedPlaylist = new List<string>();
            Dictionary<JToken, double> organised = new Dictionary<JToken, double>();
            JArray playlistObj = JArray.Parse(playlists);
            JArray organisedArray = new JArray();
            string trackIdSource;

            foreach (var playlist in playlistObj)
            {
                foreach (var track in playlist)
                {
                    trackIdSource = track["RowKey"].ToString().Substring(0, 1) + ":" + track["TrackId"].ToString();
                    CleanTrack analysedSong = cr.GetSongCleanTrack(trackIdSource);
                    if (!organisedPlaylist.Contains(trackIdSource) && analysedSong.Danceability != null)
                    {
                        organisedPlaylist.Add(trackIdSource);
                        organised.Add(track, double.Parse(analysedSong.Danceability.Replace(".", ",")));
                    }
                }
            }
            var items = from pair in organised
                        orderby pair.Value descending
                        select pair;
            foreach (KeyValuePair<JToken, double> track in items)
            {
                organisedArray.Add(track.Key);
            }
            return organisedArray;
        }
    }
}
