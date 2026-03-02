namespace SysPescaderiaSaavedra.Web.Models.ViewModels
{
    public class DashboardViewModel
    {
        // KPIs
        public decimal VentasHoy { get; set; }
        public decimal VentasMes { get; set; }
        public int FacturasHoy { get; set; }
        public int ClientesNuevos { get; set; }
        public int StockBajo { get; set; }
        public decimal TicketPromedio { get; set; }

        // Gráficos
        public List<VentasSemanaDto> VentasSemana { get; set; } = new();
        public List<TopProductoDto> TopProductos { get; set; } = new();
    }

    public class VentasSemanaDto
    {
        public string Fecha { get; set; } = "";
        public decimal Total { get; set; }
    }

    public class TopProductoDto
    {
        public string Producto { get; set; } = "";
        public decimal Cantidad { get; set; }
    }
}