namespace APICatalago.Filters
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

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
            _logger.LogError($"Ocorreu uma exceção: {context.Exception.Message}");

            if (context.Exception is ArgumentNullException)
            {
                context.Result = new NotFoundObjectResult(context.Exception.Message);
                context.ExceptionHandled = true;
            }
            else if (context.Exception is ArgumentException)
            {
                context.Result = new BadRequestObjectResult(context.Exception.Message);
                context.ExceptionHandled = true;
            }
            else
            {
                context.Result = new ObjectResult("Ocorreu um erro ao processar sua requisição")
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
                context.ExceptionHandled = true;
            }
        }
    }
}