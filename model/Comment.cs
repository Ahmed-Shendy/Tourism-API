using System;
using System.Collections.Generic;

namespace Tourism_Api.model;

public partial class Comment
{
    public int Id { get; set; }

    public string Content { get; set; } = null!;

    public string? PlaceName { get; set; }

    public virtual Place? PlaceNameNavigation { get; set; }
}
