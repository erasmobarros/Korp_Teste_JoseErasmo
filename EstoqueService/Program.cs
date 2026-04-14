using Microsoft.EntityFrameworkCore;
using EstoqueService.Data; 

var builder = WebApplication.CreateBuilder(args);

// 1. Configura o Banco de Dados (SQLite)
builder.Services.AddDbContext<EstoqueContext>(options =>
    options.UseSqlite("Data Source=estoque.db"));

// 2. Configura o CORS para o Angular (Porta 4200)
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAngular", policy => {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllers();

var app = builder.Build();

// 3. Ativa o CORS antes de mapear os controllers
app.UseCors("AllowAngular");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();