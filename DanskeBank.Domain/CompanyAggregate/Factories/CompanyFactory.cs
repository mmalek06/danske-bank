using DanskeBank.Domain.CompanyAggregate.Enums;
using DanskeBank.Domain.CompanyAggregate.Services;

namespace DanskeBank.Domain.CompanyAggregate.Factories;

public class CompanyFactory : ICompanyFactory
{
    private readonly IEnumerable<ICreateNotificationsStrategy> _notificationCreationStrategies;

    /// <summary>
    /// The reason for the existence of this class is as follows:
    /// 
    /// The given business rule is a construction one. It could be achieved in a few ways:
    /// 
    /// 1. By passing the strategies directly into the aggregate constructor - however, that would
    ///    make EF angry - it wouldn't know how to inject those objects. There are some workarounds
    ///    but they look like bad code to me.
    /// 2. By constructing the aggregate and immediatelly after that calling the CreateNotifications
    ///    method. That may be kind of okay-ish in such a small project, but in general should be avoided
    ///    as it leads to a business invariant being enforced in a place other than the domain.
    ///    The app service creator would have the possibility to run only the constructor, without running
    ///    the notification creation mechanisms.
    /// 3. By not bothering with separate strategies to create different schedules and just putting some
    ///    if/switch statements directly into the aggregate. That's the simplest approach and could even be
    ///    future proof if we know that the owner of our newly created company creation engine will
    ///    never expand beyond the markets mentioned in the task. Otherwise adding a new market/company size
    ///    combination handling would be a violation of the Open/Closed principle.
    /// 4. By doing the below, having its own sets of pros and cons:
    ///    Pros
    ///    - business rule is enforced in the domain
    ///    - any database-interfacing framework should never have problem instatiating object with constructors defined
    ///      the way the Company aggregate are defining them
    ///    - easy to add new cases handling
    ///    Cons
    ///    - looks unnecessary and enterprisey at first glance
    ///    - complicates the code a bit
    /// 
    /// Obviously if you didn't even want DDD in this test project, all this may look like
    /// a huge pile of overengineered code. I just assumed that in a bank you have a lot 
    /// of business rules in your domain, therefore you'd rather like to see if a candidate 
    /// is able to do at least a bit of DDD. If not, a lot of the domain code could have been
    /// dropped or moved into the application service.
    /// </summary>
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
