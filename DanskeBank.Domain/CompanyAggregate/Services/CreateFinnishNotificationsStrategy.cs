using DanskeBank.Domain.CompanyAggregate.Enums;

namespace DanskeBank.Domain.CompanyAggregate.Services;

internal class CreateFinnishNotificationsStrategy : BaseCreateNotificationStrategy
{
    public CreateFinnishNotificationsStrategy() : base(new int[] { 1, 5, 10, 15, 20}) { }

    public override bool CanHandle(Company schedule) =>
        schedule.Market is Market.Finland && schedule.CompanyType is CompanyType.Large;
}
