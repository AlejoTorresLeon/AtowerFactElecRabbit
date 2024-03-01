﻿using AtowerDocElectronico.Infraestructura.Entities;
using AtowerDocElectronico.Infraestructura.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace AtowerDocElectronico.Infraestructura.Persistencia.Postgrest
{
    public partial class AtowerDbContext : DbContext, IAtowerDbContext
    {
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<Compañia> Compañias { get; set; }
        public DbSet<Facturas> Facturas { get; set; }

        public AtowerDbContext(DbContextOptions<AtowerDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<Usuarios>().ToTable("Usuarios", schema: "Configuracion");
            modelBuilder.Entity<Roles>().ToTable("Roles", schema: "Configuracion");
            modelBuilder.Entity<Compañia>().ToTable("Compañia", schema: "Configuracion");
            modelBuilder.Entity<Facturas>().ToTable("Facturas", schema: "Envios");

            //modelBuilder.Entity<JObject>().HasNoKey();
            //modelBuilder.Entity<JToken>().HasNoKey();
            modelBuilder.Entity<Facturas>()
            .HasKey(f => f.Id); // Definiendo la clave primaria

            modelBuilder.Entity<Facturas>()
            .Property(u => u.Id)
            .ValueGeneratedOnAdd(); 

            modelBuilder.Entity<Usuarios>()
              .Property(u => u.Id)
              .ValueGeneratedOnAdd();

            modelBuilder.Entity<Roles>()
                .Property(r => r.Id)
                .ValueGeneratedOnAdd(); 

            modelBuilder.Entity<Compañia>()
                .Property(c => c.Id)
                .ValueGeneratedOnAdd();


            modelBuilder.Entity<Usuarios>()
                .HasOne(u => u.Roles)
                .WithMany()
                .HasForeignKey(u => u.IdRol);

            modelBuilder.Entity<Compañia>()
                .HasOne(c => c.Usuarios)
                .WithMany()
                .HasForeignKey(c => c.IdUsuarioCreador)
                .HasForeignKey(c => c.IdUsuarioCliente);

            modelBuilder.Entity<Facturas>()
             .HasOne(c => c.Compañia)
             .WithMany()
             .HasForeignKey(c => c.IdCompany);         


            modelBuilder.Seed();
        }


        public new int SaveChanges()
        {
            return base.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }
    }
}
