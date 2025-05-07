using APICatalago.DTOs;
using APICatalago.DTOs.Mappings;
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
    public ActionResult<IEnumerable<CategoriaDTO>> Get()
    {
        LogRequest("GET", "listar todas as categorias");

        var categorias = _categoriaService.GetCategorias();

        if (!categorias.Any())
            return LogAndReturnNotFound<IEnumerable<CategoriaDTO>>("Nenhuma categoria encontrada.");

        LogSuccess("Categorias listadas com sucesso");

        return Ok(categorias.ToDTOList());
    }

    [HttpGet("{id:int}", Name = "ObterCategoria")]
    public ActionResult<CategoriaDTO> Get(int id)
    {
        LogRequest("GET", $"categoria com ID {id}");

        var categoria = _categoriaService.GetCategoria(id);

        if (categoria == null)
            return LogAndReturnNotFound<CategoriaDTO>($"Categoria com ID {id} não encontrada.");

        LogSuccess($"Categoria com ID {id} retornada com sucesso");

        return Ok(categoria.ToDTO());
    }

    [HttpPost]
    public ActionResult<CategoriaDTO> Post(CategoriaDTO categoriaDto)
    {
        LogRequest("POST", "criar uma nova categoria");

        if (!IsValid<CategoriaDTO>(categoriaDto, out var erro)) return erro;

        var novaCategoria = _categoriaService.Create(categoriaDto.ToEntity());
        var novaCategoriaDto = novaCategoria.ToDTO();

        LogSuccess($"Categoria criada com ID {novaCategoriaDto.CategoriaId}");

        return CreatedAtRoute("ObterCategoria", new { id = novaCategoriaDto.CategoriaId }, novaCategoriaDto);
    }

    [HttpPut("{id:int:min(1)}")]
    public ActionResult<CategoriaDTO> Put(int id, CategoriaDTO categoriaDto)
    {
        LogRequest("PUT", $"atualizar categoria com ID {id}");

        if (categoriaDto == null || id != categoriaDto.CategoriaId)
            return LogAndReturnBadRequest<CategoriaDTO>("Dados inválidos ou inconsistentes.");

        if (!ModelState.IsValid)
            return LogAndReturnBadRequest<CategoriaDTO>("Modelo inválido.");

        var categoria = categoriaDto.ToEntity();
        var atualizada = _categoriaService.Update(categoria);
        var categoriaAtualizadaDTO = atualizada.ToDTO();

        LogSuccess($"Categoria com ID {id} atualizada com sucesso");

        return Ok(categoriaAtualizadaDTO);
    }

    [HttpDelete("{id:int:min(1)}")]
    public ActionResult<CategoriaDTO> Delete(int id)
    {
        LogRequest("DELETE", $"excluir categoria com ID {id}");

        var excluida = _categoriaService.Delete(id);

        if (excluida == null)
            return LogAndReturnNotFound<CategoriaDTO>($"Categoria com ID {id} não encontrada para exclusão.");

        LogSuccess($"Categoria com ID {id} excluída com sucesso");

        return Ok(excluida.ToDTO());
    }

    //==== MÉTODOS AUXILIARES ====

    private bool IsValid<T>(object dto, out ActionResult<T>? erro)
    {
        if (dto == null || !ModelState.IsValid)
        {
            erro = LogAndReturnBadRequest<T>("Dados inválidos.");
            return false;
        }

        erro = null;
        return true;
    }

    private ActionResult<T> LogAndReturnNotFound<T>(string message)
    {
        _logger.LogWarning(message);
        return NotFound(new { erro = message });
    }

    private ActionResult<T> LogAndReturnBadRequest<T>(string message)
    {
        _logger.LogError(message);
        return BadRequest(new { erro = message });
    }

    private void LogRequest(string metodo, string acao) =>
        _logger.LogInformation("Requisição {Metodo} para {Acao}.", metodo.ToUpper(), acao);

    private void LogSuccess(string mensagem) =>
        _logger.LogInformation(mensagem);
}