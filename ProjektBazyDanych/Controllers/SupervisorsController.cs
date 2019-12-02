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
