using APICatalago.DTOs;
using APICatalago.DTOs.Mappings;
using APICatalago.Models;
using APICatalago.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace APICatalago.Controllers
{
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
        public ActionResult<IEnumerable<ProdutoDTO>> Get()
        {
            LogRequest("GET", "listar todos os produtos");
            var produtos = _produtoServices.GetProdutos();

            if (!produtos.Any())
                return LogAndReturnNotFound<IEnumerable<ProdutoDTO>>("Nenhum produto encontrado.");

            LogSuccess("Produtos listados com sucesso.");
            return Ok(produtos.ToDTOList());
        }

        [HttpGet("categoria/{id:int}")]
        public ActionResult<IEnumerable<ProdutoDTO>> GetPorCategoria(int id)
        {
            LogRequest("GET", $"listar produtos da categoria com ID {id}");
            var produtos = _produtoServices.GetProdutosPorCategoria(id);

            if (!produtos.Any())
                return LogAndReturnNotFound<IEnumerable<ProdutoDTO>>($"Nenhum produto encontrado para a categoria com ID {id}.");

            LogSuccess($"Produtos da categoria com ID {id} listados com sucesso.");
            return Ok(produtos.ToDTOList());
        }

        [HttpGet("{id:int}", Name = "ObterProduto")]
        public ActionResult<ProdutoDTO> Get(int id)
        {
            LogRequest("GET", $"produto com ID {id}");
            var produto = _produtoServices.GetProduto(id);

            if (produto == null)
                return LogAndReturnNotFound<ProdutoDTO>($"Produto com ID {id} não encontrado.");

            LogSuccess($"Produto com ID {id} retornado com sucesso.");
            return Ok(produto.ToDTO());
        }

        [HttpPost]
        public ActionResult<ProdutoDTO> Post(ProdutoDTO produtoDto)
        {
            LogRequest("POST", "criar novo produto");

            if (!IsValid<ProdutoDTO>(produtoDto, out var erro)) return erro;

            var novoProduto = _produtoServices.Create(produtoDto.ToEntity());
            var novoProdutoDto = novoProduto.ToDTO();

            LogSuccess($"Produto criado com sucesso. ID: {novoProdutoDto.ProdutoId}");
            return CreatedAtRoute("ObterProduto", new { id = novoProdutoDto.ProdutoId }, novoProdutoDto);
        }

        [HttpPut("{id:int}")]
        public ActionResult<ProdutoDTO> Put(int id, ProdutoDTO produtoDto)
        {
            LogRequest("PUT", $"atualizar produto com ID {id}");

            if (produtoDto == null || id != produtoDto.ProdutoId)
                return LogAndReturnBadRequest<ProdutoDTO>("ID do produto não corresponde ou produto é nulo.");

            if (!ModelState.IsValid)
                return LogAndReturnBadRequest<ProdutoDTO>("Dados inválidos.");

            var atualizado = _produtoServices.Update(produtoDto.ToEntity());

            if (atualizado is null)
                return StatusCode(500, new { erro = $"Erro ao atualizar o produto ID: {id}." });

            LogSuccess($"Produto com ID {id} atualizado com sucesso.");
            return Ok(atualizado.ToDTO());
        }

        [HttpDelete("{id:int}")]
        public ActionResult<ProdutoDTO> Delete(int id)
        {
            LogRequest("DELETE", $"excluir produto com ID {id}");
            var excluido = _produtoServices.Delete(id);

            if (excluido == null)
                return LogAndReturnNotFound<ProdutoDTO>($"Produto com ID {id} não encontrado para exclusão.");

            LogSuccess($"Produto com ID {id} excluído com sucesso.");
            return Ok(excluido.ToDTO());
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
}