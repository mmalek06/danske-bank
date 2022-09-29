namespace DanskeBank.Domain.CompanyAggregate.Services;

public abstract class BaseCreateNotificationStrategy : ICreateNotificationsStrategy
{
    private readonly int[] _schedule;

    /// <summary>
    /// 
    /// Extending classes are expected to pass the schedule directly - for now, the values are hardcoded in their constructors.
    /// However, changing schedule for a given country would require us to recompile and redeploy the app.
    /// Maybe a better approach would be to put those values into the appsettings file - that way we can approach this problem in 2 ways:
    /// 
    /// 1. Using IOptionsSnapshot - the options are recomputed on every request, so after the appsettings have been changed, that change is
    ///    immediatelly visible in the app, but in turn requires services using it to not be registered as singletons.
    /// 2. Using IOptions - this approach is not as dynamic, but we could at least save some time on deployment, as the changes would be visible
    ///    after app restart.
    ///    
    /// For simplicity sake I went with the easiest to code solution.
    /// 
    /// </summary>
    protected BaseCreateNotificationStrategy(int[] schedule) =>
        _schedule = schedule;

    public abstract bool CanHandle(Company schedule);

    public IReadOnlyCollection<Notification> Create()
    {
        var now = DateTime.UtcNow;

        return _schedule
            .Select(dayNum =>
            {
                var nextDate = DateOnly
                    .FromDateTime(now)
                    .AddDays(dayNum);

                return new Notification(nextDate);
            })
            .ToArray();
    }
}
