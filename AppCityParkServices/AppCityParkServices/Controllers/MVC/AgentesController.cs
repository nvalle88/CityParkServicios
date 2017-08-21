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
    public class AgentesController : Controller
    {
        private CityParkApp db = new CityParkApp();

        // GET: Agentes
        public async Task<ActionResult> Index()
        {
            return View(await db.Agente.ToListAsync());
        }

        // GET: Agentes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agente agente = await db.Agente.FindAsync(id);
            if (agente == null)
            {
                return HttpNotFound();
            }
            return View(agente);
        }

        // GET: Agentes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Agentes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "AgenteId,Nombre,Apellido,Contrasena")] Agente agente)
        {
            if (ModelState.IsValid)
            {
                db.Agente.Add(agente);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(agente);
        }

        // GET: Agentes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agente agente = await db.Agente.FindAsync(id);
            if (agente == null)
            {
                return HttpNotFound();
            }
            return View(agente);
        }

        // POST: Agentes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "AgenteId,Nombre,Apellido,Contrasena")] Agente agente)
        {
            if (ModelState.IsValid)
            {
                db.Entry(agente).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(agente);
        }

        // GET: Agentes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agente agente = await db.Agente.FindAsync(id);
            if (agente == null)
            {
                return HttpNotFound();
            }
            return View(agente);
        }

        // POST: Agentes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Agente agente = await db.Agente.FindAsync(id);
            db.Agente.Remove(agente);
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
