using AB_INBEV.Domain.Commands;

namespace AB_INBEV.Domain.Validations
{
    public class RemoveEmployeeCommandValidation : EmployeeValidation<RemoveEmployeeCommand>
    {
        public RemoveEmployeeCommandValidation()
        {
            ValidateId();
        }
    }
}
