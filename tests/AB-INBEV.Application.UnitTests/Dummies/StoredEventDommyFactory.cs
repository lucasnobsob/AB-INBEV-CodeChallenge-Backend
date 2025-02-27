using AB_INBEV.Domain.Core.Events;
using Bogus;

namespace AB_INBEV.Application.UnitTests.Dummies
{
    public class StoredEventDommyFactory : Faker<StoredEvent>
    {
        public StoredEventDommyFactory()
        {
            RuleFor(e => e.Id, Guid.NewGuid());
            RuleFor(e => e.AggregateId, Guid.NewGuid());
            RuleFor(e => e.MessageType, f => f.PickRandom(new List<string> { "error", "success", "warning" }));
            RuleFor(e => e.Data, f => f.Lorem.Text());
            RuleFor(e => e.User, f => f.Person.FullName);
        }
    }
}
