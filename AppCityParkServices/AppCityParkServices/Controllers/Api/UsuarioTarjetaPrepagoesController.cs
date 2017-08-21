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
    public class UsuarioTarjetaPrepagoesController : ApiController
    {
        private CityParkApp db = new CityParkApp();

        // GET: api/UsuarioTarjetaPrepagoes
        public IQueryable<UsuarioTarjetaPrepago> GetUsuarioTarjetaPrepago()
        {
            return db.UsuarioTarjetaPrepago;
        }

        // GET: api/UsuarioTarjetaPrepagoes/5
        [ResponseType(typeof(UsuarioTarjetaPrepago))]
        public async Task<IHttpActionResult> GetUsuarioTarjetaPrepago(int id)
        {
            UsuarioTarjetaPrepago usuarioTarjetaPrepago = await db.UsuarioTarjetaPrepago.FindAsync(id);
            if (usuarioTarjetaPrepago == null)
            {
                return NotFound();
            }

            return Ok(usuarioTarjetaPrepago);
        }

        // PUT: api/UsuarioTarjetaPrepagoes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutUsuarioTarjetaPrepago(int id, UsuarioTarjetaPrepago usuarioTarjetaPrepago)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != usuarioTarjetaPrepago.UsuarioTarjetaPrepagoId)
            {
                return BadRequest();
            }

            db.Entry(usuarioTarjetaPrepago).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioTarjetaPrepagoExists(id))
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

        // POST: api/UsuarioTarjetaPrepagoes
        [ResponseType(typeof(UsuarioTarjetaPrepago))]
        public async Task<IHttpActionResult> PostUsuarioTarjetaPrepago(UsuarioTarjetaPrepago usuarioTarjetaPrepago)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.UsuarioTarjetaPrepago.Add(usuarioTarjetaPrepago);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = usuarioTarjetaPrepago.UsuarioTarjetaPrepagoId }, usuarioTarjetaPrepago);
        }

        // DELETE: api/UsuarioTarjetaPrepagoes/5
        [ResponseType(typeof(UsuarioTarjetaPrepago))]
        public async Task<IHttpActionResult> DeleteUsuarioTarjetaPrepago(int id)
        {
            UsuarioTarjetaPrepago usuarioTarjetaPrepago = await db.UsuarioTarjetaPrepago.FindAsync(id);
            if (usuarioTarjetaPrepago == null)
            {
                return NotFound();
            }

            db.UsuarioTarjetaPrepago.Remove(usuarioTarjetaPrepago);
            await db.SaveChangesAsync();

            return Ok(usuarioTarjetaPrepago);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UsuarioTarjetaPrepagoExists(int id)
        {
            return db.UsuarioTarjetaPrepago.Count(e => e.UsuarioTarjetaPrepagoId == id) > 0;
        }
    }
}