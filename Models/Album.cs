using AudioHeaven.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioHeaven.Models
{
    public class Album
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public string AlbumCover { get; set; }

        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ShortUser User { get; set; }

        public string FullCoverUrl => string.IsNullOrEmpty(AlbumCover)
            ? "default_cover.png"
            : $"{API.BaseUrl.Replace("/api", "")}/{AlbumCover.Replace("app/public", "storage")}";
    }
}