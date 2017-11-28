//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AppCityParkServices.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Parqueo
    {
        public int ParqueoId { get; set; }
        public Nullable<int> UsuarioId { get; set; }
        public Nullable<System.DateTime> FechaInicio { get; set; }
        public Nullable<System.DateTime> FechaFin { get; set; }
        public Nullable<double> Latitud { get; set; }
        public Nullable<double> Longitud { get; set; }
        public Nullable<int> TarjetaCreditoId { get; set; }
        public Nullable<int> CarroId { get; set; }
        public Nullable<int> PlazaId { get; set; }
        public Nullable<int> EmpresaId { get; set; }
    
        public virtual Carro Carro { get; set; }
        public virtual Empresa Empresa { get; set; }
        public virtual TarjetaCredito TarjetaCredito { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
