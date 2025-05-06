using APICatalago.Filters;
using APICatalago.Models;
using APICatalago.Services;
using Microsoft.AspNetCore.Mvc;

namespace APICatalago.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriasController : ControllerBase
{
    private readonly CategoriaServices _categoriaService;
    private readonly ILogger<CategoriasController> _logger;

    public CategoriasController(CategoriaServices categoriaService, ILogger<CategoriasController> logger)
    {
        _categoriaService = categoriaService;
        _logger = logger;
    }

    [HttpGet]
    [ServiceFilter(typeof(ApiLogginFilter))]
    public IActionResult Get()
    {
        _logger.LogInformation("Requisição GET para listar todas as categorias.");
        var categorias = _categoriaService.GetCategorias();

        if (!categorias.Any())
            return LogAndReturnNotFound("Nenhuma categoria encontrada.");

        _logger.LogInformation("Categorias listadas com sucesso.");
        return Ok(categorias);
    }

    [HttpGet("{id:int}", Name = "ObterCategoria")]
    public IActionResult Get(int id)
    {
        _logger.LogInformation("Requisição GET para categoria com ID {Id}.", id);
        var categoria = _categoriaService.GetCategoria(id);

        if (categoria == null)
            return LogAndReturnNotFound($"Categoria com ID {id} não encontrada.");

        _logger.LogInformation("Categoria com ID {Id} retornada com sucesso.", id);
        return Ok(categoria);
    }

    [HttpPost]
    public IActionResult Post(Categoria categoria)
    {
        _logger.LogInformation("Requisição POST para criar uma nova categoria.");

        if (!IsValid(categoria, out var erro)) return erro;

        var novaCategoria = _categoriaService.Create(categoria);
        _logger.LogInformation("Categoria criada com ID {Id}.", novaCategoria.CategoriaId);

        return CreatedAtRoute("ObterCategoria", new { id = novaCategoria.CategoriaId }, novaCategoria);
    }

    [HttpPut("{id:int:min(1)}")]
    public IActionResult Put(int id, Categoria categoria)
    {
        _logger.LogInformation("Requisição PUT para atualizar categoria com ID {Id}.", id);

        if (categoria == null || id != categoria.CategoriaId)
            return LogAndReturnBadRequest("Dados inválidos ou inconsistentes.");

        if (!ModelState.IsValid)
            return LogAndReturnBadRequest("Modelo inválido.");

        var atualizada = _categoriaService.Update(categoria);
        _logger.LogInformation("Categoria com ID {Id} atualizada com sucesso.", id);

        return Ok(atualizada);
    }

    [HttpDelete("{id:int:min(1)}")]
    public IActionResult Delete(int id)
    {
        _logger.LogInformation("Requisição DELETE para excluir categoria com ID {Id}.", id);
        var excluida = _categoriaService.Delete(id);

        if (excluida == null)
            return LogAndReturnNotFound($"Categoria com ID {id} não encontrada para exclusão.");

        _logger.LogInformation("Categoria com ID {Id} excluída com sucesso.", id);
        return Ok(excluida);
    }

    //==== MÉTODOS AUXILIARES ====

    private bool IsValid(Categoria categoria, out IActionResult erro)
    {
        if (categoria == null || !ModelState.IsValid)
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