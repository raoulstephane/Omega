using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Omega.Crawler;
using Omega.DataManager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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
            Livefusion l = new Livefusion();
        }

        [Test]
        public void connectionData()
        {
            Requests r = new Requests();
            Console.WriteLine(r.GetSongCleanTrack("d:3135556").Id);
        }

        [Test]
        public void LiveFusion_Test_With_String()
        {
            Requests r = new Requests();
            Livefusion lf = new Livefusion();
            GetATrack gt = new GetATrack();

            string playlists = GetstringPlaylist();
            JArray filteredList = lf.PlaylistAnalyser(playlists, GetstringMetadonnees());
            foreach (var musique in filteredList)
            {
                Console.WriteLine((string)musique["AlbumName"]);
            }
        }

        [Test]
        public void Agreggation_Test_With_String()
        {
            Requests r = new Requests();
            Livefusion lf = new Livefusion();
            GetATrack gt = new GetATrack();
            Agreggation a = new Agreggation();

            string playlists = GetstringPlaylist();
            JArray filteredList = a.Agreggate(playlists);
            foreach (var musique in filteredList)
            {
                Console.WriteLine((string)musique["Title"]);
            }
        }

        [Test]
        public void LiveFusion_Test_With_String_Ité()
        {
            Requests r = new Requests();
            Livefusion lf = new Livefusion();
            GetATrack gt = new GetATrack();

            string playlists = GetstringPlaylist();
            JArray playlistObj = JArray.Parse(playlists);
            foreach (var musique in playlistObj)
            {
                foreach (var track in musique)
                {
                    Console.WriteLine((string)track["Title"]);
                }
            }
            Console.WriteLine("---------------------------------------------------");
            Console.WriteLine("Critère de recherche :\nDanceability : 0.7, Marge : 10%");
            Console.WriteLine("---------------------------------------------------");
            JArray filteredList = lf.PlaylistAnalyser(playlists, GetstringMetadonnees());
            foreach (var musique in filteredList)
            {
                Console.WriteLine((string)musique["Title"]);
                var test = r.GetSongCleanTrack(musique["RowKey"].ToString().Substring(0, 1) + ":" + musique["TrackId"].ToString());
                Console.WriteLine("Danceability = " + test.Danceability);
            }
        }

        [Test]
        public void Get_Filtered_Playlist_Using_Ambiancer_Ité_Lounge()
        {
            Ambiance a = new Ambiance();
            GetATrack gt = new GetATrack();
            JArray playlistObj = JArray.Parse(GetstringPlaylist());
            foreach (var musique in playlistObj)
            {
                foreach (var track in musique)
                {
                    Console.WriteLine((string)track["Title"]);
                }
            }

            Console.WriteLine("---------------------------------------------------");
            Console.WriteLine("Critère de recherche :\nAmbiance : Lounge");
            Console.WriteLine("---------------------------------------------------");

            JArray filteredList = a.Ambiancer("Lounge", GetstringPlaylist());
            foreach (var musique in filteredList)
            {
                Console.WriteLine((string)musique["Title"]);
            }
        }

        [Test]
        public void Get_Filtered_Playlist_Using_Ambiancer_Ité_Dance()
        {
            Ambiance a = new Ambiance();
            GetATrack gt = new GetATrack();
            JArray playlistObj = JArray.Parse(GetstringPlaylist());
            foreach (var musique in playlistObj)
            {
                foreach (var track in musique)
                {
                    Console.WriteLine((string)track["Title"]);
                }
            }

            Console.WriteLine("---------------------------------------------------");
            Console.WriteLine("Critère de recherche :\nAmbiance : Dance");
            Console.WriteLine("---------------------------------------------------");

            JArray filteredList = a.Ambiancer("Dance", GetstringPlaylist());
            foreach (var musique in filteredList)
            {
                Console.WriteLine((string)musique["Title"]);
            }
        }

        [Test]
        public void Get_Filtered_Playlist_Using_Ambiancer()
        {
            Ambiance a = new Ambiance();
            GetATrack gt = new GetATrack();

            JArray filteredList = a.Ambiancer("Dance", GetstringPlaylist());
            foreach (var musique in filteredList)
            {
                Console.WriteLine((string)musique["AlbumName"]);
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
                    var tm = await c.TrackMetadonnee(id);
                    if (double.Parse(tm.danceability.Replace(".",",")) > 0.7) {
                        Console.WriteLine("");
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
                    else
                    {
                        Console.WriteLine("danceability : " + tm.danceability);
                    }
                }
            }
        }

        public string GetstringPlaylist()
        {
            string playlists;
            using (var streamReader = new StreamReader(@"C:\Users\thibault\Desktop\exemple JSON monard.txt", Encoding.UTF8))
            {
                playlists = streamReader.ReadToEnd();
            }
            return playlists;
        }

        public string GetstringMetadonnees()
        {
            string meta;
            meta = "{'Danceability' : '','Energy' : '','Loudness' : '','Speechiness' : '','Acousticness' : '','Instrumentalness' : '','Liveness' : '','Valence' : '','Tempo' : '','Popularity' : ''}";
            return meta;
        }

        public List<string> GetFilledPlaylist()
        {
            List<string> musiques = new List<string>();
            musiques.Add("d:3135556");
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
            musiques.Add("s:0xbkxTqSaW5blsYgRXpB5I");
            musiques.Add("s:7jQBORjiir0pNSKGaHevq9");
            musiques.Add("s:3LmpQiFNgFCnvAnhhvKUyI");
            musiques.Add("s:3s6ltUrI93LBKU8taezsLn");
            musiques.Add("s:4vp2J1l5RD4gMZwGFLfRAu");
            musiques.Add("s:2Foc5Q5nqNiosCNqttzHof");
            musiques.Add("s:6Z8R6UsFuGXGtiIxiD8ISb");
            musiques.Add("s:41UTnVa6DvcVPUYoXWA97h");
            musiques.Add("s:69RoAhDqFOiQb2pQvb24Ii");
            musiques.Add("s:6xMpUNOfaSkyywPiFFXZFh");
            musiques.Add("s:27pNtiDgRXH9tZM1js0Njc");
            musiques.Add("s:5ghIJDpPoe3CfHMGu71E6T");

            return musiques;
        }

        public Dictionary<string, string> ListeMusiqueLounge()
        {
            // Lounge
            //Dictionary<string, string> musiquesLounge = new Dictionary<string, string>();
            ////musiquesLounge.Add("Broke Inside My Mind", "Anitek");
            ////musiquesLounge.Add("Lounge", "Zen");
            ////musiquesLounge.Add("chill out", "Zen");
            ////musiquesLounge.Add("Talk To Me", "Miranda Shvangiradze");
            ////musiquesLounge.Add("zen lounge", "zen");
            ////musiquesLounge.Add("bar lounge", "bar lounge");
            ////musiquesLounge.Add("paris lounge", "bar lounge");
            ////musiquesLounge.Add("mood music", "bar lounge");
            //return musiquesLounge;

            Dictionary<string, string> musiquesDance = new Dictionary<string, string>();
            musiquesDance.Add("requiem", "mozart");
            musiquesDance.Add("les quatre saisons, l'été", "vivaldi");
            musiquesDance.Add("the final countdown", "europe");
            //musiquesDance.Add("valse", "evgeny grinko");
            //musiquesDance.Add("du hast", "rammstein");
            //musiquesDance.Add("ich will", "rammstein");
            //musiquesDance.Add("funkin' for jamaica", "tom browne");
            //musiquesDance.Add("mambo no. 5", "lou bega");
            //musiquesDance.Add("stayin' alive", "bee gees");
            //musiquesDance.Add("magic in the air", "magic system");
            //musiquesDance.Add("i gotta feeling", "the black eyed peas");
            //musiquesDance.Add("get lucky", "daft punk");
            //musiquesDance.Add("safe and sound", "capital cities");
            return musiquesDance;
        }
    }
}
