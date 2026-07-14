using BackEnd.DTOs.Requests.Department;
using BackEnd.DTOs.Responses.Department;
using BackEnd.Models.Entities;
using BackEnd.Repositories.Companies;
using BackEnd.Repositories.Departments;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Services.Departments
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ILogger<DepartmentService> _logger;
        private readonly ICompanyRepository _companyRepository;

        public DepartmentService(
            IDepartmentRepository departmentRepository,
            ILogger<DepartmentService> logger,
            ICompanyRepository companyRepository)
        {
            _departmentRepository = departmentRepository;
            _logger = logger;
            _companyRepository = companyRepository;
        }

        public async Task<IEnumerable<DepartmentResponse>> GetAllAsync()
        {
            try
            {
                _logger.LogInformation("Getting all departments");

                var departments = await _departmentRepository.GetAllAsync();

                _logger.LogInformation("Retrieved {Count} departments", departments.Count());

                return departments.Select(MapToResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while getting departments");
                throw;
            }
        }

        public async Task<DepartmentResponse?> GetByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("Get department by id: {Id}", id);
                var department = await _departmentRepository.GetByIdResponseAsync(id);

                if (department == null)
                {
                    _logger.LogWarning("Department not found with id: {Id}", id);
                    return null;
                }

                return department;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while getting department by id: {Id}", id);
                throw;
            }
        }

        public async Task<DepartmentResponse> CreateAsync(CreateDepartmentRequest request)
        {
            try
            {
                _logger.LogInformation("Creating department with code: {Code}", request.Code);

                if (string.IsNullOrWhiteSpace(request.Code))
                {
                    _logger.LogWarning("Department code is null or whitespace.");
                    throw new ArgumentException("Department code is required.");
                }

                await ValidateCompanyAsync(request.CompanyId);
                await ValidateUniqueCodeAsync(request.Code);
                await ValidateParentAsync(request.ParentId, request.CompanyId);

                var department = new Department()
                {
                    Code = request.Code,
                    Name = request.Name,
                    Status = request.Status,
                    Description = request.Description,
                    ParentId = request.ParentId,
                    CompanyId = request.CompanyId,
                    CreatedBy = "System",
                    CreatedAt = DateTime.UtcNow
                };

                var result = await _departmentRepository.AddAsync(department);

                _logger.LogInformation("Created department with id: {Id}", department.Id);

                var created = await _departmentRepository.GetByIdResponseAsync(result.Id);

                return created ?? MapToResponse(result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Validation failed while creating department with code: {Code}", request.Code);
                throw;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Conflict while creating department with code: {Code}", request.Code);
                throw;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error while creating department with code: {Code}", request.Code);
                throw new InvalidOperationException("Department code already exists or violates database constraints.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while creating department with code: {Code}", request.Code);
                throw;
            }
        }

        public async Task<DepartmentResponse> UpdateAsync(int id, UpdateDepartmentRequest request)
        {
            try
            {
                _logger.LogInformation("Updating department with id: {Id}", id);

                var department = await _departmentRepository.GetByIdAsync(id);
                if (department == null)
                {
                    throw new KeyNotFoundException($"Department with id {id} not found");
                }

                await ValidateCompanyAsync(request.CompanyId);
                await ValidateUniqueCodeAsync(request.Code, id);
                await ValidateParentAsync(request.ParentId, request.CompanyId, id);

                department.Code = request.Code;
                department.Name = request.Name;
                department.Status = request.Status;
                department.Description = request.Description;
                department.ParentId = request.ParentId;
                department.CompanyId = request.CompanyId;
                department.UpdatedAt = DateTime.UtcNow;
                department.UpdatedBy = "System";

                await _departmentRepository.UpdateAsync(department);

                var result = await _departmentRepository.GetByIdResponseAsync(id);

                return result ?? MapToResponse(department);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Department not found for update with id: {Id}", id);
                throw;
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Validation failed while updating department with id: {Id}", id);
                throw;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Conflict while updating department with id: {Id}", id);
                throw;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error while updating department with id: {Id}", id);
                throw new InvalidOperationException("Department update violates database constraints.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while updating department with id: {Id}", id);
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                _logger.LogInformation("Deleting department with id: {Id}", id);

                var department = await _departmentRepository.GetByIdAsync(id);
                if (department == null)
                {
                    throw new KeyNotFoundException($"Department with id {id} not found");
                }

                if (await _departmentRepository.HasChildrenAsync(id))
                {
                    throw new InvalidOperationException("Cannot delete department because it still has child departments.");
                }

                if (await _departmentRepository.HasUsersAsync(id))
                {
                    throw new InvalidOperationException("Cannot delete department because it still has users.");
                }

                await _departmentRepository.DeleteAsync(department);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Department not found for deletion with id: {Id}", id);
                throw;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Department cannot be deleted with id: {Id}", id);
                throw;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error while deleting department with id: {Id}", id);
                throw new InvalidOperationException("Department delete violates database constraints.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while deleting department with id: {Id}", id);
                throw;
            }
        }

        private async Task ValidateCompanyAsync(int companyId)
        {
            if (!await _companyRepository.ExistsAsync(companyId))
            {
                throw new ArgumentException("Company does not exist.");
            }
        }

        private async Task ValidateUniqueCodeAsync(string code, int? excludedId = null)
        {
            if (await _departmentRepository.CodeExistsAsync(code, excludedId))
            {
                throw new InvalidOperationException("Department code already exists.");
            }
        }

        private async Task ValidateParentAsync(int? parentId, int companyId, int? departmentId = null)
        {
            if (parentId == null)
            {
                return;
            }

            if (departmentId != null && parentId == departmentId)
            {
                throw new ArgumentException("Department cannot be its own parent.");
            }

            var parent = await _departmentRepository.GetByIdAsync(parentId.Value);
            if (parent == null)
            {
                throw new ArgumentException("Parent department does not exist.");
            }

            if (parent.CompanyId != companyId)
            {
                throw new ArgumentException("Parent department must belong to the same company.");
            }

            if (departmentId != null && await WouldCreateCycleAsync(departmentId.Value, parentId.Value))
            {
                throw new ArgumentException("Parent department cannot be a child of the current department.");
            }
        }

        private async Task<bool> WouldCreateCycleAsync(int departmentId, int parentId)
        {
            var currentParentId = parentId;

            while (true)
            {
                if (currentParentId == departmentId)
                {
                    return true;
                }

                var parent = await _departmentRepository.GetByIdAsync(currentParentId);
                if (parent?.ParentId == null)
                {
                    return false;
                }

                currentParentId = parent.ParentId.Value;
            }
        }

        private DepartmentResponse MapToResponse(Department department)
        {
            return new DepartmentResponse
            {
                Id = department.Id,
                Code = department.Code,
                Name = department.Name,
                Status = department.Status,
                Description = department.Description,
                CompanyId = department.CompanyId,
                CompanyName = department.Company?.Name ?? string.Empty,
                Parent = department.Parent == null
                    ? null
                    : new DepartmentTreeResponse
                    {
                        Id = department.Parent.Id,
                        Name = department.Parent.Name,
                        ParentId = department.Parent.ParentId
                    }
            };
        }
    }
}
