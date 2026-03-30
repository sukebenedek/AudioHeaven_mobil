using AudioHeaven.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioHeaven.Classes
{
    public static class UserData 
    {
        public static event EventHandler? UserChanged;

        private static User? _user;
        public static User? User
        {
            get => _user;
            set
            {
                if (_user != value)
                {
                    _user = value;
                    UserChanged?.Invoke(null, EventArgs.Empty);
                }
            }
        }

        public static string? Token { get; set; }
        public static List<Song>? Songs { get; set; }
        public static List<Album>? Albums { get; set; }
        public static string? SearchTerm { get; set; } = "";

        public static async Task<bool> SaveTokenStorage()
        {
            try
            {
                await SecureStorage.Default.SetAsync("auth_token", Token);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static async Task<string> GetTokenStorage()
        {
            return await SecureStorage.Default.GetAsync("auth_token");
        }

        public static async void DeleteTokenStorage()
        {
            SecureStorage.Default.Remove("auth_token");
            Token = null;
            User = null;
        }
    }
}
