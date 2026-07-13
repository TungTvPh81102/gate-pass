using BackEnd.DTOs.Requests.Company;
using BackEnd.Services.Companies;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [Route("api/companies")]
    [ApiController]
    public class CompanyController : BaseController
    {
        private readonly ICompanyService _companyService;

        public CompanyController(
            ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _companyService.GetAllAsync();

            return Success(
                result, result.Any() ? "Get companies successfully" : "No companies found");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _companyService.GetByIdAsync(id);

            if (result == null)
            {
                return NotFound($"Company with id {id} not found");
            }

            return Success(result, "Get company successfully");
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateCompanyRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestWithModelErrors();
            }

            var result = await _companyService.CreateAsync(request);

            return Created(result, "Company created successfully.");
        }

        [HttpPut]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateCompanyRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequestWithModelErrors();

            var result = await _companyService.UpdateAsync(id, request);

            return Success(result, "Company updated successfully");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _companyService.DeleteAsync(id);
            return NoContent("Company deleted successfully");
        }
    }
}