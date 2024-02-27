using AtowerDocElectronico.Infraestructura.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace AtowerDocElectronico.Infraestructura.Interfaces
{
    public interface IAtowerDbContext
    {
        DbSet<Roles> Roles { get; set; }
        DbSet<Usuarios> Usuarios { get; set; }
        DbSet<Compañia> Compañias { get; set; }
        DatabaseFacade Database { get; }

        int SaveChanges();
        Task<int> SaveChangesAsync();
        void Dispose();
    }
}
