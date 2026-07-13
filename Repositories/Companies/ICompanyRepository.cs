using BackEnd.Models.Entities;

namespace BackEnd.Repositories.Companies
{
    public interface ICompanyRepository
    {
        Task<IEnumerable<Company>> GetAllAsync();

        Task<Company?> GetByIdAsync(int id);

        Task<Company> AddAsync(Company company);

        Task UpdateAsync(Company company);

        Task DeleteAsync(Company company);
    }
}