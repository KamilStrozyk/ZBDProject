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
        public async Task<ActionResult> Index()
        {
            var animals = db.Animals.Include(a => a.Runway).Include(a => a.Spiece1);
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
        public async Task<ActionResult> Create([Bind(Include = "id,age,sex,origin,inZooSince,name,spiece,runwayID")] Animal animal)
        {
            if (ModelState.IsValid)
            {
                db.Animals.Add(animal);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.runwayID = new SelectList(db.Runways, "id", "id", animal.runwayID);
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
            ViewBag.runwayID = new SelectList(db.Runways, "id", "id", animal.runwayID);
            ViewBag.spiece = new SelectList(db.Spieces, "name", "name", animal.spiece);
            return View(animal);
        }

        // POST: Animals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,age,sex,origin,inZooSince,name,spiece,runwayID")] Animal animal)
        {
            if (ModelState.IsValid)
            {
                db.Entry(animal).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.runwayID = new SelectList(db.Runways, "id", "id", animal.runwayID);
            ViewBag.spiece = new SelectList(db.Spieces, "name", "name", animal.spiece);
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
