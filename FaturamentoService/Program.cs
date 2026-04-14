using Microsoft.EntityFrameworkCore;
using FaturamentoService.Data; // Importa o seu FaturamentoContext

var builder = WebApplication.CreateBuilder(args);

// 👇 1. LIGA O BANCO DE DADOS (Isso mata o Erro 500) 👇
builder.Services.AddDbContext<FaturamentoContext>(options =>
    options.UseSqlite("Data Source=faturamento.db")); 

// 2. Configura o CORS (Permite que o Angular acesse este serviço)
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAngular", policy => {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllers();

// AQUI É ONDE DAVA O ERRO: Só pode existir UM 'var app'
var app = builder.Build(); 

// 3. Ativa o CORS (Obrigatório estar antes de MapControllers)
app.UseCors("AllowAngular");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
// ... 
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<FaturamentoContext>();
    db.Database.EnsureCreated(); 
}

app.Run();


app.Run();