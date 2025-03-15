namespace Tourism_Api.Errors
{
    public static class TypeOfTourismErrors
    {
        public static Error EmptyTypeOfTourism => 
            new Error("TypeOfTourism.EmptyValue", "No Type Of Tourism Match this Name", StatusCodes.Status404NotFound);
    }
}
