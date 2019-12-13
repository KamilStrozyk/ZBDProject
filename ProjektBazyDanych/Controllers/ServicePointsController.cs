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
            foreach (var item in db.ServicePoints)
            {
                item.howManyWorkers = item.ServicePointWorkers.Count();
            }
            await db.SaveChangesAsync();
            return View(await db.ServicePoints.ToListAsync());
        }

        // GET: ServicePoints/Details/5
        public async Task<ActionResult> Details(int? id, string name = "")
        {
            ViewBag.Message = name;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServicePoint servicePoint = await db.ServicePoints.FindAsync(id);
            servicePoint.howManyWorkers = servicePoint.ServicePointWorkers.Count();
            await db.SaveChangesAsync();

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
            if (db.ServicePoints.Select(x => x.name).Contains(servicePoint.name))
            {
                ModelState.AddModelError("name", "Taki punkt już istnieje");
            }
            if (ModelState.IsValid)
            {
                db.ServicePoints.Add(servicePoint);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(servicePoint);
        }

        // GET: ServicePoints/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServicePoint servicePoint = await db.ServicePoints.FindAsync(id);
            servicePoint.howManyWorkers = servicePoint.ServicePointWorkers.Count();
            await db.SaveChangesAsync();


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
        public async Task<ActionResult> Edit([Bind(Include = "id,name,howManyWorkers,income,type")] ServicePoint servicePoint)
        {
            if (db.ServicePoints.Any(x => x.name==servicePoint.name && x.id!=servicePoint.id))
            {
                ModelState.AddModelError("name", "Taki punkt już istnieje");
            }
            if (ModelState.IsValid)
            {
                db.Entry(servicePoint).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(servicePoint);
        }

        // GET: ServicePoints/Delete/5
        public async Task<ActionResult> Delete(int? id)
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
        public async Task<ActionResult> DeleteConfirmed(int? id)
        {
            ServicePoint servicePoint = await db.ServicePoints.FindAsync(id);
            db.ServicePoints.Remove(servicePoint);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        //GET: Runways/AddServicePointWorker/5
        public async Task<ActionResult> AddServicePointWorker(int? id)
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
            var servicePointServicePointWorkers = servicePoint.ServicePointWorkers.Select(x => x.id).ToList();
            IEnumerable<ServicePointWorker> availableServicePointWorkers = db.ServicePointWorkers.
                Where(x => !servicePointServicePointWorkers.
                Contains(x.id));
            string firstServicePointWorker;
            if (!servicePointServicePointWorkers.Any())
                firstServicePointWorker = db.ServicePointWorkers.Where(x => !servicePointServicePointWorkers.Contains(x.id)).FirstOrDefault().lastName;
            else firstServicePointWorker = "brak dostępnych pracowników";

            ViewBag.lastName = new SelectList(availableServicePointWorkers, "lastName", "lastName", firstServicePointWorker);
            return View(servicePoint);

        }

        //POST: Runways/AddSupervisor/5
        [HttpPost, ActionName("AddServicePointWorker")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddServicePointWorker(string lastName, int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (lastName == null)
            {
                ModelState.AddModelError("lastName", "Nie wybrałeś pracownika");
            }
            if (ModelState.IsValid)
            {


                ServicePointWorker servicePointWorker = db.ServicePointWorkers.Where(x => x.lastName == lastName).FirstOrDefault();
                ServicePoint servicePoint = await db.ServicePoints.FindAsync(id);
                servicePoint.ServicePointWorkers.Add(servicePointWorker);
                servicePointWorker.ServicePoints.Add(servicePoint);
                db.Entry(servicePoint).State = EntityState.Modified;
                db.Entry(servicePointWorker).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Details", new { id = servicePoint.id });
            }
            else
            {
                ServicePoint servicePoint = await db.ServicePoints.FindAsync(id);
                if (servicePoint == null)
                {
                    return HttpNotFound();
                }
                var servicePointServicePointWorkers = servicePoint.ServicePointWorkers.Select(x => x.id).ToList();
                IEnumerable<ServicePointWorker> availableServicePointWorkers = db.ServicePointWorkers.
                    Where(x => !servicePointServicePointWorkers.
                    Contains(x.id));
                string firstServicePointWorker;
                if (!servicePointServicePointWorkers.Any())
                    firstServicePointWorker = db.ServicePointWorkers.Where(x => !servicePointServicePointWorkers.Contains(x.id)).FirstOrDefault().lastName;
                else firstServicePointWorker = "brak dostępnych pracowników";

                ViewBag.lastName = new SelectList(availableServicePointWorkers, "lastName", "lastName", firstServicePointWorker);
                return View(servicePoint);
            }
        }
        //GET: Runways/DeleteServicePointWorker/5
        public async Task<ActionResult> DeleteServicePointWorker(int? id)
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
            var servicePointServicePointWorkers = servicePoint.ServicePointWorkers.Select(x => x.id).ToList();
            IEnumerable<ServicePointWorker> availableServicePointWorkers = db.ServicePointWorkers.
                Where(x => servicePointServicePointWorkers.
                Contains(x.id));
            string firstServicePointWorker;
            if (servicePointServicePointWorkers.Any())
                firstServicePointWorker = availableServicePointWorkers.FirstOrDefault().lastName;
            else firstServicePointWorker = "brak dostępnych pracowników";

            ViewBag.lastName = new SelectList(availableServicePointWorkers, "lastName", "lastName", firstServicePointWorker);
            return View(servicePoint);

        }

        //POST: Runways/DeleteSupervisor/5
        [HttpPost, ActionName("DeleteServicePointWorker")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteServicePointWorker(string lastName, int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (lastName == null)
            {
                ModelState.AddModelError("lastName", "Nie wybrałeś pracownika");
            }
            if (ModelState.IsValid)
            {
                ServicePointWorker servicePointWorker = db.ServicePointWorkers.Where(x => x.lastName == lastName).FirstOrDefault();
                ServicePoint servicePoint = await db.ServicePoints.FindAsync(id);
                servicePoint.ServicePointWorkers.Remove(servicePointWorker);
                servicePointWorker.ServicePoints.Remove(servicePoint);
                db.Entry(servicePoint).State = EntityState.Modified;
                db.Entry(servicePointWorker).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Details", new { id = servicePoint.id });
            }
            else
            {

                ServicePoint servicePoint = await db.ServicePoints.FindAsync(id);
                if (servicePoint == null)
                {
                    return HttpNotFound();
                }
                var servicePointServicePointWorkers = servicePoint.ServicePointWorkers.Select(x => x.id).ToList();
                IEnumerable<ServicePointWorker> availableServicePointWorkers = db.ServicePointWorkers.
                    Where(x => servicePointServicePointWorkers.
                    Contains(x.id));
                string firstServicePointWorker;
                if (servicePointServicePointWorkers.Any())
                    firstServicePointWorker = availableServicePointWorkers.FirstOrDefault().lastName;
                else firstServicePointWorker = "brak dostępnych pracowników";

                ViewBag.lastName = new SelectList(availableServicePointWorkers, "lastName", "lastName", firstServicePointWorker);
                return View(servicePoint);
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
