namespace Tourism_Api.Entity.Admin;

public class TransferRequests
{
   public List<TourguidTransferRequest> tourguidTransferRequests { get; set; } = new List<TourguidTransferRequest>();

   public int Count { get; set; } = 0;

}

public class TourguidTransferRequest
{
    public string TourguidId { get; set; } = null!;
    public string TourguidPhoto { get; set; } = null!;
    public string TourguidName { get; set; } = null!;
    public string PlaceName { get; set; } = null!;
    public string MovedPlace { get; set; } = null!;
}