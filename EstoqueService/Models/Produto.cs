// EstoqueService/Models/Produto.cs
namespace EstoqueService.Models
{
    public class Produto
    {
        // O teste pede Código, Descrição e Saldo
        public int Id { get; set; } // Usaremos o Id como Código
        public required string Descricao { get; set; }
        public int Saldo { get; set; }
    }
}