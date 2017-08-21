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
using Newtonsoft.Json.Linq;

namespace AppCityParkServices.Controllers.Api
{
    [RoutePrefix("api/Carroes")]
    public class CarroesController : ApiController
    {

        private CityParkApp db = new CityParkApp();

        // GET: api/Carroes
        [HttpPost]
        [Route("GetCarros")]
        public IQueryable<Carro> GetCarro(JObject form)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var UsuarioId = string.Empty;

            dynamic jsonObject = form;

            try
            {
                UsuarioId = jsonObject.UsuarioId.Value;
            }
            catch (Exception)
            {

                return  null;
            }

            var usuarioId = Convert.ToInt32(UsuarioId);
            var carros = db.Carro.Where(s => s.UsuarioId == usuarioId)
                                  .Include(m=>m.Modelo.Marca);
            if (carros != null)
            {
                return carros;
            }
            return null;
        }


        // GET: api/Carroes/5
        [ResponseType(typeof(Carro))]
        public async Task<IHttpActionResult> GetCarro(int id)
        {
            Carro carro = await db.Carro.FindAsync(id);
            if (carro == null)
            {
                return NotFound();
            }

            return Ok(carro);
        }

        // PUT: api/Carroes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCarro(int id, Carro carro)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != carro.CarroId)
            {
                return BadRequest();
            }

            db.Entry(carro).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarroExists(id))
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

        // POST: api/Carroes
        [ResponseType(typeof(Carro))]
        public async Task<IHttpActionResult> PostCarro(Carro carro)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Carro.Add(carro);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = carro.CarroId }, carro);
        }

        // DELETE: api/Carroes/5
        [ResponseType(typeof(Carro))]
        public async Task<IHttpActionResult> DeleteCarro(int id)
        {
            Carro carro = await db.Carro.FindAsync(id);
            if (carro == null)
            {
                return NotFound();
            }

            db.Carro.Remove(carro);
            await db.SaveChangesAsync();

            return Ok(carro);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CarroExists(int id)
        {
            return db.Carro.Count(e => e.CarroId == id) > 0;
        }
    }
}