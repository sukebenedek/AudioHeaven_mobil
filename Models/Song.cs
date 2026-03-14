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
        public string StoredAt { get; set; }
        public string Cover { get; set; }
        public int UserId { get; set; }
        public int? AlbumId { get; set; }

        public string FullCoverUrl => $"{API.BaseUrl.Replace("/api", "")}/{Cover}";
        //public string FullCoverUrl => $"http://10.0.2.2:8000/storage/covers/BZn7MoFnKOgsOY30zpxVsohitmnn1MJbE7uxDu1f.jpg";

  //      {
  //  "id": 10,
  //  "title": "Masodik Szam2",
  //  "plays": 0,
  //  "stored_at": "app\/public\/songs\/ZGKF7ODgM7hT9aHM68dmxCitWCzhjdb03ryuBT0m.mp3",
  //  "cover": "storage\/covers\/L2Q3q4Lsq4000AZEfZaKyiCA1EH7yOKY3VWP9kwr.jpg",
  //  "user_id": 2,
  //  "album_id": 3,
  //  "created_at": "2026-03-12T07:46:28.000000Z",
  //  "updated_at": "2026-03-12T07:46:28.000000Z",
  //  "user": {
  //    "id": 2,
  //    "name": "Teszt Elek"
  //  },
  //  "album": {
  //    "id": 3,
  //    "title": "Elek albumja m\u00e1sodik felvon\u00e1s"
  //  }
  //},
  //{
  //  "id": 13,
  //  "title": "Ominozus",
  //  "plays": 0,
  //  "stored_at": "app\/public\/songs\/QwFUFw0ve5KFGVMioPUMyFKiHhFO2BHlJpM34B3t.mp3",
  //  "cover": "storage\/covers\/YQ1vqNJrHF0lzMY680QeEfsyWUP3Bdx5nn4CCVzJ.jpg",
  //  "user_id": 9,
  //  "album_id": null,
  //  "created_at": "2026-03-12T17:10:44.000000Z",
  //  "updated_at": "2026-03-12T17:10:44.000000Z",
  //  "user": {
  //    "id": 9,
  //    "name": "asdf"
  //  },
  //  "album": null
  //}
    }
}
