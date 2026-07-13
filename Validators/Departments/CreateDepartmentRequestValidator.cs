using BackEnd.DTOs.Requests.Department;
using BackEnd.Models.Entities;
using BackEnd.Repositories.Companies;
using BackEnd.Repositories.Departments;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Validators.Departments
{
    public class CreateDepartmentRequestValidator : AbstractValidator<CreateDepartmentRequest>
    {

        private readonly IDepartmentRepository _departmentRepo;
        private readonly ICompanyRepository _companyRepo;
        public CreateDepartmentRequestValidator(IDepartmentRepository departmentRepo,
            ICompanyRepository companyRepo)
        {
            _departmentRepo = departmentRepo;
            _companyRepo = companyRepo;


            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Department code is required.")
                .MaximumLength(50).WithMessage("Department code cannot exceed 50 characters.")
                .MustAsync(BeUniqueCodeInCompany).WithMessage("Department code already exists in this company.");
            ;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Department name is required.")
                .MaximumLength(200).WithMessage("Department name cannot exceed 200 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(200).WithMessage("Department description cannot exceed 200 characters.");

            RuleFor(x => x.Status)
                .InclusiveBetween((byte)0, (byte)1)
                .WithMessage("Status is invalid. Allowed values are 0 (Inactive) and 1 (Active).");

            RuleFor(x => x.CompanyId)
             .GreaterThan(0)
             .WithMessage("CompanyId must be greater than 0.");

            RuleFor(x => x.ParentId)
                .Must(id => id == null || id > 0)
                .WithMessage("ParentId must be null or greater than 0.");
        }

        private async Task<bool> BeUniqueCodeInCompany(
           CreateDepartmentRequest request,
           string code,
           CancellationToken cancellationToken)
        {
            return !await _departmentRepo.ExistsByCodeAndCompanyAsync(
                code, request.CompanyId, cancellationToken);
        }
    }
}
