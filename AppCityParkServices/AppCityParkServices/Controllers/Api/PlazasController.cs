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
using AppCityParkServices.Utils;

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

        // GET: api/Plazas/GetPlazaByBarrio
        [HttpPost]
        [Route("GetPlazaByBarrio")]
        public List<Plaza> GetPlazaByBarrio(Barrios barrio)
        {
            var PlazaDB = db.Plaza.Where(x => x.Barrio == barrio.NombreBarrio).ToList();
            return PlazaDB;
        }

        // GET: api/Plazas/GetPlazaByNombre
        [HttpPost]
        [Route("GetPlazaByNombre")]
        public Plaza GetPlazaByNombre(Plaza plaza)
        {
            Plaza PlazaDB = db.Plaza.Where(x => x.Nombre == plaza.Nombre).FirstOrDefault();           
            return PlazaDB;
        }

        // GET: api/Plazas/GetPlazaByPosition
        [HttpPost]
        [Route("GetPlazaByPosition")]
        public List<Plaza> GetPlazaByPosition(Position Position)
        {

           var PlazaDB = db.Plaza.Where(x => x.Ocupado == false).ToList();
            List<Plaza> _plazas = new List<Plaza>();

            foreach (var plaza in PlazaDB)
            {                              //Posicion del ususario, Plaza a comparar y la distancia que desamos comparar en KM
                if (GeoUtils.EstaCercaDeMi(Position, plaza, 0.1))
                {
                    _plazas.Add(plaza);
                }
            }

            return _plazas;
        }



        // GET: api/Plazas/GetBarrios
        [HttpGet]
        [Route("GetBarrios")]
        public List<Barrios> GetBarrios()
        {
            var g = db.Plaza.GroupBy(a => a.Barrio);
            var Barrios = new List<Barrios>();

            foreach (var pc in g)
            {
                Barrios.Add(new Barrios { NombreBarrio = pc.Key});
            }         
            return Barrios;
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
        public async Task<IHttpActionResult> PutPlaza(Plaza plaza)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
          
            db.Entry(plaza).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {                
                    throw;                
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