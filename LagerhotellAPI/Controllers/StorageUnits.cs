using LagerhotellAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LagerhotellAPI.Controllers
{
    [Route("storage-units")]
    [ApiController]
    public class StorageUnitsController : ControllerBase
    {
        private readonly StorageUnitService _storageUnitService;

        public StorageUnitsController(StorageUnitService storageUnitService)
        {
            _storageUnitService = storageUnitService;
        }

        [HttpPost]
        [Route("add")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> AddStorageUnit([FromBody] CreateStorageUnitRequest request)
        {
            try
            {
                string storageUnitId = await _storageUnitService.AddStorageUnit(request.StorageUnit);
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
        [Route("get-by-id")]
        public async Task<IActionResult> GetStorageUnitById([FromQuery] string storageUnitId)
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
