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
    public class ServicePointsController : Controller
    {
        private connectionString db = new connectionString();

        // GET: ServicePoints
        public async Task<ActionResult> Index()
        {
            return View(await db.ServicePoints.ToListAsync());
        }

        // GET: ServicePoints/Details/5
        public async Task<ActionResult> Details(string id, string name = "")
        {
            ViewBag.Message = name;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServicePoint servicePoint = await db.ServicePoints.FindAsync(id);
            if (servicePoint == null)
            {
                return HttpNotFound();
            }
            return View(servicePoint);
        }

        public async Task<ActionResult> Income(string name)
        {
            string message = "";
            int val = db.TotalServicePointIncome(name);
            message = "Przychód punktu wynosi: " + val.ToString();
            return RedirectToAction("Details", new { id = name, name = message });
        }
        // GET: ServicePoints/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ServicePoints/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "name,howManyWorkers,income,type")] ServicePoint servicePoint)
        {
            if (ModelState.IsValid)
            {
                db.ServicePoints.Add(servicePoint);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(servicePoint);
        }

        // GET: ServicePoints/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServicePoint servicePoint = await db.ServicePoints.FindAsync(id);
            if (servicePoint == null)
            {
                return HttpNotFound();
            }
            return View(servicePoint);
        }

        // POST: ServicePoints/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "name,howManyWorkers,income,type")] ServicePoint servicePoint)
        {
            if (ModelState.IsValid)
            {
                db.Entry(servicePoint).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(servicePoint);
        }

        // GET: ServicePoints/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServicePoint servicePoint = await db.ServicePoints.FindAsync(id);
            if (servicePoint == null)
            {
                return HttpNotFound();
            }
            return View(servicePoint);
        }

        // POST: ServicePoints/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            ServicePoint servicePoint = await db.ServicePoints.FindAsync(id);
            db.ServicePoints.Remove(servicePoint);
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
