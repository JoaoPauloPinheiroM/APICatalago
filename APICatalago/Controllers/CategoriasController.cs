using APICatalago.Context;
using APICatalago.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalago.Controllers;

[Route("[controller]")]
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
    public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
    {
        try
        {
            return _context
                .Categorias
                .AsNoTracking()
                .Include(c => c.Produtos)
                .ToList();
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Erro ao tentar recuperar os produtos das categorias!");
        }
    }

    // GET: Categorias
    [HttpGet]
    public ActionResult<IEnumerable<Categoria>> Get()
    {
        try
        {

            var categorias = _context
                .Categorias
                .AsNoTracking()
                .ToList();

            if (categorias is null)
            {
                return NotFound("Nenhuma categoria encontrada!");
            }
            return categorias;
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Erro ao tentar recuperar as categorias!");
        }
    }

    // GET: Categorias/Por Id
    [HttpGet("{id:int}", Name = "ObterCategoria")]
    public ActionResult<Categoria> Get(int id)
    {
        try
        {

            var categoria = _context.Categorias.AsNoTracking().FirstOrDefault(p => p.CategoriaId == id);
            if (categoria is null)
            {
                return NotFound("Categoria não encontrada!");
            }
            return categoria;
        }
        catch(Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Erro ao tentar recuperar a categoria!");
        }
    }

    // POST: Categorias
    [HttpPost]
    public ActionResult<Categoria> Post([FromBody] Categoria categoria)
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

    // PUT: Categorias
    [HttpPut("{id:int}")]
    public ActionResult<Categoria> Put(int id, [FromBody] Categoria categoria)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        if (categoria is null)
        {
            return BadRequest("Categoria inválida!");
        }
        _context.Entry(categoria).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        _context.SaveChanges();
        return Ok(categoria);
    }

    // DELETE: Categorias
    [HttpDelete("{id:int}")]
    public ActionResult<Categoria> Delete(int id)
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
}