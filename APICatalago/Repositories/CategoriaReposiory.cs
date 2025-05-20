using APICatalago.Context;
using APICatalago.Models;
using APICatalago.Repositories.Interfaces;

namespace APICatalago.Repositories;

public class CategoriaReposiory : Repository<Categoria>, ICategoriaRepository
{
    public CategoriaReposiory(AppDbContext context) : base(context)
    {
    }
}