using System;
using System.Collections.Generic;

namespace SysPescaderiaSaavedra.Web.Models;

public partial class Proveedore
{
    public int ProveedorId { get; set; }

    public string NombreEmpresa { get; set; } = null!;

    public string Contacto { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public string Nit { get; set; } = null!;

    public bool Estado { get; set; }

    public DateTime FechaRegistro { get; set; }

    public virtual ICollection<IngresoMercaderium> IngresoMercaderia { get; set; } = new List<IngresoMercaderium>();
}
