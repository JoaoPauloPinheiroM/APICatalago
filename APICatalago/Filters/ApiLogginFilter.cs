using Microsoft.AspNetCore.Mvc.Filters;

namespace APICatalago.Filters;

public class ApiLogginFilter : IActionFilter
{
    private readonly ILogger<ApiLogginFilter> _logger;

    public ApiLogginFilter(ILogger<ApiLogginFilter> logger) => _logger = logger;

    public void OnActionExecuting(ActionExecutingContext context)
    {
        _logger.LogInformation(">>> Antes da action - {Time}", DateTime.Now.ToLongTimeString());
        _logger.LogInformation("Model State válido: {Valid}", context.ModelState.IsValid);
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        _logger.LogInformation(">>> Depois da action - {Time}", DateTime.Now.ToLongTimeString());
        _logger.LogInformation("Model State válido: {Valid}", context.ModelState.IsValid);
        _logger.LogInformation("Status de resposta: {Status}", context.HttpContext.Response.StatusCode);
    }
}