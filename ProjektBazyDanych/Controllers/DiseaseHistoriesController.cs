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
        public async Task<ActionResult> Index(string keyword)
        {
            var diseaseHistories = db.DiseaseHistories.Include(d => d.Animal).Include(d => d.Disease);
            if (keyword != null)
            {
                diseaseHistories = db.DiseaseHistories.Where(x => x.beginDate.ToString().Contains(keyword) || x.endDate.ToString().Contains(keyword) || x.Disease.name.Contains(keyword) || x.Animal.name.Contains(keyword)).Include(d => d.Animal).Include(d => d.Disease);
                ViewBag.keyword = keyword;
            }
            return View(await diseaseHistories.ToListAsync());
        }

        // GET: DiseaseHistories/Details/5
        public async Task<ActionResult> Details(int? id)
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
            ViewBag.animalID = new SelectList(db.Animals, "name", "name");
            ViewBag.diseaseName = new SelectList(db.Diseases, "name", "name");
            return View();
        }

        // POST: DiseaseHistories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "beginDate,endDate")] DiseaseHistory diseaseHistory)
        {
            string disease = Request.Form["diseaseName"];
            string animal = Request.Form["animalID"];
            diseaseHistory.diseaseId = db.Diseases.Where(x => x.name == disease).FirstOrDefault().id;
            diseaseHistory.animalID = db.Animals.Where(x => x.name == animal).FirstOrDefault().id;
            if (diseaseHistory.beginDate > DateTime.Today)
            {
                ModelState.AddModelError("beginDate", "Data nie może być większa od obecnej");
            }
            if (diseaseHistory.endDate != null)
            {
                if (diseaseHistory.endDate > DateTime.Today)
                {
                    ModelState.AddModelError("endDate", "Data nie może być większa od obecnej");
                }
                else if (diseaseHistory.endDate < diseaseHistory.beginDate)
                {
                    ModelState.AddModelError("endDate", "Data zakończenianie musi być większa od daty rozpoczęcia");
                }
            }
            if (db.DiseaseHistories.Any(x => x.beginDate == diseaseHistory.beginDate
               && x.animalID == diseaseHistory.animalID
               && x.diseaseId == diseaseHistory.diseaseId
               && x.id != diseaseHistory.id))
            {
                ModelState.AddModelError("beginDate", "Wpis z takimi parametrami już istnieje");
                ModelState.AddModelError("animalID", "Wpis z takimi parametrami już istnieje");
                ModelState.AddModelError("diseaseId", "Wpis z takimi parametrami już istnieje");
            }
            if (ModelState.IsValid)
            {
                db.DiseaseHistories.Add(diseaseHistory);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.animalID = new SelectList(db.Animals, "name", "name");
            ViewBag.diseaseName = new SelectList(db.Diseases, "name", "name");
            return View(diseaseHistory);
        }

        // GET: DiseaseHistories/Edit/5
        public async Task<ActionResult> Edit(int? id)
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
            ViewBag.animalID = new SelectList(db.Animals, "name", "name", diseaseHistory.Animal.name);
            ViewBag.diseaseName = new SelectList(db.Diseases, "name", "name", diseaseHistory.Disease.name);
            return View(diseaseHistory);
        }

        // POST: DiseaseHistories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,beginDate,endDate")] DiseaseHistory diseaseHistory)
        {
            string disease = Request.Form["diseaseName"];
            string animal = Request.Form["animalID"];
            diseaseHistory.diseaseId = db.Diseases.Where(x => x.name == disease).FirstOrDefault().id;
            diseaseHistory.animalID = db.Animals.Where(x => x.name == animal).FirstOrDefault().id;

            if (diseaseHistory.beginDate > DateTime.Today)
            {
                ModelState.AddModelError("beginDate", "Data nie może być większa od obecnej");
            }
            if (diseaseHistory.endDate != null)
            {
                if (diseaseHistory.endDate > DateTime.Today)
                {
                    ModelState.AddModelError("endDate", "Data nie może być większa od obecnej");
                }
                else if (diseaseHistory.endDate < diseaseHistory.beginDate)
                {
                    ModelState.AddModelError("endDate", "Data zakończenianie musi być większa od daty rozpoczęcia");
                }
            }
            if (db.DiseaseHistories.Any(x => x.beginDate == diseaseHistory.beginDate
                && x.animalID == diseaseHistory.animalID
                && x.diseaseId == diseaseHistory.diseaseId
                && x.id != diseaseHistory.id))
            {
                ModelState.AddModelError("beginDate", "Wpis z takimi parametrami już istnieje");
                ModelState.AddModelError("animalID", "Wpis z takimi parametrami już istnieje");
                ModelState.AddModelError("diseaseId", "Wpis z takimi parametrami już istnieje");
            }
            if (ModelState.IsValid)
            {
                db.Entry(diseaseHistory).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.animalID = new SelectList(db.Animals, "name", "name");
            ViewBag.diseaseName = new SelectList(db.Diseases, "name", "name");
            return View(diseaseHistory);
        }

        // GET: DiseaseHistories/Delete/5
        public async Task<ActionResult> Delete(int? id)
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
        public async Task<ActionResult> DeleteConfirmed(int? id)
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
