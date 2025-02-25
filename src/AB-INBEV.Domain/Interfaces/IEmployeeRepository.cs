using AB_INBEV.Domain.Interfaces;
using AB_INBEV.Domain.Models;

namespace AB_INBEV.Domain.Interfaces
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Employee GetByEmail(string email);
    }
}
