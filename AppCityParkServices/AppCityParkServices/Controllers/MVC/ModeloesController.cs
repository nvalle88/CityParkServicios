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
    public class ModeloesController : Controller
    {
        private CityParkApp db = new CityParkApp();

        // GET: Modeloes
        public async Task<ActionResult> Index()
        {
            var modelo = db.Modelo.Include(m => m.Marca);
            return View(await modelo.ToListAsync());
        }

        // GET: Modeloes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Modelo modelo = await db.Modelo.FindAsync(id);
            if (modelo == null)
            {
                return HttpNotFound();
            }
            return View(modelo);
        }

        // GET: Modeloes/Create
        public ActionResult Create()
        {
            ViewBag.MarcaId = new SelectList(db.Marca, "MarcaId", "Nombre");
            return View();
        }

        // POST: Modeloes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ModeloId,Nombre,MarcaId")] Modelo modelo)
        {
            if (ModelState.IsValid)
            {
                db.Modelo.Add(modelo);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.MarcaId = new SelectList(db.Marca, "MarcaId", "Nombre", modelo.MarcaId);
            return View(modelo);
        }

        // GET: Modeloes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Modelo modelo = await db.Modelo.FindAsync(id);
            if (modelo == null)
            {
                return HttpNotFound();
            }
            ViewBag.MarcaId = new SelectList(db.Marca, "MarcaId", "Nombre", modelo.MarcaId);
            return View(modelo);
        }

        // POST: Modeloes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ModeloId,Nombre,MarcaId")] Modelo modelo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(modelo).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.MarcaId = new SelectList(db.Marca, "MarcaId", "Nombre", modelo.MarcaId);
            return View(modelo);
        }

        // GET: Modeloes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Modelo modelo = await db.Modelo.FindAsync(id);
            if (modelo == null)
            {
                return HttpNotFound();
            }
            return View(modelo);
        }

        // POST: Modeloes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Modelo modelo = await db.Modelo.FindAsync(id);
            db.Modelo.Remove(modelo);
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
