using APICatalago.Context;
using APICatalago.Repositories.Interfaces;

namespace APICatalago.Repositories;

public class UniOfWork : IUnitOfWork
{
    private IProdutoRepository? _produtoRepo;

    private ICategoriaRepository? _categoriaRepo;

    public AppDbContext _context;

    //Injeção de dependência do contexto
    public UniOfWork(AppDbContext context)
    {
        _context = context;
    }

    //Propriedades para acessar os repositórios, como campos publicos
    //Lazy loading -> mais flexivel para uso
    public IProdutoRepository ProdutoRepository
    {
        get
        {
            //Verifica se o repositorio ja existe -> Evito instanciar mais de uma vez
            return _produtoRepo = _produtoRepo ?? new ProdutoRepository(_context);
        }
    }

    public ICategoriaRepository CategoriaRepository
    {
        get
        {
            return _categoriaRepo = _categoriaRepo ?? new CategoriaReposiory(_context);
        }
    }

    public void Commit()
    {
        _context.SaveChanges();
    }

    //Dispose -> Destruir o contexto
    public void Dispose()
    {
        _context.Dispose();
    }
}