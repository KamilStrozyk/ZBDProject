using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ProjektBazyDanych.Controllers
{
    public class FoodsController : Controller
    {
        private connectionString db = new connectionString();

        // GET: Foods
        public async Task<ActionResult> Index(string keyword)
        {
            foreach (var item in db.Foods)
            {
                item.requirement = item.Spieces.Select(x => x.appetite * x.howMany).Sum();
            }
            await db.SaveChangesAsync();
            var foods = db.Foods.ToList();
            if (keyword != null)
            {
                foods = db.Foods.Where(x => x.name.Contains(keyword) || x.requirement.ToString().Contains(keyword)).ToList();
                ViewBag.keyword = keyword;
            }
            return View(foods);
        }

        // GET: Foods/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Food food = await db.Foods.FindAsync(id);
            food.requirement = food.Spieces.Select(x => x.appetite * x.howMany).Sum();
            if (food == null)
            {
                return HttpNotFound();
            }
            return View(food);
        }

        // GET: Foods/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Foods/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "name,requirement,currentAmount")] Food food)
        {

            do
            {
                food.id = new Random().Next();
            } while (db.Foods.Where(x => x.id == food.id).ToList().Count > 0);

            if (ModelState.IsValid)
            {
                db.Foods.Add(food);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(food);
        }

        // GET: Foods/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Food food = await db.Foods.FindAsync(id);
            food.requirement = food.Spieces.Select(x => x.appetite * x.howMany).Sum();
            if (food == null)
            {
                return HttpNotFound();
            }
            return View(food);
        }

        // POST: Foods/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "name,requirement,currentAmount")] Food food)
        {
            if (ModelState.IsValid)
            {
                db.Entry(food).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(food);
        }

        //[HttpPost]
        public async Task<ActionResult> CountFood(string name)
        {
            var result = db.countFoodRequirement(name);
            Food food = await db.Foods.FindAsync(name);
            db.Entry(food).State = EntityState.Modified;
            food.requirement = result;
            await db.SaveChangesAsync();
            return RedirectToAction("Details", new { id = food.name });
        }

        // GET: Foods/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Food food = await db.Foods.FindAsync(id);
            food.requirement = food.Spieces.Select(x => x.appetite * x.howMany).Sum();
            if (food == null)
            {
                return HttpNotFound();
            }
            return View(food);
        }

        // POST: Foods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int? id)
        {
            Food food = await db.Foods.FindAsync(id);
            db.Foods.Remove(food);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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