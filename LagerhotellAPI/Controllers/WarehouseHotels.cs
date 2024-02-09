using LagerhotelAPI.Models.FrontendModels;
using LagerhotellAPI.Services;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> AddWarehouseHotel([FromBody] AddWarehouseHotelRequest request)
    {
        await _warehouseHotelService.AddWarehouseHotel(request.WarehouseHotel);
        return Ok("Warehouse hotel added successfully.");
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DeleteWarehouseHotel(string id)
    {
        string warehouseHotelId;
        string warehouseName;
        (warehouseHotelId, warehouseName) = await _warehouseHotelService.DeleteWarehouseHotel(id);
        return Ok(new DeleteWarehouseHotelResponse(warehouseHotelId, warehouseName));
    }

    [HttpPut]
    [Route("modify")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> ModifyWarehouseHotel([FromBody] ModifyWarehouseHotelRequest request)
    {
        await _warehouseHotelService.ModifyWarehouseHotel(request.WarehouseHotel);
        return Ok("Warehouse hotel modified successfully.");
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