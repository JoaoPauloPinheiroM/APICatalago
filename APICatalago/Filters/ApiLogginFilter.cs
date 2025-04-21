using Microsoft.AspNetCore.Mvc.Filters;

namespace APICatalago.Filters;

// Filtro de ação que registra logs antes e depois da execução de uma action
public class ApiLogginFilter : IActionFilter
{
    private readonly ILogger<ApiLogginFilter> _logger;

    public ApiLogginFilter(ILogger<ApiLogginFilter> logger)
    {
        _logger = logger;
    }

    // Disparado antes da execução da action
    public void OnActionExecuting(ActionExecutingContext context)
    {
        _logger.LogInformation($">>> Antes da action - {DateTime.Now.ToLongTimeString()}");
        _logger.LogInformation($"Model State válido: {context.ModelState.IsValid}");
    }

    // Disparado após a execução da action
    public void OnActionExecuted(ActionExecutedContext context)
    {
        _logger.LogInformation($">>> Depois da action - {DateTime.Now.ToLongTimeString()}");
        _logger.LogInformation($"Model State válido: {context.ModelState.IsValid}");
        _logger.LogInformation($"Status de resposta: {context.HttpContext.Response.StatusCode}");
    }
}