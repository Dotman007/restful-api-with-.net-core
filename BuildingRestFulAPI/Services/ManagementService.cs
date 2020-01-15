using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BuildingRestFulAPI.DAL;
using BuildingRestFulAPI.Dtos;
using BuildingRestFulAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace BuildingRestFulAPI.Services
{
    public class ManagementService : IManagement
    {
        private readonly StoreContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        internal DbSet<Management> _dbSet;
        public ManagementService(StoreContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            this._dbSet = _context.Set<Management>();
            _httpContextAccessor = httpContextAccessor;
        }
        public LoginResponse Login(LoginInput input)
        {
            var user = _context.Managements.Where(c => c.Username == input.Username && c.Password == input.Password).SingleOrDefault();
            var getUrl = _context.RoleToMenus.Where(c => c.RoleId == user.RoleId).Select(c => c.Menu.Url).ToList();
            var roles = _context.ManagementRoles.Where(c => c.RoleId == user.RoleId).Select(c => c.RoleName).SingleOrDefault();
            if (user == null)
            {
                return new LoginResponse
                {
                    Message = "Invalid Username or password",
                    Urls = getUrl
                };
            }
            else
            {
                string tokengenerated = "";
                if (user != null)
                {
                    var claims = new[]
                {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                 new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Typ,roles)
                };
                    var signinKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("remitadashboardKey"));
                    var token = new JwtSecurityToken(
                        issuer: "remitadashboard",
                        audience: "remitadashboard",
                        expires: DateTime.UtcNow.AddHours(15),
                        claims: claims,
                        signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(signinKey,
                            SecurityAlgorithms.HmacSha256)
                    );
                    tokengenerated = new JwtSecurityTokenHandler().WriteToken(token);
                    _httpContextAccessor.HttpContext.Session.SetString("username", user.Username);
                    _httpContextAccessor.HttpContext.Session.SetInt32("roleId", (int)user.RoleId);
                    _httpContextAccessor.HttpContext.Session.SetString("token", tokengenerated);

                    int roleId = (int)_httpContextAccessor.HttpContext.Session.GetInt32("roleId");
                    List<Menu> menus = _context.RoleToMenus.Where(c => c.RoleId == roleId).Select(c => c.Menu).ToList();
                    DataSet ds = new DataSet();
                    ds = ToDataSet(menus);
                    DataTable table = ds.Tables[0];
                    DataRow[] parentMenus = table.Select("MenuId = 0");

                    var sb = new StringBuilder();
                    string menuString = GenerateUL(parentMenus, table, sb);
                    _httpContextAccessor.HttpContext.Session.SetString("menuString", menuString);
                    _httpContextAccessor.HttpContext.Session.SetString("menus", JsonConvert.SerializeObject(menus));

                }
                return new LoginResponse
                {
                    Message = "Login Successful",
                    Urls = getUrl,
                    Token = tokengenerated
                };
            }
        }

        public string GenerateUL(DataRow[] menu, DataTable table, StringBuilder sb)
        {
            if (menu.Length > 0)
            {
                foreach (DataRow dr in menu)
                {
                    string url = dr["PageUrl"].ToString();
                    string menuText = dr["MenuName"].ToString();
                    string icon = dr["Icon"].ToString();

                    if (url != "#")
                    {
                        string line = String.Format(@"<li><a href=""{0}""><i class=""{2}""></i> <span>{1}</span></a></li>", url, menuText, icon);
                        sb.Append(line);
                    }
                    string pid = dr["ParentId"].ToString();
                    string parentId = dr["ParentId"].ToString();
                    DataRow[] subMenu = table.Select(String.Format("ParentId = '{0}'", pid));
                    if (subMenu.Length > 0 && !pid.Equals(parentId))
                    {
                        string line = String.Format(@"<li class=""treeview""><a href=""#""><i class=""{0}""></i> <span>{1}</span><span class=""pull-right-container""><i class=""fa fa-angle-left pull-right""></i></span></a><ul class=""treeview-menu"">", icon, menuText);
                        var subMenuBuilder = new StringBuilder();
                        sb.AppendLine(line);
                        sb.Append(GenerateUL(subMenu, table, subMenuBuilder));
                        sb.Append("</ul></li>");
                    }
                }
            }
            return sb.ToString();
        }

        public DataSet ToDataSet<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            DataSet ds = new DataSet();
            ds.Tables.Add(dataTable);
            return ds;
        }
        public string Register(Management management)
        {
            try
            {
                var user = _context.Managements.Where(c => c.Username == management.Username).Count() >= 1;
                if (user == true)
                {
                    return "The user already exist";
                }
                else
                {
                    var users = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    var userId = _context.Managements.Where(c => c.Username == users).Select(c => c.Id).SingleOrDefault();
                    management.CreatorId = userId;
                    management.DateCreated = DateTime.Now;
                    _context.Managements.Add(management);
                    _context.SaveChanges();
                    return "User details added successfully";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
                throw ex;
            }
           

        }

        public List<string> GetCurrentUserUrl()
        {
            var users = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userId = _context.Managements.Where(c => c.Username == users).Select(c => c.RoleId).SingleOrDefault();
            var listUrls = _context.RoleToMenus.Where(c => c.RoleId == userId).Select(c => c.Menu.Url).ToList();
            return listUrls;
        }

        public async Task<LoginResponseMessage> Dashboard()
        {
            GenericRepository<Management> generic = new GenericRepository<Management>(_context, this._dbSet, _httpContextAccessor);
            var user = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var dashboard = await generic.ManagementDashboard(user);
            if (dashboard == null)
            {
                return null;
            }
            return dashboard;
        }

        public async Task<string> ManagementRoleNameAsync(int roleId)
        {
            var roleName = await _context.ManagementRoles.Where(c => c.RoleId == roleId).Select(c => c.RoleName).SingleOrDefaultAsync();
            return roleName;
        }

        Task<LoginResponseMessage> IManagement.ManagementRoleNameAsync(int roleId)
        {
            throw new NotImplementedException();
        }
    }
}
