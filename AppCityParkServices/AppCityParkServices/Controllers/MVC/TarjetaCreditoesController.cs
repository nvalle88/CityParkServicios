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
    public class TarjetaCreditoesController : Controller
    {
        private CityParkApp db = new CityParkApp();

        // GET: TarjetaCreditoes
        public async Task<ActionResult> Index()
        {
            var tarjetaCredito = db.TarjetaCredito.Include(t => t.Usuario);
            return View(await tarjetaCredito.ToListAsync());
        }

        // GET: TarjetaCreditoes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TarjetaCredito tarjetaCredito = await db.TarjetaCredito.FindAsync(id);
            if (tarjetaCredito == null)
            {
                return HttpNotFound();
            }
            return View(tarjetaCredito);
        }

        // GET: TarjetaCreditoes/Create
        public ActionResult Create()
        {
            ViewBag.UsuarioId = new SelectList(db.Usuario, "UsuarioId", "Nombre");
            return View();
        }

        // POST: TarjetaCreditoes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "TarjetaCreditoId,Nombre,Apellido,Numero,CVV_CVC,UsuarioId")] TarjetaCredito tarjetaCredito)
        {
            if (ModelState.IsValid)
            {
                db.TarjetaCredito.Add(tarjetaCredito);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.UsuarioId = new SelectList(db.Usuario, "UsuarioId", "Nombre", tarjetaCredito.UsuarioId);
            return View(tarjetaCredito);
        }

        // GET: TarjetaCreditoes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TarjetaCredito tarjetaCredito = await db.TarjetaCredito.FindAsync(id);
            if (tarjetaCredito == null)
            {
                return HttpNotFound();
            }
            ViewBag.UsuarioId = new SelectList(db.Usuario, "UsuarioId", "Nombre", tarjetaCredito.UsuarioId);
            return View(tarjetaCredito);
        }

        // POST: TarjetaCreditoes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "TarjetaCreditoId,Nombre,Apellido,Numero,CVV_CVC,UsuarioId")] TarjetaCredito tarjetaCredito)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tarjetaCredito).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.UsuarioId = new SelectList(db.Usuario, "UsuarioId", "Nombre", tarjetaCredito.UsuarioId);
            return View(tarjetaCredito);
        }

        // GET: TarjetaCreditoes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TarjetaCredito tarjetaCredito = await db.TarjetaCredito.FindAsync(id);
            if (tarjetaCredito == null)
            {
                return HttpNotFound();
            }
            return View(tarjetaCredito);
        }

        // POST: TarjetaCreditoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            TarjetaCredito tarjetaCredito = await db.TarjetaCredito.FindAsync(id);
            db.TarjetaCredito.Remove(tarjetaCredito);
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
