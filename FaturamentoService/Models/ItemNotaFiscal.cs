// FaturamentoService/Models/ItemNotaFiscal.cs
namespace FaturamentoService.Models
{
    public class ItemNotaFiscal
    {
        public int Id { get; set; }
        public int ProdutoId { get; set; } 
        public int Quantidade { get; set; }
        
        // Esta linha liga o item à nota no banco de dados
        public int NotaFiscalId { get; set; }
    }
}