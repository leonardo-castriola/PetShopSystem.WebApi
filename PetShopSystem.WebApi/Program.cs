using Microsoft.EntityFrameworkCore;
using PetShopSystem.WebApi.Data;
using PetShopSystem.WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. CONFIGURAÇÃO DOS SERVIÇOS (Container de DI)

builder.Services.AddControllers();

// Configura o suporte aos endpoints e geração do Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurando o Entity Framework com SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("A connection string named 'DefaultConnection' is required.");
    options.UseSqlite(connectionString);
});

// Registrando nosso serviço de negócio para Injeção de Dependência
builder.Services.AddScoped<IBanhoService, BanhoService>();


var app = builder.Build();

// Aplica migrations pendentes antes de aceitar requisições.
await using (var scope = app.Services.CreateAsyncScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await dbContext.Database.MigrateAsync();
}


// 2. CONFIGURAÇÃO DO PIPELINE DE REQUISIÇÕES (Middlewares)

// Ativa o Swagger visual apenas em ambiente de Desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // Gera a interface gráfica (/swagger/index.html)
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
