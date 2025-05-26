using APICatalago.Models;
using APICatalago.Pagination;
using APICatalogo.Repositories.Interfaces;
using X.PagedList;

namespace APICatalago.Repositories.Interfaces;

public interface ICategoriaRepository : IRepository<Categoria>
{
    Task<IPagedList<Categoria>> GetCategoriasAsync(CategoriaParameters categoriaParameters);

    Task<IPagedList<Categoria>> GetCategoriasFiltroNomeAsync(CategoriasFiltroNome categoriasParams);
}