
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
    
public partial class Multa
{

    public int MultaId { get; set; }

    public string Foto { get; set; }

    public Nullable<decimal> Valor { get; set; }

    public DateTime Fecha { get; set; }

    public Nullable<int> AgenteId { get; set; }

    public Nullable<double> Longitud { get; set; }

    public Nullable<double> latitud { get; set; }

    public string Placa { get; set; }

    public string Plaza { get; set; }



    public virtual Agente Agente { get; set; }

}

}
