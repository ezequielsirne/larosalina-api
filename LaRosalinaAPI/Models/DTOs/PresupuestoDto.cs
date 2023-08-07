using System;
using System.Collections.Generic;

namespace LaRosalinaAPI.Models;

public partial class PresupuestoDto
{
    public int id { get; set; }

    public DateTime? fecha { get; set; }

    public string nombre { get; set; }

    public string cuit { get; set; }

    public string domicilio { get; set; }

    public string telefono { get; set; }

    public string email { get; set; }

    public string forma_de_pago { get; set; }

    public decimal? total { get; set; }

    public short? eliminado { get; set; }

    public DateTime? f_alta { get; set; }

    public int? usuario_alta { get; set; }

    public DateTime? f_mod { get; set; }

    public int? usuario_mod { get; set; }

    public DateTime? f_baja { get; set; }

    public int? usuario_baja { get; set; }

    public virtual ICollection<PresupuestoDetalleDto> presupuestos_detalles { get; set; } = new List<PresupuestoDetalleDto>();
}
