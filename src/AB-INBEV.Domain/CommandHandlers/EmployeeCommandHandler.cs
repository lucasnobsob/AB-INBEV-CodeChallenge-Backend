using AB_INBEV.Domain.Commands;
using AB_INBEV.Domain.Core.Bus;
using AB_INBEV.Domain.Core.Notifications;
using AB_INBEV.Domain.Events;
using AB_INBEV.Domain.Interfaces;
using AB_INBEV.Domain.Models;
using AB_INBEV.Domain.Interfaces;
using MediatR;

namespace AB_INBEV.Domain.CommandHandlers
{
    public class EmployeeCommandHandler : CommandHandler,
        IRequestHandler<RegisterNewEmployeeCommand, bool>,
        IRequestHandler<UpdateEmployeeCommand, bool>,
        IRequestHandler<RemoveEmployeeCommand, bool>
    {
        private readonly IEmployeeRepository _customerRepository;
        private readonly IMediatorHandler Bus;

        public EmployeeCommandHandler(IEmployeeRepository customerRepository,
                                      IUnitOfWork uow,
                                      IMediatorHandler bus,
                                      INotificationHandler<DomainNotification> notifications) : base(uow, bus, notifications)
        {
            _customerRepository = customerRepository;
            Bus = bus;
        }

        public Task<bool> Handle(RegisterNewEmployeeCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return Task.FromResult(false);
            }

            var employee = new Employee(Guid.NewGuid(), message.FirstName, message.Email, message.BirthDate);

            if (_customerRepository.GetByEmail(employee.Email) != null)
            {
                Bus.RaiseEvent(new DomainNotification(message.MessageType, "The customer e-mail has already been taken."));
                return Task.FromResult(false);
            }

            _customerRepository.Add(employee);

            if (Commit())
            {
                Bus.RaiseEvent(new EmployeeRegisteredEvent(employee.Id, employee.FirstName, employee.Email, employee.BirthDate));
            }

            return Task.FromResult(true);
        }

        public Task<bool> Handle(UpdateEmployeeCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return Task.FromResult(false);
            }

            var employee = new Employee(message.Id, message.FirstName, message.Email, message.BirthDate);
            var existingCustomer = _customerRepository.GetByEmail(employee.Email);

            if (existingCustomer != null && existingCustomer.Id != employee.Id)
            {
                if (!existingCustomer.Equals(employee))
                {
                    Bus.RaiseEvent(new DomainNotification(message.MessageType, "The customer e-mail has already been taken."));
                    return Task.FromResult(false);
                }
            }

            _customerRepository.Update(employee);

            if (Commit())
            {
                Bus.RaiseEvent(new EmployeeUpdatedEvent(employee.Id, employee.FirstName, employee.Email, employee.BirthDate));
            }

            return Task.FromResult(true);
        }

        public Task<bool> Handle(RemoveEmployeeCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return Task.FromResult(false);
            }

            _customerRepository.Remove(message.Id);

            if (Commit())
            {
                Bus.RaiseEvent(new EmployeeRemovedEvent(message.Id));
            }

            return Task.FromResult(true);
        }

        public void Dispose()
        {
            _customerRepository.Dispose();
        }
    }
}
