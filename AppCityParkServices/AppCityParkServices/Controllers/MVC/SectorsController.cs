using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AppCityParkServices.Models;

namespace AppCityParkServices.Controllers.MVC
{
    public class SectorsController : Controller
    {
        private CityParkApp db = new CityParkApp();

        // GET: Sectors
        public async Task<ActionResult> Index()
        {
            var sector = db.Sector.Include(s => s.Agente);
            return View(await sector.ToListAsync());
        }

        // GET: Sectors/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sector sector = await db.Sector.FindAsync(id);
            if (sector == null)
            {
                return HttpNotFound();
            }
            return View(sector);
        }

        // GET: Sectors/Create
        public ActionResult Create()
        {
            ViewBag.AgenteId = new SelectList(db.Agente, "AgenteId", "Nombre");
            return View();
        }

        // POST: Sectors/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "SectorId,AgenteId,NombreSector")] Sector sector)
        {
            if (ModelState.IsValid)
            {
                db.Sector.Add(sector);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.AgenteId = new SelectList(db.Agente, "AgenteId", "Nombre", sector.AgenteId);
            return View(sector);
        }

        // GET: Sectors/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sector sector = await db.Sector.FindAsync(id);
            if (sector == null)
            {
                return HttpNotFound();
            }
            ViewBag.AgenteId = new SelectList(db.Agente, "AgenteId", "Nombre", sector.AgenteId);
            return View(sector);
        }

        // POST: Sectors/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "SectorId,AgenteId,NombreSector")] Sector sector)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sector).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.AgenteId = new SelectList(db.Agente, "AgenteId", "Nombre", sector.AgenteId);
            return View(sector);
        }

        // GET: Sectors/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sector sector = await db.Sector.FindAsync(id);
            if (sector == null)
            {
                return HttpNotFound();
            }
            return View(sector);
        }

        // POST: Sectors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Sector sector = await db.Sector.FindAsync(id);
            db.Sector.Remove(sector);
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
