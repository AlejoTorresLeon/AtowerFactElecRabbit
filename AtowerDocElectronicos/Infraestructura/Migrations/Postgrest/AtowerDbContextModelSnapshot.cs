﻿// <auto-generated />
using System;
using AtowerDocElectronico.Infraestructura.Persistencia.Postgrest;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AtowerDocElectronico.Infraestructura.Migrations.Postgrest
{
    [DbContext(typeof(AtowerDbContext))]
    partial class AtowerDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AtowerDocElectronico.Infraestructura.Entities.Compañia", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("Bloqueo")
                        .HasColumnType("boolean");

                    b.Property<string>("DigitoVerificacion")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("Habilitado")
                        .HasColumnType("boolean");

                    b.Property<decimal>("IdCompanyNubex")
                        .HasColumnType("numeric(20,0)");

                    b.Property<int>("IdUsuarioCliente")
                        .HasColumnType("integer");

                    b.Property<int>("IdUsuarioCreador")
                        .HasColumnType("integer");

                    b.Property<decimal>("IdUsuarioNubex")
                        .HasColumnType("numeric(20,0)");

                    b.Property<string>("Identificacion")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("RazonSocial")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("TokenNubex")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("IdUsuarioCliente");

                    b.ToTable("Compañia", "Configuracion");
                });

            modelBuilder.Entity("AtowerDocElectronico.Infraestructura.Entities.Facturas", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<byte[]>("Base64Pdf")
                        .HasColumnType("bytea");

                    b.Property<string>("Contrato")
                        .HasColumnType("text");

                    b.Property<string>("Cufe")
                        .HasColumnType("text");

                    b.Property<string>("DireccionFacturaDian")
                        .HasColumnType("text");

                    b.Property<string>("DocumentoAdquisiente")
                        .HasColumnType("text");

                    b.Property<int?>("Estado")
                        .HasColumnType("integer");

                    b.Property<string>("Factura")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FechaFactura")
                        .HasColumnType("text");

                    b.Property<int>("IdCompany")
                        .HasColumnType("integer");

                    b.Property<string>("JsonEnvioAtower")
                        .HasColumnType("text");

                    b.Property<string>("JsonEnvioNubex")
                        .HasColumnType("text");

                    b.Property<string>("JsonRespuestaNubex")
                        .HasColumnType("text");

                    b.Property<string>("NitEmpresa")
                        .HasColumnType("text");

                    b.Property<string>("NumeroFactura")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Prefijo")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal?>("ValorFactura")
                        .HasColumnType("numeric");

                    b.Property<decimal?>("ValorIva")
                        .HasColumnType("numeric");

                    b.Property<decimal?>("ValorOtro")
                        .HasColumnType("numeric");

                    b.Property<decimal?>("ValorTotal")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.HasIndex("IdCompany");

                    b.ToTable("Facturas", "Envios");
                });

            modelBuilder.Entity("AtowerDocElectronico.Infraestructura.Entities.Roles", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Codigo")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Roles", "Configuracion");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Codigo = "Dev",
                            Descripcion = "Developers"
                        },
                        new
                        {
                            Id = 2,
                            Codigo = "Admin",
                            Descripcion = "Administrador"
                        },
                        new
                        {
                            Id = 3,
                            Codigo = "Supp",
                            Descripcion = "Soporte"
                        },
                        new
                        {
                            Id = 4,
                            Codigo = "Cli",
                            Descripcion = "Cliente"
                        });
                });

            modelBuilder.Entity("AtowerDocElectronico.Infraestructura.Entities.Usuarios", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("Bloqueo")
                        .HasColumnType("boolean");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("IdRol")
                        .HasColumnType("integer");

                    b.Property<decimal>("Identificacion")
                        .HasColumnType("numeric(20,0)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("IdRol");

                    b.ToTable("Usuarios", "Configuracion");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Bloqueo = false,
                            Email = "support@atowers.com.co",
                            IdRol = 1,
                            Identificacion = 0m,
                            Nombre = "Developer Atower",
                            PasswordHash = "wvjq33+Fr8kCV1jZ9zTVaNIjiuWRI42H4FRRzuslajjZAlKJsCoDz9MxAz8X8ZigLEyVXImy24PgpQvC1vvlxg==",
                            PasswordSalt = "Tr8QpOR+IGweNWsKLLwVnv8kEFAOxwxmFUNRlRHN3KxvaEU7V5mxpnWPfFXWW5hIWelGrVErjer7UyucfCKA8A=="
                        },
                        new
                        {
                            Id = 2,
                            Bloqueo = false,
                            Email = "admin@gmail.com",
                            IdRol = 2,
                            Identificacion = 123456789m,
                            Nombre = "Admin",
                            PasswordHash = "INOApmD86A+GSlT35x9CBhrkSgVpgX3TL3Ybz7YEBRrsKX2ova2sWcRmqOsEuZFrxEhGluZMcgdAeFuaOGA2JQ==",
                            PasswordSalt = "b9BTJgJ1SeGjLn+EUD4mIbUURLdeIL2TqG5572ZM3ommw5MbJ5ebZqgBXovi5i+M3S82B6jyfqY4upBeHEslhg=="
                        });
                });

            modelBuilder.Entity("AtowerDocElectronico.Infraestructura.Entities.Compañia", b =>
                {
                    b.HasOne("AtowerDocElectronico.Infraestructura.Entities.Usuarios", "Usuarios")
                        .WithMany()
                        .HasForeignKey("IdUsuarioCliente")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Usuarios");
                });

            modelBuilder.Entity("AtowerDocElectronico.Infraestructura.Entities.Facturas", b =>
                {
                    b.HasOne("AtowerDocElectronico.Infraestructura.Entities.Compañia", "Compañia")
                        .WithMany()
                        .HasForeignKey("IdCompany")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Compañia");
                });

            modelBuilder.Entity("AtowerDocElectronico.Infraestructura.Entities.Usuarios", b =>
                {
                    b.HasOne("AtowerDocElectronico.Infraestructura.Entities.Roles", "Roles")
                        .WithMany()
                        .HasForeignKey("IdRol")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Roles");
                });
#pragma warning restore 612, 618
        }
    }
}
