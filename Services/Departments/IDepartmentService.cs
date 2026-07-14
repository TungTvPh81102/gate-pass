using BackEnd.DTOs.Requests.Department;
using BackEnd.DTOs.Responses.Department;

namespace BackEnd.Services.Departments
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentResponse>> GetAllAsync();

        Task<DepartmentResponse?> GetByIdAsync(int id);

        Task<DepartmentResponse> CreateAsync(
            CreateDepartmentRequest request);

        Task<DepartmentResponse> UpdateAsync(
            int id,
            UpdateDepartmentRequest request);

        Task DeleteAsync(int id);
    }
}
