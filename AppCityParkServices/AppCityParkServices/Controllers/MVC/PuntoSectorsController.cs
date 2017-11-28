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
    public class PuntoSectorsController : Controller
    {
        private CityParkApp db = new CityParkApp();

        // GET: PuntoSectors
        public async Task<ActionResult> Index()
        {
            var puntoSector = db.PuntoSector.Include(p => p.Sector);
            return View(await puntoSector.ToListAsync());
        }

        // GET: PuntoSectors/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PuntoSector puntoSector = await db.PuntoSector.FindAsync(id);
            if (puntoSector == null)
            {
                return HttpNotFound();
            }
            return View(puntoSector);
        }

        // GET: PuntoSectors/Create
        public ActionResult Create()
        {
            ViewBag.SectorId = new SelectList(db.Sector, "SectorId", "NombreSector");
            return View();
        }

        // POST: PuntoSectors/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "PuntoSectorId,Latitud,Longitud,SectorId")] PuntoSector puntoSector)
        {
            if (ModelState.IsValid)
            {
                db.PuntoSector.Add(puntoSector);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.SectorId = new SelectList(db.Sector, "SectorId", "NombreSector", puntoSector.SectorId);
            return View(puntoSector);
        }

        // GET: PuntoSectors/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PuntoSector puntoSector = await db.PuntoSector.FindAsync(id);
            if (puntoSector == null)
            {
                return HttpNotFound();
            }
            ViewBag.SectorId = new SelectList(db.Sector, "SectorId", "NombreSector", puntoSector.SectorId);
            return View(puntoSector);
        }

        // POST: PuntoSectors/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "PuntoSectorId,Latitud,Longitud,SectorId")] PuntoSector puntoSector)
        {
            if (ModelState.IsValid)
            {
                db.Entry(puntoSector).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.SectorId = new SelectList(db.Sector, "SectorId", "NombreSector", puntoSector.SectorId);
            return View(puntoSector);
        }

        // GET: PuntoSectors/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PuntoSector puntoSector = await db.PuntoSector.FindAsync(id);
            if (puntoSector == null)
            {
                return HttpNotFound();
            }
            return View(puntoSector);
        }

        // POST: PuntoSectors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            PuntoSector puntoSector = await db.PuntoSector.FindAsync(id);
            db.PuntoSector.Remove(puntoSector);
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
