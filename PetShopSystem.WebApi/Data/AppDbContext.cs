using Microsoft.EntityFrameworkCore;
using PetShopSystem.WebApi.Entities;

namespace PetShopSystem.WebApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Isso criará a tabela "Pets" no SQLite
        public DbSet<Pet> Pets { get; set; }
    }
}
