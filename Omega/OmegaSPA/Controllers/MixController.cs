using Newtonsoft.Json.Linq;
using Omega.Model;
using System.Web.Http;

namespace OmegaSPA.Controllers
{
    public class MixController : ApiController
    {
        [HttpPost]
        [Route("Mix/LiveFusion")]
        public JArray MixLiveFusion([FromBody] JToken data)
        {
            string meta = data["metadata"].ToString();
            JArray playlists = (JArray)data["checkedTracks"];
            string playlistsS = playlists.ToString();
            throw new System.NotImplementedException();
            //return Livefusion.PlaylistAnalyser(playlists, metaDonnees);
        }

        [HttpPost]
        [Route("Mix/Ambiance")]
        public JArray MixAmbiance([FromBody] JToken data)
        {
            string ambiance = data["ambiance"].ToString();
            JArray playlists = (JArray)data["checkedTracks"];
            string playlistsS = playlists.ToString();
            return Ambiance.Ambiancer(playlistsS, ambiance);
        }
    }
}
