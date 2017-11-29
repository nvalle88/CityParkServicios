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
using AppCityParkServices.Clases;

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
        [Route("DeleteSector")]
        public async Task<Response> DeleteSector(Sector sector)
        {
            db.Configuration.ProxyCreationEnabled = false;

            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var _Sector = await db.Sector.Where(x => x.SectorId == sector.SectorId).FirstOrDefaultAsync();

                    var agentes = await db.Agente.Where(x => x.SectorId == _Sector.SectorId).ToListAsync();

                    foreach (var agente in agentes)
                    {
                        agente.SectorId = null;
                        db.Entry(agente).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                    }

                    var _MyPolygon = await db.PuntoSector.Where(x => x.SectorId == _Sector.SectorId).ToListAsync();
                    db.PuntoSector.RemoveRange(_MyPolygon);
                    await db.SaveChangesAsync();

                    db.Sector.Remove(_Sector);
                    await db.SaveChangesAsync();

                    transaction.Commit();

                    return new Response { IsSuccess = true };
                }
                catch (Exception)
                {
                    return new Response { IsSuccess = false };

                } 
            }

            
        }


        [HttpPost]
        [Route("GetPuntoSectorPorSector")]
        public async Task< List<PuntoSector>> GetPuntoSectorPorSector(Sector sector)
        {
            db.Configuration.ProxyCreationEnabled = false;
            if (sector==null)
            {
                return  null;
            }

            var _MyPolygon =await db.PuntoSector.Where(x => x.SectorId == sector.SectorId).ToListAsync();

            return _MyPolygon;
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

        [HttpPost]
        [Route("GetSector")]
        public async Task<Response> GetAgente([FromBody]Sector sector)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                var agenteRequest = await db.Sector.Where(x => x.SectorId == sector.SectorId).FirstOrDefaultAsync();
                if (agenteRequest == null)
                {
                    return new Response { IsSuccess = false };
                }

                return new Response { IsSuccess = true, Result = agenteRequest };
            }
            catch (Exception)
            {
                return new Response { IsSuccess = false };
                throw;
            }
        }

        [HttpPost]
        [Route("GetSectoresPorEmpresa")]
        public async Task<IEnumerable<Sector>> GetAgentesPorEmpresa([FromBody] Empresa empresa)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var agentes = await db.Sector.Where(x => x.EmpresaId == empresa.EmpresaId).Include(x => x.Empresa).ToListAsync();

            if (agentes == null)
            {
                return null;
            }
            return agentes;
        }

        [HttpPost]
        [Route("EditarSector")]
        public async Task<Response> EditarSector(SectorViewModel model)
        {
            if (model == null)
            {
                return new Response { IsSuccess = false };
            }

            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {

                    var puntosEliminar =await db.PuntoSector.Where(x => x.SectorId == model.Sector.SectorId).ToListAsync();
                    db.PuntoSector.RemoveRange(puntosEliminar);
                    await db.SaveChangesAsync();

                    var sector = await db.Sector.Where(x=>x.SectorId==model.Sector.SectorId).FirstOrDefaultAsync();

                    sector.NombreSector = model.Sector.NombreSector;

                    db.Entry(sector).State=EntityState.Modified;
                    await db.SaveChangesAsync();

                    foreach (var item in model.PuntoSector)
                    {
                        db.PuntoSector.Add(new PuntoSector { Latitud = item.Latitud, Longitud = item.Longitud, SectorId = sector.SectorId });
                        await db.SaveChangesAsync();
                    }

                    transaction.Commit();

                    return new Response { IsSuccess = true };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new Response { IsSuccess = false };
                    throw;
                }
            }

        }


        [HttpPost]
        [Route("InsertarSector")]
        public async Task<Response> InsertarSector([FromBody]SectorViewModel model)
        {
            if (model == null)
            {
                return new Response { IsSuccess = false ,Message="Modelo nulo"};
            }

            using (var transaction = db.Database.BeginTransaction())
            {
                var a = "";
                try
                {
                    var sector = new Sector {NombreSector = model.Sector.NombreSector, EmpresaId = model.Sector.EmpresaId };
                    a = "antes de insertar sector";
                    db.Sector.Add(sector);
                    await db.SaveChangesAsync();
                    a = "Insertar sector";
                    foreach (var item in model.PuntoSector)
                    {
                        db.PuntoSector.Add(new PuntoSector { Latitud = item.Latitud, Longitud = item.Longitud, SectorId = sector.SectorId });
                        a = a+"Antes de insertar punto";
                        await db.SaveChangesAsync();
                        a = a + "despues insertarpunto";
                    }

                    transaction.Commit();

                    return new Response { IsSuccess = true };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new Response { IsSuccess = false , Message=ex.Message+a , Result=model};
                    throw;
                }
            }

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