using AB_INBEV.Application.EventSourcedNormalizers;
using AB_INBEV.Application.ViewModels;

namespace AB_INBEV.Application.Interfaces
{
    public interface IEmployeeAppService
    {
        void Register(EmployeeViewModel employeeViewModel);
        IEnumerable<EmployeeViewModel> GetAll();
        IEnumerable<EmployeeViewModel> GetAll(int skip, int take);
        EmployeeViewModel GetById(Guid id);
        void Update(EmployeeViewModel employeeViewModel);
        void Remove(Guid id);
        IList<EmployeeHistoryData> GetAllHistory(Guid id);
    }
}
