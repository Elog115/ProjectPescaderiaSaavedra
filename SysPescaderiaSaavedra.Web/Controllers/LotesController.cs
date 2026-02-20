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

        // ============================
        // INDEX
        // ============================
        public async Task<IActionResult> Index()
        {
            var lotes = await _context.Lotes
                .Include(l => l.Producto)
                .Include(l => l.Ingreso)
                .OrderByDescending(l => l.LoteId)
                .ToListAsync();

            return View(lotes);
        }

        // ============================
        // CREATE
        // ============================
        public IActionResult Create()
        {
            CargarCombos();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Lote lote)
        {
            DateOnly hoy = DateOnly.FromDateTime(DateTime.Now);

            if (lote.CantidadInicial <= 0)
                ModelState.AddModelError("CantidadInicial", "La cantidad debe ser mayor a 0.");

            if (lote.FechaProduccion.HasValue && lote.FechaVencimiento <= lote.FechaProduccion)
                ModelState.AddModelError("FechaVencimiento", "La fecha de vencimiento debe ser posterior a producción.");

            if (lote.FechaVencimiento < hoy)
                ModelState.AddModelError("FechaVencimiento", "No puedes registrar un lote vencido.");

            if (ModelState.IsValid)
            {
                lote.StockActual = lote.CantidadInicial;
                lote.Estado = true;

                lote.CodigoLote = $"{DateTime.Now:yyyyMMdd}-{lote.ProductoId}-{DateTime.Now:HHmm}";

                var producto = await _context.Productos.FindAsync(lote.ProductoId);
                if (producto != null)
                {
                    producto.StockGlobal += lote.CantidadInicial;
                    _context.Productos.Update(producto);
                }

                _context.Lotes.Add(lote);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            CargarCombos(lote);
            return View(lote);
        }

        // ============================
        // EDIT
        // ============================
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var lote = await _context.Lotes.FindAsync(id);
            if (lote == null) return NotFound();

            CargarCombos(lote);
            return View(lote);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Lote lote)
        {
            if (id != lote.LoteId) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(lote);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            CargarCombos(lote);
            return View(lote);
        }

        // ============================
        // CAMBIAR ESTADO
        // ============================
        public async Task<IActionResult> CambiarEstado(int id)
        {
            var lote = await _context.Lotes.FindAsync(id);
            if (lote == null) return NotFound();

            lote.Estado = !lote.Estado;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // ============================
        // MÉTODO PRIVADO PARA COMBOS
        // ============================
        private void CargarCombos(Lote? lote = null)
        {
            ViewData["ProductoId"] = new SelectList(
                _context.Productos.Where(p => p.Estado == true),
                "ProductoId",
                "Nombre",
                lote?.ProductoId
            );

            ViewData["IngresoId"] = new SelectList(
                _context.IngresoMercaderia
                    .Select(i => new
                    {
                        i.IngresoId,
                        Descripcion = "Ingreso #" + i.IngresoId
                                      + " - "
                                      + i.FechaIngreso.ToString("dd/MM/yyyy")
                    }),
                "IngresoId",
                "Descripcion",
                lote?.IngresoId
            );
        }
    }
}