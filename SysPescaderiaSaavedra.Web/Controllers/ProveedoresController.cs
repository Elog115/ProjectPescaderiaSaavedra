using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SysPescaderiaSaavedra.Web.Models;

namespace SysPescaderiaSaavedra.Web.Controllers
{
    public class ProveedoresController : Controller
    {
        private readonly PescaderiaContext _context;

        public ProveedoresController(PescaderiaContext context)
        {
            _context = context;
        }

        // ===========================
        // INDEX (Activos e Inactivos)
        // ===========================
        public async Task<IActionResult> Index()
        {
            var proveedores = await _context.Proveedores
                .OrderByDescending(p => p.FechaRegistro)
                .ToListAsync();

            return View(proveedores);
        }

        // ===========================
        // DETAILS
        // ===========================
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var proveedor = await _context.Proveedores
                .FirstOrDefaultAsync(m => m.ProveedorId == id);

            if (proveedor == null) return NotFound();

            return View(proveedor);
        }

        // ===========================
        // CREATE
        // ===========================
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Proveedores proveedor)
        {
            if (ModelState.IsValid)
            {
                proveedor.Estado = true; // Siempre inicia activo
                proveedor.FechaRegistro = DateTime.Now;

                _context.Add(proveedor);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(proveedor);
        }

        // ===========================
        // EDIT
        // ===========================
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var proveedor = await _context.Proveedores.FindAsync(id);

            if (proveedor == null) return NotFound();

            return View(proveedor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Proveedores proveedor)
        {
            if (id != proveedor.ProveedorId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(proveedor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProveedoresExists(proveedor.ProveedorId))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(proveedor);
        }

        // ===========================
        // TOGGLE ESTADO (Activar/Desactivar)
        // ===========================
        public async Task<IActionResult> ToggleEstado(int id)
        {
            var proveedor = await _context.Proveedores.FindAsync(id);

            if (proveedor == null)
                return NotFound();

            proveedor.Estado = !proveedor.Estado;

            _context.Update(proveedor);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // ===========================
        // DELETE LOGICO (Opcional)
        // ===========================
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var proveedor = await _context.Proveedores
                .FirstOrDefaultAsync(m => m.ProveedorId == id);

            if (proveedor == null) return NotFound();

            return View(proveedor);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var proveedor = await _context.Proveedores.FindAsync(id);

            if (proveedor != null)
            {
                // Eliminación lógica
                proveedor.Estado = false;
                _context.Update(proveedor);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ProveedoresExists(int id)
        {
            return _context.Proveedores.Any(e => e.ProveedorId == id);
        }
    }
}