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
    public class ParqueosController : Controller
    {
        private CityParkApp db = new CityParkApp();

        // GET: Parqueos
        public async Task<ActionResult> Index()
        {
            var parqueo = db.Parqueo.Include(p => p.TarjetaCredito).Include(p => p.Usuario);
            return View(await parqueo.ToListAsync());
        }

        // GET: Parqueos/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Parqueo parqueo = await db.Parqueo.FindAsync(id);
            if (parqueo == null)
            {
                return HttpNotFound();
            }
            return View(parqueo);
        }

        // GET: Parqueos/Create
        public ActionResult Create()
        {
            ViewBag.TarjetaCreditoId = new SelectList(db.TarjetaCredito, "TarjetaCreditoId", "Nombre");
            ViewBag.UsuarioId = new SelectList(db.Usuario, "UsuarioId", "Nombre");
            return View();
        }

        // POST: Parqueos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ParqueoId,UsuarioId,FechaInicio,FechaFin,Latitud,Longitud,TarjetaCreditoId,PlazaId")] Parqueo parqueo)
        {
            if (ModelState.IsValid)
            {
                db.Parqueo.Add(parqueo);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.TarjetaCreditoId = new SelectList(db.TarjetaCredito, "TarjetaCreditoId", "Nombre", parqueo.TarjetaCreditoId);
            ViewBag.UsuarioId = new SelectList(db.Usuario, "UsuarioId", "Nombre", parqueo.UsuarioId);
            return View(parqueo);
        }

        // GET: Parqueos/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Parqueo parqueo = await db.Parqueo.FindAsync(id);
            if (parqueo == null)
            {
                return HttpNotFound();
            }
            ViewBag.TarjetaCreditoId = new SelectList(db.TarjetaCredito, "TarjetaCreditoId", "Nombre", parqueo.TarjetaCreditoId);
            ViewBag.UsuarioId = new SelectList(db.Usuario, "UsuarioId", "Nombre", parqueo.UsuarioId);
            return View(parqueo);
        }

        // POST: Parqueos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ParqueoId,UsuarioId,FechaInicio,FechaFin,Latitud,Longitud,TarjetaCreditoId,PlazaId")] Parqueo parqueo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(parqueo).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.TarjetaCreditoId = new SelectList(db.TarjetaCredito, "TarjetaCreditoId", "Nombre", parqueo.TarjetaCreditoId);
            ViewBag.UsuarioId = new SelectList(db.Usuario, "UsuarioId", "Nombre", parqueo.UsuarioId);
            return View(parqueo);
        }

        // GET: Parqueos/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Parqueo parqueo = await db.Parqueo.FindAsync(id);
            if (parqueo == null)
            {
                return HttpNotFound();
            }
            return View(parqueo);
        }

        // POST: Parqueos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Parqueo parqueo = await db.Parqueo.FindAsync(id);
            db.Parqueo.Remove(parqueo);
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
