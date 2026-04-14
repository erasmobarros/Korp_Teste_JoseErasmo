using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EstoqueService.Data;
using EstoqueService.Models;

[Route("api/[controller]")]
[ApiController]
public class ProdutosController : ControllerBase
{
    private readonly EstoqueContext _context;

    public ProdutosController(EstoqueContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Produto>>> GetProdutos() => await _context.Produtos.ToListAsync();

    [HttpPost]
    public async Task<ActionResult<Produto>> PostProduto(Produto produto)
    {
        _context.Produtos.Add(produto);
        await _context.SaveChangesAsync();
        return Ok(produto);
    }

    // Rota especial para baixar estoque que o Faturamento vai usar
    [HttpPut("{id}/baixar")]
    public async Task<IActionResult> BaixarEstoque(int id, [FromBody] int quantidade)
    {
        var produto = await _context.Produtos.FindAsync(id);
        if (produto == null) return NotFound();
        if (produto.Saldo < quantidade) return BadRequest("Saldo insuficiente");

        produto.Saldo -= quantidade;
        await _context.SaveChangesAsync();
        return NoContent();
    }
    [HttpDelete("{id}")]
public async Task<IActionResult> DeleteProduto(int id)
{
    var produto = await _context.Produtos.FindAsync(id);
    if (produto == null) return NotFound();

    _context.Produtos.Remove(produto);
    await _context.SaveChangesAsync();
    return NoContent();
}
}