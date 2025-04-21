using APICatalago.Context;
using APICatalago.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalago.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProdutosController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProdutosController(AppDbContext context)
    {
        _context = context;
    }

    // Retorna todos os produtos cadastrados
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Produto>>> Get()
    {
        var produtos = await _context.Produtos.AsNoTracking().ToListAsync();

        if (produtos is null || !produtos.Any())
            return NotFound("Nenhum produto encontrado.");

        return produtos;
    }

    // Retorna um único produto com base no ID informado
    [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
    public async Task<ActionResult<Produto>> GetAsync(int id)
    {
        var produto = await _context.Produtos
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.ProdutoId == id);

        if (produto is null)
            return NotFound("Produto não encontrado.");

        return produto;
    }

    // Cadastra um novo produto no sistema
    [HttpPost]
    public ActionResult Post(Produto produto)
    {
        if (!ModelState.IsValid || produto is null)
            return BadRequest("Dados inválidos.");

        _context.Produtos.Add(produto);
        _context.SaveChanges();

        return CreatedAtRoute("ObterProduto", new { id = produto.ProdutoId }, produto);
    }

    // Atualiza os dados de um produto existente
    [HttpPut("{id:int:min(1)}")]
    public ActionResult Put(int id, Produto produto)
    {
        if (!ModelState.IsValid || id != produto.ProdutoId)
            return BadRequest("Dados inconsistentes ou inválidos.");

        var produtoExistente = _context.Produtos.AsNoTracking().FirstOrDefault(p => p.ProdutoId == id);
        if (produtoExistente is null)
            return NotFound("Produto não encontrado para atualização.");

        _context.Entry(produto).State = EntityState.Modified;
        _context.SaveChanges();

        return Ok(produto);
    }

    // Remove um produto com base no ID informado
    [HttpDelete("{id:int:min(1)}")]
    public ActionResult Delete(int id)
    {
        var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);

        if (produto is null)
            return NotFound("Produto não encontrado.");

        _context.Produtos.Remove(produto);
        _context.SaveChanges();

        return Ok($"{produto.Nome} removido com sucesso.");
    }
}