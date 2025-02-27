using AB_INBEV.Domain.Models;
using Bogus;

namespace AB_INBEV.Application.UnitTests.Dummies
{
    public class PhoneDummyFactory : Faker<Phone>
    {
        public PhoneDummyFactory()
        {
            RuleFor(p => p.Number, f => f.Person.Phone);
        }
    }
}
