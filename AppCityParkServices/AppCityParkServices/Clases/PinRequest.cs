using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppCityParkServices.Clases
{
    public class PinRequest
    {
        public string placa { get; set; }
        public DateTime HoraFin { get; set; }
        public double? Latitud { get; set; }
        public double? Longitud { get; set; }
    }
}