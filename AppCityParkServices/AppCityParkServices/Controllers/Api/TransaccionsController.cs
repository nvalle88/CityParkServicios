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
    public class TransaccionsController : ApiController
    {
        private CityParkApp db = new CityParkApp();

        // GET: api/Transaccions
        public IQueryable<Transaccion> GetTransaccion()
        {
            return db.Transaccion;
        }

        // GET: api/Transaccions/5
        [ResponseType(typeof(Transaccion))]
        public async Task<IHttpActionResult> GetTransaccion(int id)
        {
            Transaccion transaccion = await db.Transaccion.FindAsync(id);
            if (transaccion == null)
            {
                return NotFound();
            }

            return Ok(transaccion);
        }

        // PUT: api/Transaccions/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTransaccion(int id, Transaccion transaccion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != transaccion.TransaccionId)
            {
                return BadRequest();
            }

            db.Entry(transaccion).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransaccionExists(id))
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

        // POST: api/Transaccions
        [ResponseType(typeof(Transaccion))]
        public async Task<IHttpActionResult> PostTransaccion(Transaccion transaccion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Transaccion.Add(transaccion);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = transaccion.TransaccionId }, transaccion);
        }

        // DELETE: api/Transaccions/5
        [ResponseType(typeof(Transaccion))]
        public async Task<IHttpActionResult> DeleteTransaccion(int id)
        {
            Transaccion transaccion = await db.Transaccion.FindAsync(id);
            if (transaccion == null)
            {
                return NotFound();
            }

            db.Transaccion.Remove(transaccion);
            await db.SaveChangesAsync();

            return Ok(transaccion);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TransaccionExists(int id)
        {
            return db.Transaccion.Count(e => e.TransaccionId == id) > 0;
        }
    }
}