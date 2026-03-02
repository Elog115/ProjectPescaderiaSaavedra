using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SysPescaderiaSaavedra.Web.Models;

namespace SysPescaderiaSaavedra.Web.Controllers
{
    public class VentasController : Controller
    {
        private readonly PescaderiaContext _context;

        public VentasController(PescaderiaContext context)
        {
            _context = context;
        }

        // =========================
        // GET: Ventas
        // =========================
        public async Task<IActionResult> Index()
        {
            var ventas = await _context.Ventas
                .Include(v => v.Cliente)
                .Include(v => v.Usuario)
                .ToListAsync();

            return View(ventas);
        }

        // =========================
        // GET: Ventas/Create
        // =========================
        public IActionResult Create()
        {
            CargarCombos();
            return View(new Venta());
        }

        // =========================
        // POST: Ventas/Create
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("ClienteId,UsuarioId,Subtotal,Impuesto,Total")] Venta venta)
        {
            if (ModelState.IsValid)
            {
                venta.FechaVenta = DateTime.Now;
                venta.Estado = true;

                _context.Add(venta);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            CargarCombos(venta.ClienteId, venta.UsuarioId);
            return View(venta);
        }

        // =========================
        // GET: Ventas/Edit
        // =========================
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var venta = await _context.Ventas.FindAsync(id);

            if (venta == null)
                return NotFound();

            CargarCombos(venta.ClienteId, venta.UsuarioId);
            return View(venta);
        }

        // =========================
        // POST: Ventas/Edit
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("VentaId,ClienteId,UsuarioId,FechaVenta,Subtotal,Impuesto,Total,Estado")] Venta venta)
        {
            if (id != venta.VentaId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(venta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VentaExists(venta.VentaId))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            CargarCombos(venta.ClienteId, venta.UsuarioId);
            return View(venta);
        }

        // =========================
        // TOGGLE ESTADO
        // =========================
        public async Task<IActionResult> ToggleEstado(int id)
        {
            var venta = await _context.Ventas.FindAsync(id);

            if (venta == null)
                return NotFound();

            venta.Estado = !venta.Estado;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // =========================
        // MÉTODO PARA CARGAR COMBOS
        // =========================
        private void CargarCombos(int? clienteId = null, int? usuarioId = null)
        {
            ViewData["ClienteId"] = new SelectList(
                _context.Clientes,
                "ClienteId",
                "Nombre",
                clienteId
            );

            ViewData["UsuarioId"] = new SelectList(
                _context.Usuarios,
                "UsuarioId",
                "NombreCompleto", // 👈 CORRECTO
                usuarioId
            );
        }

        // =========================
        // VALIDACIÓN
        // =========================
        private bool VentaExists(int id)
        {
            return _context.Ventas.Any(e => e.VentaId == id);
        }
    }
}