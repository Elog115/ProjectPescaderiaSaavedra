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
    public class CategoriasController : Controller
    {
        private readonly PescaderiaContext _context;

        public CategoriasController(PescaderiaContext context)
        {
            _context = context;
        }

        // GET: Categorias
        public async Task<IActionResult> Index()
        {
            // CAMBIO: Agregamos el Where para traer solo los true
            return View(await _context.Categorias
                                .Where(c => c.Estado == true)
                                .ToListAsync());
        }

        // GET: Categorias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categorias
                .FirstOrDefaultAsync(m => m.CategoriaId == id);
            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        // GET: Categorias/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categorias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoriaId,Nombre")] Categoria categoria)
        {
            // OJO: El Bind de arriba solo trae "CategoriaId" y "Nombre". 
            // Estado y FechaRegistro vienen vacíos (false y 0001-01-01).

            if (ModelState.IsValid)
            {
                // 1. Forzamos el estado activo
                categoria.Estado = true;

                // 2. Asignamos la fecha de hoy (si no lo hace la BD automáticamente)
                categoria.FechaRegistro = DateTime.Now;

                _context.Add(categoria);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        // GET: Categorias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }
            return View(categoria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // 1. Quitamos Estado y FechaRegistro del Bind. Solo queremos editar el Nombre.
        public async Task<IActionResult> Edit(int id, [Bind("CategoriaId,Nombre")] Categoria categoria)
        {
            if (id != categoria.CategoriaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // 2. BUSCAMOS la versión original en la base de datos
                    var categoriaEnBd = await _context.Categorias.FindAsync(id);

                    if (categoriaEnBd == null)
                    {
                        return NotFound();
                    }

                    // 3. ACTUALIZAMOS SOLO LO QUE PERMITIMOS CAMBIAR
                    categoriaEnBd.Nombre = categoria.Nombre;
                    // Nota: No tocamos ni Estado ni FechaRegistro, así que se conservan los originales.

                    // 4. Guardamos cambios
                    _context.Update(categoriaEnBd);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoriaExists(categoria.CategoriaId))
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
            return View(categoria);
        }

        // GET: Categorias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categorias
                .FirstOrDefaultAsync(m => m.CategoriaId == id);
            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        // POST: Categorias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Categorias == null)
            {
                return Problem("Entity set 'PescaderiaContext.Categorias'  is null.");
            }

            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria != null)
            {
                categoria.Estado = false; // Desactivamos (Borrado Lógico)
                _context.Categorias.Update(categoria); // Marcamos como modificado
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoriaExists(int id)
        {
            return _context.Categorias.Any(e => e.CategoriaId == id);
        }
    }
}
