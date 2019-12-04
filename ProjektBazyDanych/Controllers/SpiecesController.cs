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
        public async Task<ActionResult> Index()
        {
            foreach (var item in db.Spieces)
            {
                item.howMany = item.Animals.Where(x => x.spiece==item.name).Count();
            }
            return View(await db.Spieces.ToListAsync());
        }

        // GET: Spieces/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Spiece spiece = await db.Spieces.FindAsync(id);
            spiece.howMany = spiece.Animals.Where(x => x.spiece == spiece.name).Count();
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
            if (ModelState.IsValid)
            {
                db.Spieces.Add(spiece);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(spiece);
        }

        // GET: Spieces/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Spiece spiece = await db.Spieces.FindAsync(id);
            spiece.howMany = spiece.Animals.Where(x => x.spiece == spiece.name).Count();
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
        public async Task<ActionResult> Edit([Bind(Include = "name,howMany,appetite")] Spiece spiece)
        {
            if (ModelState.IsValid)
            {
                db.Entry(spiece).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(spiece);
        }

        // GET: Spieces/Delete/5
        public async Task<ActionResult> Delete(string id)
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
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            Spiece spiece = await db.Spieces.FindAsync(id);
            db.Spieces.Remove(spiece);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        //GET: Spieces/AddFood/5
        public async Task<ActionResult> AddFood(string id)
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
            string firstFood;
            if (!spieceFood.Any())
                firstFood = db.Foods.Where(x => !spieceFood.Contains(x.name)).FirstOrDefault().name;
            else firstFood = "brak dostępnego jedzenia";

            ViewBag.name = new SelectList(availableFood,"name", "name",firstFood) ;
            return View(spiece);

        }

        //POST: Spieces/AddFood/5
        [HttpPost,ActionName("AddFood")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddFood(string name,string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                Food food = await db.Foods.FindAsync(name);
                Spiece spiece = await db.Spieces.FindAsync(id);
                spiece.Foods.Add(food);
                food.Spieces.Add(spiece);
                db.Entry(spiece).State = EntityState.Modified;
                db.Entry(food).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Details",new { id = spiece.name });
            }
            catch
            {
                Spiece spiece = await db.Spieces.FindAsync(id);
                var spieceFood = spiece.Foods.Select(x => x.name).ToList();
                IEnumerable<Food> availableFood = db.Foods.
                    Where(x => !spieceFood.
                    Contains(x.name));
                string firstFood;
                if (!spieceFood.Any())
                    firstFood = db.Foods.Where(x => !spieceFood.Contains(x.name)).FirstOrDefault().name;
                else firstFood = "brak dostępnego jedzenia";
                ViewBag.name = new SelectList(availableFood, "name", "name", firstFood);
                return View(spiece);
            }
        }
        //GET: Spieces/DeleteFood/5
        public async Task<ActionResult> DeleteFood(string id)
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
            string firstFood;
            if (!spieceFood.Any())
                firstFood = spieceFood.FirstOrDefault();
            else firstFood = "brak dostępnego jedzenia";

            ViewBag.name = new SelectList(spieceFood, "name", "name", firstFood);
            return View(spiece);

        }

        //POST: Spieces/DeleteFood/5
        [HttpPost, ActionName("DeleteFood")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteFood(string name, string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                Food food = await db.Foods.FindAsync(name);
                Spiece spiece = await db.Spieces.FindAsync(id);
                spiece.Foods.Remove(food);
                food.Spieces.Remove(spiece);
                await db.SaveChangesAsync();
                return RedirectToAction("Details", new { id = spiece.name });
            }
            catch
            {
                Spiece spiece = await db.Spieces.FindAsync(id);
                ViewBag.name = new SelectList(db.Foods.Where(x => !spiece.Foods.Contains(x)), "name", "name", db.Foods.Where(x => !spiece.Foods.Contains(x)).FirstOrDefault());
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
