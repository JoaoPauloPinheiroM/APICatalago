using APICatalago.Context;
using APICatalago.Extensions;
using APICatalago.Filters;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(
    options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string? mysqlConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
                                options.UseMySql(mysqlConnectionString,
                                ServerVersion.AutoDetect(mysqlConnectionString)));

builder.Services.AddScoped<ApiLogginFilter>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ConfigureApiExceptionHandle(); // estamos mostrando a pilha por que estamos no ambiente de desenvolvimento
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();