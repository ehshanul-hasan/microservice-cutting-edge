using Catalog.API.Model.Entity;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Context
{
    public class CatalogDBContext : DbContext
    {
        public CatalogDBContext(DbContextOptions<CatalogDBContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<Package> Packages { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
