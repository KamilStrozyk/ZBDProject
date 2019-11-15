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
    public class ServicePointWorkersController : Controller
    {
        private connectionString db = new connectionString();

        // GET: ServicePointWorkers
        public async Task<ActionResult> Index()
        {
            return View(await db.ServicePointWorkers.ToListAsync());
        }

        // GET: ServicePointWorkers/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServicePointWorker servicePointWorker = await db.ServicePointWorkers.FindAsync(id);
            if (servicePointWorker == null)
            {
                return HttpNotFound();
            }
            return View(servicePointWorker);
        }

        // GET: C
        public ActionResult Create()
        {
            return View();
        }

        // POST: ServicePointWorkers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,firstName,lastName,age,salary,employed")] ServicePointWorker servicePointWorker)
        {
            if (ModelState.IsValid)
            {
                db.ServicePointWorkers.Add(servicePointWorker);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(servicePointWorker);
        }

        // GET: ServicePointWorkers/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServicePointWorker servicePointWorker = await db.ServicePointWorkers.FindAsync(id);
            if (servicePointWorker == null)
            {
                return HttpNotFound();
            }
            return View(servicePointWorker);
        }

        // POST: ServicePointWorkers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,firstName,lastName,age,salary,employed")] ServicePointWorker servicePointWorker)
        {
            if (ModelState.IsValid)
            {
                db.Entry(servicePointWorker).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(servicePointWorker);
        }

        // GET: ServicePointWorkers/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServicePointWorker servicePointWorker = await db.ServicePointWorkers.FindAsync(id);
            if (servicePointWorker == null)
            {
                return HttpNotFound();
            }
            return View(servicePointWorker);
        }

        // POST: ServicePointWorkers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ServicePointWorker servicePointWorker = await db.ServicePointWorkers.FindAsync(id);
            db.ServicePointWorkers.Remove(servicePointWorker);
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
