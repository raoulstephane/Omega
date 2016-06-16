using NUnit.Framework;
using Omega.Crawler;
using Omega.DataManager;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Omega.Model.Tests
{
    [TestFixture]
    public class TestClass
    {
        [Test]
        public void Test()
        {
            List<string> playlist = new List<string>();

            MetaDonnees askedDonnees = new MetaDonnees();
            askedDonnees.acousticness = "0.51";
            playlist.Add("d:3135556");
            playlist.Add("s:0eGsygTp906u18L0Oimnem");
            playlist.Add("s:11hqMWwX7sF3sOGdtijofF"); 
            playlist.Add("s:06AKEBrKUckW0KREUWRnvT");
            double ratio = 0.10;
            Livefusion l = new Livefusion();
            var newPlaylist = l.PlaylistAnalyser(playlist, askedDonnees, ratio);
            Console.WriteLine(newPlaylist);
        }

        [Test]
        public void Open_And_Parse_Modes()
        {
            Ambiance a = new Ambiance();
            a.Ambiancer("Lounge", new List<string>());
        }

        [Test]
        public async Task Analyse_Songs()
        {
            Dictionary<string, string> musiques = ListeMusiqueLounge();
            Spotifycation s = new Spotifycation();
            CredentialAuth c = new CredentialAuth();
            foreach (var i in musiques)
            {
                string id = await s.Search(i.Key, i.Value, null);
                if(id != "")
                {
                    await c.TrackMetadonnee(id);
                }
                Console.WriteLine(id);
            }
        }

        public Dictionary<string, string> ListeMusiqueLounge()
        {
            Dictionary<string, string> musiques = new Dictionary<string, string>();
            musiques.Add("Broke Inside My Mind", "Anitek");
            musiques.Add("Soul Blue Tango", "Mounika");
            musiques.Add("Daily Dozen", "Astat");
            musiques.Add("Talk To Me", "Miranda Shvangiradze");
            musiques.Add("The Bane of Tadziu", "Th.e n.d");
            musiques.Add("Be a king", "Metaharmoniks");
            return musiques;
        }
    }
}
