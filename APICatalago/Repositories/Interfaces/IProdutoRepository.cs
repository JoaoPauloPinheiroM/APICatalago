using APICatalago.Models;
using APICatalago.Pagination;
using APICatalogo.Repositories.Interfaces;
using X.PagedList;

namespace APICatalago.Repositories.Interfaces;

public interface IProdutoRepository : IRepository<Produto>
{
    Task<IEnumerable<Produto>> GetProdutosPorCategoriaAsync(int id);

    Task<IPagedList<Produto>> GetProdutosAsync(ProdutosParameters produtosParameters);

    Task<IPagedList<Produto>> GetProdutosFiltroPrecoAsync(ProdutosFiltroPreco produtosParameters);
}