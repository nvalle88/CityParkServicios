using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppCityParkServices.Clases
{
    public class ParqueoHelper
    {
        public float Monto { get; set; }
        public int VendedorId { get; set; }
        public int UsuarioId { get; set; }
        public DateTime Fecha { get; set; }

        public string Placa {get; set;}
        public string Marca {get; set;}
        public string Color {get; set;}
        public string Modelo {get; set;}
           
        public DateTime FechaInicio {get;set;}
        public DateTime FechaFin {get; set;}
        public Nullable<double> Latitud { get; set;}
        public Nullable<double> Longitud { get; set;}
        public int? PlazaId { get; set; }

    }
}