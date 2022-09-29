using DanskeBank.Domain.CompanyAggregate.Enums;

namespace DanskeBank.Domain.CompanyAggregate.Services;

internal class CreateNorwegianNotificationsStrategy : BaseCreateNotificationStrategy
{
    public CreateNorwegianNotificationsStrategy() : base(new int[] { 1, 5, 10, 20 }) { }

    public override bool CanHandle(Company schedule) =>
        schedule.Market is Market.Norway;
}
