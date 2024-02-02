using LagerhotellAPI.Models;
using LagerhotellAPI.Services;
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
        public async Task<IActionResult> AddStorageUnit([FromBody] StorageUnit storageUnit)
        {
            try
            {
                await _storageUnitService.AddStorageUnit(storageUnit);
                return Ok();
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
        [Route("delete")]
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
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPut("{storageUnitId}")]
        [Route("modify")]
        public async Task<IActionResult> ModifyStorageUnit(string storageUnitId, [FromBody] StorageUnit updatedStorageUnit)
        {
            try
            {
                await _storageUnitService.ModifyStorageUnit(storageUnitId, updatedStorageUnit);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Storage unit not found.");
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpGet("{storageUnitId}")]
        [Route("get-by-id")]
        public async Task<IActionResult> GetStorageUnitById(string storageUnitId)
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
        [Route("get-all")]
        public async Task<IActionResult> GetAllStorageUnits()
        {
            try
            {
                var storageUnits = await _storageUnitService.GetAllStorageUnits();
                return Ok(storageUnits);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}
