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
    
    public partial class Agente
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Agente()
        {
            this.Multa = new HashSet<Multa>();
        }
    
        public int AgenteId { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Contrasena { get; set; }
        public Nullable<int> EmpresaId { get; set; }
        public Nullable<int> SectorId { get; set; }
    
        public virtual Empresa Empresa { get; set; }
        public virtual Sector Sector { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Multa> Multa { get; set; }
    }
}
