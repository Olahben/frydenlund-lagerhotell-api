namespace LagerhotellAPI.Models.FrontendModels;

public class CreateStorageUnitRequest
{
    public required StorageUnit StorageUnit { get; set; }
    public required string LinkedWarehouseHotelName { get; set; }
}

public class CreateStorageUnitResponse
{
    public CreateStorageUnitResponse(string id)
    {
        StorageUnitId = id;
    }
    public string StorageUnitId { get; set; }
}
