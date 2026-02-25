using System;
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

        // ============================
        // INDEX
        // ============================
        public async Task<IActionResult> Index()
        {
            var ingresos = _context.IngresoMercaderia
                .Include(i => i.Proveedor)
                .Include(i => i.Usuario);

            return View(await ingresos.ToListAsync());
        }

        // ============================
        // DETAILS
        // ============================
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var ingreso = await _context.IngresoMercaderia
                .Include(i => i.Proveedor)
                .Include(i => i.Usuario)
                .FirstOrDefaultAsync(m => m.IngresoId == id);

            if (ingreso == null) return NotFound();

            return View(ingreso);
        }

        // ============================
        // CREATE GET
        // ============================
        public IActionResult Create()
        {
            ViewData["ProveedorId"] = new SelectList(
                _context.Proveedores.Where(p => p.Estado),
                "ProveedorId",
                "NombreEmpresa");

            ViewData["UsuarioId"] = new SelectList(
                _context.Usuarios,
                "UsuarioId",
                "NombreCompleto");

            return View();
        }

        // ============================
        // CREATE POST
        // ============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IngresoId,ProveedorId,UsuarioId,FechaIngreso,TotalCompra,Estado")] IngresoMercaderia ingreso)
        {
            if (ModelState.IsValid)
            {
                //////////ingreso.Estado = true;
                ingreso.FechaIngreso = DateTime.Now;

                _context.Add(ingreso);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["ProveedorId"] = new SelectList(
                _context.Proveedores.Where(p => p.Estado),
                "ProveedorId",
                "NombreEmpresa",
                ingreso.ProveedorId);

            ViewData["UsuarioId"] = new SelectList(
                _context.Usuarios,
                "UsuarioId",
                "NombreCompleto",
                ingreso.UsuarioId);

            return View(ingreso);
        }

        // ============================
        // EDIT GET
        // ============================
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var ingreso = await _context.IngresoMercaderia.FindAsync(id);
            if (ingreso == null) return NotFound();

            ViewData["ProveedorId"] = new SelectList(
                _context.Proveedores,
                "ProveedorId",
                "NombreEmpresa",
                ingreso.ProveedorId);

            ViewData["UsuarioId"] = new SelectList(
                _context.Usuarios,
                "UsuarioId",
                "NombreCompleto",
                ingreso.UsuarioId);

            return View(ingreso);
        }

        // ============================
        // EDIT POST
        // ============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IngresoId,ProveedorId,UsuarioId,FechaIngreso,TotalCompra,Estado")] IngresoMercaderia ingreso)
        {
            if (id != ingreso.IngresoId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ingreso);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IngresoMercaderiaExists(ingreso.IngresoId))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["ProveedorId"] = new SelectList(
                _context.Proveedores,
                "ProveedorId",
                "NombreEmpresa",
                ingreso.ProveedorId);

            ViewData["UsuarioId"] = new SelectList(
                _context.Usuarios,
                "UsuarioId",
                "NombreCompleto",
                ingreso.UsuarioId);

            return View(ingreso);
        }

        // ============================
        // TOGGLE ESTADO
        // ============================
        public async Task<IActionResult> ToggleEstado(int id)
        {
            var ingreso = await _context.IngresoMercaderia.FindAsync(id);

            if (ingreso == null)
                return NotFound();

          //////////// ingreso.Estado = !ingreso.Estado;

            _context.Update(ingreso);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // ============================
        // VALIDACIÓN EXISTENCIA
        // ============================
        private bool IngresoMercaderiaExists(int id)
        {
            return _context.IngresoMercaderia.Any(e => e.IngresoId == id);
        }
    }
}