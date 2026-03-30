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
            Timeout = TimeSpan.FromSeconds(12)
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
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

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

                if (res != null)
                {
                    var prop = typeof(T).GetProperty("StatusCode");
                    if (prop != null && prop.CanWrite)
                    {
                        prop.SetValue(res, (int)response.StatusCode);
                    }
                }
                return res;
            }
            return null;
        }

        public async static Task<T?> PostMultipart(string url, MultipartFormDataContent content)
        {
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            if (!string.IsNullOrEmpty(UserData.Token))
            {
                _client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", UserData.Token);
            }

            using var response = await _client.PostAsync(url, content).ConfigureAwait(false);

            string resultString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            System.Diagnostics.Debug.WriteLine($"RAW RESPONSE: {resultString}");

            var res = JsonSerializer.Deserialize<T>(resultString, _options);

            if (res != null)
            {
                var prop = typeof(T).GetProperty("StatusCode");
                if (prop != null && prop.CanWrite)
                {
                    prop.SetValue(res, (int)response.StatusCode);
                }
            }

            return res;
        }

        public async static Task<T?> Delete(string url, object data)
        {
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            if (!string.IsNullOrEmpty(UserData.Token))
            {
                _client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", UserData.Token);
            }

            string jsonPayload = JsonSerializer.Serialize(data);
            using var request = new HttpRequestMessage(HttpMethod.Delete, url)
            {
                Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json")
            };

            using var response = await _client.SendAsync(request).ConfigureAwait(false);

            if (response != null)
            {
                string resultString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var res = JsonSerializer.Deserialize<T>(resultString, _options);

                if (res != null)
                {
                    var prop = typeof(T).GetProperty("StatusCode");
                    if (prop != null && prop.CanWrite)
                    {
                        prop.SetValue(res, (int)response.StatusCode);
                    }
                }

                return res;
            }

            return null;
        }

        public async static Task<T?> Patch(string url, object data)
        {
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            if (!string.IsNullOrEmpty(UserData.Token))
            {
                _client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", UserData.Token);
            }

            string jsonPayload = JsonSerializer.Serialize(data);

            using var request = new HttpRequestMessage(HttpMethod.Patch, url)
            {
                Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json")
            };

            using var response = await _client.SendAsync(request).ConfigureAwait(false);

            if (response != null)
            {
                string resultString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var res = JsonSerializer.Deserialize<T>(resultString, _options);

                if (res != null)
                {
                    var prop = typeof(T).GetProperty("StatusCode");
                    if (prop != null && prop.CanWrite)
                    {
                        prop.SetValue(res, (int)response.StatusCode);
                    }
                }

                return res;
            }

            return null;
        }
    }
}
