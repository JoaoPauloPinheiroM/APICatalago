using APICatalago.Context;
using APICatalago.Filters;
using APICatalago.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class CategoriasController : Controller
{
    private readonly AppDbContext _contexto;

    public CategoriasController(AppDbContext context)
    {
        _contexto = context;
    }

    // Retorna categorias junto com seus produtos (relacionamento incluído)
    [HttpGet("produtos")]
    public async Task<ActionResult<IEnumerable<Categoria>>> GetCategoriasProdutosAsync()
    {
        var categorias = await _contexto
            .Categorias
            .AsNoTracking()
            .Include(c => c.Produtos)
            .ToListAsync();

        return categorias;
    }

    // Retorna todas as categorias (sem produtos)
    [HttpGet]
    [ServiceFilter(typeof(ApiLogginFilter))] // Ativa o filtro de log para esta rota
    public async Task<ActionResult<IEnumerable<Categoria>>> GetAsync()
    {
        var categorias = await _contexto
            .Categorias
            .AsNoTracking()
            .ToListAsync();

        if (!categorias.Any())
            return NotFound("Nenhuma categoria encontrada!");

        return categorias;
    }

    // Busca uma categoria específica por ID
    [HttpGet("{id:int}", Name = "ObterCategoria")]
    public async Task<ActionResult<Categoria>> GetAsync(int id)
    {
        var categoria = await _contexto
            .Categorias
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.CategoriaId == id);

        if (categoria is null)
            return NotFound("Categoria não encontrada!");

        return categoria;
    }

    // Cria uma nova categoria
    [HttpPost]
    public ActionResult<Categoria> Post(Categoria categoria)
    {
        if (!ModelState.IsValid || categoria is null)
            return BadRequest("Dados inválidos!");

        _contexto.Categorias.Add(categoria);
        _contexto.SaveChanges();

        return CreatedAtRoute("ObterCategoria", new { id = categoria.CategoriaId }, categoria);
    }

    // Atualiza uma categoria existente
    [HttpPut("{id:int:min(1)}")]
    public ActionResult<Categoria> Put(int id, Categoria categoria)
    {
        if (id != categoria.CategoriaId || !ModelState.IsValid || categoria is null)
            return BadRequest("Dados inválidos ou inconsistentes!");

        _contexto.Entry(categoria).State = EntityState.Modified;
        _contexto.SaveChanges();

        return Ok(categoria);
    }

    // Remove uma categoria
    [HttpDelete("{id:int:min(1)}")]
    public ActionResult<Categoria> Delete(int id)
    {
        var categoria = _contexto.Categorias.FirstOrDefault(p => p.CategoriaId == id);
        if (categoria is null)
            return NotFound("Categoria não encontrada!");

        _contexto.Categorias.Remove(categoria);
        _contexto.SaveChanges();

        return Ok(categoria);
    }
}