using System;
using System.Collections.Generic;

namespace LaRosalinaAPI.Models;

public partial class MovimientoDto
{
    public int id { get; set; }

    public DateTime? fecha { get; set; }

    public string descripcion { get; set; }

    public int? id_reserva { get; set; }

    public int? id_gasto { get; set; }

    public string id_transferencia { get; set; }

    public int? tipo { get; set; }

    public int? cuenta { get; set; }

    public decimal? movimiento1 { get; set; }

    public string id_random { get; set; }

    public short? eliminado { get; set; }

    public int? usuario_alta { get; set; }

    public DateTime? f_alta { get; set; }

    public int? usuario_mod { get; set; }

    public DateTime? f_mod { get; set; }

    public int? usuario_baja { get; set; }

    public DateTime? f_baja { get; set; }

    public virtual CuentaDto cuentaDto { get; set; }

    public virtual GastoDto id_gastoNavigation { get; set; }

    public virtual MovimientoTiposDto tipoDto { get; set; }
}
