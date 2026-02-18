using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SysPescaderiaSaavedra.Web.Models;

namespace SysPescaderiaSaavedra.Web.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly PescaderiaContext _context;

        public CategoriasController(PescaderiaContext context)
        {
            _context = context;
        }

        // ===================== INDEX =====================
        public async Task<IActionResult> Index()
        {
            return View(await _context.Categorias
                .OrderByDescending(c => c.Estado)
                .ThenBy(c => c.Nombre)
                .ToListAsync());
        }

        // ===================== CREATE =====================
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre")] Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                categoria.Estado = true;
                categoria.FechaRegistro = DateTime.Now;

                _context.Add(categoria);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        // ===================== EDIT =====================
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null) return NotFound();

            return View(categoria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoriaId,Nombre")] Categoria categoria)
        {
            if (id != categoria.CategoriaId) return NotFound();

            if (ModelState.IsValid)
            {
                var categoriaBD = await _context.Categorias.FindAsync(id);
                if (categoriaBD == null) return NotFound();

                categoriaBD.Nombre = categoria.Nombre;

                _context.Update(categoriaBD);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        // ===================== TOGGLE ESTADO =====================
        public async Task<IActionResult> ToggleEstado(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);

            if (categoria == null)
            {
                TempData["Error"] = "La categoría no existe.";
                return RedirectToAction(nameof(Index));
            }

            categoria.Estado = !categoria.Estado;
            _context.Categorias.Update(categoria);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}