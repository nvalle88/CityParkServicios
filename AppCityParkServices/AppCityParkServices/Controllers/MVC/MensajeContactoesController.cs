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
    public class MensajeContactoesController : Controller
    {
        private CityParkApp db = new CityParkApp();

        // GET: MensajeContactoes
        public async Task<ActionResult> Index()
        {
            var mensajeContacto = db.MensajeContacto.Include(m => m.Usuario).Include(m => m.Vendedor);
            return View(await mensajeContacto.ToListAsync());
        }

        // GET: MensajeContactoes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MensajeContacto mensajeContacto = await db.MensajeContacto.FindAsync(id);
            if (mensajeContacto == null)
            {
                return HttpNotFound();
            }
            return View(mensajeContacto);
        }

        // GET: MensajeContactoes/Create
        public ActionResult Create()
        {
            ViewBag.UsuarioId = new SelectList(db.Usuario, "UsuarioId", "Nombre");
            ViewBag.VendedorId = new SelectList(db.Vendedor, "VendedorId", "Nombre");
            return View();
        }

        // POST: MensajeContactoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "idMensajeContacto,isUsuario,UsuarioId,VendedorId,Mensaje,Fecha")] MensajeContacto mensajeContacto)
        {
            if (ModelState.IsValid)
            {
                db.MensajeContacto.Add(mensajeContacto);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.UsuarioId = new SelectList(db.Usuario, "UsuarioId", "Nombre", mensajeContacto.UsuarioId);
            ViewBag.VendedorId = new SelectList(db.Vendedor, "VendedorId", "Nombre", mensajeContacto.VendedorId);
            return View(mensajeContacto);
        }

        // GET: MensajeContactoes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MensajeContacto mensajeContacto = await db.MensajeContacto.FindAsync(id);
            if (mensajeContacto == null)
            {
                return HttpNotFound();
            }
            ViewBag.UsuarioId = new SelectList(db.Usuario, "UsuarioId", "Nombre", mensajeContacto.UsuarioId);
            ViewBag.VendedorId = new SelectList(db.Vendedor, "VendedorId", "Nombre", mensajeContacto.VendedorId);
            return View(mensajeContacto);
        }

        // POST: MensajeContactoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "idMensajeContacto,isUsuario,UsuarioId,VendedorId,Mensaje,Fecha")] MensajeContacto mensajeContacto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mensajeContacto).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.UsuarioId = new SelectList(db.Usuario, "UsuarioId", "Nombre", mensajeContacto.UsuarioId);
            ViewBag.VendedorId = new SelectList(db.Vendedor, "VendedorId", "Nombre", mensajeContacto.VendedorId);
            return View(mensajeContacto);
        }

        // GET: MensajeContactoes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MensajeContacto mensajeContacto = await db.MensajeContacto.FindAsync(id);
            if (mensajeContacto == null)
            {
                return HttpNotFound();
            }
            return View(mensajeContacto);
        }

        // POST: MensajeContactoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            MensajeContacto mensajeContacto = await db.MensajeContacto.FindAsync(id);
            db.MensajeContacto.Remove(mensajeContacto);
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
