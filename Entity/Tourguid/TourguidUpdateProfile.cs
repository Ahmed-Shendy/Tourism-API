﻿using System.ComponentModel.DataAnnotations;

namespace Tourism_Api.Entity.Tourguid;

public class TourguidUpdateProfile
{
    
    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Phone { get; set; }

    
    public List<string> AllLangues { get; set; } = []; 

}
