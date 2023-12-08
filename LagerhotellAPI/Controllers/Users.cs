using LagerhotellAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

[ApiController]
[Route("users")]
public class UsersController : ControllerBase
{
    private readonly UserRepository _userRepository = new UserRepository();

    [Route("is-phone-number-registered-registration")]
    [HttpPost]
    public IActionResult CheckPhoneNumberExistence([FromBody] CheckPhoneNumberRequest request)
    {
        // user is null if not found
        // takes the phone number as a string
        var user = _userRepository.Get(request.PhoneNumber);
        // should return phone number response
        if (user == null)
        {
            return Ok(new CheckPhoneNumberResponse { PhoneNumberExistence = false });
        } else
        {
            // Not found as in found
            return NotFound(new CheckPhoneNumberResponse { PhoneNumberExistence = true });
        }
    }

    [Route("adduser")]
    [HttpPost]
    public IActionResult AddUser([FromBody] AddUserRequest request)
    {

            _userRepository.Add(new User(
                request.FirstName,
                request.LastName,
                request.PhoneNumber,
                request.BirthDate,
                request.Password));

        // Should return AddUserResponse
        return Ok();
    }
}