// FaturamentoService/Data/FaturamentoContext.cs
using Microsoft.EntityFrameworkCore;
using FaturamentoService.Models;

namespace FaturamentoService.Data
{
    public class FaturamentoContext : DbContext
    {
        public FaturamentoContext(DbContextOptions<FaturamentoContext> options) : base(options) { }

        // Isso vai criar as tabelas "NotasFiscais" e "ItensNota"
        public DbSet<NotaFiscal> NotasFiscais { get; set; }
        public DbSet<ItemNotaFiscal> ItensNota { get; set; }
    }
}