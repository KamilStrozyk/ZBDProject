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
    public class AnimalsController : Controller
    {
        private connectionString db = new connectionString();

        // GET: Animals
        public async Task<ActionResult> Index(string keyword)
        {
            var animals = db.Animals.Include(a => a.Runway).Include(a => a.Spiece1);

            if (keyword != null)
            {
                animals = db.Animals.Where(x => x.name.Contains(keyword) || x.age.ToString().Contains(keyword) || x.inZooSince.ToString().Contains(keyword) || x.origin.Contains(keyword) || x.sex.Contains(keyword) || x.Spiece1.name.Contains(keyword) || x.Runway.name.Contains(keyword)).Include(a => a.Runway).Include(a => a.Spiece1);
                ViewBag.keyword = keyword;
            }

            return View(await animals.ToListAsync());
        }

        // GET: Animals/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Animal animal = await db.Animals.FindAsync(id);
            animal.Runway.animalCount = animal.Runway.Animals.Count();
            await db.SaveChangesAsync();
            if (animal == null)
            {
                return HttpNotFound();
            }
            return View(animal);
        }

        // GET: Animals/Create
        public ActionResult Create()
        {

            ViewBag.runway = new SelectList(db.Runways, "name", "name");
            ViewBag.spiece = new SelectList(db.Spieces, "name", "name");
            return View();
        }

        // POST: Animals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,name,age,sex,origin,inZooSince")] Animal animal)
        {
            string spiece = Request.Form["spiece"];
            string runway = Request.Form["runway"];
            try
            {
                animal.runwayID = db.Runways.Where(x => x.name == runway).FirstOrDefault().id;
            }
            catch
            {
                ModelState.AddModelError("runwayID", "Proszę uzupełnić wybieg");
            }
            try
            {
                animal.spiece = db.Spieces.Where(x => x.name == spiece).FirstOrDefault().id;
            }
            catch
            {
                ModelState.AddModelError("spiece", "Proszę uzupełnić gatunek");
            }
            if (animal.inZooSince > DateTime.Today)
            {
                ModelState.AddModelError("inZooSince", "Data nie może być większa od obecnej");
            }
            TimeSpan inZoo = DateTime.Today - animal.inZooSince;
            var yearsInZoo = inZoo.TotalDays / 365.0;
            if (yearsInZoo > Convert.ToDouble(animal.age))
            {
                ModelState.AddModelError("inZooSince", "Wiek zwierzęcia nie może przekraczać czasu jego pobytu w zoo");
            }
            if (ModelState.IsValid)
            {
                db.Animals.Add(animal);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.runway = new SelectList(db.Runways, "name", "name", animal.runwayID);
            ViewBag.spiece = new SelectList(db.Spieces, "name", "name", animal.spiece);
            return View(animal);
        }

        // GET: Animals/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Animal animal = await db.Animals.FindAsync(id);
            if (animal == null)
            {
                return HttpNotFound();
            }
            ViewBag.runway = new SelectList(db.Runways, "name", "name", animal.Runway.name);
            ViewBag.spiece = new SelectList(db.Spieces, "name", "name", animal.Spiece1.name);
            return View(animal);
        }

        // POST: Animals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,age,sex,origin,inZooSince,name")] Animal animal)
        {

            string spiece = Request.Form["spiece"];
            string runway = Request.Form["runway"];
            animal.runwayID = db.Runways.Where(x => x.name == runway).FirstOrDefault().id;
            animal.spiece = db.Spieces.Where(x => x.name == spiece).FirstOrDefault().id;
            if (animal.inZooSince > DateTime.Today)
            {
                ModelState.AddModelError("inZooSince", "Data nie może być większa od obecnej");
            }
            if (ModelState.IsValid)
            {
                db.Entry(animal).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.runwayID = new SelectList(db.Runways, "name", "name", animal.Runway.name);
            ViewBag.spiece = new SelectList(db.Spieces, "name", "name", animal.Spiece1.name);
            return View(animal);
        }

        // GET: Animals/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Animal animal = await db.Animals.FindAsync(id);
            if (animal == null)
            {
                return HttpNotFound();
            }
            return View(animal);
        }

        // POST: Animals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Animal animal = await db.Animals.FindAsync(id);
            db.Animals.Remove(animal);
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
