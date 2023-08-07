using System;
using System.Collections.Generic;

namespace LaRosalinaAPI.Models;

public partial class HuespedDto
{
    public int id { get; set; }

    public string nombre_apellido { get; set; }

    public int? dni { get; set; }

    public string domicilio { get; set; }

    public string patente { get; set; }

    public DateTime? checkin { get; set; }

    public DateTime? checkout { get; set; }

    public int? id_reserva { get; set; }

    public short? eliminado { get; set; }

    public int? usuario_alta { get; set; }

    public DateTime? f_alta { get; set; }

    public int? usuario_mod { get; set; }

    public DateTime? f_mod { get; set; }

    public int? usuario_baja { get; set; }

    public DateTime? f_baja { get; set; }

    public virtual ReservaDto? reserva { get; set; }
}
