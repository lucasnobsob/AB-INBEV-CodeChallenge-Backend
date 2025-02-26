using AB_INBEV.Domain.Core.Models;

namespace AB_INBEV.Domain.Models
{
    public class Phone : Entity
    {
        public string Number { get; set; }

        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}
