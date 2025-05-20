using APICatalago.DTOs;
using APICatalago.DTOs.Mappings;
using APICatalago.Models;
using APICatalago.Pagination;
using APICatalago.Services;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace APICatalago.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProdutosController : ControllerBase
{
    private readonly ProdutoServices _produtoServices;
    private readonly ILogger<ProdutosController> _logger;
    private readonly IMapper _mapper;

    public ProdutosController(ProdutoServices service, ILogger<ProdutosController> logger, IMapper mapper)
    {
        _produtoServices = service;
        _logger = logger;
        _mapper = mapper;
    }

    [HttpGet("pagination")]
    public ActionResult<IEnumerable<ProdutoDTO>> GetPaginated([FromQuery] ProdutosParameters produtosParameters)
    {
        var produtos = _produtoServices.GetProdutos(produtosParameters);
        var metaData = new
        {
            produtos.TotalCount,
            produtos.PageSize,
            produtos.CurrentPage,
            produtos.TotalPages,
            produtos.HasNext,
            produtos.HasPrevious
        };
        Response.Headers["X-Pagination"] = JsonConvert.SerializeObject(metaData);
        var produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);
        return Ok(produtosDto);
    }

    [HttpGet]
    public ActionResult<IEnumerable<ProdutoDTO>> Get()
    {
        var produtos = _produtoServices.GetProdutos();

        if (!produtos.Any())
            return LogAndReturnNotFound<IEnumerable<ProdutoDTO>>("Nenhum produto encontrado.");

        // var destino = _mapper.Map<Destino>(Origem);
        var produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);
        return Ok(produtosDto);
    }

    [HttpGet("categoria/{id:int}")]
    public ActionResult<IEnumerable<ProdutoDTO>> GetPorCategoria(int id)
    {
        var produtos = _produtoServices.GetProdutosPorCategoria(id);

        if (!produtos.Any())
            return LogAndReturnNotFound<IEnumerable<ProdutoDTO>>($"Nenhum produto encontrado para a categoria com ID {id}.");

        var produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);
        return Ok(produtosDto);
    }

    [HttpGet("{id:int}", Name = "ObterProduto")]
    public ActionResult<ProdutoDTO> Get(int id)
    {
        var produto = _produtoServices.GetProduto(id);

        if (produto == null)
            return LogAndReturnNotFound<ProdutoDTO>($"Produto com ID {id} não encontrado.");

        var produtoDto = _mapper.Map<ProdutoDTO>(produto);
        return Ok(produtoDto);
    }

    [HttpPost]
    public ActionResult<ProdutoDTO> Post(ProdutoDTO produtoDto)
    {
        if (!IsValid<ProdutoDTO>(produtoDto, out var erro)) return erro;

        var produto = _mapper.Map<Produto>(produtoDto);
        var novoProduto = _produtoServices.Create(produto);
        var novoProdutoDto = _mapper.Map<ProdutoDTO>(novoProduto);

        return CreatedAtRoute("ObterProduto", new { id = novoProdutoDto.ProdutoId }, novoProdutoDto);
    }

    [HttpPatch("{id}/UpdatePartial")]
    public ActionResult<ProdutoDTOUpdateResponse> Patch(int id, JsonPatchDocument<ProdutoDTOUpdateRequest> patchProdutoDTO)
    {
        if (patchProdutoDTO is null || id <= 0)

            return BadRequest();

        var produto = _produtoServices.GetProduto(id);
        if (produto is null)

            return NotFound();

        var produtoUpdateRequest = _mapper.Map<ProdutoDTOUpdateRequest>(produto);

        patchProdutoDTO.ApplyTo(produtoUpdateRequest, ModelState);
        if (!ModelState.IsValid || !TryValidateModel(produtoUpdateRequest))

            return BadRequest(ModelState);

        _mapper.Map(produtoUpdateRequest, produto);
        _produtoServices.Update(produto);

        return Ok(_mapper.Map<ProdutoDTOUpdateResponse>(produto));
    }

    [HttpPut("{id:int}")]
    public ActionResult<ProdutoDTO> Put(int id, ProdutoDTO produtoDto)
    {
        if (produtoDto == null || id != produtoDto.ProdutoId)
            return LogAndReturnBadRequest<ProdutoDTO>("ID do produto não corresponde ou produto é nulo.");

        if (!ModelState.IsValid)
            return LogAndReturnBadRequest<ProdutoDTO>("Dados inválidos.");

        var produto = _mapper.Map<Produto>(produtoDto);
        var produtoAtualizado = _produtoServices.Update(produto);

        if (produtoAtualizado is null)
            return StatusCode(500, new { erro = $"Erro ao atualizar o produto ID: {id}." });

        var atualizadoDTO = _mapper.Map<ProdutoDTO>(produtoAtualizado);

        return Ok(atualizadoDTO);
    }

    [HttpDelete("{id:int}")]
    public ActionResult<ProdutoDTO> Delete(int id)
    {
        var excluido = _produtoServices.Delete(id);

        if (excluido == null)
            return LogAndReturnNotFound<ProdutoDTO>($"Produto com ID {id} não encontrado para exclusão.");
        var excluidoDto = _mapper.Map<ProdutoDTO>(excluido);
        return Ok(excluidoDto);
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
}