using DanskeBank.Domain.CompanyAggregate.Enums;

namespace DanskeBank.Domain.CompanyAggregate.Services;

internal class CreateSwedishNotificationsStrategy : BaseCreateNotificationStrategy
{
    public CreateSwedishNotificationsStrategy() : base(new int[] { 1, 7, 14, 28 }) { }

    public override bool CanHandle(Company schedule) =>
        schedule.Market is Market.Sweden && schedule.CompanyType is CompanyType.Small or CompanyType.Medium;
}
