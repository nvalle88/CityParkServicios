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
using AppCityParkServices.Clases;
using AppCityParkServices.Utils;

namespace AppCityParkServices.Controllers.Api
{
    [RoutePrefix("api/Agentes")]

    public class AgentesController : ApiController
    {
        private CityParkApp db = new CityParkApp();

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

                Nombreusuario = jsonObject.Agente.Value;

                Codificar codificar = new Codificar
                {
                    Entrada = jsonObject.Contrasena.Value
                };
                codificar = CodificarHelper.SHA512(codificar);


                contrasena = codificar.Salida;

            }
            catch (Exception)
            {

                return BadRequest("LLamada Incorrecta");
            }

            var existeAgente = db.Agente.
                                Where(u => u.Nombre == Nombreusuario && u.Contrasena == contrasena)
                                .Include(u => u.Multa)
                                .FirstOrDefault();

            if (existeAgente == null)
            {
                return BadRequest("Usuario o contraseña incorrecto...");
            }

            var respuestaAgente = new Agente
            {
                AgenteId=existeAgente.AgenteId,
                Apellido=existeAgente.Apellido,
                Multa=existeAgente.Multa,
                Contrasena=existeAgente.Contrasena,
                Nombre=existeAgente.Nombre,
            };

            return Ok(respuestaAgente);

        }

        [HttpPost]
        [Route("ResetPassword")]
        public async Task<Response> ResetPassword([FromBody] Agente agente)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                var agenteRequest = await db.Agente.Where(x => x.AgenteId == agente.AgenteId).FirstOrDefaultAsync();
                if (agenteRequest == null)
                {
                    return new Response { IsSuccess = false };
                }
                var cod = new Codificar
                {
                    Entrada= agenteRequest.Nombre,
                };
                   
                var codificar = CodificarHelper.SHA512(cod);
                agenteRequest.Contrasena = codificar.Salida;

                db.Entry(agenteRequest).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return new Response { IsSuccess = true };
            }
            catch (Exception)
            {
                return new Response { IsSuccess = false };
            }
        }

        [HttpPost]
        [Route("EditAgente")]
        public async Task<Response> EditAgente([FromBody]Agente agente)
        {
            try
            {
                var agenteRequest = await db.Agente.Where(x => x.AgenteId == agente.AgenteId).FirstOrDefaultAsync();
                if (agenteRequest == null)
                {
                    return new Response { IsSuccess = false };
                }
                agenteRequest.Nombre = agente.Nombre;
                agenteRequest.Apellido = agente.Apellido;
                agenteRequest.SectorId = agente.SectorId;

                db.Entry(agenteRequest).State=EntityState.Modified;
                await db.SaveChangesAsync();

                return new Response { IsSuccess = true };
            }
            catch (Exception)
            {
                return new Response { IsSuccess = false };
                throw;
            }
        }

        [HttpPost]
        [Route("GetAgente")]
        public async Task<Response> GetAgente([FromBody]Agente agente)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                var agenteRequest = await db.Agente.Where(x => x.AgenteId == agente.AgenteId).FirstOrDefaultAsync();
                if (agenteRequest == null)
                {
                    return new Response { IsSuccess = false };
                }
                if (agenteRequest.SectorId==null)
                {
                    agenteRequest.SectorId = 0;
                }
                
                return new Response { IsSuccess = true,Result=agenteRequest };
            }
            catch (Exception)
            {
                return new Response { IsSuccess = false };
                throw;
            }
        }

        [HttpPost]
        [Route("DeleteAgente")]
        public async Task<Response> DeleteAgente([FromBody]Agente agente)
        {
            try
            {
                var agenteRequest = await db.Agente.Where(x => x.AgenteId == agente.AgenteId).FirstOrDefaultAsync();
                if (agenteRequest == null)
                {
                    return new Response { IsSuccess = false };
                }

                db.Agente.Remove(agenteRequest);
                await db.SaveChangesAsync();

                return new Response { IsSuccess=true};
            }
            catch (Exception)
            {
                return new Response { IsSuccess = false };
                throw;
            }
        }


        [HttpPost]
        [Route("GetAgentesPorEmpresa")]
        public async Task<IEnumerable<AgenteRequest>> GetAgentesPorEmpresa([FromBody] Empresa empresa)
        {
           
            var agentes =await db.Agente.Where(x => x.EmpresaId == empresa.EmpresaId).Include(x=>x.Empresa).Include(x=>x.Sector).ToListAsync();

            if (agentes==null)
            {
                return null;
            }

            var listaSalida = new List<AgenteRequest>();

            foreach (var agente in agentes)
            {
                if (agente.SectorId==null)
                {
                    listaSalida.Add(new AgenteRequest
                    {
                        AgenteId = agente.AgenteId,
                        Apellido = agente.Apellido,
                        Direccion = agente.Empresa.Direccion,
                        Nombre = agente.Nombre,
                        NombreSector = Constants.Constants.ValorNull,
                        RazonSocial = agente.Empresa.RazonSocial,
                        Ruc = agente.Empresa.Ruc,
                    });
                }
                else
                {
                    listaSalida.Add(new AgenteRequest
                    {
                        AgenteId = agente.AgenteId,
                        Apellido = agente.Apellido,
                        Direccion = agente.Empresa.Direccion,
                        Nombre = agente.Nombre,
                        NombreSector = agente.Sector.NombreSector,
                        RazonSocial = agente.Empresa.RazonSocial,
                        Ruc = agente.Empresa.Ruc,
                    });
                }
                
            }

            return listaSalida;
        }
        // GET: api/Agentes
        public IQueryable<Agente> GetAgente()
        {
            return db.Agente;
        }

        // GET: api/Agentes/5
        [ResponseType(typeof(Agente))]
        public async Task<IHttpActionResult> GetAgente(int id)
        {
            Agente agente = await db.Agente.FindAsync(id);
            if (agente == null)
            {
                return NotFound();
            }

            return Ok(agente);
        }

        //// PUT: api/Agentes/5
        //[ResponseType(typeof(void))]
        //public async Task<IHttpActionResult> PutAgente(int id, Agente agente)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != agente.AgenteId)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(agente).State = EntityState.Modified;

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!AgenteExists(id))
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

        [HttpPut]
        [Route("PasswordUpdate")]
        public async Task<IHttpActionResult> PasswordUpdate(UsuarioPasswordRequest usuarioNew)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                Agente agente = await db.Agente.FindAsync(usuarioNew.UsuarioId);
                Codificar codificar = new Codificar
                {
                    Entrada = usuarioNew.Contrasena
                };
                codificar = CodificarHelper.SHA512(codificar);
                if (agente.Contrasena == codificar.Salida)
                {
                    codificar = new Codificar
                    {
                        Entrada = usuarioNew.ContrasenaNueva
                    };
                    codificar = CodificarHelper.SHA512(codificar);
                    agente.Contrasena = codificar.Salida;
                    db.Entry(agente).State = EntityState.Modified;
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
                if (!AgenteExists(usuarioNew.UsuarioId))
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


        // POST: api/Agentes
        [HttpPost]
        [Route("CreateAgente")]
        [ResponseType(typeof(Agente))]
        public async Task<Response> CreateAgente([FromBody]Agente agente)
        {
            try
            {
                if (agente == null)
                {
                    return new Response { IsSuccess = false };
                }
                db.Agente.Add(agente);
                await db.SaveChangesAsync();
                return new Response {IsSuccess=true,Result=agente};
               
            }
            catch (Exception ex)
            {

                return new Response { IsSuccess = false };
            }
        }

        [ResponseType(typeof(Agente))]
        public async Task<IHttpActionResult> PostAgente(Agente agente)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Agente.Add(agente);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = agente.AgenteId }, agente);
        }

        // DELETE: api/Agentes/5
        [ResponseType(typeof(Agente))]
        public async Task<IHttpActionResult> DeleteAgente(int id)
        {
            Agente agente = await db.Agente.FindAsync(id);
            if (agente == null)
            {
                return NotFound();
            }

            db.Agente.Remove(agente);
            await db.SaveChangesAsync();

            return Ok(agente);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AgenteExists(int id)
        {
            return db.Agente.Count(e => e.AgenteId == id) > 0;
        }
    }
}