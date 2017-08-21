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

namespace AppCityParkServices.Controllers.Api
{
    [RoutePrefix("api/Parqueos")]
    public class ParqueosController : ApiController
    {
        private CityParkApp db = new CityParkApp();

        [HttpPost]
        [Route("GetParqueos")]
        public IQueryable<Parqueo> GetParqueo(JObject form)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var UsuarioId = string.Empty;

            dynamic jsonObject = form;

            try
            {
                UsuarioId = jsonObject.UsuarioId.Value;
            }
            catch (Exception)
            {

                return null;
            }

            var usuarioId = Convert.ToInt32(UsuarioId);
            var parqueos = db.Parqueo.Where(s => s.UsuarioId == usuarioId);
            if (parqueos != null)
            {
                return parqueos;
            }
            return null;

        }

        [HttpPost]
        [Route("BuscarPlaca")]
        
        public  IHttpActionResult BuscarPlaca(JObject form)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var placa = string.Empty;

            dynamic jsonObject = form;

            try
            {
                placa = jsonObject.Placa.Value;
            }
            catch (Exception)
            {

                return null;
            }

            Response response; 
            var parqueos = db.Parqueo.Where(s => s.Carro.Placa==placa && s.FechaFin.Day==DateTime.Now.Day 
                                             && s.FechaFin.Month==DateTime.Now.Month && s.FechaFin.Year==DateTime.Now.Year)
                                             .Include(s=>s.Carro.Modelo.Marca)
                                             .Include(s=>s.Usuario)
                                             .ToList();
            if (parqueos.Count()==0)
            {
                 response = new Response
                {
                    IsSuccess=false,
                    Message= "El vehículo no cuenta con tiempo de parqueo...",
                    Result=null,
                };
                return Ok(response);

            }

            var pa = parqueos.Where(p => (p.FechaFin.TimeOfDay > DateTime.Now.TimeOfDay)&& p.FechaInicio.TimeOfDay<DateTime.Now.TimeOfDay).FirstOrDefault();


             
            if (pa == null)
            {
                response = new Response
                {
                    IsSuccess = false,
                    Message = "El vehículo no cuenta con tiempo de parqueo...",
                    Result=null,
                };
                return Ok(response);
            }


            return BadRequest("El vehículo cuenta con tiempo de parqueo..."); ;
           

        }

        // GET: api/Parqueos/5
        [ResponseType(typeof(Parqueo))]
        public async Task<IHttpActionResult> GetParqueo(int id)
        {
            Parqueo parqueo = await db.Parqueo.FindAsync(id);
            if (parqueo == null)
            {
                return NotFound();
            }

            return Ok(parqueo);
        }

        // PUT: api/Parqueos/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutParqueo(int id, Parqueo parqueo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != parqueo.ParqueoId)
            {
                return BadRequest();
            }

            db.Entry(parqueo).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParqueoExists(id))
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

        // POST: api/Parqueos
        [HttpPost]
        [Route("InsertarParqueo")]
        [ResponseType(typeof(Parqueo))]
        public async Task<IHttpActionResult> PostParqueo(Parqueo parqueo)
        {
            db.Configuration.ProxyCreationEnabled = false;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Parqueo.Add(parqueo);
            await db.SaveChangesAsync();

           

            var carro = db.Carro.Where(c=>c.CarroId==parqueo.CarroId).Include(c=>c.Modelo.Marca).FirstOrDefault();

            
            parqueo.Carro = carro;
            parqueo.CarroId = carro.CarroId;

            return Ok(parqueo);
        }

        // DELETE: api/Parqueos/5
        [ResponseType(typeof(Parqueo))]
        public async Task<IHttpActionResult> DeleteParqueo(int id)
        {
            Parqueo parqueo = await db.Parqueo.FindAsync(id);
            if (parqueo == null)
            {
                return NotFound();
            }

            db.Parqueo.Remove(parqueo);
            await db.SaveChangesAsync();

            return Ok(parqueo);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ParqueoExists(int id)
        {
            return db.Parqueo.Count(e => e.ParqueoId == id) > 0;
        }
    }
}