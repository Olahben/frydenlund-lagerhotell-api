using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LagerhotellAPI.Controllers;

[Route("auth0-users")]
[ApiController]
public class Auth0UsersController : ControllerBase
{
    private readonly Auth0UserService _auth0UserService;
    public Auth0UsersController(Auth0UserService auth0UserService)
    {
        _auth0UserService = auth0UserService;
    }

    // Endpoints are going to be protected with authorization after auth0 tokens are integrated
    // For now, they are open for testing purposes
    // Endpoints do not currently have request models or response models, this will be implemented and validated later
    [HttpGet]
    [Route("get-user")]
    public async Task<IActionResult> GetUser([FromQuery] string auth0UserId)
    {
        try
        {
            var user = await _auth0UserService.GetCompleteUser(auth0UserId);
            return Ok(user);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
        catch (UnauthorizedAccessException e)
        {
            return Unauthorized(e.Message);
        }
        catch (TooManyRequestsException e)
        {
            return StatusCode(StatusCodes.Status429TooManyRequests, e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpPost]
    [Route("signup-user")]
    public async Task<IActionResult> SignupUser([FromBody] UserAuth0 user)
    {
        try
        {
            await _auth0UserService.AddUser(user);
            return Ok();
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
        catch (UnauthorizedAccessException e)
        {
            return Unauthorized(e.Message);
        }
        catch (TooManyRequestsException e)
        {
            return StatusCode(StatusCodes.Status429TooManyRequests, e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpDelete]
    [Route("delete-user")]
    public async Task<IActionResult> DeleteUser([FromQuery] string auth0UserId)
    {
        try
        {
            await _auth0UserService.DeleteUser(auth0UserId);
            return Ok();
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
        catch (UnauthorizedAccessException e)
        {
            return Unauthorized(e.Message);
        }
        catch (TooManyRequestsException e)
        {
            return StatusCode(StatusCodes.Status429TooManyRequests, e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpPatch]
    [Route("update-and-verify-user-email")]
    public async Task<IActionResult> UpdateAndVerifyUserEmail([FromQuery] string auth0UserId, [FromQuery] string email)
    {
        try
        {
            await _auth0UserService.UpdateAndVerifyEmail(auth0UserId, email);
            return Ok();
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
        catch (UnauthorizedAccessException e)
        {
            return Unauthorized(e.Message);
        }
        catch (TooManyRequestsException e)
        {
            return StatusCode(StatusCodes.Status429TooManyRequests, e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    /// <summary>
    /// Endpoint for updating a users password when the user has not forgotten the password
    /// </summary>
    /// <param name="auth0UserId"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    [HttpPatch]
    [Route("change-user-password")]
    public async Task<IActionResult> ChangeUserPassword([FromQuery] string auth0UserId, [FromQuery] string email, [FromQuery] string password, [FromQuery] string oldPassword)
    {
        try
        {
            await _auth0UserService.IsLoginCredentialsCorrect(email, oldPassword);
        } catch (UnauthorizedAccessException)
        {
            return Unauthorized("Old password is incorrect");
        } catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
        try
        {
            await _auth0UserService.ChangeUserpassword(auth0UserId, password);
            return Ok();
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
        catch (UnauthorizedAccessException e)
        {
            return Unauthorized(e.Message);
        }
        catch (TooManyRequestsException e)
        {
            return StatusCode(StatusCodes.Status429TooManyRequests, e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpPost]
    [Route("send-forgot-password-email")]
    public async Task<IActionResult> SendForgotPasswordEmail([FromQuery] string email)
    {
        try
        {
            await _auth0UserService.SendForgotPasswordEmail(email);
            return Created();
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
        catch (UnauthorizedAccessException e)
        {
            return Unauthorized(e.Message);
        }
        catch (TooManyRequestsException e)
        {
            return StatusCode(StatusCodes.Status429TooManyRequests, e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
}
