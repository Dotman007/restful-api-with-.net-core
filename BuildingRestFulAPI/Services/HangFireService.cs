using BuildingRestFulAPI.HangFireDAL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingRestFulAPI.Services
{
    public class HangFireService : IHangFireService
    {
        private readonly BankingBaseContext _context = new BankingBaseContext();
        
        public List<RootObject> GetService()
        {
            var output = new RootObject();
            var addedOutput = new List<RootObject>();
            var services = _context.Job.Select(c => c.InvocationData).ToList();
            foreach (var item in services)
            {
                output = (RootObject)JsonConvert.DeserializeObject<RootObject>(item);
                var serialized = JsonConvert.SerializeObject(output);
                addedOutput.Add(output);
            }
            return addedOutput;
        }
    }
}
