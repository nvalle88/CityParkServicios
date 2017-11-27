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
    [RoutePrefix("api/Empresas")]
    public class EmpresasController : ApiController
    {

        private CityParkApp db = new CityParkApp();

        [Route("GetEmpresas")]
        public IQueryable<Empresa> GetEmpresa()
        {
            return db.Empresa;
        }

        // GET: api/Empresas/5

        [HttpPost]
        [Route("GetEmpresaAdmin")]
        [ResponseType(typeof(Empresa))]
        public async Task<Response> GetEmpresa([FromBody] Administrador administrador)
        {
            var empresa = await db.Empresa.FindAsync(administrador.EmpresaId);
            if (empresa == null)
            {
                return new Response { IsSuccess = false };
            }

            return new Response { IsSuccess = true, Result = empresa };
        }


        [ResponseType(typeof(Empresa))]
        public async Task<IHttpActionResult> GetEmpresa(int id)
        {
            Empresa empresa = await db.Empresa.FindAsync(id);
            if (empresa == null)
            {
                return NotFound();
            }

            return Ok(empresa);
        }

        // PUT: api/Empresas/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutEmpresa(int id, Empresa empresa)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != empresa.EmpresaId)
            {
                return BadRequest();
            }

            db.Entry(empresa).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmpresaExists(id))
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

        // POST: api/Empresas
        [ResponseType(typeof(Empresa))]
        public async Task<IHttpActionResult> PostEmpresa(Empresa empresa)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Empresa.Add(empresa);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = empresa.EmpresaId }, empresa);
        }

        // DELETE: api/Empresas/5
        [ResponseType(typeof(Empresa))]
        public async Task<IHttpActionResult> DeleteEmpresa(int id)
        {
            Empresa empresa = await db.Empresa.FindAsync(id);
            if (empresa == null)
            {
                return NotFound();
            }

            db.Empresa.Remove(empresa);
            await db.SaveChangesAsync();

            return Ok(empresa);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EmpresaExists(int id)
        {
            return db.Empresa.Count(e => e.EmpresaId == id) > 0;
        }
    }
}