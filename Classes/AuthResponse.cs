using AudioHeaven.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioHeaven.Classes
{
    public class AuthResponse
    {
        public User? User { get; set; }
        public string? Token { get; set; }
        public string? Message { get; set; }
        public int? StatusCode { get; set; }
    }
}
