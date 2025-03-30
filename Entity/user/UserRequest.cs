using Tourism_Api.model;

namespace Tourism_Api.Entity.user;

public class UserRequest
{

    public string Name { get; set; }

    
    public string Email { get; set; }

    public string Password { get; set; }
    
    public string? Photo { get; set; }


    public string? Country { get; set; }

    public string? Phone { get; set; }

    public DateOnly? BirthDate { get; set; }

    public string? Gender { get; set; }

}
