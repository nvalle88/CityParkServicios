
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
    
public partial class Codigo
{

    public int CodigoId { get; set; }

    public string Codigo1 { get; set; }

    public int UsuarioId { get; set; }

    public int DispositivoId { get; set; }

    public bool Usado { get; set; }



    public virtual Usuario Usuario { get; set; }

    public virtual Dispositivo Dispositivo { get; set; }

}

}