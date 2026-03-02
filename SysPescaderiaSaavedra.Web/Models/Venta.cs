using System;
using System.Collections.Generic;

namespace SysPescaderiaSaavedra.Web.Models;

public partial class Venta
{
    public int VentaId { get; set; }

    public int ClienteId { get; set; }

    public int UsuarioId { get; set; }

    public DateTime FechaVenta { get; set; }

    public decimal Subtotal { get; set; }

    public decimal Impuesto { get; set; }

    public decimal Total { get; set; }

    public bool Estado { get; set; }

    public virtual Cliente? Cliente { get; set; }

    public virtual ICollection<DetalleVenta> DetalleVenta { get; set; } = new List<DetalleVenta>();

    public virtual Usuario? Usuario { get; set; }
}
