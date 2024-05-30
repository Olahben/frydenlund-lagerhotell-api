using Microsoft.AspNetCore.Mvc;

namespace LagerhotellAPI.Controllers
{
    [Route("storage-units")]
    [ApiController]
    public class StorageUnitsController : ControllerBase
    {
        private readonly StorageUnitService _storageUnitService;
        private readonly WarehouseHotelService _warehouseHotelService;

        public StorageUnitsController(StorageUnitService storageUnitService, WarehouseHotelService warehouseHotelService)
        {
            _storageUnitService = storageUnitService;
            _warehouseHotelService = warehouseHotelService;
        }

        [HttpPost]
        [Route("add")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> AddStorageUnit([FromBody] CreateStorageUnitRequest request)
        {
            try
            {
                WarehouseHotel linkedWarehouseHotel = await _warehouseHotelService.GetWarehouseHotelByName(request.LinkedWarehouseHotelName);
                if (linkedWarehouseHotel == null)
                {
                    return NotFound("Warehouse hotel not found.");
                }
                else if (linkedWarehouseHotel.WarehouseHotelId == null)
                {
                    return NotFound("Warehouse hotel Id not found.");
                }
                string storageUnitId = await _storageUnitService.AddStorageUnit(request.StorageUnit, linkedWarehouseHotel.WarehouseHotelId);
                return Ok(new CreateStorageUnitResponse(storageUnitId));
            }
            catch (InvalidOperationException)
            {
                return Conflict("Storage unit already exists.");
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpDelete("{storageUnitId}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteStorageUnit(string storageUnitId)
        {
            try
            {
                await _storageUnitService.DeleteStorageUnit(storageUnitId);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Storage unit not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpPut]
        [Route("modify")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> ModifyStorageUnit([FromBody] ModifyStorageUnitRequest request)
        {
            try
            {
                await _storageUnitService.ModifyStorageUnit(request.StorageUnit.StorageUnitId, request.StorageUnit);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound($"Storage unit not found, error: {ex}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpGet]
        [Route("get-by-id/{storageUnitId}")]
        public async Task<IActionResult> GetStorageUnitById([FromRoute] string storageUnitId)
        {
            try
            {
                var storageUnit = await _storageUnitService.GetStorageUnitById(storageUnitId);
                if (storageUnit != null)
                {
                    return Ok(storageUnit);
                }
                else
                {
                    return NotFound("Storage unit not found.");
                }
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("get-by-warehouse-hotel-id/{warehouseHotelId}")]
        public async Task<IActionResult> GetStorageUnitsByWarehouseHotelId(string warehouseHotelId)
        {
            try
            {
                var storageUnits = await _storageUnitService.GetStorageUnitsByWarehouseHotelId(warehouseHotelId);
                return Ok(storageUnits);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("No storage units found for the given warehouse hotel Id.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
                return StatusCode(500);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStorageUnits([FromQuery] int? skip, int? take)
        {
            try
            {
                var storageUnits = await _storageUnitService.GetAllStorageUnits(skip, take);
                return Ok(storageUnits);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}
