using System;
using System.Collections.Generic;

namespace SysPescaderiaSaavedra.Web.Models;

public partial class Producto
{
    public int ProductoId { get; set; }

    public int CategoriaId { get; set; }

    public string Nombre { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public string UnidadMedida { get; set; } = null!;

    public decimal PrecioVenta { get; set; }

    public decimal StockGlobal { get; set; }

    public bool PublicadoWeb { get; set; }

    public bool Estado { get; set; }

    public DateTime FechaRegistro { get; set; }

    public virtual Categoria Categoria { get; set; } = null!;

    public virtual ICollection<DetalleVentum> DetalleVenta { get; set; } = new List<DetalleVentum>();

    public virtual ICollection<Lote> Lotes { get; set; } = new List<Lote>();
}
