using ProjektBazyDanych.Controllers;
using System.Web.Mvc;
using System.Web.Routing;

namespace ProjektBazyDanych
{
    public class RouteConfig
    {
        public static string login;
        public static void RegisterRoutes(RouteCollection routes)
        {
            //routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            if (LoginController.logged)
            {
                login = LoginController.login;
                routes.Clear();
                routes.MapRoute(
                    name: "AdminPanel",
                    url: "Admin",
                    defaults: new { controller = "Users", action = "Index" }

                );
                routes.MapRoute(
                    name: "LoginPanel",
                    url: "Logout",
                    defaults: new { controller = "Login", action = "Logout" }

                );

                routes.MapRoute(
                    name: "Default",
                    url: "{controller}/{action}/{id}",
                    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                );
            }
            else
            {
                login = LoginController.login;
                routes.Clear();
                routes.MapRoute(
                    name: "login1",
                    url: "{ *.}",

                    defaults: new { controller = "Login", action = "Login" }
                );
                routes.MapRoute(
                name: "login2",
                url: "",

                defaults: new { controller = "Login", action = "Login" }
            );
            }
        }
    }
}