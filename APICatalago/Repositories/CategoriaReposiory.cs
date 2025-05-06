using APICatalago.Context;
using APICatalago.Models;
using APICatalago.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace APICatalago.Repositories;

public class CategoriaReposiory : Repository<Categoria>, ICategoriaRepository
{
    public CategoriaReposiory(AppDbContext context) : base(context)
    {
    }
}