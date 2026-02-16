using Microsoft.AspNetCore.Mvc;
using SysPescaderiaSaavedra.Web.Models;
using System;
using System.Linq;

namespace SysPescaderiaSaavedra.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly PescaderiaContext _context;

        public HomeController(PescaderiaContext context)
        {
            _context = context;
        }

        // Página principal
        public IActionResult Index()
        {
            return View();
        }

        // Dashboard
        public IActionResult Dashboard()
        {
            var hoy = DateTime.Today;
            var hace7Dias = hoy.AddDays(-6);

            // ?? Ventas del día
            decimal ventasHoy = _context.Ventas
                .Where(v => v.FechaVenta >= hoy && v.FechaVenta < hoy.AddDays(1))
                .Sum(v => (decimal?)v.Total) ?? 0;

            // ?? Ventas del mes
            decimal ventasMes = _context.Ventas
                .Where(v => v.FechaVenta.Month == hoy.Month && v.FechaVenta.Year == hoy.Year)
                .Sum(v => (decimal?)v.Total) ?? 0;

            // ?? Productos con stock bajo
            int stockBajo = _context.Productos
                .Count(p => p.StockGlobal <= 5);

            // ?? Ventas últimos 7 días (FORMA CORRECTA)
            var ventasSemana = _context.Ventas
                .Where(v => v.FechaVenta >= hace7Dias)
                .Select(v => new
                {
                    Fecha = v.FechaVenta,
                    v.Total
                })
                .AsEnumerable() // ?? AQUÍ se pasa a memoria
                .GroupBy(v => v.Fecha.Date)
                .Select(g => new
                {
                    fecha = g.Key.ToString("dd/MM"),
                    total = g.Sum(x => x.Total)
                })
                .OrderBy(x => x.fecha)
                .ToList();

            ViewBag.VentasHoy = ventasHoy;
            ViewBag.VentasMes = ventasMes;
            ViewBag.StockBajo = stockBajo;
            ViewBag.VentasSemana = ventasSemana;

            return View();
        }

    }
}
