using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaConsultasUVV.Data;
using SistemaConsultasUVV.Models;
using System.Security.Claims;

namespace SistemaConsultasUVV.Controllers
{
    [Authorize]
    public class ConsultasController : Controller
    {
        private readonly AppDbContext _context;

        public ConsultasController(AppDbContext context)
        {
            _context = context;
        }

        private int GetUsuarioIdLogado()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            return claim != null ? int.Parse(claim.Value) : 0;
        }

        public async Task<IActionResult> Index()
        {
            int usuarioId = GetUsuarioIdLogado();
            var consultas = await _context.Consultas
                .Where(c => c.UsuarioId == usuarioId)
                .ToListAsync();
            return View(consultas);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Consulta consulta)
        {
            consulta.UsuarioId = GetUsuarioIdLogado();
            ModelState.Remove("Usuario");

            if (ModelState.IsValid)
            {
                _context.Add(consulta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(consulta);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            int usuarioId = GetUsuarioIdLogado();
            var consulta = await _context.Consultas
                .FirstOrDefaultAsync(c => c.Id == id && c.UsuarioId == usuarioId);

            if (consulta == null) return NotFound();
            return View(consulta);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Consulta consulta)
        {
            if (id != consulta.Id) return NotFound();

            consulta.UsuarioId = GetUsuarioIdLogado();
            ModelState.Remove("Usuario");

            if (ModelState.IsValid)
            {
                _context.Update(consulta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(consulta);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            int usuarioId = GetUsuarioIdLogado();
            var consulta = await _context.Consultas
                .FirstOrDefaultAsync(c => c.Id == id && c.UsuarioId == usuarioId);

            if (consulta == null) return NotFound();
            return View(consulta);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            int usuarioId = GetUsuarioIdLogado();
            var consulta = await _context.Consultas
                .FirstOrDefaultAsync(c => c.Id == id && c.UsuarioId == usuarioId);

            if (consulta != null)
            {
                _context.Consultas.Remove(consulta);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}