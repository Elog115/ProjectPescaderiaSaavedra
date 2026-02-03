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
    public class LotesController : Controller
    {
        private readonly PescaderiaContext _context;

        public LotesController(PescaderiaContext context)
        {
            _context = context;
        }

        // GET: Lotes
        public async Task<IActionResult> Index()
        {
            var pescaderiaContext = _context.Lotes.Include(l => l.Ingreso).Include(l => l.Producto);
            return View(await pescaderiaContext.ToListAsync());
        }

        // GET: Lotes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lote = await _context.Lotes
                .Include(l => l.Ingreso)
                .Include(l => l.Producto)
                .FirstOrDefaultAsync(m => m.LoteId == id);
            if (lote == null)
            {
                return NotFound();
            }

            return View(lote);
        }

        // GET: Lotes/Create
        public IActionResult Create()
        {
            ViewData["IngresoId"] = new SelectList(_context.IngresoMercaderia, "IngresoId", "IngresoId");
            ViewData["ProductoId"] = new SelectList(_context.Productos, "ProductoId", "ProductoId");
            return View();
        }

        // POST: Lotes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IngresoId,ProductoId,CantidadInicial,CostoUnitario,FechaProduccion,FechaVencimiento")] Lote lote)
        {
            // --- 1. AJUSTE DE FECHAS (DateOnly vs DateTime) ---
            // Obtenemos la fecha de hoy sin la hora, para poder comparar con tus campos DateOnly
            DateOnly fechaHoy = DateOnly.FromDateTime(DateTime.Now);

            // Validación A: Fecha Vencimiento vs Fecha Producción
            // (Solo validamos si FechaProduccion tiene valor, ya que es nullable en tu modelo)
            if (lote.FechaProduccion.HasValue && lote.FechaVencimiento <= lote.FechaProduccion.Value)
            {
                ModelState.AddModelError("FechaVencimiento", "La fecha de vencimiento debe ser posterior a la producción.");
            }

            // Validación B: No permitir registrar lotes ya vencidos hoy
            if (lote.FechaVencimiento < fechaHoy)
            {
                ModelState.AddModelError("FechaVencimiento", "No puedes registrar un lote que ya está vencido.");
            }

            // Validación C: Cantidad positiva
            if (lote.CantidadInicial <= 0)
            {
                ModelState.AddModelError("CantidadInicial", "La cantidad inicial debe ser mayor a 0.");
            }

            if (ModelState.IsValid)
            {
                // --- 2. LOGICA AUTOMÁTICA ---

                lote.Estado = true;
                lote.StockActual = lote.CantidadInicial;

                // Generamos Código: AÑO-MES-DIA - ID_PROD - HORA
                // Nota: DateTime.Now se usa aquí solo para texto, eso sí está permitido
                lote.CodigoLote = $"{DateTime.Now:yyyyMMdd}-{lote.ProductoId}-{DateTime.Now:HHmm}";

                // --- 3. ACTUALIZAR STOCK (SOLUCIÓN AL ERROR DEL DECIMAL) ---
                var producto = await _context.Productos.FindAsync(lote.ProductoId);

                if (producto != null)
                {
                    // CORRECCIÓN: Como StockGlobal no acepta nulos, sumamos directamente.
                    producto.StockGlobal += lote.CantidadInicial;

                    _context.Productos.Update(producto);
                }

                // --- 4. GUARDAR ---
                _context.Add(lote);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Recargar listas si falla
            ViewData["IngresoId"] = new SelectList(_context.IngresoMercaderia, "IngresoId", "IngresoId", lote.IngresoId);
            // Nota: Agregué .Where(p => p.Estado == true) para que solo salgan productos activos
            ViewData["ProductoId"] = new SelectList(_context.Productos.Where(p => p.Estado == true), "ProductoId", "Nombre", lote.ProductoId);

            return View(lote);
        }

        // GET: Lotes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lote = await _context.Lotes.FindAsync(id);
            if (lote == null)
            {
                return NotFound();
            }
            ViewData["IngresoId"] = new SelectList(_context.IngresoMercaderia, "IngresoId", "IngresoId", lote.IngresoId);
            ViewData["ProductoId"] = new SelectList(_context.Productos, "ProductoId", "ProductoId", lote.ProductoId);
            return View(lote);
        }

        // POST: Lotes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LoteId,IngresoId,ProductoId,CodigoLote,CantidadInicial,StockActual,CostoUnitario,FechaProduccion,FechaVencimiento,Estado")] Lote lote)
        {
            if (id != lote.LoteId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lote);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoteExists(lote.LoteId))
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
            ViewData["IngresoId"] = new SelectList(_context.IngresoMercaderia, "IngresoId", "IngresoId", lote.IngresoId);
            ViewData["ProductoId"] = new SelectList(_context.Productos, "ProductoId", "ProductoId", lote.ProductoId);
            return View(lote);
        }

        // GET: Lotes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lote = await _context.Lotes
                .Include(l => l.Ingreso)
                .Include(l => l.Producto)
                .FirstOrDefaultAsync(m => m.LoteId == id);
            if (lote == null)
            {
                return NotFound();
            }

            return View(lote);
        }

        // POST: Lotes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lote = await _context.Lotes.FindAsync(id);
            if (lote != null)
            {
                _context.Lotes.Remove(lote);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LoteExists(int id)
        {
            return _context.Lotes.Any(e => e.LoteId == id);
        }
    }
}
