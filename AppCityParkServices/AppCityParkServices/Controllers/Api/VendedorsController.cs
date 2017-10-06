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
    public class VendedorsController : ApiController
    {
        private CityParkApp db = new CityParkApp();

        // GET: api/Vendedors
        public IQueryable<Vendedor> GetVendedor()
        {
            return db.Vendedor;
        }

        // GET: api/Vendedors/5
        [ResponseType(typeof(Vendedor))]
        public async Task<IHttpActionResult> GetVendedor(int id)
        {
            Vendedor vendedor = await db.Vendedor.FindAsync(id);
            if (vendedor == null)
            {
                return NotFound();
            }

            return Ok(vendedor);
        }

        // PUT: api/Vendedors/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutVendedor(int id, Vendedor vendedor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != vendedor.VendedorId)
            {
                return BadRequest();
            }

            db.Entry(vendedor).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VendedorExists(id))
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

        // POST: api/Vendedors
        [ResponseType(typeof(Vendedor))]
        public async Task<IHttpActionResult> PostVendedor(Vendedor vendedor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Vendedor.Add(vendedor);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (VendedorExists(vendedor.VendedorId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = vendedor.VendedorId }, vendedor);
        }

        // DELETE: api/Vendedors/5
        [ResponseType(typeof(Vendedor))]
        public async Task<IHttpActionResult> DeleteVendedor(int id)
        {
            Vendedor vendedor = await db.Vendedor.FindAsync(id);
            if (vendedor == null)
            {
                return NotFound();
            }

            db.Vendedor.Remove(vendedor);
            await db.SaveChangesAsync();

            return Ok(vendedor);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool VendedorExists(int id)
        {
            return db.Vendedor.Count(e => e.VendedorId == id) > 0;
        }
    }
}