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
    public class DispositivoesController : ApiController
    {
        private CityParkApp db = new CityParkApp();

        // GET: api/Dispositivoes
        public IQueryable<Dispositivo> GetDispositivo()
        {
            return db.Dispositivo;
        }

        // GET: api/Dispositivoes/5
        [ResponseType(typeof(Dispositivo))]
        public async Task<IHttpActionResult> GetDispositivo(int id)
        {
            Dispositivo dispositivo = await db.Dispositivo.FindAsync(id);
            if (dispositivo == null)
            {
                return NotFound();
            }
            return Ok(dispositivo);
        }

        // PUT: api/Dispositivoes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutDispositivo(int id, Dispositivo dispositivo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != dispositivo.DispositivoId)
            {
                return BadRequest();
            }

            db.Entry(dispositivo).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DispositivoExists(id))
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

        // POST: api/Dispositivoes
        [ResponseType(typeof(Dispositivo))]
        public async Task<IHttpActionResult> PostDispositivo(Dispositivo dispositivo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Dispositivo.Add(dispositivo);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = dispositivo.DispositivoId }, dispositivo);
        }

        // DELETE: api/Dispositivoes/5
        [ResponseType(typeof(Dispositivo))]
        public async Task<IHttpActionResult> DeleteDispositivo(int id)
        {
            Dispositivo dispositivo = await db.Dispositivo.FindAsync(id);
            if (dispositivo == null)
            {
                return NotFound();
            }

            db.Dispositivo.Remove(dispositivo);
            await db.SaveChangesAsync();

            return Ok(dispositivo);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DispositivoExists(int id)
        {
            return db.Dispositivo.Count(e => e.DispositivoId == id) > 0;
        }
    }
}