using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlTypes;

namespace Controllers;

[Route("company-users")]
[ApiController]
public class CompanyUsers : ControllerBase
{
    private readonly ICompanyUserService _companyUserService;
    private readonly TokenService _tokenService;
    private readonly LagerhotellAPI.Models.DomainModels.Validators.CompanyUserValidator _companyUserValidator = new();

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
            return Ok(new GetCompanyUserResponse(companyUser));
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
            return Ok(new GetCompanyUsersResponse(companyUsers));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex}");
            return StatusCode(500);
        }
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> CreateCompanyUserAsync([FromBody] CreateCompanyUserRequest request)
    {
        try
        {
            _companyUserValidator.ValidateAndThrow(request.CompanyUser);
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
        try
        {
            (string userId, string userAcessToken) = await _companyUserService.CreateCompanyUserAsync(request.CompanyUser);
            return Ok(new CreateCompanyUserResponse(userId, userAcessToken));
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
    public async Task<IActionResult> UpdateCompanyUserAsync([FromBody] UpdateCompanyUserRequest request)
    {
        try
        {
            _companyUserValidator.ValidateAndThrow(request.CompanyUser);
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex);
        }
        try
        {
            await _companyUserService.UpdateCompanyUserAsync(request.CompanyUser.CompanyUserId, request.CompanyUser);
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
