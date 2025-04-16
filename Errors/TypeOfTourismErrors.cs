namespace Tourism_Api.Errors
{
    public static class TypeOfTourismErrors
    {
        public static Error EmptyTypeOfTourism => 
            new Error("TypeOfTourism.EmptyValue", "No Type Of Tourism Match this Name", StatusCodes.Status404NotFound);

        public static Error NotFound =>
            new Error("TypeOfTourism.NotFound", "Not Found", StatusCodes.Status404NotFound);
    }
}
