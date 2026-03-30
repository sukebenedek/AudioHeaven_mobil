    using AudioHeaven.Classes;
using CommunityToolkit.Mvvm.Messaging.Messages;
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
        public DateTime? UpdatedAt { get; set; }

        public ShortUser? User { get; set; }

        public List<Song>? Songs { get; set; }

        public string FullCoverUrl => string.IsNullOrEmpty(AlbumCover)
            ? "default_album_cover.png"
            : $"{API.BaseUrl.Replace("/api", "")}/{AlbumCover.Replace("app/public", "storage")}";

        public string LengthFormatted
        {
            get
            {
                int totalSeconds = Songs?.Sum(s => s.Length) ?? 0;

                int hours = totalSeconds / 3600;
                int minutes = (totalSeconds % 3600) / 60;
                int seconds = totalSeconds % 60;

                if (hours > 0)
                    return $"{hours}:{minutes:D2}:{seconds:D2}";
                else
                    return $"{minutes}:{seconds:D2}";
            }
        }
    }
}