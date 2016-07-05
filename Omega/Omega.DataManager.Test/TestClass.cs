using Microsoft.WindowsAzure.Storage.Table;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Omega.DataManager.Test
{
    [TestFixture]
    public class TestClass
    {
        [Test]
        public void TestQueu()
        {
            DatabaseQueries.InsertSpotifyTrack("a","aa","aaa","aaaa","aaaaa","aaaaaa",1111111, "aaaaaaaa" );
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
        public async Task Get_Table_Content()
        {
            Requests r = new Requests();
            CloudTable table = r.ConnectCleanTrackTable();
            TableContinuationToken continuationToken = null;
            TableQuery<CleanTrack> tableQuery = new TableQuery<CleanTrack>();
            TableQuerySegment<CleanTrack> tableQueryResult;
            List<CleanTrack> list = new List<CleanTrack>();

            do
            {
                tableQueryResult = await table.ExecuteQuerySegmentedAsync(tableQuery, continuationToken);
                continuationToken = tableQueryResult.ContinuationToken;
                for (int i = 0; i < tableQueryResult.Results.Count; i++)
                {
                    string trackId = tableQueryResult.Results[i].Id;
                    string trackTitle = tableQueryResult.Results[i].Id;
                    string source = tableQueryResult.Results[i].Source.Substring(0, 1);
                    list.Add(tableQueryResult.Results[i]);
                    //Thread.Sleep(1000);
                }
            } while (continuationToken != null);
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
