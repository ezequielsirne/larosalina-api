using System;
using System.Collections.Generic;

namespace LaRosalinaAPI.Models;

public partial class DepartamentoDto
{
    public int id { get; set; }

    public string descripcion { get; set; }

    public string planta { get; set; }

    public string vista { get; set; }
}
