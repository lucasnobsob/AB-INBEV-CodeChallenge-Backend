using AB_INBEV.Domain.Commands;

namespace AB_INBEV.Domain.Validations
{
    public class RegisterNewEmployeeCommandValidation : EmployeeValidation<RegisterNewEmployeeCommand>
    {
        public RegisterNewEmployeeCommandValidation()
        {
            ValidateName();
            ValidateBirthDate();
            ValidateEmail();
        }
    }
}
