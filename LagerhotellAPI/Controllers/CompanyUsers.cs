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
    [Route("get-by-phone-number/{phoneNumber}")]
    public async Task<IActionResult> GetCompanyUserByPhoneNumber([FromRoute] string phoneNumber)
    {
        try
        {
            CompanyUser companyUser = await _companyUserService.GetCompanyUserByPhoneNumber(phoneNumber);
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

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginCompanyUserByEmailRequest request)
    {
        try
        {
            (string token, string id) = await _companyUserService.LoginCompanyUserByEmail(request.Email, request.Password);
            return Ok(new LoginCompanyUserByEmailResponse(token, id));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (SqlTypeException)
        {
            return Conflict("Incorrect password");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex}");
            return StatusCode(500);
        }
    }

    [HttpPut]
    [Route("reset-password")]
    [Authorize]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        try
        {
            await _companyUserService.ResetPassword(request.UserId, request.NewPassword, request.OldPassword);
            return Ok("Password was reset");
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (SqlAlreadyFilledException)
        {
            return UnprocessableEntity("New password is the same as the old one");
        }
        catch (SqlTypeException)
        {
            return Conflict("Incorrect password");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex}");
            return StatusCode(500);
        }
    }
}
