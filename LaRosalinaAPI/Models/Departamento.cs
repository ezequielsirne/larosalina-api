using System;
using System.Collections.Generic;

namespace LaRosalinaAPI.Models;

public partial class departamento
{
    public int id { get; set; }

    public string descripcion { get; set; }

    public string planta { get; set; }

    public string vista { get; set; }

    public short? activo { get; set; }

    public virtual ICollection<reserva> reservas { get; set; } = new List<reserva>();
}
