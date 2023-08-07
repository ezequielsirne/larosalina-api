using System;
using System.Collections.Generic;

namespace LaRosalinaAPI.Models;

public partial class WebLogErrorMessage
{
    public long Id { get; set; }

    public string LogErrorMessage { get; set; }

    public string Querry { get; set; }

    public string Origen { get; set; }

    public DateTime? FError { get; set; }
}
