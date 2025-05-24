using APICatalago.Context;
using APICatalago.Models;
using APICatalago.Pagination;
using APICatalago.Repositories.Interfaces;

namespace APICatalago.Repositories;

public class CategoriaReposiory : Repository<Categoria>, ICategoriaRepository
{
    public CategoriaReposiory(AppDbContext context) : base(context)
    {
    }

    public PagedList<Categoria> GetCategorias(CategoriaParameters categoriaParameters)
    {
        var categorias = GetAll().OrderBy(c => c.CategoriaId).AsQueryable();

        var categoriasOrdenadas = PagedList<Categoria>.ToPagedList(categorias, categoriaParameters.PageNumber, categoriaParameters.PageSize);

        return categoriasOrdenadas;
    }

    public PagedList<Categoria> GetCategoriasFiltroNome(CategoriasFiltroNome categoriasParams)
    {
        var categorias = GetAll().AsQueryable();
        if (!string.IsNullOrEmpty(categoriasParams.Nome))
        {
            categorias = categorias.Where(c => c.Nome.Contains(categoriasParams.Nome, StringComparison.OrdinalIgnoreCase));
        }

        var categoriasFiltradas = PagedList<Categoria>.ToPagedList(categorias, categoriasParams.PageNumber, categoriasParams.PageSize);
        return categoriasFiltradas;
    }
}