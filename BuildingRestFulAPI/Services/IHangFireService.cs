using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingRestFulAPI.Services
{
    public interface IHangFireService
    {
        List<RootObject> GetService();
    }
    public class RootObject
    {
        public string Type { get; set; }
        public string Method { get; set; }
        public string ParameterTypes { get; set; }
        public object Arguments { get; set; }
    }
    //public class SuperRootObject
    //{
    //  public  RootObject objects { get; set; }
    //}
}
