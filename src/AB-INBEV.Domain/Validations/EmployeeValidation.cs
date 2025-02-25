using AB_INBEV.Domain.Commands;
using FluentValidation;

namespace AB_INBEV.Domain.Validations
{
    public abstract class EmployeeValidation<T> : AbstractValidator<T> where T : EmployeeCommand
    {
        protected void ValidateName()
        {
            RuleFor(c => c.FirstName)
                .NotEmpty().WithMessage("Please ensure you have entered the Name")
                .Length(2, 50).WithMessage("The Name must have between 2 and 50 characters");

            RuleFor(c => c.LastName)
                .NotEmpty().WithMessage("Please ensure you have entered the Name")
                .Length(2, 50).WithMessage("The Name must have between 2 and 50 characters");
        }

        protected void ValidateBirthDate()
        {
            RuleFor(c => c.BirthDate)
                .NotEmpty()
                .Must(HaveMinimumAge)
                .WithMessage("The customer must have 18 years or more");
        }

        protected void ValidateEmail()
        {
            RuleFor(c => c.Email)
                .NotEmpty()
                .EmailAddress();
        }

        protected void ValidateId()
        {
            RuleFor(c => c.Id)
                .NotEqual(Guid.Empty);
        }

        protected static bool HaveMinimumAge(DateTime birthDate)
        {
            return birthDate <= DateTime.Now.AddYears(-18);
        }
    }
}
