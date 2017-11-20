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
using System.Diagnostics;

namespace AppCityParkServices.Controllers.Api
{
    [RoutePrefix("api/SOes")]

    public class SOesController : ApiController
    {
        private CityParkApp db = new CityParkApp();


        /*
         Este metodo nos permite verificar si existe el sistema operativo y la version del mismo, al no existir se guardara en nuestra base de datos*/
        //Post api/SOes/SOCHeck
        [HttpPost]
        [Route("SOCheck")]
        public async Task<Response>SOCheck(JObject form)
        {
            //Falta ingresar el dispositivo, y transformar la ciudad en empresa
            db.Configuration.ProxyCreationEnabled = false;
            var SO = string.Empty;
            var Version = string.Empty;
            var UniqueId = string.Empty;
            var ciudad = string.Empty;
            int CiudadId = 0;
            var UsuarioId = 0;


            dynamic jsonObject = form;
            try
            {
               var User = jsonObject.UsuarioId.Value;
                SO = jsonObject.OS.Value;
                Version = jsonObject.Version.Value;
                UniqueId = jsonObject.UniqueId.Value;
                ciudad = jsonObject.Ciudad.Value;
                UsuarioId = (int)User;

                switch (ciudad)
                {
                    case "Cuenca":
                        CiudadId = 1;
                        Debug.WriteLine("EMOV");
                        break;

                    case "Quito":
                        Debug.WriteLine("Municipio Rumiñahui");
                        CiudadId = 2;
                        break;
                }

               

            }
            catch (Exception)
            {

                return
                    new Response {
                        IsSuccess = true,
                        Message="Los objetos estan corruptos",                     
                                                
                    };
            }

            var existeSOes = db.SO.
                                Where(u => u.Nombre == SO).ToList();                                

            if (existeSOes != null)
            {
                bool exist = false;
                SO existeSO = new SO();
                foreach (SO existeSOs in existeSOes)
                {
                    if ((existeSOs.Version.Replace(" ", "")) == (Version.Replace(" ", "")))
                    {
                        exist = true;
                        existeSO = existeSOs;
                    }
                }

                if (exist)
                {
                    Dispositivo device = new Dispositivo
                    {
                        SOId = existeSO.SOId,
                        UsuarioId = UsuarioId,
                        EmpresaId = CiudadId,
                        UniqueId = UniqueId,
                    };
                   await DeviceCheck(device);
                    return new Response
                    {
                        IsSuccess = true,
                        Message = "El sistema y la version ya existen",
                        Result = existeSO,
                    };
                }
                else
                {
                    //añadir la version
                    var _SO = new SO
                    {
                        Version = Version,
                        Nombre = SO,
                    };
                    db.SO.Add(_SO);
                    await db.SaveChangesAsync();
                    CreatedAtRoute("DefaultApi", new { id = _SO.SOId }, _SO);
                    Dispositivo device = new Dispositivo
                    {
                        SOId=_SO.SOId,
                        UsuarioId= UsuarioId,
                        EmpresaId=CiudadId,
                        UniqueId=UniqueId,
                    };
                   await DeviceCheck(device);

                    return
                    new Response
                    {
                        IsSuccess = true,
                        Message = "Nueva version al Sistema",
                        Result=_SO,
                    };
                }
            }
            else
            {
                var _SO = new SO
                {
                    Version = Version,
                    Nombre = SO,
                };
                db.SO.Add(_SO);
                await db.SaveChangesAsync();
                CreatedAtRoute("DefaultApi", new { id = _SO.SOId }, _SO);
                Dispositivo device = new Dispositivo
                {
                    SOId = _SO.SOId,
                    UsuarioId = UsuarioId,
                    EmpresaId = CiudadId,
                    UniqueId = UniqueId,
                };
                await DeviceCheck(device);
                return
                new Response
                {
                    IsSuccess = true,
                    Message = "Se Agrego un Nuevo SO",
                    Result=_SO,
                };

            }




           return new Response
            {
                IsSuccess = true,
                Message = "El sistema y la version ya existen",
            };

        }


        public async Task DeviceCheck(Dispositivo device)
        {
            //Buscamos el Dispositivo en la base de datos
            var dispositivo = db.Dispositivo.
                               Where(u => u.UniqueId == device.UniqueId)
                               .FirstOrDefault();

            //Si el dispositivo no existe lo agregamos
            if (dispositivo == null)
            {
                db.Dispositivo.Add(device);
                await db.SaveChangesAsync();
                CreatedAtRoute("DefaultApi", new { id = device.DispositivoId }, device);
            }
            //si el dispositivo existe debemos actualizarlo por si cambio de usuario o de empresa
            else {
                dispositivo.EmpresaId = device.EmpresaId;
                dispositivo.UsuarioId = device.UsuarioId;
                db.Entry(dispositivo).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }


        }



        // GET: api/SOes
        public IQueryable<SO> GetSO()
        {
            return db.SO;
        }

        // GET: api/SOes/5
        [ResponseType(typeof(SO))]
        public async Task<IHttpActionResult> GetSO(int id)
        {
            SO sO = await db.SO.FindAsync(id);
            if (sO == null)
            {
                return NotFound();
            }

            return Ok(sO);
        }

        // PUT: api/SOes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSO(int id, SO sO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != sO.SOId)
            {
                return BadRequest();
            }

            db.Entry(sO).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SOExists(id))
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

        //// POST: api/SOes
        [ResponseType(typeof(SO))]
        public async Task<IHttpActionResult> PostSO(SO sO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SO.Add(sO);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = sO.SOId }, sO);
        }


        // DELETE: api/SOes/5
        [ResponseType(typeof(SO))]
        public async Task<IHttpActionResult> DeleteSO(int id)
        {
            SO sO = await db.SO.FindAsync(id);
            if (sO == null)
            {
                return NotFound();
            }

            db.SO.Remove(sO);
            await db.SaveChangesAsync();

            return Ok(sO);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SOExists(int id)
        {
            return db.SO.Count(e => e.SOId == id) > 0;

        }
    }
}