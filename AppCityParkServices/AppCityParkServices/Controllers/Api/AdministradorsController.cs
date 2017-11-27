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
using AppCityParkServices.Utils;

namespace AppCityParkServices.Controllers.Api
{
    [RoutePrefix("api/Administradores")]
    public class AdministradorsController : ApiController
    {
        private CityParkApp db = new CityParkApp();

        // GET: api/Administradors
        public IQueryable<Administrador> GetAdministrador()
        {
            return db.Administrador;
        }

        [HttpPost]
        [Route("GetUserAdmin")]
        public async Task<Response> GetUserAdmin([FromBody] Administrador administrador)
        {
            db.Configuration.ProxyCreationEnabled = false;
            if (administrador==null)
            {
                return new Response { IsSuccess = false };
            }

            var userAdmin = await db.Administrador.Where(x=>x.Nombre==administrador.Nombre).FirstOrDefaultAsync();
            if (userAdmin == null)
            {
                return new Response { IsSuccess = false, Message = Mensaje.RegistroNoEncontrado };
            }
            return new Response { IsSuccess = true, Result = userAdmin};
        }

        [HttpPost]
        [Route("Login")]
        public async Task<Response> Login([FromBody] LoginRequest loginRequest)
        {
            db.Configuration.ProxyCreationEnabled = false;
            try
            {
                if (string.IsNullOrEmpty(loginRequest.UserName) || string.IsNullOrEmpty(loginRequest.Password))
                {
                    return new Response { IsSuccess = false };
                }

                var login = await db.Administrador.Where(x => x.Nombre == loginRequest.UserName && x.Contrasela == loginRequest.Password).FirstOrDefaultAsync();
                if (login == null)
                {
                    return new Response { IsSuccess = false, Message = Mensaje.UsuarioContrasenaInvalido };
                }
                return new Response { IsSuccess = true, Result = login };
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        // GET: api/Administradors/5
        [ResponseType(typeof(Administrador))]
        public async Task<IHttpActionResult> GetAdministrador(int id)
        {
            Administrador administrador = await db.Administrador.FindAsync(id);
            if (administrador == null)
            {
                return NotFound();
            }

            return Ok(administrador);
        }

        // PUT: api/Administradors/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutAdministrador(int id, Administrador administrador)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != administrador.AdministradorId)
            {
                return BadRequest();
            }

            db.Entry(administrador).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdministradorExists(id))
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

        // POST: api/Administradors
        [ResponseType(typeof(Administrador))]
        public async Task<IHttpActionResult> PostAdministrador(Administrador administrador)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Administrador.Add(administrador);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = administrador.AdministradorId }, administrador);
        }

        // DELETE: api/Administradors/5
        [ResponseType(typeof(Administrador))]
        public async Task<IHttpActionResult> DeleteAdministrador(int id)
        {
            Administrador administrador = await db.Administrador.FindAsync(id);
            if (administrador == null)
            {
                return NotFound();
            }

            db.Administrador.Remove(administrador);
            await db.SaveChangesAsync();

            return Ok(administrador);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AdministradorExists(int id)
        {
            return db.Administrador.Count(e => e.AdministradorId == id) > 0;
        }
    }
}