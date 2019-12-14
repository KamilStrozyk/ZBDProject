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
    public class SupervisorsController : Controller
    {
        private connectionString db = new connectionString();

        // GET: Supervisors
        public async Task<ActionResult> Index()
        {
            return View(await db.Supervisors.ToListAsync());
        }

        // GET: Supervisors/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supervisor supervisor = await db.Supervisors.FindAsync(id);
            foreach(var item in supervisor.Runways)
            {
                item.animalCount = item.Animals.Count();
            }
            await db.SaveChangesAsync();
            if (supervisor == null)
            {
                return HttpNotFound();
            }
            return View(supervisor);
        }

        // GET: Supervisors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Supervisors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,firstName,lastName,age,salary,employed")] Supervisor supervisor)
        {
            if (supervisor.employed > DateTime.Today)
            {
                ModelState.AddModelError("employed", "Data nie może być większa od obecnej");
            }
            if (ModelState.IsValid)
            {
                db.Supervisors.Add(supervisor);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(supervisor);
        }

        // GET: Supervisors/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supervisor supervisor = await db.Supervisors.FindAsync(id);
            if (supervisor == null)
            {
                return HttpNotFound();
            }
            return View(supervisor);
        }

        // POST: Supervisors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,firstName,lastName,age,salary,employed")] Supervisor supervisor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(supervisor).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(supervisor);
        }

        // GET: Supervisors/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supervisor supervisor = await db.Supervisors.FindAsync(id);
            if (supervisor == null)
            {
                return HttpNotFound();
            }
            return View(supervisor);
        }

        // POST: Supervisors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Supervisor supervisor = await db.Supervisors.FindAsync(id);
            db.Supervisors.Remove(supervisor);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        //GET: Runways/AddRunway/5
        public async Task<ActionResult> AddRunway(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supervisor supervisor = await db.Supervisors.FindAsync(id);
            if (supervisor == null)
            {
                return HttpNotFound();
            }
            var supervisorRunways = supervisor.Runways.Select(x => x.id).ToList();
            IEnumerable<Runway> availableRunways = db.Runways.
                Where(x => !supervisorRunways.
                Contains(x.id));
            string firstRunway;
            if (!supervisorRunways.Any())
                firstRunway = db.Runways.Where(x => !supervisorRunways.Contains(x.id)).FirstOrDefault().name;
            else firstRunway = "brak dostępnego wybiegu";

            ViewBag.name = new SelectList(availableRunways, "name", "name", firstRunway);
            return View(supervisor);

        }

        //POST: Runways/AddSupervisor/5
        [HttpPost, ActionName("AddRunway")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddRunway(string name, int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (name == null)
            {
                ModelState.AddModelError("name", "Nie wybrałeś wybiegu");
            }
            if (ModelState.IsValid)
            {
                Supervisor supervisor = await db.Supervisors.FindAsync(id);
                Runway runway =  db.Runways. Where(x => x.name == name).FirstOrDefault();
                runway.Supervisors.Add(supervisor);
                supervisor.Runways.Add(runway);
                db.Entry(runway).State = EntityState.Modified;
                db.Entry(supervisor).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Details", new { id = supervisor.id });
            }
            else
            {
                Supervisor supervisor = await db.Supervisors.FindAsync(id);
                if (supervisor == null)
                {
                    return HttpNotFound();
                }
                var supervisorRunways = supervisor.Runways.Select(x => x.id).ToList();
                IEnumerable<Runway> availableRunways = db.Runways.
                    Where(x => !supervisorRunways.
                    Contains(x.id));
                string firstRunway;
                if (!supervisorRunways.Any())
                    firstRunway = db.Runways.Where(x => !supervisorRunways.Contains(x.id)).FirstOrDefault().name;
                else firstRunway = "brak dostępnego wybiegu";

                ViewBag.name = new SelectList(availableRunways, "name", "name", firstRunway);
                return View(supervisor);
            }
        }
        //GET: Runways/DeleteRunway/5
        public async Task<ActionResult> DeleteRunway(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supervisor supervisor = await db.Supervisors.FindAsync(id);
            if (supervisor == null)
            {
                return HttpNotFound();
            }
            var supervisorRunways = supervisor.Runways.Select(x => x.id).ToList();
            IEnumerable<Runway> availableRunways = db.Runways.
                Where(x => supervisorRunways.
                Contains(x.id));
            string firstRunway;
            if (supervisorRunways.Any())
                firstRunway = db.Runways.FirstOrDefault().name;
            else firstRunway = "brak dostępnego wybiegu";

            ViewBag.name = new SelectList(availableRunways, "name", "name", firstRunway);
            return View(supervisor);

        }

        //POST: Runways/DeleteRunway/5
        [HttpPost, ActionName("DeleteRunway")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteRunway(string name, int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (name == null)
            {
                ModelState.AddModelError("name", "Nie wybrałeś wybiegu");
            }
            if (ModelState.IsValid)
            {
                Supervisor supervisor = await db.Supervisors.FindAsync(id);
                Runway runway = db.Runways.Where(x => x.name == name).FirstOrDefault();
                runway.Supervisors.Remove(supervisor);
                supervisor.Runways.Remove(runway);
                db.Entry(runway).State = EntityState.Modified;
                db.Entry(supervisor).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Details", new { id = supervisor.id });
            }
            else
            {
                Supervisor supervisor = await db.Supervisors.FindAsync(id);
                if (supervisor == null)
                {
                    return HttpNotFound();
                }
                var supervisorRunways = supervisor.Runways.Select(x => x.id).ToList();
                IEnumerable<Runway> availableRunways = db.Runways.
                    Where(x => supervisorRunways.
                    Contains(x.id));
                string firstRunway;
                if (supervisorRunways.Any())
                    firstRunway = db.Runways.FirstOrDefault().name;
                else firstRunway = "brak dostępnego wybiegu";

                ViewBag.name = new SelectList(availableRunways, "name", "name", firstRunway);
                return View(supervisor);
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
