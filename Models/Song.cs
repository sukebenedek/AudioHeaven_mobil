using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AudioHeaven.Models
{
    public class Song
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Plays { get; set; }
        public string StoredAt { get; set; }
        public string Cover { get; set; }
        public int UserId { get; set; }
        public int? AlbumId { get; set; } 

        public string FullCoverUrl => $"http://10.0.2.2:8000/storage/{Cover.Replace("app/public/", "")}";
    }
}
