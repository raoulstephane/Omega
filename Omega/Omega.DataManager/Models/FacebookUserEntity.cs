using Microsoft.WindowsAzure.Storage.Table;

namespace Omega.DataManager.Models
{
    class FacebookUserEntity : TableEntity
    {
        public string Email { get; set; }

        public FacebookUserEntity() { }

        public FacebookUserEntity( string facebookId, string email )
        {
            PartitionKey = string.Empty;
            RowKey = facebookId;
            Email = email;
        }
    }
}
