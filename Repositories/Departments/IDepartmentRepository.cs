using BackEnd.Models.Entities;

namespace BackEnd.Repositories.Departments
{
    public interface IDepartmentRepository
    {
        //Task<IEnumerable<Department>> GetAllAsync();

        //Task<Department?> GetByIdAsync(int id);

        //Task<Department> AddAsync(Department company);

        //Task UpdateAsync(Department company);

        //Task DeleteAsync(Department company);

        Task ExistsByCodeAndCompanyAsync(string code, int companyId);
    }
}
