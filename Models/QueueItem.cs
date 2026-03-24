using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioHeaven.Models
{
    public class QueueItem
    {
        public int Id { get; set; }
        public int Position { get; set; }
        public int UserId { get; set; }
        public int SongId { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Song Song { get; set; } = new();
    }

    public class QueueResponse
    {
        public List<QueueItem> Queue { get; set; } = new();
    }
}
