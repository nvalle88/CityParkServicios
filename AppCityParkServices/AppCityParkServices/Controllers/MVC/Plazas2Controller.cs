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
    public class Plazas2Controller : Controller
    {
        private CityParkApp db = new CityParkApp();

        // GET: Plazas2
        public async Task<ActionResult> Index()
        {
            var plaza = db.Plaza.Include(p => p.Empresa);
            return View(await plaza.ToListAsync());
        }

        // GET: Plazas2/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Plaza plaza = await db.Plaza.FindAsync(id);
            if (plaza == null)
            {
                return HttpNotFound();
            }
            return View(plaza);
        }

        // GET: Plazas2/Create
        public ActionResult Create()
        {
            ViewBag.EmpresaId = new SelectList(db.Empresa, "EmpresaId", "RazonSocial");
            return View();
        }

        // POST: Plazas2/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "PlazaId,Nombre,Barrio,Direccion,Ocupado,Longitud,Latitud,EmpresaId")] Plaza plaza)
        {
            if (ModelState.IsValid)
            {
                db.Plaza.Add(plaza);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.EmpresaId = new SelectList(db.Empresa, "EmpresaId", "RazonSocial", plaza.EmpresaId);
            return View(plaza);
        }

        // GET: Plazas2/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Plaza plaza = await db.Plaza.FindAsync(id);
            if (plaza == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmpresaId = new SelectList(db.Empresa, "EmpresaId", "RazonSocial", plaza.EmpresaId);
            return View(plaza);
        }

        // POST: Plazas2/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "PlazaId,Nombre,Barrio,Direccion,Ocupado,Longitud,Latitud,EmpresaId")] Plaza plaza)
        {
            if (ModelState.IsValid)
            {
                db.Entry(plaza).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.EmpresaId = new SelectList(db.Empresa, "EmpresaId", "RazonSocial", plaza.EmpresaId);
            return View(plaza);
        }

        // GET: Plazas2/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Plaza plaza = await db.Plaza.FindAsync(id);
            if (plaza == null)
            {
                return HttpNotFound();
            }
            return View(plaza);
        }

        // POST: Plazas2/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Plaza plaza = await db.Plaza.FindAsync(id);
            db.Plaza.Remove(plaza);
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
