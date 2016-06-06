using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Omega.DataManager
{
    class DatabaseQueries
    {
        public void InsertUser(UserEntity user)
        {
            // Retrieve the storage account from the connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
               CloudConfigurationManager.GetSetting( "StorageConnectionString" ) );

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Create the CloudTable object that represents the "people" table.
            CloudTable table = tableClient.GetTableReference( "people" );

            // Create the TableOperation object that inserts the customer entity.
            TableOperation insertOperation = TableOperation.Insert( user );

            // Execute the insert operation.
            table.Execute( insertOperation );
        }
        
    }
}
