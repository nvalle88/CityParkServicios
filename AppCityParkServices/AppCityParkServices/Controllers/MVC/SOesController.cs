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
    public class SOesController : Controller
    {
        private CityParkApp db = new CityParkApp();

        // GET: SOes
        public async Task<ActionResult> Index()
        {
            return View(await db.SO.ToListAsync());
        }

        // GET: SOes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SO sO = await db.SO.FindAsync(id);
            if (sO == null)
            {
                return HttpNotFound();
            }
            return View(sO);
        }

        // GET: SOes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SOes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "SOId,Nombre,Version")] SO sO)
        {
            if (ModelState.IsValid)
            {
                db.SO.Add(sO);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(sO);
        }

        // GET: SOes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SO sO = await db.SO.FindAsync(id);
            if (sO == null)
            {
                return HttpNotFound();
            }
            return View(sO);
        }

        // POST: SOes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "SOId,Nombre,Version")] SO sO)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sO).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(sO);
        }

        // GET: SOes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SO sO = await db.SO.FindAsync(id);
            if (sO == null)
            {
                return HttpNotFound();
            }
            return View(sO);
        }

        // POST: SOes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            SO sO = await db.SO.FindAsync(id);
            db.SO.Remove(sO);
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
