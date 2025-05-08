namespace Tourism_Api.Entity.Places;

public class ALLPlaces
{
    public string Name { get; set; } 

    public string Photo { get; set; }

    public decimal GoogleRate { get; set; }

       // public string Tourism_Name { get; set; }

}

public class All_Places
{
    public List<ALLPlaces> Places { get; set; } = new List<ALLPlaces>();
}
