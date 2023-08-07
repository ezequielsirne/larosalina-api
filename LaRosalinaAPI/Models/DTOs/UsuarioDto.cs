using System;
using System.Collections.Generic;

namespace LaRosalinaAPI.Models;

public partial class UsuarioDto
{
    public int id { get; set; }

    public string nombre_apellido { get; set; }

    public string nick { get; set; }

    public short? activo { get; set; }

    public int? usuario_alta { get; set; }

    public DateTime? f_alta { get; set; }

    public int? usuario_mod { get; set; }

    public DateTime? f_mod { get; set; }

    public int? usuario_baja { get; set; }

    public DateTime? f_baja { get; set; }

    public DateTime? last_login { get; set; }
}
