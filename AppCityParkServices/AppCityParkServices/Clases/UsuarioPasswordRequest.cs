using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppCityParkServices.Clases
{
    public class UsuarioPasswordRequest
    {
        public int UsuarioId { get; set; }

        public string Contrasena { get; set; }

        public string ContrasenaNueva { get; set; }

        public string Correo { get; set; }
    }
}