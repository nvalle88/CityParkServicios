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
    public class CodigoesController : Controller
    {
        private CityParkApp db = new CityParkApp();

        // GET: Codigoes
        public async Task<ActionResult> Index()
        {
            var codigo = db.Codigo.Include(c => c.Dispositivo).Include(c => c.Usuario);
            return View(await codigo.ToListAsync());
        }

        // GET: Codigoes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Codigo codigo = await db.Codigo.FindAsync(id);
            if (codigo == null)
            {
                return HttpNotFound();
            }
            return View(codigo);
        }

        // GET: Codigoes/Create
        public ActionResult Create()
        {
            ViewBag.DispositivoId = new SelectList(db.Dispositivo, "DispositivoId", "DispositivoId");
            ViewBag.UsuarioId = new SelectList(db.Usuario, "UsuarioId", "Nombre");
            return View();
        }

        // POST: Codigoes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "CodigoId,Codigo1,UsuarioId,DispositivoId,Usado")] Codigo codigo)
        {
            if (ModelState.IsValid)
            {
                db.Codigo.Add(codigo);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.DispositivoId = new SelectList(db.Dispositivo, "DispositivoId", "DispositivoId", codigo.DispositivoId);
            ViewBag.UsuarioId = new SelectList(db.Usuario, "UsuarioId", "Nombre", codigo.UsuarioId);
            return View(codigo);
        }

        // GET: Codigoes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Codigo codigo = await db.Codigo.FindAsync(id);
            if (codigo == null)
            {
                return HttpNotFound();
            }
            ViewBag.DispositivoId = new SelectList(db.Dispositivo, "DispositivoId", "DispositivoId", codigo.DispositivoId);
            ViewBag.UsuarioId = new SelectList(db.Usuario, "UsuarioId", "Nombre", codigo.UsuarioId);
            return View(codigo);
        }

        // POST: Codigoes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "CodigoId,Codigo1,UsuarioId,DispositivoId,Usado")] Codigo codigo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(codigo).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.DispositivoId = new SelectList(db.Dispositivo, "DispositivoId", "DispositivoId", codigo.DispositivoId);
            ViewBag.UsuarioId = new SelectList(db.Usuario, "UsuarioId", "Nombre", codigo.UsuarioId);
            return View(codigo);
        }

        // GET: Codigoes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Codigo codigo = await db.Codigo.FindAsync(id);
            if (codigo == null)
            {
                return HttpNotFound();
            }
            return View(codigo);
        }

        // POST: Codigoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Codigo codigo = await db.Codigo.FindAsync(id);
            db.Codigo.Remove(codigo);
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
