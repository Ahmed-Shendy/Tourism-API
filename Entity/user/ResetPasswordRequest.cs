using System.ComponentModel.DataAnnotations;

namespace Tourism_Api.model;

public class ResetPasswordRequest
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; }


    public string NewPassword { get; set; }

    [Required(ErrorMessage = "Confirm code is required")]
    [StringLength(6, MinimumLength = 6, ErrorMessage = "Confirm code must be exactly 6 characters long")]
    public string? ConfirmCode { get; set; }

    [Required(ErrorMessage = "Confirm password is required")]
    [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }
}