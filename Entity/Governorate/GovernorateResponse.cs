namespace Tourism_Api.Entity.Governorate
{
    // public record GovernorateResponse(string Name , string? Photo);

    public class GovernorateResponse
    {
        public string Name { get; set; } = string.Empty;
        public string? Photo { get; set; } = string.Empty;

    }

    public class Governorates
    {
        public List<GovernorateResponse> AllGovernorate { get; set; } = new List<GovernorateResponse>();

    }


}
