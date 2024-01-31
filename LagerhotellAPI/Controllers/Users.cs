using LagerhotellAPI.Models;
using LagerhotellAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



namespace Controllers
{
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        private readonly GetUser.GetUserResponse _getuserResponse = new GetUser.GetUserResponse();
        private readonly TokenService _tokenService;

        public UsersController(TokenService tokenService, UserRepository userRepository)
        {
            _tokenService = tokenService;
            _userRepository = userRepository;
        }

        [Route("check-phone/{phoneNumber}")]
        [HttpGet]
        public IActionResult CheckPhoneNumberExistence(string phoneNumber)
        {
            var user = _userRepository.Get(phoneNumber);
            if (user == null)
            {
                return Ok(new CheckPhoneNumber.CheckPhoneNumberResponse { PhoneNumberExistence = true });
            }
            else
            {
                return Conflict(new CheckPhoneNumber.CheckPhoneNumberResponse { PhoneNumberExistence = false });
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
             request.Address,
             request.PostalCode,
             request.City,
             request.Password);

            Jwt jwt = _tokenService.CreateJwt(user.Id, user.PhoneNumber);

            return Ok(new AddUserResponse { UserId = user.Id, Token = jwt.Token });
        }
        [Route("log-in")]
        [HttpPost]
        public IActionResult Login([FromBody] Login.LoginRequest request)
        {
            // Hent ut passordet fra brukeren, ved hjelp av mobilnummeret
            // Lag en ny metode paa userRepository som sjekker om passordene er like
            // den returnerer en bool
            // Hvis passordene ikke er like, returner DeclineAccess eller noe saant
            // Hvis passordene er like, returner JWT
            // Hvis passord er feil, returner tom JWT
            // Get user
            User? user = _userRepository.Get(request.PhoneNumber);
            if (user != null)
            {
                if (_userRepository.DoPasswordsMatch(request.Password, user.Password))
                {
                    Jwt jwt = _tokenService.CreateJwt(user.Id, user.PhoneNumber);
                    return Ok(new Login.LoginResponse { Token = jwt.Token });
                }

                return Unauthorized();
            }
            return NotFound();
        }

        [Authorize]
        [Route("get-user/{userId}")]
        [HttpGet]
        public IActionResult GetUser(string userId)
        {
            User? user = _userRepository.GetUserById(userId);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(_getuserResponse.GetUserResponseFunc(user.Id, user.FirstName, user.LastName, user.PhoneNumber, user.BirthDate, user.Address, user.PostalCode, user.City, user.Password));
        }

        [Authorize]
        [Route("get-user-by-phone-number/{phoneNumber}")]
        [HttpGet]
        public IActionResult GetUserByPhoneNumber(string phoneNumber)
        {
            var user = _userRepository.Get(phoneNumber);
            if (user != null)
            {
                return Ok(new GetUserByPhoneNumber.GetUserByPhoneNumberResponse { Id = user.Id, });
            }
            return NotFound();

        }

        [Authorize]
        [Route("update-user-values")]
        [HttpPut]
        public IActionResult UpdateUserValues([FromBody] UpdateUserValuesRequest request)
        {
            try
            {
                _userRepository.UpdateUserValues(request.FirstName, request.LastName, request.PhoneNumber, request.BirthDate, request.Password, request.Address, request.PostalCode, request.City);
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

        }
    }
}