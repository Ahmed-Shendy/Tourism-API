using System.ComponentModel.DataAnnotations;

namespace Tourism_Api.Entity.user;

public class AddTourguidRate
{
    public required string tourguidId { get; set; }
    
    [Range(1, 5)]
    public int rate { get; set; }
}
