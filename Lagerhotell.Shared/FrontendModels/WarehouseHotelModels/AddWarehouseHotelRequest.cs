namespace LagerhotellAPI.Models.FrontendModels;

public class AddWarehouseHotelRequest
{
    public WarehouseHotel WarehouseHotel { get; set; }
    public List<byte[]> Images { get; set; }
    public AddWarehouseHotelRequest(WarehouseHotel warehouseHotel, List<byte[]> images)
    {
        WarehouseHotel = warehouseHotel;
        Images = images;
    }
}
