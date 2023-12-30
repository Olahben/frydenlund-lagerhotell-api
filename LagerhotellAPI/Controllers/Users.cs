using Lagerhotell.Shared;
using LagerhotellAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

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
            return Conflict(new CheckPhoneNumber.CheckPhoneNumberResponse { PhoneNumberExistence = true });
        }
        else
        {
            return Ok(new CheckPhoneNumber.CheckPhoneNumberResponse { PhoneNumberExistence = false });
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
        return Ok(_getuserResponse.GetUserResponseFunc(user.Id, user.FirstName, user.LastName, user.PhoneNumber, user.BirthDate, user.Password));
    }

    [Route("get-user-by-phone-number")]
    [HttpPost]
    public IActionResult GetUserByPhoneNumber([FromBody] GetUserByPhoneNumberRequest request)
    {
        var user = _userRepository.Get(request.PhoneNumber);
        return Ok(_getUserByPhoneNumberResponse.GetUserByPhoneNumberResponseFunc(user.Id, user.FirstName, user.LastName, user.PhoneNumber, user.BirthDate, user.Password));

    }

    [Route("create-jwt")]
    [HttpPost]
    public IActionResult CreateJwt(CreateJwt.CreateJwtRequest request)
    {
        string bitSecret;
        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            var secretKeyByteArray = new byte[32];
            rng.GetBytes(secretKeyByteArray);
            bitSecret = Convert.ToBase64String(secretKeyByteArray);
        }
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(bitSecret));
        var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var tokeOptions = new JwtSecurityToken(
            issuer: "https://localhost:7272",
            audience: "https://localhost:5001",
            claims: new List<Claim>()
            {
            new Claim(ClaimTypes.Name, request.PhoneNumber)
                // Add other claims here as needed
            },
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: signinCredentials
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
        return Ok(new CreateJwt.CreateJwtResponse { JWT = tokenString });
    }

}