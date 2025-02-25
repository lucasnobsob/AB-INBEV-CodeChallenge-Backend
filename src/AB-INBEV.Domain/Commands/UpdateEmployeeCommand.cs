using AB_INBEV.Domain.Validations;

namespace AB_INBEV.Domain.Commands
{
    public class UpdateEmployeeCommand : EmployeeCommand
    {
        public UpdateEmployeeCommand(Guid id, string name, string email, DateTime birthDate)
        {
            Id = id;
            FirstName = name;
            Email = email;
            BirthDate = birthDate;
        }

        public override bool IsValid()
        {
            ValidationResult = new UpdateEmployeeCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
