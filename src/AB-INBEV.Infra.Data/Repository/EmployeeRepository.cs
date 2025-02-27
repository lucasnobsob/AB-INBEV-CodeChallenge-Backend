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

        public async Task<Employee> GetByEmail(string email)
        {
            return await DbSet.AsNoTracking().FirstOrDefaultAsync(c => c.Email == email);
        }

        public override async Task<IEnumerable<Employee>> GetAll()
        {
            return await DbSet.Include(i => i.Phones).ToListAsync();
        }
    }
}
