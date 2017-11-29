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
    
    public partial class Transaccion
    {
        public int TransaccionId { get; set; }
        public Nullable<int> UsuarioId { get; set; }
        public Nullable<int> VendedorId { get; set; }
        public Nullable<double> Monto { get; set; }
        public System.DateTime Fecha { get; set; }
        public Nullable<int> EmpresaId { get; set; }
    
        public virtual Empresa Empresa { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual Vendedor Vendedor { get; set; }
    }
}
