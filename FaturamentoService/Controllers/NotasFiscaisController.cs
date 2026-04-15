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

            if (nota.Itens != null && nota.Itens.Any())
            {
                foreach (var item in nota.Itens)
                {
                    var response = await httpClient.PutAsJsonAsync($"api/produtos/{item.ProdutoId}/baixar", item.Quantidade);
                    response.EnsureSuccessStatusCode(); 
                }
            }

            nota.Status = "Fechada";
            await _context.SaveChangesAsync();

            return Ok(new { message = "Nota impressa e estoque atualizado com sucesso!", status = nota.Status });
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
} 