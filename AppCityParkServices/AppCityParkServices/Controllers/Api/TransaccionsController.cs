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

        //Transaccion para Saldos -Recargas y Debitos
        #region Recargar Saldo
        [HttpPost]
        [Route("RecargarSaldo")]
        public async Task<IHttpActionResult> RecargarSaldo(RefillRequest transaccion)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    db.Configuration.ProxyCreationEnabled = false;

                    var codigo = db.Codigo.Where(s => s.Codigo1 == transaccion.codigo.Codigo1).FirstOrDefault();
                    var IngresarSaldo = db.Saldo.Where(s => s.UsuarioId == codigo.UsuarioId).FirstOrDefault();
                    transaccion.transaccion.UsuarioId = codigo.UsuarioId;
                    if (IngresarSaldo == null)
                    {
                        var nuevoSaldo = new Saldo
                        {
                            UsuarioId = codigo.UsuarioId,
                            Saldo1 =(double)transaccion.transaccion.Monto,
                        };
                        db.Saldo.Add(nuevoSaldo);
                        await db.SaveChangesAsync();
                        transaction.Commit();
                        return Ok(nuevoSaldo);
                    }

                    IngresarSaldo.Saldo1 = IngresarSaldo.Saldo1 + (double)transaccion.transaccion.Monto;
                    db.Entry(IngresarSaldo).State = EntityState.Modified;

                    await db.SaveChangesAsync();
                    
                    transaction.Commit();

                    //______________

                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }
                    db.Transaccion.Add(transaccion.transaccion);
                    await db.SaveChangesAsync();

                    //_______________
                   List<string> tags= new List<string>();

                    var PlazaDB = db.Dispositivo.Where(x => x.UsuarioId == transaccion.transaccion.UsuarioId).ToList();
                    foreach (Dispositivo element in PlazaDB)
                    {
                        tags.Add(element.UniqueId);
                    }
                    AzureHubUtils.SendNotificationAsync("Se han acreditado "+ transaccion.transaccion.Monto +"$", tags);

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

        #region Debitar Saldo
        [HttpPost]
        [Route("DebitarSaldo")]
        public async Task<IHttpActionResult> DebitarSaldo(Transaccion transaccion)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    Double monto = (double) ((transaccion.Monto / 30) * 0.50);
                    transaccion.Monto = monto;

                    var DebitarSaldo = db.Saldo.Where(s => s.UsuarioId == transaccion.UsuarioId).FirstOrDefault();

                    if (DebitarSaldo == null)
                    {
                        return BadRequest("No tiene Saldo Disponible");
                    }

                    DebitarSaldo.Saldo1 = DebitarSaldo.Saldo1 - monto;
                    db.Entry(DebitarSaldo).State = EntityState.Modified;
                    db.SaveChangesAsync().Wait();                  
                    //______________

                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }
                    db.Transaccion.Add(transaccion);
                     db.SaveChangesAsync().Wait();

                    //_______________
                    List<string> tags = new List<string>();

                    var PlazaDB = db.Dispositivo.Where(x => x.UsuarioId == transaccion.UsuarioId).ToList();
                    foreach (Dispositivo element in PlazaDB)
                    {
                        tags.Add(element.UniqueId);
                    }
                    AzureHubUtils.SendNotificationAsync("Se han debitado " + monto + "$  por la compra de tiempo de parqueo", tags);
                    transaction.Commit();

                    return Ok(DebitarSaldo);
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return BadRequest("Error");
                }
            }
        }

        #endregion


        //La Transaccion para Parquear esta en ParqueController


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