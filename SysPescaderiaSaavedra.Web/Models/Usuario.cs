using System;
using System.Collections.Generic;

namespace SysPescaderiaSaavedra.Web.Models;

public partial class Usuario
{
    public int UsuarioId { get; set; }

    public int RolId { get; set; }

    public string NombreUsuario { get; set; } = null!;

    public string ClaveHash { get; set; } = null!;

    public string NombreCompleto { get; set; } = null!;

    public string Email { get; set; } = null!;

    public bool Estado { get; set; }

    public DateTime FechaRegistro { get; set; }

    public virtual ICollection<IngresoMercaderia> IngresoMercaderia { get; set; } = new List<IngresoMercaderia>();

    public virtual Roles Rol { get; set; } = null!;

    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();
}
