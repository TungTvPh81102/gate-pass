using BackEnd.DTOs.Responses.Department;
using BackEnd.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Repositories.Departments
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ApplicationDbContext _context;

        public DepartmentRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Department>> GetAllAsync()
        {
            return await _context.Departments
                .AsNoTracking()
                .Include(d => d.Company)
                .Include(d => d.Parent)
                .ToListAsync();
        }

        public async Task<Department?> GetByIdAsync(int id)
        {
            return await _context.Departments.FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<DepartmentResponse?> GetByIdResponseAsync(int id)
        {
            return await _context.Departments
                .AsNoTracking()
                .Where(d => d.Id == id)
                .Select(d => new DepartmentResponse
                {
                    Id = d.Id,
                    Code = d.Code,
                    Name = d.Name,
                    Status = d.Status,
                    Description = d.Description,
                    CompanyId = d.CompanyId,
                    CompanyName = d.Company != null ? d.Company.Name : string.Empty,
                    Parent = d.Parent == null
                        ? null
                        : new DepartmentTreeResponse
                        {
                            Id = d.Parent.Id,
                            Name = d.Parent.Name,
                            ParentId = d.Parent.ParentId
                        }
                })
                .FirstOrDefaultAsync();
        }

        public async Task<Department> AddAsync(Department department)
        {
            _context.Departments.Add(department);

            await _context.SaveChangesAsync();

            return department;
        }

        public async Task UpdateAsync(Department department)
        {
            _context.Departments.Update(department);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Department department)
        {
            _context.Departments.Remove(department);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Departments.AnyAsync(d => d.Id == id);
        }

        public async Task<bool> CodeExistsAsync(string code, int? excludedId = null)
        {
            return await _context.Departments
                .AnyAsync(d => d.Code == code && (excludedId == null || d.Id != excludedId));
        }

        public async Task<bool> HasChildrenAsync(int id)
        {
            return await _context.Departments.AnyAsync(d => d.ParentId == id);
        }

        public async Task<bool> HasUsersAsync(int id)
        {
            return await _context.Users.AnyAsync(u => u.DepartmentId == id);
        }
    }
}
