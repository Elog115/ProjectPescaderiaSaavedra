using System;
using System.Collections.Generic;

namespace SysPescaderiaSaavedra.Web.Models;

public partial class DetalleVentum
{
    public int DetalleVentaId { get; set; }

    public int VentaId { get; set; }

    public int ProductoId { get; set; }

    public decimal Cantidad { get; set; }

    public decimal PrecioVentaUnitario { get; set; }

    public decimal Subtotal { get; set; }

    public virtual Producto Producto { get; set; } = null!;

    public virtual Venta Venta { get; set; } = null!;
}
