using APICatalago.Context;
using APICatalago.Models;
using APICatalago.Pagination;
using APICatalago.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net.WebSockets;

namespace APICatalago.Repositories;

public class ProdutoRepository : Repository<Produto>, IProdutoRepository
{
    public ProdutoRepository(AppDbContext context) : base(context)
    {
    }

    public PagedList<Produto> GetProdutos(ProdutosParameters protudosParameters)
    {
        var produtos = GetAll()
            .OrderBy(p => p.ProdutoId).AsQueryable();

        var produtosOrdenados =
            PagedList<Produto>.ToPagedList(produtos, protudosParameters.PageNumber, protudosParameters.PageSize);

        return produtosOrdenados;
    }

    public PagedList<Produto> GetProdutosFiltroPreco(ProdutosFiltroPreco produtosFiltroparameters)
    {
        var produtos = GetAll().AsQueryable();
        if (produtosFiltroparameters.Preco.HasValue && !string.IsNullOrEmpty(produtosFiltroparameters.PrecoCriterio))
        {
            switch (produtosFiltroparameters.PrecoCriterio.ToLower())
            {
                case "maior":
                    produtos = produtos.Where(p => p.Preco > produtosFiltroparameters.Preco.Value).OrderBy(p => p.Preco);
                    break;

                case "menor":
                    produtos = produtos.Where(p => p.Preco < produtosFiltroparameters.Preco.Value).OrderBy(p => p.Preco);
                    break;

                case "igual":
                    produtos = produtos.Where(p => p.Preco == produtosFiltroparameters.Preco.Value).OrderBy(p => p.Preco);
                    break;

                default:
                    throw new ArgumentException("Critério de preço inválido.");
            }
        }

        var produtosFiltrados = PagedList<Produto>.ToPagedList(produtos, produtosFiltroparameters.PageNumber, produtosFiltroparameters.PageSize);
        return produtosFiltrados;
    }

    public IEnumerable<Produto> GetProdutosPorCategoria(int id)
    {
        return GetAll()
            .Where(p => p.CategoriaId == id)
            .ToList();
    }
}