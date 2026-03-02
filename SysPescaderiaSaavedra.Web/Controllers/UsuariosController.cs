using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SysPescaderiaSaavedra.Web.Models;

namespace SysPescaderiaSaavedra.Web.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly PescaderiaContext _context;

        public UsuariosController(PescaderiaContext context)
        {
            _context = context;
        }

        // ============================
        // INDEX
        // ============================
        public async Task<IActionResult> Index()
        {
            var usuarios = await _context.Usuarios
                .Include(u => u.Rol)
                .OrderByDescending(u => u.UsuarioId)
                .ToListAsync();

            return View(usuarios);
        }

        // ============================
        // CREATE
        // ============================
        public IActionResult Create()
        {
            ViewData["RolId"] = new SelectList(
                _context.Roles.Where(r => r.Estado == true),
                "RolId",
                "Nombre");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                usuario.Estado = true;
                usuario.FechaRegistro = DateTime.Now;

                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["RolId"] = new SelectList(
                _context.Roles.Where(r => r.Estado == true),
                "RolId",
                "Nombre",
                usuario.RolId);

            return View(usuario);
        }

        // ============================
        // EDIT
        // ============================
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
                return NotFound();

            ViewData["RolId"] = new SelectList(
                _context.Roles.Where(r => r.Estado == true),
                "RolId",
                "Nombre",
                usuario.RolId);

            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Usuario usuario)
        {
            if (id != usuario.UsuarioId)
                return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["RolId"] = new SelectList(
                _context.Roles.Where(r => r.Estado == true),
                "RolId",
                "Nombre",
                usuario.RolId);

            return View(usuario);
        }

        // ============================
        // TOGGLE ESTADO (LOGICO)
        // ============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleEstado(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
                return NotFound();

            usuario.Estado = !usuario.Estado;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}