using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omega.DataManager
{
    public class UserEntity : TableEntity
    {
        public UserEntity( string lastName, string firstName )
        {
            //PartitionKey = lastName;
            //RowKey = firstName;
        }

        public UserEntity() { }

        public string email { get; set; }
        //Image image { get; set; }
        public string IdFacebook_Spotify_Deezer { get; set; }
    }
}
