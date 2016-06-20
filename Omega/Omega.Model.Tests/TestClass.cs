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
            double ratio = 10;
            Livefusion l = new Livefusion();
            var newPlaylist = l.PlaylistAnalyser(playlist, askedDonnees, ratio);
            Console.WriteLine(newPlaylist);
        }

        [Test]
        public void connectionData()
        {
            Requests r = new Requests();
            Console.WriteLine(r.GetSongCleanTrack("d:3135556").Id);
        }

        [Test]
        public void Open_And_Parse_Modes()
        {
            Ambiance a = new Ambiance();
            List<string> musiques = new List<string>();
            musiques.Add("d:3135556");
            musiques.Add("s:0eGsygTp906u18L0Oimnem");
            musiques.Add("s:11hqMWwX7sF3sOGdtijofF");
            musiques.Add("s:06AKEBrKUckW0KREUWRnvT");
            musiques.Add("s:2TOuZquHSQiHHYPNDUBlNb");
            musiques.Add("s:0BXN8iOELMzUhlIIKdhYXx");
            musiques.Add("s:2zzZCSQbKvxSCImoCLmWKz");
            musiques.Add("s:3R8dCpuEH2QkkUETNBqmrH");
            musiques.Add("s:2NEAbdfS4laGWAETgYDeiY");
            musiques.Add("s:0TaHfsr7Gt5epPSeVIpwV6");
            List<string> filteredList = a.Ambiancer("Lounge", musiques);
            foreach (string musique in filteredList)
            {
                Console.WriteLine(musiques);
            }
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
                    Console.WriteLine("--------------------------------------");
                    Console.WriteLine(i.Key + ", " + i.Value);
                    Console.WriteLine(id);
                    await c.TrackMetadonnee(id);
                    Console.WriteLine("");
                    var tm = await c.TrackMetadonnee(id);
                    Console.WriteLine("acousticness : " + tm.acousticness);
                    Console.WriteLine("danceability : " + tm.danceability);
                    Console.WriteLine("energy : " + tm.energy);
                    Console.WriteLine("instrumentalness : " + tm.instrumentalness);
                    Console.WriteLine("liveness : " + tm.liveness);
                    Console.WriteLine("loudness : " + tm.loudness);
                    Console.WriteLine("speechiness : " + tm.speechiness);
                    Console.WriteLine("tempo : " + tm.tempo);
                    Console.WriteLine("valence : " + tm.valence);
                }
            }
        }

        public Dictionary<string, string> ListeMusiqueLounge()
        {
            // Lounge
            Dictionary<string, string> musiquesLounge = new Dictionary<string, string>();
            //musiquesLounge.Add("Broke Inside My Mind", "Anitek");
            //musiquesLounge.Add("Lounge", "Zen");
            //musiquesLounge.Add("chill out", "Zen");
            //musiquesLounge.Add("Talk To Me", "Miranda Shvangiradze");
            //musiquesLounge.Add("zen lounge", "zen");
            //musiquesLounge.Add("bar lounge", "bar lounge");
            //musiquesLounge.Add("paris lounge", "bar lounge");
            //musiquesLounge.Add("mood music", "bar lounge");
            //musiquesLounge.Add("dani california", "red hot chili peppers");
            musiquesLounge.Add("lose yourself", "eminem");
            musiquesLounge.Add("mambo no. 5", "lou bega");
            musiquesLounge.Add("stayin' alive", "bee gees");
            musiquesLounge.Add("magic in the air", "magic system");
            musiquesLounge.Add("i gotta feeling", "the black eyed peas");
            return musiquesLounge;
        }
    }
}
