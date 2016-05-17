using System.Collections.Generic;
using System.Threading.Tasks;

namespace Omega.Crawler
{
    public class Undeplicate
    {
        DeezerConnect dc = new DeezerConnect();
        Spotifycation s = new Spotifycation();

        public async Task<List<string>> UndeplicateTrackList(List<UserInfoAndStuff> list)
        {
            List<string> tmp = new List<string>();

            foreach(UserInfoAndStuff u in list)
            {
                if(u.Source == "deezer")
                {
                    DeezerMetadonnees DeezerTrack = await dc.Connect(u.TrackId);
                    string id = await s.Search(DeezerTrack.title, DeezerTrack.artist.name, DeezerTrack.album.title);
                    if (!tmp.Exists(w => w == ("New_" + id) || w == id))
                    {
                        tmp.Add("New_" + id);
                    }
                }
                else if(u.Source == "spotify")
                {
                    if (!tmp.Exists(w => w == ("New_" + u.TrackId) || w == u.TrackId))
                    {
                        tmp.Add("New_" + u.TrackId);
                    }
                }
            }
            return tmp;
        }
    }
}
