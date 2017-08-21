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
    public class SaldoesController : Controller
    {
        private CityParkApp db = new CityParkApp();

        // GET: Saldoes
        public async Task<ActionResult> Index()
        {
            var saldo = db.Saldo.Include(s => s.Usuario);
            return View(await saldo.ToListAsync());
        }

        // GET: Saldoes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Saldo saldo = await db.Saldo.FindAsync(id);
            if (saldo == null)
            {
                return HttpNotFound();
            }
            return View(saldo);
        }

        // GET: Saldoes/Create
        public ActionResult Create()
        {
            ViewBag.UsuarioId = new SelectList(db.Usuario, "UsuarioId", "Nombre");
            return View();
        }

        // POST: Saldoes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "SaldoId,Saldo1,UsuarioId")] Saldo saldo)
        {
            if (ModelState.IsValid)
            {
                db.Saldo.Add(saldo);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.UsuarioId = new SelectList(db.Usuario, "UsuarioId", "Nombre", saldo.UsuarioId);
            return View(saldo);
        }

        // GET: Saldoes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Saldo saldo = await db.Saldo.FindAsync(id);
            if (saldo == null)
            {
                return HttpNotFound();
            }
            ViewBag.UsuarioId = new SelectList(db.Usuario, "UsuarioId", "Nombre", saldo.UsuarioId);
            return View(saldo);
        }

        // POST: Saldoes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "SaldoId,Saldo1,UsuarioId")] Saldo saldo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(saldo).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.UsuarioId = new SelectList(db.Usuario, "UsuarioId", "Nombre", saldo.UsuarioId);
            return View(saldo);
        }

        // GET: Saldoes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Saldo saldo = await db.Saldo.FindAsync(id);
            if (saldo == null)
            {
                return HttpNotFound();
            }
            return View(saldo);
        }

        // POST: Saldoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Saldo saldo = await db.Saldo.FindAsync(id);
            db.Saldo.Remove(saldo);
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
