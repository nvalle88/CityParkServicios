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
    public class TransaccionsController : Controller
    {
        private CityParkApp db = new CityParkApp();

        // GET: Transaccions
        public async Task<ActionResult> Index()
        {
            var transaccion = db.Transaccion.Include(t => t.Usuario).Include(t => t.Vendedor);
            return View(await transaccion.ToListAsync());
        }

        // GET: Transaccions/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaccion transaccion = await db.Transaccion.FindAsync(id);
            if (transaccion == null)
            {
                return HttpNotFound();
            }
            return View(transaccion);
        }

        // GET: Transaccions/Create
        public ActionResult Create()
        {
            ViewBag.UsuarioId = new SelectList(db.Usuario, "UsuarioId", "Nombre");
            ViewBag.VendedorId = new SelectList(db.Vendedor, "VendedorId", "Nombre");
            return View();
        }

        // POST: Transaccions/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "TransaccionId,UsuarioId,VendedorId,Monto,Fecha")] Transaccion transaccion)
        {
            if (ModelState.IsValid)
            {
                db.Transaccion.Add(transaccion);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.UsuarioId = new SelectList(db.Usuario, "UsuarioId", "Nombre", transaccion.UsuarioId);
            ViewBag.VendedorId = new SelectList(db.Vendedor, "VendedorId", "Nombre", transaccion.VendedorId);
            return View(transaccion);
        }

        // GET: Transaccions/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaccion transaccion = await db.Transaccion.FindAsync(id);
            if (transaccion == null)
            {
                return HttpNotFound();
            }
            ViewBag.UsuarioId = new SelectList(db.Usuario, "UsuarioId", "Nombre", transaccion.UsuarioId);
            ViewBag.VendedorId = new SelectList(db.Vendedor, "VendedorId", "Nombre", transaccion.VendedorId);
            return View(transaccion);
        }

        // POST: Transaccions/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "TransaccionId,UsuarioId,VendedorId,Monto,Fecha")] Transaccion transaccion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(transaccion).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.UsuarioId = new SelectList(db.Usuario, "UsuarioId", "Nombre", transaccion.UsuarioId);
            ViewBag.VendedorId = new SelectList(db.Vendedor, "VendedorId", "Nombre", transaccion.VendedorId);
            return View(transaccion);
        }

        // GET: Transaccions/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaccion transaccion = await db.Transaccion.FindAsync(id);
            if (transaccion == null)
            {
                return HttpNotFound();
            }
            return View(transaccion);
        }

        // POST: Transaccions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Transaccion transaccion = await db.Transaccion.FindAsync(id);
            db.Transaccion.Remove(transaccion);
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
