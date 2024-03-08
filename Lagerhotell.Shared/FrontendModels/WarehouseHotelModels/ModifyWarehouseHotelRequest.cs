namespace LagerhotellAPI.Models.FrontendModels;

public class ModifyWarehouseHotelRequest
{
    public ModifyWarehouseHotelRequest(WarehouseHotel warehouseHotel, string oldWarehouseHotelName)
    {
        WarehouseHotel = warehouseHotel;
        OldWarehouseHotelName = oldWarehouseHotelName;
    }
    public WarehouseHotel WarehouseHotel { get; set; }
    public string OldWarehouseHotelName { get; set; }
}
