using APICatalago.Context;
using APICatalago.Models;
using APICatalago.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace APICatalago.Repositories;

public class ProdutoRepository : Repository<Produto>, IProdutoRepository
{
    public ProdutoRepository(AppDbContext context) : base(context)
    {
    }

    public IEnumerable<Produto> GetProdutosPorCategoria(int id)
    {
        return GetAll()
            .Where(p => p.CategoriaId == id)
            .ToList();
    }
}