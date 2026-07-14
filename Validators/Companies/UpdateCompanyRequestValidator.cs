using BackEnd.DTOs.Requests.Company;
using FluentValidation;

namespace BackEnd.Validators.Companies
{
    public class UpdateCompanyRequestValidator : AbstractValidator<UpdateCompanyRequest>
    {
        public UpdateCompanyRequestValidator()
        {
            RuleFor(x => x.Name)
                .MaximumLength(200).WithMessage("Company name cannot exceed 200 characters")
                .When(x => x.Name != null);

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters")
                .When(x => x.Description != null);

            RuleFor(x => x.Address)
                .MaximumLength(500).WithMessage("Address cannot exceed 500 characters")
                .When(x => x.Address != null);

            RuleFor(x => x.Phone)
                .MaximumLength(20).WithMessage("Phone number cannot exceed 20 characters")
                .Matches(@"^[\d\+\-\s\(\)]+$")
                    .WithMessage("Invalid phone number format")
                .When(x => x.Phone != null);

            RuleFor(x => x.Tax)
                .MaximumLength(50).WithMessage("Tax code cannot exceed 50 characters")
                .When(x => x.Tax != null);

            RuleFor(x => x.Status)
                .InclusiveBetween((byte)0, (byte)1)
                .When(x => x.Status != null);

            RuleFor(x => x.UpdatedBy)
                .MaximumLength(200).WithMessage("UpdatedBy cannot exceed 200 characters")
                .When(x => x.UpdatedBy != null);
        }
    }
}
