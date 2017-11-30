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
    public class SalarioBasicoesController : Controller
    {
        private CityParkApp db = new CityParkApp();

        // GET: SalarioBasicoes
        public async Task<ActionResult> Index()
        {
            return View(await db.SalarioBasico.ToListAsync());
        }

        // GET: SalarioBasicoes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SalarioBasico salarioBasico = await db.SalarioBasico.FindAsync(id);
            if (salarioBasico == null)
            {
                return HttpNotFound();
            }
            return View(salarioBasico);
        }

        // GET: SalarioBasicoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SalarioBasicoes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "SalarioBasicoId,Monto,Fecha")] SalarioBasico salarioBasico)
        {
            if (ModelState.IsValid)
            {
                db.SalarioBasico.Add(salarioBasico);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(salarioBasico);
        }

        // GET: SalarioBasicoes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SalarioBasico salarioBasico = await db.SalarioBasico.FindAsync(id);
            if (salarioBasico == null)
            {
                return HttpNotFound();
            }
            return View(salarioBasico);
        }

        // POST: SalarioBasicoes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "SalarioBasicoId,Monto,Fecha")] SalarioBasico salarioBasico)
        {
            if (ModelState.IsValid)
            {
                db.Entry(salarioBasico).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(salarioBasico);
        }

        // GET: SalarioBasicoes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SalarioBasico salarioBasico = await db.SalarioBasico.FindAsync(id);
            if (salarioBasico == null)
            {
                return HttpNotFound();
            }
            return View(salarioBasico);
        }

        // POST: SalarioBasicoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            SalarioBasico salarioBasico = await db.SalarioBasico.FindAsync(id);
            db.SalarioBasico.Remove(salarioBasico);
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
