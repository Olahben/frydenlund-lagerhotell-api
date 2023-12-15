using Lagerhotell.Shared;
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
    public IActionResult CheckPhoneNumberExistence([FromBody] CheckPhoneNumber.CheckPhoneNumberRequest request)
    {
        // user is null if not found
        var user = _userRepository.Get(request.PhoneNumber);
        // should return phone number response
        if (user == null)
        {
            return Ok(new CheckPhoneNumber.CheckPhoneNumberResponse { PhoneNumberExistence = false });
        } else
        {
            return Conflict(new CheckPhoneNumber.CheckPhoneNumberResponse { PhoneNumberExistence = true });
        }
    }

    [Route("adduser")]
    [HttpPost]
    public IActionResult AddUser([FromBody] AddUserRequest request)
    {
        var user = _userRepository.Get(request.PhoneNumber);
        if (user != null)
        {

            return Conflict(new CheckPhoneNumber.CheckPhoneNumberResponse { PhoneNumberExistence = true });
        } else

            _userRepository.Add(new User(
              request.FirstName,
              request.LastName,
              request.PhoneNumber,
              request.BirthDate,
              request.Password));

        var newUser = _userRepository.Get(request.PhoneNumber);

        return Ok(new AddUserResponse { UserId = newUser.Id });
    }

    [Route("check-password")]
    [HttpPost]
    public IActionResult ReturnPassword([FromBody] CheckPassword.CheckPasswordRequest request)
    {
        string password = _userRepository.Password(request.PhoneNumber);
        Console.WriteLine(password);
        return Ok(new CheckPassword.CheckPasswordResponse { Password = password });
    }
}