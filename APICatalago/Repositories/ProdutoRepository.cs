using APICatalago.Context;
using APICatalago.Models;
using APICatalago.Pagination;
using APICatalago.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net.WebSockets;
using X.PagedList;

namespace APICatalago.Repositories;

public class ProdutoRepository : Repository<Produto>, IProdutoRepository
{
    public ProdutoRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IPagedList<Produto>> GetProdutosAsync(ProdutosParameters produtosParameters)
    {
        var produtos = await GetAllAsync();
        var produtosOrdenados = produtos.OrderBy(p => p.ProdutoId).AsQueryable();

        //var resultado =
        //    PagedList<Produto>.ToPagedList(prdutosOrdenados, produtosParameters.PageNumber, produtosParameters.PageSize);

        var resultado = await produtosOrdenados.ToPagedListAsync(produtosParameters.PageNumber, produtosParameters.PageSize);

        return resultado;
    }

    public async Task<IPagedList<Produto>> GetProdutosFiltroPrecoAsync(ProdutosFiltroPreco produtosFiltroparameters)
    {
        var produtos = await GetAllAsync();
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

        //var produtosFiltrados = PagedList<Produto>.ToPagedList(produtos.AsQueryable(), produtosFiltroparameters.PageNumber, produtosFiltroparameters.PageSize);
        var produtosFiltrados = await produtos.AsQueryable()
            .ToPagedListAsync(produtosFiltroparameters.PageNumber, produtosFiltroparameters.PageSize);
        return produtosFiltrados;
    }

    public async Task<IEnumerable<Produto>> GetProdutosPorCategoriaAsync(int id)
    {
        var produtos = await GetAllAsync();

        return produtos.Where(p => p.CategoriaId == id).ToList();
    }
}