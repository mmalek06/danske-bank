using DanskeBank.Domain.CompanyAggregate.Enums;
using DanskeBank.Domain.CompanyAggregate.Services;

namespace DanskeBank.Domain.CompanyAggregate.Factories;

public class CompanyFactory : ICompanyFactory
{
    private readonly IEnumerable<ICreateNotificationsStrategy> _notificationCreationStrategies;

    public CompanyFactory(IEnumerable<ICreateNotificationsStrategy> notificationCreationStrategies) =>
        _notificationCreationStrategies = notificationCreationStrategies;

    public Company Create(
        Guid id, 
        string companyName, 
        string companyNumber, 
        CompanyType companyType, 
        Market market)
    {
        var company = new Company(id, companyName, companyNumber, companyType, market);

        company.CreateNotifications(_notificationCreationStrategies);

        return company;
    }
}
