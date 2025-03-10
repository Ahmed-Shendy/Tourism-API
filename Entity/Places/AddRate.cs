using System.ComponentModel.DataAnnotations;

namespace Tourism_Api.Entity.Places;

public class AddRate
{
    [Range(0,5)]
    public decimal Rate { get; set; }
    public string PlaceName { get; set; }
}
