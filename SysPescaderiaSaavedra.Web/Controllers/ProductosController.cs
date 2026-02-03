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
    public class ProductosController : Controller
    {
        private readonly PescaderiaContext _context;

        public ProductosController(PescaderiaContext context)
        {
            _context = context;
        }

        // GET: Productos
        public async Task<IActionResult> Index()
        {
            // LOGICA: 
            // 1. .Include: Hace un JOIN para traer el nombre de la Categoria
            // 2. .Where: Solo productos activos (Borrado lógico)
            var pescaderiaContext = _context.Productos
                                            .Include(p => p.Categoria)
                                            .Where(p => p.Estado == true);

            return View(await pescaderiaContext.ToListAsync());
        }

        // GET: Productos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(m => m.ProductoId == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // GET: Productos/Create
        public IActionResult Create()
        {
            // LOGICA: Solo permitir elegir categorías que estén activas
            ViewData["CategoriaId"] = new SelectList(
                _context.Categorias.Where(c => c.Estado == true),
                "CategoriaId",
                "Nombre");

            return View();
        }

        // POST: Productos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductoId,Codigo,Nombre,Descripcion,PrecioVenta,Stock,CategoriaId")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                // LOGICA DE NEGOCIO:
                producto.Estado = true;
                producto.FechaRegistro = DateTime.Now;

                // Validación extra: No permitir precios negativos
                if (producto.PrecioVenta < 0)
                {
                    ModelState.AddModelError("PrecioVenta", "El precio no puede ser negativo.");
                }
                else
                {
                    _context.Add(producto);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            // Si algo falla, recargamos la lista de categorías para que no explote la vista
            ViewData["CategoriaId"] = new SelectList(_context.Categorias.Where(c => c.Estado == true), "CategoriaId", "Nombre", producto.CategoriaId);
            return View(producto);
        }

        // GET: Productos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "CategoriaId", producto.CategoriaId);
            return View(producto);
        }

        // POST: Productos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductoId,CategoriaId,Nombre,Descripcion,UnidadMedida,PrecioVenta,StockGlobal,PublicadoWeb,Estado,FechaRegistro")] Producto producto)
        {
            if (id != producto.ProductoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(producto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoExists(producto.ProductoId))
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
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "CategoriaId", producto.CategoriaId);
            return View(producto);
        }

        // GET: Productos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(m => m.ProductoId == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // POST: Productos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto != null)
            {
                // LOGICA: Soft Delete
                producto.Estado = false;
                _context.Update(producto); // Actualizamos estado
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        

        private bool ProductoExists(int id)
        {
            return _context.Productos.Any(e => e.ProductoId == id);
        }
    }
}
