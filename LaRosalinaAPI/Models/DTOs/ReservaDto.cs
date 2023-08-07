using System;
using System.Collections.Generic;

namespace LaRosalinaAPI.Models;

public partial class ReservaDto
{
    public int id { get; set; }

    public DateTime? checkin { get; set; }

    public DateTime? checkout { get; set; }

    public int? departamento { get; set; }

    public short? late_checkout { get; set; }

    public int? noches { get; set; }

    public string nombre_apellido { get; set; }

    public string telefono { get; set; }

    public string mail { get; set; }

    public string localidad { get; set; }

    public int? adultos { get; set; }

    public int? menores { get; set; }

    public int? mascotas { get; set; }

    public string observaciones { get; set; }

    public decimal? total { get; set; }

    public decimal? saldo { get; set; }

    public int? estado { get; set; }

    public string estadoDescripcion { get; set; }

    public short? eliminado { get; set; }

    public int? usuario_alta { get; set; }

    public DateTime? f_alta { get; set; }

    public int? usuario_mod { get; set; }

    public DateTime? f_mod { get; set; }

    public int? usuario_baja { get; set; }

    public DateTime? f_baja { get; set; }

    public virtual DepartamentoDto departamentoDto { get; set; }

    public virtual EstadoReservaDto estadoDto { get; set; }

    public virtual ICollection<huespede> huespedes { get; set; } = new List<huespede>();

    public virtual ICollection<movimiento> movimientos { get; set; } = new List<movimiento>();
}
