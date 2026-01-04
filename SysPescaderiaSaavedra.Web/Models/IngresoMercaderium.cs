using System;
using System.Collections.Generic;

namespace SysPescaderiaSaavedra.Web.Models;

public partial class IngresoMercaderium
{
    public int IngresoId { get; set; }

    public int ProveedorId { get; set; }

    public int UsuarioId { get; set; }

    public DateTime FechaIngreso { get; set; }

    public decimal TotalCompra { get; set; }

    public virtual ICollection<Lote> Lotes { get; set; } = new List<Lote>();

    public virtual Proveedore Proveedor { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
