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
    [RoutePrefix("api/Usuarios")]
    public class UsuariosController : ApiController
    {
        private CityParkApp db = new CityParkApp();

        // GET: api/Usuarios
        public IQueryable<Usuario> GetUsuario()
        {
            return db.Usuario;
        }

        [HttpPost]
        [Route("Login")]
        public IHttpActionResult Login(JObject form)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var Nombreusuario = string.Empty;
            var contrasena = string.Empty;
            dynamic jsonObject = form;

            try
            {

                Nombreusuario = jsonObject.Usuario.Value;
                contrasena = jsonObject.Contrasena.Value;
            }
            catch (Exception)
            {

                return BadRequest("LLamada Incorrecta");
            }

            var existeUsuario = db.Usuario.
                                Where(u => u.Nombre == Nombreusuario && u.Contrasena == contrasena)
                                .FirstOrDefault();

            if (existeUsuario == null)
            {
                return BadRequest("Usuario o contraseña incorrecto...");
            }

            var respuestaUsuario = new Usuario
            {
                UsuarioId = existeUsuario.UsuarioId,
                Carro = existeUsuario.Carro,
                UsuarioTarjetaPrepago = existeUsuario.UsuarioTarjetaPrepago,
                TarjetaCredito = existeUsuario.TarjetaCredito,
                Parqueo = existeUsuario.Parqueo,
                Saldo = existeUsuario.Saldo,
                Contrasena = existeUsuario.Contrasena,
                Nombre = existeUsuario.Nombre,
                
            };

            return Ok(respuestaUsuario);

        }


        // GET: api/Usuarios/5
        [ResponseType(typeof(Usuario))]
        public async Task<IHttpActionResult> GetUsuario(int id)
        {
            Usuario usuario = await db.Usuario.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            return Ok(usuario);
        }

        // PUT: api/Usuarios/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutUsuario(int id, Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != usuario.UsuarioId)
            {
                return BadRequest();
            }

            db.Entry(usuario).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
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

        // POST: api/Usuarios
        [ResponseType(typeof(Usuario))]
        public async Task<IHttpActionResult> PostUsuario(Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Usuario.Add(usuario);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = usuario.UsuarioId }, usuario);
        }

        // DELETE: api/Usuarios/5
        [ResponseType(typeof(Usuario))]
        public async Task<IHttpActionResult> DeleteUsuario(int id)
        {
            Usuario usuario = await db.Usuario.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            db.Usuario.Remove(usuario);
            await db.SaveChangesAsync();

            return Ok(usuario);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UsuarioExists(int id)
        {
            return db.Usuario.Count(e => e.UsuarioId == id) > 0;
        }
    }
}