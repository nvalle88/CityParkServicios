
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
    
public partial class Vendedor
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public Vendedor()
    {

        this.Transaccion = new HashSet<Transaccion>();

    }


    public int VendedorId { get; set; }

    public string Nombre { get; set; }

    public string Apellido { get; set; }

    public string Contrasena { get; set; }

    public int EmpresaId { get; set; }



    public virtual Empresa Empresa { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Transaccion> Transaccion { get; set; }

}

}
