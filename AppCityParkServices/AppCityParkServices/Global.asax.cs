using AppCityParkServices.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace AppCityParkServices
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private CityParkApp db;
 
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            db = new CityParkApp();

            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 60000;
            timer.Elapsed += timer_Elapsed;
            timer.Start();

        }

        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //Models.Proposal_DBEntities DB = new Models.Proposal_DBEntities();
            //Models.tbl_Year Y = new Models.tbl_Year();
            //Y.YearName = "13" + DateTime.Now.Second.ToString();
            //DB.tbl_Year.Add(Y);
            //DB.SaveChanges();

            //Para obtener los vehiculos parqueados

            Debug.WriteLine(" método para controlar los parqueos");

            List<Parqueo> ParqueoDB = db.Parqueo.Where(x => x.FechaFin >= DateTime.Now).ToList();
            var Parqueados = new List<Parqueo>();            
            foreach (var parqueo in ParqueoDB)
            {
                if (parqueo.FechaFin.ToLocalTime() >= DateTime.Now.ToLocalTime())
                {
                    Parqueados.Add(parqueo);
                }
            }

            List<Plaza> PlazaDB = db.Plaza.Where(x => x.Ocupado == true).ToList();
            var Plazas = new List<Plaza>();
            foreach (var plaza in PlazaDB)
            {
                var ocupado = false;
                int Id = plaza.PlazaId;

                foreach (var parqueado in Parqueados)
                {
                    if (plaza.PlazaId==parqueado.PlazaId)
                    {
                        // si la plaza se encuentra ocupada y consta en algún parqueo, se deja de seguir revisando 
                        ocupado = true;
                        break;
                    }
                    
                }

                if (!ocupado)
                {
                    //var _plaza = db.Plaza.Where(x => x.PlazaId == Id ).FirstOrDefault();
                    //_plaza.Ocupado = ocupado;


                    Debug.WriteLine(" Cambiando....");

                    using (CityParkApp context = new CityParkApp())
                    {
                        // "id" is the id in your table (parameter passed)
                        Plaza _plaza = context.Plaza.Where(x => x.PlazaId==Id).FirstOrDefault();   // Find may be only for Entity Framework. If you can't use it, use the line below
                                                                           // Person person = context.Person.Where(x => x.ColumnId == id).First();
                        _plaza.Ocupado = false;
                        context.SaveChanges();
                    }

                    //Se actualiza la plaza si no esta ocupada

                }

            }





        }


      




    }
}
