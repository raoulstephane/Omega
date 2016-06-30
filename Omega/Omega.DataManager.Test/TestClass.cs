using NUnit.Framework;
using System;

namespace Omega.DataManager.Test
{
    [TestFixture]
    public class TestClass
    {
        [Test]
        public void TestQueu()
        {
            DatabaseQueries.InsertSpotifyTrack("a","aa","aaa","aaaa","aaaaa","aaaaaa","aaaaaaa");
        }

        [Test]
        public void Create_Table()
        {
            Requests r = new Requests();
            r.ConnectCleanTrackTable();
        }

        [Test]
        public void Add_Track()
        {
            Requests r = new Requests();
            r.AddSongCleanTrack(new MetaDonnees(), "", "", "2zzZCSQbKvxSCImoCLmWKz", "", "s", "", "");
        }

        [Test]
        public void Create_DataTable()
        {
            DatabaseCreator dc = new DatabaseCreator();
            dc.CreateCleanTrackTable();
        }

        [Test]
        public void Get_Track()
        {
            Requests r = new Requests();
            r.GetSongCleanTrack("s:2zzZCSQbKvxSCImoCLmWKz");
            Console.WriteLine();
        }

        [Test]
        public void Delete_Track()
        {
            Requests r = new Requests();
            r.DeleteTrack("s:2zzZCSQbKvxSCImoCLmWKz");
            Console.WriteLine();
        }
    }
}
