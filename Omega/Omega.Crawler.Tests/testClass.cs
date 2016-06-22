using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Omega.Crawler;
using Omega.DataManager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Omega.Crawler.Tests
{
    [TestFixture]
    public class testClass
    {
        [Test]
        public void Test_Unduplicate_TrackList_And_Analyse_Song()
        {
            List<UserInfoAndStuff> users = InsertUser();
            Dictionary<string, MetaDonnees> trackWithInfo = new Dictionary<string, MetaDonnees>();
            Undeplicate u = new Undeplicate();
            Analyser a = new Analyser();
            CredentialAuth c = new CredentialAuth();

            List<string> finale = u.UndeplicateTrackList(users).Result;

            Console.WriteLine("Liste avant Dédoublonnage");
            for (int i = 0; i < users.Count; i++)
            {
                Console.WriteLine(users[i].TrackId);
            }

            Console.WriteLine("\nListe après Dédoublonnage");
            for (int i = 0; i < finale.Count; i++)
            {
                Console.WriteLine(finale[i]);
            }
            
            Console.WriteLine("");

            foreach(string id in finale)
            {
                string tmpId = id.Substring(4);
                //trackWithInfo.Add(id, await a.AnalyseNewSong(c, tmpId));
                Console.WriteLine(tmpId + " est à jour");
            }
        }

        [Test]
        public void Test_Unduplicate_Song()
        {
            List<UserInfoAndStuff> users = InsertUser();
            Undeplicate u = new Undeplicate();

            List<string> finale = u.UndeplicateTrackList(users).Result;

            Console.WriteLine("Liste avant Dédoublonnage");
            for (int i = 0; i < users.Count; i++)
            {
                Console.WriteLine(users[i].TrackId);
            }

            Console.WriteLine("\nListe après Dédoublonnage");
            for (int i = 0; i < finale.Count; i++)
            {
                Console.WriteLine(finale[i]);
            }
        }

        [Test]
        public async Task Test_Connect_Deezer()
        {
            DeezerConnect d = new DeezerConnect();
            Track t = await d.Connect("3135556");
            Console.WriteLine(t.Artist);
            Console.WriteLine(t.AlbumName);
            Console.WriteLine(t.Title);
        }

        [Test]
        public async Task Test_Spotify_search()
        {
            string track = "Broke Inside My Mind";
            string artist = null;
            string album = null;
            Spotifycation s = new Spotifycation();
            var se = await s.Search(track, artist, album);
            MetaDonnees m = new MetaDonnees();
            CredentialAuth ca = new CredentialAuth();
            var te = await ca.TrackMetadonnee(se);
            Console.WriteLine(te);
        }

        [Test]
        public async Task Deezer_to_Spotify()
        {
            string DeezerId = "3135556";
            DeezerConnect d = new DeezerConnect();
            Spotifycation s = new Spotifycation();
            CredentialAuth c = new CredentialAuth();
            Track dm = await d.Connect(DeezerId);
            string spotifyId = await s.Search(dm.Title, dm.Artist, dm.AlbumName);
            await c.TrackMetadonnee(spotifyId);
        }

        [Test]
        public async Task TestAllBase()
        {
            DatabaseCreator db = new DatabaseCreator();
            db.CreateCleanTrackTable();
            Analyser a = new Analyser();
            await a.AnalyseNewSong(new Controller(), "0eGsygTp906u18L0Oimnem", "spotify");
        }

        [Test]
        public void Test()
        {
            DatabaseCreator db = new DatabaseCreator();
            db.CreateCleanTrackTable();
        }

        [Test]
        public void TestRemove()
        {

        }

        public List<UserInfoAndStuff> InsertUser()
        {
            List<UserInfoAndStuff> Users = new List<UserInfoAndStuff>();

            UserInfoAndStuff u1 = new UserInfoAndStuff();
            u1.UserId = "a@a.a";
            u1.Source = "spotify";
            u1.TrackId = "0eGsygTp906u18L0Oimnem";
            Users.Add(u1);

            UserInfoAndStuff u2 = new UserInfoAndStuff();
            u2.UserId = "b@a.a";
            u2.Source = "spotify";
            u2.TrackId = "0eGsygTp906u18L0Oimnem";
            Users.Add(u2);

            UserInfoAndStuff u3 = new UserInfoAndStuff();
            u3.UserId = "c@a.a";
            u3.Source = "spotify";
            u3.TrackId = "06AKEBrKUckW0KREUWRnvT";
            Users.Add(u3);

            UserInfoAndStuff u4 = new UserInfoAndStuff();
            u4.UserId = "d@a.a";
            u4.Source = "spotify";
            u4.TrackId = "11hqMWwX7sF3sOGdtijofF";
            Users.Add(u4);

            UserInfoAndStuff u5 = new UserInfoAndStuff();
            u5.UserId = "e@a.a";
            u5.Source = "spotify";
            u5.TrackId = "06AKEBrKUckW0KREUWRnvT";
            Users.Add(u5);

            UserInfoAndStuff u6 = new UserInfoAndStuff();
            u6.UserId = "e@a.a";
            u6.Source = "deezer";
            u6.TrackId = "3135556";
            Users.Add(u6);

            return Users;
        }
    }
}