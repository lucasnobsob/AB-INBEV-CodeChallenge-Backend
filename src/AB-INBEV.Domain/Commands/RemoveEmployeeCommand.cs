using AB_INBEV.Domain.Validations;

namespace AB_INBEV.Domain.Commands
{
    public class RemoveEmployeeCommand : EmployeeCommand
    {
        public RemoveEmployeeCommand(Guid id)
        {
            Id = id;
            AggregateId = id;
        }

        public override bool IsValid()
        {
            ValidationResult = new RemoveEmployeeCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
