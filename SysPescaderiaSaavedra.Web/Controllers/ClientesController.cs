using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SysPescaderiaSaavedra.Web.Models;

namespace SysPescaderiaSaavedra.Web.Controllers
{
    public class ClientesController : Controller
    {
        private readonly PescaderiaContext _context;

        public ClientesController(PescaderiaContext context)
        {
            _context = context;
        }

        // ===============================
        // INDEX (MUESTRA TODOS)
        // ===============================
        public async Task<IActionResult> Index()
        {
            var clientes = await _context.Clientes
                .OrderByDescending(c => c.ClienteId)
                .ToListAsync();

            return View(clientes);
        }

        // ===============================
        // CREATE
        // ===============================
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Nombre,Apellido,Identificacion,Telefono,Email,Direccion")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                cliente.FechaRegistro = DateTime.Now;
                cliente.Estado = true;

                _context.Add(cliente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        // ===============================
        // EDIT
        // ===============================
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return NotFound();

            return View(cliente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("ClienteId,Nombre,Apellido,Identificacion,Telefono,Email,Direccion,Estado")] Cliente cliente)
        {
            if (id != cliente.ClienteId)
                return NotFound();

            if (ModelState.IsValid)
            {
                var clienteDb = await _context.Clientes.FindAsync(id);
                if (clienteDb == null)
                    return NotFound();

                clienteDb.Nombre = cliente.Nombre;
                clienteDb.Apellido = cliente.Apellido;
                clienteDb.Identificacion = cliente.Identificacion;
                clienteDb.Telefono = cliente.Telefono;
                clienteDb.Email = cliente.Email;
                clienteDb.Direccion = cliente.Direccion;
                clienteDb.Estado = cliente.Estado;

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(cliente);
        }

        // ===============================
        // TOGGLE ESTADO (POST SEGURO)
        // ===============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleEstado(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente == null)
                return NotFound();

            cliente.Estado = !cliente.Estado;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}