namespace LagerhotellAPI.Models.FrontendModels;

public class ModifyWarehouseHotelRequest
{
    public ModifyWarehouseHotelRequest(WarehouseHotel warehouseHotel)
    {
        WarehouseHotel = warehouseHotel;
    }
    WarehouseHotel WarehouseHotel { get; set; }
}
