namespace LagerhotellAPI.Models.DomainModels;

public class Location
{
    public Location(string name)
    {
        Name = name;
    }

    public string Name { get; set; }
}
