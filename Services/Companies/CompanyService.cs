using BackEnd.DTOs.Requests.Company;
using BackEnd.DTOs.Responses;
using BackEnd.Models.Entities;
using BackEnd.Repositories.Companies;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Services.Companies
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _repository;
        private readonly ILogger<CompanyService> _logger;

        public CompanyService(ICompanyRepository repository, ILogger<CompanyService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<CompanyResponse> CreateAsync(CreateCompanyRequest request)
        {
            try
            {
                _logger.LogInformation("Creating company with code: {Code}", request.Code);

                if (string.IsNullOrWhiteSpace(request.Code))
                {
                    _logger.LogWarning("Company code is null or whitespace.");
                    throw new ArgumentException("Company code is required");
                }

                var company = new Company()
                {
                    Code = request.Code,
                    Name = request.Name,
                    Status = request.Status,
                    Description = request.Description,
                    Address = request.Address,
                    Phone = request.Phone,
                    Tax = request.Tax,
                    CreatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                };

                var result = await _repository.AddAsync(company);

                if (result == null)
                {
                    _logger.LogError("Failed to create company.");
                    throw new Exception("Failed to create company.");
                }

                return new CompanyResponse
                {
                    Id = result.Id,
                    Code = result.Code,
                    Name = result.Name,
                    Status = result.Status,
                    Description = result.Description
                };
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Validation failed for company creation");
                throw;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError("Database error when creating company with code: {Code}");
                throw new InvalidOperationException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error when creating company");
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                _logger.LogInformation("Deleting company with id: {Id}", id);

                var company = await _repository.GetByIdAsync(id);
                if (company == null)
                {
                    _logger.LogWarning("Cannot delete - Company not found with id: {Id}", id);
                    throw new KeyNotFoundException($"Company with id {id} not found");
                }

                await _repository.DeleteAsync(company);

                _logger.LogInformation("Company deleted successfully with id: {Id}", id);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Company not found for deletion");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when deleting company with id: {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<CompanyResponse>> GetAllAsync()
        {
            try
            {
                _logger.LogInformation("Getting all companies");

                var companies = await _repository.GetAllAsync();

                _logger.LogInformation("Retrieved {Count} companies", companies.Count());

                return companies.Select(c => new CompanyResponse
                {
                    Id = c.Id,
                    Code = c.Code,
                    Name = c.Name,
                    Status = c.Status,
                    Description = c.Description
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error when creating company");
                throw;
            }
        }

        public async Task<CompanyResponse?> GetByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("Getting company by id: {Id}", id);

                var company = await _repository.GetByIdAsync(id);

                if (company == null)
                {
                    _logger.LogWarning("Company not found with id: {Id}", id);
                    return null;
                }

                return new CompanyResponse
                {
                    Id = company.Id,
                    Code = company.Code,
                    Name = company.Name,
                    Status = company.Status,
                    Description = company.Description,
                    Address = company.Address,
                    CreatedAt = company.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when getting company by id: {Id}", id);
                throw;
            }
        }

        public async Task<CompanyResponse> UpdateAsync(int id, UpdateCompanyRequest request)
        {
            try
            {
                _logger.LogInformation("Updating company with id: {Id}", id);

                var company = await _repository.GetByIdAsync(id);

                if (company == null)
                {
                    _logger.LogWarning("Cannot update - Company not found with id: {Id}", id);
                    throw new KeyNotFoundException($"Company with id {id} not found");
                }

                company.Name = request.Name ?? company.Name;
                company.Description = request.Description ?? company.Description;
                company.Address = request.Address ?? company.Address;
                company.Phone = request.Phone ?? company.Phone;
                company.Tax = request.Tax ?? company.Tax;
                company.Status = request.Status ?? company.Status;
                company.UpdatedBy = request.UpdatedBy ?? company.UpdatedBy;
                company.UpdatedAt = DateTime.UtcNow;

                await _repository.UpdateAsync(company);

                _logger.LogInformation("Company updated successfully with id: {Id}", id);

                return MapToResponse(company);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Company not found for update");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when updating company with id: {Id}", id);
                throw;
            }
        }

        private CompanyResponse MapToResponse(Models.Entities.Company company)
        {
            return new CompanyResponse
            {
                Id = company.Id,
                Code = company.Code,
                Name = company.Name,
                Status = company.Status,
                Description = company.Description,
                Address = company.Address,
                Phone = company.Phone,
                Tax = company.Tax,
                CreatedAt = company.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")
            };
        }
    }
}