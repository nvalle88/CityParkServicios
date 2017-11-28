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
    [RoutePrefix("api/Sectors")]


    public class SectorsController : ApiController
    {
        private CityParkApp db = new CityParkApp();

        // GET: api/Sectors
        public IQueryable<Sector> GetSector()
        {
            return db.Sector;
        }

        [HttpPost]
        [Route("GetMyPolygon")]
        public List<PuntoSector> GetMyPolygon(Agente agente)
        {
            db.Configuration.ProxyCreationEnabled = false;

            var _agente = db.Agente.Where(x => x.AgenteId == agente.AgenteId).FirstOrDefault();

            var _Sector = db.Sector.Where(x => x.SectorId == _agente.SectorId).FirstOrDefault();

            var _MyPolygon = db.PuntoSector.Where(x => x.SectorId == _Sector.SectorId).ToList();

            return _MyPolygon;                      
        }

        // GET: api/Sectors/5
        [ResponseType(typeof(Sector))]
        public async Task<IHttpActionResult> GetSector(int id)
        {
            Sector sector = await db.Sector.FindAsync(id);
            if (sector == null)
            {
                return NotFound();
            }

            return Ok(sector);
        }

        // PUT: api/Sectors/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSector(int id, Sector sector)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != sector.SectorId)
            {
                return BadRequest();
            }

            db.Entry(sector).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SectorExists(id))
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

        // POST: api/Sectors
        [ResponseType(typeof(Sector))]
        public async Task<IHttpActionResult> PostSector(Sector sector)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Sector.Add(sector);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (SectorExists(sector.SectorId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = sector.SectorId }, sector);
        }

        // DELETE: api/Sectors/5
        [ResponseType(typeof(Sector))]
        public async Task<IHttpActionResult> DeleteSector(int id)
        {
            Sector sector = await db.Sector.FindAsync(id);
            if (sector == null)
            {
                return NotFound();
            }

            db.Sector.Remove(sector);
            await db.SaveChangesAsync();

            return Ok(sector);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SectorExists(int id)
        {
            return db.Sector.Count(e => e.SectorId == id) > 0;
        }
    }
}