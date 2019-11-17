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
    public class DiseaseHistoriesController : Controller
    {
        private connectionString db = new connectionString();

        // GET: DiseaseHistories
        public async Task<ActionResult> Index()
        {
            var diseaseHistories = db.DiseaseHistories.Include(d => d.Animal).Include(d => d.Disease);
            return View(await diseaseHistories.ToListAsync());
        }

        // GET: DiseaseHistories/Details/5
        public async Task<ActionResult> Details(DateTime id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DiseaseHistory diseaseHistory = await db.DiseaseHistories.FindAsync(id);
            if (diseaseHistory == null)
            {
                return HttpNotFound();
            }
            return View(diseaseHistory);
        }

        // GET: DiseaseHistories/Create
        public ActionResult Create()
        {
            ViewBag.animalID = new SelectList(db.Animals, "id", "sex");
            ViewBag.diseaseName = new SelectList(db.Diseases, "name", "name");
            return View();
        }

        // POST: DiseaseHistories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "beginDate,animalID,diseaseName,endDate")] DiseaseHistory diseaseHistory)
        {
            if (ModelState.IsValid)
            {
                db.DiseaseHistories.Add(diseaseHistory);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.animalID = new SelectList(db.Animals, "id", "sex", diseaseHistory.animalID);
            ViewBag.diseaseName = new SelectList(db.Diseases, "name", "name", diseaseHistory.diseaseName);
            return View(diseaseHistory);
        }

        // GET: DiseaseHistories/Edit/5
        public async Task<ActionResult> Edit(DateTime id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DiseaseHistory diseaseHistory = await db.DiseaseHistories.FindAsync(id);
            if (diseaseHistory == null)
            {
                return HttpNotFound();
            }
            ViewBag.animalID = new SelectList(db.Animals, "id", "sex", diseaseHistory.animalID);
            ViewBag.diseaseName = new SelectList(db.Diseases, "name", "name", diseaseHistory.diseaseName);
            return View(diseaseHistory);
        }

        // POST: DiseaseHistories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "beginDate,animalID,diseaseName,endDate")] DiseaseHistory diseaseHistory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(diseaseHistory).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.animalID = new SelectList(db.Animals, "id", "sex", diseaseHistory.animalID);
            ViewBag.diseaseName = new SelectList(db.Diseases, "name", "name", diseaseHistory.diseaseName);
            return View(diseaseHistory);
        }

        // GET: DiseaseHistories/Delete/5
        public async Task<ActionResult> Delete(DateTime id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DiseaseHistory diseaseHistory = await db.DiseaseHistories.FindAsync(id);
            if (diseaseHistory == null)
            {
                return HttpNotFound();
            }
            return View(diseaseHistory);
        }

        // POST: DiseaseHistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(DateTime id)
        {
            DiseaseHistory diseaseHistory = await db.DiseaseHistories.FindAsync(id);
            db.DiseaseHistories.Remove(diseaseHistory);
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
