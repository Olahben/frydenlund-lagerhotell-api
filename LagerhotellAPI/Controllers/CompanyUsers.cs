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
    private readonly Auth0UserService _auth0UserService;

    public CompanyUsers(ICompanyUserService companyUserService, Auth0UserService auth0UserService)
    {
        _companyUserService = companyUserService;
        _auth0UserService = auth0UserService;
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
    [Authorize(Roles = "Administrator")]
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
        catch (KeyNotFoundException e)
        {
            return NotFound();
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
            await _auth0UserService.DeleteUser(id);
        } catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        } catch (UnauthorizedAccessException e)
        {
            return Unauthorized(e.Message);
        } catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
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

    [HttpGet]
    [Route("does-similar-user-exist/{companyNumber}/{phoneNumber}/{email}")]
    public async Task<IActionResult> DoesSimilarUserExist(
    [FromRoute] string companyNumber,
    [FromRoute] string phoneNumber,
    [FromRoute] string email)
    {
        try
        {
            bool result = await _companyUserService.DoesSimilarUserExist(companyNumber, phoneNumber, email);
            if (result)
            {
                return Conflict("A user with similar credentials already exist");
            }
            return Ok();
        }
        catch (KeyNotFoundException)
        {
            return NotFound("Company is not registered in Norway");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex}");
            return StatusCode(500);
        }
    }

    [HttpGet]
    [Route("get-user-by-auth0-id/{auth0Id}")]
    public async Task<IActionResult> GetUserByAuth0Id([FromRoute] string auth0Id)
    {
        try
        {
            CompanyUser companyUser = await _companyUserService.GetUserByAuth0Id(auth0Id);
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
}
