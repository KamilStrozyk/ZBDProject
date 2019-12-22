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
            foreach(var item in servicePointWorker.ServicePoints)
            {
                item.howManyWorkers = item.ServicePointWorkers.Count();
            }
            await db.SaveChangesAsync();
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
            if (servicePointWorker.employed > DateTime.Today)
            {
                ModelState.AddModelError("employed", "Data nie może być większa od obecnej");
            }
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
            if (servicePointWorker.employed > DateTime.Today)
            {
                ModelState.AddModelError("employed", "Data nie może być większa od obecnej");
            }
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
        //GET: Runways/AddServicePoint/5
        public async Task<ActionResult> AddServicePoint(int? id)
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
            var servicePointWorkerServicePoints = servicePointWorker.ServicePoints.Select(x => x.name).ToList();
            IEnumerable<ServicePoint> availableServicePoints = db.ServicePoints.
                Where(x => !servicePointWorkerServicePoints.
                Contains(x.name));
          

            ViewBag.name = new SelectList(availableServicePoints, "name", "name");
            return View(servicePointWorker);

        }

        //POST: Runways/AddSupervisor/5
        [HttpPost, ActionName("AddServicePoint")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddServicePoint(string name, int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (name == null)
            {
                ModelState.AddModelError("lastName", "Nie wybrałeś pracownika");
            }
            if (ModelState.IsValid)
            {
                ServicePoint servicePoint = db.ServicePoints.Where(x => x.name == name).FirstOrDefault();
                ServicePointWorker servicePointWorker = await db.ServicePointWorkers.FindAsync(id);
                servicePoint.ServicePointWorkers.Add(servicePointWorker);
                servicePointWorker.ServicePoints.Add(servicePoint);
                db.Entry(servicePoint).State = EntityState.Modified;
                db.Entry(servicePointWorker).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Details", new { id = servicePointWorker.id });
            }
            else
            {
                ServicePointWorker servicePointWorker = await db.ServicePointWorkers.FindAsync(id);
                if (servicePointWorker == null)
                {
                    return HttpNotFound();
                }
                var servicePointWorkerServicePoints = servicePointWorker.ServicePoints.Select(x => x.name).ToList();
                IEnumerable<ServicePoint> availableServicePoints = db.ServicePoints.
                    Where(x => !servicePointWorkerServicePoints.
                    Contains(x.name));
                string firstServicePoint;
               

                ViewBag.name = new SelectList(availableServicePoints, "name", "name");
                return View(servicePointWorker);
            }
        }
        //GET: Runways/DeleteServicePoint/5
        public async Task<ActionResult> DeleteServicePoint(int? id)
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
            var servicePointWorkerServicePoints = servicePointWorker.ServicePoints.Select(x => x.name).ToList();
            IEnumerable<ServicePoint> availableServicePoints = db.ServicePoints.
                Where(x => servicePointWorkerServicePoints.
                Contains(x.name));
            string firstServicePoint;
           

            ViewBag.name = new SelectList(availableServicePoints, "name", "name");
            return View(servicePointWorker);

        }

        //POST: Runways/DeleteSupervisor/5
        [HttpPost, ActionName("DeleteServicePoint")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteServicePoint(string name, int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (name == null)
            {
                ModelState.AddModelError("lastName", "Nie wybrałeś pracownika");
            }
            if (ModelState.IsValid)
            {
                ServicePoint servicePoint = db.ServicePoints.Where(x => x.name == name).FirstOrDefault();
                ServicePointWorker servicePointWorker = await db.ServicePointWorkers.FindAsync(id);
                servicePoint.ServicePointWorkers.Remove(servicePointWorker);
                servicePointWorker.ServicePoints.Remove(servicePoint);
                
                db.Entry(servicePoint).State = EntityState.Modified;
                db.Entry(servicePointWorker).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Details", new { id = servicePointWorker.id });
            }
            else
            {
  
                ServicePointWorker servicePointWorker = await db.ServicePointWorkers.FindAsync(id);
                if (servicePointWorker == null)
                {
                    return HttpNotFound();
                }
                var servicePointWorkerServicePoints = servicePointWorker.ServicePoints.Select(x => x.name).ToList();
                IEnumerable<ServicePoint> availableServicePoints = db.ServicePoints.
                    Where(x => servicePointWorkerServicePoints.
                    Contains(x.name));
              

                ViewBag.name = new SelectList(availableServicePoints, "name", "name");
                return View(servicePointWorker);
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
