using AudioHeaven.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioHeaven.Models
{
    public class Playlist
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int? UserId { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ShortUser? User { get; set; }
        public List<Song> Songs { get; set; } = new();
        public Pivot? Pivot { get; set; }

        public string FullCoverUrl => Songs.Count() > 0 && Songs[0] != null ? Songs[0].FullCoverUrl : $"{API.BaseUrl.Replace("/api", "")}/storage/defaults/default_album_cover.png";
        //http://127.0.0.1:8000/storage/defaults/default_album_cover.png

        public string SongsCount => $"{Songs?.Count ?? 0} songs";

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

    public class Pivot
    {
        public int PlaylistId { get; set; }
        public int SongId { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
