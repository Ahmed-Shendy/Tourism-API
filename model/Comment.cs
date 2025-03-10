using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tourism_Api.model;

public partial class Comment
{
    public int Id { get; set; }

    public string Content { get; set; } = null!;

    public string? PlaceName { get; set; }

    [ForeignKey("User")]
    public  string UserId { get; set; }

    public virtual  User User { get; set; }

    public virtual Place? PlaceNameNavigation { get; set; }

}
