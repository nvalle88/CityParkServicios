using AppCityParkServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppCityParkServices.Clases
{
    public class UsuarioLoginRequest
    {
        
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
            public UsuarioLoginRequest()
            {
                this.Carro = new HashSet<Carro>();
                this.Codigo = new HashSet<Codigo>();
                this.Codigo1 = new HashSet<Codigo>();
                this.Dispositivo = new HashSet<Dispositivo>();
                this.Parqueo = new HashSet<Parqueo>();
                this.Saldo = new HashSet<Saldo>();
                this.TarjetaCredito = new HashSet<TarjetaCredito>();
                this.Transaccion = new HashSet<Transaccion>();
                this.UsuarioTarjetaPrepago = new HashSet<UsuarioTarjetaPrepago>();
            }

            public int UsuarioId { get; set; }
            public string Nombre { get; set; }
            public string Contrasena { get; set; }
            public string Correo { get; set; }
            public double Saldo1 { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
            public virtual ICollection<Carro> Carro { get; set; }
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
            public virtual ICollection<Codigo> Codigo { get; set; }
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
            public virtual ICollection<Codigo> Codigo1 { get; set; }
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
            public virtual ICollection<Dispositivo> Dispositivo { get; set; }
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
            public virtual ICollection<Parqueo> Parqueo { get; set; }
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
            public virtual ICollection<Saldo> Saldo { get; set; }
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
            public virtual ICollection<TarjetaCredito> TarjetaCredito { get; set; }
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
            public virtual ICollection<Transaccion> Transaccion { get; set; }
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
            public virtual ICollection<UsuarioTarjetaPrepago> UsuarioTarjetaPrepago { get; set; }
        
   

    }
}