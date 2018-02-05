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
using AppCityParkServices.Utils;
using AppCityParkServices.Constants;
using System.Collections.ObjectModel;

namespace AppCityParkServices.Controllers.Api
{
    [RoutePrefix("api/Parqueos")]
    public class ParqueosController : ApiController
    {
        private CityParkApp db = new CityParkApp();
        // GET: api/Parqueos/GetParqueados
        [HttpPost]
        [Route("GetParqueados")]
        public List<PinRequest> GetParqueados(Agente agente_)
        {
            var _agente = db.Agente.Where(x => x.AgenteId == agente_.AgenteId).FirstOrDefault();


            List<Parqueo> ParqueoDB = db.Parqueo.Where(x => x.FechaFin >= DateTime.Now).ToList();

            var _sector = db.Sector.Where(x => x.SectorId == _agente.SectorId).FirstOrDefault();
            List<PuntoSector> _PolygonSector = new List<PuntoSector>();
            _PolygonSector = db.PuntoSector.Where(x => x.SectorId == _sector.SectorId).ToList();
            ObservableCollection<Position> PoligonoAgente = new ObservableCollection<Position>();
             foreach ( var a in _PolygonSector)
            {
                PoligonoAgente.Add(new Position { latitude = (double)a.Latitud, longitude = (double)a.Longitud });
            }
            var Parqueados = new List<PinRequest>();
           
            foreach (var parqueos in ParqueoDB)
            {
                if (parqueos.FechaFin.ToLocalTime() >= DateTime.Now.ToLocalTime())
                {
                    if (GeoUtils.EstaEnMiSector(PoligonoAgente,parqueos ))
                        Parqueados.Add(new PinRequest
                    {
                        placa = parqueos.Carro.Placa,
                        HoraFin = parqueos.FechaFin,
                        Latitud = parqueos.Latitud,
                        Longitud = parqueos.Longitud,
                    });
                }
            }
            return Parqueados;
        }

        [HttpPost]
        [Route("GetParqueos")]
        public Parqueo GetParqueo(UsuarioRequest usuario)
        {
            List<Parqueo> ParqueoDB = db.Parqueo.Where(x => x.UsuarioId == usuario.UsuarioId).ToList();
            Parqueo parqueo = null;

            parqueo= ParqueoDB.OrderByDescending(x => x.ParqueoId).First();

            return parqueo;           
        }

        [HttpPost]
        [Route("GetTiempo")]
        public TiempoRequest GetTiempo(UsuarioRequest usuario)
        {
            List<Parqueo> ParqueoDB = db.Parqueo.Where(x => x.UsuarioId == usuario.UsuarioId).ToList();
            Parqueo parqueo = null;

            parqueo = ParqueoDB.OrderByDescending(x => x.ParqueoId).First();
            var tiempoComprado = parqueo.FechaFin.TimeOfDay - parqueo.FechaInicio.TimeOfDay;
            Debug.WriteLine(tiempoComprado);
            var tiempoRestante = parqueo.FechaFin.TimeOfDay - DateTime.UtcNow.TimeOfDay;
            if (parqueo.FechaFin >= DateTime.Now.Date)
            {
                Debug.WriteLine(tiempoRestante);
                var tiempos = new TiempoRequest
                {
                    Comprado = tiempoComprado,
                    Restante = tiempoRestante,
                };

                return tiempos;
            }
            return null;
        }

        [HttpPost]
        [Route("BuscarPlaca")]
        public IHttpActionResult BuscarPlaca(JObject form)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var placa = string.Empty;
            var plazaNombre = string.Empty;
            var AgenteId = 0;

            dynamic jsonObject = form;

        try
        {
                placa = (jsonObject.Placa.Value).Replace("-", "").ToUpper();
                plazaNombre = jsonObject.Plaza.Value;
                AgenteId = (int)jsonObject.AgenteId.Value;
            



            Response response;

            var Plazas = db.Plaza.Where(x => x.Nombre == plazaNombre && x.Ocupado == true).ToList();
            var Agente = db.Agente.Where(x => x.AgenteId == AgenteId).FirstOrDefault();
            if (Plazas == null)
            {
                response = new Response
                {
                    IsSuccess = true,
                    Message = "La plaza se encuentra desocupada o no existe",
                    Result = null,
                };
                return Ok(response);
            }
            Plaza Plaza = new Plaza();
            foreach (var ifplaza in Plazas)
            { if (ifplaza.EmpresaId == Agente.EmpresaId)
                {
                    Plaza = ifplaza;
                }
            }

            var parqueos = db.Parqueo.Where(s => s.Carro.Placa == placa && s.FechaFin.Day == DateTime.Now.Day
                                             && s.FechaFin.Month == DateTime.Now.Month
                                             && s.FechaFin.Year == DateTime.Now.Year
                                             && s.PlazaId == Plaza.PlazaId)
                                             .Include(s => s.Carro.Modelo.Marca)
                                             .Include(s => s.Usuario)
                                             .ToList();
            if (parqueos.Count() == 0)
            {
                response = new Response
                {
                    IsSuccess = false,
                    Message = "El vehículo no cuenta con tiempo de parqueo...",
                    Result = null,
                };
                return Ok(response);
            }

            var pa = parqueos.Where(p => (p.FechaFin.TimeOfDay > DateTime.Now.TimeOfDay) && p.FechaInicio.TimeOfDay < DateTime.Now.TimeOfDay).FirstOrDefault();



            if (pa == null)
            {
                response = new Response
                {
                    IsSuccess = false,
                    Message = "El vehículo no cuenta con tiempo de parqueo...",
                    Result = null,
                };
                return Ok(response);
            }


            return BadRequest("El vehículo cuenta con tiempo de parqueo..."); ;

        }
             catch (Exception)
            {
                return null;
            }

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

        // POST: api/Parqueos/InsertarParqueoHelper
        [HttpPost]
        [Route("InsertarParqueoHelper")]
        [ResponseType(typeof(ParqueoHelper))]
        public async Task<IHttpActionResult> InsertarParqueoHelper(ParqueoHelper parqueoHelper)
        {
            db.Configuration.ProxyCreationEnabled = false;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }          
                // verificamos si la Marca existe sino la agregamos
                #region marca
                Marca MarcaDB = db.Marca.Where(x => x.Nombre == parqueoHelper.Marca).FirstOrDefault();
                if (MarcaDB == null)
                {
                    Marca marca = new Marca { Nombre = parqueoHelper.Marca };
                    db.Marca.Add(marca);
                    await db.SaveChangesAsync();
                    MarcaDB = db.Marca.Where(x => x.Nombre == parqueoHelper.Marca).FirstOrDefault();
                }
                #endregion
                // verificamos si el Modelo existe sino lo agregamos
                #region Modelo
                Modelo ModeloDB = db.Modelo.Where(x => x.Nombre == parqueoHelper.Modelo).FirstOrDefault();
                if (ModeloDB == null)
                {
                    Modelo modelo = new Modelo
                    {
                        Nombre = parqueoHelper.Modelo,
                        MarcaId = MarcaDB.MarcaId
                    };
                    db.Modelo.Add(modelo);
                    await db.SaveChangesAsync();
                    ModeloDB = db.Modelo.Where(x => x.Nombre == parqueoHelper.Modelo).FirstOrDefault();
                }
                #endregion
                // verificamos si El carro existe sino lo agregamos
                #region Carro
                Carro CarroDB = db.Carro.Where(x => x.UsuarioId == parqueoHelper.UsuarioId && x.Placa == parqueoHelper.Placa).FirstOrDefault();
                if (CarroDB == null)
                {
                    Carro Carro = new Carro
                    {
                        ModeloId = ModeloDB.ModeloId,
                        Placa = parqueoHelper.Placa,
                        UsuarioId = parqueoHelper.UsuarioId,
                        Color = parqueoHelper.Color
                    };
                    db.Carro.Add(Carro);
                    await db.SaveChangesAsync();
                    CarroDB = db.Carro.Where(x => x.UsuarioId == parqueoHelper.UsuarioId && x.Placa == parqueoHelper.Placa).FirstOrDefault();
                }
            #endregion
            // extraemos las coordenadas de la plaza para utilizarla en el parqueo
                #region Plaza
            Plaza plaza = db.Plaza.Where(x => x.PlazaId == parqueoHelper.PlazaId).FirstOrDefault();

            #endregion

            //Ingresamos el parqueo
            var parqueo = new Parqueo
                {
                    FechaInicio = parqueoHelper.FechaInicio,
                    FechaFin = parqueoHelper.FechaFin,
                    CarroId = CarroDB.CarroId,
                    Latitud = plaza.Latitud,
                    Longitud = plaza.Longitud,
                    UsuarioId = parqueoHelper.UsuarioId,
                    PlazaId = parqueoHelper.PlazaId,
                };

                db.Parqueo.Add(parqueo);
                await db.SaveChangesAsync();
                var carro = db.Carro.Where(c => c.CarroId == parqueo.CarroId).Include(c => c.Modelo.Marca).FirstOrDefault();
                parqueo.Carro = carro;
                parqueo.CarroId = carro.CarroId;
                if (parqueoHelper.VendedorId > 0)
                {
                    var transaccion = new Transaccion
                    {
                        VendedorId = parqueoHelper.VendedorId,
                        Monto = parqueoHelper.Monto,
                        Fecha = parqueoHelper.Fecha,
                        UsuarioId = parqueoHelper.UsuarioId,
                    };
                    db.Transaccion.Add(transaccion);
                    await db.SaveChangesAsync();
                }
            else
                {
                //si no es un vendedor el que parquea, se procede a realizar el debito del saldo                                           
                }
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