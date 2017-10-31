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
    public class DispositivoesController : Controller
    {
        private CityParkApp db = new CityParkApp();

        // GET: Dispositivoes
        public async Task<ActionResult> Index()
        {
            var dispositivo = db.Dispositivo.Include(d => d.Empresa).Include(d => d.SO);
            return View(await dispositivo.ToListAsync());
        }

        // GET: Dispositivoes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dispositivo dispositivo = await db.Dispositivo.FindAsync(id);
            if (dispositivo == null)
            {
                return HttpNotFound();
            }
            return View(dispositivo);
        }

        // GET: Dispositivoes/Create
        public ActionResult Create()
        {
            ViewBag.EmpresaId = new SelectList(db.Empresa, "EmpresaId", "RazonSocial");
            ViewBag.SOId = new SelectList(db.SO, "SOId", "Nombre");
            return View();
        }

        // POST: Dispositivoes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "DispositivoId,SOId,UsuarioId,UniqueId,EmpresaId")] Dispositivo dispositivo)
        {
            if (ModelState.IsValid)
            {
                db.Dispositivo.Add(dispositivo);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.EmpresaId = new SelectList(db.Empresa, "EmpresaId", "RazonSocial", dispositivo.EmpresaId);
            ViewBag.SOId = new SelectList(db.SO, "SOId", "Nombre", dispositivo.SOId);
            return View(dispositivo);
        }

        // GET: Dispositivoes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dispositivo dispositivo = await db.Dispositivo.FindAsync(id);
            if (dispositivo == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmpresaId = new SelectList(db.Empresa, "EmpresaId", "RazonSocial", dispositivo.EmpresaId);
            ViewBag.SOId = new SelectList(db.SO, "SOId", "Nombre", dispositivo.SOId);
            return View(dispositivo);
        }

        // POST: Dispositivoes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "DispositivoId,SOId,UsuarioId,UniqueId,EmpresaId")] Dispositivo dispositivo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dispositivo).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.EmpresaId = new SelectList(db.Empresa, "EmpresaId", "RazonSocial", dispositivo.EmpresaId);
            ViewBag.SOId = new SelectList(db.SO, "SOId", "Nombre", dispositivo.SOId);
            return View(dispositivo);
        }

        // GET: Dispositivoes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dispositivo dispositivo = await db.Dispositivo.FindAsync(id);
            if (dispositivo == null)
            {
                return HttpNotFound();
            }
            return View(dispositivo);
        }

        // POST: Dispositivoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Dispositivo dispositivo = await db.Dispositivo.FindAsync(id);
            db.Dispositivo.Remove(dispositivo);
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
