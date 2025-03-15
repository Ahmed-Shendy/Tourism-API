namespace Tourism_Api.Errors
{
    public static class GovernerateErrors
    {
        public static Error EmptyGovernerate =
            new Error("Governerate.EmptyValue", "No Governerate Match this Name", StatusCodes.Status404NotFound);
    }
}
