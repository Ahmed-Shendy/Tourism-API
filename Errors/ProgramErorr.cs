using Tourism_Api.Abstractions;

namespace Tourism_Api.Errors;

public static class ProgramErorr
{
    public static readonly Error ProgramNotFound =
        new("Program.ProgramNotFound", "No Program was found ", StatusCodes.Status404NotFound);



    public static readonly Error ProgramUnque =
        new("Program.ProgramUnque", "Program name must be unique.", StatusCodes.Status400BadRequest);
}

