using AB_INBEV.Domain.Events;
using MediatR;

namespace AB_INBEV.Domain.EventHandlers
{
    public class EmployeeEventHandler :
        INotificationHandler<EmployeeRegisteredEvent>,
        INotificationHandler<EmployeeUpdatedEvent>,
        INotificationHandler<EmployeeRemovedEvent>
    {
        public Task Handle(EmployeeUpdatedEvent message, CancellationToken cancellationToken)
        {
            // Send some notification e-mail

            return Task.CompletedTask;
        }

        public Task Handle(EmployeeRegisteredEvent message, CancellationToken cancellationToken)
        {
            // Send some greetings e-mail

            return Task.CompletedTask;
        }

        public Task Handle(EmployeeRemovedEvent message, CancellationToken cancellationToken)
        {
            // Send some see you soon e-mail

            return Task.CompletedTask;
        }
    }
}
