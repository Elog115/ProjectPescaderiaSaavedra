using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SysPescaderiaSaavedra.Web.Models;

namespace SysPescaderiaSaavedra.Web.Controllers
{
    public class RolesController : Controller
    {
        private readonly PescaderiaContext _context;

        public RolesController(PescaderiaContext context)
        {
            _context = context;
        }

        // =========================
        // GET: Roles
        // =========================
        public async Task<IActionResult> Index(string filtro)
        {
            var roles = from r in _context.Roles
                        select r;

            switch (filtro)
            {
                case "activos":
                    roles = roles.Where(r => r.Estado == true);
                    break;

                case "inactivos":
                    roles = roles.Where(r => r.Estado == false);
                    break;
            }

            ViewData["FiltroActual"] = filtro;

            return View(await roles
                .OrderByDescending(r => r.RolId)
                .ToListAsync());
        }

        // =========================
        // TOGGLE ESTADO (DESDE INDEX)
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleEstado(int id)
        {
            var rol = await _context.Roles.FindAsync(id);

            if (rol == null)
                return NotFound();

            rol.Estado = !rol.Estado; // Cambia el estado

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // =========================
        // GET: Roles/Create
        // =========================
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RolId,Nombre,Descripcion,Estado")] Roles roles)
        {
            if (ModelState.IsValid)
            {
                roles.FechaRegistro = DateTime.Now;
                _context.Add(roles);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(roles);
        }

        // =========================
        // GET: Roles/Edit
        // =========================
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var rol = await _context.Roles.FindAsync(id);

            if (rol == null)
                return NotFound();

            return View(rol);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Roles roles)
        {
            if (id != roles.RolId)
                return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(roles);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(roles);
        }
    }
}