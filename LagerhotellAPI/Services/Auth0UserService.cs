global using LagerhotellAPI.Models.DbModels.Auth0;
global using System.Net.Http.Headers;
global using System.Text.Json;
global using System.Text;
namespace LagerhotellAPI.Services;

public class Auth0UserService
{
    private readonly string _managementApiId;
    private readonly string _bearerToken;
    private readonly string _domain;
    private readonly string _clientId;
    private readonly string _dbName = "Lagerhotell";
    private HttpClient client = new();

    public Auth0UserService(IConfiguration configuration)
    {
        _bearerToken = configuration["Auth0:ApiBearerToken"];
        _domain = configuration["Auth0:Domain"];
        _managementApiId = $"https://{_domain}";
        _clientId = configuration["Auth0:ClientId"];
    }

    public async Task? AddUser(UserAuth0 user)
    {
        string endpoint = _managementApiId + "/dbconnections/signup";
        object userMetaData;
        if (user.NormalUserMetadata != null)
        {
            userMetaData = new
            {
                user_id = user.UserId,
                first_name = user.NormalUserMetadata.FirstName,
                last_name = user.NormalUserMetadata.LastName,
                phone_number = user.NormalUserMetadata.PhoneNumber,
                email = user.NormalUserMetadata.Email,
                birth_date = user.NormalUserMetadata.BirthDate,
                street_address = user.NormalUserMetadata.Address.StreetAddress,
                postal_code = user.NormalUserMetadata.Address.PostalCode,
                city = user.NormalUserMetadata.Address.City,
                is_administrator = user.NormalUserMetadata.IsAdministrator.ToString()
            };
        }
        else
        {
            userMetaData = new
            {
                company_user_id = user.CompanyUserMetaData.CompanyUserId,
                first_name = user.CompanyUserMetaData.FirstName,
                last_name = user.CompanyUserMetaData.LastName,
                name = user.CompanyUserMetaData.Name,
                phone_number = user.CompanyUserMetaData.PhoneNumber,
                email = user.CompanyUserMetaData.Email,
                company_number = user.CompanyUserMetaData.CompanyNumber,
                street_address = user.CompanyUserMetaData.Address.StreetAddress,
                postal_code = user.CompanyUserMetaData.Address.PostalCode,
                city = user.CompanyUserMetaData.Address.City
            };
        }
        var jsonData = new
        {
            client_id = _clientId,
            email = user.Email,
            password = user.Password,
            connection = _dbName,
            user_metadata = userMetaData
        };
        var json = JsonSerializer.Serialize(jsonData);
        var data = new StringContent(json, null, "application/json");
        var response = await client.PostAsync(endpoint, data);
        var responseContent = await response.Content.ReadAsStringAsync();
        response.EnsureSuccessStatusCode();
    }
}
