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
        public static readonly string BaseUrl = "http://10.0.2.2:8000/api";
        //public static readonly string BaseUrl = "http://192.168.1.10:8000/api";

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
                var response = await HTTPCommunication<Dictionary<string, string>>.Post($"{BaseUrl}/logout", new { });

                if (response != null && response.ContainsKey("message") &&
                    response["message"].Contains("succesfully"))
                {
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
    }
}
