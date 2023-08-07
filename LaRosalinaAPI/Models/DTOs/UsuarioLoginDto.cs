using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LaRosalinaAPI.Models;

public partial class UsuarioLoginDto
{
    [Required(ErrorMessage = "Tenés que escribir el nick")]
    public string nick { get; set; }
    [Required(ErrorMessage = "Tenés que escribir la contraseña")]
    public string password { get; set; }
}
