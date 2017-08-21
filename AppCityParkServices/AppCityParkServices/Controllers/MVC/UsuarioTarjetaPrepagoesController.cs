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
    public class UsuarioTarjetaPrepagoesController : Controller
    {
        private CityParkApp db = new CityParkApp();

        // GET: UsuarioTarjetaPrepagoes
        public async Task<ActionResult> Index()
        {
            var usuarioTarjetaPrepago = db.UsuarioTarjetaPrepago.Include(u => u.TarjetaPrepago).Include(u => u.Usuario);
            return View(await usuarioTarjetaPrepago.ToListAsync());
        }

        // GET: UsuarioTarjetaPrepagoes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsuarioTarjetaPrepago usuarioTarjetaPrepago = await db.UsuarioTarjetaPrepago.FindAsync(id);
            if (usuarioTarjetaPrepago == null)
            {
                return HttpNotFound();
            }
            return View(usuarioTarjetaPrepago);
        }

        // GET: UsuarioTarjetaPrepagoes/Create
        public ActionResult Create()
        {
            ViewBag.TarjetaPrepagoId = new SelectList(db.TarjetaPrepago, "TarjetaPrepagoId", "Numero");
            ViewBag.UsuarioId = new SelectList(db.Usuario, "UsuarioId", "Nombre");
            return View();
        }

        // POST: UsuarioTarjetaPrepagoes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "UsuarioTarjetaPrepagoId,UsuarioId,TarjetaPrepagoId")] UsuarioTarjetaPrepago usuarioTarjetaPrepago)
        {
            if (ModelState.IsValid)
            {
                db.UsuarioTarjetaPrepago.Add(usuarioTarjetaPrepago);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.TarjetaPrepagoId = new SelectList(db.TarjetaPrepago, "TarjetaPrepagoId", "Numero", usuarioTarjetaPrepago.TarjetaPrepagoId);
            ViewBag.UsuarioId = new SelectList(db.Usuario, "UsuarioId", "Nombre", usuarioTarjetaPrepago.UsuarioId);
            return View(usuarioTarjetaPrepago);
        }

        // GET: UsuarioTarjetaPrepagoes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsuarioTarjetaPrepago usuarioTarjetaPrepago = await db.UsuarioTarjetaPrepago.FindAsync(id);
            if (usuarioTarjetaPrepago == null)
            {
                return HttpNotFound();
            }
            ViewBag.TarjetaPrepagoId = new SelectList(db.TarjetaPrepago, "TarjetaPrepagoId", "Numero", usuarioTarjetaPrepago.TarjetaPrepagoId);
            ViewBag.UsuarioId = new SelectList(db.Usuario, "UsuarioId", "Nombre", usuarioTarjetaPrepago.UsuarioId);
            return View(usuarioTarjetaPrepago);
        }

        // POST: UsuarioTarjetaPrepagoes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "UsuarioTarjetaPrepagoId,UsuarioId,TarjetaPrepagoId")] UsuarioTarjetaPrepago usuarioTarjetaPrepago)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usuarioTarjetaPrepago).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.TarjetaPrepagoId = new SelectList(db.TarjetaPrepago, "TarjetaPrepagoId", "Numero", usuarioTarjetaPrepago.TarjetaPrepagoId);
            ViewBag.UsuarioId = new SelectList(db.Usuario, "UsuarioId", "Nombre", usuarioTarjetaPrepago.UsuarioId);
            return View(usuarioTarjetaPrepago);
        }

        // GET: UsuarioTarjetaPrepagoes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsuarioTarjetaPrepago usuarioTarjetaPrepago = await db.UsuarioTarjetaPrepago.FindAsync(id);
            if (usuarioTarjetaPrepago == null)
            {
                return HttpNotFound();
            }
            return View(usuarioTarjetaPrepago);
        }

        // POST: UsuarioTarjetaPrepagoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            UsuarioTarjetaPrepago usuarioTarjetaPrepago = await db.UsuarioTarjetaPrepago.FindAsync(id);
            db.UsuarioTarjetaPrepago.Remove(usuarioTarjetaPrepago);
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
