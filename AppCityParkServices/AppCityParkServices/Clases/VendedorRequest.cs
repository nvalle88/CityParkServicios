using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppCityParkServices.Clases
{
    public class VendedorRequest
    {
        public int VendedorId { get; set; }

        public string Nombre { get; set; }

        public string Apellido { get; set; }

        public string Contrasena { get; set; }
    }   

}