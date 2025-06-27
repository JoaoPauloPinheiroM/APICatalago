using Microsoft.EntityFrameworkCore;
using APICatalago.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace APICatalago.Context
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        // Constructor that accepts DbContextOptions and passes it to the base class
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }

        // This method is called by the runtime to configure the database context
        public DbSet<Categoria> Categorias { get; set; }

        public DbSet<Produto> Produtos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}