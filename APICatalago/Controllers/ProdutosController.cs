using APICatalago.Models;
using APICatalago.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace APICatalago.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProdutosController : ControllerBase
{
    private readonly ProdutoServices _produtoServices;
    private readonly ILogger<ProdutosController> _logger;

    public ProdutosController(ProdutoServices service, ILogger<ProdutosController> logger)
    {
        _produtoServices = service;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Get()
    {
        _logger.LogInformation("Requisição GET para listar todos os produtos.");
        var produtos = _produtoServices.GetProdutos();

        if (!produtos.Any())
            return LogAndReturnNotFound("Nenhum produto encontrado.");

        _logger.LogInformation("Lista de produtos retornada com sucesso.");
        return Ok(produtos);
    }

    [HttpGet("categoria/{id:int}")]
    public IActionResult GetPorCategoria(int id)
    {
        _logger.LogInformation("Requisição GET para listar produtos da categoria com ID {Id}.", id);
        var produtos = _produtoServices.GetProdutosPorCategoria(id);
        if (!produtos.Any())
            return LogAndReturnNotFound($"Nenhum produto encontrado para a categoria com ID {id}.");
        _logger.LogInformation("Lista de produtos da categoria com ID {Id} retornada com sucesso.", id);
        return Ok(produtos);
    }

    [HttpGet("{id:int}", Name = "ObterProduto")]
    public IActionResult Get(int id)
    {
        _logger.LogInformation("Requisição GET para produto com ID {Id}.", id);
        var produto = _produtoServices.GetProduto(id);

        if (produto == null)
            return LogAndReturnNotFound($"Produto com ID {id} não encontrado.");

        _logger.LogInformation("Produto com ID {Id} retornado com sucesso.", id);
        return Ok(produto);
    }

    [HttpPost]
    public IActionResult Post(Produto produto)
    {
        _logger.LogInformation("Requisição POST para criar novo produto.");

        if (!IsValid(produto, out var erro))
            return erro;

        var novoProduto = _produtoServices.Create(produto);
        _logger.LogInformation("Produto criado com sucesso. ID: {Id}", novoProduto.ProdutoId);

        return CreatedAtRoute("ObterProduto", new { id = novoProduto.ProdutoId }, novoProduto);
    }

    [HttpPut("{id:int}")]
    public IActionResult Put(int id, Produto produto)
    {
        _logger.LogInformation("Requisição PUT para atualizar produto com ID {Id}.", id);

        if (produto == null || id != produto.ProdutoId)
            return LogAndReturnBadRequest("ID do produto não corresponde ou produto é nulo.");

        if (!IsValid(produto, out var erro))
            return erro;

        var atualizado = _produtoServices.Update(produto);
        if (atualizado is null)
            return StatusCode(500, $"Erro ao atualizar o produto id: {id}.");

        _logger.LogInformation("Produto com ID {Id} atualizado com sucesso.", id);
        return Ok(produto);
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        _logger.LogInformation("Requisição DELETE para produto com ID {Id}.", id);
        var produto = _produtoServices.Delete(id);

        if (produto == null)
            return LogAndReturnNotFound($"Produto com ID {id} não encontrado para exclusão.");

        _logger.LogInformation("Produto com ID {Id} excluído com sucesso.", id);
        return Ok($"{produto.Nome} removido com sucesso.");
    }

    //==== MÉTODOS AUXILIARES ====

    private bool IsValid(Produto produto, out IActionResult erro)
    {
        if (produto == null || !ModelState.IsValid)
        {
            erro = LogAndReturnBadRequest("Dados inválidos.");
            return false;
        }

        erro = null!;
        return true;
    }

    private IActionResult LogAndReturnNotFound(string message)
    {
        _logger.LogWarning(message);
        return NotFound(new { erro = message });
    }

    private IActionResult LogAndReturnBadRequest(string message)
    {
        _logger.LogError(message);
        return BadRequest(new { erro = message });
    }
}