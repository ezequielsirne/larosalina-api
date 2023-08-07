using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LaRosalinaAPI.Models;

public partial class UsuarioUpdateDto
{
    [Required]
    public int id { get; set; }

    [Required(ErrorMessage = "Tenés que poner el nombre")]
    public string? nombre_apellido { get; set; }
    [Required(ErrorMessage = "Tenés que poner el nick")]
    public string? nick { get; set; }
    [Required]
    public short? activo { get; set; }
    [Required]

    public int? usuario_mod { get; set; }
    [Required]
    public DateTime? f_mod { get; set; }
}
