using AB_INBEV.Domain.Models;
using AB_INBEV.Infra.Data.Context;
using AB_INBEV.Infra.Data.Repository;
using AB_INBEV.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AB_INBEV.Infra.Data.Repository
{
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(ApplicationDbContext context)
            : base(context)
        {

        }

        public Employee GetByEmail(string email)
        {
            return DbSet.AsNoTracking().FirstOrDefault(c => c.Email == email);
        }
    }
}
