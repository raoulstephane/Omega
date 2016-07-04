using Newtonsoft.Json.Linq;
using Omega.Model;
using System.Web.Http;

namespace OmegaSPA.Controllers
{
    public class MixController : ApiController
    {
        [Route("Mix/{metaDonnees}/{playlists}/LiveFusion")]
        public JArray MixLiveFusion(string metaDonnees, string playlists)
        {
            return Livefusion.PlaylistAnalyser(metaDonnees, playlists);
        }
    }
}
