using ProjektBazyDanych.Logic;
using ProjektBazyDanych.Repository;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ProjektBazyDanych.Controllers
{
    public class UsersController : Controller
    {
        private connectionString db = new connectionString();
        private UserLogic userLogic = new UserLogic();

        [Route("/Admin")]
        // GET: Users
        public async Task<ActionResult> Index(string message = "", bool isGood = false)
        {
            if(!isGood)
                ViewBag.Message = message;
            else
                ViewBag.GoodMessage = message;

            ViewBag.FoodList = new SelectList(db.Foods, "name", "name");
            ViewBag.ServicePointList = new SelectList(db.ServicePoints, "name", "name");
            ViewBag.DiseaesList = new SelectList(db.Diseases, "name", "name");
            return View(await db.Users.ToListAsync());
        }

        //public async Task<ActionResult> Index(string message)
        //{
        //    return View(await db.Users.ToListAsync());
        //}

        // GET: Users/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "login,passwordHash,passwordSalt,email,firstName,lastName")] User user)
        {
            if (ModelState.IsValid)
            {
                user = userLogic.GeneratePassword(user);
                db.Users.Add(user);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "login,passwordHash,passwordSalt,email,firstName,lastName")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                user = userLogic.GeneratePassword(user);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            User user = await db.Users.FindAsync(id);
            db.Users.Remove(user);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> DeleteInfectedAnimals(string name)
        {
            try
            {
                int result = db.deleteInfectedAnimals(name);
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", new { message = "Wystąpił błąd, spróbuj ponownie." });
            };

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> CountFoodRequirement(string name)
        {
            try
            {
                var result = db.countFoodRequirement(name);
                Food food = db.Foods.Where(x =>x.name == name).SingleOrDefault();
                db.Entry(food).State = EntityState.Modified;
                food.requirement = result;
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", new { message = "Wystąpił błąd, spróbuj ponownie." });
            };

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> TotalServicePointIncome(string name)
        {
            try
            {
                int result = db.TotalServicePointIncome(name);
                return RedirectToAction("Index", new { message = "Dochód punktu usług to: " +result.ToString(), isGood = true});
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", new { message = "Wystąpił błąd, spróbuj ponownie." });
            };

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}