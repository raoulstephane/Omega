using Microsoft.WindowsAzure.Storage.Table;
namespace Omega.DataManager
{
    class RowTrackEntity : TableEntity
    {
        public RowTrackEntity( UserEntity user, PlaylistEntity playlist )
        {
            //PartitionKey = user.email;
            //RowKey = ;
        }

        public RowTrackEntity() { }

        public string Title { get; set; }

        public string PhoneNumber { get; set; }
    }
}
