using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Linq;

namespace ProjektBazyDanych.Controllers
{
    public class DiseasesController : Controller
    {
        private connectionString db = new connectionString();

        // GET: Diseases
        public async Task<ActionResult> Index()
        {
            return View(await db.Diseases.ToListAsync());
        }

        // GET: Diseases/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Disease disease = await db.Diseases.FindAsync(id);
            if (disease == null)
            {
                return HttpNotFound();
            }
            return View(disease);
        }

        // GET: Diseases/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Diseases/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "name")] Disease disease)
        {
            if (db.Diseases.Any(x => x.name == disease.name && x.id != disease.id))
            {
                ModelState.AddModelError("name", "Taka choroba już istnieje");
            }
            if (ModelState.IsValid)
            {
                db.Diseases.Add(disease);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(disease);
        }

        public async Task<ActionResult> DeleteAnimals(string name)
        {
            db.deleteInfectedAnimals(name);
            int id = db.Diseases.Where(x => x.name == name).FirstOrDefault().id;
            return RedirectToAction("Details", new { id = id  });
        }

        // GET: Diseases/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Disease disease = await db.Diseases.FindAsync(id);
            if (disease == null)
            {
                return HttpNotFound();
            }
            return View(disease);
        }

        // POST: Diseases/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,name")] Disease disease)
        {
            if (db.Diseases.Any(x => x.name == disease.name && x.id != disease.id))
            {
                ModelState.AddModelError("name", "Taka choroba już istnieje");
            }
            if (ModelState.IsValid)
            {
                db.Entry(disease).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(disease);
        }

        // GET: Diseases/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Disease disease = await db.Diseases.FindAsync(id);
            if (disease == null)
            {
                return HttpNotFound();
            }
            return View(disease);
        }

        // POST: Diseases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int? id)
        {
            Disease disease = await db.Diseases.FindAsync(id);
            db.Diseases.Remove(disease);
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