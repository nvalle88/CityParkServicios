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
    public class PuntoSectorsController : ApiController
    {
        private CityParkApp db = new CityParkApp();

        // GET: api/PuntoSectors
        public IQueryable<PuntoSector> GetPuntoSector()
        {
            return db.PuntoSector;
        }

        // GET: api/PuntoSectors/5
        [ResponseType(typeof(PuntoSector))]
        public async Task<IHttpActionResult> GetPuntoSector(int id)
        {
            PuntoSector puntoSector = await db.PuntoSector.FindAsync(id);
            if (puntoSector == null)
            {
                return NotFound();
            }

            return Ok(puntoSector);
        }

        // PUT: api/PuntoSectors/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPuntoSector(int id, PuntoSector puntoSector)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != puntoSector.PuntoSectorId)
            {
                return BadRequest();
            }

            db.Entry(puntoSector).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PuntoSectorExists(id))
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

        // POST: api/PuntoSectors
        [ResponseType(typeof(PuntoSector))]
        public async Task<IHttpActionResult> PostPuntoSector(PuntoSector puntoSector)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PuntoSector.Add(puntoSector);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PuntoSectorExists(puntoSector.PuntoSectorId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = puntoSector.PuntoSectorId }, puntoSector);
        }

        // DELETE: api/PuntoSectors/5
        [ResponseType(typeof(PuntoSector))]
        public async Task<IHttpActionResult> DeletePuntoSector(int id)
        {
            PuntoSector puntoSector = await db.PuntoSector.FindAsync(id);
            if (puntoSector == null)
            {
                return NotFound();
            }

            db.PuntoSector.Remove(puntoSector);
            await db.SaveChangesAsync();

            return Ok(puntoSector);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PuntoSectorExists(int id)
        {
            return db.PuntoSector.Count(e => e.PuntoSectorId == id) > 0;
        }
    }
}