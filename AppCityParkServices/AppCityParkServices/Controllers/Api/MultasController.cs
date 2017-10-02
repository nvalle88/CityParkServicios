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
using System.IO;
using AppCityParkServices.Clases;

namespace AppCityParkServices.Controllers.Api
{
    [RoutePrefix("api/Multas")]
    public class MultasController : ApiController
    {
        private CityParkApp db = new CityParkApp();


        [HttpPost]
        [Route("SetFoto")]
        public IHttpActionResult SetFoto(FotoRequest fotoRequest)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var multa = db.Multa.Find(fotoRequest.Id);

            if (multa==null)
            {
                return BadRequest("La multa no existe..."); 
            };

            var stream = new MemoryStream(fotoRequest.Array);

            var file = string.Format("{0}.jpg", fotoRequest.Id);
            var folder = "~/Content/Multas";
            var fullPath = string.Format("{0}/{1}",folder,file);

           //if(!Directory.Exists(folder))
           // {
           //     Directory.CreateDirectory(folder);
           // }

            var response = FileHelper.UploadFoto(stream,folder,file);

            if (!response)
            {
                BadRequest("Imagen de la multa no se pudo subir al servidor...");
            }

            multa.Foto = fullPath;
            db.Entry(multa).State = EntityState.Modified;
            db.SaveChanges();
            return Ok("OK");

        }
        // GET: api/Multas
        [HttpPost]
        [Route("GetMultas")]
        public IQueryable<Multa> GetMulta(JObject form)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var AgenteId = string.Empty;

            dynamic jsonObject = form;

            try
            {
                AgenteId = jsonObject.AgenteId.Value;
            }
            catch (Exception)
            {

                return null;
            }

            var agenteId = Convert.ToInt32(AgenteId);
            var multa = db.Multa.Where(s => s.AgenteId == agenteId 
                                       && s.Fecha.Day ==DateTime.Now.Day 
                                       && s.Fecha.Month ==DateTime.Now.Month 
                                       && s.Fecha.Year ==DateTime.Now.Year
                                       )
                                       .Include(s=>s.Agente)
                                       .OrderByDescending(s=>s.Fecha);
            if (multa != null)
            {
                return multa;
            }
            return null;
        }

        // GET: api/Multas/5
        [ResponseType(typeof(Multa))]
        public async Task<IHttpActionResult> GetMulta(int id)
        {
            Multa multa = await db.Multa.FindAsync(id);
            if (multa == null)
            {
                return NotFound();
            }

            return Ok(multa);
        }

        // PUT: api/Multas/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutMulta(int id, Multa multa)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != multa.MultaId)
            {
                return BadRequest();
            }

            db.Entry(multa).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MultaExists(id))
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

        // POST: api/Multas
        [HttpPost]
        [Route("InsertarMulta")]
        [ResponseType(typeof(Multa))]
        public async Task<IHttpActionResult> PostMulta(Multa multa)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Multa.Add(multa);
            await db.SaveChangesAsync();

            return Ok(multa);
        }

        // DELETE: api/Multas/5
        [ResponseType(typeof(Multa))]
        public async Task<IHttpActionResult> DeleteMulta(int id)
        {
            Multa multa = await db.Multa.FindAsync(id);
            if (multa == null)
            {
                return NotFound();
            }

            db.Multa.Remove(multa);
            await db.SaveChangesAsync();

            return Ok(multa);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MultaExists(int id)
        {
            return db.Multa.Count(e => e.MultaId == id) > 0;
        }
    }
}