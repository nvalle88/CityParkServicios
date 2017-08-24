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
    [RoutePrefix("api/Modelos")]
    public class ModeloesController : ApiController
    {
        private CityParkApp db = new CityParkApp();

        // GET: api/Modeloes
        public IQueryable<Modelo> GetModelo()
        {
            db.Configuration.ProxyCreationEnabled = false;
            return db.Modelo.Include(m=>m.Marca);
        }
        
        // GET: api/Modeloes/GetModeloByMarca
        [HttpPost]
        [Route("GetModelosByMarca")]
        public List<ModeloRequest> GetModeloByMarca(MarcaRequest _marca)
        {
            
            var ModelosDB=db.Modelo.Where(x => x.MarcaId == _marca.Id).ToList();
            var Modelos = new List<ModeloRequest>();
            foreach (var item in ModelosDB)
            {
                Modelos.Add(new ModeloRequest { Id = item.ModeloId, Nombre = item.Nombre });
            }
            return Modelos;

        }


        // GET: api/Modeloes/5
        [ResponseType(typeof(Modelo))]
        public async Task<IHttpActionResult> GetModelo(int id)
        {
            Modelo modelo = await db.Modelo.FindAsync(id);
            if (modelo == null)
            {
                return NotFound();
            }
            return Ok(modelo);
        }

        // PUT: api/Modeloes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutModelo(int id, Modelo modelo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != modelo.ModeloId)
            {
                return BadRequest();
            }

            db.Entry(modelo).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ModeloExists(id))
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

        // POST: api/Modeloes
        [ResponseType(typeof(Modelo))]
        public async Task<IHttpActionResult> PostModelo(Modelo modelo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Modelo.Add(modelo);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = modelo.ModeloId }, modelo);
        }

        // DELETE: api/Modeloes/5
        [ResponseType(typeof(Modelo))]
        public async Task<IHttpActionResult> DeleteModelo(int id)
        {
            Modelo modelo = await db.Modelo.FindAsync(id);
            if (modelo == null)
            {
                return NotFound();
            }

            db.Modelo.Remove(modelo);
            await db.SaveChangesAsync();

            return Ok(modelo);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ModeloExists(int id)
        {
            return db.Modelo.Count(e => e.ModeloId == id) > 0;
        }
    }
}