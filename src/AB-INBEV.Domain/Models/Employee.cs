using AB_INBEV.Domain.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace AB_INBEV.Domain.Models
{
    public class Employee : Entity
    {
        public Employee(Guid id, string firstName, string lastName, string email, string document, DateTime birthDate, IEnumerable<string> phones)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Document = document;
            BirthDate = birthDate;
            Phones = GetPhones(phones, id);
        }

        public Employee(Guid id, string firstName, string lastName, string email, string document, DateTime birthDate)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Document = document;
            BirthDate = birthDate;
        }

        public Employee(Guid id, string firstName)
        {
            Id = id;
            FirstName = firstName;
        }

        public Employee() { }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Document { get; set; }

        public IEnumerable<Phone> Phones { get; set; }

        public DateTime BirthDate { get; set; }

        private IEnumerable<Phone> GetPhones(IEnumerable<string> phoneList, Guid EmployeeId)
        {
            var phones = new List<Phone>();
            foreach (var phone in phoneList)
                phones.Add(new Phone { EmployeeId = EmployeeId, Number = phone });

            return phones;
        }
    }
}
