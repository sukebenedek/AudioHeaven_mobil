using AudioHeaven.Classes;
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
        public int Length { get; set; }
        public string StoredAt { get; set; }
        public string Cover { get; set; }
        public int UserId { get; set; }
        public int? AlbumId { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ShortUser User { get; set; }
        public ShortAlbum? Album { get; set; }

        public string FullCoverUrl => $"{API.BaseUrl.Replace("/api", "")}/{Cover}";
        public string FullAudioUrl => $"{API.BaseUrl}/play/{Id}";
        //public string FullCoverUrl => $"http://10.0.2.2:8000/storage/covers/BZn7MoFnKOgsOY30zpxVsohitmnn1MJbE7uxDu1f.jpg";

        public string LengthFormatted
        {
            get
            {
                int hours = Length / 3600;
                int minutes = (Length % 3600) / 60;
                int seconds = Length % 60;

                if (hours > 0)
                    return $"{hours}:{minutes:D2}:{seconds:D2}";
                else
                    return $"{minutes}:{seconds:D2}";
            }
        }
    }

    public class IndexedSong
    {
        public int Index { get; set; }
        public Song Song { get; set; }
    }
}
