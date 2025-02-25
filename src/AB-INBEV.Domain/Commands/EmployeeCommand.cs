using AB_INBEV.Domain.Core.Commands;

namespace AB_INBEV.Domain.Commands
{
    public abstract class EmployeeCommand : Command
    {
        public Guid Id { get; protected set; }

        public string FirstName { get; protected set; }

        public string LastName { get; protected set; }

        public string Email { get; protected set; }

        public string Password { get; set; }

        public string DocumentNumber { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime BirthDate { get; set; }
    }
}
