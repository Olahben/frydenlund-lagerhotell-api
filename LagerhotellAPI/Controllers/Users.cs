global using LagerhotellAPI.Models.DomainModels;
global using LagerhotellAPI.Models.FrontendModels;
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
                return Ok(new CheckPhoneNumber.CheckPhoneNumberResponse { PhoneNumberExistence = false });
            }
            else
            {
                return Ok(new CheckPhoneNumber.CheckPhoneNumberResponse { PhoneNumberExistence = true });
            }
        }

        [Route("check-email/{email}")]
        [HttpGet]
        public IActionResult CheckEmailExistence(string email)
        {
            var user = _userRepository.GetByEmail(email);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                return Ok();
            }
        }

        [Route("add-user")]
        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] AddUserRequest request)
        {
            bool doesSimilarUserExist = await _userRepository.DoesSimilarUserExist(request.PhoneNumber, request.Email);
            if (doesSimilarUserExist)
            {

                return Conflict(new CheckPhoneNumber.CheckPhoneNumberResponse { PhoneNumberExistence = true });
            }
            User user;
            try
            {
                user = await _userRepository.Add(
                 request.FirstName,
                 request.LastName,
                 request.PhoneNumber,
                 request.BirthDate,
                 request.Address,
                 request.PostalCode,
                 request.City,
                 request.Password,
                 request.IsAdministrator,
                 request.Email);
            } catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, new { Message = "Something went wrong with adding a user to auth0 or the database"});
            }

            Jwt jwt = _tokenService.CreateJwt(user.Id, user.PhoneNumber, request.IsAdministrator);

            return Ok(new AddUserResponse { UserId = user.Id, Token = jwt.Token });
        }
        [Route("log-in")]
        [HttpPost]
        public IActionResult Login([FromBody] Login.LoginRequest request)
        {
            User? user = _userRepository.Get(request.PhoneNumber);
            if (user != null)
            {
                if (_userRepository.DoPasswordsMatch(request.Password, user.Password))
                {
                    Jwt jwt = _tokenService.CreateJwt(user.Id, user.PhoneNumber, user.IsAdministrator);
                    return Ok(new Login.LoginResponse { Token = jwt.Token });
                }

                return Unauthorized();
            }
            return NotFound();
        }

        [Route("log-in-by-email")]
        [HttpPost]
        public IActionResult LoginByEmail([FromBody] LoginByEmailRequest request)
        {
            User? user = _userRepository.GetByEmail(request.Email);
            if (user != null)
            {
                if (_userRepository.DoPasswordsMatch(request.Password, user.Password))
                {
                    Jwt jwt = _tokenService.CreateJwt(user.Id, user.PhoneNumber, user.IsAdministrator);
                    return Ok(new LoginByEmailResponse(jwt.Token));
                }

                return Unauthorized();
            }
            return NotFound();
        }


        [Route("get-user-by-email/{email}")]
        [HttpGet]
        [Authorize]
        public IActionResult GetUserByEmail(string email)
        {
            var user = _userRepository.GetByEmail(email);
            if (user != null)
            {
                return Ok(new GetByEmailResponse(user));
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
            return Ok(new GetUser.GetUserResponse() { User = user });
        }

        [Authorize]
        [Route("get-user-by-phone-number/{phoneNumber}")]
        [HttpGet]
        public IActionResult GetUserByPhoneNumber(string phoneNumber)
        {
            var user = _userRepository.Get(phoneNumber);
            if (user != null)
            {
                return Ok(new GetUserByPhoneNumber.GetUserByPhoneNumberResponse { User = user, Id = user.Id });
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
                _userRepository.UpdateUserValues(request.FirstName, request.LastName, request.PhoneNumber, request.BirthDate, request.Password, request.Address.StreetAddress, request.Address.PostalCode, request.Address.City, request.IsAdministrator, request.Email);
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return NotFound(ex.Message);
            }

        }

        [Authorize(Roles = "Administrator")]
        [Route("get-all/{skip}/{take}")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers(int? skip, int? take)
        {
            try
            {
                var users = await _userRepository.GetAllUsers(skip, take);
                return Ok(users);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500);
            }
        }

        [Authorize]
        [Route("delete-user/{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteUserById(string id)
        {
            try
            {
                await _userRepository.DeleteUser(id);
                return Ok();
            }
            catch (KeyNotFoundException e)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("does-similar-user-exist/{phoneNumber}/{email}")]
        public async Task<IActionResult> DoesSimilarUserExist([FromRoute] string phoneNumber, [FromRoute] string email)
        {
            bool doesSimilarUserExist = await _userRepository.DoesSimilarUserExist(phoneNumber, email);
            if (doesSimilarUserExist)
            {
                return Conflict();
            }
            return Ok();
        }
    }
}