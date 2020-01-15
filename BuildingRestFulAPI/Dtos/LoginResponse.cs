using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingRestFulAPI.Dtos
{
    public class LoginResponse
    {
        public string Message { get; set; }

        public List<string> Urls { get; set; }

        public string Token { get; set; }

    }

    public class LoginResponseMessage
    {
        public string Username { get; set; }

        public int UserId { get; set; }

        public string RoleName { get; set; }

        public string Token { get; set; }

    }
}
