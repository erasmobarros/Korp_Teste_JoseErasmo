using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FaturamentoService.Data; // Verifique se o nome da pasta Data está correto
using FaturamentoService.Models;

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

        // GET: api/NotasFiscais
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NotaFiscal>>> GetNotasFiscais()
        {
            return await _context.NotasFiscais.ToListAsync();
        }

        // POST: api/NotasFiscais (Cria a nota como "Aberta")
        [HttpPost]
        public async Task<ActionResult<NotaFiscal>> PostNotaFiscal(NotaFiscal nota)
        {
            nota.Status = "Aberta"; // Garante que começa aberta
            _context.NotasFiscais.Add(nota);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetNotasFiscais), new { id = nota.Id }, nota);
        }

        // POST: api/NotasFiscais/5/imprimir
        [HttpPost("{id}/imprimir")]
        public async Task<IActionResult> Imprimir(int id)
        {
            var nota = await _context.NotasFiscais.FindAsync(id);

            if (nota == null) return NotFound();
            if (nota.Status != "Aberta") return BadRequest("Esta nota já foi fechada.");

            // 1. Muda o status para Fechada
            nota.Status = "Fechada";
            
            // 2. Salva a alteração
            await _context.SaveChangesAsync();

            // Aqui você retornaria o sucesso para o Angular
            return Ok(new { message = "Nota impressa e status atualizado!", status = nota.Status });
        }
    }
}