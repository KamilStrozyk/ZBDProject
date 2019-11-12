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
            using (var db = new connectionString())
            {
                var animal = new Animal
                {
                    name = "Przemek",
                    sex = "man",
                    age = 21,
                    origin = "XDD"
                };

                db.Animals.Add(animal);
                db.SaveChanges();
            }

            return View();
        }

        public ActionResult Contact()
        {
            string message = "";
            using (var db = new connectionString())
            {
                var query = from animal in db.Animals
                            orderby animal.name
                            select animal;

                foreach (var item in query)
                {
                    message += item.name + ";";
                }
            }
            ViewBag.Message = message;
            return View();
        }
    }
}