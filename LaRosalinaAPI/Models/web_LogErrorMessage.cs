using System;
using System.Collections.Generic;

namespace LaRosalinaAPI.Models;

public partial class web_LogErrorMessage
{
    public long id { get; set; }

    public string logErrorMessage { get; set; }

    public string querry { get; set; }

    public string origen { get; set; }

    public DateTime? f_error { get; set; }
}
