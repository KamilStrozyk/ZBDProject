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
    public class SettlementsController : Controller
    {
        private connectionString db = new connectionString();

        // GET: Settlements
        public async Task<ActionResult> Index()
        {
            var settlements = db.Settlements.Include(s => s.Shipment).Include(s => s.Shipment1);
            return View(await settlements.ToListAsync());
        }

        // GET: Settlements/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Settlement settlement = await db.Settlements.FindAsync(id);
            if (settlement == null)
            {
                return HttpNotFound();
            }
            return View(settlement);
        }

        // GET: Settlements/Create
        public ActionResult Create()
        {
            ViewBag.shipmentId = new SelectList(db.Shipments, "id", "supplierName");
            ViewBag.shipmentId = new SelectList(db.Shipments, "id", "supplierName");
            return View();
        }

        // POST: Settlements/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "shipmentId,creationDate,modificationDate,year,month,sum,approved")] Settlement settlement)
        {
            if (ModelState.IsValid)
            {
                db.Settlements.Add(settlement);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.shipmentId = new SelectList(db.Shipments, "id", "supplierName", settlement.shipmentId);
            ViewBag.shipmentId = new SelectList(db.Shipments, "id", "supplierName", settlement.shipmentId);
            return View(settlement);
        }

        // GET: Settlements/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Settlement settlement = await db.Settlements.FindAsync(id);
            if (settlement == null)
            {
                return HttpNotFound();
            }
            ViewBag.shipmentId = new SelectList(db.Shipments, "id", "supplierName", settlement.shipmentId);
            ViewBag.shipmentId = new SelectList(db.Shipments, "id", "supplierName", settlement.shipmentId);
            return View(settlement);
        }

        // POST: Settlements/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "shipmentId,creationDate,modificationDate,year,month,sum,approved")] Settlement settlement)
        {
            if (ModelState.IsValid)
            {
                db.Entry(settlement).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.shipmentId = new SelectList(db.Shipments, "id", "supplierName", settlement.shipmentId);
            ViewBag.shipmentId = new SelectList(db.Shipments, "id", "supplierName", settlement.shipmentId);
            return View(settlement);
        }

        // GET: Settlements/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Settlement settlement = await db.Settlements.FindAsync(id);
            if (settlement == null)
            {
                return HttpNotFound();
            }
            return View(settlement);
        }

        // POST: Settlements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Settlement settlement = await db.Settlements.FindAsync(id);
            db.Settlements.Remove(settlement);
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
