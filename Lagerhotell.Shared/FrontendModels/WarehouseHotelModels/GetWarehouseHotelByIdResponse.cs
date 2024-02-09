namespace LagerhotellAPI.Models.FrontendModels;

public class GetWarehouseHotelByIdResponse
{
    public GetWarehouseHotelByIdResponse(WarehouseHotel warehouseHotel)
    {
        WarehouseHotel = warehouseHotel;
    }
    WarehouseHotel WarehouseHotel { get; set; }
}
