using System;
using System.Collections.Generic;

namespace LaRosalinaAPI.Models;

public partial class web_suceso
{
    public long id { get; set; }

    public int? id_user { get; set; }

    public string descripcion { get; set; }

    public DateTime? f_suceso { get; set; }
}
