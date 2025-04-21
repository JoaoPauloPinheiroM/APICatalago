using APICatalago.Context;
using APICatalago.Extensions;
using APICatalago.Filters;
using APICatalago.Logging;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Configura os controllers e evita loops de refer�ncia no JSON
builder.Services.AddControllers(options =>
{
    // Adiciona o filtro de exce��o global
    options.Filters.Add(typeof(ApiExceptionFilter));
}).AddJsonOptions(options =>
{
    // Configura o JsonSerializer para ignorar ciclos de refer�ncia
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

// Habilita documenta��o Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Conex�o e configura��o do EF Core com MySQL
string? mysqlConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(mysqlConnectionString, ServerVersion.AutoDetect(mysqlConnectionString))
);

// Filtro de logging para monitorar chamadas de API
builder.Services.AddScoped<ApiLogginFilter>();

// Adiciona um logger customizado com n�vel m�nimo de "Information"
builder.Logging.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfiguration
{
    LogLevel = LogLevel.Information
}));

var app = builder.Build();

// Configura��es para ambiente de desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ConfigureApiExceptionHandle(); // Tratamento detalhado de erros
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();