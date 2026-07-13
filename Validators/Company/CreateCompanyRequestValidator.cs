using BackEnd.DTOs.Requests.Company;
using FluentValidation;

namespace BackEnd.Validators.Company
{
    public class CreateCompanyRequestValidator : AbstractValidator<CreateCompanyRequest>
    {
        public CreateCompanyRequestValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Company code is required.")
                .MaximumLength(50);

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Company name is required.")
                .MaximumLength(255);

            RuleFor(x => x.Description)
                .MaximumLength(500);

            RuleFor(x => x.Address)
                .MaximumLength(255);

            RuleFor(x => x.Phone)
                .MaximumLength(20)
                .Matches(@"^[0-9+\-\s()]*$")
                .When(x => !string.IsNullOrWhiteSpace(x.Phone))
                .WithMessage("Phone number is invalid.");

            RuleFor(x => x.Tax)
                .MaximumLength(20);

            RuleFor(x => x.Status)
                .InclusiveBetween((byte)0, (byte)1);
        }
    }
}
