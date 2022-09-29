using DanskeBank.Domain.CompanyAggregate;
using DanskeBank.Domain.CompanyAggregate.Enums;
using DanskeBank.Domain.CompanyAggregate.Services;

namespace DanskeBank.Tests.Domain;

public class CompanyAggregateTests
{
    [Fact]
    public void GivenInputData_WhenNoStrategiesArePassed_ThenNoScheduleIsCreated_AndAllOtherDataEqualsThePassedInData()
    {
        var sut = new Company(
            Guid.Empty, 
            "test", 
            "test", 
            CompanyType.Small, 
            Market.Finland);

        sut.CreateNotifications(Array.Empty<ICreateNotificationsStrategy>());

        CheckScalarFields(sut);
        sut.Schedule.Should().BeEmpty();
    }

    [Fact]
    public void GivenInputData_WhenStrategyUnableToHandleConstructedCompanyIsPassed_ThenNoScheduleIsCreated_AndAllOtherDataEqualsThePassedInData()
    {
        var strategyMock = new Mock<ICreateNotificationsStrategy>();

        strategyMock
            .Setup(m => m.CanHandle(It.IsAny<Company>()))
            .Returns(false);

        var sut = new Company(
            Guid.Empty, 
            "test", 
            "test", 
            CompanyType.Small, 
            Market.Finland);

        sut.CreateNotifications(new[] { strategyMock.Object });

        CheckScalarFields(sut);
        sut.Schedule.Should().BeEmpty();
    }

    [Fact]
    public void GivenInputData_WhenStrategyAbleToHandleConstructedCompanyIsPassed_ThenScheduleIsCreated_AndAllOtherDataEqualsThePassedInData()
    {
        var strategyMock = new Mock<ICreateNotificationsStrategy>();
        var notification = new Notification(DateOnly.FromDateTime(DateTime.UtcNow).AddDays(1));

        strategyMock
            .Setup(m => m.CanHandle(It.IsAny<Company>()))
            .Returns(true);
        strategyMock
            .Setup(m => m.Create())
            .Returns(new[] { notification });

        var sut = new Company(
            Guid.Empty, 
            "test", 
            "test", 
            CompanyType.Small, 
            Market.Finland);

        sut.CreateNotifications(new[] { strategyMock.Object });

        CheckScalarFields(sut);
        sut.Schedule.Should().BeEquivalentTo(new[] { notification });
    }

    private void CheckScalarFields(Company company)
    {
        company.CompanyName.Should().Be("test");
        company.CompanyNumber.Should().Be("test");
        company.CompanyType.Should().Be(CompanyType.Small);
        company.Market.Should().Be(Market.Finland);
    }
}
