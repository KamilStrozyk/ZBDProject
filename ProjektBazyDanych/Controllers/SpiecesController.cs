using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjektBazyDanych;

namespace ProjektBazyDanych.Controllers
{
    public class SpiecesController : Controller
    {
        private connectionString db = new connectionString();

        // GET: Spieces
        public async Task<ActionResult> Index(string keyword)
        {
            foreach (var item in db.Spieces)
            {
                item.howMany = item.Animals.Where(x => x.spiece == item.id).Count();
            }
            await db.SaveChangesAsync();

            var species = db.Spieces.ToList();
            if (keyword != null)
            {
                species = db.Spieces.Where(x => x.name.Contains(keyword) || x.howMany.ToString().Contains(keyword) ||  x.appetite.ToString().Contains(keyword)).ToList();
                ViewBag.keyword = keyword;
            }
            return View(species);
        }

        // GET: Spieces/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Spiece spiece = await db.Spieces.FindAsync(id);
            spiece.howMany = spiece.Animals.Where(x => x.spiece == spiece.id).Count();
            foreach (var item in spiece.Foods)
            {
                item.requirement = item.Spieces.Select(x => x.appetite * x.howMany/x.Foods.Count()).Sum();
            }
            await db.SaveChangesAsync();

            if (spiece == null)
            {
                return HttpNotFound();
            }
            return View(spiece);
        }

        // GET: Spieces/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Spieces/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "name,howMany,appetite")] Spiece spiece)
        {
            if (db.Spieces.Any(x => x.name == spiece.name && x.id != spiece.id))
            {
                ModelState.AddModelError("name", "Taki gatunek już istnieje");
            }
            if (ModelState.IsValid)
            {
                db.Spieces.Add(spiece);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(spiece);
        }

        // GET: Spieces/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Spiece spiece = await db.Spieces.FindAsync(id);
            spiece.howMany = spiece.Animals.Where(x => x.spiece == spiece.id).Count();
            await db.SaveChangesAsync();

            if (spiece == null)
            {
                return HttpNotFound();
            }
            return View(spiece);
        }

        // POST: Spieces/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,name,howMany,appetite")] Spiece spiece)
        {
            if (db.Spieces.Any(x => x.name == spiece.name && x.id != spiece.id))
            {
                ModelState.AddModelError("name", "Taki gatunek już istnieje");
            }
            if (ModelState.IsValid)
            {
                db.Entry(spiece).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(spiece);
        }

        // GET: Spieces/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Spiece spiece = await db.Spieces.FindAsync(id);
            if (spiece == null)
            {
                return HttpNotFound();
            }
            return View(spiece);
        }


        // POST: Spieces/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int? id)
        {
            Spiece spiece = await db.Spieces.FindAsync(id);
            db.Spieces.Remove(spiece);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        //GET: Spieces/AddFood/5
        public async Task<ActionResult> AddFood(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Spiece spiece = await db.Spieces.FindAsync(id);
            if (spiece == null)
            {
                return HttpNotFound();
            }
            var spieceFood = spiece.Foods.Select(x => x.name).ToList();
            IEnumerable<Food> availableFood = db.Foods.
                Where(x => !spieceFood.
                Contains(x.name));


            ViewBag.name = new SelectList(availableFood, "name", "name");
            return View(spiece);

        }

        //POST: Spieces/AddFood/5
        [HttpPost, ActionName("AddFood")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddFood(string name, int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (name == null)
            {
                ModelState.AddModelError("name", "Nie wybrałeś jedzenia");
            }
            if (ModelState.IsValid)
            {
                Food food = await db.Foods.Where(x => x.name == name).SingleOrDefaultAsync();
                Spiece spiece = await db.Spieces.FindAsync(id);
                spiece.Foods.Add(food);
                food.Spieces.Add(spiece);
                db.Entry(spiece).State = EntityState.Modified;
                db.Entry(food).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Details", new { id = spiece.id });
            }
            else
            {
                Spiece spiece = await db.Spieces.FindAsync(id);
                var spieceFood = spiece.Foods.Select(x => x.name).ToList();
                IEnumerable<Food> availableFood = db.Foods.
                    Where(x => !spieceFood.
                    Contains(x.name));

                ViewBag.name = new SelectList(availableFood, "name", "name");
                return View(spiece);
            }
        }
        //GET: Spieces/DeleteFood/5
        public async Task<ActionResult> DeleteFood(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Spiece spiece = await db.Spieces.FindAsync(id);
            if (spiece == null)
            {
                return HttpNotFound();
            }
            var spieceFood = spiece.Foods.Select(x => x.name).ToList();
            IEnumerable<Food> availableFood = db.Foods.
                Where(x => spieceFood.
                Contains(x.name));


            ViewBag.name = new SelectList(availableFood, "name", "name");
            return View(spiece);

        }

        //POST: Spieces/DeleteFood/5
        [HttpPost, ActionName("DeleteFood")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteFood(string name, int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (name == null)
            {
                ModelState.AddModelError("name", "Nie wybrałeś jedzenia");
            }
            if (ModelState.IsValid)
            {
                Food food = await db.Foods.Where(x => x.name == name).SingleOrDefaultAsync();
                Spiece spiece = await db.Spieces.FindAsync(id);
                spiece.Foods.Remove(food);
                food.Spieces.Remove(spiece);
                await db.SaveChangesAsync();
                return RedirectToAction("Details", new { id = spiece.id });
            }
            else
            {
                Spiece spiece = await db.Spieces.FindAsync(id);
                ViewBag.name = new SelectList(db.Foods.Where(x => spiece.Foods.Contains(x)), "name", "name");
                return View(spiece);
            }
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
