using System;
using System.Collections.Generic;

namespace LaRosalinaAPI.Models;

public partial class WebSuceso
{
    public long Id { get; set; }

    public int? IdUser { get; set; }

    public string Descripcion { get; set; }

    public DateTime? FSuceso { get; set; }
}
