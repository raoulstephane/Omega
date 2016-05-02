using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omega.Crawler
{
    public class Analyser
    {
        public async Task AnalyseSong()
        {
            CredentialAuth c = new CredentialAuth();
            string songIdTest = "06AKEBrKUckW0KREUWRnvT";
            await c.Connect(songIdTest);
        }
    }
}
