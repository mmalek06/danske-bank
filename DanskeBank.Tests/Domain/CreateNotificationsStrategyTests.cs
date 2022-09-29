using DanskeBank.Domain.CompanyAggregate;
using DanskeBank.Domain.CompanyAggregate.Enums;
using DanskeBank.Domain.CompanyAggregate.Services;
using System.Reflection;

namespace DanskeBank.Tests.Domain;

public class CreateNotificationsStrategyTests
{
    [Theory, MemberData(nameof(AllImplementationsTestData))]
    public void GivenAllTheImplementations_ThenNoTwoImplementationsShouldBeAbleToProcessTheCompanyAtTheSameTime(
        IEnumerable<ICreateNotificationsStrategy> strategies, 
        Company company)
    {
        var implementationsAbleToHandleCompany = strategies.Count(s => s.CanHandle(company));

        implementationsAbleToHandleCompany.Should().Be(1);
    }

    [Theory, MemberData(nameof(NegativeCanCreateTestData))]
    public void GivenACompany_WhenItsDataDontMatchWithBusinessRule_ThenStrategyIsNotAbleToCreateNotifications(
        ICreateNotificationsStrategy strategy, 
        Market market, 
        CompanyType companyType)
    {
        var company = new Company(
            Guid.Empty, 
            "test", 
            "test", 
            companyType, 
            market);
        var canCreate = strategy.CanHandle(company);

        canCreate.Should().BeFalse();
    }

    [Theory, MemberData(nameof(PositiveCanCreateTestData))]
    public void GivenACompany_WhenItsDataMatchesWithBusinessRule_ThenStrategyIsAbleToCreateNotifications(
        ICreateNotificationsStrategy strategy, 
        Market market, 
        CompanyType companyType)
    {
        var company = new Company(
            Guid.Empty, 
            "test", 
            "test", 
            companyType, 
            market);
        var canCreate = strategy.CanHandle(company);

        canCreate.Should().BeTrue();
    }

    [Theory, MemberData(nameof(CreateTestData))]
    public void GivenAnyStrategy_WhenCreateIsInvoked_ThenNotificationListIsCreated(
        ICreateNotificationsStrategy strategy, 
        IEnumerable<DateTime> dates)
    {
        var notifications = strategy.Create();

        notifications.Should().BeEquivalentTo(dates.Select(dt => new Notification(DateOnly.FromDateTime(dt))));
    }

    public static IEnumerable<object> AllImplementationsTestData =>
        new List<object[]>
        {
            new object[] { CreateAllNotificationStrategies(), CreateCompany(CompanyType.Small, Market.Sweden) },
            new object[] { CreateAllNotificationStrategies(), CreateCompany(CompanyType.Medium, Market.Sweden) },
            new object[] { CreateAllNotificationStrategies(), CreateCompany(CompanyType.Small, Market.Norway) },
            new object[] { CreateAllNotificationStrategies(), CreateCompany(CompanyType.Medium, Market.Norway) },
            new object[] { CreateAllNotificationStrategies(), CreateCompany(CompanyType.Large, Market.Norway) },
            new object[] { CreateAllNotificationStrategies(), CreateCompany(CompanyType.Large, Market.Finland) },
            new object[] { CreateAllNotificationStrategies(), CreateCompany(CompanyType.Small, Market.Denmark) },
            new object[] { CreateAllNotificationStrategies(), CreateCompany(CompanyType.Medium, Market.Denmark) },
            new object[] { CreateAllNotificationStrategies(), CreateCompany(CompanyType.Large, Market.Denmark) }
        };

    public static IEnumerable<object[]> NegativeCanCreateTestData =>
        new List<object[]>
        {
            new object[] { new CreateDanishNotificationsStrategy(), Market.Finland, CompanyType.Small },
            new object[] { new CreateDanishNotificationsStrategy(), Market.Norway, CompanyType.Small },
            new object[] { new CreateDanishNotificationsStrategy(), Market.Sweden, CompanyType.Small },
            new object[] { new CreateFinnishNotificationsStrategy(), Market.Denmark, CompanyType.Small },
            new object[] { new CreateFinnishNotificationsStrategy(), Market.Norway, CompanyType.Small },
            new object[] { new CreateFinnishNotificationsStrategy(), Market.Sweden, CompanyType.Small },
            new object[] { new CreateFinnishNotificationsStrategy(), Market.Finland, CompanyType.Small },
            new object[] { new CreateFinnishNotificationsStrategy(), Market.Finland, CompanyType.Medium },
            new object[] { new CreateNorwegianNotificationsStrategy(), Market.Denmark, CompanyType.Small },
            new object[] { new CreateNorwegianNotificationsStrategy(), Market.Finland, CompanyType.Small },
            new object[] { new CreateNorwegianNotificationsStrategy(), Market.Sweden, CompanyType.Small },
            new object[] { new CreateSwedishNotificationsStrategy(), Market.Denmark, CompanyType.Small },
            new object[] { new CreateSwedishNotificationsStrategy(), Market.Finland, CompanyType.Small },
            new object[] { new CreateSwedishNotificationsStrategy(), Market.Norway, CompanyType.Small },
            new object[] { new CreateSwedishNotificationsStrategy(), Market.Sweden, CompanyType.Large },
        };

    public static IEnumerable<object[]> PositiveCanCreateTestData =>
        new List<object[]>
        {
            new object[] { new CreateDanishNotificationsStrategy(), Market.Denmark, CompanyType.Small },
            new object[] { new CreateFinnishNotificationsStrategy(), Market.Finland, CompanyType.Large },
            new object[] { new CreateNorwegianNotificationsStrategy(), Market.Norway, CompanyType.Small },
            new object[] { new CreateSwedishNotificationsStrategy(), Market.Sweden, CompanyType.Small },
            new object[] { new CreateSwedishNotificationsStrategy(), Market.Sweden, CompanyType.Medium },
        };

    public static IEnumerable<object[]> CreateTestData =>
        new List<object[]>
        {
            new object[] { new CreateDanishNotificationsStrategy(), new[] { 1, 5, 10, 15, 20 }.Select(x => DateTime.UtcNow.Date.AddDays(x)) },
            new object[] { new CreateFinnishNotificationsStrategy(), new[] { 1, 5, 10, 15, 20 }.Select(x => DateTime.UtcNow.Date.AddDays(x)) },
            new object[] { new CreateNorwegianNotificationsStrategy(), new[] { 1, 5, 10, 20 }.Select(x => DateTime.UtcNow.Date.AddDays(x)) },
            new object[] { new CreateSwedishNotificationsStrategy(), new[] { 1, 7, 14, 28 }.Select(x => DateTime.UtcNow.Date.AddDays(x)) }
        };

    private static IEnumerable<object> CreateAllNotificationStrategies() =>
        Assembly
            .GetAssembly(typeof(ICreateNotificationsStrategy))!
            .GetTypes()
            .Where(t => !t.IsAbstract && t.GetInterface(nameof(ICreateNotificationsStrategy)) is not null)
            .Select(t => Activator.CreateInstance(t))!
            .Cast<ICreateNotificationsStrategy>();

    private static Company CreateCompany(CompanyType companyType, Market market) =>
        new(Guid.Empty,
            "test",
            "test",
            companyType,
            market);
}
