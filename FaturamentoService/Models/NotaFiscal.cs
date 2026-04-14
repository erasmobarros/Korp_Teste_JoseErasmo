public class NotaFiscal {
    public int Id { get; set; } // O ID do banco já garante a numeração sequencial
    public string ClienteNome { get; set; } = "";
    public string Status { get; set; } = "Aberta"; // Inicia como Aberta
    public List<ItemNotaFiscal> Itens { get; set; } = new();
}

public class ItemNotaFiscal {
    public int Id { get; set; }
    public int ProdutoId { get; set; }
    public string Descricao { get; set; } = "";
    public int Quantidade { get; set; }
}