using DanskeBank.Domain.CompanyAggregate.Enums;
using DanskeBank.Domain.CompanyAggregate.Services;
using DanskeBank.Domain.SeedWork;

namespace DanskeBank.Domain.CompanyAggregate;

public class Company : Entity, IAggregateRoot
{
    public string CompanyName { get; private set; }

    public string CompanyNumber { get; private set; }

    public CompanyType CompanyType { get; private set; }

    public Market Market { get; private set; }

    public IReadOnlyCollection<Notification> Schedule { get; private set; } = new List<Notification>();

    internal Company(
        Guid id,
        string companyName,
        string companyNumber,
        CompanyType companyType,
        Market market)
    {
        Id = id;
        CompanyName = companyName;
        CompanyNumber = companyNumber;
        CompanyType = companyType;
        Market = market;
    }

    internal void CreateNotifications(IEnumerable<ICreateNotificationsStrategy> notificationCreationStrategies)
    {
        var strategy = notificationCreationStrategies.SingleOrDefault(s => s.CanHandle(this));

        if (strategy is null)
            return;

        Schedule = strategy.Create();
    }
}
