namespace LagerhotellAPI.Models.FrontendModels;

public class AddWarehouseHotelRequest
{
    public WarehouseHotel WarehouseHotel { get; set; }
    public AddWarehouseHotelRequest(WarehouseHotel warehouseHotel)
    {
        WarehouseHotel = warehouseHotel;
    }
}
