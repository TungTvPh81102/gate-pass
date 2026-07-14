using BackEnd.DTOs.Requests.Department;
using BackEnd.Services.Departments;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [Route("api/departments")]
    [ApiController]
    public class DepartmentController : BaseController
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(
            IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _departmentService.GetAllAsync();

            return Success(
                result, result.Any() ? "Get departments successfully" : "No departments found");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _departmentService.GetByIdAsync(id);

            if (result == null)
            {
                return NotFound($"Department with id {id} not found");
            }

            return Success(result, "Get department successfully");
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateDepartmentRequest request)
        {
            var result = await _departmentService.CreateAsync(request);

            return Created(result, "Department created successfully.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateDepartmentRequest request)
        {
            var result = await _departmentService.UpdateAsync(id, request);

            return Success(result, "Department updated successfully.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _departmentService.DeleteAsync(id);

            return NoContent();
        }
    }
}
