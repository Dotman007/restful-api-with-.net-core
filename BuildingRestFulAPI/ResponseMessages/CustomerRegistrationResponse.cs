using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingRestFulAPI.ResponseMessages
{
    public struct CustomerRegistrationResponse
    {
        public string Response { get; set; }

        public string StatusCode { get; set; }

        public string ResponseStatus { get; set; }
    }

}
