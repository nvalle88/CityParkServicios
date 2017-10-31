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
    public class SOesController : ApiController
    {
        private CityParkApp db = new CityParkApp();

        // GET: api/SOes
        public IQueryable<SO> GetSO()
        {
            return db.SO;
        }

        // GET: api/SOes/5
        [ResponseType(typeof(SO))]
        public async Task<IHttpActionResult> GetSO(int id)
        {
            SO sO = await db.SO.FindAsync(id);
            if (sO == null)
            {
                return NotFound();
            }

            return Ok(sO);
        }

        // PUT: api/SOes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSO(int id, SO sO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != sO.SOId)
            {
                return BadRequest();
            }

            db.Entry(sO).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SOExists(id))
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

        // POST: api/SOes
        [ResponseType(typeof(SO))]
        public async Task<IHttpActionResult> PostSO(SO sO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SO.Add(sO);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = sO.SOId }, sO);
        }

        // DELETE: api/SOes/5
        [ResponseType(typeof(SO))]
        public async Task<IHttpActionResult> DeleteSO(int id)
        {
            SO sO = await db.SO.FindAsync(id);
            if (sO == null)
            {
                return NotFound();
            }

            db.SO.Remove(sO);
            await db.SaveChangesAsync();

            return Ok(sO);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SOExists(int id)
        {
            return db.SO.Count(e => e.SOId == id) > 0;
        }
    }
}