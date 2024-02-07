using LagerhotellAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace LagerhotellAPI.Controllers;

[Route("api/warehouse-hotels")]
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
    public async Task<IActionResult> AddWarehouseHotel([FromBody] WarehouseHotel warehouseHotel)
    {
        await _warehouseHotelService.AddWarehouseHotel(warehouseHotel);
        return Ok("Warehouse hotel added successfully.");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteWarehouseHotel(string id)
    {
        await _warehouseHotelService.DeleteWarehouseHotel(id);
        return Ok("Warehouse hotel deleted successfully.");
    }

    [HttpPut]
    [Route("modify")]
    public async Task<IActionResult> ModifyWarehouseHotel([FromBody] WarehouseHotel warehouseHotel)
    {
        await _warehouseHotelService.ModifyWarehouseHotel(warehouseHotel);
        return Ok("Warehouse hotel modified successfully.");
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetWarehouseHotelById(string id)
    {
        try
        {
            var warehouseHotel = await _warehouseHotelService.GetWarehouseHotelById(id);
            return Ok(warehouseHotel);
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
            return Ok(warehouseHotels);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}