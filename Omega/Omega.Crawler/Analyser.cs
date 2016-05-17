using System.Threading.Tasks;

namespace Omega.Crawler
{
    public class Analyser
    {
        public async Task<MetaDonnees> AnalyseNewSong(CredentialAuth c, string trackId)
        {
            return await c.TrackMetadonnee(trackId);
        }

        public async Task AnalyseSong(CredentialAuth c, string trackId)
        {
            await c.TrackMetadonnee(trackId);
        }
    }
}
