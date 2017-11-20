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
    [RoutePrefix("api/Codigoes")]

    public class CodigoesController : ApiController
    {
        private CityParkApp db = new CityParkApp();

        // GET: api/Codigoes
        public IQueryable<Codigo> GetCodigo()
        {
            return db.Codigo;
        }

        // GET: api/Codigoes/5
        [ResponseType(typeof(Codigo))]
        public async Task<IHttpActionResult> GetCodigo(int id)
        {
            Codigo codigo = await db.Codigo.FindAsync(id);
            if (codigo == null)
            {
                return NotFound();
            }

            return Ok(codigo);
        }

        // GET: api/Codigoes/5
        [HttpPost]
        [Route("GetRandomCode")]
        public async Task<Response> GetRandomCode(CodigoRequest usuario)
        {
            int CodeLength = 4;
            string _allowedChars = "ABCDEFGHJKLMNPQRSTUVWXYZ1234567890";
            Byte[] randomBytes = new Byte[CodeLength];
            char[] chars = new char[CodeLength];
            int allowedCharCount = _allowedChars.Length;
            for (int i = 0; i < CodeLength; i++)
            {
                Random randomObj = new Random();
                randomObj.NextBytes(randomBytes);
                chars[i] = _allowedChars[(int)randomBytes[i] % allowedCharCount];
            }
            //Verificar si existe el dispositivo caso contrario Registrarlo


            //

            Codigo _codigo = new Codigo
            {
                UsuarioId = usuario.UsuarioId,
                Codigo1 = new string(chars),
                DispositivoId = usuario.DispositivoId,
                Usado = false,
            };


            db.Codigo.Add(_codigo);
            await db.SaveChangesAsync();

            CreatedAtRoute("DefaultApi", new { id = _codigo.CodigoId },_codigo);
            return
            new Response
            {
                IsSuccess = true,
                Message = "Gódigo generado con éxito",
                Result = _codigo,
            };


            
        }


        // PUT: api/Codigoes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCodigo(int id, Codigo codigo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != codigo.CodigoId)
            {
                return BadRequest();
            }

            db.Entry(codigo).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CodigoExists(id))
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

        // POST: api/Codigoes
        [ResponseType(typeof(Codigo))]
        public async Task<IHttpActionResult> PostCodigo(Codigo codigo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Codigo.Add(codigo);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = codigo.CodigoId }, codigo);
        }

        // DELETE: api/Codigoes/5
        [ResponseType(typeof(Codigo))]
        public async Task<IHttpActionResult> DeleteCodigo(int id)
        {
            Codigo codigo = await db.Codigo.FindAsync(id);
            if (codigo == null)
            {
                return NotFound();
            }

            db.Codigo.Remove(codigo);
            await db.SaveChangesAsync();

            return Ok(codigo);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CodigoExists(int id)
        {
            return db.Codigo.Count(e => e.CodigoId == id) > 0;
        }

    }
}