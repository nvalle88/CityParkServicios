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
    public class CarroesController : Controller
    {
        private CityParkApp db = new CityParkApp();

        // GET: Carroes
        public async Task<ActionResult> Index()
        {
            var carro = db.Carro.Include(c => c.Modelo).Include(c => c.Usuario);
            return View(await carro.ToListAsync());
        }

        // GET: Carroes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Carro carro = await db.Carro.FindAsync(id);
            if (carro == null)
            {
                return HttpNotFound();
            }
            return View(carro);
        }

        // GET: Carroes/Create
        public ActionResult Create()
        {
            ViewBag.ModeloId = new SelectList(db.Modelo, "ModeloId", "Nombre");
            ViewBag.UsuarioId = new SelectList(db.Usuario, "UsuarioId", "Nombre");
            return View();
        }

        // POST: Carroes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "CarroId,ModeloId,UsuarioId,Placa,Color")] Carro carro)
        {
            if (ModelState.IsValid)
            {
                db.Carro.Add(carro);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ModeloId = new SelectList(db.Modelo, "ModeloId", "Nombre", carro.ModeloId);
            ViewBag.UsuarioId = new SelectList(db.Usuario, "UsuarioId", "Nombre", carro.UsuarioId);
            return View(carro);
        }

        // GET: Carroes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Carro carro = await db.Carro.FindAsync(id);
            if (carro == null)
            {
                return HttpNotFound();
            }
            ViewBag.ModeloId = new SelectList(db.Modelo, "ModeloId", "Nombre", carro.ModeloId);
            ViewBag.UsuarioId = new SelectList(db.Usuario, "UsuarioId", "Nombre", carro.UsuarioId);
            return View(carro);
        }

        // POST: Carroes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "CarroId,ModeloId,UsuarioId,Placa,Color")] Carro carro)
        {
            if (ModelState.IsValid)
            {
                db.Entry(carro).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ModeloId = new SelectList(db.Modelo, "ModeloId", "Nombre", carro.ModeloId);
            ViewBag.UsuarioId = new SelectList(db.Usuario, "UsuarioId", "Nombre", carro.UsuarioId);
            return View(carro);
        }

        // GET: Carroes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Carro carro = await db.Carro.FindAsync(id);
            if (carro == null)
            {
                return HttpNotFound();
            }
            return View(carro);
        }

        // POST: Carroes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Carro carro = await db.Carro.FindAsync(id);
            db.Carro.Remove(carro);
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
