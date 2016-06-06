using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;
using Omega.DataManager;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Omega.Crawler
{
    class Program
    {
        //static Analyser a = new Analyser();
        //static CredentialAuth c = new CredentialAuth();
        //static Requests r = new Requests();
        //static GetATrack gt = new GetATrack();
        //static DatabaseCreator dc = new DatabaseCreator();
        static Controller ct = new Controller();

        static void Main( string[] args )
        {
            //dc.CreateCleanTrackTable();
            //ct.GetAnalyser().AnalyseNewSong(ct, "3135556", "deezer").Wait();
            Crawl().Wait();
            //CheckQueu().Wait();
        }

        static async Task Crawl()
        {
            for (;;)
            {
                await BrowseTrack();
            }
        }

        public static async Task CheckQueu()
        {
            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));

            // Create the queue client.
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            // Retrieve a reference to a container.
            CloudQueue queue = queueClient.GetQueueReference("myqueue");

            // Create the queue if it doesn't already exist
            queue.CreateIfNotExists();

            CloudQueueMessage message;

            while ((message = await queue.GetMessageAsync()) != null)
            {
                string trackId;
                string source;
                if(message.AsString.Substring(0, 7) == "spotify")
                {
                    trackId = message.AsString.Substring(7);
                    source = "spotify";
                }
                else
                {
                    trackId = message.AsString.Substring(6);
                    source = "deezer";
                }
                await ct.GetAnalyser().AnalyseNewSong(ct, trackId, source);
                await queue.DeleteMessageAsync(message);
                Console.WriteLine("Queu Checked");
                Thread.Sleep(1000);
            }
        }

        public static async Task BrowseTrack()
        {
            CloudTable table = ct.GetRequests().ConnectCleanTrackTable();
            TableContinuationToken continuationToken = null;
            TableQuery<CleanTrack> tableQuery = new TableQuery<CleanTrack>();
            TableQuerySegment<CleanTrack> tableQueryResult;

            do
            {
                tableQueryResult = await table.ExecuteQuerySegmentedAsync(tableQuery, continuationToken);
                continuationToken = tableQueryResult.ContinuationToken;

                for(int i = 0; i < tableQueryResult.Results.Count; i++)
                {
                    await CheckQueu();
                    string trackId = tableQueryResult.Results[i].Id;
                    string source = tableQueryResult.Results[i].Source;
                    await ct.GetAnalyser().AnalyseSong(ct, trackId, source);
                    Console.WriteLine("Table Checked");
                    Thread.Sleep(1000);
                }
            } while (continuationToken != null);
        }
    }
}
