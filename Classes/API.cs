using AudioHeaven.Models;
using CommunityToolkit.Maui.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AudioHeaven.Classes
{
    public static class API
    {
        //public static readonly string BaseUrl = "http://10.0.2.2:8000/api";
        public static readonly string BaseUrl = "http://192.168.1.10:8000/api";

        public static async Task<AuthResponse?> LoginAsync(string email, string password)
        {
            var loginData = new
            {
                email = email,
                password = password
            };

            try
            {
                return await HTTPCommunication<AuthResponse>.Post($"{BaseUrl}/login", loginData);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"API HIBA: {ex.Message}");
                return null;
            }
        }

        public static async Task<AuthResponse?> LoginAsyncToken()
        {
            try
            {
                // We call the /me endpoint which returns the current user's data
                var user = await HTTPCommunication<User>.Get($"{BaseUrl}/me");

                if (user != null)
                {
                    // We wrap it in an AuthResponse to match your existing app logic
                    return new AuthResponse
                    {
                        User = user,
                        Token = UserData.Token // Keep using the token we already have
                    };
                }
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Token Login Error: {ex.Message}");
                return null;
            }
        }

        public static async Task<AuthResponse?> RegisterAsync(string name, string email, string password)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            using var content = new MultipartFormDataContent();
            content.Add(new StringContent(name), "username");
            content.Add(new StringContent(email), "email");
            content.Add(new StringContent(password), "password");

            try
            {
                var response = await client.PostAsync($"{BaseUrl}/register", content);
                string resultString = await response.Content.ReadAsStringAsync();

                System.Diagnostics.Debug.WriteLine($"RAW RESPONSE: {resultString}");

                if (response.IsSuccessStatusCode || (int)response.StatusCode == 422)
                {
                    try
                    {
                        return JsonSerializer.Deserialize<AuthResponse>(resultString,
                            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    }
                    catch (JsonException jsonEx)
                    {
                        System.Diagnostics.Debug.WriteLine($"JSON Parsing Error: {jsonEx.Message}");
                        return new AuthResponse { Message = "Unexpected response format from server." };
                    }
                }

                return new AuthResponse { Message = $"Server Error: {response.StatusCode}" };
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Network Error: {ex.Message}");
                return new AuthResponse { Message = "Server is not reachable. Check your connection." };
            }
        }

        public static async Task<bool> LogoutAsync()
        {
            try
            {
                var response = await HTTPCommunication<AuthResponse>.Post($"{BaseUrl}/logout", new { });

                if (response != null && response.StatusCode == 200)
                {
                    UserData.User = null;
                    UserData.Token = null;
                    UserData.DeleteTokenStorage();
                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"LOGOUT HIBA: {ex.Message}");
            }

            return false;
        }

        public static async Task<ObservableCollection<Song>?> GetUserSongsAsync()
        {
            try
            {
                if (UserData.User == null) throw new Exception("There is no user");

                string url = $"{BaseUrl}/users/{UserData.User.Id}/songs";

                var response = await HTTPCommunication<List<Song>>.Get(url);

                return response != null ? response.ToObservableCollection() : new ObservableCollection<Song>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"SONG FETCH HIBA: {ex.Message}");
                return new ObservableCollection<Song>();
            }
        }

        //public static async Task<ObservableCollection<Album>?> GetUserAlbumsAsync()
        //{
        //    try
        //    {
        //        if (UserData.User == null) throw new Exception("There is no user");

        //        string url = $"{BaseUrl}/users/{UserData.User.Id}/albums";

        //        var response = await HTTPCommunication<List<Album>>.Get(url);

        //        return response != null ? response.ToObservableCollection() : new ObservableCollection<Album>();
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Diagnostics.Debug.WriteLine($"Album FETCH HIBA: {ex.Message}");
        //        return new ObservableCollection<Album>();
        //    }
        //}

        public static async Task<ObservableCollection<Album>?> GetAlbumsSearchAsync(string query, int? take = null)
        {
            try
            {
                if (UserData.User == null) throw new Exception("There is no user");

                string url = $"{BaseUrl}/albums?search={query}";

                var response = await HTTPCommunication<List<Album>>.Get(url);

                if (take != null)
                    return response != null ? response.Take(take.Value).ToObservableCollection() : new ObservableCollection<Album>();
                else 
                    return response != null ? response.ToObservableCollection() : new ObservableCollection<Album>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Album FETCH HIBA: {ex.Message}");
                return new ObservableCollection<Album>();
            }
        }

        public static async Task<ObservableCollection<Song>?> GetSongsSearchAsync(string query, int? take = null)
        {
            try
            {
                if (UserData.User == null) throw new Exception("There is no user");

                string url = $"{BaseUrl}/songs?search={query}";

                var response = await HTTPCommunication<List<Song>>.Get(url);

                if (take != null)
                    return response != null ? response.Take(take.Value).ToObservableCollection() : new ObservableCollection<Song>();
                else
                    return response != null ? response.ToObservableCollection() : new ObservableCollection<Song>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Song FETCH HIBA: {ex.Message}");
                return new ObservableCollection<Song>();
            }
        }

        public static async Task<ObservableCollection<User>?> GetUsersSearchAsync(string query, int? take = null)
        {
            try
            {
                if (UserData.User == null) throw new Exception("There is no user");

                string url = $"{BaseUrl}/users?search={query}";

                var response = await HTTPCommunication<List<User>>.Get(url);

                if (take != null)
                    return response != null ? response.Take(take.Value).ToObservableCollection() : new ObservableCollection<User>();
                else
                    return response != null ? response.ToObservableCollection() : new ObservableCollection<User>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"User FETCH HIBA: {ex.Message}");
                return new ObservableCollection<User>();
            }
        }
    }
}
