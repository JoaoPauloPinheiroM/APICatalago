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
// CONFIGURA��O DA APLICA��O WEB (BUILDER)
// =================================================================================================
var builder = WebApplication.CreateBuilder(args);

// --- 1. Configura��o de Servi�os (Inje��o de Depend�ncia) --------------------------------------

// Configura��o base para Controllers e tratamento de JSON
builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(ApiExceptionFilter)); // Filtro global para tratamento de exce��es.
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; // Evita loops em serializa��es.
})
.AddNewtonsoftJson(); // Suporte opcional ao Newtonsoft.Json.

// Documenta��o da API (Swagger/OpenAPI)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configura��o do Entity Framework Core com MySQL
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
    options.SaveToken = true; // Salva o token no contexto da requisi��o.
    options.RequireHttpsMetadata = true; // Exige HTTPS para seguran�a.
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, // Valida o emissor do token.
        ValidateAudience = true, // Valida o p�blico do token.
        ValidateLifetime = true, // Valida o tempo de vida do token.
        ValidateIssuerSigningKey = true, // Valida a chave de assinatura do token.
        ClockSkew = TimeSpan.Zero, // N�o permite toler�ncia de tempo para expira��o.
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"], // Emissor do token, definido na configura��o.
        ValidAudience = builder.Configuration["JWT:ValidAudience"], // P�blico do token, definido na configura��o.
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey)) // Chave de assinatura sim�trica.
    };
});

// Configura��o do ASP.NET Core Identity (Autentica��o e Autoriza��o)
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Registros de Reposit�rios e Unidade de Trabalho
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UniOfWork>();

// Registros de Servi�os da Aplica��o
builder.Services.AddScoped<CategoriaServices>();
builder.Services.AddScoped<ProdutoServices>();

// Configura��o do AutoMapper (Mapeamento de Objetos)
builder.Services.AddAutoMapper(typeof(ProdutoMapper)); // Registra perfis do assembly de ProdutoMapper.

// Logging e Filtros Customizados
builder.Services.AddScoped<ApiLogginFilter>(); // Filtro para logging de requisi��es.
builder.Logging.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfiguration
{
    LogLevel = LogLevel.Information // N�vel de log para o provedor customizado.
}));

// =================================================================================================
// CONSTRU��O DA APLICA��O (APP)
// =================================================================================================
var app = builder.Build();

// --- 2. Configura��o do Pipeline de Requisi��es HTTP (Middleware) -----------------------------

// Middlewares para ambiente de Desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ConfigureApiExceptionHandle();
}
else
{
    // Configurar tratamento de erro para produ��o
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection(); // For�a HTTPS.

// Middlewares de Autentica��o e Autoriza��o
// A ordem � importante: UseAuthentication antes de UseAuthorization.
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers(); // Mapeia as rotas para os controllers.

// =================================================================================================
// EXECU��O DA APLICA��O
// =================================================================================================
app.Run();