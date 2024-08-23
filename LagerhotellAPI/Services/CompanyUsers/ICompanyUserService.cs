using LagerhotellAPI.Models.DbModels;
namespace LagerhotellAPI.Services;
public interface ICompanyUserService
{

    Task<CompanyUserDocument> GetCompanyUserAsync(string id);
    Task<List<CompanyUserDocument>> GetCompanyUsersAsync(int? take, int? skip);
    Task<CompanyUser> CreateCompanyUserAsync(CompanyUser companyUser);
    Task UpdateCompanyUserAsync(string id, CompanyUser companyUser);
    Task DeleteCompanyUserAsync(string id);

}
