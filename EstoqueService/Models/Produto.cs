namespace EstoqueService.Models
{
    public class Produto
    {
        
        public int Id { get; set; } // Usaremos o Id como Código
        public required string Descricao { get; set; }
        public int Saldo { get; set; }
    }
}