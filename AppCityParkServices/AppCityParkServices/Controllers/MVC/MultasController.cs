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
    public class MultasController : Controller
    {
        private CityParkApp db = new CityParkApp();

        // GET: Multas
        public async Task<ActionResult> Index()
        {
            var multa = db.Multa.Include(m => m.Agente);
            return View(await multa.ToListAsync());
        }

        // GET: Multas/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Multa multa = await db.Multa.FindAsync(id);
            if (multa == null)
            {
                return HttpNotFound();
            }
            return View(multa);
        }

        // GET: Multas/Create
        public ActionResult Create()
        {
            ViewBag.AgenteId = new SelectList(db.Agente, "AgenteId", "Nombre");
            ViewBag.CarroId = new SelectList(db.Carro, "CarroId", "Placa");
            ViewBag.UsuarioId = new SelectList(db.Usuario, "UsuarioId", "Nombre");
            return View();
        }

        // POST: Multas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "MultaId,Foto,CarroId,UsuarioId,Valor,AgenteId")] Multa multa)
        {
            if (ModelState.IsValid)
            {
                db.Multa.Add(multa);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.AgenteId = new SelectList(db.Agente, "AgenteId", "Nombre", multa.AgenteId);
           
           
            return View(multa);
        }

        // GET: Multas/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Multa multa = await db.Multa.FindAsync(id);
            if (multa == null)
            {
                return HttpNotFound();
            }
            ViewBag.AgenteId = new SelectList(db.Agente, "AgenteId", "Nombre", multa.AgenteId);
            
            return View(multa);
        }

        // POST: Multas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "MultaId,Foto,CarroId,UsuarioId,Valor,AgenteId")] Multa multa)
        {
            if (ModelState.IsValid)
            {
                db.Entry(multa).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.AgenteId = new SelectList(db.Agente, "AgenteId", "Nombre", multa.AgenteId);
           
            return View(multa);
        }

        // GET: Multas/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Multa multa = await db.Multa.FindAsync(id);
            if (multa == null)
            {
                return HttpNotFound();
            }
            return View(multa);
        }

        // POST: Multas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Multa multa = await db.Multa.FindAsync(id);
            db.Multa.Remove(multa);
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
