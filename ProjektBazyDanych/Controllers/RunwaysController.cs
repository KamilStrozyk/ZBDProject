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
    public class RunwaysController : Controller
    {
        private connectionString db = new connectionString();

        // GET: Runways
        public async Task<ActionResult> Index()
        {
            return View(await db.Runways.ToListAsync());
        }

        // GET: Runways/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Runway runway = await db.Runways.FindAsync(id);
            if (runway == null)
            {
                return HttpNotFound();
            }
            return View(runway);
        }

        // GET: Runways/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Runways/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,area,animalCount")] Runway runway)
        {
            if (ModelState.IsValid)
            {
                db.Runways.Add(runway);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(runway);
        }

        // GET: Runways/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Runway runway = await db.Runways.FindAsync(id);
            if (runway == null)
            {
                return HttpNotFound();
            }
            return View(runway);
        }

        // POST: Runways/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,area,animalCount")] Runway runway)
        {
            if (ModelState.IsValid)
            {
                db.Entry(runway).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(runway);
        }

        // GET: Runways/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Runway runway = await db.Runways.FindAsync(id);
            if (runway == null)
            {
                return HttpNotFound();
            }
            return View(runway);
        }

        // POST: Runways/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Runway runway = await db.Runways.FindAsync(id);
            db.Runways.Remove(runway);
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
