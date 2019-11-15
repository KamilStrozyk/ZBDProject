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
