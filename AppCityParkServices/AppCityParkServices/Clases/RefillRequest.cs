using AppCityParkServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppCityParkServices.Clases
{
    public class RefillRequest
    {
        public Transaccion transaccion { get; set; }
        public Codigo codigo { get; set; }
    }
}