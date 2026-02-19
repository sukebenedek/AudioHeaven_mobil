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
    }
}
