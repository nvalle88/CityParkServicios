using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using AppCityParkServices.Models;

namespace AppCityParkServices.Controllers.Api
{
    [RoutePrefix("api/Plazas")]

    public class PlazasController : ApiController
    {
        private CityParkApp db = new CityParkApp();

        // GET: api/Plazas
        public IQueryable<Plaza> GetPlaza()
        {
            return db.Plaza;
        }

        // GET: api/Plazas/GetPlazaByNombre
        [HttpPost]
        [Route("GetPlazaByNombre")]
        public Plaza GetPlazaByNombre(Plaza plaza)
        {
            Plaza PlazaDB = db.Plaza.Where(x => x.Nombre == plaza.Nombre).FirstOrDefault();           
            return PlazaDB;
        }

        // GET: api/Plazas/5
        [ResponseType(typeof(Plaza))]
        public async Task<IHttpActionResult> GetPlaza(int id)
        {
            Plaza plaza = await db.Plaza.FindAsync(id);


            if (plaza == null)
            {
                return NotFound();
            }

            return Ok(plaza);
        }

        // PUT: api/Plazas/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPlaza(int id, Plaza plaza)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != plaza.PlazaId)
            {
                return BadRequest();
            }

            db.Entry(plaza).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlazaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Plazas
        [Route("insertarPlaza")]

        [ResponseType(typeof(Plaza))]
        public async Task<IHttpActionResult> PostPlaza(Plaza plaza)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Plaza.Add(plaza);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = plaza.PlazaId }, plaza);
        }

        // DELETE: api/Plazas/5
        [ResponseType(typeof(Plaza))]
        public async Task<IHttpActionResult> DeletePlaza(int id)
        {
            Plaza plaza = await db.Plaza.FindAsync(id);
            if (plaza == null)
            {
                return NotFound();
            }

            db.Plaza.Remove(plaza);
            await db.SaveChangesAsync();

            return Ok(plaza);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PlazaExists(int id)
        {
            return db.Plaza.Count(e => e.PlazaId == id) > 0;
        }
    }
}