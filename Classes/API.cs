using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioHeaven.Classes
{
    public static class API
    {
        public static readonly string BaseUrl = "http://localhost:8000/api";

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
            catch (Exception)
            {
                return null;
            }
        }
    }
}
