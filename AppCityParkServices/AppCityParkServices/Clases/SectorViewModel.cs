using AppCityParkServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppCityParkServices.Clases
{
    public class SectorViewModel
    {
        public Sector Sector { get; set; }
        public List<PuntoSector> PuntoSector { get; set; }
    }
}