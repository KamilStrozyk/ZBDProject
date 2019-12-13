using Microsoft.AspNetCore.Identity;
using ProjektBazyDanych.Logic;
using ProjektBazyDanych.Repository;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace ProjektBazyDanych.Controllers
{
    public class LoginController : Controller
    {
        private connectionString db = new connectionString();
        private PasswordLogic passwordLogic = new PasswordLogic();
        public static bool logged = true;
        public static string login = "";
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login([Bind(Include = "login,passwordHash,passwordSalt,email,firstName,lastName")] User user)
        {
            UserRepository userRepository = new UserRepository();

            if (userRepository.GetAll().Count() == 0)
            {
                UserLogic userLogic = new UserLogic();
                userLogic.AddAdmin();
            }

            User DBuser = await db.Users.FindAsync(user.login);
            if ((DBuser != null) && (passwordLogic.TestPasswordHasher(user.passwordSalt, DBuser.passwordSalt, DBuser.passwordHash)))
            {
                logged = true;
                login = DBuser.login;
                RouteConfig.RegisterRoutes(RouteTable.Routes);
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("passwordSalt", "Niepoprawny login lub hasło");
            return View(user);
        }

        public async Task<ActionResult> Logout()
        {
            logged = false;
            login = "";
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            return RedirectToAction("Login", "Login");
        }
    }
}