using AB_INBEV.Application.EventSourcedNormalizers;
using AB_INBEV.Application.ViewModels;

namespace AB_INBEV.Application.Interfaces
{
    public interface IEmployeeAppService
    {
        Task Register(EmployeeViewModel employeeViewModel);
        Task<IEnumerable<EmployeeViewModel>> GetAll();
        Task<IEnumerable<EmployeeViewModel>> GetAll(int skip, int take);
        Task<EmployeeViewModel> GetById(Guid id);
        Task Update(EmployeeViewModel employeeViewModel);
        Task Remove(Guid id);
        Task<IList<EmployeeHistoryData>> GetAllHistory(Guid id);
    }
}
