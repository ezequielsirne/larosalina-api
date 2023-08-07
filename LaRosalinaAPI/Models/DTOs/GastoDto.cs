using System;
using System.Collections.Generic;

namespace LaRosalinaAPI.Models;

public partial class GastoDto
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

    public virtual CategoriaGastosDto categoriaDto { get; set; }

    public virtual CuentaDto cuentaDto { get; set; }

    public virtual ICollection<MovimientoDto> movimientos { get; set; } = new List<MovimientoDto>();

    public virtual UsuarioDto responsable { get; set; }
}
