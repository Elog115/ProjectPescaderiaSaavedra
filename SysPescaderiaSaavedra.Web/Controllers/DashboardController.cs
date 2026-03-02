using Microsoft.AspNetCore.Mvc;
using SysPescaderiaSaavedra.Web.Models;
using SysPescaderiaSaavedra.Web.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace SysPescaderiaSaavedra.Web.Controllers
{
    public class DashboardController : Controller
    {
        private readonly PescaderiaContext _context;

        public DashboardController(PescaderiaContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var hoy = DateTime.Today;
            var inicioMes = new DateTime(hoy.Year, hoy.Month, 1);
            var fechaInicio = DateTime.Now.AddDays(-7);

            var model = new DashboardViewModel();

            // ===== KPIs =====
            model.VentasHoy = _context.Ventas
                .Where(v => v.FechaVenta.Date == hoy)
                .Sum(v => (decimal?)v.Total) ?? 0;

            model.VentasMes = _context.Ventas
                .Where(v => v.FechaVenta >= inicioMes)
                .Sum(v => (decimal?)v.Total) ?? 0;

            model.FacturasHoy = _context.Ventas
                .Count(v => v.FechaVenta.Date == hoy);

            model.ClientesNuevos = _context.Clientes
                .Count(c => c.FechaRegistro >= inicioMes);

            model.StockBajo = _context.Productos
                .Count(p => p.StockGlobal <= 5);

            model.TicketPromedio = model.FacturasHoy > 0
                ? model.VentasHoy / model.FacturasHoy
                : 0;

            // ===== Ventas últimos 7 días =====
            model.VentasSemana = _context.Ventas
                .Where(v => v.FechaVenta >= fechaInicio)
                .ToList()
                .GroupBy(v => v.FechaVenta.Date)
                .Select(g => new VentasSemanaDto
                {
                    Fecha = g.Key.ToString("dd/MM"),
                    Total = g.Sum(x => x.Total)
                })
                .OrderBy(x => x.Fecha)
                .ToList();

            // ===== Top Productos =====
            model.TopProductos = _context.DetalleVenta
                .Include(d => d.Producto)
                .Where(d => d.Producto != null)
                .GroupBy(d => new { d.ProductoId, d.Producto!.Nombre })
                .Select(g => new TopProductoDto
                {
                    Producto = g.Key.Nombre,
                    Cantidad = g.Sum(x => x.Cantidad)
                })
                .OrderByDescending(x => x.Cantidad)
                .Take(5)
                .ToList();

            return View(model);
        }
    }
}