namespace LagerhotellAPI.Models.FrontendModels;

public class AddOrderRequest
{
    public required Order Order { get; set; }
}
public class AddOrderResponse
{
    public AddOrderResponse(string id)
    {
        OrderId = id;
    }
    public string OrderId { get; set; }

}
