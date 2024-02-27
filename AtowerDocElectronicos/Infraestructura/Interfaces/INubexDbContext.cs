
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace AtowerDocElectronico.Infraestructura.Interfaces
{
    public interface INubexDbContext
    {
        DatabaseFacade Database { get; }



        int SaveChanges();
        Task<int> SaveChangesAsync();
        void Dispose();
    }
}
