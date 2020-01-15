//using BuildingRestFulAPI.DAL;
//using BuildingRestFulAPI.Models;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Filters;
//using Microsoft.AspNetCore.Routing;
//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Security.Claims;
//using System.Threading.Tasks;

//namespace BuildingRestFulAPI.AuthenticationHelper
//{
//    public class AuthorizeAction : ActionFilterAttribute
//    {
//        //private IConfiguration _configuration;
//        //public AuthorizedAction()
//        //{
//        //    _configuration = new IConfig();
//        //}
//        private readonly StoreContext _context;
//        public AuthorizeAction(StoreContext context)
//        {
//            _context = context;
//        }

//        public override void OnResultExecuting(ResultExecutingContext filterContext)
//        {

//        }

//        public override void OnActionExecuting(ActionExecutingContext filterContext)
//        {
//            base.OnActionExecuting(filterContext);
//            var user = filterContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
//            if (user == null)
//            {
//                filterContext.Result = new RedirectToRouteResult(
//                   new RouteValueDictionary(new { message = "Not Authorized" }));
//                return;
//            }
//            else if(user != null)
//            {
//                var menus = JsonConvert.DeserializeObject<List<Menu>>(filterContext.HttpContext.Session.GetString("menus"));
//                var userId = _context.Managements.Where(c => c.Username == user).Select(c=>c.Id).SingleOrDefault();
//                var roleId = _context.Managements.Where(c => c.Id == userId).Select(c => c.RoleId).SingleOrDefault();
//                var allMenus = (from links in _context.RoleToMenus
//                                join menu in _context.Menus
//                                on links.MenuId equals menu.MenuId
//                                where links.RoleId == roleId
//                                select new { menu.MenuName}).ToList();
//                var menuss = JsonConvert.DeserializeObject<List<Menu>>(filterContext.HttpContext.Session.SetString(allMenus,"menus"));


//                var menus = JsonConvert.DeserializeObject<List<Menu>>(allMenus);
//                    var controllerName = filterContext.RouteData.Values["controller"];
//                    var actionName = filterContext.RouteData.Values["action"];
//                    string url = "/" + controllerName + "/" + actionName;
//                    if (!menus)

//                    {
//                        filterContext.Result = new RedirectToRouteResult(
//                            new RouteValueDictionary { { "controller", "Account" }, { "action", "Login" } });
//                        return;
//                    }
//                }
                
                
//            }
//            if (filterContext.HttpContext.Session.GetString("email") == null)
//            {
//                filterContext.Result = new RedirectToRouteResult(
//                    new RouteValueDictionary { { "controller", "Account" }, { "action", "Login" } });
//                return;
//            }
//            //var menus = JsonConvert.DeserializeObject<List<Menu>>(filterContext.HttpContext.Session.GetString("menus"));
//            //var controllerName = filterContext.RouteData.Values["controller"];
//            //var actionName = filterContext.RouteData.Values["action"];
//            //string url = "/" + controllerName + "/" + actionName;
//            //if (!menus.Where(s => s.Url == url).Any())

//            //{
//            //    filterContext.Result = new RedirectToRouteResult(
//            //        new RouteValueDictionary { { "controller", "Account" }, { "action", "Login" } });
//            //    return;
//            //}
//        }

//    }
//}


  