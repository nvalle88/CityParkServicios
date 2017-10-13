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
    [RoutePrefix("api/Transaccions")]

    public class TransaccionsController : ApiController
    {
        private CityParkApp db = new CityParkApp();

        // GET: api/Transaccions
        public IQueryable<Transaccion> GetTransaccion()
        {
            return db.Transaccion;
        }

        // GET: api/Transaccions/5
        [ResponseType(typeof(Transaccion))]
        public async Task<IHttpActionResult> GetTransaccion(int id)
        {
            Transaccion transaccion = await db.Transaccion.FindAsync(id);
            if (transaccion == null)
            {
                return NotFound();
            }

            return Ok(transaccion);
        }

        // PUT: api/Transaccions/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTransaccion(int id, Transaccion transaccion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != transaccion.TransaccionId)
            {
                return BadRequest();
            }

            db.Entry(transaccion).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransaccionExists(id))
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

        // POST: api/Transaccions
        [ResponseType(typeof(Transaccion))]
        public async Task<IHttpActionResult> PostTransaccion(Transaccion transaccion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Transaccion.Add(transaccion);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = transaccion.TransaccionId }, transaccion);
        }

        //Transaccion para Recargar Saldo
        #region Recargar Saldo
        [HttpPost]
        [Route("RecargarSaldo")]
        public async Task<IHttpActionResult> RecargarSaldo(Transaccion transaccion)
        {            
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    db.Configuration.ProxyCreationEnabled = false;
                   var usuario = Convert.ToInt32(transaccion.UsuarioId);
                   var IngresarSaldo = db.Saldo.Where(s => s.UsuarioId == usuario).FirstOrDefault();
                    if (IngresarSaldo == null)
                    {
                        var nuevoSaldo = new Saldo
                        {
                            UsuarioId = usuario,
                            Saldo1 =(decimal)transaccion.Monto,
                        };
                        db.Saldo.Add(nuevoSaldo);
                        await db.SaveChangesAsync();
                        transaction.Commit();
                        return Ok(nuevoSaldo);
                    }
                    IngresarSaldo.Saldo1 = IngresarSaldo.Saldo1 + (decimal)transaccion.Monto;
                    db.Entry(IngresarSaldo).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    transaction.Commit();

                    //______________

                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }
                    db.Transaccion.Add(transaccion);
                    await db.SaveChangesAsync();
                    
                    //_______________

                    return Ok(IngresarSaldo);
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return BadRequest("Error");
                }
            }
        }

        #endregion

        //Transaccion para Parquear
        #region Parqueo Transaccion

        public async Task<IHttpActionResult> PostParqueo(Parqueo parqueo)
        {
            db.Configuration.ProxyCreationEnabled = false;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Parqueo.Add(parqueo);
            await db.SaveChangesAsync();
            var carro = db.Carro.Where(c => c.CarroId == parqueo.CarroId).Include(c => c.Modelo.Marca).FirstOrDefault();
            parqueo.Carro = carro;
            parqueo.CarroId = carro.CarroId;
            return Ok(parqueo);
        }


        #endregion


        // DELETE: api/Transaccions/5
        [ResponseType(typeof(Transaccion))]
        public async Task<IHttpActionResult> DeleteTransaccion(int id)
        {
            Transaccion transaccion = await db.Transaccion.FindAsync(id);
            if (transaccion == null)
            {
                return NotFound();
            }

            db.Transaccion.Remove(transaccion);
            await db.SaveChangesAsync();

            return Ok(transaccion);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TransaccionExists(int id)
        {
            return db.Transaccion.Count(e => e.TransaccionId == id) > 0;
        }
    }
}