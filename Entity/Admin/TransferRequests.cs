namespace Tourism_Api.Entity.Admin;

public class TransferRequests
{
   public List<TourguidTransferRequest> tourguidTransfer { get; set; } = new List<TourguidTransferRequest>();
  // public List<TourguidTransTripRequest> TourguidTransferTrips { get; set; } = [];


    public int Count { get; set; } = 0;

}

public class TourguidTransferRequest
{
    public string TourguidId { get; set; } = null!;
    public string? TourguidPhoto { get; set; } = null!;
    public string TourguidName { get; set; } = null!;
    public string Movedto { get; set; } = null!;
}
public class TourguidTransTripRequest
{
    public string TourguidId { get; set; } = null!;
    public string? TourguidPhoto { get; set; } = null!;
    public string TourguidName { get; set; } = null!;
    //public string TripName { get; set; } = null!;
    public string MovedTripe { get; set; } = null!;
}