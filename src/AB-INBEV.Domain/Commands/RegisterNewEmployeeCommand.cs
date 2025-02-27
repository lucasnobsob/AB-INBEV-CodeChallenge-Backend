
using AB_INBEV.Domain.Validations;

namespace AB_INBEV.Domain.Commands
{
    public class RegisterNewEmployeeCommand : EmployeeCommand
    {
        public RegisterNewEmployeeCommand(string firstName, string lastName, string email, string document, DateTime birthDate, IEnumerable<string> phones)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Document = document;
            Phones = phones;
            BirthDate = birthDate;
        }

        public RegisterNewEmployeeCommand(Guid id, string firstName)
        {
            Id = id;
            FirstName = firstName;
        }

        public override bool IsValid()
        {
            ValidationResult = new RegisterNewEmployeeCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
