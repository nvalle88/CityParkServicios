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
    [RoutePrefix("api/MensajeContactoes")]

    public class MensajeContactoesController : ApiController
    {
        private CityParkApp db = new CityParkApp();

        // GET: api/MensajeContactoes
        public IQueryable<MensajeContacto> GetMensajeContacto()
        {
            return db.MensajeContacto;
        }

        // GET: api/MensajeContactoes/5
        [ResponseType(typeof(MensajeContacto))]
        public async Task<IHttpActionResult> GetMensajeContacto(int id)
        {
            MensajeContacto mensajeContacto = await db.MensajeContacto.FindAsync(id);
            if (mensajeContacto == null)
            {
                return NotFound();
            }

            return Ok(mensajeContacto);
        }

        // PUT: api/MensajeContactoes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutMensajeContacto(int id, MensajeContacto mensajeContacto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != mensajeContacto.idMensajeContacto)
            {
                return BadRequest();
            }

            db.Entry(mensajeContacto).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MensajeContactoExists(id))
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

        // POST: api/MensajeContactoes
        [ResponseType(typeof(MensajeContacto))]
        public async Task<IHttpActionResult> PostMensajeContacto(MensajeContacto mensajeContacto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.MensajeContacto.Add(mensajeContacto);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (MensajeContactoExists(mensajeContacto.idMensajeContacto))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = mensajeContacto.idMensajeContacto }, mensajeContacto);
        }

        // POST: api/MensajeContactoes/Enviar
        [HttpPost]
        [Route("Enviar")]
        [ResponseType(typeof(MensajeContacto))]
        public async Task<IHttpActionResult> Enviar(MensajeContacto mensajeContacto)
        {           

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.MensajeContacto.Add(mensajeContacto);
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (MensajeContactoExists(mensajeContacto.idMensajeContacto))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
            CreatedAtRoute("DefaultApi", new { id = mensajeContacto.idMensajeContacto }, mensajeContacto);
            return Ok("Enviado");
        }

        // DELETE: api/MensajeContactoes/5
        [ResponseType(typeof(MensajeContacto))]
        public async Task<IHttpActionResult> DeleteMensajeContacto(int id)
        {
            MensajeContacto mensajeContacto = await db.MensajeContacto.FindAsync(id);
            if (mensajeContacto == null)
            {
                return NotFound();
            }

            db.MensajeContacto.Remove(mensajeContacto);
            await db.SaveChangesAsync();

            return Ok(mensajeContacto);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MensajeContactoExists(int id)
        {
            return db.MensajeContacto.Count(e => e.idMensajeContacto == id) > 0;
        }
    }
}