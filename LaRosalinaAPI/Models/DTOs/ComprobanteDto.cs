using System;
using System.Collections.Generic;

namespace LaRosalinaAPI.Models;

public partial class ComprobanteDto
{
    public int id { get; set; }

    public string id_random { get; set; }

    public string archivo { get; set; }

    public string archivo_corto { get; set; }

    public string extension { get; set; }

    public short? activo { get; set; }

    public int? usuario_alta { get; set; }

    public DateTime? f_alta { get; set; }

    public int? usuario_baja { get; set; }

    public DateTime? f_baja { get; set; }

    public string file { get; set; }
}
