namespace LagerhotellAPI.Models.FrontendModels;

public class ModifyWarehouseHotelRequest
{
    public ModifyWarehouseHotelRequest(WarehouseHotel warehouseHotel)
    {
        WarehouseHotel = warehouseHotel;
    }
    public WarehouseHotel WarehouseHotel { get; set; }
}
