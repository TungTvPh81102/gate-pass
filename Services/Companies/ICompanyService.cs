using BackEnd.DTOs.Requests.Company;
using BackEnd.DTOs.Responses;

namespace BackEnd.Services.Companies
{
    public interface ICompanyService
    {
        Task<IEnumerable<CompanyResponse>> GetAllAsync();

        Task<CompanyResponse?> GetByIdAsync(int id);

        Task<CompanyResponse> CreateAsync(
            CreateCompanyRequest request);

        Task<CompanyResponse> UpdateAsync(
            int id,
            UpdateCompanyRequest request);

        Task DeleteAsync(int id);
    }
}