using APICatalago.Context;
using APICatalago.Models;
using APICatalago.Pagination;
using APICatalago.Repositories.Interfaces;
using X.PagedList;

namespace APICatalago.Repositories;

public class CategoriaReposiory : Repository<Categoria>, ICategoriaRepository
{
    public CategoriaReposiory(AppDbContext context) : base(context)
    {
    }

    public async Task<IPagedList<Categoria>> GetCategoriasAsync(CategoriaParameters categoriaParameters)
    {
        var categorias = await GetAllAsync();
        var categoriaOrdenada = categorias.OrderBy(c => c.CategoriaId).AsQueryable();

        //var resultado = PagedList<Categoria>.ToPagedList(categoriaOrdenada, categoriaParameters.PageNumber, categoriaParameters.PageSize);
        var resultado = await categoriaOrdenada.ToPagedListAsync(categoriaParameters.PageNumber, categoriaParameters.PageSize);
        return resultado;
    }

    public async Task<IPagedList<Categoria>> GetCategoriasFiltroNomeAsync(CategoriasFiltroNome categoriasParams)
    {
        var categorias = await GetAllAsync();
        if (!string.IsNullOrEmpty(categoriasParams.Nome))
        {
            categorias = categorias.Where(c => c.Nome != null && c.Nome.Contains(categoriasParams.Nome, StringComparison.OrdinalIgnoreCase));
        }

        //var categoriasFiltradas = PagedList<Categoria>.ToPagedList(categorias.AsQueryable(), categoriasParams.PageNumber, categoriasParams.PageSize);
        var categoriasFiltradas = await categorias.ToPagedListAsync(categoriasParams.PageNumber, categoriasParams.PageSize);
        return categoriasFiltradas;
    }
}