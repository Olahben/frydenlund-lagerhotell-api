using LagerhotellAPI.Models;
using LagerhotellAPI.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("users")]
public class UsersController : ControllerBase
{
    private readonly UserRepository _userRepository = new UserRepository();
    private readonly GetUserResponse _getuserResponse = new GetUserResponse();
    private readonly GetUserByPhoneNumberResponse _getUserByPhoneNumberResponse = new GetUserByPhoneNumberResponse();
    private readonly TokenService _tokenService = new();

    [Route("is-phone-number-registered-registration")]
    [HttpPost]
    public IActionResult CheckPhoneNumberExistence([FromBody] CheckPhoneNumber.CheckPhoneNumberRequest request)
    {
        // user is null if not found
        var user = _userRepository.Get(request.PhoneNumber);
        // should return phone number response
        if (user == null)
        {
            return Conflict(new CheckPhoneNumber.CheckPhoneNumberResponse { PhoneNumberExistence = true });
        }
        else
        {
            return Ok(new CheckPhoneNumber.CheckPhoneNumberResponse { PhoneNumberExistence = false });
        }
    }

    [Route("add-user")]
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
    public IActionResult CheckPassword([FromBody] CheckPassword.CheckPasswordRequest request)
    {
        // Hent ut passordet fra brukeren, ved hjelp av mobilnummeret
        // Lag en ny metode paa userRepository som sjekker om passordene er like
        // den returnerer en bool
        // Hvis passordene ikke er like, returner DeclineAccess eller noe saant
        // Hvis passordene er like, returner JWT
        // Hvis passord er feil, returner tom JWT
        // Get user
        User? user = _userRepository.Get(request.PhoneNumber);
        if (_userRepository.DoPasswordsMatch(request.Password, user.Password))
        {
            Jwt jwt = _tokenService.CreateJwt(user.Id, user.PhoneNumber);
            return Ok(new CheckPassword.CheckPasswordResponse { Token = jwt.Token });
        }

        return Unauthorized();
    }

    [Route("get-user")]
    [HttpPost]
    public IActionResult GetUser([FromBody] LagerhotellAPI.Models.GetUserRequest request)
    {
        User? user = _userRepository.GetUserById(request.UserId);
        return Ok(_getuserResponse.GetUserResponseFunc(user.Id, user.FirstName, user.LastName, user.PhoneNumber, user.BirthDate, user.Password));
    }

    [Route("get-user-by-phone-number")]
    [HttpPost]
    public IActionResult GetUserByPhoneNumber([FromBody] GetUserByPhoneNumberRequest request)
    {
        var user = _userRepository.Get(request.PhoneNumber);
        return Ok(_getUserByPhoneNumberResponse.GetUserByPhoneNumberResponseFunc(user.Id, user.FirstName, user.LastName, user.PhoneNumber, user.BirthDate, user.Password));

    }
}