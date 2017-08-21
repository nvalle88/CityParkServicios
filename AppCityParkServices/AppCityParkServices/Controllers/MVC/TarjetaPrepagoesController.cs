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
    public class TarjetaPrepagoesController : Controller
    {
        private CityParkApp db = new CityParkApp();

        // GET: TarjetaPrepagoes
        public async Task<ActionResult> Index()
        {
            return View(await db.TarjetaPrepago.ToListAsync());
        }

        // GET: TarjetaPrepagoes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TarjetaPrepago tarjetaPrepago = await db.TarjetaPrepago.FindAsync(id);
            if (tarjetaPrepago == null)
            {
                return HttpNotFound();
            }
            return View(tarjetaPrepago);
        }

        // GET: TarjetaPrepagoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TarjetaPrepagoes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "TarjetaPrepagoId,Numero,Saldo,FechaVence,Activa")] TarjetaPrepago tarjetaPrepago)
        {
            if (ModelState.IsValid)
            {
                db.TarjetaPrepago.Add(tarjetaPrepago);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(tarjetaPrepago);
        }

        // GET: TarjetaPrepagoes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TarjetaPrepago tarjetaPrepago = await db.TarjetaPrepago.FindAsync(id);
            if (tarjetaPrepago == null)
            {
                return HttpNotFound();
            }
            return View(tarjetaPrepago);
        }

        // POST: TarjetaPrepagoes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "TarjetaPrepagoId,Numero,Saldo,FechaVence,Activa")] TarjetaPrepago tarjetaPrepago)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tarjetaPrepago).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(tarjetaPrepago);
        }

        // GET: TarjetaPrepagoes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TarjetaPrepago tarjetaPrepago = await db.TarjetaPrepago.FindAsync(id);
            if (tarjetaPrepago == null)
            {
                return HttpNotFound();
            }
            return View(tarjetaPrepago);
        }

        // POST: TarjetaPrepagoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            TarjetaPrepago tarjetaPrepago = await db.TarjetaPrepago.FindAsync(id);
            db.TarjetaPrepago.Remove(tarjetaPrepago);
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
