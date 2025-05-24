using APICatalago.Models;
using APICatalago.Pagination;
using APICatalogo.Repositories.Interfaces;

namespace APICatalago.Repositories.Interfaces;

public interface IProdutoRepository : IRepository<Produto>
{
    IEnumerable<Produto> GetProdutosPorCategoria(int id);

    PagedList<Produto> GetProdutos(ProdutosParameters protudosParameters);

    PagedList<Produto> GetProdutosFiltroPreco(ProdutosFiltroPreco produtosparameters);
}