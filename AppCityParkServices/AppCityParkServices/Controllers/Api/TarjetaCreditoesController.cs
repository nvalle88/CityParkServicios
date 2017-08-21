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
    [RoutePrefix("api/TarjetasCreditoes")]
    public class TarjetaCreditoesController : ApiController
    {
        private CityParkApp db = new CityParkApp();

        [HttpPost]
        [Route("GetTarjetasCreditos")]
        public IQueryable<TarjetaCredito> GetTarjetaCredito(JObject form)
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

                return null;
            }

            var usuarioId = Convert.ToInt32(UsuarioId);
            var tarjetaCreditos = db.TarjetaCredito.Where(s => s.UsuarioId == usuarioId);
            if (tarjetaCreditos != null)
            {
                return tarjetaCreditos;
            }
            return null;
        }

        // GET: api/TarjetaCreditoes/5
        [ResponseType(typeof(TarjetaCredito))]
        public async Task<IHttpActionResult> GetTarjetaCredito(int id)
        {
            TarjetaCredito tarjetaCredito = await db.TarjetaCredito.FindAsync(id);
            if (tarjetaCredito == null)
            {
                return NotFound();
            }

            return Ok(tarjetaCredito);
        }

        // PUT: api/TarjetaCreditoes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTarjetaCredito(int id, TarjetaCredito tarjetaCredito)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tarjetaCredito.TarjetaCreditoId)
            {
                return BadRequest();
            }

            db.Entry(tarjetaCredito).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TarjetaCreditoExists(id))
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

        // POST: api/TarjetaCreditoes
        [ResponseType(typeof(TarjetaCredito))]
        public async Task<IHttpActionResult> PostTarjetaCredito(TarjetaCredito tarjetaCredito)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TarjetaCredito.Add(tarjetaCredito);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tarjetaCredito.TarjetaCreditoId }, tarjetaCredito);
        }

        // DELETE: api/TarjetaCreditoes/5
        [ResponseType(typeof(TarjetaCredito))]
        public async Task<IHttpActionResult> DeleteTarjetaCredito(int id)
        {
            TarjetaCredito tarjetaCredito = await db.TarjetaCredito.FindAsync(id);
            if (tarjetaCredito == null)
            {
                return NotFound();
            }

            db.TarjetaCredito.Remove(tarjetaCredito);
            await db.SaveChangesAsync();

            return Ok(tarjetaCredito);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TarjetaCreditoExists(int id)
        {
            return db.TarjetaCredito.Count(e => e.TarjetaCreditoId == id) > 0;
        }
    }
}