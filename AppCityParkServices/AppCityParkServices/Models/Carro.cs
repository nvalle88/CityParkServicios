
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
    
public partial class Carro
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public Carro()
    {

        this.Parqueo = new HashSet<Parqueo>();

    }


    public int CarroId { get; set; }

    public int ModeloId { get; set; }

    public int UsuarioId { get; set; }

    public string Placa { get; set; }

    public string Color { get; set; }



    public virtual Modelo Modelo { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Parqueo> Parqueo { get; set; }

    public virtual Usuario Usuario { get; set; }

}

}
