using Microsoft.AspNetCore.Mvc.Filters;

namespace APICatalago.Filters;

public class ApiLogginFilter : IActionFilter

{
    private readonly ILogger<ApiLogginFilter> _logger;

    public ApiLogginFilter(ILogger<ApiLogginFilter> logger)
    {
        _logger = logger;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        //executando antes da action
        _logger.LogInformation($">>>>>>>>>>>>>>>Executando antes da action<<<<<<<<<<<<<<<<< {DateTime.Now.ToLongTimeString()}");
        _logger.LogInformation($"Model State : {context.ModelState.IsValid}");
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        //executando depois da action
        _logger.LogInformation($">>>>>>>>>>>>>>>Executando depois da action<<<<<<<<<<<<<<< {DateTime.Now.ToLongTimeString()}");
        _logger.LogInformation($"Model State : {context.ModelState.IsValid}");
        _logger.LogInformation($"Status code : {context.HttpContext.Response.StatusCode}");
    }
}