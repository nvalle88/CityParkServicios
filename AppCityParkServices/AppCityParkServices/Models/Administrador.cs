//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AppCityParkServices.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Administrador
    {
        public int AdministradorId { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Contrasela { get; set; }
        public int EmpresaId { get; set; }
    
        public virtual Empresa Empresa { get; set; }
    }
}
