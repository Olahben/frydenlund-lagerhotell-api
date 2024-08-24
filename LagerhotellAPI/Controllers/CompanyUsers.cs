using Microsoft.AspNetCore.Mvc;
using System.Data.SqlTypes;

namespace Controllers;

[Route("company-users")]
[ApiController]
public class CompanyUsers : ControllerBase
{
    private readonly ICompanyUserService _companyUserService;

    public CompanyUsers(ICompanyUserService companyUserService)
    {
        _companyUserService = companyUserService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCompanyUserAsync([FromRoute] string id)
    {
        try
        {
            CompanyUser companyUser = await _companyUserService.GetCompanyUserAsync(id);
            return Ok(companyUser);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex}");
            return StatusCode(500);
        }
    }

    [HttpGet]
    [Route("all")]
    public async Task<IActionResult> GetCompanyUsersAsync([FromQuery] int? take, [FromQuery] int? skip)
    {
        try
        {
            List<CompanyUser> companyUsers = await _companyUserService.GetCompanyUsersAsync(take, skip);
            return Ok(companyUsers);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex}");
            return StatusCode(500);
        }
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> CreateCompanyUserAsync([FromBody] CompanyUser companyUser)
    {
        try
        {
            CompanyUser createdCompanyUser = await _companyUserService.CreateCompanyUserAsync(companyUser);
            return Ok(createdCompanyUser);
        }
        catch (SqlAlreadyFilledException ex)
        {
            Console.WriteLine($"{ex}");
            return Conflict("Company user already exists");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex}");
            return StatusCode(500);
        }
    }

    [HttpPut]
    [Route("modify")]
    [Authorize]
    public async Task<IActionResult> UpdateCompanyUserAsync([FromBody] CompanyUser companyUser)
    {
        try
        {
            await _companyUserService.UpdateCompanyUserAsync(companyUser.CompanyUserId, companyUser);
            return Ok("Company user was modified");
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex}");
            return StatusCode(500);
        }
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteCompanyUserAsync([FromRoute] string id)
    {
        try
        {
            await _companyUserService.DeleteCompanyUserAsync(id);
            return Ok("Company user was deleted");
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex}");
            return StatusCode(500);
        }
    }
}
