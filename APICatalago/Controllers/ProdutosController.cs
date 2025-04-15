using APICatalago.Context;
using APICatalago.Models;
using Microsoft.AspNetCore.Http;
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

    // Rota async para obter todos os produtos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Produto>>> Get()
    {
        try
        {
            var produtos = await _context.Produtos.AsNoTracking().ToListAsync();
            if (produtos is null || !produtos.Any())
            {
                return NotFound("Produtos não encontrados!");
            }
            return produtos;
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao buscar produtos: {ex.Message}");
        }
    }

    // Rota async para obter produtos por categoria
    [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
    public async Task<ActionResult<Produto>> GetAsync(int id)
    {
        try
        {
            var produto = await _context.Produtos.AsNoTracking()
                .FirstOrDefaultAsync(p => p.ProdutoId == id);
            if (produto is null)
            {
                return NotFound("Produto não encontrado!");
            }
            return produto;
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao buscar o produto: {ex.Message}");
        }
    }

    // Rota para adicionar um novo produto
    [HttpPost]
    public ActionResult Post(Produto produto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (produto is null)
            {
                return BadRequest("Produto inválido!");
            }

            _context.Produtos.Add(produto);
            _context.SaveChanges();

            return new CreatedAtRouteResult("ObterProduto",
                new { id = produto.ProdutoId }, produto);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao adicionar produto: {ex.Message}");
        }
    }

    // Rota para atualizar um produto existente
    [HttpPut("{id:int:min(1)}")]
    public ActionResult Put(int id, Produto produto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != produto.ProdutoId)
            {
                return BadRequest("Produto inválido!");
            }

            var produtoExistente = _context.Produtos.AsNoTracking().FirstOrDefault(p => p.ProdutoId == id);
            if (produtoExistente is null)
            {
                return NotFound("Produto não encontrado para atualização!");
            }

            _context.Entry(produto).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(produto);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao atualizar produto: {ex.Message}");
        }
    }

    // Rota para remover um produto
    [HttpDelete("{id:int:min(1)}")]
    public ActionResult Delete(int id)
    {
        try
        {
            var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);
            if (produto is null)
            {
                return NotFound("Produto não encontrado!");
            }

            _context.Produtos.Remove(produto);
            _context.SaveChanges();

            return Ok($"{produto.Nome} removido com sucesso!");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao remover produto: {ex.Message}");
        }
    }
}