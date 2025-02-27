using AB_INBEV.Domain.Models;
using Bogus;

namespace AB_INBEV.Application.UnitTests.Dummies
{
    public class EmployeeDummyFactory : Faker<Employee>
    {
        public EmployeeDummyFactory(int phones = 1)
        {
            RuleFor(e => e.Id, Guid.NewGuid());
            RuleFor(e => e.FirstName, f => f.Person.FirstName);
            RuleFor(e => e.LastName, f => f.Person.LastName);
            RuleFor(e => e.Email, f => f.Person.Email);
            RuleFor(e => e.Document, f => f.Random.Long(100000000000, 999999999999).ToString());
            RuleFor(e => e.BirthDate, f => f.Person.DateOfBirth);
            RuleFor(e => e.Phones, f => new PhoneDummyFactory().Generate(phones));
        }
    }
}
