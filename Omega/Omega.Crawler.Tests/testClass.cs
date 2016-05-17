using NUnit.Framework;
using Omega.Crawler;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Omega.Crawler.Tests
{
    [TestFixture]
    public class testClass
    {
        [Test]
        public void Test_Unduplicate_TrackList()
        {
            List<UserInfoAndStuff> users = InsertUser();
            Undeplicate u = new Undeplicate();

            List<string> finale = u.UndeplicateTrackList(users);

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
            await d.Connect("3135556");
        }

        [Test]
        public async Task Test_Spotify_search()
        {
            string track = "hysteria";
            string artist = "Muse";
            string album = "absolution";
            Spotifycation s = new Spotifycation();
            await s.Search(track, artist, album);
        }

        [Test]
        public async Task Deezer_to_Spotify()
        {
            string DeezerId = "3135556";
            DeezerConnect d = new DeezerConnect();
            Spotifycation s = new Spotifycation();
            CredentialAuth c = new CredentialAuth();
            DeezerMetadonnees dm = await d.Connect(DeezerId);
            string spotifyId = await s.Search(dm.title, dm.artist.name, dm.album.title);
            await c.TrackMetadonnee(spotifyId, false);
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

            return Users;
        }
    }
}