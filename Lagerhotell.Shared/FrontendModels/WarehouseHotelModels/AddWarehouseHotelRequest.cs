namespace LagerhotelAPI.Models.FrontendModels;

public class AddWarehouseHotelRequest
{
    WarehouseHotel WarehouseHotel { get; set; }
    public AddWarehouseHotelRequest(WarehouseHotel warehouseHotel)
    {
        WarehouseHotel = warehouseHotel;
    }
}
