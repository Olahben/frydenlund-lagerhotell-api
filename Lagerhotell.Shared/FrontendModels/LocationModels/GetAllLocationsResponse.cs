namespace LagerhotellAPI.Models;

public class GetAllLocationsResponse
{
    public GetAllLocationsResponse(List<Location> locations)
    {
        Locations = locations;
    }
    public List<Location> Locations { get; set; }
}
