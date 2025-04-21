using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace APICatalago.Filters
{
    // Filtro global para capturar e tratar exceções não tratadas
    public class ApiExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<ApiExceptionFilter> _logger;

        public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger)
        {
            _logger = logger;
        }

        // Executado automaticamente quando uma exceção ocorre durante uma requisição
        public void OnException(ExceptionContext context)
        {
            // Registra o erro no log
            _logger.LogError($"Ocorreu uma exceção: {context.Exception.Message}");

            // Retorna resposta padrão com status 500 (erro interno)
            context.Result = new ObjectResult("Ocorreu um erro ao processar sua requisição")
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }
    }
}