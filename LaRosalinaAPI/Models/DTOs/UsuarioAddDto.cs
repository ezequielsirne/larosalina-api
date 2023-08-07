using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LaRosalinaAPI.Models;

public partial class UsuarioAddDto
{
    [Required(ErrorMessage="Tenés que poner el nombre")]
    public string? nombre_apellido { get; set; }
    [Required(ErrorMessage = "Tenés que poner el nick")]
    public string? nick { get; set; }
    [Required(ErrorMessage = "Tenés que poner la contraseña")]
    public string? password { get; set; }
    [Required]
    public short? activo { get; set; }

    public int? usuario_alta { get; set; }

    public DateTime? f_alta { get; set; }
}
