using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AudioHeaven.Classes
{
    public static class API
    {
        public static readonly string BaseUrl = "http://10.0.2.2:8000/api";

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
    }
}
