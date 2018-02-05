using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppCityParkServices.Clases
{
    public class PlazaRequest
    {
        public int PlazaId { get; set; }

        public string Nombre { get; set; }

        public Nullable<double> Longitud { get; set; }

        public Nullable<double> Latitud { get; set; }

        public Nullable<int> EmpresaId { get; set; }

        public Nullable<bool> Ocupado { get; set; }


    }
}