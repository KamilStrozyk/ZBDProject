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
    public class ShipmentsController : Controller
    {
        private connectionString db = new connectionString();

        // GET: Shipments
        public async Task<ActionResult> Index()
        {
            var shipments = db.Shipments.Include(s => s.Settlement).Include(s => s.Supplier);
            return View(await shipments.ToListAsync());
        }

        // GET: Shipments/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shipment shipment = await db.Shipments.FindAsync(id);
            if (shipment == null)
            {
                return HttpNotFound();
            }
            return View(shipment);
        }

        // GET: Shipments/Create
        public ActionResult Create()
        {
            ViewBag.id = new SelectList(db.Settlements, "shipmentId", "shipmentId");
            ViewBag.supplierName = new SelectList(db.Suppliers, "name", "name");
            ViewBag.id = new SelectList(db.Settlements, "shipmentId", "shipmentId");
            return View();
        }

        // POST: Shipments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,shipmentDate,supplierName,amount")] Shipment shipment)
        {
            if (ModelState.IsValid)
            {
                db.Shipments.Add(shipment);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.id = new SelectList(db.Settlements, "shipmentId", "shipmentId", shipment.id);
            ViewBag.supplierName = new SelectList(db.Suppliers, "name", "name", shipment.supplierName);
            ViewBag.id = new SelectList(db.Settlements, "shipmentId", "shipmentId", shipment.id);
            return View(shipment);
        }

        // GET: Shipments/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shipment shipment = await db.Shipments.FindAsync(id);
            if (shipment == null)
            {
                return HttpNotFound();
            }
            ViewBag.id = new SelectList(db.Settlements, "shipmentId", "shipmentId", shipment.id);
            ViewBag.supplierName = new SelectList(db.Suppliers, "name", "name", shipment.supplierName);
            ViewBag.id = new SelectList(db.Settlements, "shipmentId", "shipmentId", shipment.id);
            return View(shipment);
        }

        // POST: Shipments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,shipmentDate,supplierName,amount")] Shipment shipment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(shipment).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.id = new SelectList(db.Settlements, "shipmentId", "shipmentId", shipment.id);
            ViewBag.supplierName = new SelectList(db.Suppliers, "name", "name", shipment.supplierName);
            ViewBag.id = new SelectList(db.Settlements, "shipmentId", "shipmentId", shipment.id);
            return View(shipment);
        }

        // GET: Shipments/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shipment shipment = await db.Shipments.FindAsync(id);
            if (shipment == null)
            {
                return HttpNotFound();
            }
            return View(shipment);
        }

        // POST: Shipments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Shipment shipment = await db.Shipments.FindAsync(id);
            db.Shipments.Remove(shipment);
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
