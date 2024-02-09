namespace LagerhotellAPI.Models.FrontendModels;

public class GetAllWarehouseHotelsResponse
{
    public GetAllWarehouseHotelsResponse(List<WarehouseHotel> warehouseHotels)
    {
        WarehouseHotels = warehouseHotels;
    }
    List<WarehouseHotel> WarehouseHotels { get; set; }
}
