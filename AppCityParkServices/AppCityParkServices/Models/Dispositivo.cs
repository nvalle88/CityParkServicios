
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
    
public partial class Dispositivo
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public Dispositivo()
    {

        this.Codigo = new HashSet<Codigo>();

    }


    public int DispositivoId { get; set; }

    public Nullable<int> SOId { get; set; }

    public Nullable<int> UsuarioId { get; set; }

    public string UniqueId { get; set; }

    public Nullable<int> EmpresaId { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Codigo> Codigo { get; set; }

    public virtual Empresa Empresa { get; set; }

    public virtual SO SO { get; set; }

}

}