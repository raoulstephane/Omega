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
            throw new System.NotImplementedException();
            //return Livefusion.PlaylistAnalyser(metaDonnees, playlists);
        }
    }
}
