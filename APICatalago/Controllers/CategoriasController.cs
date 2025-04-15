using APICatalago.Context;
using APICatalago.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalago.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriasController : Controller
{
    private readonly AppDbContext _context;

    public CategoriasController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Categorias/Produtos
    [HttpGet("produtos")]
    public async Task<ActionResult<IEnumerable<Categoria>>> GetCategoriasProdutosAsync()
    {
        try
        {
            return await _context
                .Categorias
                .AsNoTracking()
                .Include(c => c.Produtos)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar recuperar os produtos das categorias: {ex.Message}");
        }
    }

    // GET: Categorias
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Categoria>>> GetAsync()
    {
        try
        {
            var categorias = await _context
                .Categorias
                .AsNoTracking()
                .ToListAsync();

            if (!categorias.Any())
            {
                return NotFound("Nenhuma categoria encontrada!");
            }

            return categorias;
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar recuperar as categorias: {ex.Message}");
        }
    }

    // GET: Categorias/Por Id
    [HttpGet("{id:int}", Name = "ObterCategoria")]
    public async Task<ActionResult<Categoria>> GetAsync(int id)
    {
        try
        {
            var categoria = await _context
                .Categorias
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.CategoriaId == id);

            if (categoria is null)
            {
                return NotFound("Categoria não encontrada!");
            }

            return categoria;
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar recuperar a categoria: {ex.Message}");
        }
    }

    // POST: Categorias
    [HttpPost]
    public ActionResult<Categoria> Post(Categoria categoria)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (categoria is null)
            {
                return BadRequest("Categoria inválida!");
            }

            _context.Categorias.Add(categoria);
            _context.SaveChanges();

            return CreatedAtRoute("ObterCategoria", new { id = categoria.CategoriaId }, categoria);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao adicionar categoria: {ex.Message}");
        }
    }

    // PUT: Categorias
    [HttpPut("{id:int:min(1)}")]
    public ActionResult<Categoria> Put(int id, Categoria categoria)
    {
        try
        {
            if (id != categoria.CategoriaId)
            {
                return BadRequest("Id inválido!");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (categoria is null)
            {
                return BadRequest("Categoria inválida!");
            }

            _context.Entry(categoria).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(categoria);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao atualizar categoria: {ex.Message}");
        }
    }

    // DELETE: Categorias
    [HttpDelete("{id:int:min(1)}")]
    public ActionResult<Categoria> Delete(int id)
    {
        try
        {
            var categoria = _context.Categorias.FirstOrDefault(p => p.CategoriaId == id);
            if (categoria is null)
            {
                return NotFound("Categoria não encontrada!");
            }

            _context.Categorias.Remove(categoria);
            _context.SaveChanges();

            return Ok(categoria);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao remover categoria: {ex.Message}");
        }
    }
}