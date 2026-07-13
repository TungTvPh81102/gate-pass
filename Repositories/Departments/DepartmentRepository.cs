using BackEnd.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;

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

        public async Task<Department?> ExistsByCodeAndCompanyAsync(string code, int companyid)
        {
            return await _context.Departments
        .AnyAsync(d => d.Code == code && d.CompanyId == companyid);
        }
    }
}
