using BackEnd.DTOs.Requests.Company;
using FluentValidation;

namespace BackEnd.Validators.Companies
{
    public class CreateCompanyRequestValidator : AbstractValidator<CreateCompanyRequest>
    {
        public CreateCompanyRequestValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Company code is required.")
                .MaximumLength(50).WithMessage("Company code cannot exceed 50 characters");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Company name is required.")
                .MaximumLength(255);

            RuleFor(x => x.Description)
                .MaximumLength(500);

            RuleFor(x => x.Address)
                .MaximumLength(255);

            RuleFor(x => x.Phone)
                .MaximumLength(20).WithMessage("Phone number cannot exceed 20 characters")
                .Matches(@"^[0-9+\-\s()]*$").WithMessage("Invalid phone number format")
                .When(x => !string.IsNullOrWhiteSpace(x.Phone))
                .WithMessage("Phone number is invalid.");

            RuleFor(x => x.Tax)
                .MaximumLength(20).WithMessage("Tax code cannot exceed 50 characters").When(x => !string.IsNullOrEmpty(x.Tax));

            RuleFor(x => x.Status)
                .InclusiveBetween((byte)0, (byte)1);
        }
    }
}