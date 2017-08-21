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
    [RoutePrefix("api/Saldoes")]

    public class SaldoesController : ApiController
    {
        private CityParkApp db = new CityParkApp();

        // GET: api/Saldoes
        public IQueryable<Saldo> GetSaldo()
        {
            return db.Saldo;
        }


        [HttpPost]
        [Route("ConsultarSaldo")]
        public IHttpActionResult GetSaldo(JObject form)
        {
            db.Configuration.ProxyCreationEnabled = false;

            var UsuarioId =string.Empty;

            dynamic jsonObject = form;

            try
            {
                UsuarioId = jsonObject.UsuarioId.Value;
            }
            catch (Exception)
            {

                return BadRequest("LLamada Incorrecta");
            }

            var usuario = Convert.ToInt32(UsuarioId);
            Saldo saldo = db.Saldo.Where(s => s.UsuarioId == usuario).FirstOrDefault();
            if (saldo == null)
            {
                return BadRequest("El usuario no posee saldo");
            }

            return Ok(saldo);
        }


        // GET: api/Saldoes/5
        [ResponseType(typeof(Saldo))]
        public async Task<IHttpActionResult> GetSaldo(int id)
        {
            Saldo saldo = await db.Saldo.FindAsync(id);
            if (saldo == null)
            {
                return NotFound();
            }

            return Ok(saldo);
        }

        // PUT: api/Saldoes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSaldo(int id, Saldo saldo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != saldo.SaldoId)
            {
                return BadRequest();
            }

            db.Entry(saldo).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SaldoExists(id))
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

        // POST: api/Saldoes
        [ResponseType(typeof(Saldo))]
        public async Task<IHttpActionResult> PostSaldo(Saldo saldo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Saldo.Add(saldo);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = saldo.SaldoId }, saldo);
        }

        // DELETE: api/Saldoes/5
        [ResponseType(typeof(Saldo))]
        public async Task<IHttpActionResult> DeleteSaldo(int id)
        {
            Saldo saldo = await db.Saldo.FindAsync(id);
            if (saldo == null)
            {
                return NotFound();
            }

            db.Saldo.Remove(saldo);
            await db.SaveChangesAsync();

            return Ok(saldo);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SaldoExists(int id)
        {
            return db.Saldo.Count(e => e.SaldoId == id) > 0;
        }
    }
}