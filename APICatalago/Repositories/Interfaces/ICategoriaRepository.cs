using APICatalago.Models;
using APICatalago.Pagination;
using APICatalogo.Repositories.Interfaces;

namespace APICatalago.Repositories.Interfaces;

public interface ICategoriaRepository : IRepository<Categoria>
{
    PagedList<Categoria> GetCategorias(CategoriaParameters categoriaParameters);

    PagedList<Categoria> GetCategoriasFiltroNome(CategoriasFiltroNome categoriasParams);
}