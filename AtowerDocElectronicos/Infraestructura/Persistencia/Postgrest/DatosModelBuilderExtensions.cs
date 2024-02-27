using AtowerDocElectronico.Infraestructura.Entities;
using Microsoft.EntityFrameworkCore;

namespace AtowerDocElectronico.Infraestructura.Persistencia.Postgrest
{
    public static class DatosModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Roles>().HasData(
                new Roles { Id = 1, Codigo = "Dev", Descripcion = "Developers" },
                new Roles { Id = 2, Codigo = "Admin", Descripcion = "Administrador" },
                new Roles { Id = 3, Codigo = "Supp", Descripcion = "Soporte" },
                new Roles { Id = 4, Codigo = "Cli", Descripcion = "Cliente" }
                // Agrega más datos si es necesario
            );

            modelBuilder.Entity<Usuarios>().HasData(
                new Usuarios
                {
                    Id = 1,
                    Identificacion = 000000,
                    Nombre = "Developer Atower",
                    Email = "support@atowers.com.co",
                    IdRol = 1,
                    Bloqueo = false,
                    PasswordSalt = "Tr8QpOR+IGweNWsKLLwVnv8kEFAOxwxmFUNRlRHN3KxvaEU7V5mxpnWPfFXWW5hIWelGrVErjer7UyucfCKA8A==",
                    PasswordHash = "wvjq33+Fr8kCV1jZ9zTVaNIjiuWRI42H4FRRzuslajjZAlKJsCoDz9MxAz8X8ZigLEyVXImy24PgpQvC1vvlxg=="
                },
                new Usuarios
                {
                    Id = 2,
                    Identificacion = 123456789,
                    Nombre = "Admin",
                    Email = "admin@gmail.com",
                    IdRol = 2,
                    Bloqueo = false,
                    PasswordSalt = "b9BTJgJ1SeGjLn+EUD4mIbUURLdeIL2TqG5572ZM3ommw5MbJ5ebZqgBXovi5i+M3S82B6jyfqY4upBeHEslhg==",
                    PasswordHash = "INOApmD86A+GSlT35x9CBhrkSgVpgX3TL3Ybz7YEBRrsKX2ova2sWcRmqOsEuZFrxEhGluZMcgdAeFuaOGA2JQ=="
                }
                // Agrega más datos si es necesario
            );

            
        }
    }
}
