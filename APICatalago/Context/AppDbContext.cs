using Microsoft.EntityFrameworkCore;
using APICatalago.Models;

namespace APICatalago.Context
{
    public class AppDbContext : DbContext
    {
        // Constructor that accepts DbContextOptions and passes it to the base class
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }

        // This method is called by the runtime to configure the database context
        public DbSet<Categoria> Categorias { get; set; }

        public DbSet<Produto> Produtos { get; set; }
    }
}