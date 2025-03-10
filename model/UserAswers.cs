using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace Tourism_Api.model;

public class UserAswers
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public bool Question1 { get; set; }
    public bool Question2 { get; set; }
    public bool Question3 { get; set; }
    public bool Question4 { get; set; }
    public bool Question5 { get; set; }
    public bool Question6 { get; set; }
    public bool Question7 { get; set; }
    public bool Question8 { get; set; }
    public bool Question9 { get; set; }
    public bool Question10 { get; set; }

    [ForeignKey("User")]
    public string UserId { get; set; }
    public User User { get; set; }

    [ForeignKey("Program")]
    public string ProgramName { get; set; }
    public Program Program { get; set; }



}
