using AB_INBEV.Domain.Commands;

namespace AB_INBEV.Domain.Validations
{
    public class UpdateEmployeeCommandValidation : EmployeeValidation<UpdateEmployeeCommand>
    {
        public UpdateEmployeeCommandValidation()
        {
            ValidateId();
            ValidateName();
            ValidateBirthDate();
            ValidateEmail();
        }
    }
}
