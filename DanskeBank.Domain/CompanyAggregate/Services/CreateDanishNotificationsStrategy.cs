using DanskeBank.Domain.CompanyAggregate.Enums;

namespace DanskeBank.Domain.CompanyAggregate.Services;

public class CreateDanishNotificationsStrategy : BaseCreateNotificationStrategy
{
    public CreateDanishNotificationsStrategy() : base(new[] { 1, 5, 10, 15, 20 }) { }

    public override bool CanHandle(Company schedule) =>
        schedule.Market is Market.Denmark;
}
