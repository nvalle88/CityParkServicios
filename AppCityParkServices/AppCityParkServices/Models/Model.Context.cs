﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class CityParkApp : DbContext
    {
        public CityParkApp()
            : base("name=CityParkApp")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Administrador> Administrador { get; set; }
        public virtual DbSet<Carro> Carro { get; set; }
        public virtual DbSet<Codigo> Codigo { get; set; }
        public virtual DbSet<Dispositivo> Dispositivo { get; set; }
        public virtual DbSet<Empresa> Empresa { get; set; }
        public virtual DbSet<Marca> Marca { get; set; }
        public virtual DbSet<Modelo> Modelo { get; set; }
        public virtual DbSet<Multa> Multa { get; set; }
        public virtual DbSet<Parqueo> Parqueo { get; set; }
        public virtual DbSet<Plaza> Plaza { get; set; }
        public virtual DbSet<Saldo> Saldo { get; set; }
        public virtual DbSet<SO> SO { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<TarjetaCredito> TarjetaCredito { get; set; }
        public virtual DbSet<TarjetaPrepago> TarjetaPrepago { get; set; }
        public virtual DbSet<TipoMultas> TipoMultas { get; set; }
        public virtual DbSet<Transaccion> Transaccion { get; set; }
        public virtual DbSet<UsuarioTarjetaPrepago> UsuarioTarjetaPrepago { get; set; }
        public virtual DbSet<Vendedor> Vendedor { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<Agente> Agente { get; set; }
        public virtual DbSet<PuntoSector> PuntoSector { get; set; }
        public virtual DbSet<Sector> Sector { get; set; }
    }
}
