global using LagerhotellAPI.Models.DbModels.Auth0;
global using System.Net.Http.Headers;
global using System.Text.Json;
global using System.Text;
global using System.Net;
global using LagerhotellAPI.Models.CustomExceptionModels;
using LagerhotellAPI.Models;
namespace LagerhotellAPI.Services;

public class Auth0UserService
{
    private readonly string _usersApiId;
    private readonly string _bearerToken;
    private readonly string _domain;
    private readonly string _clientId;
    private readonly string _managementApiId;
    private readonly string _dbName = "Lagerhotell";
    private HttpClient client = new();
    private readonly CompanyUserService _companyUserService;

    public Auth0UserService(IConfiguration configuration, MongoDbSettings settings, TokenService tokenService)
    {
        _bearerToken = configuration["Auth0:ApiBearerToken"];
        _domain = configuration["Auth0:Domain"];
        _usersApiId = $"https://{_domain}";
        _clientId = configuration["Auth0:ClientId"];
        _companyUserService = new CompanyUserService(settings, tokenService);
        _managementApiId = $"https://{_domain}/api/v2";
    }

    public async Task? AddUser(UserAuth0 user)
    {
        bool isCompanyUser;
        try
        {
            await _companyUserService.GetCompanyUserAsync(user.UserId);
            isCompanyUser = true;
        }
        catch (KeyNotFoundException e)
        {
            isCompanyUser = false;
        }
        string endpoint = _usersApiId + "/dbconnections/signup";
        var jsonData = new
        {
            client_id = _clientId,
            email = user.Email,
            password = user.Password,
            connection = _dbName,
            user_metadata = new
            {
                company_user = isCompanyUser.ToString(),
                user_id = user.UserId
            }
        };
        var json = JsonSerializer.Serialize(jsonData);
        var data = new StringContent(json, null, "application/json");
        var response = await client.PostAsync(endpoint, data);


        var responseContent = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                throw new BadRequestException(responseContent);
            }
        }
    }

    /// <summary>
    /// Gets a users information from Auth0 (will be finished as soon as auth0 tokens are implemented for this application)
    /// </summary>
    /// <param name="accesstoken"></param>
    /// <returns></returns>
    /// <exception cref="BadRequestException"></exception>
    public async Task<object> GetUser(string accesstoken)
    {
        string endpoint = _usersApiId + "/userinfo";
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);
        var response = await client.GetAsync(endpoint);
        try
        {
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            object user = JsonSerializer.Deserialize<object>(responseContent);
            return user;
        }
        catch (HttpRequestException e)
        {
            throw new BadRequestException($"Invalid access token, {e}");
        }
    }

    public async Task<UserAuth0> GetCompleteUser(string auth0Id)
    {
        string endpoint = _managementApiId + $"/users/{auth0Id}";
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _bearerToken);
        var response = await client.GetAsync(endpoint);
        try
        {
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var user = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(responseContent, options);
            string userId = user["user_metadata"].GetProperty("user_id").ToString();
            string email = user["email"].ToString();

            return new UserAuth0(userId, email)
            {
                UserAuth0Id = auth0Id
            };
        }
        catch (HttpRequestException e)
        {
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new KeyNotFoundException($"{e.Message}");
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                throw new BadRequestException($"{e.Message}");
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedAccessException($"{e.Message}");
            }
            else if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                throw new Exception($"{e.Message}");
            }
            else if (response.StatusCode == HttpStatusCode.TooManyRequests)
            {
                throw new TooManyRequestsException($"{e.Message}");
            }
            throw new Exception(e.ToString());
        }
    }
}
