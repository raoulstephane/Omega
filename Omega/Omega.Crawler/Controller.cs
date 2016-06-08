using Omega.DataManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omega.Crawler
{
    public class Controller
    {
        Analyser a;
        CredentialAuth c;
        Requests r;
        GetATrack gt;
        DatabaseCreator dc;
        DeezerConnect de;
        Spotifycation s;
        SpotifyToDeezer std;

        public Controller()
        {
            a = new Analyser();
            c = new CredentialAuth();
            r = new Requests();
            gt = new GetATrack();
            dc = new DatabaseCreator();
            de = new DeezerConnect();
            s = new Spotifycation();
            std = new SpotifyToDeezer();
        }

        public SpotifyToDeezer SpotifyToDeezer()
        {
            return std;
        }

        public Spotifycation GetSpotifycation()
        {
            return s;
        }

        public Analyser GetAnalyser()
        {
            return a;
        }

        public CredentialAuth GetCredentialAuth()
        {
            return c;
        }

        public Requests GetRequests()
        {
            return r;
        }

        public GetATrack GetGetATrack()
        {
            return gt;
        }

        public DatabaseCreator GetDatabaseCreator()
        {
            return dc;
        }

        public DeezerConnect GetDeezerConnect()
        {
            return de;
        }
    }
}
