using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingRestFulAPI
{
    public class AppSettings
    {
        public string Secret = "this is my custom Secret key for authnetication";
        public string ClientBaseUrl { get; set; }
    }
}
