using System;
using System.Collections.Generic;

namespace Tourism_Api.model;

public partial class ProgramsPhoto
{
    public string ProgramName { get; set; } = null!;

    public string Photo { get; set; } = null!;

    public virtual Program ProgramNameNavigation { get; set; } = null!;
}
