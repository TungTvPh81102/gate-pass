using BackEnd.DTOs.Requests.User;
using FluentValidation;

namespace BackEnd.Validators.Users;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
     public CreateUserRequestValidator()
     {
         RuleFor(x => x.DepartmentId)
             .NotNull().WithMessage("DepartmentId is required");
             
         RuleFor(x => x.EmployeeId)
             .NotEmpty().WithMessage("EmployeeId is required")
             .MinimumLength(6).WithMessage("EmployeeId must be at least 6 characters long")
             .MaximumLength(200).WithMessage("EmployeeId must be less than 200 characters long");

         RuleFor(x => x.Name)
             .NotEmpty().WithMessage("Name is required")
             .MinimumLength(6).WithMessage("Name must be at least 6 characters long")
             .MaximumLength(200).WithMessage("Name must be less than 200 characters long");
         
         RuleFor(x=> x.Email)
             .NotEmpty().WithMessage("Email is required")
             .EmailAddress().WithMessage("Email must be a valid email address")
             .MaximumLength(20).WithMessage("Email must be less than 200 characters long")
             .Matches(@"^\S*$").WithMessage("Email must not contain any spaces")
             .Must(email => !email.Contains("..")).WithMessage("Email cannot contain consecutive dots");
         
         RuleFor(x=> x.Password)
             .NotEmpty().WithMessage("Password is required")
             .MinimumLength(6).WithMessage("Password must be at least 6 characters long")
             .MaximumLength(100).WithMessage("Password must be less than 100 characters long")
             .Matches(@"^\S*$").WithMessage("Password must not contain any spaces")
             .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
             .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter")
             .Matches("[0-9]").WithMessage("Password must contain at least one number")
             .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character (e.g., !@#$%)")
             ;
         
         RuleFor(x=> x.Status)
             .Must(status => status is 1 or 2 or 3)
             .WithMessage("Status is invalid. Allowed values are 1,2,3");
     }
}