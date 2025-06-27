// =================================================================================================
// USING STATEMENTS
// =================================================================================================
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

using APICatalago.Context;
using APICatalago.DTOs.Mappings;
using APICatalago.Filters;
using APICatalago.Logging;
using APICatalago.Repositories;
using APICatalago.Repositories.Interfaces;
using APICatalago.Services;
using APICatalogo.Repositories.Interfaces;
using APICatalago.Filters.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using APICatalago.Models;

// =================================================================================================
// CONFIGURAÇÃO DA APLICAÇÃO WEB (BUILDER)
// =================================================================================================
var builder = WebApplication.CreateBuilder(args);

// --- 1. Configuração de Serviços (Injeção de Dependência) --------------------------------------

// Configuração base para Controllers e tratamento de JSON
builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(ApiExceptionFilter)); // Filtro global para tratamento de exceções.
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; // Evita loops em serializações.
})
.AddNewtonsoftJson(); // Suporte opcional ao Newtonsoft.Json.

// Documentação da API (Swagger/OpenAPI)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuração do Entity Framework Core com MySQL
string? mysqlConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(mysqlConnectionString, ServerVersion.AutoDetect(mysqlConnectionString))
);

var secretKey = builder.Configuration["JWT:SecretKey"]
    ?? throw new InvalidOperationException("chave invalida");

builder.Services.AddAuthentication(Options =>
{
    Options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    Options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true; // Salva o token no contexto da requisição.
    options.RequireHttpsMetadata = true; // Exige HTTPS para segurança.
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, // Valida o emissor do token.
        ValidateAudience = true, // Valida o público do token.
        ValidateLifetime = true, // Valida o tempo de vida do token.
        ValidateIssuerSigningKey = true, // Valida a chave de assinatura do token.
        ClockSkew = TimeSpan.Zero, // Não permite tolerância de tempo para expiração.
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"], // Emissor do token, definido na configuração.
        ValidAudience = builder.Configuration["JWT:ValidAudience"], // Público do token, definido na configuração.
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey)) // Chave de assinatura simétrica.
    };
});

// Configuração do ASP.NET Core Identity (Autenticação e Autorização)
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Registros de Repositórios e Unidade de Trabalho
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UniOfWork>();

// Registros de Serviços da Aplicação
builder.Services.AddScoped<CategoriaServices>();
builder.Services.AddScoped<ProdutoServices>();

// Configuração do AutoMapper (Mapeamento de Objetos)
builder.Services.AddAutoMapper(typeof(ProdutoMapper)); // Registra perfis do assembly de ProdutoMapper.

// Logging e Filtros Customizados
builder.Services.AddScoped<ApiLogginFilter>(); // Filtro para logging de requisições.
builder.Logging.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfiguration
{
    LogLevel = LogLevel.Information // Nível de log para o provedor customizado.
}));

// =================================================================================================
// CONSTRUÇÃO DA APLICAÇÃO (APP)
// =================================================================================================
var app = builder.Build();

// --- 2. Configuração do Pipeline de Requisições HTTP (Middleware) -----------------------------

// Middlewares para ambiente de Desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ConfigureApiExceptionHandle();
}
else
{
    // Configurar tratamento de erro para produção
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection(); // Força HTTPS.

// Middlewares de Autenticação e Autorização
// A ordem é importante: UseAuthentication antes de UseAuthorization.
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers(); // Mapeia as rotas para os controllers.

// =================================================================================================
// EXECUÇÃO DA APLICAÇÃO
// =================================================================================================
app.Run();