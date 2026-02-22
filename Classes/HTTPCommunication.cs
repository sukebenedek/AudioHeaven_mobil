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
        public async static Task<T?> Get(string url)
        {
            using var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            using var response = await client.SendAsync(request).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                string resultString = response.Content.ReadAsStringAsync().Result;
                T? root = JsonSerializer.Deserialize<T>(resultString);
                return root;
            }
            return null;
        }

        public async static Task<T?> Post(string url, object data)
        {
            using var client = new HttpClient();
            string jsonPayload = JsonSerializer.Serialize(data);
            using var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            using var response = await client.PostAsync(url, content).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                string resultString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return JsonSerializer.Deserialize<T>(resultString);
            }
            return null;
        }
    }
}
