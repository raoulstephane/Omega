using System.Collections.Generic;

namespace Omega.Crawler
{
    public class Undeplicate
    {
        public List<string> UndeplicateTrackList(List<UserInfoAndStuff> list)
        {
            List<string> tmp = new List<string>();

            foreach(UserInfoAndStuff u in list)
            {
                if(!tmp.Exists(w => w == u.TrackId))
                {
                    tmp.Add("New_" + u.TrackId);
                }
            }
            return tmp;
        }
    }
}
