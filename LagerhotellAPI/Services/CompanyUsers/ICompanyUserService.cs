namespace LagerhotellAPI.Services;
public interface ICompanyUserService
{

    Task<CompanyUser> GetCompanyUserAsync(string id);
    Task<List<CompanyUser>> GetCompanyUsersAsync(int? take, int? skip);
    Task<(string, string)> CreateCompanyUserAsync(CompanyUser companyUser);
    Task UpdateCompanyUserAsync(string id, CompanyUser companyUser);
    Task DeleteCompanyUserAsync(string id);

}
