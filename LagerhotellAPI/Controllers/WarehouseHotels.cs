using LagerhotellAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Controllers;

[Route("warehouse-hotels")]
[ApiController]
public class WarehouseHotelsController : ControllerBase
{
    private readonly WarehouseHotelService _warehouseHotelService;

    public WarehouseHotelsController(WarehouseHotelService warehouseHotelService)
    {
        _warehouseHotelService = warehouseHotelService;
    }

    [HttpPost]
    [Route("add")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> AddWarehouseHotel([FromBody] AddWarehouseHotelRequest request)
    {
        try
        {
            string warehouseHotelId = await _warehouseHotelService.AddWarehouseHotel(request.WarehouseHotel);
            return Ok(new AddWarehouseHotelResponse(warehouseHotelId));
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"{ex}");
            return Conflict("Warehouse hotel already exists");
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DeleteWarehouseHotel(string id)
    {
        try
        {
            string warehouseHotelId;
            string warehouseName;
            (warehouseHotelId, warehouseName) = await _warehouseHotelService.DeleteWarehouseHotel(id);
            return Ok(new DeleteWarehouseHotelResponse(warehouseHotelId, warehouseName));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPut]
    [Route("modify")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> ModifyWarehouseHotel([FromBody] ModifyWarehouseHotelRequest request)
    {
        try
        {
            await _warehouseHotelService.ModifyWarehouseHotel(request.WarehouseHotel, request.OldWarehouseHotelName);
            return Ok("Warehouse hotel modified successfully.");
        }
        catch (KeyNotFoundException ex)
        {
            Console.WriteLine(ex);
            return NotFound();
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetWarehouseHotelById(string id)
    {
        try
        {
            var warehouseHotel = await _warehouseHotelService.GetWarehouseHotelById(id);
            return Ok(new GetWarehouseHotelByIdResponse(warehouseHotel));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpGet]
    [Route("get-by-name/{name}")]
    public async Task<IActionResult> GetWarehouseHotelByName([FromRoute] string name)
    {
        try
        {
            var warehouseHotel = await _warehouseHotelService.GetWarehouseHotelByName(name);
            return Ok(new GetWarehouseHotelByIdResponse(warehouseHotel));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllWarehouseHotels([FromQuery] int? skip, int? take)
    {
        try
        {
            var warehouseHotels = await _warehouseHotelService.GetAllWarehouseHotels(skip, take);
            return Ok(new GetAllWarehouseHotelsResponse(warehouseHotels));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}