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
using AppCityParkServices.Utils;
using AppCityParkServices.Clases;

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
                Codificar codificar = new Codificar { Entrada = jsonObject.Contrasena.Value, };
                codificar = CodificarHelper.SHA512(codificar);
                contrasena = codificar.Salida;
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

          
            Saldo saldo = db.Saldo.Where(s => s.UsuarioId == existeUsuario.UsuarioId).FirstOrDefault();

            var respuestaUsuario = new UsuarioLoginRequest
            {
                UsuarioId = existeUsuario.UsuarioId,
                Carro = existeUsuario.Carro,
                UsuarioTarjetaPrepago = existeUsuario.UsuarioTarjetaPrepago,
                TarjetaCredito = existeUsuario.TarjetaCredito,
                Parqueo = existeUsuario.Parqueo,
                Saldo = null,
                Saldo1= saldo.Saldo1,
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

        //Metodo PUT para Cambiar Contraseña 
        [HttpPut]
        [Route("PasswordUpdate")]
        public async Task<IHttpActionResult> PutUsuario(UsuarioPasswordRequest usuarioNew)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                Usuario usuario = await db.Usuario.FindAsync(usuarioNew.UsuarioId);
                Codificar codificar = new Codificar
                {
                    Entrada = usuarioNew.Contrasena
                };
                codificar = CodificarHelper.SHA512(codificar);
                if (usuario.Contrasena == codificar.Salida)
                {
                    codificar = new Codificar
                    {
                        Entrada = usuarioNew.ContrasenaNueva
                    };
                    codificar = CodificarHelper.SHA512(codificar);
                    usuario.Contrasena = codificar.Salida;
                    db.Entry(usuario).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    return Ok("Se actualizo correctamente");

                }
                else
                {
                    return NotFound();
                }

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(usuarioNew.UsuarioId))
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



        // PUT: api/Usuarios/5
        //[ResponseType(typeof(void))]
        //public async Task<IHttpActionResult> PutUsuario(Usuario usuarioNew)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
           

        //    try
        //    {
        //        Usuario usuario = await db.Usuario.FindAsync(usuarioNew.UsuarioId);
        //        Codificar codificar = new Codificar
        //        {
        //            Entrada = usuarioNew.Contrasena
        //        };
        //        codificar = CodificarHelper.SHA512(codificar);

        //        usuario.Contrasena = codificar.Salida;
        //        db.Entry(usuario).State = EntityState.Modified;
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!UsuarioExists(usuarioNew.UsuarioId))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        // POST: api/Usuarios
        [ResponseType(typeof(Usuario))]
        public async Task<IHttpActionResult> PostUsuario(Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
           
            //Creamos creamos e instanciamos el objeto codificar con la contraseña del usuario
            Codificar codificar = new Codificar
            {
                Entrada=usuario.Contrasena,
            };
            //al objeto codificar le pasamos el metodo utils para que nos devuelva la contraseña codificacada
            codificar = CodificarHelper.SHA512(codificar);
            //Agregamos la contraseña codificada a nuestro objeto usuario.
            usuario.Contrasena = codificar.Salida;


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