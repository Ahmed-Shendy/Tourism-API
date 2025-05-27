using Tourism_Api.model;

namespace Tourism_Api.Entity.user;

public class UserRequest
{

    public string Name { get; set; }

    
    public string Email { get; set; }

    public string Password { get; set; }
    

    public string? Country { get; set; }

    public string? Phone { get; set; }
    
    public string? language { get; set; } = null;


    public DateTime? BirthDate { get; set; } = null!;



    public string? Gender { get; set; }

}
