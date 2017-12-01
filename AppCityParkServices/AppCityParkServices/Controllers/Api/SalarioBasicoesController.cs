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
    [RoutePrefix("api/SalariosBasicos")]
    public class SalarioBasicoesController : ApiController
    {
        private CityParkApp db = new CityParkApp();

        // GET: api/SalarioBasicoes
        [HttpPost]
        [Route("GetSalarioBasico")]
        public async Task<Response> GetSalarioBasico(SalarioBasico salarioBasico)
        {
            db.Configuration.ProxyCreationEnabled = false;
            try
            {
                if (salarioBasico == null)
                {
                    return new Response { IsSuccess = false };
                }
                var salarioRequest = await db.SalarioBasico.Where(x => x.SalarioBasicoId == salarioBasico.SalarioBasicoId).FirstOrDefaultAsync();
                if (salarioRequest == null)
                {
                    return new Response { IsSuccess = false };
                }
                return new Response { IsSuccess = true, Result = salarioRequest };
            }
            catch (Exception)
            {
                return new Response { IsSuccess = false };
            }
        }

        // GET: api/SalarioBasicoes/5
        [HttpPost]
        [Route("GetSalariosBasicosPorEmpresa")]
        public async Task<List<SalarioBasico>> GetSalariosBasicosPorEmpresa([FromBody] Empresa empresa)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                if (empresa.EmpresaId <= 0)
                {
                    return null;
                }

                var listaSalariosBasicos = await db.SalarioBasico.Where(x => x.EmpresaId == empresa.EmpresaId).Include(x => x.Empresa).ToListAsync();
                if (listaSalariosBasicos == null)
                {
                    return new List<SalarioBasico>();
                }

                return listaSalariosBasicos;
            }
            catch (Exception)
            {
                return new List<SalarioBasico>();
            }
        }

        // PUT: api/SalarioBasicoes/5
        [HttpPost]
        [Route("EditSalarioBasico")]
        public async Task<Response> EditSalarioBasico([FromBody] SalarioBasico salarioBasico)
        {
            try
            {
                if (salarioBasico==null)
                {
                    return new Response { IsSuccess=false };
                }
                var salarioRequest =await db.SalarioBasico.Where(x => x.SalarioBasicoId == salarioBasico.SalarioBasicoId).FirstOrDefaultAsync();
                if (salarioRequest==null)
                {
                    return new Response { IsSuccess = false };
                }
                salarioRequest.Descripcion = salarioBasico.Descripcion;
                salarioRequest.Fecha = salarioBasico.Fecha;
                salarioRequest.Monto = salarioBasico.Monto;

                db.Entry(salarioRequest).State = EntityState.Modified;
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
        [Route("CreateSalarioBasico")]
        public async Task<Response> CreateSalarioBasico([FromBody]SalarioBasico salarioBasico)
        {
            try
            {
                if (salarioBasico == null)
                {
                    return new Response { IsSuccess = false };
                }
                db.SalarioBasico.Add(salarioBasico);
                await db.SaveChangesAsync();

                return new Response { IsSuccess = true };
            }
            catch (Exception)
            {
                return new Response { IsSuccess = false };
            }
        }

        [HttpPost]
        [Route("DeleteSalarioBasico")]
        public async Task<Response> DeleteSalarioBasico([FromBody] SalarioBasico salarioBasico)
        {
            try
            {
                var salarioRequest = await db.SalarioBasico.Where(x => x.SalarioBasicoId == salarioBasico.SalarioBasicoId).FirstOrDefaultAsync();
                if (salarioRequest == null)
                {
                    return new Response { IsSuccess = false };
                }

                db.SalarioBasico.Remove(salarioRequest);
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