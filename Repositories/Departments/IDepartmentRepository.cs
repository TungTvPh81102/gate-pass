using BackEnd.DTOs.Responses.Department;
using BackEnd.Models.Entities;

namespace BackEnd.Repositories.Departments
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<Department>> GetAllAsync();

        Task<Department?> GetByIdAsync(int id);

        Task<DepartmentResponse?> GetByIdResponseAsync(int id);

        Task<Department> AddAsync(Department department);

        Task UpdateAsync(Department department);

        Task DeleteAsync(Department department);

        Task<bool> ExistsAsync(int id);

        Task<bool> CodeExistsAsync(string code, int? excludedId = null);

        Task<bool> HasChildrenAsync(int id);

        Task<bool> HasUsersAsync(int id);
    }
}
