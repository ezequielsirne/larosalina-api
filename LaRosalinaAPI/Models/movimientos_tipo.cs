using System;
using System.Collections.Generic;

namespace LaRosalinaAPI.Models;

public partial class movimientos_tipo
{
    public int id { get; set; }

    public string descripcion { get; set; }

    public virtual ICollection<movimiento> movimientos { get; set; } = new List<movimiento>();
}
