using AB_INBEV.Domain.Core.Events;

namespace AB_INBEV.Domain.Events
{
    public class EmployeeRemovedEvent : Event
    {
        public EmployeeRemovedEvent(Guid id)
        {
            Id = id;
            AggregateId = id;
        }

        public Guid Id { get; set; }
    }
}
