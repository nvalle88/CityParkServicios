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
    public class SalarioBasicoesController : ApiController
    {
        private CityParkApp db = new CityParkApp();

        // GET: api/SalarioBasicoes
        public IQueryable<SalarioBasico> GetSalarioBasico()
        {
            return db.SalarioBasico;
        }

        // GET: api/SalarioBasicoes/5
        [ResponseType(typeof(SalarioBasico))]
        public async Task<IHttpActionResult> GetSalarioBasico(int id)
        {
            SalarioBasico salarioBasico = await db.SalarioBasico.FindAsync(id);
            if (salarioBasico == null)
            {
                return NotFound();
            }

            return Ok(salarioBasico);
        }

        // PUT: api/SalarioBasicoes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSalarioBasico(int id, SalarioBasico salarioBasico)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != salarioBasico.SalarioBasicoId)
            {
                return BadRequest();
            }

            db.Entry(salarioBasico).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SalarioBasicoExists(id))
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

        // POST: api/SalarioBasicoes
        [ResponseType(typeof(SalarioBasico))]
        public async Task<IHttpActionResult> PostSalarioBasico(SalarioBasico salarioBasico)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SalarioBasico.Add(salarioBasico);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = salarioBasico.SalarioBasicoId }, salarioBasico);
        }

        // DELETE: api/SalarioBasicoes/5
        [ResponseType(typeof(SalarioBasico))]
        public async Task<IHttpActionResult> DeleteSalarioBasico(int id)
        {
            SalarioBasico salarioBasico = await db.SalarioBasico.FindAsync(id);
            if (salarioBasico == null)
            {
                return NotFound();
            }

            db.SalarioBasico.Remove(salarioBasico);
            await db.SaveChangesAsync();

            return Ok(salarioBasico);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SalarioBasicoExists(int id)
        {
            return db.SalarioBasico.Count(e => e.SalarioBasicoId == id) > 0;
        }
    }
}