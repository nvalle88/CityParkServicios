using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppCityParkServices.Clases
{
    public class MultaRequest
    {
        public int MultaId { get; set; }
        public int SalarioBasicoId { get; set; }    
        public int AgenteId { get; set; }
        public int EmpresaId { get; set; }
        public int TipoMultaId { get; set; }
    }
}