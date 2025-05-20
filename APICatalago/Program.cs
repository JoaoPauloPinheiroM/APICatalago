using APICatalago.Context;
using APICatalago.DTOs.Mappings;
using APICatalago.Filters;
using APICatalago.Filters.Extensions;
using APICatalago.Logging;
using APICatalago.Repositories;
using APICatalago.Repositories.Interfaces;
using APICatalago.Services;

using APICatalogo.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

//Cria o builder para configurar o aplicativo
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
}).AddNewtonsoftJson();

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

//Cofigura as dependencias do repositorio
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UniOfWork>();

// Configura a dependencia do service
builder.Services.AddScoped<CategoriaServices>();
builder.Services.AddScoped<ProdutoServices>();

//Configura do DTO
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile(new ProdutoMapper());
});
// Substitua a linha problem�tica com a seguinte abordagem expl�cita para resolver a ambiguidade:
builder.Services.AddAutoMapper(typeof(ProdutoMapper));

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