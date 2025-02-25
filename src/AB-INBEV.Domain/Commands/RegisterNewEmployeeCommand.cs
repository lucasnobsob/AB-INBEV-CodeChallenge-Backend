
using AB_INBEV.Domain.Validations;

namespace AB_INBEV.Domain.Commands
{
    public class RegisterNewEmployeeCommand : EmployeeCommand
    {
        public RegisterNewEmployeeCommand(string name, string email, DateTime birthDate)
        {
            FirstName = name;
            Email = email;
            BirthDate = birthDate;
        }

        public override bool IsValid()
        {
            ValidationResult = new RegisterNewEmployeeCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
