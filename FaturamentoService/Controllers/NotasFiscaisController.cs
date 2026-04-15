using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FaturamentoService.Data; 
using FaturamentoService.Models;
using System.Net.Http.Json;

namespace FaturamentoService.Controllers
{
    [Route("api/notafiscais")]
    [ApiController]
    public class NotasFiscaisController : ControllerBase
    {
        private readonly FaturamentoContext _context;

        public NotasFiscaisController(FaturamentoContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NotaFiscal>>> GetNotasFiscais()
        {
            return await _context.NotasFiscais.Include(n => n.Itens).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<NotaFiscal>> PostNotaFiscal(NotaFiscal nota)
        {
            nota.Status = "Aberta"; 
            _context.NotasFiscais.Add(nota);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetNotasFiscais), new { id = nota.Id }, nota);
        }

        [HttpPost("{id}/imprimir")]
        public async Task<IActionResult> Imprimir(int id)
        {
            var nota = await _context.NotasFiscais
                                     .Include(n => n.Itens) 
                                     .FirstOrDefaultAsync(n => n.Id == id);

            if (nota == null) return NotFound("Nota não encontrada.");
            if (nota.Status != "Aberta") return BadRequest("Esta nota já foi fechada.");

            using var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://localhost:5121/");

            // 🔥 Flag para saber se algum produto zerou
            bool estoqueZerou = false;

            if (nota.Itens != null && nota.Itens.Any())
            {
                foreach (var item in nota.Itens)
                {
                    // 1. Faz a baixa no estoque
                    var response = await httpClient.PutAsJsonAsync($"api/produtos/{item.ProdutoId}/baixar", item.Quantidade);
                    response.EnsureSuccessStatusCode(); 

                    // 2. Lê a resposta do EstoqueService para ver quanto sobrou
                    var produtoAtualizado = await response.Content.ReadFromJsonAsync<ProdutoEstoqueDTO>();
                    
                    // 3. Se sobrou 0, ativamos o alerta!
                    if (produtoAtualizado != null && produtoAtualizado.Saldo == 0)
                    {
                        estoqueZerou = true;
                    }
                }
            }

            nota.Status = "Fechada";
            await _context.SaveChangesAsync();

            // 🔥 Se zerou, mandamos um retorno especial para o Angular!
            if (estoqueZerou)
            {
                return Ok(new { 
                    message = "Nota impressa com sucesso! ATENÇÃO: Um ou mais produtos esgotaram no estoque.", 
                    status = nota.Status, 
                    alertaEstoque = true // O Angular vai ler isso aqui!
                });
            }

            return Ok(new { message = "Nota impressa e estoque atualizado com sucesso!", status = nota.Status, alertaEstoque = false });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotaFiscal(int id)
        {
            var nota = await _context.NotasFiscais
                                     .Include(n => n.Itens)
                                     .FirstOrDefaultAsync(n => n.Id == id);

            if (nota == null) return NotFound();

            if (nota.Itens != null && nota.Itens.Any())
            {
                _context.RemoveRange(nota.Itens);
            }

            _context.NotasFiscais.Remove(nota);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

    // 🔥 Classe auxiliar para ler apenas o Saldo que vem lá do EstoqueService
    public class ProdutoEstoqueDTO
    {
        public int Saldo { get; set; }
    }
}