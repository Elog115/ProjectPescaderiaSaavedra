using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SysPescaderiaSaavedra.Web.Models;

namespace SysPescaderiaSaavedra.Web.Controllers
{
    public class IngresoMercaderiasController : Controller
    {
        private readonly PescaderiaContext _context;

        public IngresoMercaderiasController(PescaderiaContext context)
        {
            _context = context;
        }

        // GET: IngresoMercaderias
        public async Task<IActionResult> Index()
        {
            var pescaderiaContext = _context.IngresoMercaderia.Include(i => i.Proveedor).Include(i => i.Usuario);
            return View(await pescaderiaContext.ToListAsync());
        }

        // GET: IngresoMercaderias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ingresoMercaderia = await _context.IngresoMercaderia
                .Include(i => i.Proveedor)
                .Include(i => i.Usuario)
                .FirstOrDefaultAsync(m => m.IngresoId == id);
            if (ingresoMercaderia == null)
            {
                return NotFound();
            }

            return View(ingresoMercaderia);
        }

        // GET: IngresoMercaderias/Create
        public IActionResult Create()
        {
            ViewData["ProveedorId"] = new SelectList(_context.Proveedores, "ProveedorId", "ProveedorId");
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "UsuarioId");
            return View();
        }

        // POST: IngresoMercaderias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IngresoId,ProveedorId,UsuarioId,FechaIngreso,TotalCompra")] IngresoMercaderia ingresoMercaderia)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ingresoMercaderia);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProveedorId"] = new SelectList(_context.Proveedores, "ProveedorId", "ProveedorId", ingresoMercaderia.ProveedorId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "UsuarioId", ingresoMercaderia.UsuarioId);
            return View(ingresoMercaderia);
        }

        // GET: IngresoMercaderias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ingresoMercaderia = await _context.IngresoMercaderia.FindAsync(id);
            if (ingresoMercaderia == null)
            {
                return NotFound();
            }
            ViewData["ProveedorId"] = new SelectList(_context.Proveedores, "ProveedorId", "ProveedorId", ingresoMercaderia.ProveedorId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "UsuarioId", ingresoMercaderia.UsuarioId);
            return View(ingresoMercaderia);
        }

        // POST: IngresoMercaderias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IngresoId,ProveedorId,UsuarioId,FechaIngreso,TotalCompra")] IngresoMercaderia ingresoMercaderia)
        {
            if (id != ingresoMercaderia.IngresoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ingresoMercaderia);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IngresoMercaderiaExists(ingresoMercaderia.IngresoId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProveedorId"] = new SelectList(_context.Proveedores, "ProveedorId", "ProveedorId", ingresoMercaderia.ProveedorId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "UsuarioId", ingresoMercaderia.UsuarioId);
            return View(ingresoMercaderia);
        }

        // GET: IngresoMercaderias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ingresoMercaderia = await _context.IngresoMercaderia
                .Include(i => i.Proveedor)
                .Include(i => i.Usuario)
                .FirstOrDefaultAsync(m => m.IngresoId == id);
            if (ingresoMercaderia == null)
            {
                return NotFound();
            }

            return View(ingresoMercaderia);
        }

        // POST: IngresoMercaderias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ingresoMercaderia = await _context.IngresoMercaderia.FindAsync(id);
            if (ingresoMercaderia != null)
            {
                _context.IngresoMercaderia.Remove(ingresoMercaderia);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IngresoMercaderiaExists(int id)
        {
            return _context.IngresoMercaderia.Any(e => e.IngresoId == id);
        }
    }
}
