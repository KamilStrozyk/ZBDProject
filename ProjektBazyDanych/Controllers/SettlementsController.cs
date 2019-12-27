using ProjektBazyDanych.Logic;
using ProjektBazyDanych.Logic.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ProjektBazyDanych.Controllers
{
    public class SettlementsController : Controller
    {
        private connectionString db = new connectionString();
        ISettlementLogic settlementLogic = new SettlementLogic();
        // GET: Settlements
        public async Task<ActionResult> Index(string keyword)
        {
            var settlements = db.Settlements.Include(s => s.Shipment);
            if (keyword != null)
            {
                settlements = db.Settlements.Where(x => x.creationDate.ToString().Contains(keyword) || x.modificationDate.ToString().Contains(keyword) || x.month.ToString().Contains(keyword) || x.year.ToString().Contains(keyword) || x.sum.ToString().Contains(keyword) || x.Shipment.Supplier.name.Contains(keyword) || x.Shipment.shipmentDate.ToString().Contains(keyword)).Include(s => s.Shipment);
                ViewBag.keyword = keyword;
            }
            return View(settlements);
        }

        // GET: Settlements/Details/5
        public async Task<ActionResult> Details(string id)
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

            ViewBag.shipmentId = settlementLogic.CreateShipmentList();
            return View();
        }

        // POST: Settlements/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,shipmentId,creationDate,modificationDate,year,month,sum,approved")] Settlement settlement)
        {
            settlement.creationDate = DateTime.Now;
            settlement.modificationDate = DateTime.Now;
            settlement.approved = false;
            do
            {
                settlement.id = new Random().Next().ToString();
            } while (db.Settlements.Where(x => x.id == settlement.id).ToList().Count > 0);

            if (ModelState.IsValid && db.Settlements.Where(x => x.shipmentId == settlement.shipmentId).ToList().Count == 0)
            {
                db.Settlements.Add(settlement);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("shipmentId", "Istnieje już rozliczenie dla tej dostawy");

            ViewBag.shipmentId = settlementLogic.CreateShipmentList();
            return View(settlement);
        }

        // GET: Settlements/Edit/5
        public async Task<ActionResult> Edit(string id)
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

            ViewBag.shipmentId = settlementLogic.CreateShipmentList(settlement.shipmentId.ToString());
            return View(settlement);
        }

        // POST: Settlements/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,shipmentId,creationDate,modificationDate,year,month,sum,approved")] Settlement settlement)
        {
            settlement.modificationDate = DateTime.Now;
            if (ModelState.IsValid && db.Settlements.Where(x => x.shipmentId == settlement.shipmentId && x.id!=settlement.id).ToList().Count == 0)
            {
                db.Entry(settlement).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("shipmentId", "Istnieje już rozliczenie dla tej dostawy");
            ViewBag.shipmentId = settlementLogic.CreateShipmentList(settlement.shipmentId.ToString());
            return View(settlement);
        }

        // GET: Settlements/Delete/5
        public async Task<ActionResult> Delete(string id)
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
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            Settlement settlement = await db.Settlements.FindAsync(id);
            db.Settlements.Remove(settlement);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Approve(string id)
        {
            Settlement settlement = await db.Settlements.FindAsync(id);
            settlement.approved = true;
            if (ModelState.IsValid)
            {
                db.Entry(settlement).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Details", new { id });

            }
            else
            {
                return RedirectToAction("Details", new { id });
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