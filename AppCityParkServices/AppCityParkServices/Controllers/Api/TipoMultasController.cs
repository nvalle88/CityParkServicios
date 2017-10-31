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
    public class TipoMultasController : ApiController
    {
        private CityParkApp db = new CityParkApp();

        // GET: api/TipoMultas
        public IQueryable<TipoMultas> GetTipoMultas()
        {
            return db.TipoMultas;
        }

        // GET: api/TipoMultas/5
        [ResponseType(typeof(TipoMultas))]
        public async Task<IHttpActionResult> GetTipoMultas(int id)
        {
            TipoMultas tipoMultas = await db.TipoMultas.FindAsync(id);
            if (tipoMultas == null)
            {
                return NotFound();
            }

            return Ok(tipoMultas);
        }

        // PUT: api/TipoMultas/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTipoMultas(int id, TipoMultas tipoMultas)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tipoMultas.TipoMultaId)
            {
                return BadRequest();
            }

            db.Entry(tipoMultas).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoMultasExists(id))
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

        // POST: api/TipoMultas
        [ResponseType(typeof(TipoMultas))]
        public async Task<IHttpActionResult> PostTipoMultas(TipoMultas tipoMultas)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TipoMultas.Add(tipoMultas);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tipoMultas.TipoMultaId }, tipoMultas);
        }

        // DELETE: api/TipoMultas/5
        [ResponseType(typeof(TipoMultas))]
        public async Task<IHttpActionResult> DeleteTipoMultas(int id)
        {
            TipoMultas tipoMultas = await db.TipoMultas.FindAsync(id);
            if (tipoMultas == null)
            {
                return NotFound();
            }

            db.TipoMultas.Remove(tipoMultas);
            await db.SaveChangesAsync();

            return Ok(tipoMultas);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TipoMultasExists(int id)
        {
            return db.TipoMultas.Count(e => e.TipoMultaId == id) > 0;
        }
    }
}