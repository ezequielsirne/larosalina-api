using System;
using System.Collections.Generic;

namespace LaRosalinaAPI.Models;

public partial class presupuestos_detalle
{
    public int id { get; set; }

    public int? id_presupuesto { get; set; }

    public string descripcion { get; set; }

    public decimal? importe { get; set; }

    public short? eliminado { get; set; }

    public DateTime? f_alta { get; set; }

    public int? usuario_alta { get; set; }

    public DateTime? f_mod { get; set; }

    public int? usuario_mod { get; set; }

    public DateTime? f_baja { get; set; }

    public int? usuario_baja { get; set; }

    public virtual presupuesto id_presupuestoNavigation { get; set; }
}
