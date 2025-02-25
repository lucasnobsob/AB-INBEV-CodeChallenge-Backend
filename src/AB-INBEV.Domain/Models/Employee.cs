using AB_INBEV.Domain.Core.Models;

namespace AB_INBEV.Domain.Models
{
    public class Employee : Entity
    {
        public Employee(Guid id, string firstName, string email, DateTime birthDate)
        {
            Id = id;
            FirstName = firstName;
            Email = email;
            BirthDate = birthDate;
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string DocumentNumber { get; set; }

        public string PhoneNumber { get; set; }

        public string ManagersName { get; set; }

        public string Password { get; set; }

        public DateTime BirthDate { get; set; }
    }
}
