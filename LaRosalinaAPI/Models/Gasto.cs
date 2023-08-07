using System;
using System.Collections.Generic;

namespace LaRosalinaAPI.Models;

public partial class gasto
{
    public int id { get; set; }

    public DateTime? fecha { get; set; }

    public string descripcion { get; set; }

    public int? categoria { get; set; }

    public int? cuenta { get; set; }

    public decimal? importe { get; set; }

    public string observaciones { get; set; }

    public string id_random { get; set; }

    public short? eliminado { get; set; }

    public int? usuario_alta { get; set; }

    public DateTime? f_alta { get; set; }

    public int? usuario_mod { get; set; }

    public DateTime? f_mod { get; set; }

    public int? usuario_baja { get; set; }

    public DateTime? f_baja { get; set; }

    public virtual categorias_gasto categoriaNavigation { get; set; }

    public virtual cuenta cuentaNavigation { get; set; }

    public virtual ICollection<movimiento> movimientos { get; set; } = new List<movimiento>();
}
