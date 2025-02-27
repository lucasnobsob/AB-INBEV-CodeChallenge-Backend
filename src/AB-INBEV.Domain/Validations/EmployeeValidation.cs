using AB_INBEV.Domain.Commands;
using FluentValidation;
using System.Text.RegularExpressions;

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
                .WithMessage("The employee must have 18 years or more");
        }

        protected void ValidateDocument()
        {
            RuleFor(c => c.Document)
                .NotEmpty()
                .Must(IsValidDoc);
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

        private bool IsValidDoc(string doc)
        {
            if (doc.Length >= 12)
                return IsValidCPF(doc);

            return IsValidRG(doc);
        }

        private bool IsValidCPF(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf)) return false;

            cpf = Regex.Replace(cpf, "[^0-9]", ""); // Remove caracteres não numéricos

            if (cpf.Length != 11 || new string(cpf[0], 11) == cpf) return false; // Evita CPFs com todos os dígitos iguais

            int[] multiplicador1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf = cpf.Substring(0, 9);
            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            int digito1 = resto < 2 ? 0 : 11 - resto;

            tempCpf += digito1;
            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            int digito2 = resto < 2 ? 0 : 11 - resto;

            return cpf.EndsWith(digito1.ToString() + digito2.ToString());
        }

        private bool IsValidRG(string rg)
        {
            if (string.IsNullOrWhiteSpace(rg)) return false;

            rg = Regex.Replace(rg, "[^0-9]", ""); // Remove caracteres não numéricos

            return rg.Length >= 7 && rg.Length <= 9; // RGs geralmente possuem 7 a 9 dígitos
        }
    }
}
