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
    [RoutePrefix("api/TiposMultas")]
    public class TiposMultasController : ApiController
    {
        private CityParkApp db = new CityParkApp();

        // GET: api/SalarioBasicoes
        [HttpPost]
        [Route("GetTipoMulta")]
        public async Task<Response> GetTipoMulta(TipoMultas tipoMultas)
        {
            db.Configuration.ProxyCreationEnabled = false;
            try
            {
                if (tipoMultas == null)
                {
                    return new Response { IsSuccess = false };
                }
                var d = await db.TipoMultas.ToListAsync();
                var Request = await db.TipoMultas.Where(x => x.TipoMultaId == tipoMultas.TipoMultaId).FirstOrDefaultAsync();
                if (Request == null)
                {
                    return new Response { IsSuccess = false };
                }
                return new Response { IsSuccess = true, Result = Request };
            }
            catch (Exception)
            {
                return new Response { IsSuccess = false };
            }
        }

        // GET: api/SalarioBasicoes/5
        [HttpPost]
        [Route("GetTiposMultasPorEmpresa")]
        public async Task<List<TipoMultas>> GetSalariosBasicosPorEmpresa([FromBody] Empresa empresa)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                if (empresa.EmpresaId <= 0)
                {
                    return null;
                }

                var lista = await db.TipoMultas.Where(x => x.EmpresaId == empresa.EmpresaId).ToListAsync();
                if (lista == null)
                {
                    return new List<TipoMultas>();
                }

                return lista;
            }
            catch (Exception)
            {
                return new List<TipoMultas>();
            }
        }

        // PUT: api/SalarioBasicoes/5
        [HttpPost]
        [Route("EditTipoMulta")]
        public async Task<Response> EditTipoMulta([FromBody] TipoMultas tipoMultas)
        {
            try
            {
                if (tipoMultas == null)
                {
                    return new Response { IsSuccess = false };
                }
                var Request = await db.TipoMultas.Where(x => x.TipoMultaId == tipoMultas.TipoMultaId).FirstOrDefaultAsync();
                if (Request == null)
                {
                    return new Response { IsSuccess = false };
                }
                Request.Descripcion = tipoMultas.Descripcion;
                Request.Porcentaje = tipoMultas.Porcentaje;
                Request.Multa = tipoMultas.Multa;

                db.Entry(Request).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return new Response { IsSuccess = true };
            }
            catch (Exception)
            {
                return new Response { IsSuccess = false };
            }
        }

        // POST: api/SalarioBasicoes
        [HttpPost]
        [Route("CreateTipoMulta")]
        public async Task<Response> CreateTipoMulta([FromBody]TipoMultas tipoMultas)
        {
            try
            {
                if (tipoMultas == null)
                {
                    return new Response { IsSuccess = false };
                }
                db.TipoMultas.Add(tipoMultas);
                await db.SaveChangesAsync();

                return new Response { IsSuccess = true };
            }
            catch (Exception)
            {
                return new Response { IsSuccess = false };
            }
        }

        [HttpPost]
        [Route("DeleteTipoMulta")]
        public async Task<Response> DeleteTipoMulta([FromBody] TipoMultas tipoMultas)
        {
            try
            {
                var Request = await db.TipoMultas.Where(x => x.TipoMultaId == tipoMultas.TipoMultaId).FirstOrDefaultAsync();
                if (Request == null)
                {
                    return new Response { IsSuccess = false };
                }

                db.TipoMultas.Remove(Request);
                await db.SaveChangesAsync();

                return new Response { IsSuccess = true };
            }
            catch (Exception)
            {
                return new Response { IsSuccess = false };
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SalarioBasicoExists(int id)
        {
            return db.SalarioBasico.Count(e => e.SalarioBasicoId == id) > 0;
        }
    }
}