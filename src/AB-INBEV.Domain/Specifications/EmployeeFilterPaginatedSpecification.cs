using AB_INBEV.Domain.Models;

namespace AB_INBEV.Domain.Specifications
{
    public class EmployeeFilterPaginatedSpecification : BaseSpecification<Employee>
    {
        public EmployeeFilterPaginatedSpecification(int skip, int take)
            : base(i => true)
        {
            ApplyPaging(skip, take);
        }
    }
}
