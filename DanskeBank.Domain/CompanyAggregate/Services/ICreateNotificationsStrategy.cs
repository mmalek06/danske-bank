namespace DanskeBank.Domain.CompanyAggregate.Services;

public interface ICreateNotificationsStrategy
{
    public bool CanHandle(Company schedule);

    public IReadOnlyCollection<Notification> Create();
}
