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


        return Ok();
    }

    [Route("is-phone-number-registered-registration")]
    [HttpPost]
    public IActionResult CheckPhoneNumberExistence([FromBody] CheckPhoneNumberRequest request)
    {
        var user = _userRepository.Get(request.PhoneNumber);
        // logic to check if it exists
        return Ok(new CheckPhoneNumberResponse { PhoneNumberExistence = true});
    }
}