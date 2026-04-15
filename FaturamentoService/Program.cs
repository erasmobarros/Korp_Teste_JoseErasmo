using Microsoft.EntityFrameworkCore;
using FaturamentoService.Data; 

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<FaturamentoContext>(options =>
    options.UseSqlite("Data Source=faturamento.db")); 

builder.Services.AddCors(options => {
    options.AddPolicy("AllowAngular", policy => {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllers();

var app = builder.Build(); 

app.UseCors("AllowAngular");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<FaturamentoContext>();
    db.Database.EnsureCreated(); 
}

app.Run();


app.Run();