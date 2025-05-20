using APICatalago.Context;
using APICatalago.Models;
using APICatalago.Pagination;
using APICatalago.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace APICatalago.Repositories;

public class ProdutoRepository : Repository<Produto>, IProdutoRepository
{
    public ProdutoRepository(AppDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Método para retornar todos os produtos paginados
    /// </summary>
    public PagedList<Produto> GetProdutos(ProdutosParameters protudosParameters)
    {
        var produtos = GetAll()
            .OrderBy(p => p.ProdutoId).AsQueryable();

        var produtosOrdenados =
            PagedList<Produto>.ToPagedList(produtos, protudosParameters.PageNumber, protudosParameters.PageSize);

        return produtosOrdenados;
    }

    public IEnumerable<Produto> GetProdutosPorCategoria(int id)
    {
        return GetAll()
            .Where(p => p.CategoriaId == id)
            .ToList();
    }
}