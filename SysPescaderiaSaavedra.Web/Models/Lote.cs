using System;
using System.Collections.Generic;

namespace SysPescaderiaSaavedra.Web.Models;

public partial class Lote
{
    public int LoteId { get; set; }

    public int IngresoId { get; set; }

    public int ProductoId { get; set; }

    public string? CodigoLote { get; set; }

    public decimal CantidadInicial { get; set; }

    public decimal StockActual { get; set; }

    public decimal CostoUnitario { get; set; }

    public DateOnly? FechaProduccion { get; set; }

    public DateOnly FechaVencimiento { get; set; }

    public bool Estado { get; set; }

    public virtual IngresoMercaderia Ingreso { get; set; } = null!;

    public virtual Producto Producto { get; set; } = null!;
}
