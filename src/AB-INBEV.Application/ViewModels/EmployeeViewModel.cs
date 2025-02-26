using AB_INBEV.Domain.Models;

namespace AB_INBEV.Application.ViewModels
{
    public class EmployeeViewModel
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Document { get; set; }

        public IEnumerable<string> Phones { get; set; }

        public DateTime BirthDate { get; set; }
    }
}
