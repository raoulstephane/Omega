using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omega.Model
{
    public class TrackObj
    {
        public string UserID { get; set; }
        public string TrackId { get; set; }
        public string Title { get; set; }
        public string AlbumName { get; set; }
        public string Popularity { get; set; }
        public string Cover { get; set; }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string Timestamp { get; set; }
    }
}
