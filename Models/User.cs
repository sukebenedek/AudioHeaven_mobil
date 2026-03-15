using AudioHeaven.Classes;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioHeaven.Models
{
        public class User
        {
            public int Id { get; set; }
            public string Name { get; set; } 
            public string? Email { get; set; }
            public string profile_picture { get; set; } 

            public string FullProfilePicUrl => $"{API.BaseUrl.Replace("/api", "")}/{profile_picture}";
        }
}
