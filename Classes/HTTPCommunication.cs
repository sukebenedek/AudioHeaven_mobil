using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AudioHeaven.Classes
{
    public static class HTTPCommunication<T> where T : class
    {
            private static readonly JsonSerializerOptions _options = new()
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
            };

        private static readonly HttpClient _client = new()
        {
            Timeout = TimeSpan.FromSeconds(5)
        };

        public async static Task<T?> Get(string url)
            {
                _client.DefaultRequestHeaders.Clear();
                _client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (!string.IsNullOrEmpty(UserData.Token))
                {
                    _client.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", UserData.Token);
                }

                using var response = await _client.GetAsync(url).ConfigureAwait(false);

                if (response != null)
                {
                    string resultString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonSerializer.Deserialize<T>(resultString, _options);
                }
            return null;
        }

        public async static Task<T?> Post(string url, object data)
            {
                _client.DefaultRequestHeaders.Add("Accept", "application/json");

                if (!string.IsNullOrEmpty(UserData.Token))
                {
                        _client.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", UserData.Token);
                }
                //System.Diagnostics.Debug.WriteLine($"SENDING TOKEN: {UserData.Token}");

                string jsonPayload = JsonSerializer.Serialize(data);
                using var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                using var response = await _client.PostAsync(url, content).ConfigureAwait(false);
                System.Diagnostics.Debug.WriteLine($"LOGOUT HIBA: {response}");

                if (response != null)
                {
                    string resultString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var res = JsonSerializer.Deserialize<T>(resultString, _options);

                    // --- NEW LOGIC TO INJECT STATUS CODE ---
                    if (res != null)
                    {
                        // Check if the class T has a property named "StatusCode"
                        var prop = typeof(T).GetProperty("StatusCode");
                        if (prop != null && prop.CanWrite)
                        {
                            // Set it to the actual HTTP status code (e.g., 200, 401, 404)
                            prop.SetValue(res, (int)response.StatusCode);
                        }
                    }
                    return res;
            }
                return null;
            }
    }
}
