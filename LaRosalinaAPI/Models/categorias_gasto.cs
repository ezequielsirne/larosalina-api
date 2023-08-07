using System;
using System.Collections.Generic;

namespace LaRosalinaAPI.Models;

public partial class categorias_gasto
{
    public int id { get; set; }

    public string descripcion { get; set; }

    public int? activo { get; set; }

    public int? usuario_alta { get; set; }

    public DateTime? f_alta { get; set; }

    public int? usuario_mod { get; set; }

    public DateTime? f_mod { get; set; }

    public int? usuario_baja { get; set; }

    public DateTime? f_baja { get; set; }

    public virtual ICollection<gasto> gastos { get; set; } = new List<gasto>();
}
