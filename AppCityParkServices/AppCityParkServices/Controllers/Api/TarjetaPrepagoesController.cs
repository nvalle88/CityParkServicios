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
    [RoutePrefix("api/TarjetaPrepagoes")]

    public class TarjetaPrepagoesController : ApiController
    {
        private CityParkApp db = new CityParkApp();


        [HttpPost]
        [Route("BuscarSaldo")]
        public IHttpActionResult BuscarCodigoTarjeta(JObject form)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    db.Configuration.ProxyCreationEnabled = false;

                    var Codigo = string.Empty;
                    var UsuarioId = string.Empty;


                    dynamic jsonObject = form;

                    try
                    {
                        Codigo = jsonObject.Codigo.Value;
                        UsuarioId = jsonObject.UsuarioId.Value;
                    }
                    catch (Exception)
                    {

                        return BadRequest("LLamada Incorrecta");
                    }

                    var usuario = Convert.ToInt32(UsuarioId);

                    var existeTarjeta = db.TarjetaPrepago
                                      .Where(t => t.Numero == Codigo && t.Activa == false && t.FechaVence >= DateTime.Now)
                                      .FirstOrDefault();
                    if (existeTarjeta == null)
                    {
                        return BadRequest("El código de la tarjeta no existe o está consumida...");
                    }

                    existeTarjeta.Activa = true;
                    db.Entry(existeTarjeta).State = EntityState.Modified;
                    db.SaveChanges();

                    var usuarioPrepago = new UsuarioTarjetaPrepago
                    {
                        UsuarioId = usuario,
                        TarjetaPrepagoId = existeTarjeta.TarjetaPrepagoId,
                    };

                    db.UsuarioTarjetaPrepago.Add(usuarioPrepago);
                    db.SaveChanges();
                    var IngresarSaldo = db.Saldo.Where(s => s.UsuarioId == usuario).FirstOrDefault();

                    if (IngresarSaldo == null)
                    {
                        var nuevoSaldo = new Saldo
                        {
                            UsuarioId = usuario,
                            Saldo1 = existeTarjeta.Saldo,
                        };

                        db.Saldo.Add(nuevoSaldo);
                        db.SaveChanges();
                        transaction.Commit();
                        return Ok(nuevoSaldo);

                    }

                    IngresarSaldo.Saldo1 = IngresarSaldo.Saldo1 + existeTarjeta.Saldo;

                    db.Entry(IngresarSaldo).State = EntityState.Modified;

                    db.SaveChanges();

                    transaction.Commit();
                    return Ok(IngresarSaldo);
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return BadRequest("Error");

                }
            }
        }


        // GET: api/TarjetaPrepagoes
        public IQueryable<TarjetaPrepago> GetTarjetaPrepago()
        {
            return db.TarjetaPrepago;
        }

        // GET: api/TarjetaPrepagoes/5
        [ResponseType(typeof(TarjetaPrepago))]
        public async Task<IHttpActionResult> GetTarjetaPrepago(int id)
        {
            TarjetaPrepago tarjetaPrepago = await db.TarjetaPrepago.FindAsync(id);
            if (tarjetaPrepago == null)
            {
                return NotFound();
            }

            return Ok(tarjetaPrepago);
        }

        // PUT: api/TarjetaPrepagoes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTarjetaPrepago(int id, TarjetaPrepago tarjetaPrepago)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tarjetaPrepago.TarjetaPrepagoId)
            {
                return BadRequest();
            }

            db.Entry(tarjetaPrepago).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TarjetaPrepagoExists(id))
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

        // POST: api/TarjetaPrepagoes
        [ResponseType(typeof(TarjetaPrepago))]
        public async Task<IHttpActionResult> PostTarjetaPrepago(TarjetaPrepago tarjetaPrepago)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TarjetaPrepago.Add(tarjetaPrepago);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tarjetaPrepago.TarjetaPrepagoId }, tarjetaPrepago);
        }

        // DELETE: api/TarjetaPrepagoes/5
        [ResponseType(typeof(TarjetaPrepago))]
        public async Task<IHttpActionResult> DeleteTarjetaPrepago(int id)
        {
            TarjetaPrepago tarjetaPrepago = await db.TarjetaPrepago.FindAsync(id);
            if (tarjetaPrepago == null)
            {
                return NotFound();
            }

            db.TarjetaPrepago.Remove(tarjetaPrepago);
            await db.SaveChangesAsync();

            return Ok(tarjetaPrepago);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TarjetaPrepagoExists(int id)
        {
            return db.TarjetaPrepago.Count(e => e.TarjetaPrepagoId == id) > 0;
        }
    }
}