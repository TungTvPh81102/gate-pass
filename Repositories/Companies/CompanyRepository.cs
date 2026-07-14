using BackEnd.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Repositories.Companies
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly ApplicationDbContext _context;

        public CompanyRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Company>> GetAllAsync()
        {
            return await _context.Companies
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Company?> GetByIdAsync(int id)
        {
            return await _context.Companies
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Company> AddAsync(Company company)
        {
            await _context.Companies.AddAsync(company);

            await _context.SaveChangesAsync();

            return company;
        }

        public async Task UpdateAsync(Company company)
        {
            _context.Companies.Update(company);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Company company)
        {
            _context.Companies.Remove(company);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Companies.AnyAsync(x => x.Id == id);
        }

        public async Task<bool> HasDepartmentsAsync(int id)
        {
            return await _context.Departments.AnyAsync(x => x.CompanyId == id);
        }
    }
}
