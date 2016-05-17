using System.Threading.Tasks;

namespace Omega.Crawler
{
    public class Analyser
    {
        string songIdTest = "06AKEBrKUckW0KREUWRnvT";
        
        public async Task AnalyseNewSong(CredentialAuth c)
        {
            await c.TrackMetadonnee(songIdTest, true);
        }

        public async Task AnalyseSong(CredentialAuth c)
        {
            await c.TrackMetadonnee(songIdTest, false);
        }
    }
}
