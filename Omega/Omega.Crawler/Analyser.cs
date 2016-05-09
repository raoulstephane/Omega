using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omega.Crawler
{
    public class Analyser
    {
        string songIdTest = "06AKEBrKUckW0KREUWRnvT";
        CredentialAuth c = new CredentialAuth();
        public async Task AnalyseNewSong()
        {
            await c.Connect(songIdTest, true);
        }

        public async Task AnalyseSong()
        {
            await c.Connect(songIdTest, false);
        }
    }
}
