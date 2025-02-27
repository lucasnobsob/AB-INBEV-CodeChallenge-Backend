using AB_INBEV.Domain.Validations;

namespace AB_INBEV.Domain.Commands
{
    public class UpdateEmployeeCommand : EmployeeCommand
    {
        public UpdateEmployeeCommand(Guid id, string firstName, string lastName, string email, string document, DateTime birthDate, IEnumerable<string> phones)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Document = document;
            Phones = phones;
            BirthDate = birthDate;
        }

        public UpdateEmployeeCommand(Guid id, string firstName)
        {
            Id = id;
            FirstName = firstName;
        }

        public override bool IsValid()
        {
            ValidationResult = new UpdateEmployeeCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
