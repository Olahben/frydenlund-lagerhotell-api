using Lagerhotell.Shared;
using LagerhotellAPI.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("users")]
public class UsersController : ControllerBase
{
    private readonly UserRepository _userRepository = new UserRepository();
    private readonly GetUserResponse _getuserResponse = new GetUserResponse();
    private readonly GetUserByPhoneNumberResponse _getUserByPhoneNumberResponse = new GetUserByPhoneNumberResponse();

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
        }
        else
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
        }

        user = _userRepository.Add(
         request.FirstName,
         request.LastName,
         request.PhoneNumber,
         request.BirthDate,
         request.Password);

        return Ok(new AddUserResponse { UserId = user.Id });
    }

    [Route("check-password")]
    [HttpPost]
    public IActionResult ReturnPassword([FromBody] CheckPassword.CheckPasswordRequest request)
    {
        string password = _userRepository.Password(request.PhoneNumber);
        Console.WriteLine(password);
        return Ok(new CheckPassword.CheckPasswordResponse { Password = password });
    }

    [Route("get-user")]
    [HttpPost]
    public IActionResult GetUser([FromBody] LagerhotellAPI.Models.GetUserRequest request)
    {
        User? user = _userRepository.GetUserById(request.UserId);
        return Ok(_getuserResponse.GetUserResponseFunc(user.FirstName, user.LastName, user.PhoneNumber, user.BirthDate, user.Password, user.Id));
    }

    [Route("get-user-by-phone-number")]
    [HttpPost]
    public IActionResult GetUserByPhoneNumber([FromBody] GetUserByPhoneNumberRequest request)
    {
        var user = _userRepository.Get(request.PhoneNumber);
        Console.WriteLine(user.FirstName);
        return Ok(_getUserByPhoneNumberResponse.GetUserByPhoneNumberResponseFunc(user.FirstName, user.LastName, user.PhoneNumber, user.BirthDate, user.Password, user.Id));

    }

}