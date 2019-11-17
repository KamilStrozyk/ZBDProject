using ProjektBazyDanych.Logic;
using System.Linq;
using System.Web.Mvc;

namespace ProjektBazyDanych.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            // Testowa operacja czy wszystko działa
            var logic = new UserLogic();
            logic.AddAdmin();
            return View();
        }

        public ActionResult Contact()
        {
            var logic = new UserLogic();
            logic.ChangeAdminEmail("admin2@admin2.com");
            return View();
        }
    }
}