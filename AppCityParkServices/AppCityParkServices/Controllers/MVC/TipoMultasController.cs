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
    public class TipoMultasController : Controller
    {
        private CityParkApp db = new CityParkApp();

        // GET: TipoMultas
        public async Task<ActionResult> Index()
        {
            var tipoMultas = db.TipoMultas.Include(t => t.Empresa).Include(t => t.SalarioBasico);
            return View(await tipoMultas.ToListAsync());
        }

        // GET: TipoMultas/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoMultas tipoMultas = await db.TipoMultas.FindAsync(id);
            if (tipoMultas == null)
            {
                return HttpNotFound();
            }
            return View(tipoMultas);
        }

        // GET: TipoMultas/Create
        public ActionResult Create()
        {
            ViewBag.EmpresaId = new SelectList(db.Empresa, "EmpresaId", "RazonSocial");
            ViewBag.SalarioBasicoId = new SelectList(db.SalarioBasico, "SalarioBasicoId", "SalarioBasicoId");
            return View();
        }

        // POST: TipoMultas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "TipoMultaId,Multa,Descripcion,EmpresaId,Porcentaje,SalarioBasicoId")] TipoMultas tipoMultas)
        {
            if (ModelState.IsValid)
            {
                db.TipoMultas.Add(tipoMultas);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.EmpresaId = new SelectList(db.Empresa, "EmpresaId", "RazonSocial", tipoMultas.EmpresaId);
            ViewBag.SalarioBasicoId = new SelectList(db.SalarioBasico, "SalarioBasicoId", "SalarioBasicoId", tipoMultas.SalarioBasicoId);
            return View(tipoMultas);
        }

        // GET: TipoMultas/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoMultas tipoMultas = await db.TipoMultas.FindAsync(id);
            if (tipoMultas == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmpresaId = new SelectList(db.Empresa, "EmpresaId", "RazonSocial", tipoMultas.EmpresaId);
            ViewBag.SalarioBasicoId = new SelectList(db.SalarioBasico, "SalarioBasicoId", "SalarioBasicoId", tipoMultas.SalarioBasicoId);
            return View(tipoMultas);
        }

        // POST: TipoMultas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "TipoMultaId,Multa,Descripcion,EmpresaId,Porcentaje,SalarioBasicoId")] TipoMultas tipoMultas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipoMultas).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.EmpresaId = new SelectList(db.Empresa, "EmpresaId", "RazonSocial", tipoMultas.EmpresaId);
            ViewBag.SalarioBasicoId = new SelectList(db.SalarioBasico, "SalarioBasicoId", "SalarioBasicoId", tipoMultas.SalarioBasicoId);
            return View(tipoMultas);
        }

        // GET: TipoMultas/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoMultas tipoMultas = await db.TipoMultas.FindAsync(id);
            if (tipoMultas == null)
            {
                return HttpNotFound();
            }
            return View(tipoMultas);
        }

        // POST: TipoMultas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            TipoMultas tipoMultas = await db.TipoMultas.FindAsync(id);
            db.TipoMultas.Remove(tipoMultas);
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
