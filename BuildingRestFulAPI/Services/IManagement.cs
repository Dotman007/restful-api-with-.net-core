using BuildingRestFulAPI.Dtos;
using BuildingRestFulAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingRestFulAPI.Services
{
    public interface IManagement
    {
        string Register(Management management);
        LoginResponse Login(LoginInput input);
        string GenerateUL(DataRow[] menu, DataTable table, StringBuilder sb);
        DataSet ToDataSet<T>(List<T> items);
        List<string> GetCurrentUserUrl();
        Task<LoginResponseMessage> Dashboard();
        Task<LoginResponseMessage> ManagementRoleNameAsync(int roleId);
    }
}
