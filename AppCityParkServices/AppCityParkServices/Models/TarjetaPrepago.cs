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
    
    public partial class TarjetaPrepago
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TarjetaPrepago()
        {
            this.UsuarioTarjetaPrepago = new HashSet<UsuarioTarjetaPrepago>();
        }
    
        public int TarjetaPrepagoId { get; set; }
        public string Numero { get; set; }
        public decimal Saldo { get; set; }
        public System.DateTime FechaVence { get; set; }
        public bool Activa { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UsuarioTarjetaPrepago> UsuarioTarjetaPrepago { get; set; }
    }
}
