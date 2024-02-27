using AtowerDocElectronico.Infraestructura.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AtowerDocElectronico.Infraestructura.Persistencia.MySQL
{
    public partial class NubexDbContext: DbContext, INubexDbContext
    {
        public NubexDbContext(DbContextOptions<NubexDbContext> options) : base(options) { }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    }
}
