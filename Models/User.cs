using AudioHeaven.Classes;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AudioHeaven.Models
{
        public class User
        {
            public int Id { get; set; }
            public string Name { get; set; } = "Default Username";
            public string? Email { get; set; }
            public string profile_picture { get; set; } 

            public string FullProfilePicUrl => string.IsNullOrEmpty(profile_picture)
            ? "default_profile_picture.png"
            : $"{API.BaseUrl.Replace("/api", "")}/{profile_picture}";
        }

    public class UserProfileResponse
    {
        public User User { get; set; }

        public List<Song> Songs { get; set; }

        public List<Album> Albums { get; set; }
    }
}
