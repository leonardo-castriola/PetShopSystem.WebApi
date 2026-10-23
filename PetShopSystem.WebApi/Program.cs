using Microsoft.EntityFrameworkCore;
using PetShopSystem.WebApi.Data;
using PetShopSystem.WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Configurando o Entity Framework com SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrando nosso serviço de negócio para Injeção de Dependência
builder.Services.AddScoped<IBanhoService, BanhoService>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
